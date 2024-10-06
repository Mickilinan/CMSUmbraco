using CMSUmbraco.Models;
using CMSUmbraco.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace CMSUmbraco.Controllers
{
    public class SupportSurfaceController : SurfaceController
    {
        public SupportSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)

        {
        }

        [HttpPost]
        public async Task<IActionResult> HandleSubmit(EmailModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || !IsValidEmail(model.Email))
            {
                TempData["error_email"] = true;
                return RedirectToCurrentUmbracoPage();
            }

            EmailService emailService = new EmailService();
            var result = await emailService.SendEmailAsync(model.Email, "Support Request");

            if (result)
            {
                TempData["success"] = "Thanks! We will get back to you ASAP!";
                TempData.Remove("email");
                return RedirectToCurrentUmbracoPage();
            }
            else
            {
                TempData["error_email"] = "An error occurred while submitting the form";
                return RedirectToCurrentUmbracoPage();
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
