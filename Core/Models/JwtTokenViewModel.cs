using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    /// <summary>
    /// JwtToken视图模型
    /// </summary>
    public class JwtTokenViewModel
    {
        /// <summary>
        /// token
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public double expires_in { get; set; }
        /// <summary>
        /// token类型
        /// </summary>
        public string token_type { get; set; }
    }
}
