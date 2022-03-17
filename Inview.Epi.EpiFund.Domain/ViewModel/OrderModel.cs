using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class OrderModel
	{
		public string APN
		{
			get;
			set;
		}

		public IEnumerable<string> Apns
		{
			get;
			set;
		}

		public Guid AssetId
		{
			get;
			set;
		}

		public string AssetName
		{
			get;
			set;
		}

		public int AssetNumber
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public string County
		{
			get;
			set;
		}

		public DateTime? DateOfOrder
		{
			get;
			set;
		}

		public DateTime? DateOfSubmit
		{
			get;
			set;
		}

		[Display(Name="Date Paid")]
		[Required]
		public DateTime? DatePaid
		{
			get;
			set;
		}

		[Display(Name="Amount Paid")]
		[Required]
		public double? Due
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}

		public int InsuranceCompanyId
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public Inview.Epi.EpiFund.Domain.Enum.OrderStatus OrderStatus
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public int TitleCompanyId
		{
			get;
			set;
		}

		public int? TitleOrderPaymentId
		{
			get;
			set;
		}

		public AssetType? Type
		{
			get;
			set;
		}

		public OrderModel()
		{
		}
	}
}