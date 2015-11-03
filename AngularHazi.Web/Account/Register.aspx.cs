using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using AngularHazi.Web.Services.Identity;
using AngularHazi.Data.Models;

namespace AngularHazi.Web.Account
{
    public partial class Register : Page
    {
        public ApplicationUserManager UserManager { get; set; }

        public ApplicationSignInManager SignInManager { get; set; }

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            IdentityResult result = UserManager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                //string code = manager.GenerateEmailConfirmationToken(user.Id);
                //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                SignInManager.SignIn( user, isPersistent: false, rememberBrowser: false);
                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else 
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }
    }
}