using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VoucherAppFCC.Model
{
    public class tb_Blacktie_Voucher
    {
        public string Voucher_code { get; set; }
        public string Account_Name { get; set; }
        public string Mobile { get; set; }
        public string Status { get; set; }
        public Boolean Use_Status { get; set; }
    }
    public class SearchVoucherModel
    {

        public string name { get; set; }
        public string datefrom { get; set; }
        public string dateto { get; set; }
        public int start { get; set; }
        public int page_size { get; set; }
        public string VoucherCode { get; set; }
        public string CustMobile { get; set; }
        public string CustName { get; set; }
    }
    public class SaveoucherModel
    {

        public string CaseID { get; set; }
        public string Redeemer_Mobile { get; set; }
        public string VoucherCode { get; set; }
        public string Redeemer_Name { get; set; }
    }

}
