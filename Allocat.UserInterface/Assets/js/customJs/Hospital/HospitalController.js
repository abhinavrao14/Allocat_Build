app.controller("HospitalController", function ($scope, HospitalService, HospitalTypeService, StateService, CityService, $window, MsgService) {

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

app.controller("ProductHospitalController", function ($scope, Product_HospitalService, ProductMasterService, $window, MsgService, ResourceService, $timeout, $q, $log, $http) {

    var HospitalId = document.getElementById("InfoId").value;

    if (HospitalId != "") {
        $scope.HospitalId = HospitalId;
    }

    var InfoType = document.getElementById("InfoType").value;
    if (InfoType != "") {
        $scope.InfoType = InfoType;
    }

    $scope.webApiRootPath = ResourceService.webApiRootPath;
    //$scope.SortDirection = "ASC";
    //$scope.SortExpression = "ProductMasterName";
    // $scope.CurrentPage = 1;
    //  $scope.SearchBy = "";
    //$scope.descriptionLimit = 15;
    $scope.dataLoading = true;
    //$scope.TotalProducts = 0;
    //$scope.PageSizes = [10, 20, 50, 100];
    //$scope.PageSize = $scope.PageSizes[0];
    //$scope.TotalPage = 0;

    //new
    $scope.simulateQuery = true;
    $scope.AllProductMasters = loadAllProducts($http);
    $scope.querySearch = querySearch;
    $scope.selectedItemChange = selectedItemChange;
    $scope.searchTextChange = searchTextChange;
    function querySearch(query) {
        var results = query ? $scope.AllProductMasters.filter(createFilterFor(query)) : $scope.AllProductMasters, deferred;
        if ($scope.simulateQuery) {
            deferred = $q.defer();
            $timeout(function () { deferred.resolve(results); }, Math.random() * 1000, false);
            return deferred.promise;
        } else {
            return results;
        }
    }

    function loadAllProducts() {
        var product_Hospital_DTO = new Object();

        product_Hospital_DTO.OperationType = 'GetAll';

        var AllProductMasters = [];
        var url = '';
        var result = [];
        url = ResourceService.webApiRootPath + "ProductHospital";
        $http({
            method: 'GET',
            url: url,
            params: product_Hospital_DTO
        }).then(function successCallback(response) {
            console.log(response.data.AllProductMasters);
            AllProductMasters = response.data.AllProductMasters;
            $scope.dataLoading = false;
            angular.forEach(AllProductMasters, function (product, key) {
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
            $scope.SelectedProductMaster = item.display;
            $scope.SearchBy = item.value;

            GetProductMaster(item.value);
            GetProductVariations($scope.SearchBy);
            GetProductSubstitutes($scope.SearchBy);
        }
        else {
            $scope.SearchBy = '';

        }
        //$scope.CurrentPage = 1;
        //$scope.PageSize = $scope.PageSizes[0];
    }
    //new end


    function GetProductMaster(ProductMasterName) {

        var ProductMasterGetByIdDTO = {
            Name: ProductMasterName,
            OperationType: "GetByProductMasterName"
        };

        ProductMasterService.Get(ProductMasterGetByIdDTO)
        .success(function (data, status, headers, config) {
            $scope.ProductMasterDetail = data.ProductMaster_Hospital;
        })
        .error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });

    };

    //function GetProductVariations(SearchBy, CurrentPage, PageSize, SortExpression, SortDirection) {
    function GetProductVariations(SearchBy) {
        var product_Hospital_DTO = new Object();

        //productList_TissueBank_DTO.TissueBankId = $scope.TissueBankId;
        //productList_TissueBank_DTO.CurrentPage = CurrentPage;
        //productList_TissueBank_DTO.SortExpression = SortExpression;
        //productList_TissueBank_DTO.SortDirection = SortDirection;
        //productList_TissueBank_DTO.PageSize = PageSize;
        //productList_TissueBank_DTO.SearchBy = SearchBy;
        product_Hospital_DTO.OperationType = 'GetProductVariations';
        product_Hospital_DTO.ProductMasterName = SearchBy;

        var response = Product_HospitalService.Get(product_Hospital_DTO);

        response
        .success(function (data, status, headers, config) {
            var str = data.ReturnMessage[0];
            var arr = str.split(" ");
            $scope.TotalProductVariations = arr[0];

            $scope.ProductVariations = data.ProductVariations;
            console.log(data.ProductVariations);
            console.log(arr[0]);
            $scope.TotalPage = Math.ceil($scope.TotalProductVariations / $scope.PageSize);
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        }).finally(function () {
            $scope.dataLoading = false;
        });
    };

    function GetProductSubstitutes(SearchBy) {
        var product_Hospital_DTO = new Object();

        //productList_TissueBank_DTO.TissueBankId = $scope.TissueBankId;
        //productList_TissueBank_DTO.CurrentPage = CurrentPage;
        //productList_TissueBank_DTO.SortExpression = SortExpression;
        //productList_TissueBank_DTO.SortDirection = SortDirection;
        //productList_TissueBank_DTO.PageSize = PageSize;
        //productList_TissueBank_DTO.SearchBy = SearchBy;
        product_Hospital_DTO.OperationType = 'GetProductSubstitutes';
        product_Hospital_DTO.ProductMasterName = SearchBy;

        var response = Product_HospitalService.Get(product_Hospital_DTO);

        response
        .success(function (data, status, headers, config) {
            var str = data.ReturnMessage[0];
            var arr = str.split(" ");
            $scope.TotalProductSubstitutes = arr[0];

            $scope.ProductSubstitutes = data.ProductSubstitutes;
            console.log('ProductSubstitutes' + data.ProductSubstitutes);
            console.log(arr[0]);

            //$scope.TotalPage = Math.ceil($scope.TotalProductVariations / $scope.PageSize);
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        }).finally(function () {
            $scope.dataLoading = false;
        });
    };




    //$scope.GetTbProduct = function (TissueBankProductMasterId) {
    //    if (TissueBankProductMasterId != "" && TissueBankId != "" && InfoType != "") {
    //        var productList_TissueBank_DTO = new Object();
    //        productList_TissueBank_DTO.TissueBankProductMasterId = TissueBankProductMasterId;
    //        productList_TissueBank_DTO.TissueBankId = TissueBankId;
    //        productList_TissueBank_DTO.InfoType = InfoType;
    //        productList_TissueBank_DTO.OperationType = 'GetById';
    //        var response = ProductService.GetTbProduct(productList_TissueBank_DTO);
    //        response
    //        .success(function (data, status, headers, config) {
    //            var str = data.ReturnMessage[0];
    //            var arr = str.split(" ");
    //            $scope.TotalTbProducts = arr[0];
    //            console.log(data.TbProducts);
    //            $scope.TbProducts = data.TbProducts;
    //        }).error(function (data, status, headers, config) {
    //            var Message = MsgService.makeMessage(data.ReturnMessage)
    //            message('error', 'Error!', Message);
    //        }).finally(function () {
    //            $scope.dataLoading = false;
    //        });
    //    }
    //};

    // getTbProductMasters($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    //$scope.getPreviousPage = function () {
    //    $scope.CurrentPage = $scope.CurrentPage - 1;
    //    getTbProductMasters($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    //};

    //$scope.getNextPage = function () {
    //    $scope.CurrentPage = $scope.CurrentPage + 1;
    //    getTbProductMasters($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    //};

    //$scope.getLastPage = function () {
    //    $scope.CurrentPage = $scope.TotalPage;
    //    getTbProductMasters($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    //};

    //$scope.getFirstPage = function () {
    //    $scope.CurrentPage = 1;
    //    getTbProductMasters($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    //};

    //$scope.search = function (SearchBy) {
    //    $scope.SearchBy = SearchBy;
    //    getTbProductMasters($scope.SearchBy, 1, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    //    $scope.CurrentPage = 1;
    //    $scope.PageSize = $scope.PageSizes[0];
    //};

    //$scope.SortOptionChanged = function () {
    //    var str = $scope.SortOption;
    //    var arr = str.split("-");

    //    $scope.CurrentPage = 1;
    //    $scope.SortExpression = arr[0];

    //    if (arr[1] == 'Z To A') {
    //        $scope.SortDirection = "DESC"
    //    }
    //    else if (arr[1] == 'A To Z') {
    //        $scope.SortDirection = "ASC"
    //    }
    //    else if (arr[1] == 'Low To High') {
    //        $scope.SortDirection = "ASC"
    //    }
    //    else if (arr[1] == 'High To Low') {
    //        $scope.SortDirection = "DESC"
    //    }

    //    getTbProductMasters($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    //};

    //$scope.PageSizeChanged = function () {
    //    $scope.CurrentPage = 1;
    //    getTbProductMasters($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    //};

    function message(type, title, content) {
        var notify = {
            type: type,
            title: title,
            content: content
        };
        $scope.$emit('notify', notify);
    }
});

app.controller("ProductManageHospitalController", function ($scope, Product_HospitalService, ProductMasterService, $window, MsgService, ResourceService, $timeout, $q, $log, $http) {

    $scope.HospitalId = document.getElementById("InfoId").value;
    $scope.ProductMasterName = document.getElementById("ProductMasterName").value;
    $scope.ProductType = document.getElementById("ProductType").value;
    $scope.ProductSize = document.getElementById("ProductSize").value;
    $scope.PreservationType = document.getElementById("PreservationType").value;
    $scope.SourceName = document.getElementById("SourceName").value;
    $scope.InfoType = document.getElementById("InfoType").value;
    var SelectedTissueBankProductIds = [];
    $scope.webApiRootPath = ResourceService.webApiRootPath;
    $scope.dataLoading = true;

    GetTbOffering();
    GetProductMaster($scope.ProductMasterName);

    function GetProductMaster(ProductMasterName) {

        var ProductMasterGetByIdDTO = {
            Name: ProductMasterName,
            OperationType: "GetByProductMasterName"
        };

        ProductMasterService.Get(ProductMasterGetByIdDTO)
        .success(function (data, status, headers, config) {
            $scope.ProductMasterDetail = data.ProductMaster_Hospital;
        })
        .error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });

    };


    function GetTbOffering() {
        var product_Hospital_DTO = new Object();

        product_Hospital_DTO.OperationType = 'GetTbOffering';
        product_Hospital_DTO.ProductMasterName = $scope.ProductMasterName;
        product_Hospital_DTO.ProductType = $scope.ProductType;
        product_Hospital_DTO.ProductSize = $scope.ProductSize;
        product_Hospital_DTO.PreservationType = $scope.PreservationType;
        product_Hospital_DTO.SourceName = $scope.SourceName;

        var response = Product_HospitalService.Get(product_Hospital_DTO);

        response
        .success(function (data, status, headers, config) {
            var str = data.ReturnMessage[0];
            var arr = str.split(" ");
            $scope.TotalTbOffering = arr[0];

            $scope.TbOfferings = data.TbOfferings;
            console.log(data.TbOfferings);
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        }).finally(function () {
            $scope.dataLoading = false;
        });

    };

    $scope.TbOfferingChecked = function (TbOffering) {
        var found = false;
        for (var i = 0; i < SelectedTissueBankProductIds.length; i++) {
            if (SelectedTissueBankProductIds[i] == TbOffering.TissueBankProductId) {
                found = true;
                break;
            }
        }
        if (found == true) {
            SelectedTissueBankProductIds.splice(i, 1);
        }
        else {
            SelectedTissueBankProductIds.push(TbOffering.TissueBankProductId);
        }
        console.log(SelectedTissueBankProductIds)
    };

    $scope.SendRFQ = function () {
        $window.location.href = '/Hospital/RequestForQuote/Create?TBPIds=' + SelectedTissueBankProductIds;
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


app.controller("RFQCreateHospitalController", function ($scope, Product_HospitalService, ProductMasterService, $window, MsgService, ResourceService, $timeout, $q, $log, $http) {
        var SelectedTissueBankProductIds = [];
        var count = document.getElementById("count").value;
        $scope.TissueBankProductIds=[];
        for (var i=1;i<=count;++i)
        {
            var value = document.getElementById("TissueBankProductId+" + i).value;
            $scope.TissueBankProductIds.push(value);
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