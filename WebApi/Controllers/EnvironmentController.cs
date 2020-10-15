using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace WebApi.Controllers
{
    /// <summary>
    /// 环境
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class EnvironmentController : ApiControllerBase
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }
        public EnvironmentController(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }
        /// <summary>
        /// 获取环境
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            return "环境："+Env.EnvironmentName + "; Appsetting值：" + Configuration["Name"]+"; 托管进程："+ Process.GetCurrentProcess().ProcessName; ;
        }
    }
}
