using DTO.Client;
using Helper.CommonModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using ClientWebApi.ViewModel.Client;

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

    }
}
