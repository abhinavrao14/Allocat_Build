app.service("HospitalService", function ($http) {

    this.AddHospital = function (hospital_DTO) {
        var response = $http({
            url: "http://localhost:63744/api/Hospital",
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
