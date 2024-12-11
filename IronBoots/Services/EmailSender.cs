﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace IronBoots.Services
{
        public class EmailSender : IEmailSender
        {
            public Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                Console.WriteLine($"Sending email to {email}: {subject}");
                return Task.CompletedTask;
            }
        }
}
