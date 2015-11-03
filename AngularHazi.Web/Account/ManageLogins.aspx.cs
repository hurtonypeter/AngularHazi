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
    public partial class ManageLogins : System.Web.UI.Page
    {
        public ApplicationUserManager UserManager { get; set; }

        public ApplicationSignInManager SignInManager { get; set; }

        protected string SuccessMessage
        {
            get;
            private set;
        }
        protected bool CanRemoveExternalLogins
        {
            get;
            private set;
        }

        private bool HasPassword()
        {
            return UserManager.HasPassword(User.Identity.GetUserId());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CanRemoveExternalLogins = UserManager.GetLogins(User.Identity.GetUserId()).Count() > 1;

            SuccessMessage = String.Empty;
            successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
        }

        public IEnumerable<UserLoginInfo> GetLogins()
        {
            var accounts = UserManager.GetLogins(User.Identity.GetUserId());
            CanRemoveExternalLogins = accounts.Count() > 1 || HasPassword();
            return accounts;
        }

        public void RemoveLogin(string loginProvider, string providerKey)
        {
            var result = UserManager.RemoveLogin(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            string msg = string.Empty;
            if (result.Succeeded)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                msg = "?m=RemoveLoginSuccess";
            }
            Response.Redirect("~/Account/ManageLogins" + msg);
        }
    }
}