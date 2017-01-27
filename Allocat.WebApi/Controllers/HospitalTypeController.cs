using Allocat.ApplicationService;
using Allocat.DataModel;
using Allocat.DataService;
using Allocat.DataServiceInterface;
using Allocat.WebApi.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Allocat.WebApi.Controllers
{
    public class HospitalTypeController : ApiController
    {
        IHospitalTypeDataService hospitalTypeDataService;

        public HospitalTypeController()
        {
            hospitalTypeDataService = new HospitalTypeDataService();
        }

        public HttpResponseMessage Get()
        {
            CommonApiModel commonApiModel = new CommonApiModel();
            TransactionalInformation transaction = new TransactionalInformation();

            HospitalTypeBusinessService hospitalTypeBusinessService = new HospitalTypeBusinessService(hospitalTypeDataService);

            IEnumerable<HospitalType> HospitalTypes = hospitalTypeBusinessService.GetHospitalType
                (out transaction);
            commonApiModel.HospitalTypes = HospitalTypes;

            commonApiModel.ReturnStatus = transaction.ReturnStatus;
            commonApiModel.ReturnMessage = transaction.ReturnMessage;
            commonApiModel.IsAuthenicated = true;

            if (transaction.ReturnStatus == true)
            {
                var response = Request.CreateResponse<CommonApiModel>(HttpStatusCode.OK, commonApiModel);
                return response;
            }

            var badResponse = Request.CreateResponse<CommonApiModel>(HttpStatusCode.BadRequest, commonApiModel);
            return badResponse;
        }
    }
}
