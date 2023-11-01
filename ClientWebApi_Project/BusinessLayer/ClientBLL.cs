using DataLayer.Entities;
using DTO.Client;
using Helper.CommonModel;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BusinessLayer
{
    public class ClientBLL
    {
        public readonly ClientApiDbContext _db;
        public readonly IHostingEnvironment _hostEnvironment;
        public ClientBLL(ClientApiDbContext db, IHostingEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }
        public CommonResponse GetClient()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                List<ClientMst> lstGetClient = _db.ClientMsts.Where(u => u.IsDelete == false).ToList();

                if (lstGetClient.Count > 0)
                {
                    response.Data = lstGetClient.Adapt<List<GetClientResDTO>>();
                    response.Status = true;
                    response.Message = "client data are found";
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "client data are not found";
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                }
            }
            catch { throw; }
            return response;
        }
        public CommonResponse AddClient(AddClientReqDTO addClientReqDTO)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                AddClientResDTO addClientResDTO = new AddClientResDTO();
                ClientMst clientMst = new ClientMst();
                var clientmstlist = _db.ClientMsts.Where(x => x.IsDelete == false).ToList();
                if (clientmstlist.Where(u => u.Fullname == addClientReqDTO.Fullname).ToList().Count > 0)
                {

                    response.Message = "Fullname already exists";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
                else if (clientmstlist.Where(u => u.Username == addClientReqDTO.Username).ToList().Count > 0)
                {
                    response.Message = "username already exists";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
                else
                {
                    string filename = uploadfile(addClientReqDTO.Image);
                    clientMst.Fullname = addClientReqDTO.Fullname.Trim();
                    clientMst.Gender = addClientReqDTO.Gender.Trim();
                    clientMst.Dob = addClientReqDTO.Dob;
                    clientMst.Username = addClientReqDTO.Username.Trim();
                    clientMst.Password = addClientReqDTO.Password.Trim();
                    clientMst.Image = filename;
                    clientMst.CreatedBy = 1;
                    clientMst.IsActive = true;
                    clientMst.CreatedOn = DateTime.Now;

                    _db.ClientMsts.Add(clientMst);
                    _db.SaveChanges();

                    addClientResDTO.Id = clientMst.Id;

                    if (addClientResDTO != null)
                    {
                        response.Data = addClientResDTO;
                        response.Status = true;
                        response.Message = "client added successfully";
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                    }
                }
            }
            catch { throw; }
            return response;
        }

        public CommonResponse UpdateClient(UpdateClientReqDTO updateClientReqDTO)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                UpdateClientResDTO updateClientResDTO = new UpdateClientResDTO();

                var updateClient = _db.ClientMsts.FirstOrDefault(x => x.Id == updateClientReqDTO.Id && x.IsDelete == false);

                if (updateClient != null)
                {
                    var clientmstlist = _db.ClientMsts.Where(x => x.IsDelete == false).ToList();

                    if (clientmstlist.FirstOrDefault(u => u.Fullname == updateClientReqDTO.Fullname && u.Id != updateClientReqDTO.Id) != null)
                    {
                        response.Message = "Fullname already exists";
                        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    }
                    else if (clientmstlist.FirstOrDefault(u => u.Username == updateClientReqDTO.Username && u.Id != updateClientReqDTO.Id) != null)
                    {
                        response.Message = "username already exists";
                        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    }
                    else
                    {
                        string filename = uploadfile(updateClientReqDTO.Image);
                        updateClient.Fullname = updateClientReqDTO.Fullname.Trim();
                        updateClient.Gender = updateClientReqDTO.Gender.Trim();
                        updateClient.Dob = updateClientReqDTO.Dob;
                        updateClient.Username = updateClientReqDTO.Username.Trim();
                        updateClient.Password = updateClientReqDTO.Password.Trim();
                        updateClient.Image = filename;
                        updateClient.UpdatedOn = DateTime.Now;
                        updateClient.IsActive = true;
                        updateClient.UpdateBy = updateClient.Id;

                        _db.Entry(updateClient).State = EntityState.Modified;
                        _db.SaveChanges();

                        updateClientResDTO.Id = updateClient.Id;

                        if (updateClientResDTO != null)
                        {
                            response.Data = updateClientResDTO;
                            response.Status = true;
                            response.Message = "client updated successfully";
                            response.StatusCode = System.Net.HttpStatusCode.OK;
                        }
                    }
                }
                else
                {
                    response.Message = "requestd id not valid";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
            }
            catch { throw; }
            return response;
        }

        public CommonResponse DeleteClient(DeleteClientReqDTO deleteClientReqDTO)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                DeleteClientResDTO deleteClientResDTO = new DeleteClientResDTO();

                var deleteClient = _db.ClientMsts.FirstOrDefault(x => x.Id == deleteClientReqDTO.Id && x.IsActive == true);

                if (deleteClient != null)
                {
                    deleteClient.UpdatedOn = DateTime.Now;
                    deleteClient.IsDelete = true;
                    _db.Entry(deleteClient).State = EntityState.Modified;
                    _db.SaveChanges();

                    deleteClientResDTO.Id = deleteClient.Id;

                    if (deleteClientResDTO != null)
                    {
                        response.Data = deleteClientResDTO;
                        response.Status = true;
                        response.Message = "client deleted successfully";
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "client is not found";
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                }
            }
            catch { throw; }
            return response;
        }

        public CommonResponse Login(LoginReqDTO loginReqDTO)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                LoginResDTO loginResDTO = new LoginResDTO();
                var clientmstlist = _db.ClientMsts.Where(x => x.IsDelete == false).ToList();

                if (clientmstlist.Where(u => u.Username == loginReqDTO.Username && u.Password == loginReqDTO.Password).ToList().Count > 0)
                {
                    response.Message = "login";
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "invalid";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }

            }
            catch { throw; }
            return response;
        }


        public string uploadfile(dynamic filepath)
        {
            string path = Path.Combine(_hostEnvironment.WebRootPath, "Images");
            string fileName = filepath.FileName;
            string filePath = Path.Combine(path, fileName);
            if (fileName != null)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    filepath.CopyTo(fileStream);
                }
            }
            return filePath;
        }
    }
}