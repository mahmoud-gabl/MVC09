using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using IKEA.DAL.Models.Identity;

namespace IKEA.BLL.Common.Services.EmailSettings
{
    public class EmailSettings : IEmailSettings
    {
        public void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            //Sender- Reciver
            //Reciver= ranahatem447@gmail.com => User Who Try To reset Password
            client.Credentials = new NetworkCredential("ranahatem447@gmail.com", "zkqyekdysbeuyhit");//Generate Password
            client.Send("ranahatem447@gmail.com", email.To, email.Subject, email.Body);
           
        }
    }
}
