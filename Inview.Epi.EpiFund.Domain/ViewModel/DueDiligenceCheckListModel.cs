using Inview.Epi.EpiFund.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class DueDiligenceCheckListModel
	{
		[Display(Name="Current Rent Rolls")]
		public ImportanceLevel CurrentRentRolls
		{
			get;
			set;
		}

		[Display(Name="Current Year Appraisal")]
		public ImportanceLevel CurrentYearAppraisal
		{
			get;
			set;
		}

		[Display(Name="Current YTD Operating Reports")]
		public ImportanceLevel CurrentYTDOperatingReports
		{
			get;
			set;
		}

		public List<SelectListItem> ImportanceLevels
		{
			get;
			set;
		}

		[Display(Name="Old Appraisal")]
		public ImportanceLevel OldAppraisals
		{
			get;
			set;
		}

		[Display(Name="Original Title Policy Report of Current Owner")]
		public ImportanceLevel OriginalTitlePolicyReport
		{
			get;
			set;
		}

		[Display(Name="Other inspection items")]
		public List<DueDiligenceOptionalModel> OtherInspectionItems
		{
			get;
			set;
		}

		[Display(Name="Phase I Environment Report (date importance)")]
		public ImportanceLevel Phase1EnvironmentReport
		{
			get;
			set;
		}

		[Display(Name="Preliminary Title Report")]
		public ImportanceLevel PreliminaryTitleReport
		{
			get;
			set;
		}

		[Display(Name="Prior Years Operating Records")]
		public ImportanceLevel PriorYearsOperatingReports
		{
			get;
			set;
		}

		[Display(Name="Property Inspection Report (Independent)")]
		public ImportanceLevel PropertyInspectionReport
		{
			get;
			set;
		}

		[Display(Name="Property Specifications")]
		public ImportanceLevel PropertySpecifications
		{
			get;
			set;
		}

		[Display(Name="Site Video")]
		public ImportanceLevel SiteImages
		{
			get;
			set;
		}

		[Display(Name="Tenant Leases (Commercial Asset)")]
		public ImportanceLevel TenantLeases
		{
			get;
			set;
		}

		public DueDiligenceCheckListModel()
		{
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem()
			{
				Text = "Mandatory",
				Value = ImportanceLevel.Mandatory.ToString()
			};
			selectListItems.Add(selectListItem);
			SelectListItem selectListItem1 = new SelectListItem()
			{
				Text = "Moderate",
				Value = ImportanceLevel.Moderate.ToString()
			};
			selectListItems.Add(selectListItem1);
			SelectListItem selectListItem2 = new SelectListItem()
			{
				Text = "Optional",
				Value = ImportanceLevel.Optional.ToString()
			};
			selectListItems.Add(selectListItem2);
			this.ImportanceLevels = selectListItems;
			this.OtherInspectionItems = new List<DueDiligenceOptionalModel>();
		}
	}
}