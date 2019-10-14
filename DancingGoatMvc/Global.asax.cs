using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

using CMS.AspNet.Platform;
using CMS.ContactManagement;

using Kentico.Web.Mvc;

namespace DancingGoat
{
    /// <summary>
    /// Describes the application.
    /// </summary>
    public class DancingGoatApplication : HttpApplication
    {
        /// <summary>
        /// Occurs when application starts.
        /// </summary>
        protected void Application_Start()
        {
            // Enable and configure selected Kentico ASP.NET MVC integration features
            ApplicationConfig.RegisterFeatures(ApplicationBuilder.Current);

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Register routes including system routes for enabled features
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Registers implementation to the dependency resolver
            DependencyResolverConfig.Register();
        }


        /// <summary>
        /// Occurs when an unhandled exception in application is thrown.
        /// </summary>
        protected void Application_Error()
        {
            ApplicationErrorLogger.LogLastApplicationError();
            Handle404Error();
        }


        /// <summary>
        /// Overrides basic application-wide implementation of the <see cref="P:System.Web.UI.PartialCachingAttribute.VaryByCustom" /> property.
        /// </summary>
        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            var parameters = custom.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                                   .OrderBy(p => p);

            var parts = new List<string>();
            foreach (var parameter in parameters)
            {
                switch (parameter)
                {
                    case "User":
                        parts.Add($"User={context.User.Identity.Name}");
                        break;

                    case "Persona":
                        // Gets the current contact, without creating a new anonymous contact for new visitors
                        var existingContact = ContactManagementContext.GetCurrentContact(createAnonymous: false);
                        var contactPersonaID = existingContact?.ContactPersonaID;
                        parts.Add($"Persona={contactPersonaID}|{context.User.Identity.Name}");
                        break;

                    case "Host":
                        parts.Add($"Host={context.Request.GetEffectiveUrl().Host}");
                        break;
                }
            }

            if (parts.Count > 0)
            {
                return string.Join("|", parts);
            }

            return base.GetVaryByCustomString(context, custom);
        }


        /// <summary>
        /// Handles the 404 error and setups the response for being handled by IIS (IIS behavior is specified in the "httpErrors" section in the web.config file).
        /// </summary>
        private void Handle404Error()
        {
            var error = Server.GetLastError();
            if ((error as HttpException)?.GetHttpCode() == 404)
            {
                Server.ClearError();
                Response.StatusCode = 404;
            }
        }
    }
}