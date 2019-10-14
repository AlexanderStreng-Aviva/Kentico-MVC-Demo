using System;
using System.Web;
using System.Web.Mvc;

using CMS.SiteProvider;

using DancingGoat;

using Kentico.Membership;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;

using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace DancingGoat
{
    /// <summary>
    /// Wraps application authentication configuration.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configures the application authentication.
        /// </summary>
        public void Configuration(IAppBuilder app)
        {
            // Register Kentico Membership identity implementation
            app.CreatePerOwinContext(() => UserManager.Initialize(app, new UserManager(new UserStore(SiteContext.CurrentSiteName))));
            app.CreatePerOwinContext<SignInManager>(SignInManager.Create);

            // Configure the sign in cookie
            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<UserManager, User, int>(
                        // Sets the interval after which the validity of the user's security stamp is checked
                        validateInterval: TimeSpan.FromMinutes(1),
                        regenerateIdentityCallback: (manager, user) => manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie),
                        getUserIdCallback: ((claimsIdentity) => int.Parse(claimsIdentity.GetUserId()))),
                    // Redirect to logon page with return url
                    OnApplyRedirect = context => context.Response.Redirect(urlHelper.Action("Login", "Account") + new Uri(context.RedirectUri).Query)
                },
                ExpireTimeSpan = TimeSpan.FromDays(14),
                SlidingExpiration = true
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }
    }
}
