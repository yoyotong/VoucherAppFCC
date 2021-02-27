using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoucherAppFCC.Datalayer;
using VoucherAppFCC.Infrastructure;
using VoucherAppFCC.Model;
using static VoucherAppFCC.Services.VoucherService;

namespace VoucherAppFCC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VoucherController : ControllerBase
    {

        private readonly IVoucher _Service;
        private readonly IJwtAuthManager _jwtAuthManager;

        public VoucherController(IVoucher iService, IJwtAuthManager jwtAuthManager)
        { 
            _Service = iService;
            _jwtAuthManager = jwtAuthManager;
        } 
        [HttpGet("GetLists")]
        public ActionResult GetLists(SearchVoucherModel _Search)
        {

            Messenger mess_ = new Messenger();
            mess_ = _Service.GetLists(_Search);
            return Ok(mess_);

        } 
 
        [HttpGet("UseVoucher")]
        public ActionResult UseVoucher(string VoucherCode)
        {

            Messenger mess_ = new Messenger();
            mess_ = _Service.UseVoucher(VoucherCode);
            return Ok(mess_);

        }
        


    }
}
