﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace SalesAdminPortal.Helpers
{
    public static class IdentityExtensions
    {
        public static string GetName(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            if (identity is ClaimsIdentity ci)
            {
                return ci.FindFirstValue("Name");
            }
            return null;
        }

        public static string GetAgentCode(this IIdentity identity)
        {
            if(identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            if(identity is ClaimsIdentity ci)
            {
                return ci.FindFirstValue("AgentCode");
            }
            return null;
        }

        public static string GetAccountType(this IIdentity identity)
        {
            if(identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            if(identity is ClaimsIdentity ci)
            {
                return ci.FindFirstValue("IsSalesMaster");
            }
            return null;
        }

        public static string IsSuperAdmin(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            if (identity is ClaimsIdentity ci)
            {
                return ci.FindFirstValue("IsSuperAdmin");
            }
            return null;
        }
    }
}