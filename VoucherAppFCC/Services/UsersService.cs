using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using VoucherAppFCC.Controllers;
using VoucherAppFCC.Datalayer;
using VoucherAppFCC.Model;

namespace VoucherAppFCC.Services
{
    public interface IUserService
    {
        bool IsAnExistingUser(string userName);
        bool IsValidUserCredentials(string userName, string password);
        string GetUserRole(string userName);
        Messenger GetLists(tb_Blacktie_User obj);
        Messenger Getlogin(string User_Name, string User_Password);
        Messenger ResetPassword(string username, string password, string NewPassword, string AccessToken);
        tb_Blacktie_User GetUserinfo(string User_Name);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;


        private readonly IDictionary<string, string> _users = new Dictionary<string, string>
        {
            { "admin", "admin" } 
             
        };
        // inject your database here for user validation
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public bool IsValidUserCredentials(string userName, string password)
        {
            _logger.LogInformation($"Validating user [{userName}]");
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            return _users.TryGetValue(userName, out var p) && p == password;
        }

        public bool IsAnExistingUser(string userName)
        {
            return _users.ContainsKey(userName);
        }

        public string GetUserRole(string userName)
        {
            if (!IsAnExistingUser(userName))
            {
                return string.Empty;
            }

            if (userName == "admin")
            {
                return UserRoles.Admin;
            }

            return UserRoles.BasicUser;
        }


        public Messenger GetLists(tb_Blacktie_User obj)
        {

            Messenger mess_ = new Messenger();
            try
            {
                List<tb_Blacktie_User> userList_ = new List<tb_Blacktie_User>();
                using (var session = new SessionFactory())
                {

                    string sql_ = @" select *  from tb_Blacktie_User (nolock)  ";
                    userList_ = session.Exec<tb_Blacktie_User>(sql_).ToList();
                }
                mess_.ObjModel = userList_;
                mess_.Status = true;
                mess_.Data = "ok";

            }
            catch (Exception ex)
            {
                mess_.Status = false;
                mess_.Data = ex.Message.ToString();
            }
            return mess_;

        }


        public Messenger Getlogin(string User_Name, string User_Password)
        {

            Messenger mess_ = new Messenger();
            try
            {
                tb_Blacktie_User user_ = new tb_Blacktie_User();
                using (var session = new SessionFactory())
                {

                    string sql_ = @" exec SP_O_API_Authentication_Vouche @username='" + User_Name + "',@password='" + User_Password + "'";
                    user_ = session.Exec<tb_Blacktie_User>(sql_).FirstOrDefault();
                }
                mess_.ObjModel = user_;
                mess_.Status = true;
                mess_.Data = "ok";

            }
            catch (Exception ex)
            {
                mess_.Status = false;
                mess_.Data = ex.Message.ToString();
            }
            return mess_;
        }


        public tb_Blacktie_User GetUserinfo(string User_Name)
        {
            tb_Blacktie_User user_ = new tb_Blacktie_User();
            try
            {
                using (var session = new SessionFactory())
                {

                    string sql_ = @" select *  from tb_Blacktie_User (nolock) where User_Name='" + User_Name + "' ";
                    user_ = session.Exec<tb_Blacktie_User>(sql_).FirstOrDefault();
                }
                return user_;
            }
            catch (Exception ex)
            {
                return user_;
            }


        }


        public Messenger ResetPassword(string username, string password, string NewPassword, string AccessToken)
        {

            Messenger mess_ = new Messenger();
            try
            {


                MD5Service MD5Service_ = new MD5Service();
                NewPassword = MD5Service_.Encrypt(NewPassword);


                using (var session = new SessionFactory())
                {
                    string sql_ = @" Update tb_Blacktie_User set  User_Password='" + NewPassword + "',AccessToken='" + AccessToken + "' where User_Name= '" + username + "'";
                   session.Exec(sql_);
                }

                mess_.ObjModel = "";
                mess_.Status = true;
                mess_.Data = "ok";

            }
            catch (Exception ex)
            {
                mess_.Status = false;
                mess_.Data = ex.Message.ToString();
            }
            return mess_;
        }


    }

    public static class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string BasicUser = nameof(BasicUser);
    }
}
