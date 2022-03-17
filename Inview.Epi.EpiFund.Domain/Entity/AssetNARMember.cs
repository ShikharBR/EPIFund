using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class AssetNARMember
	{
		public virtual Inview.Epi.EpiFund.Domain.Entity.Asset Asset
		{
			get;
			set;
		}

		public virtual Guid AssetId
		{
			get;
			set;
		}

		[Key]
		public virtual Guid AssetNARMemberId
		{
			get;
			set;
		}

		public virtual Inview.Epi.EpiFund.Domain.Entity.NARMember NARMember
		{
			get;
			set;
		}

		public virtual int NarMemberId
		{
			get;
			set;
		}

		public virtual string SelectedNARMemberId
		{
			get;
			set;
		}

		public AssetNARMember()
		{
		}

		public AssetNARMember Clone()
		{
			AssetNARMember assetNARMember = this;
			Inview.Epi.EpiFund.Domain.Entity.NARMember nARMember = new Inview.Epi.EpiFund.Domain.Entity.NARMember()
			{
				CellPhoneNumber = assetNARMember.NARMember.CellPhoneNumber,
				CommissionAmount = assetNARMember.NARMember.CommissionAmount,
				CommissionShareAgr = assetNARMember.NARMember.CommissionShareAgr,
				CompanyAddressLine1 = assetNARMember.NARMember.CompanyAddressLine1,
				CompanyAddressLine2 = assetNARMember.NARMember.CompanyAddressLine2,
				CompanyCity = assetNARMember.NARMember.CompanyCity,
				CompanyName = assetNARMember.NARMember.CompanyName,
				CompanyState = assetNARMember.NARMember.CompanyState,
				CompanyZip = assetNARMember.NARMember.CompanyZip,
				DateOfCsaConfirm = assetNARMember.NARMember.DateOfCsaConfirm,
				Email = assetNARMember.NARMember.Email,
				FaxNumber = assetNARMember.NARMember.FaxNumber,
				FirstName = assetNARMember.NARMember.FirstName,
				IsActive = true,
				LastName = assetNARMember.NARMember.LastName,
				WorkPhoneNumber = assetNARMember.NARMember.WorkPhoneNumber
			};
			AssetNARMember assetNARMember1 = new AssetNARMember()
			{
				Asset = assetNARMember.Asset,
				AssetId = assetNARMember.AssetId,
				AssetNARMemberId = Guid.NewGuid(),
				NARMember = nARMember,
				SelectedNARMemberId = string.Empty
			};
			return assetNARMember1;
		}
	}
}