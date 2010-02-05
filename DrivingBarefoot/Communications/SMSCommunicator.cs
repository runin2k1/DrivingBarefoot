using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace DrivingBarefoot.Communications
{
    /// <summary>
    /// Basically the same as EmailCommunicator, this class requires message length be restricted and that the TO address match the appropriate SMS Gateway for the phone.  
    /// Usually this requires the user to provide their phone number, and the correct provider information which can be matched against a
    /// lookup of some sort(online, stored in db, etc...)
    /// </summary>
    public class SMSCommunicator : Communicator
    {

        public override int MaxMessageLength
        {
            get { return 160; }
        }

        public override void SendMessage()
        {
            if (FromAddress == null || ToAddress == null || Message == null)
                throw new MessageFieldsIncompleteException("From, To, or Message Body not specified");

            MailAddress from = new MailAddress(FromAddress);
            MailAddress to = new MailAddress(ToAddress);

            MailMessage mailMessage = new MailMessage(FromAddress, ToAddress);

            mailMessage.Subject = Subject;
            mailMessage.Body = Message;

            SmtpClient smtp = new SmtpClient("localhost");
            smtp.Send(mailMessage);
        }

        public static void SendTestMessage()
        {
            MailAddress from = new MailAddress("runi_zzzz@gmail.com");
            MailAddress to = new MailAddress("707321xxxx@txt.att.net");

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
