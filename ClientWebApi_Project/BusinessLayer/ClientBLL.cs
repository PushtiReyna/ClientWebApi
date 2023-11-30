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
                if (clientmstlist.Where(u => u.Fullname == addClientReqDTO.Fullname.Trim()).ToList().Count > 0)
                {
                    response.Message = "Fullname already exists";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
                else if (clientmstlist.Where(u => u.Username == addClientReqDTO.Username.Trim()).ToList().Count > 0)
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
                    clientMst.Ctc = addClientReqDTO.Ctc;
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

                    if (clientmstlist.FirstOrDefault(u => u.Fullname == updateClientReqDTO.Fullname.Trim() && u.Id != updateClientReqDTO.Id) != null)
                    {
                        response.Message = "Fullname already exists";
                        response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    }
                    else if (clientmstlist.FirstOrDefault(u => u.Username == updateClientReqDTO.Username.Trim() && u.Id != updateClientReqDTO.Id) != null)
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

                var deleteClient = _db.ClientMsts.FirstOrDefault(x => x.Id == deleteClientReqDTO.Id && x.IsDelete == false);

                if (deleteClient != null)
                {
                    deleteClient.UpdatedOn = DateTime.Now;
                    deleteClient.IsDelete = true;
                    deleteClient.UpdateBy = 1;
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


        public CommonResponse UploadDocument(UploadDocumentReqDTO uploadDocumentReqDTO)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                UploadDocumentResDTO uploadDocumentResDTO = new UploadDocumentResDTO();

                var updateClient = _db.ClientMsts.FirstOrDefault(x => x.Token == uploadDocumentReqDTO.Token && x.IsDelete == false);

                if (updateClient != null)
                {
                    string filename = uploadDocument(uploadDocumentReqDTO.Document);

                    updateClient.DocumentName = uploadDocumentReqDTO.DocumentName;
                    updateClient.Document = filename;
                    updateClient.UpdatedOn = DateTime.Now;
                    updateClient.IsActive = true;
                    updateClient.UpdateBy = 1;

                    _db.Entry(updateClient).State = EntityState.Modified;
                    _db.SaveChanges();

                    uploadDocumentResDTO.Id = updateClient.Id;

                    if (uploadDocumentResDTO != null)
                    {
                        response.Data = uploadDocumentResDTO;
                        response.Status = true;
                        response.Message = "document updated successfully";
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "requested Token not valid";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
            }
            catch { throw; }
            return response;
        }

        #region post Method
        ////post Method
        //public CommonResponse UploadDocument(UploadDocumentReqDTO uploadDocumentReqDTO)
        //{
        //    CommonResponse response = new CommonResponse();
        //    try
        //    {
        //        UploadDocumentResDTO uploadDocumentResDTO = new UploadDocumentResDTO();

        //        var updateClient = _db.ClientMsts.FirstOrDefault(x => x.Token == uploadDocumentReqDTO.Token && x.IsDelete == false);

        //        if (updateClient != null)
        //        {
        //            var documentDetail = _db.DocumentMsts.FirstOrDefault(x => x.Id == updateClient.Id);
        //            DocumentMst documentMst = new DocumentMst();
        //            if (documentDetail == null)
        //            {
        //                string filename = uploadDocument(uploadDocumentReqDTO.Document);

        //                documentMst.DocumentName = uploadDocumentReqDTO.DocumentName;
        //                documentMst.Document = filename;
        //                documentMst.IsActive = true;
        //                documentMst.Id = updateClient.Id;
        //                documentMst.CreatedBy = true;
        //                documentMst.CreatedOn = DateTime.Now;

        //                _db.DocumentMsts.Add(documentMst);
        //                _db.SaveChanges();

        //                uploadDocumentResDTO.Id = updateClient.Id;

        //                if (uploadDocumentResDTO != null)
        //                {
        //                    response.Data = uploadDocumentResDTO;
        //                    response.Status = true;
        //                    response.Message = "document updated successfully";
        //                    response.StatusCode = System.Net.HttpStatusCode.OK;
        //                }
        //            }
        //            else
        //            {
        //                response.Message = "User already upload document";
        //                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        //            }
        //        }
        //        else
        //        {
        //            response.Message = "requested Token not valid";
        //            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        //        }
        //    }
        //    catch { throw; }
        //    return response;
        //} 
        #endregion


        public string uploadDocument(dynamic documentPath)
        {
            string path = Path.Combine(_hostEnvironment.WebRootPath, "Document");
            string fileName = documentPath.FileName;
            string filePath = Path.Combine(path, fileName);
            string relativePath = Path.Combine("Documents", fileName);

            if (fileName != null)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    documentPath.CopyTo(fileStream);
                }
            }
            return relativePath;
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
                        expires: DateTime.Now.AddMinutes(30),
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
                    clientmst.TokenExpiryTime = DateTime.Now.AddMinutes(30);
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
                        ValidateLifetime = false,
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
                                   expires: DateTime.Now.AddMinutes(30),
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
                            clientmst.TokenExpiryTime = DateTime.Now.AddMinutes(30);
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

        public CommonResponse SalaryClient(SalaryClientReqDTO salaryClientReqDTO)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                SalaryClientResDTO salaryClientResDTO = new SalaryClientResDTO();

                var clientDetail = _db.ClientMsts.FirstOrDefault(x => x.Id == salaryClientReqDTO.Id && x.IsDelete == false);

                if (clientDetail != null)
                {
                    SalaryMst salaryMst = new SalaryMst();
                    salaryMst.Id = salaryClientReqDTO.Id;
                    // salaryMst.Month = salaryClientReqDTO.Month.Month;
                    // salaryMst.Year = salaryClientReqDTO.Month.Year;

                    salaryMst.Month = salaryClientReqDTO.Month;
                    salaryMst.Year = salaryClientReqDTO.Year;

                    salaryMst.FixedBasicSalary = (decimal)(clientDetail.Ctc) * ((decimal)(40.0 / 100.0));

                    salaryMst.FixedHra = salaryMst.FixedBasicSalary * ((decimal)0.4);

                    salaryMst.FixedConveyanceAllowance = 1600;

                    salaryMst.FixedMedicalAllowance = 1250;

                    salaryMst.AdditionalHraAllowance = 2400;

                    salaryMst.TotalDays = 29;

                    salaryMst.PayableDays = salaryMst.TotalDays;

                    var PayableDays_TotalDays = salaryMst.PayableDays / salaryMst.TotalDays;

                    salaryMst.GrossSalaryPayable = ((decimal)(clientDetail.Ctc) * PayableDays_TotalDays);

                    salaryMst.Basic = (salaryMst.FixedBasicSalary * PayableDays_TotalDays);

                    var GrossSalaryPayable_Basic = (salaryMst.GrossSalaryPayable - salaryMst.Basic);

                    var FixedHra_PayableDays_TotalDays = (salaryMst.FixedHra * PayableDays_TotalDays);

                    var basic = ((decimal)0.4) * salaryMst.Basic;

                    // HouseRentAllowance = IF((O7-P7)<0.4*P7,O7-P7,G7*N7/K7)
                    if (GrossSalaryPayable_Basic < basic)
                    {
                        salaryMst.HouseRentAllowance = GrossSalaryPayable_Basic;
                    }
                    else
                    {
                        salaryMst.HouseRentAllowance = FixedHra_PayableDays_TotalDays;
                    }
                    //Pf = IF((F7)<15001,P7*12%,0)
                    if (salaryMst.FixedBasicSalary < 15001)
                    {
                        salaryMst.PfOne = salaryMst.Basic * ((decimal)0.12);
                    }
                    else
                    {
                        salaryMst.PfOne = 0;
                    }

                    salaryMst.PfTwo = salaryMst.PfOne;

                    salaryMst.EmployerContPf = salaryMst.PfTwo;

                    var HouseRentAllowance_EmployerContPf = salaryMst.HouseRentAllowance - salaryMst.EmployerContPf;

                    //ConveyanceAllowance = IF((O7-P7-Q7-R7)<1600*N7/K7,(O7-P7-Q7-R7),H7*N7/K7)
                    if ((GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf) < 1600 * PayableDays_TotalDays)
                    {
                        salaryMst.ConveyanceAllowance = GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf;
                    }
                    else
                    {
                        salaryMst.ConveyanceAllowance = salaryMst.FixedConveyanceAllowance * PayableDays_TotalDays;
                    }

                    // MedicalAllowance = IF((O7-P7-Q7-R7-S7)<1250*N7/K7,O7-P7-Q7-R7-S7,I7*N7/K7)
                    if ((GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf - salaryMst.ConveyanceAllowance) < 1250 * PayableDays_TotalDays)
                    {
                        salaryMst.MedicalAllowance = GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf - salaryMst.ConveyanceAllowance;
                    }
                    else
                    {
                        salaryMst.MedicalAllowance = salaryMst.FixedMedicalAllowance * PayableDays_TotalDays; ;
                    }

                    // Additional HRA Allowance =  IF((O7 - P7 - Q7 - R7 - S7 - T7) < 2400 * N7 / K7, O7 - P7 - Q7 - R7 - S7 - T7, J7 * N7 / K7)
                    if ((GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf - salaryMst.ConveyanceAllowance - salaryMst.MedicalAllowance) < 2400 * PayableDays_TotalDays)
                    {
                        salaryMst.SalaryAdditionalHraAllowance = GrossSalaryPayable_Basic - HouseRentAllowance_EmployerContPf - salaryMst.ConveyanceAllowance - salaryMst.MedicalAllowance;
                    }
                    else
                    {
                        salaryMst.SalaryAdditionalHraAllowance = salaryMst.AdditionalHraAllowance * PayableDays_TotalDays;
                    }

                    //FlexibleAllowance=O7-SUM(P7:U7) //O7 - (P7 + Q7 + R7 + S7 + T7 + U7)
                    salaryMst.FlexibleAllowance = salaryMst.GrossSalaryPayable - (salaryMst.Basic + salaryMst.HouseRentAllowance + salaryMst.EmployerContPf + salaryMst.ConveyanceAllowance + salaryMst.MedicalAllowance + salaryMst.SalaryAdditionalHraAllowance);

                    salaryMst.Incentives = 0;


                    // Total = SUM(P7:V7)+W7 // P7 + Q7 + R7 + S7 + T7 + U7 + V7 + W7
                    salaryMst.Total = (decimal)(salaryMst.Basic + salaryMst.HouseRentAllowance + salaryMst.EmployerContPf + salaryMst.ConveyanceAllowance + salaryMst.MedicalAllowance + salaryMst.SalaryAdditionalHraAllowance + salaryMst.FlexibleAllowance + salaryMst.Incentives);

                    salaryMst.Esic = 0;

                    //Pt = =IF(X7<6000,0,IF(X7<9000,80,IF(X7<12000,150,IF(X7>=12000,200))))
                    if (salaryMst.Total < 6000)
                    {
                        salaryMst.Pt = 0;
                    }
                    else if (salaryMst.Total < 9000)
                    {
                        salaryMst.Pt = 80;
                    }
                    else if (salaryMst.Total < 12000)
                    {
                        salaryMst.Pt = 150;
                    }
                    else
                    {
                        salaryMst.Pt = 200;
                    }

                    salaryMst.Advances = 0;
                    salaryMst.IncomeTax = 0;

                    //=SUM(Y7:AE7) Y7 + Z7 + AA7 + AB7 + AC7 + AD7 + AE7

                    salaryMst.TotalDed = salaryMst.PfOne + salaryMst.PfTwo + salaryMst.Pt;

                    // ActualNetPayable X7-AF7
                    salaryMst.ActualNetPayable = salaryMst.Total - salaryMst.TotalDed;

                    salaryMst.IsActive = true;
                    salaryMst.CreatedBy = 1;
                    salaryMst.CreatedOn = DateTime.Now;

                    _db.SalaryMsts.Add(salaryMst);
                    _db.SaveChanges();

                    salaryClientResDTO.Salaryid = salaryMst.Salaryid;

                    response.Data = salaryClientResDTO;
                    response.Status = true;
                    response.Message = "Data added successfully";
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "client id is not valid";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
            }
            catch { throw; }
            return response;
        }
    }
}