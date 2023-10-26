using DTO.Client;
using Helper.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IClient
    {
        public CommonResponse AddClient(AddClientReqDTO addClientReqDTO);
    }
}
