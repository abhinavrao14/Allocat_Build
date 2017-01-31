app.service("StateService", function ($http, ResourceService) {

    this.GetStates = function () {
        var response = $http({
            method: 'Get',
            url: ResourceService.webApiRootPath+'State'
        });
        return response;
    };
});


app.service("CityService", function ($http, ResourceService) {

    this.GetCities = function (StateId) {
        var response = $http({
            method: 'Get',
            url: ResourceService.webApiRootPath+'City',
            params: { StateId: StateId }
        });
        return response;
    };
});

app.service("HospitalTypeService", function ($http, ResourceService) {

    this.GetHospitalTypes = function () {
        var response = $http({
            method: 'Get',
            url: ResourceService.webApiRootPath+'HospitalType'
        });
        return response;
    };
});


app.service("MsgService", function () {
    this.makeMessage = function (MessageArray) {
        var errStr = ''
        for (var i = 0; i < MessageArray.length; i++) {
            if (errStr == '') {
                errStr = MessageArray[i];
            }
            else {
                errStr = errStr + '<br/>' + MessageArray[i];
            }
        }
        return errStr;
    }
});

app.service("ProductMasterService", function ($http, ResourceService) {

    this.GetAllProductMaster = function (productMasterInquiryDTO) {
        var response = $http({
            method: 'GET',
            url: ResourceService.webApiRootPath+'ProductMasterApi/GetProductMasters/',
            params: productMasterInquiryDTO
        });
        console.log(response);
        return response;
    };

    this.getProductMasterById = function (ProductMasterId) {
        var response = $http({
            method: "get",
            url: ResourceService.webApiRootPath+'ProductMaster/',
            params: {
                ProductMasterId: ProductMasterId
            }
        });
        return response;
    }
});

app.service("DomainScopeService", function ($http, ResourceService) {

    this.getAllDomainScope = function () {
        var response = $http({
            method: 'Get',
            url: ResourceService.webApiRootPath+'DomainScope/',
        });
        return response;
    };
});

app.factory('ResourceService', function () {
    return {
        //webApiRootPath: 'http://allocat.net/Webapi/'
        //rootPath: 'http://allocat.net/'
        webApiRootPath: 'http://localhost:63744/api/'
        //rootPath: 'http://localhost:63744/'
    };
});
