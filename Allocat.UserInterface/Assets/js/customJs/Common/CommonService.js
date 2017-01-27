app.service("StateService", function ($http) {

    this.GetStates = function () {
        var response = $http({
            method: 'Get',
            url: 'http://localhost:63744/api/State'
        });
        return response;
    };
});


app.service("CityService", function ($http) {

    this.GetCities = function (StateId) {
        var response = $http({
            method: 'Get',
            url: 'http://localhost:63744/api/City',
            params: { StateId: StateId }
        });
        return response;
    };
});

app.service("HospitalTypeService", function ($http) {

    this.GetHospitalTypes = function () {
        var response = $http({
            method: 'Get',
            url: 'http://localhost:63744/api/HospitalType'
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