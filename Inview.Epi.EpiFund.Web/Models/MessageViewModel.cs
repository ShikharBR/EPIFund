using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Inview.Epi.EpiFund.Web.Models
{
    public enum MessageTypes { None, Success, Error, Info };
    public class MessageViewModel
    {
        public string MessageText { get; set; }
        public MessageTypes Type { get; set; }

        public MessageViewModel(MessageTypes type, string message)
        {
            this.MessageText = message;
            this.Type = type;
        }

        public string MessageClass
        {
            get
            {
                StringBuilder sb = new StringBuilder("alert");

                switch (Type)
                {
                    case MessageTypes.Success:
                        sb.Append(" alert-success");
                        break;
                    case MessageTypes.Error:
                        sb.Append(" alert-danger");
                        break;
                    case MessageTypes.Info:
                        sb.Append(" alert-info");
                        break;
                    default:
                        break;
                }

                return sb.ToString();
            }
        }
    }
}