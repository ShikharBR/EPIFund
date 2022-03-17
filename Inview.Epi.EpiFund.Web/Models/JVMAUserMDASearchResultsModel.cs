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
	public class JVMAUserMDASearchResultsModel : BaseSearchResultsModel
	{
		[Display(Name="Actual COE End Date")]
		public DateTime? ActualCOEEnd
		{
			get;
			set;
		}

		[Display(Name="Actual COE Start Date")]
		public DateTime? ActualCOEStart
		{
			get;
			set;
		}

		[Display(Name="Asset ID #")]
		public int? AssetNumber
		{
			get;
			set;
		}

		public PagedList.IPagedList<JVMAUserMDAViewModel> Assets
		{
			get;
			set;
		}

		public List<SelectListItem> AssetTypes
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

		[Display(Name="Date of IPA End Date")]
		public DateTime? DateOfMDAEnd
		{
			get;
			set;
		}

		[Display(Name="Date of IPA Start Date")]
		public DateTime? DateOfMDAStart
		{
			get;
			set;
		}

		[Display(Name="Date USC Ref Fee Paid End Date")]
		public DateTime? DateRefFeePaidEnd
		{
			get;
			set;
		}

		[Display(Name="Date USC Ref Fee Paid Start Date")]
		public DateTime? DateRefFeePaidStart
		{
			get;
			set;
		}

		public string ParticipantFullName
		{
			get;
			set;
		}

		[Display(Name="Proposed COE End Date")]
		public DateTime? ProposedCOEEnd
		{
			get;
			set;
		}

		[Display(Name="Proposed COE Start Date")]
		public DateTime? ProposedCOEStart
		{
			get;
			set;
		}

		public string ReferredUserFullName
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

		public int UserId
		{
			get;
			set;
		}

		public JVMAUserMDASearchResultsModel()
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