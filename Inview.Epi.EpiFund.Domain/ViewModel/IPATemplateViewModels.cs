using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class IPATemplateViewModels : BaseModel
	{
		[Display(Name="Assets")]
		public List<AssetDescriptionModel> Assets
		{
			get;
			set;
		}

		[Display(Name="City")]
		[Required]
		public string City
		{
			get;
			set;
		}

		[Display(Name="Name of Corporate Entity")]
		[Required]
		public string CompanyName
		{
			get;
			set;
		}

		[Display(Name="Title")]
		[Required]
		public string CorpTitle
		{
			get;
			set;
		}

		public string DateAccepted
		{
			get;
			set;
		}

		[Display(Name="Execution Day")]
		public int Day
		{
			get
			{
				return DateTime.Now.Day;
			}
		}

		[Display(Name="Email")]
		[Required]
		public string Email
		{
			get;
			set;
		}

		[Display(Name="Fax")]
		[Required]
		public string Fax
		{
			get;
			set;
		}

		[Display(Name="Acronym of Corporate Entity")]
		[Required]
		public string FirstNameOfCorporateEntity
		{
			get;
			set;
		}

		[Display(Name="Execution Month")]
		public string Month
		{
			get
			{
				return DateTime.Now.ToString("MMMM");
			}
		}

		[Display(Name="Phone")]
		[Required]
		public string Phone
		{
			get;
			set;
		}

		public string PropertyAddress1
		{
			get;
			set;
		}

		public string PropertyAddress2
		{
			get;
			set;
		}

		public string PropertyApns
		{
			get;
			set;
		}

		public string PropertyCity
		{
			get;
			set;
		}

		public string PropertyCounty
		{
			get;
			set;
		}

		public string PropertyName
		{
			get;
			set;
		}

		public string PropertyState
		{
			get;
			set;
		}

		public string PropertyType
		{
			get;
			set;
		}

		public string PropertyZip
		{
			get;
			set;
		}

		[Display(Name="State")]
		[Required]
		public string State
		{
			get;
			set;
		}

		[Display(Name="State of Origin of the Corporate Entity")]
		[Required]
		public string StateOfOriginOfCorporateEntity
		{
			get;
			set;
		}

		[Display(Name="Type of Corporate Entity")]
		[Required]
		public CorporateEntityType TypeOfCorporateEntity
		{
			get;
			set;
		}

		public List<SelectListItem> TypesOfCorporateEntity
		{
			get;
			set;
		}

		[Display(Name="Address Line 1")]
		[Required]
		public string UserAddressLine1
		{
			get;
			set;
		}

		[Display(Name="Address Line 2")]
		public string UserAddressLine2
		{
			get;
			set;
		}

		[Display(Name="First Name of Registrant")]
		[Required]
		public string UserFirstName
		{
			get;
			set;
		}

		[Display(Name="Last Name of Registrant")]
		[Required]
		public string UserLastName
		{
			get;
			set;
		}

		[Display(Name="Execution Year")]
		public int Year
		{
			get
			{
				return DateTime.Now.Year;
			}
		}

		public IPATemplateViewModels()
		{
			this.Assets = new List<AssetDescriptionModel>();
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem()
			{
				Text = "Corporation",
				Value = CorporateEntityType.Corporation.ToString()
			};
			selectListItems.Add(selectListItem);
			SelectListItem selectListItem1 = new SelectListItem()
			{
				Text = "Limited Liability Corporation",
				Value = CorporateEntityType.LimitedLiabilityCompany.ToString()
			};
			selectListItems.Add(selectListItem1);
			SelectListItem selectListItem2 = new SelectListItem()
			{
				Text = "Limited Liability Partnership",
				Value = CorporateEntityType.LimitedLiabilityPartnership.ToString()
			};
			selectListItems.Add(selectListItem2);
			SelectListItem selectListItem3 = new SelectListItem()
			{
				Text = "Joint Venture",
				Value = CorporateEntityType.JointVenture.ToString()
			};
			selectListItems.Add(selectListItem3);
			SelectListItem selectListItem4 = new SelectListItem()
			{
				Text = "Sole Proprietorship",
				Value = CorporateEntityType.SoleProprietorship.ToString()
			};
			selectListItems.Add(selectListItem4);
			this.TypesOfCorporateEntity = selectListItems;
		}
	}
}