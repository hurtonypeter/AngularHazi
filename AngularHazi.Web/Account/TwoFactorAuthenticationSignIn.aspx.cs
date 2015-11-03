using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using AngularHazi.Web.Services.Identity;
using AngularHazi.Data.Models;

namespace AngularHazi.Web.Account
{
    public partial class TwoFactorAuthenticationSignIn : System.Web.UI.Page
    {
        public ApplicationUserManager UserManager { get; set; }

        public ApplicationSignInManager SignInManager { get; set; }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            var userId = SignInManager.GetVerifiedUserId<ApplicationUser, string>();
            if (userId == null)
            {
                Response.Redirect("/Account/Error", true);
            }
            var userFactors = UserManager.GetValidTwoFactorProviders(userId);
            Providers.DataSource = userFactors.Select(x => x).ToList();
            Providers.DataBind();            
        }

        protected void CodeSubmit_Click(object sender, EventArgs e)
        {
            bool rememberMe = false;
            bool.TryParse(Request.QueryString["RememberMe"], out rememberMe);
            
            var result = SignInManager.TwoFactorSignIn<ApplicationUser, string>(SelectedProvider.Value, Code.Text, isPersistent: rememberMe, rememberBrowser: RememberBrowser.Checked);
            switch (result)
            {
                case SignInStatus.Success:
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    break;
                case SignInStatus.LockedOut:
                    Response.Redirect("/Account/Lockout");
                    break;
                case SignInStatus.Failure:
                default:
                    FailureText.Text = "Invalid code";
                    ErrorMessage.Visible = true;
                    break;
            }
        }

        protected void ProviderSubmit_Click(object sender, EventArgs e)
        {
            if (!SignInManager.SendTwoFactorCode(Providers.SelectedValue))
            {
                Response.Redirect("/Account/Error");
            }

            var user = UserManager.FindById(SignInManager.GetVerifiedUserId<ApplicationUser, string>());
            if (user != null)
            {
                var code = UserManager.GenerateTwoFactorToken(user.Id, Providers.SelectedValue);
            }

            SelectedProvider.Value = Providers.SelectedValue;
            sendcode.Visible = false;
            verifycode.Visible = true;
        }
    }
}