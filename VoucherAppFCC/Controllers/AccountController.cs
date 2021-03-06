﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VoucherAppFCC.Datalayer;
using VoucherAppFCC.Infrastructure;
using VoucherAppFCC.Model;
using VoucherAppFCC.Services;

namespace VoucherAppFCC.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public AccountController(ILogger<AccountController> logger, IUserService userService, IJwtAuthManager jwtAuthManager)
        {
            _logger = logger;
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
        }
        [AllowAnonymous]
        [HttpPost("GetLogin")]
        public ActionResult GetLogin(string username, string password)
        {
          
            Messenger mess_ = new Messenger();
        
            mess_ = _userService.Getlogin(username, password);

            if (mess_.Status == true)

            {
                tb_Blacktie_User _User = new tb_Blacktie_User();
                _User = _userService.GetUserinfo(username);
                var role = _userService.GetUserRole(_User.RoleUser);
                var claims = new[]
                {
                new Claim(ClaimTypes.Name,username),
                new Claim(ClaimTypes.Role, role)
            };

                JwtAuthResult _jwtResult = new JwtAuthResult(); 
                _jwtResult = _jwtAuthManager.GenerateTokens(username, claims, DateTime.Now);
                _User.AccessToken = _jwtResult.AccessToken;
                _User.RoleUser = role;
                mess_.ObjModel = _User; 
                _User = null;
            }
            else
            {
                mess_.Status = false;
                mess_.message = " ชื่อ-รหัสเข้าใช้งาน: " + username + " ไม่ถูกต้อง ";

            }
           
            return Ok(mess_ );

        }
        [AllowAnonymous]
        [HttpGet("ResetPassword")]
        public ActionResult ResetPassword(string username, string password,  string newpassword)
        {

            Messenger mess_ = new Messenger(); 
            tb_Blacktie_User _User = new tb_Blacktie_User();
            _User = _userService.GetUserinfo(username);
            if (_User != null)
            {
                var role = _userService.GetUserRole(_User.RoleUser);
                var claims = new[]
                {
                new Claim(ClaimTypes.Name,username),
                new Claim(ClaimTypes.Role, role)
            };
                JwtAuthResult _jwtResult = new JwtAuthResult();
                _jwtResult = _jwtAuthManager.GenerateTokens(username, claims, DateTime.Now);

                mess_ = _userService.ResetPassword(username, password, newpassword, _jwtResult.AccessToken);
                _jwtResult = null;
            }
            else
            {
                mess_.Status = false;
                mess_.message = " ชื่อ-รหัส เข้าใช้งาน: " + username + " ไม่ถูกต้อง ";

            }
            return Ok(mess_);

        } 
        [HttpPost("GetLists")]
        public ActionResult GetLists(SearchModel _Search)
        {

            Messenger mess_ = new Messenger(); 
                mess_ = _userService.GetLists(_Search); 
            return Ok(mess_);

        }


        

    }
}
