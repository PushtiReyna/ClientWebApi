using DataLayer.Entities;
using DTO.Login;
using Helper.CommonModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class LoginBLL
    {
        public readonly ClientApiDbContext _db;
        public readonly IConfiguration _configuration;
        public LoginBLL(ClientApiDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
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

    }
}
