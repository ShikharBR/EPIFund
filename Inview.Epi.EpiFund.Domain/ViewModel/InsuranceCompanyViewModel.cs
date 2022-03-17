using Inview.Epi.EpiFund.Domain.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class InsuranceCompanyViewModel : BaseModel
	{
		[Display(Name="Company Address")]
		public string CompanyAddress
		{
			get;
			set;
		}

		[Display(Name="Company Address 2")]
		public string CompanyAddress2
		{
			get;
			set;
		}

		[Display(Name="Company City")]
		public string CompanyCity
		{
			get;
			set;
		}

		[Display(Name="Insurance Company Name")]
		[Required]
		public string CompanyName
		{
			get;
			set;
		}

		[Display(Name="Company Phone Number")]
		public string CompanyPhone
		{
			get;
			set;
		}

		[Display(Name="Company State")]
		public string CompanyState
		{
			get;
			set;
		}

		[Display(Name="Website")]
		public string CompanyURL
		{
			get;
			set;
		}

		[Display(Name="Company Zip")]
		public string CompanyZip
		{
			get;
			set;
		}

		[Display(Name="Created On")]
		public DateTime CreateDate
		{
			get;
			set;
		}

		public int InsuranceCompanyId
		{
			get;
			set;
		}

		[Display(Name="Is Active?")]
		public bool IsActive
		{
			get;
			set;
		}

		public InsuranceCompanyViewModel()
		{
		}

		public void EntityToModel(PCInsuranceCompany entity)
		{
			this.CompanyAddress = entity.CompanyAddress1;
			this.CompanyAddress2 = entity.CompanyAddress2;
			this.CompanyCity = entity.CompanyCity;
			this.CompanyName = entity.CompanyName;
			this.CompanyPhone = entity.CompanyPhoneNumber;
			this.CompanyState = entity.CompanyState;
			this.CompanyURL = entity.CompanyURL;
			this.CompanyZip = entity.CompanyZip;
			this.CreateDate = entity.CreateDate;
			this.IsActive = entity.IsActive;
			this.InsuranceCompanyId = entity.PCInsuranceCompanyId;
		}

		public PCInsuranceCompany ModelToEntity()
		{
			PCInsuranceCompany pCInsuranceCompany = new PCInsuranceCompany()
			{
				CompanyAddress1 = this.CompanyAddress,
				CompanyAddress2 = this.CompanyAddress2,
				CompanyCity = this.CompanyCity,
				CompanyName = this.CompanyName,
				CompanyPhoneNumber = this.CompanyPhone,
				CompanyState = this.CompanyState,
				CompanyURL = this.CompanyURL,
				CompanyZip = this.CompanyZip,
				CreateDate = this.CreateDate,
				IsActive = this.IsActive,
				PCInsuranceCompanyId = this.InsuranceCompanyId
			};
			return pCInsuranceCompany;
		}
	}
}