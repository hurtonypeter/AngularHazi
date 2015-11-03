using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AngularHazi.Web.Services.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace AngularHazi.Web.Account
{
    public partial class VerifyPhoneNumber : System.Web.UI.Page
    {
        public ApplicationUserManager UserManager { get; set; }

        public ApplicationSignInManager SignInManager { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            var phonenumber = Request.QueryString["PhoneNumber"];
            var code = UserManager.GenerateChangePhoneNumberToken(User.Identity.GetUserId(), phonenumber);           
            PhoneNumber.Value = phonenumber;
        }

        protected void Code_Click(object sender, EventArgs e)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid code");
                return;
            }

            var result = UserManager.ChangePhoneNumber(User.Identity.GetUserId(), PhoneNumber.Value, Code.Text);

            if (result.Succeeded)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());

                if (user != null)
                {
                    SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    Response.Redirect("/Account/Manage?m=AddPhoneNumberSuccess");
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
        }
    }
}