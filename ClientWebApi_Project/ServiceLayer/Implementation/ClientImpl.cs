using BusinessLayer;
using DTO.Client;
using Helper.CommonModel;
using Microsoft.AspNetCore.Http;
using ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Implementation
{
    public  class ClientImpl : IClient
    {
        public readonly ClientBLL _clientBLL;
        public ClientImpl(ClientBLL clientBLL)
        {
            _clientBLL = clientBLL;
        }
        public CommonResponse GetClient()
        {
            return _clientBLL.GetClient();
        }
        public CommonResponse AddClient(AddClientReqDTO addClientReqDTO)
        {
            return _clientBLL.AddClient(addClientReqDTO);
        }

        public CommonResponse UpdateClient(UpdateClientReqDTO updateClientReqDTO)
        {
            return _clientBLL.UpdateClient(updateClientReqDTO);
        }
        public CommonResponse DeleteClient(DeleteClientReqDTO deleteClientReqDTO)
        {
            return _clientBLL.DeleteClient(deleteClientReqDTO);
        }
    }
}
