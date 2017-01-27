app.controller("HospitalController", function ($scope, HospitalService,HospitalTypeService, StateService, CityService, $window, MsgService) {

    //angular validation
    $scope.phoneNumber = /^\d{3}\d{3}\d{4}/;
    $scope.validateEmail = /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/
    $scope.HospitalNameValidate = /^[A-Za-z\s]+$/;
    $scope.personName = /^[a-zA-Z]*$/;
    $scope.userName = /^\S{3,}$/;
    $scope.dateOptions = {
        'starting-day': 1
    };
    $scope.url = /(http(s)?:\\)?([\w-]+\.)+[\w-]+[.com|.in|.org]+(\[\?%&=]*)?/;

    StateService.GetStates()
       .success(function (data, status, headers, config) {
           $scope.States = data.States;
       })
       .error(function (data, status, headers, config) {
           var Message = MsgService.makeMessage(data.ReturnMessage)
           message('error', 'Error!', Message);
       });

    HospitalTypeService.GetHospitalTypes()
       .success(function (data, status, headers, config) {
           $scope.HospitalTypes = data.HospitalTypes;
       })
       .error(function (data, status, headers, config) {
           var Message = MsgService.makeMessage(data.ReturnMessage)
           message('error', 'Error!', Message);
       });

    $scope.GetCities = function (stateId) {
        GetCities(stateId);
    };

    $scope.AddHospital = function () {
        var hospital_DTO = {
            HospitalName: $scope.HospitalName,
            UserName: $scope.UserName,
            HospitalTypeID: $scope.HospitalType.HospitalTypeID,
            BusinessURL: $scope.BusinessURL,
            RegistrationNumber: $scope.RegistrationNumber,
            ContactPersonName: $scope.ContactPersonName,
            ContactPersonNumber: $scope.ContactPersonNumber,
            HospitalEmailId: $scope.HospitalEmailId,
            HospitalAddress: $scope.HospitalAddress,
            CityId: $scope.city.CityID,
        };

        console.log(hospital_DTO);

        var response = HospitalService.AddHospital(hospital_DTO);
        response.success(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('success', 'Success!', Message);

            ClearFields();
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });
    };

    GetCities = function (stateId) {
        //$scope.GetCities = function (stateId) {
        if ($scope.state != null) {
            if (stateId) {
                CityService.GetCities(stateId)
                    .success(function (data, status, headers, config) {
                        if (data.Cities == "") {
                            var Message = MsgService.makeMessage(data.ReturnMessage)
                            message('error', 'Error!', Message);
                            //message('info', 'Not available!', 'Cities are not available for state : <b>' + state.StateName + '</b>');
                            $scope.Cities = null;
                        }
                        else {
                            $scope.Cities = data.Cities;
                        }
                    }).error(function (data, status, headers, config) {
                        var Message = MsgService.makeMessage(data.ReturnMessage)
                        message('error', 'Error!', Message);
                        $scope.Cities = null;
                    });
            }
            else {
                $scope.Cities = null;
            }
        }
        else {
            $scope.Cities = null;
        }
    }

    function ClearFields() {
        $scope.HospitalName = "";
        $scope.ContactPersonName = "";
        $scope.ContactPersonNumber = "";
        $scope.HospitalEmailId = "";
        $scope.BusinessURL = "";
        $scope.HospitalAddress = "";
        $scope.CityId = "";
        $scope.HospitalType = { HospitalTypeID: 0 },
        $scope.RegistrationNumber = "",
        $scope.city = null;
        $scope.state = { StateId: 0 };
    }

    function message(type, title, content) {
        var notify = {
            type: type,
            title: title,
            content: content
        };
        $scope.$emit('notify', notify);
    }
});

app.filter('updateById', function () {
    return function (input, id, data) {
        var i = 0, len = input.length;
        for (; i < len; i++) {
            if (+input[i].HospitalProductId == +id) {
                input[i].ProductDescription = data;
                return input;
            }
        }
        return null;
    }
});