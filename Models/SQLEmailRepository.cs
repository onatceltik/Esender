using Esender.ModelsAppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Esender.Models
{
    public class SQLEmailRepository : IEmailRepository
    {
        private AppDbContext context;

        public SQLEmailRepository(AppDbContext context)
        {
            this.context = context;
        }

        public EmailModel Add(EmailModel NewEmail)
        {
            context.Emails.Add(NewEmail);
            context.SaveChanges();
            return NewEmail;
        }

        public int Delete(string e_mail_address)
        {
            EmailModel tmp_email = GetAllEmails().Where(i => i.Email == e_mail_address).SingleOrDefault<EmailModel>();
            int ret = 0;

            if (tmp_email != null)
            {
                context.Emails.Remove(tmp_email);
                ret = context.SaveChanges();
            }

            return ret;
        }

        public IEnumerable<EmailModel> GetAllEmails()
        {
            return context.Emails;
        }
    }
}
