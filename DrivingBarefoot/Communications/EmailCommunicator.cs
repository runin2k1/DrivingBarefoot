using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace DrivingBarefoot.Communications
{
    public class EmailCommunicator : Communicator
    {
        public override int MaxMessageLength
        {
            get { return 0; }
        }

        public override void SendMessage()
        {
            base.SendMessage();

            if (FromAddress == null || ToAddress == null || Message == null)
                throw new MessageFieldsIncompleteException("From, To, or Message Body not specified");

            MailAddress from = new MailAddress(FromAddress);
            MailAddress to = new MailAddress(ToAddress);

            MailMessage mailMessage = new MailMessage(from, to);

            mailMessage.Subject = Subject;
            mailMessage.Body = Message;

            SmtpClient smtp = new SmtpClient("localhost");
            smtp.Send(mailMessage);
        }

        public static void SendTestMessage()
        {
            MailAddress from = new MailAddress("runin2k1@gmail.com");
            MailAddress to = new MailAddress("runin2k1@gmail.com");

            MailMessage mailMessage = new MailMessage(from, to);

            mailMessage.Subject = "A Test from DB.EmailCommunicator";
            mailMessage.Body = "I am a message.";

            SmtpClient smtp = new SmtpClient("localhost");
            smtp.Send(mailMessage);
        }

        public override bool DeliveryAddressValidates()
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
