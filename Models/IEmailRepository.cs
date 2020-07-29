using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Esender.Models
{
    public interface IEmailRepository
    {
        EmailModel Add(EmailModel NewEmail);
        int Delete(string e_mail_address);
        IEnumerable<EmailModel> GetAllEmails();
    }
}
