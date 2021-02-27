using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VoucherAppFCC.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }

        public IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
        public string getvalue(string AppSetting)
        {
          return  conf.GetValue<string>(AppSetting );
        }
    }
}
