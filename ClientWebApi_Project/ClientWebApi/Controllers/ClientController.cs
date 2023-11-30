using ClientWebApi.ViewModel;
using DTO.Client;
using Helper.CommonModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;
using Mapster;
using Microsoft.AspNetCore.Authorization;

namespace ClientWebApi.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        public readonly IClient _client;
       
        public ClientController(IClient client)
        {
            _client = client;
        }


        [HttpGet]
        public CommonResponse GetClient()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = _client.GetClient();
                List<GetClientResDTO> lstGetClientResDTO = response.Data;
                response.Data = lstGetClientResDTO.Adapt<List<GetClientResViewModel>>();
              
            }
            catch { throw; }
            return response;
        }

        [HttpPost]
        [Route("AddClient")]
        public CommonResponse AddClient([FromForm] AddClientReqViewModel addClientReqViewModel)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = _client.AddClient(addClientReqViewModel.Adapt<AddClientReqDTO>());
                AddClientResDTO addClientResDTO = response.Data;
                response.Data = addClientResDTO.Adapt<AddClientResViewModel>();
            }
            catch { throw; }
            return response;
        }

        [HttpPut]
        [Route("UpdateClient")]
        public CommonResponse UpdateClient([FromForm] UpdateClientReqViewModel updateClientReqViewModel)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = _client.UpdateClient(updateClientReqViewModel.Adapt<UpdateClientReqDTO>());
                UpdateClientResDTO updateClientResDTO = response.Data;
                response.Data = updateClientResDTO.Adapt<UpdateClientResViewModel>();
            }
            catch { throw; }
            return response;
        }

        [HttpDelete]
        [Route("DeleteClient")]
        public CommonResponse DeleteClient(DeleteClientReqViewModel deleteClientReqViewModel)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = _client.DeleteClient(deleteClientReqViewModel.Adapt<DeleteClientReqDTO>());
                DeleteClientResDTO deleteClientResDTO = response.Data;
                response.Data = deleteClientResDTO.Adapt<DeleteClientResViewModel>();
            }
            catch { throw; }
            return response;
        }


        [HttpPut]
        [Route("UploadDocument")]
        public CommonResponse UploadDocument([FromForm] UploadDocumentReqViewModel uploadDocumentReqViewModel)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = _client.UploadDocument(uploadDocumentReqViewModel.Adapt<UploadDocumentReqDTO>());
                UploadDocumentResDTO uploadDocumentResDTO = response.Data;
                response.Data = uploadDocumentResDTO.Adapt<UploadDocumentResViewModel>();
            }
            catch { throw; }
            return response;
        }

        #region MyRegion
        //[HttpPost]
        //[Route("UploadDocument")]
        //public CommonResponse UploadDocument([FromForm] UploadDocumentReqViewModel uploadDocumentReqViewModel)
        //{
        //    CommonResponse response = new CommonResponse();
        //    try
        //    {
        //        response = _client.UploadDocument(uploadDocumentReqViewModel.Adapt<UploadDocumentReqDTO>());
        //        UploadDocumentResDTO uploadDocumentResDTO = response.Data;
        //        response.Data = uploadDocumentResDTO.Adapt<UploadDocumentResViewModel>();
        //    }
        //    catch { throw; }
        //    return response;
        //} 
        #endregion

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public CommonResponse Login(LoginReqViewModel loginReqViewModel)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = _client.Login(loginReqViewModel.Adapt<LoginReqDTO>());
                LoginResDTO loginResDTO = response.Data;
                response.Data = loginResDTO.Adapt<LoginResViewModel>(); 
            }
            catch { throw; }
            return response;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Refresh")]
        public CommonResponse Refresh(RefreshReqViewModel refreshReqViewModel)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = _client.Refresh(refreshReqViewModel.Adapt<RefreshReqDTO>());
                RefreshResDTO refreshResDTO = response.Data;
                response.Data = refreshResDTO.Adapt<RefreshResViewModel>();
              
            }
            catch { throw; }
            return response;
        }

        [HttpPost]
        [Route("SalaryClient")]
        public CommonResponse SalaryClient(SalaryClientReqViewModel salaryClientReqViewModel)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = _client.SalaryClient(salaryClientReqViewModel.Adapt<SalaryClientReqDTO>());
                SalaryClientResDTO salaryClientReqDTO = response.Data;
                response.Data = salaryClientReqDTO.Adapt<SalaryClientResViewModel>();
            }
            catch { throw; }
            return response;
        }

    }
}
