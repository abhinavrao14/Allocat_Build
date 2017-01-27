app.controller("ProductController", function ($scope, ProductService, $window, MsgService) {

    var TissueBankId = document.getElementById("TissueBankId").value;

    if (TissueBankId != "") {
        $scope.TissueBankId = TissueBankId;
    }

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

    $scope.SortOptions = ['ProductMasterName-A To Z', 'ProductMasterName-Z To A', 'ProductType-A To Z',
    'ProductType-Z To A', 'ProductCode-A To Z', 'ProductCode-Z To A', 'PreservationType-A To Z',
    'PreservationType-Z To A', 'UnitPrice-High To Low', 'UnitPrice-Low To High', ];

    $scope.SortOption = $scope.SortOptions[0];

    $scope.editMode = false;

    function getProducts(SearchBy, CurrentPage, PageSize, SortExpression, SortDirection) {
        var productList_TissueBank_DTO = new Object();

        productList_TissueBank_DTO.TissueBankId = $scope.TissueBankId;
        productList_TissueBank_DTO.CurrentPage = CurrentPage;
        productList_TissueBank_DTO.SortExpression = SortExpression;
        productList_TissueBank_DTO.SortDirection = SortDirection;
        productList_TissueBank_DTO.PageSize = PageSize;
        productList_TissueBank_DTO.SearchBy = SearchBy;

        var response = ProductService.getProducts(productList_TissueBank_DTO);

        response
        .success(function (data, status, headers, config) {
            var str = data.ReturnMessage[0];
            var arr = str.split(" ");

            $scope.TotalProducts = arr[0];

            $scope.Products = data.Products;
            $scope.TotalPage = Math.ceil($scope.TotalProducts / $scope.PageSize);
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        }).finally(function () {
            $scope.dataLoading = false;
        });;
    }

    getProducts($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);

    $scope.getPreviousPage = function () {
        $scope.CurrentPage = $scope.CurrentPage - 1;
        getProducts($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.getNextPage = function () {
        $scope.CurrentPage = $scope.CurrentPage + 1;
        getProducts($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.getLastPage = function () {
        $scope.CurrentPage = $scope.TotalPage;
        getProducts($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.getFirstPage = function () {
        $scope.CurrentPage = 1;
        getProducts($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.search = function () {
        getProducts($scope.SearchBy, 1, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
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

        getProducts($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.PageSizeChanged = function () {
        $scope.CurrentPage = 1;
        getProducts($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
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

app.controller("ProductDetailController", function ($filter, $scope, ProductDetailService, $window, ProductMasterService, MsgService) {
    $scope.editMode = false;
    $scope.addNewMode = false;
    $scope.IsAvailableOptions = ['Yes', 'No'];
    $scope.ProductCodeExp = /^\s*\w*\s*$/;
    $scope.dataLoading = true;

    $scope.TissueBankId = document.getElementById("TissueBankId").value;
    $scope.CreatedBy = document.getElementById("UserId").value;
    $scope.LastModifiedBy = document.getElementById("UserId").value;

    var ProductMasterId = document.getElementById("ProductMasterId").value;
    if (ProductMasterId != "") {
        GetTissueBankProductsByProductMasterId($scope.TissueBankId, ProductMasterId);
    }

    ProductMasterService.getProductMasterById(ProductMasterId)
        .success(function (data, status, headers, config) {
            $scope.ProductMaster_TissueBank = data.ProductMaster_TissueBank;
        })
        .error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });

    ProductDetailService.GetPreservationTypes()
        .success(function (data, status, headers, config) {
            $scope.PreservationTypeOptions = data.PreservationTypes;
        })
        .error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });

    ProductDetailService.GetProductTypes(ProductMasterId)
        .success(function (data, status, headers, config) {
            $scope.ProductTypeOptions = data.ProductTypes;
        })
        .error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });

    ProductDetailService.GetProductSizes(ProductMasterId)
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


    function GetTissueBankProductsByProductMasterId(TissueBankId, ProductMasterId) {

        var response = ProductDetailService.GetTissueBankProductsByProductMasterId(TissueBankId, ProductMasterId);

        response.success(function (data, status, headers, config) {
            $scope.ProductsByProductMasterId = data.ProductsByProductMasterId;
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        }).finally(function () {
            $scope.dataLoading = false;
        });;;
    }

    $scope.save = function () {
        var errStr = '';
        var Products = $scope.ProductsByProductMasterId;

        //  if ($scope.form_ConfirmOnExit.$dirty == true && $scope.form_ConfirmOnExit.$pristine == false) {

        //-------change LastModifiedBy of every row
        for (var i = 0; i < Products.length; i++) {

            Products[i].LastModifiedBy = $scope.LastModifiedBy;

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

        if (errStr == '') {
            var response = ProductDetailService.saveTissueBankProducts(Products);

            response
           .success(function (data, status, headers, config) {
               var Message = MsgService.makeMessage(data.ReturnMessage)
               message('success', 'Success!', Message);

               $scope.editMode = false;
               $scope.form_ConfirmOnExit.$dirty = false;

               //-------get data again from database
               GetTissueBankProductsByProductMasterId($scope.TissueBankId, ProductMasterId);
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

        ////-------get data again from database
        //GetTissueBankProductsByProductMasterId($scope.TissueBankId, ProductMasterId);
        //console.log("not dirty");
        //  }
    }

    $scope.addNew = function () {
        $scope.addNewMode = true;
        $scope.form_ConfirmOnExit.$dirty = true;
        $scope.ProductsByProductMasterId.push({
            'TissueBankProductId': "0",
            'TissueBankId': $scope.TissueBankId,
            'ProductMasterId': ProductMasterId,
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
                GetTissueBankProductsByProductMasterId($scope.TissueBankId, ProductMasterId);
            }
        }
        else {
            $scope.editMode = false;
        }
    };

    $scope.CheckTypeCombination = function (ProductType, ProductSize, index) {
        for (var i = 0; i < $scope.ProductsByProductMasterId.length; ++i) {
            if ($scope.ProductsByProductMasterId[i].ProductType == ProductType && index != i) {
                if ($scope.ProductsByProductMasterId[i].ProductSize == ProductSize) {
                    $scope.ProductsByProductMasterId[index].ProductType = null;
                    message('error', 'Error!', ' Type:<b>' + ProductType + '</b> is already present for Size:<b>' + ProductSize + '</b>!');
                    break;
                }
            }
        }
    };

    $scope.CheckSizeCombination = function (ProductType, ProductSize, index) {
        for (var i = 0; i < $scope.ProductsByProductMasterId.length; ++i) {
            if ($scope.ProductsByProductMasterId[i].ProductSize == ProductSize && index != i) {
                if ($scope.ProductsByProductMasterId[i].ProductType == ProductType) {
                    $scope.ProductsByProductMasterId[index].ProductSize = null;
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
    //    //$scope.ProductsByProductMasterId.splice(length - 1, 1);
    //    var products = $filter('filter')($scope.ProductsByProductMasterId, 'new');
    //    console.log(products);
    //    //console.log($scope.ProductsByProductMasterId);

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


    ////-------- description pop up start
    //$scope.Cancel = function () {
    //    ngDialog.close();
    //}

    //$scope.Submit = function () {
    //    ngDialog.close();
    //    //console.log($scope.ngDialogData.selectedDescription + "," + $scope.ngDialogData.selectedTissueBankProductId);
    //    //console.log($scope.ProductsByProductMasterId[0].ProductDescription);

    //    var arr = $filter('updateById')($scope.ProductsByProductMasterId, $scope.ngDialogData.selectedTissueBankProductId, $scope.ngDialogData.selectedDescription);
    //    $scope.ProductsByProductMasterId = arr;
    //    console.log($scope.ProductsByProductMasterId);

    //};

    //$scope.openDescriptionPopUp = function (ProductDescription, TissueBankProductId) {
    //    var new_dialog = ngDialog.open({ id: 'fromAService', template: 'firstDialogId', controller: 'ProductDetailController', data: { selectedDescription: ProductDescription, selectedTissueBankProductId: TissueBankProductId } });

    //    //console.log(ProductDescription + "," + TissueBankProductId);
    //};
    ////------- description pop up end


    ////------- remove product start
    //$scope.checkAll = function () {
    //    if (!$scope.selectedAll) {
    //        $scope.selectedAll = true;
    //    } else {
    //        $scope.selectedAll = false;
    //    }
    //    angular.forEach($scope.ProductsByProductMasterId, function (ProductsByProductMasterId) {
    //        ProductsByProductMasterId.selected = $scope.selectedAll;
    //    });
    //};

    //$scope.remove = function () {
    //    var newDataList = [];
    //    $scope.selectedAll = false;
    //    angular.forEach($scope.ProductsByProductMasterId, function (selected) {
    //        if (!selected.selected) {
    //            newDataList.push(selected);
    //        }
    //    });
    //    $scope.ProductsByProductMasterId = newDataList;
    //};
    ////------- remove product end

});

app.controller("RFQController", function ($scope, RFQService, MsgService, $window) {

    var TissueBankId = document.getElementById("TissueBankId").value;
    if (TissueBankId != "") {
        $scope.TissueBankId = TissueBankId;
    }
    $scope.SortDirection = '';
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

        RFQService.getRFQs(rfq_TissueBank_DTO)
        .success(function (data, status, headers, config) {

            var str = data.ReturnMessage[0];
            var arr = str.split(" ");

            $scope.TotalRFQs = arr[0];
            $scope.RequestForQuotes = data.RequestForQuotes;
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

    $scope.search = function () {
        GetRFQs($scope.SearchBy, 1, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
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

        GetRFQs($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.PageSizeChanged = function () {
        $scope.CurrentPage = 1;
        GetRFQs($scope.SearchBy, $scope.CurrentPage, $scope.PageSize, $scope.SortExpression, $scope.SortDirection);
    };

    $scope.GetRequestDetail = function (RequestForQuoteId) {
        GetRequestDetailByRequestForQuoteId(RequestForQuoteId);
        GetRequestResponseByRequestForQuoteId(RequestForQuoteId);
    };

    GetRequestDetailByRequestForQuoteId = function (RequestForQuoteId) {

        RFQService.GetRfqDetailByRequestForQuoteId(RequestForQuoteId)
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

app.controller("RFQDetailController", function ($scope, RFQService, MsgService, $window, $timeout, appInfo) {
    $scope.CreatedBy = document.getElementById("UserId").value;
    $scope.LastModifiedBy = document.getElementById("UserId").value;

    appInfo.setInfo({ message: "No file selected." });
    appInfo.setInfo({ busy: false });

    var TissueBankId = document.getElementById("TissueBankId").value;
    if (TissueBankId != "") {
        $scope.TissueBankId = TissueBankId;
    }

    var RequestForQuoteId = document.getElementById("RequestForQuoteId").value;
    if (RequestForQuoteId != "") {
        $scope.RequestForQuoteId = RequestForQuoteId;

        GetRequestDetailByRequestForQuoteId(RequestForQuoteId);
        GetRequestResponseByRequestForQuoteId(RequestForQuoteId);
    }

    var editMode = document.getElementById("editMode").value;
    if (editMode != "") {
        $scope.editMode = editMode;
    }

    function GetRequestDetailByRequestForQuoteId(RequestForQuoteId) {
        RFQService.GetRfqDetailByRequestForQuoteId(RequestForQuoteId)
        .success(function (data, status, headers, config) {
            $scope.RFQDetail = data.RequestForQuoteDetail[0];
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });
    };

    $scope.CalculateTotal = function () {
        if ($scope.RFQDetail.UnitPrice != null && $scope.RFQDetail.UnitPrice != '') {
            $scope.RFQDetail.LineTotal = $scope.RFQDetail.Quantity * $scope.RFQDetail.UnitPrice;
        }
        else {
            $scope.RFQDetail.LineTotal = 0;
        }
        $scope.RFQDetail.Total = $scope.RFQDetail.SalesTax + $scope.RFQDetail.LineTotal;
    };

    $scope.trixChange = function (e, editor) {
        var document = editor.getDocument()
        $scope.ResponseBody = document.toString();
    }

    $scope.submitResponse = function () {
        if ($window.UploadedAttachmentName != null && $window.UploadedAttachmentName != '') {
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

        if ($window.UploadedAttachmentName == '') {
            $window.UploadedAttachmentName == null;
        }

        if ($scope.form_ConfirmOnExit.$dirty == true || $window.UploadedAttachmentName != '') {
            if ($window.confirm("You have some unsaved changes.Do you really want to cancel?")) {

                $scope.form_ConfirmOnExit.$dirty = false;

                //get data again from db
                GetRequestDetailByRequestForQuoteId(RequestForQuoteId);
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

        if ($window.UploadedAttachmentName == '') {
            $window.UploadedAttachmentName == null;
        }
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

           //default values
           $scope.ResponseBody = '';
           GetRequestDetailByRequestForQuoteId(RequestForQuoteId);
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

app.controller("OrderController", function ($scope, OrderService, MsgService, $window, $sce) {

    var TissueBankId = document.getElementById("TissueBankId").value;
    if (TissueBankId != "") {
        $scope.TissueBankId = TissueBankId;
    }
    $scope.SortDirection = '';
    $scope.SortExpression = '';
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

app.controller("OrderDetailController", function ($scope, OrderService, MsgService, $window, $timeout) {
    $scope.CreatedBy = document.getElementById("UserId").value;
    $scope.LastModifiedBy = document.getElementById("UserId").value;
    $scope.ShippingMethod = '';
    var TissueBankId = document.getElementById("TissueBankId").value;
    if (TissueBankId != "") {
        $scope.TissueBankId = TissueBankId;
    }

    var OrderId = document.getElementById("OrderId").value;
    if (OrderId != "") {
        $scope.OrderId = OrderId;

        GetOrderDetailByOrderId(OrderId);
    }

    function GetOrderDetailByOrderId(OrderId) {
        OrderService.GetOrderDetailByOrderId(OrderId)
        .success(function (data, status, headers, config) {
            $scope.OrderDetail = data.OrderDetail[0];
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
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

    $scope.declineOrder = function () {
        Order_Ack_Decline(5);
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
           GetOrderDetailByOrderId(OrderId);
       })
       .error(function (data, status, headers, config) {
           var Message = MsgService.makeMessage(data.ReturnMessage)
           message('error', 'Error!', Message);
       });
        $scope.OrderDetail.DeclineRemark = '';
    }
});

app.controller("UserController", function ($scope, UserService, MsgService, $window, $sce) {

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

    $scope.PageSizes = [10, 20, 50, 100];
    $scope.PageSize = $scope.PageSizes[0];

    function GetUsers(SearchBy, CurrentPage, PageSize, SortExpression, SortDirection) {
        var user_DTO = new Object();

        user_DTO.TissueBankId = $scope.TissueBankId;
        user_DTO.CurrentPage = CurrentPage;
        user_DTO.SortExpression = SortExpression;
        user_DTO.SortDirection = SortDirection;
        user_DTO.PageSize = PageSize;
        user_DTO.SearchBy = SearchBy;

        UserService.GetUsers(user_DTO)
        .success(function (data, status, headers, config) {

            var str = data.ReturnMessage[0];
            var arr = str.split(" ");
            console.log(data.Users);
            $scope.TotalUsers = arr[0];
            $scope.Users = data.Users;
            $scope.TotalPage = Math.ceil($scope.TotalUsers / $scope.PageSize);

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

    $scope.search = function () {
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

app.controller("UserDetailController", function ($scope, UserDetailService, MsgService, $window, $timeout) {
    $scope.S_UserId = "";
    $scope.UserDetail = {};
    $scope.UserRoles = [];
    $scope.Operation = 'Add User';
    $scope.Password = '';

    $scope.validateFullName = /^[A-Za-z\s]+$/;
    $scope.validateNumber = /^\d{3}\d{3}\d{4}/;
    $scope.validateEmail = /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/

    $scope.InfoId = document.getElementById("InfoId").value;
    $scope.CreatedBy = document.getElementById("LoggedUserId").value;
    $scope.LastModifiedBy = document.getElementById("LoggedUserId").value;

    GetTissueBankRoles();

    var S_UserId = document.getElementById("S_UserId").value;

    if (S_UserId != "") {
        $scope.S_UserId = S_UserId;
        GetUserDetail($scope.S_UserId);
        $scope.Operation = 'Manage User';
    }

    function GetUserDetail(UserId) {
        UserDetailService.GetUserDetail(UserId, "getUserDetail")
        .success(function (data, status, headers, config) {
            $scope.UserDetail = data.UserDetail[0];

            console.log(data.UserDetail[0]);
            GetUserRoles(UserId);
        }).error(function (data, status, headers, config) {
            var Message = MsgService.makeMessage(data.ReturnMessage)
            message('error', 'Error!', Message);
        });
    };

    function GetUserRoles(UserId) {
        UserDetailService.GetUserRoles(UserId, "getUserRole")
        .success(function (data, status, headers, config) {
            $scope.UserRoles = data.UserRoles;
            console.log(data.UserRoles);
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
               message('success', 'Success!', Message);
               //default values
               GetUserDetail($scope.S_UserId);
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

    $scope.PasswordSubmit = function () {
        var userMngmnt_User_CUD_DTO = {};
        userMngmnt_User_CUD_DTO.UserId = $scope.S_UserId;
        userMngmnt_User_CUD_DTO.LastModifiedBy = $scope.LastModifiedBy;
        userMngmnt_User_CUD_DTO.Password = $scope.UserDetail.Password;
        userMngmnt_User_CUD_DTO.OperationType = 'changePass';

        //console.log(userMngmnt_User_CUD_DTO);

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
    };

    $scope.PasswordCancel = function () {
        $scope.showModal = false;
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

app.controller("TissueBankProfileController", function ($scope, TissueBankService, StateService, CityService, $window, MsgService) {
    $scope.tissueBank = {};
    $scope.tissueBank.States = [];
    $scope.tissueBank.BillingStates = [];
    $scope.tissueBank.TissueBankId = document.getElementById("TissueBankId").value;
    $scope.tissueBank.LoggedUserId = document.getElementById("LoggedUserId").value;

    //angular validation
    $scope.phoneNumber = /^\d{3}\d{3}\d{4}/;
    $scope.validateEmail = /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/
    $scope.tbNameValidate = /^[A-Za-z\s]+$/;
    $scope.personName = /^[A-Za-z\s]+$/;
    $scope.userName = /^\S{3,}$/;
    $scope.dateOptions = {
        'starting-day': 1
    };
    $scope.url = /(http(s)?:\\)?([\w-]+\.)+[\w-]+[.com|.in|.org]+(\[\?%&=]*)?/;

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

    $scope.GetCities = function (stateId) {
        GetCities(stateId);
    };

    $scope.GetBillingCities = function (stateId) {
        GetBillingCities(stateId);
    };

    $scope.AddTb = function () {
        if ($window.confirm("We will charge $25 as Registration-Fees. Are you agreed with this?")) {
            var err = '';
            err = ValidateTbSubmit();

            if (ValidateTbSubmit() == '') {
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
                    ExpiryDate: $scope.tissueBank.ExpiryDate,
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
                    console.log(data);
                    //var Message = MsgService.makeMessage(data.ReturnMessage)
                    //message('success', 'Success!', Message);
                    //  ClearFields();
                }).error(function (data, status, headers, config) {
                    console.log(data.ReturnMessage);
                    var Message = MsgService.makeMessage(data.ReturnMessage)
                    message('error', 'Error!', Message);
                });
            }
            else {
                message('error', 'Error!', err);
            }
        }
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
            $scope.tissueBank.OperationType = 'UpdateTissueBankDetail';
            $scope.tissueBank.TissueBankId = document.getElementById("TissueBankId").value;
            $scope.tissueBank.UserId = document.getElementById("LoggedUserId").value;

            //NOTE: billing City name and billing state name is going empty
            console.log($scope.tissueBank);

            var response = TissueBankService.UpdateTbDetail($scope.tissueBank);
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
        $scope.tissueBank.OperationType = 'UpdateBillingDetail';
        $scope.tissueBank.TissueBankId = document.getElementById("TissueBankId").value;
        $scope.tissueBank.UserId = document.getElementById("LoggedUserId").value;

        //NOTE: billing City name and billing state name is going empty
        console.log($scope.tissueBank);

        var response = TissueBankService.UpdateTbDetail($scope.tissueBank);
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
    };

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

