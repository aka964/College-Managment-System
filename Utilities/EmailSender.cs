using Microsoft.AspNetCore.Identity.UI.Services;

namespace SchoolManagementApp.MVC.Utilities

{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}