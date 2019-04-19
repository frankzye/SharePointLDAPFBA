using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.IdentityModel.Tokens;
using Microsoft.SharePoint.IdentityModel;
using Microsoft.SharePoint.IdentityModel.Pages;
using System.Security.Principal;
using Microsoft.IdentityModel.WindowsTokenService;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Web;
using Microsoft.IdentityModel.Claims;
using Microsoft.SharePoint.Administration.Claims;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Web;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace YR.Auth
{
    public partial class logon : IdentityModelSignInPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.RequestContext.HttpContext.User != null
                && Request.RequestContext.HttpContext.User.Identity != null
                && !Request.RequestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var userName = Request.QueryString["user"] ?? "jackey";

                var token = SPSecurityContext.SecurityTokenForFormsAuthentication(new Uri(SPContext.Current.Web.Url),
                        "FBAMember", "FBARole", userName, FBAMember.Md5Hash(userName), SPFormsAuthenticationOption.None);

                SPFederationAuthenticationModule fam = SPFederationAuthenticationModule.Current;

                fam.SetPrincipalAndWriteSessionToken(token);

                RedirectToSuccessUrl();
            }
            else
            {
                RedirectToSuccessUrl();
            }
        }
    }
}
