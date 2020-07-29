using Esender.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Linq;

namespace Esender.Controllers
{
    [Authorize]
    public class EmailOperationsController : Controller
    {
        #region init
        private readonly IEmailRepository _email_repo;
        private readonly IWebHostEnvironment _hosting_env;

        public EmailOperationsController(IEmailRepository _email_repo,
                                         IWebHostEnvironment _hosting_env)
        {
            this._email_repo = _email_repo;
            this._hosting_env = _hosting_env;
        }

        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region add
        [HttpGet]
        public IActionResult AddEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEmail(EmailModel e_mail_address)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (e_mail_address != null)
                    {
                        _email_repo.Add(e_mail_address);
                        return RedirectToActionPermanent("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("EmailExists");
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return View();
        }
        #endregion

        #region delete
        [HttpGet]
        public IActionResult DeleteEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteEmail(string e_mail_address)
        {
            try
            {
                if (e_mail_address != string.Empty)
                {
                    int res = _email_repo.Delete(e_mail_address);
                    if (res > 0)
                    {
                        // successfully deleted
                        return RedirectToActionPermanent("Index", "Home");
                    }
                    else
                    {
                        //e_mail not exist
                        return RedirectToActionPermanent("Index", "Home");
                    }

                }
                else
                {
                    return RedirectToActionPermanent("Index", "Home");
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return View();
        }
         #endregion

        #region E-mail sending action
        // Integrate TinyMCE Html Editor: Get Html as string and send email to all of the added email addresses
        [HttpGet]
        public IActionResult SendMail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendMailAsync(BulkMailModel model)
        {
            try
            {
                model.Email_list = _email_repo.GetAllEmails().ToList<EmailModel>();

                MimeMessage message = new MimeMessage();

                // sender info
                MailboxAddress from = new MailboxAddress("Admin", "onat.celtik27@gmail.com");
                message.From.Add(from);
                message.Subject = ".NET Core Esender Mail";

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = model.Mail_text;
                message.Body = bodyBuilder.ToMessageBody();

                foreach (var mail_addrs in model.Email_list)
                {
                    MailboxAddress to = new MailboxAddress(mail_addrs.FirstName
                                                            + " "
                                                            + mail_addrs.LastName,
                                                           mail_addrs.Email);
                    message.To.Add(to);
                }

                #region OAuth2 but gives a uri_mismatch_error (commented)
                /*
                const string GMailAccount = "onat.celtik27@gmail.com"; //User.Identity.Name

                var clientSecrets = new ClientSecrets
                {
                    ClientId = "993637890339-f3rpvl6rbl3raa7usd9301gvsmv49gcq.apps.googleusercontent.com",
                    ClientSecret = "RyjONIbfVo18iADMGdusLg_X"
                };

                var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    // Cache tokens in ~/.local/share/google-filedatastore/CredentialCacheFolder on Linux/Mac
                    DataStore = new FileDataStore("CredentialCacheFolder", false),
                    Scopes = new[] { "https://mail.google.com/" },
                    ClientSecrets = clientSecrets
                });

                var codeReceiver = new LocalServerCodeReceiver();
                var authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);
                var credential = await authCode.AuthorizeAsync(GMailAccount, CancellationToken.None);

                if (authCode.ShouldRequestAuthorizationCode(credential.Token))
                    await credential.RefreshTokenAsync(CancellationToken.None);

                var oauth2 = new SaslMechanismOAuth2(credential.UserId, credential.Token.AccessToken);

                using (var imap_client = new ImapClient())
                {
                    await imap_client.ConnectAsync("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
                    await imap_client.AuthenticateAsync(oauth2);
                    await imap_client.DisconnectAsync(true);
                }
                */
                #endregion

                SmtpClient client = new SmtpClient();
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate("onat.celtik27@gmail.com", "ldboymuunjenvlqy");

                client.Send(message);
                client.Disconnect(true);
                client.Dispose();

                return RedirectToActionPermanent("Index", "Home");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}
