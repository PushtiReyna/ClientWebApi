using DTO.Client;
using Helper.CommonModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IClient
    {
        public CommonResponse GetClient();
        public CommonResponse AddClient(AddClientReqDTO addClientReqDTO);

        public CommonResponse UpdateClient(UpdateClientReqDTO updateClientReqDTO);

        public CommonResponse DeleteClient(DeleteClientReqDTO deleteClientReqDTO);

        public CommonResponse Login(LoginReqDTO loginReqDTO);

        public CommonResponse Refresh(RefreshReqDTO refreshReqDTO);
    }
}
