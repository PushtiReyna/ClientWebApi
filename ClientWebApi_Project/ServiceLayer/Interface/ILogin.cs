using DTO.Login;
using Helper.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface ILogin
    {
        public CommonResponse Login(LoginReqDTO loginReqDTO);

        public CommonResponse Refresh(RefreshReqDTO refreshReqDTO);
    }
}
