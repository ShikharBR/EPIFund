using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Inview.Epi.EpiFund.Business
{
	public class SellerManager : ISellerManager
	{
		private IEPIContextFactory _factory;

		public SellerManager(IEPIContextFactory factory)
		{
			this._factory = factory;
		}

		public List<SellerIPAReceivedViewModel> GetSellerIPAsReceived(int userId)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			List<SellerIPAReceivedViewModel> list = (
				from ua in ePIRepository.AssetUserMDAs
				join a in ePIRepository.Assets on ua.AssetId equals a.AssetId
				join u in ePIRepository.Users on ua.UserId equals u.UserId
				where a.ListedByUserId == (int?)userId
				select new SellerIPAReceivedViewModel()
				{
					CorporateName = u.CompanyName,
					AlternateEmail = u.AlternateEmail,
					ContactHomeNumber = u.HomeNumber,
					ContactFaxNumber = u.FaxNumber,
					ContactCellNumber = u.CellNumber,
					ContactWorkNumber = u.WorkNumber,
					City = u.City,
					FirstName = u.FirstName,
					LastName = u.LastName,
					State = u.StateOfOriginCorporateEntity,
					Email = u.Username,
					DateIPAReceived = ua.SignMDADate,
					AssetId = a.AssetId,
					UserId = ua.UserId,
					ContactPhoneNumber = u.CellNumber,
					NameOfAsset = a.ProjectName
				}).ToList<SellerIPAReceivedViewModel>();
			foreach (SellerIPAReceivedViewModel sellerIPAReceivedViewModel in list)
			{
				sellerIPAReceivedViewModel.HasBuyerOtherIPAs = ePIRepository.AssetUserMDAs.Count<AssetUserMDA>((AssetUserMDA ua) => ua.UserId == sellerIPAReceivedViewModel.UserId) > 1;
				sellerIPAReceivedViewModel.DoesBuyerHavePOF = (
					from l in ePIRepository.LOIs
					join loid in ePIRepository.LOIDocuments on l.LOIId equals loid.LOIId
					where (int)loid.Type == 2 && l.UserId == sellerIPAReceivedViewModel.UserId
					select loid).Any<LOIDocument>();
			}
			return list;
		}

		public List<SellerLOIReceivedViewModel> GetSellerLOIsReceived(int userId)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			List<SellerLOIReceivedViewModel> list = (
				from l in ePIRepository.LOIs
				join a in ePIRepository.Assets on l.AssetId equals a.AssetId
				join u in ePIRepository.Users on l.UserId equals u.UserId
				where a.ListedByUserId == (int?)userId
				select new SellerLOIReceivedViewModel()
				{
					CorporateName = u.CompanyName,
					AlternateEmail = u.AlternateEmail,
					ContactHomeNumber = u.HomeNumber,
					ContactFaxNumber = u.FaxNumber,
					ContactCellNumber = l.CellPhoneNumber,
					ContactWorkNumber = l.WorkPhoneNumber,
					City = u.City,
					FirstName = u.FirstName,
					LastName = u.LastName,
					State = u.StateOfOriginCorporateEntity,
					Email = u.Username,
					OfferPrice = l.OfferingPurchasePrice,
					COENumberOfDays = l.ClosingDateNumberOfDays,
					AssetId = a.AssetId,
					UserId = l.UserId,
					DateLOIReceived = l.LOIDate,
					LoiId = l.LOIId,
					NameOfAsset = a.ProjectName,
					ContactPhoneNumber = u.CellNumber,
					HasReadLOI = l.ReadByListedByUser,
					CorporateRepBusinessNumber = u.WorkNumber,
					CorporateRepCellNumber = u.CellNumber
				}).ToList<SellerLOIReceivedViewModel>();
			foreach (SellerLOIReceivedViewModel userFileId in list)
			{
				SellerLOIReceivedViewModel sellerLOIReceivedViewModel = userFileId;
				DateTime dateLOIReceived = userFileId.DateLOIReceived;
				sellerLOIReceivedViewModel.ProposedCOEDate = dateLOIReceived.AddDays((double)Convert.ToInt32(userFileId.COENumberOfDays));
				UserFile userFile = ePIRepository.UserFiles.FirstOrDefault<UserFile>((UserFile w) => w.FileName.Contains("LOI") && (w.AssetId == userFileId.AssetId) && w.UserId == userFileId.UserId);
				if (userFile != null)
				{
					userFileId.UserFileId = userFile.UserFileId;
				}
			}
			return list;
		}

		public int GetUnreadLOICount(int userId)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			int num = (
				from l in ePIRepository.LOIs
				join a in ePIRepository.Assets on l.AssetId equals a.AssetId
				join u in ePIRepository.Users on l.UserId equals u.UserId
				where a.ListedByUserId == (int?)userId && !l.ReadByListedByUser
				select l).Count<LOI>();
			return num;
		}

		public void MarkLOIAsRead(Guid loiId)
		{
			IEPIRepository ePIRepository = this._factory.Create();
			LOI lOI = ePIRepository.LOIs.FirstOrDefault<LOI>((LOI l) => l.LOIId == loiId);
			if (lOI != null)
			{
				lOI.ReadByListedByUser = true;
				ePIRepository.Save();
			}
		}
	}
}