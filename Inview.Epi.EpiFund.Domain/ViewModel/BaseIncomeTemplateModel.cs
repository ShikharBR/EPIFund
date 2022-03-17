using System.ComponentModel.DataAnnotations;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public class BaseIncomeTemplateModel
    {
        [Display(Name = "Account Number")]
        public string AccountNumber
        {
            get;
            set;
        }

        [Display(Name = "Payer's Name")]
        public string PayerName
        {
            get;
            set;
        }

        [Display(Name = "Payer's Telephone Number")]
        public string PayerTelephone
        {
            get;
            set;
        }

        [Display(Name = "Payer's Federal Identification Number")]
        public string PayerFederalId
        {
            get;
            set;
        }

        [Display(Name = "Payer's Address")]
        public string PayerAddress
        {
            get;
            set;
        }

        [Display(Name = "Payer's City")]
        public string PayerCity
        {
            get;
            set;
        }

        [Display(Name = "Payer's State")]
        public string PayerState
        {
            get;
            set;
        }

        [Display(Name = "Payer's ZIP")]
        public string PayerZip
        {
            get;
            set;
        }

        [Display(Name = "Payer's Country")]
        public string PayerCountry
        {
            get;
            set;
        }

        [Display(Name = "Recipient's Name")]
        public string RecipientName
        {
            get;
            set;
        }

        [Display(Name = "Recipient's Telephone Number")]
        public string RecipientTelephone
        {
            get;
            set;
        }

        [Display(Name = "Recipient's Identification Number")]
        public string RecipientFederalId
        {
            get;
            set;
        }

        [Display(Name = "Recipient's Street Address(including apt. no.)")]
        public string RecipientAddress
        {
            get;
            set;
        }

        [Display(Name = "Recipient's City")]
        public string RecipientCity
        {
            get;
            set;
        }

        [Display(Name = "Recipient's State")]
        public string RecipientState
        {
            get;
            set;
        }

        [Display(Name = "Recipient's Zip")]
        public string RecipientZip
        {
            get;
            set;
        }

        [Display(Name = "Recipient's Country")]
        public string RecipientCountry
        {
            get;
            set;
        }
    }
}
