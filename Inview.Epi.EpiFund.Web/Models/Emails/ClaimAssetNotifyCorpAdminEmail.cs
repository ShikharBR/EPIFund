using System;
using Postal;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
    public class ClaimAssetNotifyCorpAdminEmail : Email
    {
        public string Message { get; set; }
        public string To { get; set; }
        public string UserEmail { get; set; }
    }
}