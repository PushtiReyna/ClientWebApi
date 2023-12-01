using BusinessLayer;
using DTO.Login;
using Helper.CommonModel;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Implementation
{
    public class LoginImpl : ILogin
    {
        public readonly LoginBLL _loginBLL;
        public LoginImpl(LoginBLL loginBLL)
        {
            _loginBLL = loginBLL;
        }

        public CommonResponse Login(LoginReqDTO loginReqDTO)
        {
            return _loginBLL.Login(loginReqDTO);
        }

        public CommonResponse Refresh(RefreshReqDTO refreshReqDTO)
        {
            return _loginBLL.Refresh(refreshReqDTO);
        }
    }
}
