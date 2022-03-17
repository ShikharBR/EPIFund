using System;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Domain.Entity
{
	public class NARMember
	{
		public virtual string CellPhoneNumber
		{
			get;
			set;
		}

		public virtual double? CommissionAmount
		{
			get;
			set;
		}

		public virtual bool CommissionShareAgr
		{
			get;
			set;
		}

		public virtual string CompanyAddressLine1
		{
			get;
			set;
		}

		public virtual string CompanyAddressLine2
		{
			get;
			set;
		}

		public virtual string CompanyCity
		{
			get;
			set;
		}

		public virtual string CompanyName
		{
			get;
			set;
		}

		public virtual string CompanyState
		{
			get;
			set;
		}

		public virtual string CompanyZip
		{
			get;
			set;
		}

		public virtual DateTime? DateOfCsaConfirm
		{
			get;
			set;
		}

		public virtual string Email
		{
			get;
			set;
		}

		public virtual string FaxNumber
		{
			get;
			set;
		}

		public virtual string FirstName
		{
			get;
			set;
		}

		public virtual string FullName
		{
			get
			{
				return string.Concat(this.FirstName, " ", this.LastName);
			}
		}

		public virtual bool IsActive
		{
			get;
			set;
		}

		public virtual string LastName
		{
			get;
			set;
		}

		public virtual int NarMemberId
		{
			get;
			set;
		}

		public virtual bool NotOnList
		{
			get;
			set;
		}

		public virtual int? ReferredByUserId
		{
			get;
			set;
		}

		public virtual bool? Registered
		{
			get;
			set;
		}

		public virtual string Website
		{
			get;
			set;
		}

		public virtual string WorkPhoneNumber
		{
			get;
			set;
		}

		public NARMember()
		{
		}

		public NARMember Clone()
		{
			NARMember nARMember = this;
			NARMember nARMember1 = new NARMember()
			{
				CellPhoneNumber = nARMember.CellPhoneNumber,
				CommissionAmount = nARMember.CommissionAmount,
				CommissionShareAgr = nARMember.CommissionShareAgr,
				CompanyAddressLine1 = nARMember.CompanyAddressLine1,
				CompanyAddressLine2 = nARMember.CompanyAddressLine2,
				CompanyCity = nARMember.CompanyCity,
				CompanyName = nARMember.CompanyName,
				CompanyState = nARMember.CompanyState,
				CompanyZip = nARMember.CompanyZip,
				DateOfCsaConfirm = nARMember.DateOfCsaConfirm,
				Email = nARMember.Email.ToLower(),
				FaxNumber = nARMember.FaxNumber,
				FirstName = nARMember.FirstName,
				IsActive = true,
				LastName = nARMember.LastName,
				WorkPhoneNumber = nARMember.WorkPhoneNumber,
				NotOnList = false,
				ReferredByUserId = nARMember.ReferredByUserId,
				Registered = nARMember.Registered
			};
			return nARMember1;
		}
	}
}