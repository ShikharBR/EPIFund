using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.Helpers;
using Inview.Epi.EpiFund.Domain.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Web.Models
{
	public class JVMANetUpSearchResultsModel : BaseSearchResultsModel
	{
		public List<SelectListItem> AssetTypes
		{
			get;
			set;
		}

		[Display(Name="City")]
		public string City
		{
			get;
			set;
		}

		[Display(Name="Date of DF Submittal End Date")]
		public DateTime? DateOfDFSubmittalEnd
		{
			get;
			set;
		}

		[Display(Name="Date of DF Submittal Start Date")]
		public DateTime? DateOfDFSubmittalStart
		{
			get;
			set;
		}

		[Display(Name="Date of LOI Submittal End Date")]
		public DateTime? DateOfLOISubmittalEnd
		{
			get;
			set;
		}

		[Display(Name="Date of LOI Submittal Start Date")]
		public DateTime? DateOfLOISubmittalStart
		{
			get;
			set;
		}

		[Display(Name="Date of Upload End Date")]
		public DateTime? DateOfUploadEnd
		{
			get;
			set;
		}

		[Display(Name="Date of Upload Start Date")]
		public DateTime? DateOfUploadStart
		{
			get;
			set;
		}

		[Display(Name="Date Ref Registered End Date")]
		public DateTime? DateRefRegisteredEnd
		{
			get;
			set;
		}

		[Display(Name="Date Ref Registered Start Date")]
		public DateTime? DateRefRegisteredStart
		{
			get;
			set;
		}

		[Display(Name="First Name")]
		public string FirstName
		{
			get;
			set;
		}

		public string FullName
		{
			get;
			set;
		}

		[Display(Name="Last Name")]
		public string LastName
		{
			get;
			set;
		}

		[Display(Name="Asset Type")]
		public string SelectedAssetType
		{
			get;
			set;
		}

		[Display(Name="State")]
		public string State
		{
			get;
			set;
		}

		public PagedList.IPagedList<JVMANetUpViewModel> Uploads
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		[Display(Name="User Type")]
		public Inview.Epi.EpiFund.Domain.Enum.UserType? UserType
		{
			get;
			set;
		}

		public JVMANetUpSearchResultsModel()
		{
			this.AssetTypes = new List<SelectListItem>()
			{
				new SelectListItem()
				{
					Value = null,
					Text = "All",
					Selected = true
				},
				new SelectListItem()
				{
					Value = "MF",
					Text = EnumHelper.GetEnumDescription(AssetType.MultiFamily)
				},
				new SelectListItem()
				{
					Value = "MHP",
					Text = EnumHelper.GetEnumDescription(AssetType.MHP)
				},
				new SelectListItem()
				{
					Value = "CO",
					Text = EnumHelper.GetEnumDescription(AssetType.Office)
				},
				new SelectListItem()
				{
					Value = "CR",
					Text = EnumHelper.GetEnumDescription(AssetType.Retail)
				},
				new SelectListItem()
				{
					Value = "Med",
					Text = EnumHelper.GetEnumDescription(AssetType.Medical)
				},
				new SelectListItem()
				{
					Value = "Mix",
					Text = EnumHelper.GetEnumDescription(AssetType.MixedUse)
				},
				new SelectListItem()
				{
					Value = "Ind",
					Text = EnumHelper.GetEnumDescription(AssetType.Industrial)
				},
				new SelectListItem()
				{
					Value = "Hot",
					Text = EnumHelper.GetEnumDescription(AssetType.Hotel)
				},
				new SelectListItem()
				{
					Value = "SF",
					Text = EnumHelper.GetEnumDescription(AssetType.ConvenienceStoreFuel)
				}
			};
		}
	}
}