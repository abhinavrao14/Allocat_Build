using Allocat.ApplicationService;
using Allocat.DataModel;
using Allocat.DataService;
using Allocat.DataServiceInterface;
using Allocat.WebApi.WebApiModel;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Allocat.WebApi.Controllers
{
    public class HospitalController : ApiController
    {
        IHospitalDataService tbDataService;

        public HospitalController()
        {
            tbDataService = new HospitalDataService();
        }

        [HttpPost]
        public HttpResponseMessage Post(Hospital_DTO Hospital_DTO)
        {
            return Request.CreateResponse<int>(HttpStatusCode.OK, 1);
            TransactionalInformation transaction = new TransactionalInformation();

            HospitalApiModel tbApiModel = new HospitalApiModel();

            HospitalBusinessService hospitalBusinessService = new HospitalBusinessService(tbDataService);

            if (Hospital_DTO.HospitalId == 0)
            {
                hospitalBusinessService.AddHospital(Hospital_DTO.HospitalId, Hospital_DTO.HospitalName, Hospital_DTO.ContactPersonName, Hospital_DTO.ContactPersonNumber, Hospital_DTO.HospitalEmailId, Hospital_DTO.BusinessURL, Hospital_DTO.HospitalAddress, Hospital_DTO.CityId, Hospital_DTO.RegistrationNumber, Hospital_DTO.UserName, Hospital_DTO.HospitalTypeID, out transaction);
            }
            else
            {
                //orderBusinessService.UpdateHospital(Hospital, out transaction);
            }

            tbApiModel.ReturnMessage = transaction.ReturnMessage;
            tbApiModel.ReturnStatus = transaction.ReturnStatus;

            if (transaction.ReturnStatus == false)
            {
                tbApiModel.ValidationErrors = transaction.ValidationErrors;
                return Request.CreateResponse<HospitalApiModel>(HttpStatusCode.BadRequest, tbApiModel);
            }
            else
            {
                return Request.CreateResponse<HospitalApiModel>(HttpStatusCode.OK, tbApiModel);
            }
        }
    }
}
