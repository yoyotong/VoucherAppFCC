using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VoucherAppFCC.Model
{
    public class tb_Blacktie_User
    {

        public int User_ID { get; set; }
        public string User_Name { get; set; }
        public string User_Password { get; set; }
        public string Fullname { get; set; }
        public string Counter { get; set; }
        public string Company { get; set; }
        public Boolean Use_Status { get; set; }
        public Boolean User_LogIn { get; set; }
        public DateTime Date_LogIn { get; set; }
        public string Time_LogIn { get; set; }
        public DateTime Date_Logout { get; set; }
        public string Time_Logout { get; set; }
        public Boolean is_dispatcher { get; set; }
        public Boolean is_payment_management { get; set; }
        public string AccessToken { get; set; }
        public string RoleUser { get; set; }

    }

    public class SearchModel
    {

        public string name { get; set; }
        public string datefrom { get; set; }
        public string dateto { get; set; }
        public int start { get; set; }
        public int page_size { get; set; }
    }

    public class Userinfo
    {
        public string User_ID { get; set; }
        public string User_Name { get; set; }
        public string AccessToken { get; set; }
        public string RoleUser { get; set; }
    }
}
