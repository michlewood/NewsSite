using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NewsSite.Data
{
    public class ReturnModel
    {
        public IList<Claim> Claims { get; set; }
        public ApplicationUser User { get; set; }
    }
}
