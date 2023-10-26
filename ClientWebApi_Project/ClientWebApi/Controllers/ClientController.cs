using ClientWebApi.ViewModel;
using DTO.Client;
using Helper.CommonModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;
using Mapster;

namespace ClientWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        public readonly IClient _client;
      //  public readonly IWebHostEnvironment _webHostEnvironment;

        public ClientController(IClient client /*, IWebHostEnvironment webHostEnvironment*/)
        {
            _client = client;
          //  _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public CommonResponse AddClient(AddClientReqViewModel addClientReqViewModel) 
        { 
            CommonResponse response = new CommonResponse();
            try
            {
                //string filename = uploadfile(addClientReqViewModel);
                response = _client.AddClient(addClientReqViewModel.Adapt<AddClientReqDTO>());
                AddClientResDTO addClientResDTO = response.Data;
                response.Data = addClientResDTO.Adapt<AddClientResViewModel>();
            }
            catch { throw; }
            return response;
        }

        //[NonAction]
        //public string uploadfile(AddClientReqViewModel addClientReqViewModel)
        //{
        //    string path = Path.Combine("D:\\project\\ClientWebApi\\ClientWebApi\\ClientWebApi_Project\\ClientWebApi\\Image\\");
        //    using (Stream stream = new FileStream(path, FileMode.Create))
        //    {
        //        addClientReqViewModel.pic.CopyTo(stream);

        //    }
        //    return path;
        //}
    }
}
