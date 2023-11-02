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
  

    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        public readonly IClient _client;
       
        public ClientController(IClient client)
        {
            _client = client;
        }


        [HttpGet, Authorize]
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

        [HttpPost, Authorize]
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

        [HttpPut, Authorize]
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

        [HttpDelete, Authorize]
        public CommonResponse DeleteClient([FromForm] DeleteClientReqViewModel deleteClientReqViewModel)
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

        [HttpPost]
        [Route("Login")]
        public CommonResponse Login([FromForm] LoginReqViewModel loginReqViewModel)
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
    }
}
