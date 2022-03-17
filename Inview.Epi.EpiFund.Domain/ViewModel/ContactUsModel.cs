using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class ContactUsModel
	{
		[Display(Name="Contact Number")]
		[Required]
		public string ContactNumber
		{
			get;
			set;
		}

		public DateTime DateOfInquiry
		{
			get;
			set;
		}

		[Display(Name="Email Address")]
		[Required]
		public string EmailAddress
		{
			get;
			set;
		}

		[Display(Name="Name")]
		[Required]
		public string Name
		{
			get;
			set;
		}

		[DataType(DataType.MultilineText)]
		[Display(Name="Questions/Comments")]
		[Required]
		[StringLength(500, MinimumLength=1, ErrorMessage="Questions/comments must be between 1 and 500 characters!")]
		public string QuestionsComments
		{
			get;
			set;
		}

		[Display(Name="Select which topic(s) your questions/comments are in regards to. Thank you.")]
		public List<string> SelectedTopics
		{
			get;
			set;
		}

		public List<SelectListItem> Topics
		{
			get;
			set;
		}

		public ContactUsModel()
		{
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem()
			{
				Text = "Site Registration",
				Value = "Site Registration"
			};
			selectListItems.Add(selectListItem);
			SelectListItem selectListItem1 = new SelectListItem()
			{
				Text = "[My USC Page]",
				Value = "[My USC Page]"
			};
			selectListItems.Add(selectListItem1);
			SelectListItem selectListItem2 = new SelectListItem()
			{
				Text = "NCND Agreement",
				Value = "NCND Agreement"
			};
			selectListItems.Add(selectListItem2);
			SelectListItem selectListItem3 = new SelectListItem()
			{
				Text = "IPA Agreement",
				Value = "MDA Agreement"
			};
			selectListItems.Add(selectListItem3);
			SelectListItem selectListItem4 = new SelectListItem()
			{
				Text = "Search Criteria Form",
				Value = "Search Criteria Form"
			};
			selectListItems.Add(selectListItem4);
			SelectListItem selectListItem5 = new SelectListItem()
			{
				Text = "IC Agreement",
				Value = "IC Agreement"
			};
			selectListItems.Add(selectListItem5);
			SelectListItem selectListItem6 = new SelectListItem()
			{
				Text = "JV Marketing Agreement",
				Value = "JV Marketing Agreement"
			};
			selectListItems.Add(selectListItem6);
			SelectListItem selectListItem7 = new SelectListItem()
			{
				Text = "my [Search Criteria] Form",
				Value = "My [Search Criteria] Form"
			};
			selectListItems.Add(selectListItem7);
			SelectListItem selectListItem8 = new SelectListItem()
			{
				Text = "USC Growth Opportunities",
				Value = "Employment Opportunities"
			};
			selectListItems.Add(selectListItem8);
			SelectListItem selectListItem9 = new SelectListItem()
			{
				Text = "Email Notifications",
				Value = "Email Notifications"
			};
			selectListItems.Add(selectListItem9);
			SelectListItem selectListItem10 = new SelectListItem()
			{
				Text = "USC Asset Inventory Views",
				Value = "USC Asset Inventory Views"
			};
			selectListItems.Add(selectListItem10);
			SelectListItem selectListItem11 = new SelectListItem()
			{
				Text = "Assets I own and wish to sell",
				Value = "Assets I own and wish to sell"
			};
			selectListItems.Add(selectListItem11);
			SelectListItem selectListItem12 = new SelectListItem()
			{
				Text = "Debt/Equity Funding Partner",
				Value = "Debt/Equity Funding Partner"
			};
			selectListItems.Add(selectListItem12);
			SelectListItem selectListItem13 = new SelectListItem()
			{
				Text = "USC Asset Views for User Portfolio",
				Value = "USC Asset Views for User Portfolio"
			};
			selectListItems.Add(selectListItem13);
			this.Topics = selectListItems;
		}
	}
}