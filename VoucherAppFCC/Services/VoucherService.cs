using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoucherAppFCC.Datalayer;
using VoucherAppFCC.Model;

namespace VoucherAppFCC.Services
{
    public class VoucherService
    {


        public interface IVoucher
        {
            bool IsAnExistingUser(string userName);
            bool IsValidUserCredentials(string userName, string password);
           
            Messenger GetLists(SearchVoucherModel _Search); 
            Messenger UseVoucher(string VoucherCode);
        }

        public Messenger GetLists(SearchVoucherModel _Search)
        {

            Messenger mess_ = new Messenger();
            try
            {
                List<tb_Blacktie_Voucher> userList_ = new List<tb_Blacktie_Voucher>();
                using (var session = new SessionFactory())
                {

                    string sql_ = @" exec SP_O_API_GetVoucher  @VoucherCode='"+ _Search.VoucherCode + "',@CustName ='" + _Search.CustName + "'',@@CustMobile ='" + _Search.CustMobile + "' ";
                    userList_ = session.Exec<tb_Blacktie_Voucher>(sql_).ToList();
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

        public Messenger UseVoucher(string VoucherCode)
        {
            // 

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
}
