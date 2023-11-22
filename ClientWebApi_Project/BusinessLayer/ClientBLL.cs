using DataLayer.Entities;
using DTO.Client;
using Helper.CommonModel;
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace BusinessLayer
{
    public class ClientBLL
    {
        public readonly ClientApiDbContext _db;
        public readonly IHostingEnvironment _hostEnvironment;
        public readonly IConfiguration _configuration;
        public ClientBLL(ClientApiDbContext db, IHostingEnvironment hostEnvironment, IConfiguration configuration)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
        }
        public CommonResponse GetClient()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                List<GetClientResDTO> lstGetClientResDTO = _db.ClientMsts.Where(u => u.IsDelete == false).ToList().Adapt<List<GetClientResDTO>>();

                if (lstGetClientResDTO.Count > 0)
                {
                    response.Data = lstGetClientResDTO;
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

                var clientmst = _db.ClientMsts.FirstOrDefault(u => u.Username == loginReqDTO.Username && u.Password == loginReqDTO.Password && u.IsDelete == false);

                if (clientmst != null)
                {
                    #region
                    //var clientmstlist = _db.ClientMsts.Where(x => x.IsDelete == false).ToList();

                    //if (clientmstlist.Where(u => u.Username == loginReqDTO.Username && u.Password == loginReqDTO.Password).ToList().Count > 0)

                    //{


                    //    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@2410"));
                    //    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    //    var tokenOptions = new JwtSecurityToken(
                    //        issuer: "https://localhost:7246/",
                    //        audience: "https://localhost:7246/",
                    //        claims: new List<Claim>(),
                    //        expires: DateTime.Now.AddMinutes(5),
                    //        signingCredentials: signinCredentials
                    //    );
                    //    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    #endregion

                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                    var claims = new[]
                    {
                          new Claim(ClaimTypes.Name,loginReqDTO.Username),
                          new Claim("Password",loginReqDTO.Password)
                    };
                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.Now.AddMinutes(2),
                        signingCredentials: credentials);

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    var randomNumber = new byte[32];
                    string refreshtokenstring = null;

                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(randomNumber);
                        refreshtokenstring = Convert.ToBase64String(randomNumber);

                    }

                    clientmst.Token = tokenString;
                    clientmst.TokenExpiryTime = DateTime.Now.AddMinutes(2);
                    clientmst.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                    clientmst.RefreshToken = refreshtokenstring;
                    clientmst.UpdatedOn = DateTime.Now;

                    _db.Entry(clientmst).State = EntityState.Modified;
                    _db.SaveChanges();


                    loginResDTO.Token = tokenString;
                    loginResDTO.RefreshToken = refreshtokenstring;
                    loginResDTO.TokenExpiryTime = clientmst.TokenExpiryTime;

                    response.Data = loginResDTO;
                    response.Message = "Token get successfully";
                    response.Status = true;
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    #region 
                    //}
                    //else
                    //{
                    //    response.Message = "Request is not correct";
                    //    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    //} 
                    #endregion
                }
                else
                {
                    response.Message = "username and password is not correct";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
            }
            catch { throw; }
            return response;
        }


        public string uploadfile(dynamic filepath)
        {

            string folder = "Images/";

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

        public CommonResponse Refresh(RefreshReqDTO refreshReqDTO)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                RefreshResDTO refreshResDTO = new RefreshResDTO();

                var clientmst = _db.ClientMsts.FirstOrDefault(u => u.Token == refreshReqDTO.Token && u.RefreshToken == refreshReqDTO.RefreshToken && u.IsDelete == false);


                if (clientmst != null)
                {
                    string Token = refreshReqDTO.Token;
                    string refreshToken = refreshReqDTO.RefreshToken;
                    #region
                    var tokenHandler = new JwtSecurityTokenHandler();
                    SecurityToken securityToken;
                    var principal = tokenHandler.ValidateToken(Token, new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))

                    }, out securityToken);

                    var jwtSecurityToken = securityToken as JwtSecurityToken;

                    var username = principal.Identity.Name;

                    //var user = _db.ClientMsts.FirstOrDefault(u => u.Username == username);
                    if (username == clientmst.Username)
                    {
                        #endregion

                        //if refresh token expired
                        if (clientmst.RefreshToken != refreshToken || clientmst.RefreshTokenExpiryTime <= DateTime.Now)
                        {
                            response.Message = "refresh token is expired";
                            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        }
                        //if token is not expired
                        else if (clientmst.Token != Token || clientmst.TokenExpiryTime >= DateTime.Now)
                        {
                            response.Message = " token is not expired";
                            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        }
                        else
                        {
                            //if token expired but refreh token not expired
                            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                            var claims = new[]
                            {
                               new Claim(ClaimTypes.Name,clientmst.Username),
                               new Claim("Password",clientmst.Password)
                        };
                            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                                   _configuration["Jwt:Audience"],
                                   claims,
                                   expires: DateTime.Now.AddMinutes(2),
                                   signingCredentials: credentials);

                            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                            var newToken = new JwtSecurityTokenHandler().WriteToken(token);

                            var randomNumber = new byte[32];
                            string refreshtokenstring = null;

                            using (var rng = RandomNumberGenerator.Create())
                            {
                                rng.GetBytes(randomNumber);
                                refreshtokenstring = Convert.ToBase64String(randomNumber);

                            }
                            var newRefreshToken = refreshtokenstring;

                            #region 
                            clientmst.TokenExpiryTime = DateTime.Now.AddMinutes(2);
                            clientmst.Token = newToken;
                            clientmst.RefreshToken = newRefreshToken;
                            clientmst.UpdatedOn = DateTime.Now;

                            _db.Entry(clientmst).State = EntityState.Modified;
                            _db.SaveChanges(); 
                            #endregion

                            refreshResDTO.Token = newToken;
                            refreshResDTO.RefreshToken = newRefreshToken;

                            if (refreshResDTO != null)
                            {
                                response.Data = refreshResDTO;
                                response.Message = "Token get successfully";
                                response.Status = true;
                                response.StatusCode = System.Net.HttpStatusCode.OK;
                            }
                        }
                        #region 
                    }
                    else
                    {
                        response.Message = "user is not correct";
                        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    }
                    #endregion
                }
                else
                {
                    response.Message = "Request is not correct";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
            }
            catch { throw; }
            return response;
        }
    }
}