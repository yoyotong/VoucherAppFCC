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
        Messenger GetLists(SearchModel _Search);
        Messenger Getlogin(string User_Name, string User_Password);
        Messenger ResetPassword(string username, string password, string NewPassword, string AccessToken);
        tb_Blacktie_User GetUserinfo(string User_Name);
        Messenger GetVoucherLists(SearchVoucherModel _Search);
        Messenger UseVoucher(string VoucherCode);
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
            if (string.IsNullOrEmpty(userName))
            {
                return string.Empty;
            }

            if (userName == "admin")
            {
                return UserRoles.Admin;
            }

            return UserRoles.BasicUser;
        }


        public Messenger GetLists(SearchModel _Search)
        {

            Messenger mess_ = new Messenger();
            try
            {
                List<tb_Blacktie_User> userList_ = new List<tb_Blacktie_User>();
                using (var session = new SessionFactory())
                {

                    string sql_ = @" select *  from tb_Blacktie_User (nolock) where Use_Status =1     ";
                    if (!string.IsNullOrEmpty(_Search.name))
                    {
                        _Search.name = _Search.name.Trim();
                        sql_ += " and (    Fullname like'%" + _Search.name + "%'";
                        sql_ += " or User_Name like'%" + _Search.name + "%' )";

                    }



                    userList_ = session.Exec<tb_Blacktie_User>(sql_).ToList();
                }
                mess_.ObjModel = userList_;
                mess_.Status = true;
                mess_.message = "ok";

            }
            catch (Exception ex)
            {
                mess_.Status = false;
                mess_.message = ex.Message.ToString();
            }
            return mess_;

        }


        public Messenger Getlogin(string username, string password)
        {

            Messenger mess_ = new Messenger();
            try
            {
                tb_Blacktie_User user_ = new tb_Blacktie_User();
                using (var session = new SessionFactory())
                {
                    MD5Service MD5Service_ = new MD5Service();
                    password = MD5Service_.Encrypt(password);
                    string sql_ = @" exec SP_O_API_Authentication_Vouche @username='" + username + "',@password='" + password + "'";
                    user_ = session.Exec<tb_Blacktie_User>(sql_).FirstOrDefault();
                }

                Userinfo return_ = new Userinfo();
                return_.User_ID = user_.User_ID.ToString();
                return_.User_Name = user_.User_Name.ToString();
                return_.AccessToken = user_.AccessToken.ToString();
                return_.RoleUser = GetUserRole(user_.RoleUser);
                if (user_ != null)
                {

                    mess_.ObjModel = return_;
                    mess_.Status = true;
                    mess_.message = "ok";
                }
                else
                {
                    mess_.Status = false;
                    mess_.message = "ไม่พบข้อมูล *** ";
                }

                return_ = null;
            }
            catch (Exception ex)
            {
                mess_.Status = false;
                mess_.message = ex.Message.ToString();
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
                mess_.message = "Reset Password Success";

            }
            catch (Exception ex)
            {
                mess_.Status = false;
                mess_.message = ex.Message.ToString();
            }
            return mess_;
        }


        public Messenger ResetAccessToken(string username, string AccessToken)
        {

            Messenger mess_ = new Messenger();
            try
            {


                MD5Service MD5Service_ = new MD5Service();

                using (var session = new SessionFactory())
                {
                    string sql_ = @" Update tb_Blacktie_User set  AccessToken='" + AccessToken + "' where User_Name= '" + username + "'";
                    session.Exec(sql_);
                }

                mess_.ObjModel = "";
                mess_.Status = true;
                mess_.message = "Reset AccessToken Success";

            }
            catch (Exception ex)
            {
                mess_.Status = false;
                mess_.message = ex.Message.ToString();
            }
            return mess_;
        }

        public Messenger GetVoucherLists(SearchVoucherModel _Search)
        {
            Messenger mess_ = new Messenger();
            try
            {
                List<tb_Blacktie_Voucher> userList_ = new List<tb_Blacktie_Voucher>();
                using (var session = new SessionFactory())
                {

                    string sql_ = @" exec SP_O_API_GetVoucher  @VoucherCode='" + _Search.VoucherCode + "',@CustName ='" + _Search.CustName + "',@CustMobile ='" + _Search.CustMobile + "' ";
                    userList_ = session.Exec<tb_Blacktie_Voucher>(sql_).ToList();
                }
                if (userList_.Count != 0)
                {
                    for (int i = 0; i < userList_.Count; i++)
                    {
                        userList_[i].Status = "ใช้ไปแล้ว";
                        if (userList_[i].Use_Status == false)
                        { userList_[i].Status = "ยังไม่ได้ใช้"; }
                    
                    }
                }
                mess_.ObjModel = userList_;
                mess_.Status = true;
                mess_.message = "ok";

            }
            catch (Exception ex)
            {
                mess_.Status = false;
                mess_.message = ex.Message.ToString();
            }
            return mess_;
        }

        public Messenger UseVoucher(string VoucherCode)
        {
            Messenger mess_ = new Messenger();
            try
            {
                tb_Blacktie_User user_ = new tb_Blacktie_User();
                using (var session = new SessionFactory())
                {

                    string sql_ = @" exec SP_U_API_BookingVoucher @VoucherCode='" + VoucherCode + "' ";
                    user_ = session.Exec<tb_Blacktie_User>(sql_).FirstOrDefault();
                }
                mess_.ObjModel = user_;
                mess_.Status = true;
                mess_.message = "Success";

            }
            catch (Exception ex)
            {
                mess_.Status = false;
                mess_.message = ex.Message.ToString();
            }
            return mess_;
        }

        //public Messenger GetVoucherLists(SearchVoucherModel _Search)
        //{

        //    Messenger mess_ = new Messenger();
        //    try
        //    {
        //        List<tb_Blacktie_Voucher> userList_ = new List<tb_Blacktie_Voucher>();
        //        using (var session = new SessionFactory())
        //        {

        //            string sql_ = @" exec SP_O_API_GetVoucher  @VoucherCode='" + _Search.VoucherCode + "',@CustName ='" + _Search.CustName + "'',@@CustMobile ='" + _Search.CustMobile + "' ";
        //            userList_ = session.Exec<tb_Blacktie_Voucher>(sql_).ToList();
        //        }
        //        mess_.ObjModel = userList_;
        //        mess_.Status = true;
        //        mess_.message = "ok";

        //    }
        //    catch (Exception ex)
        //    {
        //        mess_.Status = false;
        //        mess_.message = ex.Message.ToString();
        //    }
        //    return mess_;

        //}


        //public Messenger UseVoucher(string VoucherCode)
        //{
        //    // 

        //    Messenger mess_ = new Messenger();
        //    try
        //    {
        //        tb_Blacktie_User user_ = new tb_Blacktie_User();
        //        using (var session = new SessionFactory())
        //        {

        //            string sql_ = @" exec SP_U_API_BookingVoucher @VoucherCode='" + VoucherCode + "' ";
        //            user_ = session.Exec<tb_Blacktie_User>(sql_).FirstOrDefault();
        //        }
        //        mess_.ObjModel = user_;
        //        mess_.Status = true;
        //        mess_.message = "Success";

        //    }
        //    catch (Exception ex)
        //    {
        //        mess_.Status = false;
        //        mess_.message = ex.Message.ToString();
        //    }
        //    return mess_;
        //}


    }

    public static class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string BasicUser = nameof(BasicUser);
    }

}

