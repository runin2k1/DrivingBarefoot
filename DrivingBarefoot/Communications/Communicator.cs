using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace DrivingBarefoot.Communications
{
    public abstract class Communicator
    {
        protected int _maxMessageLength;

        public abstract int MaxMessageLength { get; }

        private string _to;

        public string ToAddress
        {
            get
            {
                return _to;
            }
            set
            {
                _to = value;
            }
        }

        private string _from;
        public string FromAddress
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
            }
        }

        private string _message;

        /// <summary>
        /// Message must be set after TemplateFile is set for the body to fill the &lt;body&gt; template field
        /// </summary>
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;

                if (template != null)
                {
                    template.TemplateFields.Remove("<body>");

                    AddTemplateField("<body>", _message);
                }
            }
        }

        private string _subject = "";
        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;

                if (template != null)
                {
                    template.TemplateFields.Remove("<subject>");

                    AddTemplateField("<subject>", _subject);
                }
            }
        }

        private CommunicatorTemplate template;

        public string TemplateFile
        {
            set
            {
                template = new CommunicatorTemplate(value);
            }
        }

        public void AddTemplateField(String name, String value)
        {
            if (template == null)
                throw new MessageFieldsIncompleteException("template file not specified - can't add template fields without a template");
           
            template.TemplateFields.Add(name, value);
        }

        public void AddTemplateFields(Hashtable fields)
        {
            if (template == null)
                throw new MessageFieldsIncompleteException("template file not specified - can't add template fields without a template");

            Hashtable templateFields = template.TemplateFields;

            foreach (string key in fields.Keys)
            {
                templateFields.Add(key, fields[key]);
            }
        }
        /// <summary>
        /// Override this function to implement the actual message sending functionality
        /// -- call base.SendMessage() before sending the message to support the templating
        /// functionality
        /// </summary>
        public virtual void SendMessage()
        {
            if (template != null)
            {
                template.FillTemplate();
                _message = template.GetFilledTemplate();
            }
        }

        public abstract bool DeliveryAddressValidates();
    }

    public class MessageTooLongException : Exception
    {

    }

    public class MessageFieldsIncompleteException : Exception
    {
        public MessageFieldsIncompleteException(String message) : base(message)
        {

        }
    }
}
