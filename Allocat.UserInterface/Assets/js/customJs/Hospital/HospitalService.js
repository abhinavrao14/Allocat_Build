app.service("HospitalService", function ($http) {

    this.AddHospital = function (hospital_DTO) {
        var response = $http({
            url: ResourceService.webApiRootPath+'Hospital',
            dataType: 'json',
            method: 'POST',
            data: hospital_DTO,
            headers: {
                "Content-Type": "application/json"
            }
        });
        return response;
    };
});

app.service("Product_HospitalService", function ($http, ResourceService) {

    this.Get = function (product_Hospital_DTO) {
        var response = $http({
            method: "Get",
            url: ResourceService.webApiRootPath + "ProductHospital",
            params: product_Hospital_DTO
        });
        return response;
    };
});
