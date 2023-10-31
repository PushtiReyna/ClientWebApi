using DataLayer.Entities;
using DTO.Client;
using Helper.CommonModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BusinessLayer
{
    public class ClientBLL
    {
        public readonly ClientApiDbContext _db;

        public ClientBLL(ClientApiDbContext db)
        {
            _db = db;

        }
        public CommonResponse GetClient()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                List<GetClientResDTO> lstGetClientResDTO = new List<GetClientResDTO>();
                GetClientResDTO getClientResDTO = new GetClientResDTO();
                var ClientList = _db.ClientMsts.Where(u => u.IsDelete == false).ToList();

                foreach (var clientMst in ClientList)
                {
                    getClientResDTO = new GetClientResDTO();
                    getClientResDTO.Id = clientMst.Id;
                    getClientResDTO.Fullname = clientMst.Fullname;
                    getClientResDTO.Gender = clientMst.Gender;
                    getClientResDTO.Dob = clientMst.Dob;
                    getClientResDTO.Image = clientMst.Image;
                    getClientResDTO.Username = clientMst.Username;

                    lstGetClientResDTO.Add(getClientResDTO);
                }
                if (lstGetClientResDTO.Count > 0)
                {
                    response.Data = lstGetClientResDTO;
                    response.Status = true;
                    response.Message = " list is found.";
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    response.Message = " list is not found";
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

                if (addClientReqDTO.Fullname.Length > 0)
                {
                    if (_db.ClientMsts.Where(u => u.Fullname == addClientReqDTO.Fullname && u.Id != addClientReqDTO.Id || u.Username == addClientReqDTO.Username && u.Id != addClientReqDTO.Id).Any())
                    {
                        response.Message = "Fullname or username already exists";
                        response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    }
                    else
                    {
                        clientMst.Fullname = addClientReqDTO.Fullname.Trim();
                        clientMst.Gender = addClientReqDTO.Gender.Trim();
                        clientMst.Dob = addClientReqDTO.Dob;
                        clientMst.Username = addClientReqDTO.Username.Trim();
                        clientMst.Password = addClientReqDTO.Password.Trim();
                        clientMst.Image = addClientReqDTO.Image.FileName;
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
                            response.Message = "client added successfully";
                            response.StatusCode = System.Net.HttpStatusCode.OK;
                        }
                    }
                }
                else
                {
                    response.Message = "full name can not be null";
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
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

               var clientMst = _db.ClientMsts.FirstOrDefault(x => x.Id == updateClientReqDTO.Id && x.IsDelete == false);

                if (clientMst != null)
                {
                    if (updateClientReqDTO.Fullname.Length > 0)
                    {
                        if (_db.ClientMsts.Where(u => u.Fullname == updateClientReqDTO.Fullname && u.Id != updateClientReqDTO.Id || u.Username == updateClientReqDTO.Username && u.Id != updateClientReqDTO.Id).Any())
                        {
                            response.Message = "Fullname or username already exists";
                            response.StatusCode = System.Net.HttpStatusCode.NotFound;
                        }
                        else
                        {
                            clientMst.Fullname = updateClientReqDTO.Fullname.Trim();
                            clientMst.Gender = updateClientReqDTO.Gender.Trim();
                            clientMst.Dob = updateClientReqDTO.Dob;
                            clientMst.Username = updateClientReqDTO.Username.Trim();
                            clientMst.Password = updateClientReqDTO.Password.Trim();
                            clientMst.Image = updateClientReqDTO.Image.FileName;
                            clientMst.UpdatedOn = DateTime.Now;
                            clientMst.IsActive = true;
                            clientMst.UpdateBy = true;

                            _db.Entry(clientMst).State = EntityState.Modified;
                            _db.SaveChanges();

                            updateClientResDTO.Id = clientMst.Id;

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
                        response.Message = "full name can not be null";
                        response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    }
                }
                else
                {
                    response.Message = "Id is not valid";
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
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
                ClientMst clientMst = new ClientMst();

                clientMst = _db.ClientMsts.FirstOrDefault(x => x.Id == deleteClientReqDTO.Id && x.IsDelete == false);

                if (clientMst != null)
                {
                    clientMst.Id = deleteClientReqDTO.Id;
                    clientMst.IsActive = false;
                    clientMst.IsDelete = true;
                    _db.Remove(clientMst).State = EntityState.Modified;
                    _db.SaveChanges();

                    deleteClientResDTO.Id = clientMst.Id;

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
    }
}