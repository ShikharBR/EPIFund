using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class RecordPaymentModel
	{
		[Display(Name="Asset ID#")]
		public Guid AssetId
		{
			get;
			set;
		}

		[Display(Name="Asset ID#")]
		public int AssetNumber
		{
			get;
			set;
		}

		[Display(Name="Sale Amount")]
		public double PaymentAmount
		{
			get;
			set;
		}

		[Display(Name="Other Payment Information")]
		public string PaymentInformation
		{
			get;
			set;
		}

		[Display(Name="Payment Type")]
		public Inview.Epi.EpiFund.Domain.Enum.PaymentType PaymentType
		{
			get;
			set;
		}

		public List<SelectListItem> PaymentTypeList
		{
			get;
			set;
		}

		public int RecordedByUserId
		{
			get;
			set;
		}

		public RecordPaymentModel()
		{
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem()
			{
				Text = "Cash",
				Value = Inview.Epi.EpiFund.Domain.Enum.PaymentType.Cash.ToString()
			};
			selectListItems.Add(selectListItem);
			SelectListItem selectListItem1 = new SelectListItem()
			{
				Text = "Check",
				Value = Inview.Epi.EpiFund.Domain.Enum.PaymentType.Check.ToString()
			};
			selectListItems.Add(selectListItem1);
			SelectListItem selectListItem2 = new SelectListItem()
			{
				Text = "Bank Transfer",
				Value = Inview.Epi.EpiFund.Domain.Enum.PaymentType.BankTransfer.ToString()
			};
			selectListItems.Add(selectListItem2);
			SelectListItem selectListItem3 = new SelectListItem()
			{
				Text = "Credit Card",
				Value = Inview.Epi.EpiFund.Domain.Enum.PaymentType.CreditCard.ToString()
			};
			selectListItems.Add(selectListItem3);
			SelectListItem selectListItem4 = new SelectListItem()
			{
				Text = "Other",
				Value = Inview.Epi.EpiFund.Domain.Enum.PaymentType.Other.ToString()
			};
			selectListItems.Add(selectListItem4);
			this.PaymentTypeList = selectListItems;
		}
	}
}