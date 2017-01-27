(function () {
    'use strict';

    angular
        .module('app.photo')
        .factory('photoManager', photoManager);

    photoManager.$inject = ['$q', 'photoManagerClient', 'appInfo'];

    function photoManager($q, photoManagerClient, appInfo) {
        var service = {
            photos: [],
            load: load,
            upload: upload,
            remove: remove,
            photoExists: photoExists,
            status: {
                uploading: false
            }
        };

        return service;

        function load() {
            appInfo.setInfo({ busy: true, message: "loading files" })

            service.photos.length = 0;

            return photoManagerClient.query()
                                .$promise
                                .then(function (result) {
                                    result.photos
                                            .forEach(function (photo) {
                                                service.photos.push(photo);
                                            });

                                    appInfo.setInfo({ message: "photos loaded successfully" });

                                    return result.$promise;
                                },
                                function (result) {
                                    appInfo.setInfo({ message: "something went wrong: " + result.data.message });
                                    return $q.reject(result);
                                })
                                ['finally'](
                                function () {
                                    appInfo.setInfo({ busy: false });
                                });
        }

        function upload(photos) {
            service.status.uploading = true;
            appInfo.setInfo({ busy: true, message: "uploading files" });

            var formData = new FormData();


            angular.forEach(photos, function (photo) {
                //UploadedAttachmentName = Math.round(new Date().getTime() / 1000) + '_' + photo.name.substring(0, 10);
                UploadedAttachmentName = photo.name;
                formData.append(UploadedAttachmentName, photo);
                //since it is one now, so taking one
            });

            return photoManagerClient.save(formData)
                                        .$promise
                                        .then(function (result) {
                                            if (result && result.photos) {
                                                result.photos.forEach(function (photo) {
                                                    if (!photoExists(photo.name)) {
                                                        service.photos.push(photo);
                                                    }
                                                });
                                            }

                                            appInfo.setInfo({ message: "file uploaded successfully" });

                                            return result.$promise;
                                        },
                                        function (result) {
                                            appInfo.setInfo({ message: "something went wrong: " + result.data.message });
                                            return $q.reject(result);
                                        })
                                        ['finally'](
                                        function () {
                                            appInfo.setInfo({ busy: false });
                                            service.status.uploading = false;
                                        });
        }

        function remove(photo) {
            appInfo.setInfo({ busy: true, message: "deleting file " + photo.name });

            return photoManagerClient.remove({ fileName: photo.name })
                                        .$promise
                                        .then(function (result) {
                                            //if the photo was deleted successfully remove it from the photos array
                                            var i = service.photos.indexOf(photo);
                                            service.photos.splice(i, 1);

                                            appInfo.setInfo({ message: "files deleted" });

                                            return result.$promise;
                                        },
                                        function (result) {
                                            appInfo.setInfo({ message: "something went wrong: " + result.data.message });
                                            return $q.reject(result);
                                        })
                                        ['finally'](
                                        function () {
                                            appInfo.setInfo({ busy: false });
                                        });
        }

        function photoExists(photoName) {
            var res = false
            service.photos.forEach(function (photo) {
                if (photo.name === photoName) {
                    res = true;
                }
            });

            return res;
        }
    }
})();