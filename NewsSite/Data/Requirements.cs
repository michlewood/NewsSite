using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsSite.Data
{
    public class HiddenNewsRequirement : IAuthorizationRequirement
    {
    }
    public class AgeRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get; } = 20;
    }
}
