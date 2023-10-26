using DataLayer.Entities;
using DTO.Client;
using Helper.CommonModel;

namespace BusinessLayer
{
    public class ClientBLL
    {
        public readonly ClientApiDbContext _db;
       

        public ClientBLL(ClientApiDbContext db)
        {
            _db = db;
           
        }
        public CommonResponse AddClient(AddClientReqDTO addClientReqDTO)
        {
            CommonResponse response = new CommonResponse();
            try
            {

                AddClientResDTO addClientResDTO = new AddClientResDTO();
                //string filename = uploadfile(addClientReqDTO);
                ClientMst clientMst = new ClientMst();
                clientMst.Fullname = addClientReqDTO.Fullname.Trim();
                clientMst.Gender = addClientReqDTO.Gender.Trim();
                clientMst.Dob = addClientReqDTO.Dob;
                clientMst.Username = addClientReqDTO.Username.Trim();
                clientMst.Password = addClientReqDTO.Password.Trim();
                clientMst.Image = addClientReqDTO.Image.Trim();
                clientMst.CreatedBy = true;
                clientMst.IsActive = true;
                clientMst.CreatedOn = DateTime.Now;
           
                _db.ClientMsts.Add(clientMst);
                _db.SaveChanges();

                addClientResDTO.Id = clientMst.Id;

                if (addClientResDTO != null)
                {
                    response.Data = addClientResDTO;
                    response.Status = true;
                    response.Message = "client is add";
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "client is not add.";
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                }


            }
            catch { throw; }
            return response;
        }
    
    //public string uploadfile(AddClientReqDTO addClientReqDTO)
    //    {
    //        string path = Path.Combine("D:\\project\\ClientWebApi\\ClientWebApi\\ClientWebApi_Project\\ClientWebApi\\Image\\");
    //        using (Stream stream = new FileStream(path, FileMode.Create))
    //        {
    //            addClientReqDTO.Image.CopyTo(stream);
               
    //        }
    //        return path;
    //    }

       
    }
}