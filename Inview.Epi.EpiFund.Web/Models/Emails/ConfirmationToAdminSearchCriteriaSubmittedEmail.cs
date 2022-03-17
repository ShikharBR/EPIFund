using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Postal;

namespace Inview.Epi.EpiFund.Web.Models.Emails
{
    public class ConfirmationPISellerAssetIsPublishedEmail : Email
    {
        public string To { get; set; }
        public string Email { get; set; }
        public string AssetNumber { get; set; }
        public string AssetAddressOneLine { get; set; }
        public string AssetDescription { get; set; }
        public string APN { get; set; }
        public string VestingEntity { get; set; }
        public string CorpOfficer { get; set; }
        public DateTime DatePublished { get; set; }
    }

    public class ConfirmationPISellerAssetIsUnPublishedEmail : Email
    {
        public string To { get; set; }
        public string Email { get; set; }
        public string AssetNumber { get; set; }
        public string AssetAddressOneLine { get; set; }
        public string AssetDescription { get; set; }
        public string APN { get; set; }
        public string VestingEntity { get; set; }
        public string CorpOfficer { get; set; }
        public DateTime DatePublished { get; set; }
    }
}