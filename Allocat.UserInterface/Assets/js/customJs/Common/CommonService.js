app.service("StateService", function ($http, ResourceService) {

    this.GetStates = function () {
        var response = $http({
            method: 'Get',
            url: ResourceService.webApiRootPath + 'State'
        });
        return response;
    };
});


app.service("CityService", function ($http, ResourceService) {

    this.GetCities = function (StateId) {
        var response = $http({
            method: 'Get',
            url: ResourceService.webApiRootPath + 'City',
            params: { StateId: StateId }
        });
        return response;
    };
});

app.service("HospitalTypeService", function ($http, ResourceService) {

    this.GetHospitalTypes = function () {
        var response = $http({
            method: 'Get',
            url: ResourceService.webApiRootPath + 'HospitalType'
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

    this.getProductMasterById = function (productMasterGetByIdDTO) {
        var response = $http({
            method: "get",
            url: ResourceService.webApiRootPath + 'ProductMaster/',
            params: productMasterGetByIdDTO
        });
        return response;
    }

    this.GetAllProductMaster = function (productMasterInquiryDTO) {
        var response = $http({
            method: 'GET',
            url: ResourceService.webApiRootPath + 'ProductMasterApi/GetProductMasters/',
            params: productMasterInquiryDTO
        });
        console.log(response);
        return response;
    };
});

app.service("DomainScopeService", function ($http, ResourceService) {

    this.getAllDomainScope = function () {
        var response = $http({
            method: 'Get',
            url: ResourceService.webApiRootPath + 'DomainScope/',
        });
        return response;
    };
});

app.factory('ResourceService', function () {
    return {
        //webApiRootPath: 'http://allocat.net/Webapi/api/',
        //webApiContentRootPath: 'http://allocat.net/Webapi/'
        webApiContentRootPath: 'http://localhost:63744/',
        //rootPath: 'http://allocat.net/',
        webApiRootPath: 'http://localhost:63744/api/'
        //rootPath: 'http://localhost:63744/',
    };
});

app.factory('InputService', function () {
    return {
        zipcodeLength: 5
        , creditCardMinLength: 12
        , creditCardMaxLength: 19
        , cvvLength: 3
        , faxNumberMinLength: 5
        , faxNumberMaxLength: 20
        , tissueBankStateLicenseMaxLength: 50
        , aATBLicenseNumberMaxLength: 30
        , name_AlphaSpacesPattern: /^[A-Za-z\s]+$/
        , addressPattern: /^[A-Za-z0-9\s]+$/
        , userNamePattern: /^[A-Za-z0-9]+$/
        , emailPattern: /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/
        , phoneNumberPattern: /^\d{3}\d{3}\d{4}/
        , phoneNumberLength: 10
        , faxNumberPattern: /^\d+$/
        , ZipCodePattern: /^\d+$/
        , CustomerServiceLandLineNumberPattern: /^\d+$/
        , CreditCardNumberPattern: /^\d+$/
        , expiryLength: 4
        , AATBLicenseNumberPattern: /^[A-Za-z0-9\s]+$/
        , TissueBankStateLicensePattern: /^[A-Za-z0-9\s]+$/
        //FaxNumberPattern: '^\+[0-9]{1,3}\([0-9]{3}\)[0-9]{7}$'
    };
});
