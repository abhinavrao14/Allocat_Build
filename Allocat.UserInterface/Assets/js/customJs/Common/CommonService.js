app.service("StateService", function ($http) {

    this.GetStates = function () {
        var response = $http({
            method: 'Get',
            url: 'http://allocat.net/Webapi/api/State'
        });
        return response;
    };
});


app.service("CityService", function ($http) {

    this.GetCities = function (StateId) {
        var response = $http({
            method: 'Get',
            url: 'http://allocat.net/Webapi/api/City',
            params: { StateId: StateId }
        });
        return response;
    };
});

app.service("HospitalTypeService", function ($http) {

    this.GetHospitalTypes = function () {
        var response = $http({
            method: 'Get',
            url: 'http://allocat.net/Webapi/api/HospitalType'
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

app.service("ProductMasterService", function ($http) {

    this.GetAllProductMaster = function (productMasterInquiryDTO) {
        var response = $http({
            method: 'GET',
            url: 'http://allocat.net/Webapi/api/ProductMasterApi/GetProductMasters/',
            params: productMasterInquiryDTO
        });
        console.log(response);
        return response;
    };

    this.getProductMasterById = function (ProductMasterId) {
        var response = $http({
            method: "get",
            url: "http://allocat.net/Webapi/api/ProductMaster/",
            params: {
                ProductMasterId: ProductMasterId
            }
        });
        return response;
    }
});

app.service("DomainScopeService", function ($http) {

    this.getAllDomainScope = function () {
        var response = $http({
            method: 'Get',
            url: 'http://allocat.net/Webapi/api/DomainScope/',
        });
        return response;
    };
});