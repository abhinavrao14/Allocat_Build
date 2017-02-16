app.controller("ProductController", function ($scope, ProductService, $window, MsgService, ResourceService, $timeout, $q, $log, $http) {

    var TissueBankId = document.getElementById("TissueBankId").value;

    if (TissueBankId != "") {
        $scope.TissueBankId = TissueBankId;
    }

    var InfoType = document.getElementById("InfoType").value;
    if (InfoType != "") {
        $scope.InfoType = InfoType;
    }

    $scope.webApiRootPath = ResourceService.webApiRootPath;
    $scope.SortDirection = "ASC";
    $scope.SortExpression = "ProductMasterName";
    $scope.CurrentPage = 1;
    $scope.SearchBy = "";
    $scope.descriptionLimit = 15;
    $scope.dataLoading = true;
    $scope.TotalProducts = 0;
    $scope.PageSizes = [10, 20, 50, 100];
    $scope.PageSize = $scope.PageSizes[0];
    $scope.TotalPage = 0;

    //$scope.SortOptions = ['ProductMasterName-A To Z', 'ProductMasterName-Z To A', 'ProductType-A To Z',
    //'ProductType-Z To A', 'ProductCode-A To Z', 'ProductCode-Z To A', 'PreservationType-A To Z',
    //'PreservationType-Z To A', 'UnitPrice-High To Low', 'UnitPrice-Low To High', ];

    //$scope.SortOption = $scope.SortOptions[0];

    $scope.editMode = false;

    //new
    $scope.simulateQuery = true;
    $scope.AllTbProductMasters = loadAllProducts($http);
    $scope.querySearch = querySearch;
    $scope.selectedItemChange = selectedItemChange;
    $scope.searchTextChange = searchTextChange;
    function querySearch(query) {
        var results = query ? $scope.AllTbProductMasters.filter(createFilterFor(query)) : $scope.AllTbProductMasters, deferred;
        if ($scope.simulateQuery) {
            deferred = $q.defer();
            $timeout(function () { deferred.resolve(results); }, Math.random() * 1000, false);
            return deferred.promise;
        } else {
            return results;
        }
    }

    function loadAllProducts() {
        var productList_TissueBank_DTO = new Object();

        productList_TissueBank_DTO.TissueBankId = TissueBankId;
        productList_TissueBank_DTO.CurrentPage = 1;
        productList_TissueBank_DTO.SortExpression = 'ProductMasterName';
        productList_TissueBank_DTO.SortDirection = 'ASC';
        productList_TissueBank_DTO.PageSize = 100000;
        productList_TissueBank_DTO.SearchBy = '';
        productList_TissueBank_DTO.OperationType = 'GetAll';

        var allProducts = [];
        var url = '';
        var result = [];
        url = ResourceService.webApiRootPath + "ProductApi";
        $http({
            method: 'GET',
            url: url,
            params: productList_TissueBank_DTO
        }).then(function successCallback(response) {
            console.log(response.data.TbProductMasters);
            allProducts = response.data.TbProductMasters;
            angular.forEach(allProducts, function (product, key) {
                result.push(
                    {
                        value: product.ProductMasterName.toLowerCase(),
                        display: product.ProductMasterName
                    });
            });
        }, function errorCallback(response) {
            console.log('Oops! Something went wrong while fetching the data. Status Code: ' + response.status + ' Status statusText: ' + response.statusText);
        });
        return result;
    }

    function createFilterFor(query) {
        //var lowercaseQuery = angular.lowercase(query);
        return function filterFn(product) {
            return (product.value.indexOf(query) === 0);
        };

    }

    function searchTextChange(text) {
        $log.info('Text changed to ' + text);
    }

    function selectedItemChange(item) {
        $log.info('Item changed to ' + JSON.stringify(item));
        if (item != null) {
            $scope.SearchBy = item.value;
        }
        else {
            $scope.SearchBy = '';

        }
        getTbProductMasters($scope.SearchBy, 1, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
        $scope.CurrentPage = 1;
        $scope.PageSize = $scope.PageSizes[0];
    }
    //new end


    function getTbProductMasters(SearchBy, CurrentPage, PageSize, SortExpression, SortDirection) {
        var productList_TissueBank_DTO = new Object();

        productList_TissueBank_DTO.TissueBankId = $scope.TissueBankId;
        productList_TissueBank_DTO.CurrentPage = CurrentPage;
        productList_TissueBank_DTO.SortExpression = SortExpression;
        productList_TissueBank_DTO.SortDirection = SortDirection;
        productList_TissueBank_DTO.PageSize = PageSize;
        productList_TissueBank_DTO.SearchBy = SearchBy;
        productList_TissueBank_DTO.OperationType = 'GetAll';

        var response = ProductService.getTbProductMasters(productList_TissueBank_DTO);

        response
        .success(function (data, status, headers, config) {
            var str = data.ReturnMessage[0];
            var arr = str.split(" ");
            $scope.TotalTbProductMasters = arr[0];

            $scope.TbProductMasters = data.TbProductMasters;

            $scope.TotalPage = Math.ceil($scope.TotalTbProductMasters / $scope.PageSize);
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        }).finally(function () {
            $scope.dataLoading = false;
        });
    }


    $scope.GetTbProduct = function (TissueBankProductMasterId) {

        if (TissueBankProductMasterId != "" && TissueBankId != "" && InfoType != "") {

            var productList_TissueBank_DTO = new Object();
            productList_TissueBank_DTO.TissueBankProductMasterId = TissueBankProductMasterId;
            productList_TissueBank_DTO.TissueBankId = TissueBankId;
            productList_TissueBank_DTO.InfoType = InfoType;
            productList_TissueBank_DTO.OperationType = 'GetById';

            var response = ProductService.GetTbProduct(productList_TissueBank_DTO);

            response
            .success(function (data, status, headers, config) {
                var str = data.ReturnMessage[0];
                var arr = str.split(" ");

                $scope.TotalTbProducts = arr[0];
                console.log(data.TbProducts);
                $scope.TbProducts = data.TbProducts;
            }).error(function (data, status, headers, config) {
                var Message = MsgService.makeMessage(data.ReturnMessage)
                message('error', 'Error!', Message);
            }).finally(function () {
                $scope.dataLoading = false;
            });
        }
    };


    getTbProductMasters($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);

    $scope.getPreviousPage = function () {
        $scope.CurrentPage = $scope.CurrentPage - 1;
        getTbProductMasters($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.getNextPage = function () {
        $scope.CurrentPage = $scope.CurrentPage + 1;
        getTbProductMasters($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.getLastPage = function () {
        $scope.CurrentPage = $scope.TotalPage;
        getTbProductMasters($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.getFirstPage = function () {
        $scope.CurrentPage = 1;
        getTbProductMasters($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.search = function (SearchBy) {
        $scope.SearchBy = SearchBy;
        getTbProductMasters($scope.SearchBy, 1, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
        $scope.CurrentPage = 1;
        $scope.PageSize = $scope.PageSizes[0];
    };

    $scope.SortOptionChanged = function () {
        var str = $scope.SortOption;
        var arr = str.split("-");

        $scope.CurrentPage = 1;
        $scope.SortExpression = arr[0];

        if (arr[1] == 'Z To A') {
            $scope.SortDirection = "DESC"
        }
        else if (arr[1] == 'A To Z') {
            $scope.SortDirection = "ASC"
        }
        else if (arr[1] == 'Low To High') {
            $scope.SortDirection = "ASC"
        }
        else if (arr[1] == 'High To Low') {
            $scope.SortDirection = "DESC"
        }

        getTbProductMasters($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.PageSizeChanged = function () {
        $scope.CurrentPage = 1;
        getTbProductMasters($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };



    function message(type, title, content) {
        var notify = {
            type: type,
            title: title,
            content: content
        };
        $scope.$emit('notify', notify);
    }
});

app.controller("ProductDetailController", function ($filter, $scope, ProductDetailService, $window, ProductMasterService, MsgService, ResourceService) {
    $scope.editMode = false;
    $scope.addNewMode = false;
    $scope.IsAvailableOptions = ['Yes', 'No'];
    $scope.ProductCodeExp = /^\s*\w*\s*$/;
    $scope.dataLoading = true;
    $scope.webApiRootPath = ResourceService.webApiRootPath;

    $scope.TissueBankId = document.getElementById("TissueBankId").value;
    $scope.CreatedBy = document.getElementById("UserId").value;
    $scope.LastModifiedBy = document.getElementById("UserId").value;
    $scope.TbProducts = [];

    var InfoType = document.getElementById("InfoType").value;
    if (InfoType != "") {
        $scope.InfoType = InfoType;
    }

    var TissueBankId = document.getElementById("TissueBankId").value;
    if (TissueBankId != "") {
        $scope.TissueBankId = TissueBankId;
    }

    var TissueBankProductMasterId = document.getElementById("TissueBankProductMasterId").value;
    if (TissueBankProductMasterId != "" && TissueBankId != "" && InfoType != "") {
        $scope.TissueBankProductMasterId = TissueBankProductMasterId;

        var productList_TissueBank_DTO = new Object();
        productList_TissueBank_DTO.TissueBankProductMasterId = TissueBankProductMasterId;
        productList_TissueBank_DTO.TissueBankId = TissueBankId;
        productList_TissueBank_DTO.InfoType = InfoType;
        productList_TissueBank_DTO.OperationType = 'GetById';

        GetTissueBankProductsByProductMasterId(productList_TissueBank_DTO);
    }

    var ProductMasterGetByIdDTO = {
        Id: TissueBankProductMasterId,
        OperationType: "GetByProductMasterName"
    };

    ProductDetailService.GetPreservationTypes()
        .success(function (data, status, headers, config) {
            $scope.PreservationTypeOptions = data.PreservationTypes;
        })
        .error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });

    ProductDetailService.GetProductTypes(TissueBankProductMasterId)
        .success(function (data, status, headers, config) {
            $scope.ProductTypeOptions = data.ProductTypes;
        })
        .error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });

    ProductDetailService.GetProductSizes(TissueBankProductMasterId)
        .success(function (data, status, headers, config) {
            $scope.ProductSizeOptions = data.ProductSizes;
        })
    .error(function (data, status, headers, config) {
        var Message = MsgService.makeMessage(data.ReturnMessage)
        message('error', 'Error!', Message);
    });

    ProductDetailService.GetSources()
        .success(function (data, status, headers, config) {
            $scope.SourceOptions = data.Sources;
        })
        .error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });


    function GetTissueBankProductsByProductMasterId(productList_TissueBank_DTO) {

        var response = ProductDetailService.GetTissueBankProductsByProductMasterId(productList_TissueBank_DTO);

        response.success(function (data, status, headers, config) {
            $scope.TbProducts = data.TbProducts;
            console.log(data.TbProducts);

            ProductMasterService.Get(ProductMasterGetByIdDTO)
                   .success(function (data2, status, headers, config) {
                       $scope.ProductMaster_TissueBank = data2.ProductMaster_TissueBank;
                   })
                   .error(function (data2, status, headers, config) {
                       var Message = MsgService.makeMessage(data2.ReturnMessage)
                       message('error', 'Error!', Message);
                   });


        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        }).finally(function () {
            $scope.dataLoading = false;
        });
    }

    $scope.save = function () {
        var errStr = '';
        var Products = $scope.TbProducts;



        //  if ($scope.form_ConfirmOnExit.$dirty == true && $scope.form_ConfirmOnExit.$pristine == false) {
        //-------change LastModifiedBy of every row
        for (var i = 0; i < Products.length; i++) {

            Products[i].LastModifiedBy = $scope.LastModifiedBy;
            if (countInArray(Products, Products[i].ProductCode) > 1) {
                errStr = errStr + 'Product Code :' + Products[i].ProductCode + ' exists twice <br />';
            }

            //------start------date validation
            //if (Products[i].AvailabilityStartDate != null) {
            //    if (Products[i].AvailabilityStartDate.match(/^(\d{4})\-(\d{2})\-(\d{2})T(\d{2}):(\d{2}):(\d{2})$/) == null && (Products[i].AvailabilityStartDate.match(/^(\d{4})\-(\d{2})\-(\d{2}) (\d{2}):(\d{2}):(\d{2})$/)) == null) {
            //        if (errStr == '') {
            //            errStr = 'Row ' + i + 1 + ' is having invalid start date';
            //        }
            //        else {
            //            errStr = errStr + '<br/>Row ' + i + 1 + ' is having invalid start date';
            //        }
            //    }
            //}
            //if (Products[i].AvailabilityEndDate != null) {
            //    if (Products[i].AvailabilityEndDate.match(/^(\d{4})\-(\d{2})\-(\d{2})T(\d{2}):(\d{2}):(\d{2})$/) == null && (Products[i].AvailabilityEndDate.match(/^(\d{4})\-(\d{2})\-(\d{2}) (\d{2}):(\d{2}):(\d{2})$/)) == null) {
            //        if (errStr == '') {
            //            errStr = 'Row ' + i + 1 + ' is having invalid end date';
            //        }
            //        else {
            //            errStr = errStr + '<br/>Row ' + i + 1 + ' is having invalid end date';
            //        }
            //    }
            //}
            //------end------date validation
        }

        function countInArray(Products, ProductCode) {
            var count = 0;
            for (var i = 0; i < Products.length; i++) {
                if (Products[i].ProductCode === ProductCode) {
                    count++;
                }
            }
            return count;
        }

        if (errStr == '') {
            console.log(Products);
            var response = ProductDetailService.saveTissueBankProducts(Products);

            response
           .success(function (data, status, headers, config) {
               var Message = MsgService.makeMessage(data.ReturnMessage)
               message('success', 'Success!', Message);

               $scope.editMode = false;
               $scope.form_ConfirmOnExit.$dirty = false;

               //-------get data again from database
               var productList_TissueBank_DTO = new Object();
               productList_TissueBank_DTO.TissueBankProductMasterId = TissueBankProductMasterId;
               productList_TissueBank_DTO.TissueBankId = TissueBankId;
               productList_TissueBank_DTO.InfoType = InfoType;
               productList_TissueBank_DTO.OperationType = 'GetById';

               GetTissueBankProductsByProductMasterId(productList_TissueBank_DTO);
           })
           .error(function (data, status, headers, config) {
               var Message = MsgService.makeMessage(data.ReturnMessage)
               message('error', 'Error!', Message);
           });
        }
        else {
            message('error', 'Error!', errStr);
        }
        //    }
        //   else {
        //$scope.editMode = false;
        //  //$scope.form_ConfirmOnExit.$dirty = false;
        //message('warning', 'Success!', 'No change made in products.');
        //-------get data again from database
        //GetTissueBankProductsByProductMasterId($scope.TissueBankId, ProductMasterId);
        //console.log("not dirty");
        //  }
    }

    $scope.addNew = function () {
        $scope.addNewMode = true;
        $scope.form_ConfirmOnExit.$dirty = true;
        $scope.TbProducts.push({
            'TissueBankProductId': "0",
            'TissueBankId': $scope.TissueBankId,
            'TissueBankProductMasterId': TissueBankProductMasterId,
            'ProductType': "",
            'ProductCode': "",
            'ProductSize': "",
            'PreservationType': "",
            'ProductDescription': "",
            'UnitPrice': "",
            'SourceId': "",
            'IsAvailableForSale': "",
            'AvailabilityStartDate': "",
            'AvailabilityEndDate': "",
            'CreatedBy': $scope.CreatedBy,
            'LastModifiedBy': $scope.LastModifiedBy,
        });
    };

    $scope.cancel = function () {
        if ($scope.form_ConfirmOnExit.$dirty == true) {
            if ($window.confirm("You have some unsaved changes.Do you really want to cancel?")) {
                $scope.form_ConfirmOnExit.$dirty = false;
                $scope.editMode = false;

                //get data again from db
                GetTissueBankProductsByProductMasterId(TissueBankProductMasterId);
            }
        }
        else {
            $scope.editMode = false;
        }
    };

    $scope.CheckTypeCombination = function (ProductType, ProductSize, index) {
        for (var i = 0; i < $scope.TbProducts.length; ++i) {
            if ($scope.TbProducts[i].ProductType == ProductType && index != i) {
                if ($scope.TbProducts[i].ProductSize == ProductSize) {
                    $scope.TbProducts[index].ProductType = null;
                    message('error', 'Error!', ' Type:<b>' + ProductType + '</b> is already present for Size:<b>' + ProductSize + '</b>!');
                    break;
                }
            }
        }
    };

    $scope.CheckSizeCombination = function (ProductType, ProductSize, index) {
        for (var i = 0; i < $scope.TbProducts.length; ++i) {
            if ($scope.TbProducts[i].ProductSize == ProductSize && index != i) {
                if ($scope.TbProducts[i].ProductType == ProductType) {
                    $scope.TbProducts[index].ProductSize = null;
                    message('error', 'Error!', 'Size:<b>' + ProductSize + '</b> is already present for Type:<b>' + ProductType + '</b>!');
                    break;
                }
            }
        }
    };

    function message(type, title, content) {
        var notify = {
            type: type,
            title: title,
            content: content
        };
        $scope.$emit('notify', notify);
    }

    //for opening and closing pop-up description
    //$scope.selectedTissueBankProductId = 0;
    //$scope.selectedDescription = "";
    //$scope.selectedIndex = 0;
    //$scope.ProductTypeOptions = ['AdMatrix1', 'AdMatrix2', 'Admatrix3'];
    //$scope.ProductSizeOptions = ['8 mm', '10 mm', '12 mm', '14 mm', 'L= 4cm W= 4cm T= 0.1mm', 'L= 4cm W= 6cm T= 0.1mm'];
    //$scope.SourceOptions = [{ 'SourceId': 1, 'SourceName': 'Synthetic' }, { 'SourceId': 2, 'SourceName': 'Allograft' }];
    //$scope.PreservationTypeOptions = ['Sterile', 'Saline', 'Preservon', 'Lyophilized',
    //                                'Irradiated', 'Frozen', 'Freeze-Dried', 'Desiccated'];
    //-------------save new functionality start
    //$scope.saveNew = function () {
    //    $scope.addNewMode = false;
    //    //$scope.TbProducts.splice(length - 1, 1);
    //    var products = $filter('filter')($scope.TbProducts, 'new');
    //    console.log(products);
    //    //console.log($scope.TbProducts);
    //    // var response = ProductDetailService.saveTissueBankProducts(products);
    //    // response
    //    //.success(function (data, status, headers, config) {
    //    //    console.log(data);
    //    //    GetTissueBankProductsByProductMasterId($scope.TissueBankId, ProductMasterId);
    //    //}).error(function (data, status, headers, config) {
    //    //    console.log(data);
    //    //});
    //};
    //-------------save new functionality end
    //-------- description pop up start
    //$scope.Cancel = function () {
    //    ngDialog.close();
    //}
    //$scope.Submit = function () {
    //    ngDialog.close();
    //    //console.log($scope.ngDialogData.selectedDescription + "," + $scope.ngDialogData.selectedTissueBankProductId);
    //    //console.log($scope.TbProducts[0].ProductDescription);
    //    var arr = $filter('updateById')($scope.TbProducts, $scope.ngDialogData.selectedTissueBankProductId, $scope.ngDialogData.selectedDescription);
    //    $scope.TbProducts = arr;
    //    console.log($scope.TbProducts);
    //};
    //$scope.openDescriptionPopUp = function (ProductDescription, TissueBankProductId) {
    //    var new_dialog = ngDialog.open({ id: 'fromAService', template: 'firstDialogId', controller: 'ProductDetailController', data: { selectedDescription: ProductDescription, selectedTissueBankProductId: TissueBankProductId } });
    //    //console.log(ProductDescription + "," + TissueBankProductId);
    //};
    //------- description pop up end
    //------- remove product start
    //$scope.checkAll = function () {
    //    if (!$scope.selectedAll) {
    //        $scope.selectedAll = true;
    //    } else {
    //        $scope.selectedAll = false;
    //    }
    //    angular.forEach($scope.TbProducts, function (TbProducts) {
    //        TbProducts.selected = $scope.selectedAll;
    //    });
    //};
    //$scope.remove = function () {
    //    var newDataList = [];
    //    $scope.selectedAll = false;
    //    angular.forEach($scope.TbProducts, function (selected) {
    //        if (!selected.selected) {
    //            newDataList.push(selected);
    //        }
    //    });
    //    $scope.TbProducts = newDataList;
    //};
    //------- remove product end

});

app.controller("RFQController", function ($scope, RFQService, MsgService, $window, ResourceService) {

    var TissueBankId = document.getElementById("TissueBankId").value;
    if (TissueBankId != "") {
        $scope.TissueBankId = TissueBankId;
    }

    var InfoType = document.getElementById("InfoType").value;
    if (InfoType != "") {
        $scope.InfoType = InfoType;
    }

    $scope.webApiRootPath = ResourceService.webApiRootPath;
    $scope.webApiContentRootPath = ResourceService.webApiContentRootPath;

    $scope.SortDirection = 'ASC';
    $scope.SortExpression = '';
    $scope.CurrentPage = 1;
    $scope.SearchBy = "";
    $scope.descriptionLimit = 15;
    $scope.dataLoading = true;
    $scope.TotalRFQs = 0;
    $scope.editMode = false;
    $scope.TotalPage = 0;

    $scope.PageSizes = [10, 20, 50, 100];
    $scope.PageSize = $scope.PageSizes[0];

    function GetRFQs(SearchBy, CurrentPage, PageSize, SortExpression, SortDirection) {
        var rfq_TissueBank_DTO = new Object();

        rfq_TissueBank_DTO.TissueBankId = $scope.TissueBankId;
        rfq_TissueBank_DTO.CurrentPage = CurrentPage;
        rfq_TissueBank_DTO.SortExpression = SortExpression;
        rfq_TissueBank_DTO.SortDirection = SortDirection;
        rfq_TissueBank_DTO.PageSize = PageSize;
        rfq_TissueBank_DTO.SearchBy = SearchBy;
        rfq_TissueBank_DTO.OperationType = "GetAll";


        RFQService.getRFQs(rfq_TissueBank_DTO)
        .success(function (data, status, headers, config) {

            var str = data.ReturnMessage[0];
            var arr = str.split(" ");

            $scope.TotalRFQs = arr[0];
            $scope.RequestForQuotes = data.RequestForQuotes;

            $scope.SortDirection = SortDirection;
            $scope.SortExpression = SortExpression;
            $scope.CurrentPage = CurrentPage;
            $scope.SearchBy = SearchBy;

            $scope.TotalPage = Math.ceil($scope.TotalRFQs / $scope.PageSize);
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        }).finally(function () {
            $scope.dataLoading = false;
        });
    }

    GetRFQs($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);

    $scope.getPreviousPage = function () {
        $scope.CurrentPage = $scope.CurrentPage - 1;
        GetRFQs($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };
    $scope.getLastPage = function () {
        $scope.CurrentPage = $scope.TotalPage;
        GetRFQs($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.getNextPage = function () {
        $scope.CurrentPage = $scope.CurrentPage + 1;
        GetRFQs($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };
    $scope.getFirstPage = function () {
        $scope.CurrentPage = 1;
        GetRFQs($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.search = function (SearchBy) {
        $scope.CurrentPage = 1;
        $scope.PageSize = $scope.PageSizes[0];
        $scope.SearchBy = SearchBy;
        GetRFQs($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.Sort = function (SortExpression) {
        $scope.CurrentPage = 1;
        $scope.SortExpression = SortExpression;

        if ($scope.SortDirection == 'ASC')
            $scope.SortDirection = 'DESC';
        else
            $scope.SortDirection = 'ASC';

        GetRFQs($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.PageSizeChanged = function (PageSize) {
        $scope.CurrentPage = 1;
        $scope.PageSize = PageSize;
        GetRFQs($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.GetRequestDetail = function (RequestForQuoteId) {
        var rfq_TissueBank_DTO = new Object();

        rfq_TissueBank_DTO.RequestForQuoteId = RequestForQuoteId;
        rfq_TissueBank_DTO.TissueBankId = TissueBankId;
        rfq_TissueBank_DTO.InfoType = InfoType;
        rfq_TissueBank_DTO.OperationType = "GetById";

        GetRequestDetailByRequestForQuoteId(rfq_TissueBank_DTO);

        GetRequestResponseByRequestForQuoteId(RequestForQuoteId);
    };

    GetRequestDetailByRequestForQuoteId = function (rfq_TissueBank_DTO) {

        RFQService.GetRfqDetailByRequestForQuoteId(rfq_TissueBank_DTO)
        .success(function (data, status, headers, config) {
            $scope.RFQDetail = data.RequestForQuoteDetail[0];

        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });
    };

    GetRequestResponseByRequestForQuoteId = function (RequestForQuoteId) {
        RFQService.GetRequestResponseByRequestForQuoteId(RequestForQuoteId)
        .success(function (data, status, headers, config) {
            $scope.RequestResponses = data.RequestResponses;
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });
    };

    function message(type, title, content) {
        var notify = {
            type: type,
            title: title,
            content: content
        };
        $scope.$emit('notify', notify);
    }
});

app.controller("RFQDetailController", function ($scope, RFQService, MsgService, $window, $timeout, appInfo, ResourceService) {
    $scope.CreatedBy = document.getElementById("UserId").value;
    $scope.LastModifiedBy = document.getElementById("UserId").value;
    $scope.webApiRootPath = ResourceService.webApiRootPath;
    $scope.webApiContentRootPath = ResourceService.webApiContentRootPath;
    $scope.dataLoading = true;

    appInfo.setInfo({ message: "No file selected." });
    appInfo.setInfo({ busy: false });

    var TissueBankId = document.getElementById("TissueBankId").value;
    if (TissueBankId != "") {
        $scope.TissueBankId = TissueBankId;
    }

    var InfoType = document.getElementById("InfoType").value;
    if (InfoType != "") {
        $scope.InfoType = InfoType;
    }

    var ResRFQSalesTaxPercentage = document.getElementById("ResRFQSalesTaxPercentage").value;
    if (ResRFQSalesTaxPercentage != "") {
        $scope.ResRFQSalesTaxPercentage = ResRFQSalesTaxPercentage;
    }

    var RequestForQuoteId = document.getElementById("RequestForQuoteId").value;
    if (RequestForQuoteId != "" && TissueBankId != "" && InfoType != "") {
        $scope.RequestForQuoteId = RequestForQuoteId;

        var rfq_TissueBank_DTO = new Object();

        rfq_TissueBank_DTO.RequestForQuoteId = $scope.RequestForQuoteId;
        rfq_TissueBank_DTO.TissueBankId = TissueBankId;
        rfq_TissueBank_DTO.InfoType = InfoType;
        rfq_TissueBank_DTO.OperationType = "GetById";

        GetRequestDetailByRequestForQuoteId(rfq_TissueBank_DTO);

        GetRequestResponseByRequestForQuoteId(RequestForQuoteId);
    }

    var editMode = document.getElementById("editMode").value;
    if (editMode != "") {
        $scope.editMode = editMode;
    }

    function GetRequestDetailByRequestForQuoteId(rfq_TissueBank_DTO) {
        RFQService.GetRfqDetailByRequestForQuoteId(rfq_TissueBank_DTO)
        .success(function (data, status, headers, config) {
            $scope.RFQDetail = data.RequestForQuoteDetail[0];
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        }).finally(function () {
            $scope.dataLoading = false;
        });
    };

    $scope.CalculateTotal = function () {

        if ($scope.RFQDetail.UnitPrice != null && $scope.RFQDetail.UnitPrice != '') {
            $scope.RFQDetail.LineTotal = $scope.RFQDetail.Quantity * $scope.RFQDetail.UnitPrice;
        }
        else {
            $scope.RFQDetail.LineTotal = 0;
        }
        $scope.RFQDetail.SalesTax = ($scope.RFQDetail.LineTotal * $scope.ResRFQSalesTaxPercentage) / 100;
        $scope.RFQDetail.Total = $scope.RFQDetail.SalesTax + $scope.RFQDetail.LineTotal;
    };

    $scope.trixChange = function (e, editor) {
        var document = editor.getDocument()
        $scope.ResponseBody = document.toString();
    }

    $scope.submitResponse = function () {
        if ($window.UploadedAttachmentName != null && $window.UploadedAttachmentName != "") {
            if ($scope.ResponseBody == null || $scope.ResponseBody == '') {
                $scope.showModal = true;
            }
            else {
                console.log('attachment with reply')
                SubmitResponse($scope.RFQDetail.StatusId);
            }
        }
        else {
            console.log('no attachment')
            SubmitResponse($scope.RFQDetail.StatusId);
        }
    };

    $scope.CancelSubmit = function () {
        $scope.showModal = false;
    }

    $scope.ConfirmSubmit = function () {

        //$window.UploadedAttachmentName = null;
        $scope.showModal = false;
        //appInfo.setInfo({ message: "No file selected." });
        //appInfo.setInfo({ busy: false });

        SubmitResponse($scope.RFQDetail.StatusId);
    };

    function message(type, title, content) {
        var notify = {
            type: type,
            title: title,
            content: content
        };
        $scope.$emit('notify', notify);
    };

    $scope.Edit_Cancel = function () {

        if ($window.UploadedAttachmentName == null) {
            $window.UploadedAttachmentName == "";
        }

        if ($scope.form_ConfirmOnExit.$dirty == true || $window.UploadedAttachmentName != "") {
            if ($window.confirm("You have some unsaved changes.Do you really want to cancel?")) {

                $scope.form_ConfirmOnExit.$dirty = false;

                //get data again from db
                var rfq_TissueBank_DTO = new Object();

                rfq_TissueBank_DTO.RequestForQuoteId = $scope.RequestForQuoteId;
                rfq_TissueBank_DTO.TissueBankId = TissueBankId;
                rfq_TissueBank_DTO.InfoType = InfoType;
                rfq_TissueBank_DTO.OperationType = "GetById";

                GetRequestDetailByRequestForQuoteId(rfq_TissueBank_DTO);
                $scope.ResponseBody = '';
                $scope.editMode = !$scope.editMode;

                //default for photo
                ResetPhoto($scope);
                //$window.UploadedAttachmentName = null;
                //$scope.showModal = false;
                //appInfo.setInfo({ message: "No file selected." });
                //appInfo.setInfo({ busy: false });
            }
        }
        else {
            $scope.editMode = !$scope.editMode;
        }
    };

    function ResetPhoto($scope) {
        $window.UploadedAttachmentName = null;
        $scope.showModal = false;
        appInfo.setInfo({ message: "No file selected." });
        appInfo.setInfo({ busy: false });
    };

    $scope.DeclineRFQ = function () {
        SubmitResponse(5);
    };

    function SubmitResponse(StatusId) {
        if ($scope.RFQDetail.TissueBankSendByDate != '' && $scope.RFQDetail.TissueBankSendByDate != null) {
            if (isNaN(Date.parse($scope.RFQDetail.TissueBankSendByDate))) {
                message('error', 'Error!', 'Invalid shipping date');
                console.log('Invalid date');
                return;
            }
        }
        //if ($window.UploadedAttachmentName == "") {
        //    $window.UploadedAttachmentName == null;
        //}
        var RFQ_TissueBank_Edit_DTO = {};
        RFQ_TissueBank_Edit_DTO.TissueBankId = $scope.TissueBankId;
        RFQ_TissueBank_Edit_DTO.ResponseBody = $scope.ResponseBody;
        RFQ_TissueBank_Edit_DTO.AttachmentName = $window.UploadedAttachmentName;
        RFQ_TissueBank_Edit_DTO.CreatedBy = $scope.CreatedBy;
        RFQ_TissueBank_Edit_DTO.LastModifiedBy = $scope.LastModifiedBy;
        RFQ_TissueBank_Edit_DTO.RequestForQuoteId = $scope.RequestForQuoteId;
        RFQ_TissueBank_Edit_DTO.StatusId = StatusId;
        RFQ_TissueBank_Edit_DTO.DeclineRemark = $scope.RFQDetail.DeclineRemark;
        RFQ_TissueBank_Edit_DTO.Quantity = $scope.RFQDetail.Quantity;
        RFQ_TissueBank_Edit_DTO.UnitPrice = $scope.RFQDetail.UnitPrice;
        RFQ_TissueBank_Edit_DTO.LineTotal = $scope.RFQDetail.LineTotal;
        RFQ_TissueBank_Edit_DTO.SalesTax = $scope.RFQDetail.SalesTax;
        RFQ_TissueBank_Edit_DTO.Total = $scope.RFQDetail.Total;
        RFQ_TissueBank_Edit_DTO.TissueBankSendByDate = $scope.RFQDetail.TissueBankSendByDate;
        RFQ_TissueBank_Edit_DTO.ShippingMethod = $scope.RFQDetail.ShippingMethod;

        console.log(RFQ_TissueBank_Edit_DTO);

        var response = RFQService.RequestForQuote_Edit(RFQ_TissueBank_Edit_DTO);

        ResetPhoto($scope);

        response
       .success(function (data, status, headers, config) {
           $scope.editMode = false;
           $window.UploadedAttachmentName = "";
           $scope.form_ConfirmOnExit.$dirty = false;
           //default values
           $scope.ResponseBody = '';
           var rfq_TissueBank_DTO = new Object();

           rfq_TissueBank_DTO.RequestForQuoteId = $scope.RequestForQuoteId;
           rfq_TissueBank_DTO.TissueBankId = TissueBankId;
           rfq_TissueBank_DTO.InfoType = InfoType;
           rfq_TissueBank_DTO.OperationType = "GetById";

           GetRequestDetailByRequestForQuoteId(rfq_TissueBank_DTO);

           GetRequestResponseByRequestForQuoteId(RequestForQuoteId);
           var Message = MsgService.makeMessage(data.ReturnMessage)
           message('success', 'Success!', Message);
       })
       .error(function (data, status, headers, config) {
           var Message = MsgService.makeMessage(data.ReturnMessage)
           message('error', 'Error!', Message);
       });
    }

    function GetRequestResponseByRequestForQuoteId(RequestForQuoteId) {
        RFQService.GetRequestResponseByRequestForQuoteId(RequestForQuoteId)
        .success(function (data, status, headers, config) {
            $scope.RequestResponses = data.RequestResponses;
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });
    };
});

app.controller("OrderController", function ($scope, OrderService, MsgService, $window, $sce, ResourceService) {
    $scope.webApiRootPath = ResourceService.webApiRootPath;

    var TissueBankId = document.getElementById("TissueBankId").value;
    if (TissueBankId != "") {
        $scope.TissueBankId = TissueBankId;
    }
    $scope.SortDirection = 'ASC';
    $scope.SortExpression = 'ProductMasterName';
    $scope.CurrentPage = 1;
    $scope.SearchBy = "";
    $scope.descriptionLimit = 15;
    $scope.dataLoading = true;
    $scope.TotalOrders = 0;
    $scope.editMode = false;
    $scope.TotalPage = 0;

    $scope.PageSizes = [10, 20, 50, 100];
    $scope.PageSize = $scope.PageSizes[0];

    function GetOrders(SearchBy, CurrentPage, PageSize, SortExpression, SortDirection) {
        var order_TissueBank_DTO = new Object();

        order_TissueBank_DTO.TissueBankId = $scope.TissueBankId;
        order_TissueBank_DTO.CurrentPage = CurrentPage;
        order_TissueBank_DTO.SortExpression = SortExpression;
        order_TissueBank_DTO.SortDirection = SortDirection;
        order_TissueBank_DTO.PageSize = PageSize;
        order_TissueBank_DTO.SearchBy = SearchBy;
        order_TissueBank_DTO.OperationType = 'GetAll';

        OrderService.GetOrders(order_TissueBank_DTO)
        .success(function (data, status, headers, config) {

            var str = data.ReturnMessage[0];
            var arr = str.split(" ");
            console.log(data.Orders);
            $scope.TotalOrders = arr[0];
            $scope.Orders = data.Orders;
            $scope.TotalPage = Math.ceil($scope.TotalOrders / $scope.PageSize);

        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        }).finally(function () {
            $scope.dataLoading = false;
        });
    }

    GetOrders($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);

    $scope.getPreviousPage = function () {
        $scope.CurrentPage = $scope.CurrentPage - 1;
        GetOrders($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.getNextPage = function () {
        $scope.CurrentPage = $scope.CurrentPage + 1;
        GetOrders($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };
    $scope.getLastPage = function () {
        $scope.CurrentPage = $scope.TotalPage;
        GetOrders($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.getFirstPage = function () {
        $scope.CurrentPage = 1;
        GetOrders($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.search = function () {
        GetOrders($scope.SearchBy, 1, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);

        $scope.CurrentPage = 1;
        $scope.PageSize = $scope.PageSizes[0];
    };

    $scope.Sort = function (SortExpression) {
        $scope.CurrentPage = 1;
        $scope.SortExpression = SortExpression;

        if ($scope.SortDirection == 'ASC')
            $scope.SortDirection = 'DESC';
        else
            $scope.SortDirection = 'ASC';

        GetOrders($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.PageSizeChanged = function () {
        $scope.CurrentPage = 1;
        GetOrders($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.GetOrderDetail = function (OrderId) {
        OrderService.GetOrderDetailByOrderId(OrderId)
        .success(function (data, status, headers, config) {
            $scope.OrderDetail = data.OrderDetail[0];
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });
    };

    function message(type, title, content) {
        var notify = {
            type: type,
            title: title,
            content: content
        };
        $scope.$emit('notify', notify);
    }
});

app.controller("OrderDetailController", function ($scope, OrderService, MsgService, $window, $timeout, ResourceService) {
    $scope.CreatedBy = document.getElementById("UserId").value;
    $scope.LastModifiedBy = document.getElementById("UserId").value;
    $scope.ShippingMethod = '';
    $scope.webApiRootPath = ResourceService.webApiRootPath;
    $scope.dataLoading = true;
    $scope.showDeclineModal = false;
    $scope.showModal = false;

    var TissueBankId = document.getElementById("TissueBankId").value;
    if (TissueBankId != "") {
        $scope.TissueBankId = TissueBankId;
    }

    var InfoType = document.getElementById("InfoType").value;
    if (InfoType != "") {
        $scope.InfoType = InfoType;
    }

    var OrderId = document.getElementById("OrderId").value;
    if (OrderId != "" && TissueBankId != "" && InfoType != "") {
        $scope.OrderId = OrderId;
        var order_TissueBank_DTO = new Object();

        order_TissueBank_DTO.OrderId = $scope.OrderId;
        order_TissueBank_DTO.TissueBankId = TissueBankId;
        order_TissueBank_DTO.InfoType = InfoType;
        order_TissueBank_DTO.OperationType = 'GetById';

        GetOrderDetailByOrderId(order_TissueBank_DTO);
    }

    function GetOrderDetailByOrderId(order_TissueBank_DTO) {
        OrderService.GetOrderDetailByOrderId(order_TissueBank_DTO)
        .success(function (data, status, headers, config) {
            $scope.OrderDetail = data.OrderDetail[0];
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        }).finally(function () {
            $scope.dataLoading = false;
        });
    };

    $scope.Acknowledge = function () {

        if ($scope.OrderDetail.TissueBankSendByDate != '' && $scope.OrderDetail.TissueBankSendByDate != null) {
            if (isNaN(Date.parse($scope.OrderDetail.TissueBankSendByDate))) {
                message('error', 'Error!', 'Tissue Bank Send By Date is invalid');
                console.log('Invalid date');
                return;
            }
        }
        $scope.showModal = true;
    };

    $scope.ConfirmAcknowledge = function () {
        $scope.showModal = false;
        Order_Ack_Decline(1008);
    };

    function message(type, title, content) {
        var notify = {
            type: type,
            title: title,
            content: content
        };
        $scope.$emit('notify', notify);
    };

    $scope.DeclineOrder = function () {
        $scope.showDeclineModal = true;
    };

    $scope.ConfirmDeclineOrder = function () {
        Order_Ack_Decline(5);
        $scope.showDeclineModal = false;
    };

    function Order_Ack_Decline(StatusId) {
        var order_Ack_Decline_DTO = {};
        order_Ack_Decline_DTO.OrderId = $scope.OrderId;
        order_Ack_Decline_DTO.StatusId = StatusId;
        order_Ack_Decline_DTO.DeclineRemark = $scope.OrderDetail.DeclineRemark;
        order_Ack_Decline_DTO.ShippingMethod = $scope.OrderDetail.ShippingMethod;
        order_Ack_Decline_DTO.TissueBankSendByDate = $scope.OrderDetail.TissueBankSendByDate;
        order_Ack_Decline_DTO.LastModifiedBy = $scope.LastModifiedBy;

        console.log(order_Ack_Decline_DTO);

        var response = OrderService.Order_Ack_Decline(order_Ack_Decline_DTO);

        response
       .success(function (data, status, headers, config) {
           var Message = MsgService.makeMessage(data.ReturnMessage)
           message('success', 'Success!', Message);
           //default values
           var order_TissueBank_DTO = new Object();

           order_TissueBank_DTO.OrderId = $scope.OrderId;
           order_TissueBank_DTO.TissueBankId = TissueBankId;
           order_TissueBank_DTO.InfoType = InfoType;
           order_TissueBank_DTO.OperationType = 'GetById';

           GetOrderDetailByOrderId(order_TissueBank_DTO);
       })
       .error(function (data, status, headers, config) {
           var Message = MsgService.makeMessage(data.ReturnMessage)
           message('error', 'Error!', Message);
       });
        $scope.OrderDetail.DeclineRemark = '';
    }

    $scope.Received = function () {
        var order_Ack_Decline_DTO = {};
        order_Ack_Decline_DTO.OrderId = $scope.OrderId;
        order_Ack_Decline_DTO.StatusId = 1009;
        order_Ack_Decline_DTO.ShippingMethod = $scope.OrderDetail.ShippingMethod;
        order_Ack_Decline_DTO.TissueBankSendByDate = $scope.OrderDetail.TissueBankSendByDate;
        order_Ack_Decline_DTO.LastModifiedBy = $scope.LastModifiedBy;

        console.log(order_Ack_Decline_DTO);

        var response = OrderService.Order_Ack_Decline(order_Ack_Decline_DTO);

        response
       .success(function (data, status, headers, config) {
           var Message = MsgService.makeMessage(data.ReturnMessage)
           message('success', 'Success!', Message);
           //default values
           var order_TissueBank_DTO = new Object();

           order_TissueBank_DTO.OrderId = $scope.OrderId;
           order_TissueBank_DTO.TissueBankId = TissueBankId;
           order_TissueBank_DTO.InfoType = InfoType;
           order_TissueBank_DTO.OperationType = 'GetById';

           GetOrderDetailByOrderId(order_TissueBank_DTO);
       })
       .error(function (data, status, headers, config) {
           var Message = MsgService.makeMessage(data.ReturnMessage)
           message('error', 'Error!', Message);
       });
        $scope.OrderDetail.DeclineRemark = '';
    };
});

app.controller("UserController", function ($scope, UserService, MsgService, $window, $sce, ResourceService) {

    $scope.TissueBankId = document.getElementById("InfoId").value;

    $scope.SortDirection = 'ASC';
    $scope.SortExpression = 'FullName';
    $scope.CurrentPage = 1;
    $scope.SearchBy = '';
    $scope.descriptionLimit = 15;
    $scope.dataLoading = true;
    $scope.TotalUsers = 0;
    $scope.editMode = false;
    $scope.TotalPage = 0;

    $scope.PageSizes = [3, 10, 20, 50, 100];
    $scope.PageSize = $scope.PageSizes[0];

    var msg = document.getElementById("msg").value;

    if (msg != "") {
        $scope.Message = msg;
        document.getElementById("msg").value = '';
    }

    function GetUsers(SearchBy, CurrentPage, PageSize, SortExpression, SortDirection) {
        var user_DTO = new Object();

        user_DTO.TissueBankId = $scope.TissueBankId;
        user_DTO.CurrentPage = CurrentPage;
        user_DTO.SortExpression = SortExpression;
        user_DTO.SortDirection = SortDirection;
        user_DTO.PageSize = PageSize;
        user_DTO.SearchBy = SearchBy;
        user_DTO.OperationType = 'GetAll';

        UserService.GetUsers(user_DTO)
        .success(function (data, status, headers, config) {

            var str = data.ReturnMessage[0];
            var arr = str.split(" ");
            console.log(data.Users);
            $scope.TotalUsers = arr[0];
            $scope.Users = data.Users;
            $scope.TotalPage = Math.ceil($scope.TotalUsers / $scope.PageSize);

            for (var i = 0; i < $scope.Users.length; ++i) {
                $scope.Users[i].UserRoles = $sce.trustAsHtml($scope.Users[i].UserRoles);
            }

        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        }).finally(function () {
            $scope.dataLoading = false;
        });
    }

    GetUsers($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);

    $scope.getPreviousPage = function () {
        $scope.CurrentPage = $scope.CurrentPage - 1;
        GetUsers($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.getNextPage = function () {
        $scope.CurrentPage = $scope.CurrentPage + 1;
        GetUsers($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.getLastPage = function () {
        $scope.CurrentPage = $scope.TotalPage;
        GetUsers($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.getFirstPage = function () {
        $scope.CurrentPage = 1;
        GetUsers($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.search = function (SearchBy) {
        $scope.SearchBy = SearchBy;
        GetUsers($scope.SearchBy, 1, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);

        $scope.CurrentPage = 1;
        $scope.PageSize = $scope.PageSizes[0];
    };

    $scope.Sort = function (SortExpression) {
        $scope.CurrentPage = 1;
        $scope.SortExpression = SortExpression;

        if ($scope.SortDirection == 'ASC')
            $scope.SortDirection = 'DESC';
        else
            $scope.SortDirection = 'ASC';

        GetUsers($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.PageSizeChanged = function () {
        $scope.CurrentPage = 1;
        GetUsers($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.GetUserDetail = function (OrderId) {
        UserService.GetUserDetail(OrderId)
        .success(function (data, status, headers, config) {
            $scope.OrderDetail = data.OrderDetail[0];
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });
    };




    function message(type, title, content) {
        var notify = {
            type: type,
            title: title,
            content: content
        };
        $scope.$emit('notify', notify);
    }
});

app.controller("UserDetailController", function ($scope, UserDetailService, MsgService, $window, $timeout, InputService) {
    $scope.S_UserId = "";
    $scope.UserDetail = {};
    $scope.UserRoles = [];
    $scope.Operation = 'Add User';
    $scope.Password = '';
    $scope.IsSendMail = false;
    $scope.dataLoading = true;

    $scope.validateFullName = /^[A-Za-z\s]+$/;
    $scope.validateEmail = /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/

    $scope.CreatedBy = document.getElementById("LoggedUserId").value;
    $scope.LastModifiedBy = document.getElementById("LoggedUserId").value;
    $scope.phoneNumberPattern = InputService.phoneNumberPattern;
    $scope.emailPattern = InputService.emailPattern;
    $scope.name_AlphaSpacesPattern = InputService.name_AlphaSpacesPattern;
    $scope.userNamePattern = InputService.userNamePattern;
    $scope.phoneNumberLength = InputService.phoneNumberLength;

    GetTissueBankRoles();

    var InfoType = document.getElementById("InfoType").value;
    if (InfoType != "") {
        $scope.InfoType = InfoType;
    }

    var InfoId = document.getElementById("InfoId").value;
    if (InfoId != "") {
        $scope.InfoId = InfoId;
    }

    var S_UserId = document.getElementById("S_UserId").value;
    if (S_UserId != "" && InfoId != "" && InfoType != "") {
        $scope.S_UserId = S_UserId;

        var user_DTO = new Object();

        user_DTO.RequestUserId = $scope.S_UserId;
        user_DTO.TissueBankId = InfoId;
        user_DTO.InfoType = InfoType;
        user_DTO.OperationType = 'GetById';
        GetUserDetail(user_DTO);
        $scope.Operation = 'Manage User';
    }


    function GetUserDetail(user_DTO) {
        UserDetailService.GetUserDetail(user_DTO)
        .success(function (data, status, headers, config) {
            $scope.UserDetail = data.UserDetail[0];

            console.log(data.UserDetail[0]);
            GetUserRoles(user_DTO);



            if (data.UserDetail[0].AllowLogin) {
                $scope.UserDetail.AllowLogin_Convert = 'Yes';
            }
            else {
                $scope.UserDetail.AllowLogin_Convert = 'No';
            }
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        }).finally(function () {
            $scope.dataLoading = false;
        });
    };

    function GetUserRoles(user_DTO) {
        user_DTO.OperationType = 'GetUserRole';

        UserDetailService.GetUserRoles(user_DTO)
        .success(function (data, status, headers, config) {
            $scope.UserRoles = data.UserRoles;



            IsUserInfoAdmin(user_DTO);

        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });
    };

    function GetTissueBankRoles() {
        UserDetailService.GetTissueBankRoles('TISSUE BANK')
        .success(function (data, status, headers, config) {
            $scope.TissueBankRoles = data.TissueBankRoles;
            console.log(data.TissueBankRoles);

        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });
    };

    function IsUserInfoAdmin(user_DTO) {

        user_DTO.OperationType = 'IsUserInfoAdmin';

        UserDetailService.IsUserInfoAdmin(user_DTO)
        .success(function (data, status, headers, config) {

            console.log('IsUserInfoAdmin : ' + data.IsUserInfoAdmin);
            if (data.IsUserInfoAdmin) {
                for (var i = 0; i < $scope.UserRoles.length; ++i) {
                    $scope.UserRoles[i].Disable = true;
                }
            }
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });
    };



    $scope.isChecked = function (tbRole) {
        if (typeof $scope.UserRoles != 'undefined') {
            var UserRoles = $scope.UserRoles;
            for (var i = 0; i < UserRoles.length; i++) {
                if (tbRole.RoleId == UserRoles[i].RoleId)
                    return true;
            }

            return false;
        }
    };

    $scope.isDisable = function (tbRole) {
        if (typeof $scope.UserRoles != 'undefined') {
            var UserRoles = $scope.UserRoles;
            for (var i = 0; i < UserRoles.length; i++) {
                if (tbRole.RoleId == UserRoles[i].RoleId && UserRoles[i].Disable == true)
                    return true;
            }

            return false;
        }
    };

    $scope.Check = function (tbRole) {
        var found = false;
        for (var i = 0; i < $scope.UserRoles.length; i++) {
            if ($scope.UserRoles[i].RoleId == tbRole.RoleId) {
                found = true;
                break;
            }
        }
        if (found == true) {
            $scope.UserRoles.splice(i, 1);
        }
        else {
            $scope.UserRoles.push(tbRole);
        }
        console.log($scope.UserRoles)
    };

    $scope.Submit = function () {
        if ($scope.UserRoles.length > 0) {
            var userMngmnt_User_CUD_DTO = {};
            userMngmnt_User_CUD_DTO.UserId = $scope.S_UserId;
            userMngmnt_User_CUD_DTO.UserName = $scope.UserDetail.UserName;
            userMngmnt_User_CUD_DTO.FullName = $scope.UserDetail.FullName;
            userMngmnt_User_CUD_DTO.MobileNumber = $scope.UserDetail.MobileNumber;
            userMngmnt_User_CUD_DTO.EmailId = $scope.UserDetail.EmailId;
            userMngmnt_User_CUD_DTO.CreatedBy = $scope.CreatedBy;
            userMngmnt_User_CUD_DTO.LastModifiedBy = $scope.LastModifiedBy;
            userMngmnt_User_CUD_DTO.InfoId = $scope.InfoId;
            userMngmnt_User_CUD_DTO.AllowLogin = $scope.UserDetail.AllowLogin;
            userMngmnt_User_CUD_DTO.TempUser_CUD = $scope.UserRoles;

            if ($scope.S_UserId == '') {
                userMngmnt_User_CUD_DTO.OperationType = 'insert';
            }
            else {
                userMngmnt_User_CUD_DTO.OperationType = 'update';
            }

            console.log(userMngmnt_User_CUD_DTO);

            var response = UserDetailService.SubmitUser(userMngmnt_User_CUD_DTO);

            response
           .success(function (data, status, headers, config) {
               var Message = MsgService.makeMessage(data.ReturnMessage)
               //message('success', 'Success!', Message);
               ////default values
               //GetUserDetail($scope.S_UserId);
               $window.location.href = '/TissueBank/User/Index?msg=' + Message;
               console.log(data);
           })
           .error(function (data, status, headers, config) {
               var Message = MsgService.makeMessage(data.ReturnMessage)
               message('error', 'Error!', Message);
           });
        }
        else {
            message('error', 'Error!', 'Please select a role for this user!');
        }
    }

    $scope.OpenPasswordModel = function () {
        $scope.showModal = true;

    };

    $scope.PasswordSubmit = function (IsSendMail) {
        var userMngmnt_User_CUD_DTO = {};
        userMngmnt_User_CUD_DTO.UserId = $scope.S_UserId;
        userMngmnt_User_CUD_DTO.LastModifiedBy = $scope.LastModifiedBy;
        userMngmnt_User_CUD_DTO.Password = $scope.UserDetail.Password;
        userMngmnt_User_CUD_DTO.OperationType = 'changePass';
        userMngmnt_User_CUD_DTO.IsSendMail = IsSendMail;

        console.log(userMngmnt_User_CUD_DTO);

        var response = UserDetailService.SubmitUser(userMngmnt_User_CUD_DTO);

        response
       .success(function (data, status, headers, config) {
           var Message = MsgService.makeMessage(data.ReturnMessage)
           message('success', 'Success!', Message);
           $scope.showModal = false;
       })
       .error(function (data, status, headers, config) {
           var Message = MsgService.makeMessage(data.ReturnMessage)
           message('error', 'Error!', Message);
       });

        $scope.UserDetail.Password = "";
        $scope.password_verify = "";
        $scope.IsSendMail = "";
    };

    $scope.PasswordCancel = function () {
        $scope.showModal = false;
        $scope.UserDetail.Password = "";
        $scope.password_verify = "";
        $scope.IsSendMail = "";
    };

    function message(type, title, content) {
        var notify = {
            type: type,
            title: title,
            content: content
        };
        $scope.$emit('notify', notify);
    };


});

app.controller("TissueBankProfileController", function ($scope, TissueBankService, StateService, CityService, $window, MsgService, InputService) {
    $scope.tissueBank = {};
    $scope.tissueBank.States = [];
    $scope.tissueBank.BillingStates = [];
    $scope.tissueBank.TissueBankId = document.getElementById("TissueBankId").value;
    $scope.tissueBank.LoggedUserId = document.getElementById("LoggedUserId").value;

    //angular validation
    $scope.phoneNumberPattern = InputService.phoneNumberPattern;
    $scope.emailPattern = InputService.emailPattern;
    $scope.name_AlphaSpacesPattern = InputService.name_AlphaSpacesPattern;
    $scope.addressPattern = InputService.addressPattern;
    $scope.userNamePattern = InputService.userNamePattern;
    $scope.faxNumberPattern = InputService.faxNumberPattern;
    $scope.CustomerServiceLandLineNumberPattern = InputService.CustomerServiceLandLineNumberPattern;
    $scope.CreditCardNumberPattern = InputService.CreditCardNumberPattern;
    $scope.ZipCodePattern = InputService.ZipCodePattern;

    $scope.creditCardMinLength = InputService.creditCardMinLength;
    $scope.cvvLength = InputService.cvvLength;
    $scope.zipcodeLength = InputService.zipcodeLength;
    $scope.faxNumberMinLength = InputService.faxNumberMinLength;
    $scope.faxNumberMaxLength = InputService.faxNumberMaxLength;
    $scope.aATBLicenseNumberMaxLength = InputService.aATBLicenseNumberMaxLength;
    $scope.tissueBankStateLicenseMaxLength = InputService.tissueBankStateLicenseMaxLength;
    $scope.expiryLength = InputService.expiryLength;
    $scope.AATBLicenseNumberPattern = InputService.AATBLicenseNumberPattern;
    $scope.TissueBankStateLicensePattern = InputService.TissueBankStateLicensePattern;

    $scope.dateOptions = {
        'starting-day': 1
    };

    $scope.months = ["0" + 1, "0" + 2, "0" + 3, "0" + 4, "0" + 5, "0" + 6, "0" + 7, "0" + 8, "0" + 9, "10", "11", "12"];
    $scope.years = [];
    var d = new Date().getFullYear().toString().substr(2, 2)
    for (var year = d ; year <= 80; ++year) {
        $scope.years.push(year);
    }

    if ($scope.tissueBank.TissueBankId == 0) {
        StateService.GetStates()
           .success(function (data, status, headers, config) {
               $scope.tissueBank.States = data.States;
               $scope.tissueBank.BillingStates = data.States;
           })
           .error(function (data, status, headers, config) {
               var Message = MsgService.makeMessage(data.ReturnMessage)
               message('error', 'Error!', Message);
           });
    }

    $scope.check = function () {
        console.log($scope.expiryMonth.toString() + $scope.expiryYear.toString());
    }

    $scope.GetCities = function (stateId) {
        GetCities(stateId);
    };

    $scope.GetBillingCities = function (stateId) {
        GetBillingCities(stateId);
    };

    $scope.NotAgreed = function () {
        $scope.showModal = false;
    }

    $scope.Agreed = function () {

        $scope.showModal = false;

        var err = '';
        err = ValidateTbSubmit();

        err = ValidateTbBillingDetailSubmit();

        if (err == '') {
            var tissueBankAdd_DTO = {
                TissueBankName: $scope.tissueBank.TissueBankName,
                ContactPersonFirstName: $scope.tissueBank.ContactPersonFirstName,
                ContactPersonLastName: $scope.tissueBank.ContactPersonLastName,
                ContactPersonNumber: $scope.tissueBank.ContactPersonNumber,
                ContactPersonEmailId: $scope.tissueBank.ContactPersonEmailId,
                FaxNumber: $scope.tissueBank.FaxNumber,
                TissueBankEmailId: $scope.tissueBank.ContactPersonEmailId,
                BusinessURL: $scope.tissueBank.BusinessURL,
                TissueBankAddress: $scope.tissueBank.TissueBankAddress,
                CityId: $scope.tissueBank.city.CityID,
                ZipCode: $scope.tissueBank.ZipCode,
                TissueBankStateLicense: $scope.tissueBank.TissueBankStateLicense,
                AATBAccredationDate: $scope.tissueBank.AATBAccredationDate,
                AATBLicenseNumber: $scope.tissueBank.AATBLicenseNumber,
                AATBExpirationDate: $scope.tissueBank.AATBExpirationDate,
                CreditCardNumber: $scope.tissueBank.CreditCardNumber,
                CreditCardType: 0,
                ExpiryDate: $scope.expiryMonth.toString() + $scope.expiryYear.toString(),
                CardCode: $scope.tissueBank.CardCode,
                BillingAddress: $scope.tissueBank.BillingAddress,
                BillingCityId: $scope.tissueBank.BillingCity.CityID,
                BillingZipCode: $scope.tissueBank.BillingZipCode,
                BillingFaxNumber: $scope.tissueBank.BillingFaxNumber,
                BillingEmailId: $scope.tissueBank.BillingEmailId,
                BillingContactNumber: $scope.tissueBank.BillingContactNumber,
                BillingCity: $scope.tissueBank.BillingCity.CityName,
                BillingState: $scope.tissueBank.BillingState.StateName,
                State: $scope.tissueBank.state.StateName,
                City: $scope.tissueBank.city.CityName,
                UserId: $scope.tissueBank.LoggedUserId,
                TissueBankId: $scope.tissueBank.TissueBankId
            };

            console.log(tissueBankAdd_DTO);

            var response = TissueBankService.AddTb(tissueBankAdd_DTO);
            response.success(function (data, status, headers, config) {
                $window.location.href = '/Response/TissueBank_Registration_Successful';
            }).error(function (data, status, headers, config) {
                console.log(data.ReturnMessage);
                var Message = MsgService.makeMessage(data.ReturnMessage)
                message('error', 'Error!', Message);
            });
        }
        else {
            message('error', 'Error!', err);
        }
    };

    $scope.AddTb = function () {
        $scope.showModal = true;
    };

    function ValidateTbSubmit() {
        var err = '';
        if (typeof $scope.tissueBank.AATBAccredationDate === 'string') {
            var TExist = $scope.tissueBank.AATBAccredationDate.includes("T");
            if (TExist == true) {
                var res = $scope.tissueBank.AATBAccredationDate.split("T");
                $scope.tissueBank.AATBAccredationDate = new Date(res[0]);
            }
        }
        if (typeof $scope.tissueBank.AATBExpirationDate === 'string') {
            TExist = $scope.tissueBank.AATBExpirationDate.includes("T");
            if (TExist == true) {
                var res = $scope.tissueBank.AATBExpirationDate.split("T");
                $scope.tissueBank.AATBExpirationDate = new Date(res[0]);
            }
        }

        if (Object.prototype.toString.call($scope.tissueBank.AATBAccredationDate) !== '[object Date]') {
            err = 'Invalid AATB Accredation Date';
        }
        if (Object.prototype.toString.call($scope.tissueBank.AATBExpirationDate) !== '[object Date]') {
            if (err == '') {
                err = 'Invalid AATB Expiration Date';
            }
            else {

                err = err + '<br/> Invalid AATB Expiration Date';
            }
        }
        if (err == '') {
            var AATBAccredationDate = new Date($scope.tissueBank.AATBAccredationDate);
            var AATBExpirationDate = new Date($scope.tissueBank.AATBExpirationDate);

            if (AATBExpirationDate < AATBAccredationDate) {
                err = 'AATB Expiration Date must be greated than AATB Accredation Date';
            }
        }


        //checking home address and office address
        if ($scope.tissueBank.TissueBankAddress == $scope.tissueBank.BillingAddress
            || $scope.tissueBank.BillingFaxNumber == $scope.tissueBank.FaxNumber
            || $scope.tissueBank.ContactPersonEmailId == $scope.tissueBank.BillingEmailId) {

            if ($scope.tissueBank.TissueBankAddress == $scope.tissueBank.BillingAddress) {
                if (err == '') {
                    err = 'TissueBank Address and Billing Address must be different.';
                }
                else {
                    err = err + '<br/>' + 'TissueBank Address and Billing Address must be different.';
                }
                $scope.tissueBank.BillingAddress = "";
            }
            if ($scope.tissueBank.BillingFaxNumber == $scope.tissueBank.FaxNumber) {
                if (err == '') {
                    err = 'Fax Number and Billing Fax Number must be different.';
                }
                else {
                    err = err + '<br/>' + ' Fax Number and Billing Fax Number must be different.';
                }
                $scope.tissueBank.BillingFaxNumber = "";
            }
            if ($scope.tissueBank.ContactPersonEmailId == $scope.tissueBank.BillingEmailId) {
                if (err == '') {
                    err = 'Email-Id and Billing Email-Id must be different.';
                }
                else {
                    err = err + '<br/>' + 'Email-Id and Billing Email-Id must be different.';
                }
                $scope.tissueBank.BillingEmailId = "";
            }
        }



        return err;
    }

    if ($scope.tissueBank.TissueBankId > 0) {
        //get tb details
        GetTissueBankById($scope.tissueBank.TissueBankId);
    }

    function GetTissueBankById(TissueBankId) {
        TissueBankService.GetTissueBankById(TissueBankId)
        .success(function (data, status, headers, config) {
            $scope.tissueBank = data.tissueBank;
            console.log($scope.tissueBank);

            //get states
            StateService.GetStates()
                .success(function (data, status, headers, config) {
                    $scope.tissueBank.States = data.States;
                    $scope.tissueBank.BillingStates = data.States;
                })
                .error(function (data, status, headers, config) {
                    var Message = MsgService.makeMessage(data.ReturnMessage)
                    message('error', 'Error!', Message);
                });

            //set billing state
            $scope.tissueBank.BillingState = { StateId: $scope.tissueBank.BillingStateId };
            //get billing city
            GetBillingCities($scope.tissueBank.BillingStateId);
            //set billing city
            $scope.tissueBank.BillingCity = { CityID: $scope.tissueBank.BillingCityId };
            //set state
            $scope.tissueBank.state = { StateId: $scope.tissueBank.StateId };
            //get city
            GetCities($scope.tissueBank.StateId);
            //set city
            $scope.tissueBank.city = { CityID: $scope.tissueBank.CityId };

        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });
    };

    $scope.UpdateTissueBankDetail = function () {
        var err = '';
        err = ValidateTbSubmit();

        if (ValidateTbSubmit() == '') {
            var tissueBankAdd_DTO = {
                OperationType: 'UpdateTissueBankDetail'
            , TissueBankId: document.getElementById("TissueBankId").value
            , UserId: document.getElementById("LoggedUserId").value

            , TissueBankName: $scope.tissueBank.TissueBankName
, ContactPersonFirstName: $scope.tissueBank.ContactPersonFirstName
, ContactPersonLastName: $scope.tissueBank.ContactPersonLastName
, ContactPersonNumber: $scope.tissueBank.ContactPersonNumber
, ContactPersonEmailId: $scope.tissueBank.ContactPersonEmailId
, FaxNumber: $scope.tissueBank.FaxNumber
, TissueBankEmailId: $scope.tissueBank.TissueBankEmailId
, BusinessURL: $scope.tissueBank.BusinessURL
, TissueBankAddress: $scope.tissueBank.TissueBankAddress
, CityId: $scope.tissueBank.CityId
, ZipCode: $scope.tissueBank.ZipCode
, CustomerServiceLandLineNumber: $scope.tissueBank.CustomerServiceLandLineNumber
, TaxPayerId: $scope.tissueBank.TaxPayerId
, TissueBankStateLicense: $scope.tissueBank.TissueBankStateLicense
, AATBLicenseNumber: $scope.tissueBank.AATBLicenseNumber
, AATBExpirationDate: $scope.tissueBank.AATBExpirationDate
, AATBAccredationDate: $scope.tissueBank.AATBAccredationDate
, CreditCardNumber: $scope.tissueBank.CreditCardNumber
, CreditCardType: $scope.tissueBank.CreditCardType
, ExpiryDate: $scope.tissueBank.ExpiryDate
, CardCode: $scope.tissueBank.CardCode
, CustomerProfileId: $scope.tissueBank.CustomerProfileId
, CustomerPaymentProfileIds: $scope.tissueBank.CustomerPaymentProfileIds
                //, BillingAddress: $scope.tissueBank.BillingAddress
, BillingCityId: $scope.tissueBank.BillingCityId
                //, BillingZipCode: $scope.tissueBank.BillingZipCode
                //, BillingFaxNumber: $scope.tissueBank.BillingFaxNumber
                //, BillingEmailId: $scope.tissueBank.BillingEmailId
                //, BillingContactNumber: $scope.tissueBank.BillingContactNumber
                //, BillingCity: $scope.tissueBank.BillingCity
                //, BillingState: $scope.tissueBank.BillingState
 , BillingStateId: $scope.tissueBank.BillingStateId
, State: $scope.tissueBank.State
, City: $scope.tissueBank.City
                //, TransactionId: $scope.tissueBank.TransactionId
                //, AuthTransactionId: $scope.tissueBank.AuthTransactionId
                //, AuthCode: $scope.tissueBank.AuthCode
                //, StatusId: $scope.tissueBank.StatusId
                //, TransactionCompleteDate: $scope.tissueBank.TransactionCompleteDate
            }

            //NOTE: billing City name and billing state name is going empty
            console.log(tissueBankAdd_DTO);

            var response = TissueBankService.UpdateTbDetail(tissueBankAdd_DTO);
            response.success(function (data, status, headers, config) {
                console.log(data);
                var Message = MsgService.makeMessage(data.ReturnMessage)
                message('success', 'Success!', Message);

                //get tb details
                GetTissueBankById($scope.tissueBank.TissueBankId);
            }).error(function (data, status, headers, config) {
                console.log(data.ReturnMessage);
                var Message = MsgService.makeMessage(data.ReturnMessage)
                message('error', 'Error!', Message);
            });
        }
        else {
            message('error', 'Error!', err);
        }
    };

    $scope.UpdateBillingDetail = function () {
        var err = ValidateTbBillingDetailSubmit();
        if (err == '') {
            $scope.dataLoading = true;
            var tissueBankAdd_DTO = {

                OperationType: 'UpdateBillingDetail'
               , TissueBankId: document.getElementById("TissueBankId").value
               , UserId: document.getElementById("LoggedUserId").value

                         , TissueBankName: $scope.tissueBank.TissueBankName
                , ContactPersonFirstName: $scope.tissueBank.ContactPersonFirstName
                , ContactPersonLastName: $scope.tissueBank.ContactPersonLastName
                //            , ContactPersonNumber: $scope.tissueBank.ContactPersonNumber
                //            , ContactPersonEmailId: $scope.tissueBank.ContactPersonEmailId
                //            , FaxNumber: $scope.tissueBank.FaxNumber
                //            , TissueBankEmailId: $scope.tissueBank.TissueBankEmailId
                //            , BusinessURL: $scope.tissueBank.BusinessURL
                //            , TissueBankAddress: $scope.tissueBank.TissueBankAddress
                //            , CityId: $scope.tissueBank.CityId
                //            , ZipCode: $scope.tissueBank.ZipCode
                //, CustomerServiceLandLineNumber: $scope.tissueBank.CustomerServiceLandLineNumber
                //, TaxPayerId: $scope.tissueBank.TaxPayerId
                //, TissueBankStateLicense: $scope.tissueBank.TissueBankStateLicense
                //, AATBLicenseNumber: $scope.tissueBank.AATBLicenseNumber
                //, AATBExpirationDate: $scope.tissueBank.AATBExpirationDate
                //, AATBAccredationDate: $scope.tissueBank.AATBAccredationDate
    , CreditCardNumber: $scope.tissueBank.CreditCardNumber

    , CreditCardType: $scope.tissueBank.CreditCardType
    , ExpiryDate: $scope.expiryMonth.toString() + $scope.expiryYear.toString()
    , CardCode: $scope.tissueBank.CardCode
    , CustomerProfileId: $scope.tissueBank.CustomerProfileId
    , CustomerPaymentProfileIds: $scope.tissueBank.CustomerPaymentProfileIds
                , BillingAddress: $scope.tissueBank.BillingAddress
    , BillingCityId: $scope.tissueBank.BillingCityId
                , BillingZipCode: $scope.tissueBank.BillingZipCode
                , BillingFaxNumber: $scope.tissueBank.BillingFaxNumber
                , BillingEmailId: $scope.tissueBank.BillingEmailId
                , BillingContactNumber: $scope.tissueBank.BillingContactNumber
                //, BillingCity: $scope.tissueBank.BillingCity
                //, BillingState: $scope.tissueBank.BillingState
    , BillingStateId: $scope.tissueBank.BillingStateId
                //, State: $scope.tissueBank.State
                //, City: $scope.tissueBank.City
                , TransactionId: $scope.tissueBank.TransactionId
                , AuthTransactionId: $scope.tissueBank.AuthTransactionId
                , AuthCode: $scope.tissueBank.AuthCode
                , StatusId: $scope.tissueBank.StatusId
                , TransactionCompleteDate: $scope.tissueBank.TransactionCompleteDate
            }

            //delete $scope.tissueBank.BillingCity;
            //delete $scope.tissueBank.BillingState;

            //$scope.tissueBank.OperationType = 'UpdateBillingDetail';
            //$scope.tissueBank.TissueBankId = document.getElementById("TissueBankId").value;
            //$scope.tissueBank.UserId = document.getElementById("LoggedUserId").value;
            //$scope.tissueBank.ExpiryDate = $scope.expiryMonth.toString() + $scope.expiryYear.toString();
            //NOTE: billing City name and billing state name is going empty
            console.log(tissueBankAdd_DTO);

            var response = TissueBankService.UpdateTbDetail(tissueBankAdd_DTO);
            response.success(function (data, status, headers, config) {
                console.log(data);
                var Message = MsgService.makeMessage(data.ReturnMessage)
                message('success', 'Success!', Message);

                //get tb details
                GetTissueBankById($scope.tissueBank.TissueBankId);
            }).error(function (data, status, headers, config) {
                console.log(data.ReturnMessage);
                var Message = MsgService.makeMessage(data.ReturnMessage)
                message('error', 'Error!', Message);
            }).finally(function () {
                $scope.dataLoading = false;
            });
        }
        else {
            message('error', 'Error!', err);
        }
    };

    function ValidateTbBillingDetailSubmit() {
        var err = '';
        var currentYear = new Date().getFullYear().toString().substr(2, 2)
        var expiryMonth = parseInt($scope.expiryMonth);
        var d = new Date();

        var currentMonth = d.getMonth();
        //0=January, 1=February etc.
        if ($scope.expiryYear == currentYear) {
            if (currentMonth + 1 > expiryMonth) {
                err = 'Invalid Expiry Month and Year. Select again.'
            }
        }

        return err;
    }

    GetCities = function (stateId) {
        if (stateId != null) {
            CityService.GetCities(stateId)
                .success(function (data, status, headers, config) {
                    if (data.Cities == "") {
                        var Message = MsgService.makeMessage(data.ReturnMessage)
                        message('error', 'Error!', Message);
                        //message('info', 'Not available!', 'Cities are not available for state : <b>' + state.StateName + '</b>');
                        $scope.tissueBank.Cities = null;
                    }
                    else {
                        $scope.tissueBank.Cities = data.Cities;
                    }
                }).error(function (data, status, headers, config) {
                    var Message = MsgService.makeMessage(data.ReturnMessage)
                    message('error', 'Error!', Message);
                    $scope.tissueBank.Cities = null;
                });
        }
        else {
            $scope.tissueBank.Cities = null;
        }
    }

    GetBillingCities = function (stateId) {
        if (stateId != null) {
            CityService.GetCities(stateId)
                .success(function (data, status, headers, config) {
                    if (data.Cities == "") {
                        var Message = MsgService.makeMessage(data.ReturnMessage)
                        message('error', 'Error!', Message);
                        //message('info', 'Not available!', 'Cities are not available for state : <b>' + state.StateName + '</b>');
                        $scope.tissueBank.BillingCities = null;
                    }
                    else {
                        $scope.tissueBank.BillingCities = data.Cities;
                    }
                }).error(function (data, status, headers, config) {
                    var Message = MsgService.makeMessage(data.ReturnMessage)
                    message('error', 'Error!', Message);
                    $scope.tissueBank.BillingCities = null;
                });
        }
        else {
            $scope.tissueBank.BillingCities = null;
        }
    }

    function ClearFields() {
        $scope.tissueBank.TissueBankName = "";
        $scope.tissueBank.ContactPersonFirstName = "";
        $scope.tissueBank.ContactPersonLastName = "";
        $scope.tissueBank.ContactPersonNumber = "";
        $scope.tissueBank.TissueBankEmailId = "";
        $scope.tissueBank.BusinessURL = "";
        $scope.tissueBank.TissueBankAddress = "";
        $scope.tissueBank.CityId = "";
        $scope.tissueBank.TissueBankStateLicense = "";
        $scope.tissueBank.AATBLicenseNumber = "";
        $scope.tissueBank.AATBExpirationDate = "";
        $scope.tissueBank.AATBAccredationDate = "";

        $scope.tissueBank.city = null;
        $scope.tissueBank.state = { StateId: 0 };
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

app.controller("UserProfileController", function ($scope, UserDetailService, MsgService, $window, $timeout, InputService, KeyValueService) {
    $scope.UserDetail = {};
    $scope.UserRoles = [];
    $scope.Password = '';
    $scope.dataLoading = true;

    var PreviousEmailId = $scope.UserDetail.EmailId;

    $scope.InfoId = document.getElementById("InfoId").value;
    $scope.LoggedUserId = document.getElementById("LoggedUserId").value;
    $scope.phoneNumberPattern = InputService.phoneNumberPattern;
    $scope.emailPattern = InputService.emailPattern;
    $scope.name_AlphaSpacesPattern = InputService.name_AlphaSpacesPattern;
    $scope.userNamePattern = InputService.userNamePattern;
    $scope.phoneNumberLength = InputService.phoneNumberLength;
    $scope.AnswerPattern = InputService.AnswerPattern;
    $scope.AnswerLength = InputService.AnswerLength;

    var InfoType = document.getElementById("InfoType").value;
    if (InfoType != "") {
        $scope.InfoType = InfoType;
    }

    if ($scope.LoggedUserId != "" && InfoId != "" && InfoType != "") {
        var user_DTO = new Object();

        user_DTO.RequestUserId = $scope.LoggedUserId;
        user_DTO.TissueBankId = $scope.InfoId;
        user_DTO.InfoType = InfoType;
        user_DTO.OperationType = 'GetById';

        GetUserDetail(user_DTO);
    }


    function GetUserDetail(user_DTO) {
        UserDetailService.GetUserDetail(user_DTO)
        .success(function (data, status, headers, config) {
            $scope.UserDetail = data.UserDetail[0];

            GetUserRoles(user_DTO);

            KeyValueService.getKeyValue("Question")
       .success(function (data2, status, headers, config) {

           $scope.SecurityQuestions = data2.KeyValues;
           $scope.PasswordQuestions = data2.KeyValues;


           $scope.SecurityQuestion = { Key: $scope.UserDetail.SecurityQuestion };
           $scope.PasswordQuestion = { Key: $scope.UserDetail.PasswordQuestion };


       }).error(function (data2, status, headers, config) {
           var Message = MsgService.makeMessage(data2.ReturnMessage)
           message('error', 'Error!', Message);
       });



        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        }).finally(function () {
            $scope.dataLoading = false;
        });
    };

    function GetUserRoles(user_DTO) {
        user_DTO.OperationType = 'GetUserRole';
        UserDetailService.GetUserRoles(user_DTO)
        .success(function (data, status, headers, config) {
            $scope.UserRoles = data.UserRoles;
            console.log(data.UserRoles);
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });
    };

    $scope.Submit = function () {
        var err = ValidateSubmit($scope);
        if (err == '') {
            var userMngmnt_User_CUD_DTO = {};
            userMngmnt_User_CUD_DTO.UserId = $scope.LoggedUserId;
            userMngmnt_User_CUD_DTO.UserName = $scope.UserDetail.UserName;
            userMngmnt_User_CUD_DTO.FullName = $scope.UserDetail.FullName;
            userMngmnt_User_CUD_DTO.MobileNumber = $scope.UserDetail.MobileNumber;
            userMngmnt_User_CUD_DTO.EmailId = $scope.UserDetail.EmailId;
            userMngmnt_User_CUD_DTO.LastModifiedBy = $scope.LoggedUserId;
            userMngmnt_User_CUD_DTO.InfoId = $scope.InfoId;
            if ($scope.SecurityQuestion != null)
                userMngmnt_User_CUD_DTO.SecurityQuestion = $scope.SecurityQuestion.Key;

            userMngmnt_User_CUD_DTO.SecurityAnswer = $scope.UserDetail.SecurityAnswer;

            if ($scope.SecurityQuestion != null)
                userMngmnt_User_CUD_DTO.PasswordQuestion = $scope.PasswordQuestion.Key;

            userMngmnt_User_CUD_DTO.PasswordAnswer = $scope.UserDetail.PasswordAnswer;

            userMngmnt_User_CUD_DTO.OperationType = 'UserUpdate';
            console.log(userMngmnt_User_CUD_DTO);

            var response = UserDetailService.SubmitUser(userMngmnt_User_CUD_DTO);

            response
           .success(function (data, status, headers, config) {
               var Message = MsgService.makeMessage(data.ReturnMessage)
               message('success', 'Success!', Message);
               //default values
               GetUserDetail($scope.LoggedUserId);
               // $window.location.href = '/TissueBank/User/Index?msg=success';
               console.log(data);
           })
           .error(function (data, status, headers, config) {
               var Message = MsgService.makeMessage(data.ReturnMessage)
               message('error', 'Error!', Message);
           });
        }
        else {
            message('error', 'Error!', err);
        }
    }

    function ValidateSubmit($scope) {
        var err = '';
        if ((($scope.UserDetail.SecurityAnswer != null && $scope.UserDetail.SecurityAnswer != '') && $scope.SecurityQuestion.Key != null) || (($scope.UserDetail.SecurityAnswer == null || $scope.UserDetail.SecurityAnswer == '') && $scope.SecurityQuestion.Key == null)) {

        }
        else {
            err = 'Please fill both Security Answer and Security Question';
        }

        if ((($scope.UserDetail.PasswordAnswer != null && $scope.UserDetail.PasswordAnswer != '') && $scope.PasswordQuestion.Key != null) || (($scope.UserDetail.PasswordAnswer == null || $scope.UserDetail.PasswordAnswer == '') && $scope.PasswordQuestion.Key == null)) {

        }
        else {
            if (err == '') {
                err = 'Please fill both Password Answer and Password Question';
            }
            else {
                err = err + '<br /> Please fill both Password Answer and Password Question';
            }
        }
        return err;
    }

    $scope.OpenPasswordModel = function () {
        $scope.showModal = true;

    };

    $scope.PasswordSubmit = function (IsSendMail) {
        var userMngmnt_User_CUD_DTO = {};
        userMngmnt_User_CUD_DTO.UserId = $scope.LoggedUserId;
        userMngmnt_User_CUD_DTO.LastModifiedBy = $scope.LoggedUserId;
        userMngmnt_User_CUD_DTO.Password = $scope.UserDetail.Password;
        userMngmnt_User_CUD_DTO.IsSendMail = true;
        userMngmnt_User_CUD_DTO.OperationType = 'changePass';

        console.log(userMngmnt_User_CUD_DTO);

        var response = UserDetailService.SubmitUser(userMngmnt_User_CUD_DTO);

        response
       .success(function (data, status, headers, config) {
           var Message = MsgService.makeMessage(data.ReturnMessage)
           message('success', 'Success!', Message);
           $scope.showModal = false;
       })
       .error(function (data, status, headers, config) {
           var Message = MsgService.makeMessage(data.ReturnMessage)
           message('error', 'Error!', Message);
       });

        $scope.UserDetail.Password = "";
        $scope.password_verify = "";
        $scope.IsSendMail = "";
    };

    $scope.PasswordCancel = function () {
        $scope.showModal = false;
        $scope.UserDetail.Password = "";
        $scope.password_verify = "";
        $scope.IsSendMail = "";
    };

    function message(type, title, content) {
        var notify = {
            type: type,
            title: title,
            content: content
        };
        $scope.$emit('notify', notify);
    };


});

app.filter('updateById', function () {
    return function (input, id, data) {
        var i = 0, len = input.length;
        for (; i < len; i++) {
            if (+input[i].TissueBankProductId == +id) {
                input[i].ProductDescription = data;
                return input;
            }
        }
        return null;
    }
});


//TissueBankUpdateObject
//var tissueBankAdd_DTO = {

//    OperationType: 'UpdateBillingDetail'
//         , TissueBankId: document.getElementById("TissueBankId").value
//         , UserId: document.getElementById("LoggedUserId").value

//                   , TissueBankName: $scope.tissueBank.TissueBankName
//          , ContactPersonFirstName: $scope.tissueBank.ContactPersonFirstName
//          , ContactPersonLastName: $scope.tissueBank.ContactPersonLastName
//    //            , ContactPersonNumber: $scope.tissueBank.ContactPersonNumber
//    //            , ContactPersonEmailId: $scope.tissueBank.ContactPersonEmailId
//    //            , FaxNumber: $scope.tissueBank.FaxNumber
//    //            , TissueBankEmailId: $scope.tissueBank.TissueBankEmailId
//    //            , BusinessURL: $scope.tissueBank.BusinessURL
//    //            , TissueBankAddress: $scope.tissueBank.TissueBankAddress
//    //            , CityId: $scope.tissueBank.CityId
//    //            , ZipCode: $scope.tissueBank.ZipCode
//    //, CustomerServiceLandLineNumber: $scope.tissueBank.CustomerServiceLandLineNumber
//    //, TaxPayerId: $scope.tissueBank.TaxPayerId
//    //, TissueBankStateLicense: $scope.tissueBank.TissueBankStateLicense
//    //, AATBLicenseNumber: $scope.tissueBank.AATBLicenseNumber
//    //, AATBExpirationDate: $scope.tissueBank.AATBExpirationDate
//    //, AATBAccredationDate: $scope.tissueBank.AATBAccredationDate
//, CreditCardNumber: $scope.tissueBank.CreditCardNumber

//, CreditCardType: $scope.tissueBank.CreditCardType
//, ExpiryDate: $scope.expiryMonth.toString() + $scope.expiryYear.toString()
//, CardCode: $scope.tissueBank.CardCode
//, CustomerProfileId: $scope.tissueBank.CustomerProfileId
//, CustomerPaymentProfileIds: $scope.tissueBank.CustomerPaymentProfileIds
//          , BillingAddress: $scope.tissueBank.BillingAddress
//, BillingCityId: $scope.tissueBank.BillingCityId
//          , BillingZipCode: $scope.tissueBank.BillingZipCode
//          , BillingFaxNumber: $scope.tissueBank.BillingFaxNumber
//          , BillingEmailId: $scope.tissueBank.BillingEmailId
//          , BillingContactNumber: $scope.tissueBank.BillingContactNumber
//    //, BillingCity: $scope.tissueBank.BillingCity
//    //, BillingState: $scope.tissueBank.BillingState
//, BillingStateId: $scope.tissueBank.BillingStateId
//    //, State: $scope.tissueBank.State
//    //, City: $scope.tissueBank.City
//          , TransactionId: $scope.tissueBank.TransactionId
//          , AuthTransactionId: $scope.tissueBank.AuthTransactionId
//          , AuthCode: $scope.tissueBank.AuthCode
//          , StatusId: $scope.tissueBank.StatusId
//          , TransactionCompleteDate: $scope.tissueBank.TransactionCompleteDate
//}

////delete $scope.tissueBank.BillingCity;
////delete $scope.tissueBank.BillingState;

////$scope.tissueBank.OperationType = 'UpdateBillingDetail';
////$scope.tissueBank.TissueBankId = document.getElementById("TissueBankId").value;
////$scope.tissueBank.UserId = document.getElementById("LoggedUserId").value;
////$scope.tissueBank.ExpiryDate = $scope.expiryMonth.toString() + $scope.expiryYear.toString();

