using System;
using System.Collections.Generic;
using System.Linq;
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
    public class VoucherController : ControllerBase
    {
        private readonly ILogger<VoucherController> _logger;
        private readonly IUserService _Service;
        private readonly IJwtAuthManager _jwtAuthManager;

        public VoucherController(ILogger<VoucherController> logger, IUserService iService, IJwtAuthManager jwtAuthManager)
        {
            _logger = logger;
            _Service = iService;
            _jwtAuthManager = jwtAuthManager;
        } 
        [HttpPost("GetLists")]
        public ActionResult GetLists(SearchVoucherModel _Search)
        {

            Messenger mess_ = new Messenger();
            mess_ = _Service.GetVoucherLists(_Search);
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
