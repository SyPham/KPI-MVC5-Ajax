using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KPI.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int LoginCount { get; set; }
    }
}