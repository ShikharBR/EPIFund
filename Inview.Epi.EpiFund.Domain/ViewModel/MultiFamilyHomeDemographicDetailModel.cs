using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
	public class MultiFamilyHomeDemographicDetailModel
	{
		[Display(Name="Will you accept properties with above ground Mobile Homes (e.g., not affixed to the lot)?")]
		public bool? AcceptGroundMH
		{
			get;
			set;
		}

		[Display(Name="Will you accept properties with only double wide spaces?")]
		public bool? AcceptsDoubleSpacesOnly
		{
			get;
			set;
		}

		[Display(Name="Will you accept properties with EFF units?")]
		public bool? AcceptsEFFUnits
		{
			get;
			set;
		}

		[Display(Name="Will you accept properties with only double wide spaces?")]
		public bool? AcceptsOneBedroomUnits
		{
			get;
			set;
		}

		public bool? AcceptsSingleWide
		{
			get;
			set;
		}

		[Display(Name="Will you accept properties with only triple wide spaces?")]
		public bool? AcceptsTripleSpacesOnly
		{
			get;
			set;
		}

		[Display(Name="Age of Property Range")]
		public int AgeOfPropertyMaximum
		{
			get;
			set;
		}

		[Display(Name="Double wide space ratio as % of all Spaces")]
		public string DoubleWideSpaceRatioForAllSpaces
		{
			get;
			set;
		}

		[Display(Name="Can Asset have extensive renovation/updating needs?")]
		[Required]
		public bool? ExtensiveRenovationUpdatingNeeds
		{
			get;
			set;
		}

		public bool? ExtensiveRenovationUpdatingNeedsOptional
		{
			get;
			set;
		}

		[Display(Name="Minimum Grade classification requirement of Property")]
		[Required]
		public string GradeClassificationRequirementOfProperty
		{
			get;
			set;
		}

		public List<SelectListItem> GradeClassifications
		{
			get;
			set;
		}

		[Display(Name="Gym Facilities for Adults")]
		public bool? GymFacilitiesForAdults
		{
			get;
			set;
		}

		public bool? GymFacilitiesForAdultsOptional
		{
			get;
			set;
		}

		[Display(Name="Will you accept properties with above ground Mobile Homes (e.g., not affixed to the lot)?")]
		public bool? HasParkingRatioParameters
		{
			get;
			set;
		}

		[Display(Name="Master Metering")]
		public bool MasterMetering
		{
			get;
			set;
		}

		[Display(Name="If no, max % ratio of double wide spaces per MHP Development")]
		[Required]
		public decimal MaxRatioOfDoubleSpaces
		{
			get;
			set;
		}

		[Display(Name="If yes, max % ratio of EFF Units")]
		[Required]
		public decimal MaxRatioOfEFfUnits
		{
			get;
			set;
		}

		[Display(Name="If no, max % ratio of double wide spaces per MHP Development")]
		[Required]
		public decimal MaxRatioOfOneBedroomUnits
		{
			get;
			set;
		}

		public decimal MaxRatioOfSingleSpaces
		{
			get;
			set;
		}

		[Display(Name="If no, max % ratio of triple wide spaces per MHP Development")]
		[Required]
		public decimal MaxRatioOfTripleSpaces
		{
			get;
			set;
		}

		[Display(Name="Is Multi-Level Architecture acceptable?")]
		public bool? MultiLevelAcceptable
		{
			get;
			set;
		}

		public bool MultiLevelFourLevelsAcceptable
		{
			get;
			set;
		}

		public bool MultiLevelOtherLevelsAcceptable
		{
			get;
			set;
		}

		public bool MultiLevelThreeLevelsAcceptable
		{
			get;
			set;
		}

		public bool MultiLevelTwoLevelsAcceptable
		{
			get;
			set;
		}

		[Display(Name="Max Spaces")]
		public int NumberOfSpacesRangeMaximum
		{
			get;
			set;
		}

		[Display(Name="Min Spaces")]
		public int NumberOfSpacesRangeMinimum
		{
			get;
			set;
		}

		[Display(Name="Max")]
		public int NumberOfUnitsRangeMaximum
		{
			get;
			set;
		}

		[Display(Name="Min")]
		public int NumberOfUnitsRangeMinimum
		{
			get;
			set;
		}

		[Display(Name="Other Asset Demographic requirements")]
		public string OtherRequirements
		{
			get;
			set;
		}

		[Display(Name="Outdoor Spa's or Jacuzzi's")]
		public bool? OutdoorSpas
		{
			get;
			set;
		}

		public bool? OutdoorSpasOptional
		{
			get;
			set;
		}

		[Display(Name="If yes, define Ratio requisites:")]
		public string ParkingRatioRequisites
		{
			get;
			set;
		}

		[Display(Name="Playground area for children")]
		public bool? PlaygroundArea
		{
			get;
			set;
		}

		public bool? PlaygroundAreaOptional
		{
			get;
			set;
		}

		[Display(Name="Pool(s)")]
		public bool? Pools
		{
			get;
			set;
		}

		public bool? PoolsOptional
		{
			get;
			set;
		}

		[Display(Name="Do you require covered parking stalls for Asset Tenants?")]
		public bool? RequiresParkingStalls
		{
			get;
			set;
		}

		[Display(Name="Flat/Built Up = OK")]
		public bool RoofingFlatBuiltUp
		{
			get;
			set;
		}

		[Display(Name="Shingle = OK")]
		public bool RoofingShingleOnly
		{
			get;
			set;
		}

		[Display(Name="Tile Only")]
		public bool RoofingTileOnly
		{
			get;
			set;
		}

		[Display(Name="Security Gates")]
		public bool? SecurityGates
		{
			get;
			set;
		}

		public bool? SecurityGatesOptional
		{
			get;
			set;
		}

		[Display(Name="Separate Clubhouse")]
		public bool? SeparateClubhouse
		{
			get;
			set;
		}

		public bool? SeparateClubhouseOptional
		{
			get;
			set;
		}

		[Display(Name="Separate Office Building/Quarters")]
		public bool? SeparateOfficeBuilding
		{
			get;
			set;
		}

		public bool? SeparateOfficeBuildingOptional
		{
			get;
			set;
		}

		[Display(Name="Single wide space ratio as % of all Spaces")]
		public string SingleWideSpaceRatioForAllSpaces
		{
			get;
			set;
		}

		[Display(Name="Tenant Only")]
		public bool TenantOnly
		{
			get;
			set;
		}

		[Display(Name="Tenant Profile Restrictions/Requisites")]
		public string TenantProfileRestrictions
		{
			get;
			set;
		}

		[Display(Name="Tennis Courts for Adults")]
		public bool? TennisCourts
		{
			get;
			set;
		}

		public bool? TennisCourtsOptional
		{
			get;
			set;
		}

		[Display(Name="Do you require that Asset be deliverable as \"Turn Key\"?")]
		[Required]
		public bool? TurnKey
		{
			get;
			set;
		}

		public bool? TurnKeyOptional
		{
			get;
			set;
		}

		[Display(Name="Can Asset be an underperforming property?")]
		public bool? UnderperformingProperty
		{
			get;
			set;
		}

		public bool? UnderperformingPropertyOptional
		{
			get;
			set;
		}

		[Display(Name="Is a MHP with additional undeveloped land acceptable?")]
		public bool? UndevelopedAcceptable
		{
			get;
			set;
		}

		public MultiFamilyHomeDemographicDetailModel()
		{
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			SelectListItem selectListItem = new SelectListItem()
			{
				Value = "A+",
				Text = "A+"
			};
			selectListItems.Add(selectListItem);
			SelectListItem selectListItem1 = new SelectListItem()
			{
				Value = "A",
				Text = "A"
			};
			selectListItems.Add(selectListItem1);
			SelectListItem selectListItem2 = new SelectListItem()
			{
				Value = "A-",
				Text = "A-"
			};
			selectListItems.Add(selectListItem2);
			SelectListItem selectListItem3 = new SelectListItem()
			{
				Value = "B+",
				Text = "B+"
			};
			selectListItems.Add(selectListItem3);
			SelectListItem selectListItem4 = new SelectListItem()
			{
				Value = "B",
				Text = "B"
			};
			selectListItems.Add(selectListItem4);
			SelectListItem selectListItem5 = new SelectListItem()
			{
				Value = "B-",
				Text = "B-"
			};
			selectListItems.Add(selectListItem5);
			SelectListItem selectListItem6 = new SelectListItem()
			{
				Value = "C+",
				Text = "C+"
			};
			selectListItems.Add(selectListItem6);
			SelectListItem selectListItem7 = new SelectListItem()
			{
				Value = "C",
				Text = "C"
			};
			selectListItems.Add(selectListItem7);
			SelectListItem selectListItem8 = new SelectListItem()
			{
				Value = "C-",
				Text = "C-"
			};
			selectListItems.Add(selectListItem8);
			SelectListItem selectListItem9 = new SelectListItem()
			{
				Value = "D+",
				Text = "D+"
			};
			selectListItems.Add(selectListItem9);
			SelectListItem selectListItem10 = new SelectListItem()
			{
				Value = "D",
				Text = "D"
			};
			selectListItems.Add(selectListItem10);
			this.GradeClassifications = selectListItems;
		}
	}
}