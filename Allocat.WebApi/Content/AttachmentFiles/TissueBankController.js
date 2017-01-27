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

    $scope.PageSizes = [10, 20, 50, 100];
    $scope.PageSize = $scope.PageSizes[0];

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
            $scope.Products = data.Products;
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
    //$scope.showModal = true;\
    $scope.dataLoading = true;

    $scope.TissueBankId = document.getElementById("TissueBankId").value;
    //$scope.TissueBankId = 28;
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

    $scope.getNextPage = function () {
        $scope.CurrentPage = $scope.CurrentPage + 1;
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
            $scope.RFQDetail.Total = $scope.RFQDetail.SalesTax + $scope.RFQDetail.LineTotal;
        }
        else {
            $scope.RFQDetail.LineTotal = 0;
            $scope.RFQDetail.Total = 0;
        }
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

    $scope.Cancel=function()
    {
        
        $scope.showModal = false;
    }

    $scope.ConfirmSubmit = function () {

        $window.UploadedAttachmentName = null;
        $scope.showModal = false;
        appInfo.setInfo({ message: "No file selected." });
        appInfo.setInfo({ busy: false });

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

    $scope.editCancel = function () {
        if ($scope.form_ConfirmOnExit.$dirty == true) {
            if ($window.confirm("You have some unsaved changes.Do you really want to cancel?")) {
                $scope.form_ConfirmOnExit.$dirty = false;
                //get data again from db
                GetRequestDetailByRequestForQuoteId(RequestForQuoteId);
                $scope.ResponseBody = '';
                $scope.editMode = !$scope.editMode;
            }
        }
        else {
            $scope.editMode = !$scope.editMode;
        }
    };

    $scope.declineRFQ = function () {
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

            $scope.TotalOrders = arr[0];
            $scope.Orders = data.Orders;

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