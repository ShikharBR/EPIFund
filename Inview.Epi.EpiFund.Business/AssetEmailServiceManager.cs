using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.Helpers;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;

namespace Inview.Epi.EpiFund.Business
{
	public class AssetEmailServiceManager : IAssetEmailServiceManager
	{
		private EventLog _eventLog;

		private IEPIContextFactory _factory;

		private Timer _timer;

		private IEPIFundEmailService _email;

		private string _emailTemplatePath;

		private string _fromEmail = "info@uscreonline.com";

		public AssetEmailServiceManager(IEPIContextFactory factory, IEPIFundEmailService email)
		{
			this._email = email;
			this._factory = factory;
			this._emailTemplatePath = Path.GetFullPath("Views\\Emails");
		}

		private void _timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this._timer.Stop();
			try
			{
				IEPIRepository ePIRepository = this._factory.Create();
				List<EmailSchedule> list = (
					from w in ePIRepository.EmailSchedules
					orderby w.IntervalInDays
					select w).ToList<EmailSchedule>();
				foreach (EmailSchedule nullable in list)
				{
					bool flag = false;
					DateTime now = DateTime.Now;
					if (nullable.LastRunDate.HasValue)
					{
						DateTime value = nullable.LastRunDate.Value;
						DateTime dateTime = value.AddDays((double)nullable.IntervalInDays);
						value = dateTime.Date;
						DateTime value1 = nullable.LastRunDate.Value;
						dateTime = value.AddHours((double)value1.Hour);
						value = DateTime.Today;
						value1 = DateTime.Now;
						now = value.AddHours((double)value1.Hour);
						if (dateTime == now)
						{
							flag = true;
						}
					}
					else if (DateTime.Today == nullable.StartDate.Date)
					{
						flag = true;
					}
					if (flag)
					{
						switch (nullable.EmailScheduleType)
						{
							case EmailScheduleType.AvailabilityStatusOfListing:
							{
								this.sendListingAgentEmails();
								break;
							}
							case EmailScheduleType.SearchResult:
							{
								this.sendSearchResultEmails();
								break;
							}
							case EmailScheduleType.ListingAgentPropertyViewReport:
							{
								this.sendListingAgentPropertyViewEmails();
								break;
							}
						}
						nullable.LastRunDate = new DateTime?(now);
					}
					ePIRepository.Save();
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				this.logServiceEvent(string.Concat("Error sending emails. Error: ", exception.Message), EventLogEntryType.Error);
			}
			this._timer.Start();
		}

		public void logServiceEvent(string message, EventLogEntryType logType)
		{
			try
			{
				this._eventLog.WriteEntry(message, logType);
			}
			catch
			{
			}
		}

		private void sendListingAgentEmails()
		{
			IEPIRepository ePIRepository = this._factory.Create();
			IQueryable<User> assets = 
				from a in ePIRepository.Assets
				join u in ePIRepository.Users on a.ListedByUserId equals (int?)u.UserId
				where u.IsActive && (int)u.UserType != 4 && (int)u.UserType != 9 && (int)u.UserType != 3
				select u;
			foreach (User asset in assets)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string str = File.ReadAllText(Path.Combine(this._emailTemplatePath, "AllAssetListingStatusRequest.htm"));
				str = str.Replace("@Model.RecipientName", asset.FullName);
				IQueryable<Asset> assets1 = 
					from w in ePIRepository.Assets
					where w.ListedByUserId == (int?)asset.UserId
					select w;
				foreach (Asset asset1 in assets1)
				{
					stringBuilder.Append("<tr>");
					stringBuilder.Append("<td>");
					stringBuilder.Append(asset1.AssetNumber);
					stringBuilder.Append("</td>");
					stringBuilder.Append("<td>");
					object[] enumDescription = new object[] { "", EnumHelper.GetEnumDescription(asset1.BedCount), EnumHelper.GetEnumDescription(asset1.AssetType), asset1.City, asset1.State };
					stringBuilder.Append(string.Format("A {0} unit {1} property in {2}, {3}", enumDescription));
					stringBuilder.Append("</td>");
					stringBuilder.Append("<td>");
					stringBuilder.Append(EnumHelper.GetEnumDescription(asset1.ListingStatus));
					stringBuilder.Append("</td>");
					stringBuilder.Append("<td>");
					stringBuilder.Append("<a href=\"http://uscreonline.com/admin/changestatus?AssetNumber=\" + asset.AssetNumber\">Change Status</a>");
					stringBuilder.Append("</td>");
					stringBuilder.Append("</tr>");
				}
				str = str.Replace("@Model.AssetListingStatus", stringBuilder.ToString());
				List<string> strs = new List<string>()
				{
					asset.Username
				};
				if (asset.CorpAdminId.HasValue)
				{
					User user = ePIRepository.Users.Single<User>((User s) => s.UserId == asset.CorpAdminId.Value);
					strs.Add(user.Username);
				}
				this._email.Send(this._email.CreateMailMessage(this._fromEmail, "Asset Listing Status Request", str, strs, null));
			}
		}

		private void sendListingAgentPropertyViewEmails()
		{
			object[] lotSize;
			IEPIRepository ePIRepository = this._factory.Create();
			List<Asset> list = (
				from w in ePIRepository.Assets
				where (int)w.ListingStatus != 5 && w.Show
				select w).ToList<Asset>();
			foreach (Asset asset in list)
			{
				User user = ePIRepository.Users.Single<User>((User s) => (int?)s.UserId == asset.ListedByUserId);
				StringBuilder stringBuilder = new StringBuilder();
				string str = File.ReadAllText(Path.Combine(this._emailTemplatePath, "AssetListingView.htm"));
				str = str.Replace("@Model.RecipientName", user.FullName);
				int assetNumber = asset.AssetNumber;
				str = str.Replace("@Model.AssetNumber", assetNumber.ToString());
				if ((asset.AssetType == AssetType.MultiFamily ? false : asset.AssetType != AssetType.MHP))
				{
					CommercialAsset commercialAsset = asset as CommercialAsset;
					lotSize = new object[] { commercialAsset.LotSize, EnumHelper.GetEnumDescription(asset.AssetType), asset.City, asset.State };
					str = str.Replace("@Model.AssetDescription", string.Format("A {0} sq. ft. {1} property in {2}, {3}", lotSize));
				}
				else
				{
					MultiFamilyAsset multiFamilyAsset = asset as MultiFamilyAsset;
					lotSize = new object[] { multiFamilyAsset.TotalUnits, EnumHelper.GetEnumDescription(asset.AssetType), asset.City, asset.State };
					str = str.Replace("@Model.AssetDescription", string.Format("A {0} unit {1} property in {2}, {3}", lotSize));
				}
				List<AssetUserView> assetUserViews = (
					from w in QueryableExtensions.Include<AssetUserView, User>(ePIRepository.AssetUserViews, (AssetUserView s) => s.User)
					where w.AssetId == asset.AssetId
					select w).ToList<AssetUserView>();
				foreach (AssetUserView assetUserView in assetUserViews)
				{
					stringBuilder.Append("<tr>");
					stringBuilder.Append("<td>");
					stringBuilder.Append(assetUserView.User.FullName);
					stringBuilder.Append("</td>");
					stringBuilder.Append("<td>");
					DateTime viewDate = assetUserView.ViewDate;
					stringBuilder.Append(viewDate.ToString("MM/dd/yyyy hh:mm tt"));
					stringBuilder.Append("</td>");
					stringBuilder.Append("</tr>");
				}
				str = str.Replace("@Model.UserViews", stringBuilder.ToString());
				IEPIFundEmailService ePIFundEmailService = this._email;
				IEPIFundEmailService ePIFundEmailService1 = this._email;
				string str1 = this._fromEmail;
				List<string> strs = new List<string>()
				{
					user.Username
				};
				ePIFundEmailService.Send(ePIFundEmailService1.CreateMailMessage(str1, "Asset Listing Status Request", str, strs, null));
			}
		}

		private void sendSearchResultEmails()
		{
			bool flag;
			IEPIRepository ePIRepository = this._factory.Create();
			IQueryable<Asset> assets = 
				from w in ePIRepository.Assets
				where w.HoldForUserId.HasValue
				select w;
			foreach (Asset asset in assets)
			{
				DateTime? holdStartDate = asset.HoldStartDate;
				if (!holdStartDate.HasValue)
				{
					flag = true;
				}
				else
				{
					DateTime today = DateTime.Today;
					holdStartDate = asset.HoldStartDate;
					DateTime value = holdStartDate.Value;
					value = value.Date;
					flag = !(today > value.AddDays(7));
				}
				if (!flag)
				{
					asset.HoldForUserId = null;
					holdStartDate = null;
					asset.HoldStartDate = holdStartDate;
					asset.Show = true;
					ePIRepository.Save();
				}
			}
		}

		public void Start(EventLog log)
		{
			this._eventLog = log;
			double num = 3600000;
			if (ConfigurationManager.AppSettings["TimerInterval"] != null)
			{
				num = Convert.ToDouble(ConfigurationManager.AppSettings["TimerInterval"]);
			}
			this._timer = new Timer(num);
			this._timer.Elapsed += new ElapsedEventHandler(this._timer_Elapsed);
			this._timer.Start();
		}

		public void Stop()
		{
			this._timer.Stop();
			this._timer.Close();
			this._timer.Dispose();
		}
	}
}