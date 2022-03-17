using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class TitleCompanyUserModel
	{
		[Display(Name="Included States")]
		[Required]
		public List<SelectListItem> AvailableStates
		{
			get;
			set;
		}

		[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage="Password and confirm password do not match.")]
		[DataType(DataType.Password)]
		[Display(Name="Confirm Password")]
		public string ConfirmPassword
		{
			get;
			set;
		}

		public UserType ControllingUserType
		{
			get;
			set;
		}

		public bool DoNotShowLinks
		{
			get;
			set;
		}

		[DataType(DataType.EmailAddress)]
		[Display(Name="Email")]
		[Required]
		public string Email
		{
			get;
			set;
		}

		[Display(Name="First Name")]
		[RegularExpression("([a-zA-Z\\d]+[\\w\\d]*|)[a-zA-Z]+[\\w\\d.]*", ErrorMessage="Invalid first name")]
		[Required]
		public string FirstName
		{
			get;
			set;
		}

		public string FullName
		{
			get
			{
				return string.Concat(this.FirstName, " ", this.LastName);
			}
		}

		[Display(Name="Is Active?")]
		public bool IsActive
		{
			get;
			set;
		}

		[Display(Name="Is Manager?")]
		public bool IsManager
		{
			get;
			set;
		}

		[Display(Name="Last Name")]
		[RegularExpression("([a-zA-Z\\d]+[\\w\\d]*|)[a-zA-Z]+[\\w\\d.]*", ErrorMessage="Invalid last name")]
		[Required]
		public string LastName
		{
			get;
			set;
		}

		public string ManagingOfficerName
		{
			get;
			set;
		}

		public string OldEmail
		{
			get;
			set;
		}

		[Display(Name="Password")]
		public string Password
		{
			get;
			set;
		}

		[Display(Name="Phone Number")]
		public string PhoneNumber
		{
			get;
			set;
		}

		public List<StatesOfUS> SelectedStates
		{
			get;
			set;
		}

		public Dictionary<string, string> States
		{
			get;
			set;
		}

		public int TitleCompanyId
		{
			get;
			set;
		}

		public int TitleCompanyUserId
		{
			get;
			set;
		}

		public TitleCompanyUserModel(TitleCompanyModel model)
		{
			this.AvailableStates = new List<SelectListItem>();
			this.SelectedStates = new List<StatesOfUS>();
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "ALL", "ALL" },
				{ "AL", "Alabama" },
				{ "AK", "Alaska" },
				{ "AZ", "Arizona" },
				{ "AR", "Arkansas" },
				{ "CA", "California" },
				{ "CO", "Colorado" },
				{ "CT", "Connecticut" },
				{ "DC", "District of Columbia" },
				{ "DE", "Delaware" },
				{ "FL", "Florida" },
				{ "GA", "Georgia" },
				{ "HI", "Hawaii" },
				{ "ID", "Idaho" },
				{ "IL", "Illinois" },
				{ "IN", "Indiana" },
				{ "IA", "Iowa" },
				{ "KS", "Kansas" },
				{ "KY", "Kentucky" },
				{ "LA", "Louisiana" },
				{ "ME", "Maine" },
				{ "MD", "Maryland" },
				{ "MA", "Massachusetts" },
				{ "MI", "Michigan" },
				{ "MN", "Minnesota" },
				{ "MS", "Mississippi" },
				{ "MO", "Missouri" },
				{ "MT", "Montana" },
				{ "NE", "Nebraska" },
				{ "NV", "Nevada" },
				{ "NH", "New Hampshire" },
				{ "NJ", "New Jersey" },
				{ "NM", "New Mexico" },
				{ "NY", "New York" },
				{ "NC", "North Carolina" },
				{ "ND", "North Dakota" },
				{ "OH", "Ohio" },
				{ "OK", "Oklahoma" },
				{ "OR", "Oregon" },
				{ "PA", "Pennsylvania" },
				{ "RI", "Rhode Island" },
				{ "SC", "South Carolina" },
				{ "SD", "South Dakota" },
				{ "TN", "Tennessee" },
				{ "TX", "Texas" },
				{ "UT", "Utah" },
				{ "VT", "Vermont" },
				{ "VA", "Virginia" },
				{ "WA", "Washington" },
				{ "WV", "West Virginia" },
				{ "WI", "Wisconsin" },
				{ "WY", "Wyoming" }
			};
			this.States = strs;
			foreach (StatesOfUS selectedIncludedState in model.SelectedIncludedStates)
			{
				List<SelectListItem> availableStates = this.AvailableStates;
				SelectListItem selectListItem = new SelectListItem()
				{
					Text = this.States[selectedIncludedState.ToString()],
					Value = selectedIncludedState.ToString()
				};
				availableStates.Add(selectListItem);
			}
		}

		public TitleCompanyUserModel()
		{
			this.AvailableStates = new List<SelectListItem>();
			this.SelectedStates = new List<StatesOfUS>();
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "ALL", "ALL" },
				{ "AL", "Alabama" },
				{ "AK", "Alaska" },
				{ "AZ", "Arizona" },
				{ "AR", "Arkansas" },
				{ "CA", "California" },
				{ "CO", "Colorado" },
				{ "CT", "Connecticut" },
				{ "DC", "District of Columbia" },
				{ "DE", "Delaware" },
				{ "FL", "Florida" },
				{ "GA", "Georgia" },
				{ "HI", "Hawaii" },
				{ "ID", "Idaho" },
				{ "IL", "Illinois" },
				{ "IN", "Indiana" },
				{ "IA", "Iowa" },
				{ "KS", "Kansas" },
				{ "KY", "Kentucky" },
				{ "LA", "Louisiana" },
				{ "ME", "Maine" },
				{ "MD", "Maryland" },
				{ "MA", "Massachusetts" },
				{ "MI", "Michigan" },
				{ "MN", "Minnesota" },
				{ "MS", "Mississippi" },
				{ "MO", "Missouri" },
				{ "MT", "Montana" },
				{ "NE", "Nebraska" },
				{ "NV", "Nevada" },
				{ "NH", "New Hampshire" },
				{ "NJ", "New Jersey" },
				{ "NM", "New Mexico" },
				{ "NY", "New York" },
				{ "NC", "North Carolina" },
				{ "ND", "North Dakota" },
				{ "OH", "Ohio" },
				{ "OK", "Oklahoma" },
				{ "OR", "Oregon" },
				{ "PA", "Pennsylvania" },
				{ "RI", "Rhode Island" },
				{ "SC", "South Carolina" },
				{ "SD", "South Dakota" },
				{ "TN", "Tennessee" },
				{ "TX", "Texas" },
				{ "UT", "Utah" },
				{ "VT", "Vermont" },
				{ "VA", "Virginia" },
				{ "WA", "Washington" },
				{ "WV", "West Virginia" },
				{ "WI", "Wisconsin" },
				{ "WY", "Wyoming" }
			};
			this.States = strs;
		}
	}
}