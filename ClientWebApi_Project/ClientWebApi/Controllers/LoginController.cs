using ClientWebApi.ViewModel.Login;
using DTO.Login;
using Helper.CommonModel;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;

namespace ClientWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public readonly ILogin _login;

        public LoginController(ILogin login)
        {
            _login = login;
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("Login")]
        public CommonResponse Login(LoginReqViewModel loginReqViewModel)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = _login.Login(loginReqViewModel.Adapt<LoginReqDTO>());
                LoginResDTO loginResDTO = response.Data;
                response.Data = loginResDTO.Adapt<LoginResViewModel>();
            }
            catch { throw; }
            return response;
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("Refresh")]
        public CommonResponse Refresh(RefreshReqViewModel refreshReqViewModel)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = _login.Refresh(refreshReqViewModel.Adapt<RefreshReqDTO>());
                RefreshResDTO refreshResDTO = response.Data;
                response.Data = refreshResDTO.Adapt<RefreshResViewModel>();

            }
            catch { throw; }
            return response;
        }
    }
}
