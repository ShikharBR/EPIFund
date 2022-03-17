using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;
using Postal;
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
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;

namespace Inview.Epi.EpiFund.Business
{
	public class DocusignServiceManager : IDocusignServiceManager
	{
		private EventLog _eventLog;

		private IEPIContextFactory _factory;

		private Timer _timer;

		private IPDFService _pdf;

		private IAssetManager _asset;

		private IEPIFundEmailService _email;

		private IUserManager _user;

		private string _viewsPath = ConfigurationManager.AppSettings["EmailTemplatePath"];

		private string _fromEmail = "info@uscreonline.com";

		public DocusignServiceManager(IEPIContextFactory factory, IPDFService pdf, IAssetManager asset, IEPIFundEmailService email, IUserManager user)
		{
			this._user = user;
			this._email = email;
			this._asset = asset;
			this._factory = factory;
			this._pdf = pdf;
		}

		private void _timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this._timer.Stop();
			this.ProcessSignedDocuments(null);
			this._timer.Start();
		}

		public void logServiceEvent(string message, EventLogEntryType logType)
		{
			try
			{
				string[] strArrays = new string[1];
				string[] str = new string[] { "(", null, null, null, null };
				str[1] = DateTime.Now.ToString();
				str[2] = ") - ";
				str[3] = message;
				str[4] = Environment.NewLine;
				strArrays[0] = string.Concat(str);
				File.AppendAllLines("\\\\n7app01\\c$\\Data\\EPIFund\\error.txt", strArrays);
				this._eventLog.WriteEntry(message, logType);
			}
			catch (Exception exception)
			{
			}
		}

		public List<DocumentType> ProcessSignedDocuments(int? userId = null)
		{
			string[] strArrays;
			string str;
			int num;
			AssetUserMDA now;
			Exception exception;
			List<DocumentType> documentTypes;
			char[] chrArray;
			try
			{
				List<DocumentType> documentTypes1 = new List<DocumentType>();
				IEPIRepository ePIRepository = this._factory.Create();
				DateTime dateTime = DateTime.Now.AddDays(-7);
				IQueryable<DocusignEnvelope> docusignEnvelopes = 
					from w in ePIRepository.DocusignEnvelopes
					where !w.ReceivedSignedDocument && (w.DateCreated >= dateTime)
					select w;
				if (userId.HasValue)
				{
					docusignEnvelopes = 
						from w in docusignEnvelopes
						where w.UserId == userId.Value
						select w;
				}
				foreach (DocusignEnvelope list in docusignEnvelopes.ToList<DocusignEnvelope>())
				{
					try
					{
						documentTypes1.Add(list.DocumentType);
						string documentFromDocusign = this._pdf.GetDocumentFromDocusign(list.EnvelopeId.ToString(), list.UserId, list.DocumentType);
						if (!string.IsNullOrEmpty(documentFromDocusign))
						{
							string[] str1 = new string[] { "Received document ", documentFromDocusign, " for envelope ID# ", list.EnvelopeId.ToString(), " from Docusign" };
							this.logServiceEvent(string.Concat(str1), EventLogEntryType.Information);
							User nullable = ePIRepository.Users.Single<User>((User s) => s.UserId == list.UserId);
							UserFile userFile = new UserFile()
							{
								DateUploaded = DateTime.Now,
								FileLocation = documentFromDocusign,
								FileName = Path.GetFileNameWithoutExtension(documentFromDocusign),
								UserId = list.UserId
							};
							UserFile assetId = userFile;
							switch (list.DocumentType)
							{
								case DocumentType.NCND:
								{
									nullable.NCNDFileLocation = documentFromDocusign;
									nullable.NCNDSignDate = new DateTime?(DateTime.Now);
									ePIRepository.Save();
									assetId.FileName = "Executed NCND";
									break;
								}
								case DocumentType.MDA:
								{
									if (list.AssetNumbers != null)
									{
										StringBuilder stringBuilder = new StringBuilder();
										StringBuilder stringBuilder1 = new StringBuilder();
										string assetNumbers = list.AssetNumbers;
										chrArray = new char[] { '&' };
										strArrays = assetNumbers.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
										if (strArrays.Count<string>() != 1)
										{
											for (int i = 0; i < strArrays.Count<string>(); i++)
											{
												str = strArrays[i];
												if (str.Length > 0)
												{
													num = 0;
													int.TryParse(str, out num);
													if (num != 0)
													{
														AssetDescriptionModel assetByAssetNumber = this._asset.GetAssetByAssetNumber(num);
														if (assetByAssetNumber != null)
														{
															now = ePIRepository.AssetUserMDAs.FirstOrDefault<AssetUserMDA>((AssetUserMDA w) => (w.AssetId == assetByAssetNumber.AssetId) && w.UserId == list.UserId);
															if (now == null)
															{
																IDbSet<AssetUserMDA> assetUserMDAs = ePIRepository.AssetUserMDAs;
																AssetUserMDA assetUserMDA = new AssetUserMDA()
																{
																	AssetId = assetByAssetNumber.AssetId,
																	FileLocation = documentFromDocusign,
																	SignMDADate = DateTime.Now,
																	UserId = list.UserId
																};
																assetUserMDAs.Add(assetUserMDA);
															}
															else
															{
																now.SignMDADate = DateTime.Now;
																now.FileLocation = documentFromDocusign;
															}
															ePIRepository.Save();
															assetId.FileName = string.Concat("MDA for Asset ID# ", assetByAssetNumber.AssetNumber);
															if (!stringBuilder.ToString().Contains(assetByAssetNumber.CityStateFormattedString))
															{
																stringBuilder.Append(assetByAssetNumber.CityStateFormattedString);
																if (!(strArrays.Count<string>() <= 1 ? true : i != strArrays.Count<string>() - 2))
																{
																	stringBuilder.Append(", and ");
																}
																else if (i < strArrays.Count<string>() - 1)
																{
																	stringBuilder.Append(", ");
																}
															}
															stringBuilder1.Append(assetByAssetNumber.AssetNumber);
															if (!(strArrays.Count<string>() <= 1 ? true : i != strArrays.Count<string>() - 2))
															{
																stringBuilder1.Append(", and ");
															}
															else if (i < strArrays.Count<string>() - 1)
															{
																stringBuilder1.Append(", ");
															}
														}
													}
												}
											}
										}
										else
										{
											num = 0;
											int.TryParse(strArrays[0], out num);
											if (num != 0)
											{
												AssetDescriptionModel assetDescriptionModel = this._asset.GetAssetByAssetNumber(num);
												now = ePIRepository.AssetUserMDAs.FirstOrDefault<AssetUserMDA>((AssetUserMDA w) => (w.AssetId == assetDescriptionModel.AssetId) && w.UserId == list.UserId);
												if (now == null)
												{
													IDbSet<AssetUserMDA> assetUserMDAs1 = ePIRepository.AssetUserMDAs;
													AssetUserMDA assetUserMDA1 = new AssetUserMDA()
													{
														AssetId = assetDescriptionModel.AssetId,
														FileLocation = documentFromDocusign,
														SignMDADate = DateTime.Now,
														UserId = list.UserId
													};
													assetUserMDAs1.Add(assetUserMDA1);
												}
												else
												{
													now.SignMDADate = DateTime.Now;
													now.FileLocation = documentFromDocusign;
												}
												ePIRepository.Save();
												this.sendSignedAssetMDAConfirmationEmailUsingHtml(assetDescriptionModel, nullable, documentFromDocusign);
											}
										}
									}
									break;
								}
								case DocumentType.PersonalFinancialStatement:
								{
									assetId.FileName = "Executed Personal Financial Statement";
									assetId.Type = new UploadUserFileType?(UploadUserFileType.PFS);
									break;
								}
								case DocumentType.ICAgreement:
								{
									nullable.ICFileLocation = documentFromDocusign;
									assetId.FileName = "Executed IC Agreement";
									ePIRepository.Save();
									break;
								}
								case DocumentType.JVAgreement:
								{
									nullable.JVMarketerAgreementLocation = documentFromDocusign;
									ePIRepository.Save();
									assetId.FileName = "Executed JV Agreement";
									this.sendConfirmationJVMarketerEmailUsingHtml(nullable, documentFromDocusign);
									break;
								}
								case DocumentType.BindingContingent:
								{
								Label0:
									break;
								}
								case DocumentType.LOI:
								{
									assetId.FileName = "Executed LOI";
									if (list.AssetNumbers != null)
									{
										string assetNumbers1 = list.AssetNumbers;
										chrArray = new char[] { '&' };
										strArrays = assetNumbers1.Split(chrArray);
										string[] strArrays1 = strArrays;
										for (int j = 0; j < (int)strArrays1.Length; j++)
										{
											str = strArrays1[j];
											num = 0;
											int.TryParse(str, out num);
											if (num != 0)
											{
												AssetDescriptionModel assetByAssetNumber1 = this._asset.GetAssetByAssetNumber(num);
												assetId.AssetId = assetByAssetNumber1.AssetId;
												ePIRepository.Save();
												LOI lOI = (
													from x in ePIRepository.LOIs
													where x.Active && (x.AssetId == assetByAssetNumber1.AssetId) && (int?)x.UserId == userId
													select x).ToList<LOI>().SingleOrDefault<LOI>();
												if (lOI != null)
												{
													List<string> strs = new List<string>();
													List<LOIDocument> lOIDocuments = (
														from x in ePIRepository.LOIDocuments
														where x.LOIId == lOI.LOIId
														select x).ToList<LOIDocument>();
													foreach (LOIDocument lOIDocument in lOIDocuments)
													{
														string item = ConfigurationManager.AppSettings["DataRoot"];
														Guid lOIId = lOI.LOIId;
														strs.Add(Path.Combine(item, "Documents", lOIId.ToString(), lOIDocument.FileName));
													}
													strs.Add(documentFromDocusign);
													this.sendListingAgentLOIUsingHtml(lOI, strs, assetByAssetNumber1, nullable);
												}
											}
										}
									}
									break;
								}
								case DocumentType.SellerIPA:
								case DocumentType.NARIPA:
								{
									string assetNumbers2 = list.AssetNumbers;
									chrArray = new char[] { '&' };
									string[] strArrays2 = assetNumbers2.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
									AssetDescriptionModel assetDescriptionModel1 = this._asset.GetAssetByAssetNumber(Convert.ToInt32(strArrays2.First<string>()));
									this._asset.PublishAsset(assetDescriptionModel1.AssetId);
									assetId.FileName = string.Concat("Seller/NAR IPA for Asset # ", strArrays2.First<string>());
									assetId.AssetId = assetDescriptionModel1.AssetId;
									this.sendConfirmationPISellerAssetIsPublishedEmail(assetDescriptionModel1, documentFromDocusign, nullable.Username, nullable.FullName);
									ePIRepository.Save();
									break;
								}
								default:
                                    break;
							}
							list.ReceivedSignedDocument = true;
							list.DateReceived = new DateTime?(DateTime.Now);
							ePIRepository.UserFiles.Add(assetId);
							ePIRepository.Save();
						}
					}
					catch (Exception exception1)
					{
						exception = exception1;
						this.logServiceEvent(string.Concat("Could not retrieve documents. Error: ", exception.Message), EventLogEntryType.Error);
					}
				}
				documentTypes = documentTypes1;
			}
			catch (Exception exception2)
			{
				exception = exception2;
				this.logServiceEvent(string.Concat("Could not retrieve documents. Error: ", exception.Message), EventLogEntryType.Error);
				documentTypes = null;
			}
			return documentTypes;
		}

		private void sendConfirmationJVMarketerEmail(User user)
		{
			ConfirmationJVMarketingLenderBrokerEmail confirmationJVMarketingLenderBrokerEmail = new ConfirmationJVMarketingLenderBrokerEmail()
			{
				RecipientName = user.FullName,
				RecipientEmail = user.Username
			};
			this._email.Send(confirmationJVMarketingLenderBrokerEmail);
		}

		private void sendConfirmationJVMarketerEmailUsingHtml(User user, string path)
		{
			string str = File.ReadAllText(Path.Combine(this._viewsPath, "ConfirmationJVMarketingLenderBroker.htm"));
			str = str.Replace("@Model.RecipientName", user.FullName);
			IEPIFundEmailService ePIFundEmailService = this._email;
			IEPIFundEmailService ePIFundEmailService1 = this._email;
			string str1 = this._fromEmail;
			List<string> strs = new List<string>()
			{
				user.Username
			};
			List<string> strs1 = new List<string>()
			{
				path
			};
			ePIFundEmailService.Send(ePIFundEmailService1.CreateMailMessage(str1, "Signed JV Marketing Agreement Received", str, strs, strs1));
		}

		private void sendConfirmationPISellerAssetIsPublishedEmail(AssetDescriptionModel assetDes, string path, string username, string fullUserName)
		{
			string str = File.ReadAllText(Path.Combine(this._viewsPath, "ConfirmationPISellerAssetIsPublishedEmail.htm"));
			str = str.Replace("@Model.RecipientName", fullUserName);
			DateTime now = DateTime.Now;
			str = str.Replace("@Model.DatePublished", now.ToString("MM/dd/yyyy"));
			now = DateTime.Now;
			str = str.Replace("@Model.TimePublished", string.Concat(now.ToString("hh:mm tt"), " PST"));
			str = str.Replace("@Model.Email", username);
			str = str.Replace("@Model.To", fullUserName);
			int assetNumber = assetDes.AssetNumber;
			str = str.Replace("@Model.AssetNumber", assetNumber.ToString());
			str = str.Replace("@Model.AssetAddressOneLine", assetDes.AssetAddressOneLineFormattedString);
			str = str.Replace("@Model.AssetDescription", assetDes.Description);
			str = str.Replace("@Model.CorpOfficer", assetDes.CorporateOwnershipOfficer);
			str = str.Replace("@Model.VestingEntity", fullUserName);
			str = str.Replace("@Model.APN", assetDes.APNOneLine);
			IEPIFundEmailService ePIFundEmailService = this._email;
			IEPIFundEmailService ePIFundEmailService1 = this._email;
			string str1 = this._fromEmail;
			string str2 = string.Concat("IPA Seller Received for Asset #", assetDes.AssetNumber);
			List<string> strs = new List<string>()
			{
				username
			};
			List<string> strs1 = new List<string>()
			{
				path
			};
			ePIFundEmailService.Send(ePIFundEmailService1.CreateMailMessage(str1, str2, str, strs, strs1));
		}

		private void sendListingAgentLOI(LOI loi, List<string> docs, AssetDescriptionModel asset, User user)
		{
			NotificationToListingAgentBindingLOISubmittalEmail notificationToListingAgentBindingLOISubmittalEmail = new NotificationToListingAgentBindingLOISubmittalEmail()
			{
				RecipientEmail = loi.EmailAddress,
				RecipientName = user.FullName,
				ListingAgent = loi.To,
				ProjectName = asset.ProjectName,
				Description = asset.Description,
				Vesting = user.CompanyName,
				Principal = user.FullName,
				Attachments = new List<Attachment>()
			};
			docs.ForEach((string f) => notificationToListingAgentBindingLOISubmittalEmail.Attachments.Add(new Attachment(f)));
			this._email.Send(notificationToListingAgentBindingLOISubmittalEmail);
		}

		private void sendListingAgentLOIUsingHtml(LOI loi, List<string> docs, AssetDescriptionModel asset, User user)
		{
			string str = File.ReadAllText(Path.Combine(this._viewsPath, "NotificationToListingAgentBindingLOISubmittal.htm"));
			str = str.Replace("@Model.RecipientEmail", loi.EmailAddress);
			str = str.Replace("@Model.ListingAgent", loi.To);
			str = str.Replace("@Model.ProjectName", asset.ProjectName);
			str = str.Replace("@Model.Description", asset.Description);
			str = str.Replace("@Model.Vesting", user.CompanyName);
			str = str.Replace("@Model.Principal", user.FullName);
			str = str.Replace("@Model.Phone", loi.BusinessPhoneNumber);
			IEPIFundEmailService ePIFundEmailService = this._email;
			IEPIFundEmailService ePIFundEmailService1 = this._email;
			string str1 = this._fromEmail;
			List<string> strs = new List<string>()
			{
				loi.EmailAddress
			};
			ePIFundEmailService.Send(ePIFundEmailService1.CreateMailMessage(str1, "Signed LOI Received", str, strs, docs));
		}

		private void sendSignedAssetMDAConfirmationEmail(AssetDescriptionModel model, User user, string path)
		{
			ConfirmationSignedMDAEmail confirmationSignedMDAEmail = new ConfirmationSignedMDAEmail()
			{
				RecipientEmail = user.Username,
				RecipientName = user.FullName,
				AssetDescription = model.Description,
				AssetNumber = model.AssetNumber.ToString(),
				Attachments = new List<Attachment>()
			};
			if (File.Exists(path))
			{
				confirmationSignedMDAEmail.Attachments.Add(new Attachment(path));
			}
		}

		private void sendSignedAssetMDAConfirmationEmailUsingHtml(AssetDescriptionModel model, User user, string path)
		{
			string str = File.ReadAllText(Path.Combine(this._viewsPath, "ConfirmationSignedMDA.htm"));
			str = str.Replace("@Model.RecipientName", user.FullName);
			int assetNumber = model.AssetNumber;
			str = str.Replace("@Model.AssetNumber", assetNumber.ToString());
			str = str.Replace("@Model.AssetDescription", model.Description);
			str = str.Replace("@Model.RecipientName", user.FullName);
			str = str.Replace("@Model.CityStateFormattedString", model.CityStateFormattedString);
			IEPIFundEmailService ePIFundEmailService = this._email;
			IEPIFundEmailService ePIFundEmailService1 = this._email;
			string str1 = this._fromEmail;
			List<string> strs = new List<string>()
			{
				user.Username
			};
			List<string> strs1 = new List<string>()
			{
				path
			};
			ePIFundEmailService.Send(ePIFundEmailService1.CreateMailMessage(str1, "Signed Asset IPA Received", str, strs, strs1));
		}

		private void sendSignedAssetMDAConfirmationMultipleAssetsEmailUsingHtml(string locations, string assetNumbers, User user, string path)
		{
			string str = File.ReadAllText(Path.Combine(this._viewsPath, "ConfirmationSignedMDAMultipleAssets.htm"));
			str = str.Replace("@Model.RecipientName", user.FullName);
			str = str.Replace("@Model.AssetNumbers", assetNumbers);
			str = str.Replace("@Model.Locations", locations);
			str = str.Replace("@Model.RecipientName", user.FullName);
			IEPIFundEmailService ePIFundEmailService = this._email;
			IEPIFundEmailService ePIFundEmailService1 = this._email;
			string str1 = this._fromEmail;
			List<string> strs = new List<string>()
			{
				user.Username
			};
			List<string> strs1 = new List<string>()
			{
				path
			};
			ePIFundEmailService.Send(ePIFundEmailService1.CreateMailMessage(str1, "Signed Asset IPA Received", str, strs, strs1));
		}

		public void Start(EventLog log)
		{
			this._eventLog = log;
			this.logServiceEvent(string.Concat("VIEWS PATH: ", this._viewsPath), EventLogEntryType.Information);
			double num = 90000;
			if (ConfigurationManager.AppSettings["TimerInterval"] != null)
			{
				num = Convert.ToDouble(ConfigurationManager.AppSettings["TimerInterval"]);
			}
			this._timer = new Timer(num);
			this._timer.Elapsed += new ElapsedEventHandler(this._timer_Elapsed);
			this._timer.Start();
			this.ProcessSignedDocuments(null);
		}

		public void Stop()
		{
			this._timer.Stop();
			this._timer.Close();
			this._timer.Dispose();
		}

		public void TestSend()
		{
			User user = new User()
			{
				Username = "elizabeth.trambulo@inviewlabs.com",
				FirstName = "liz",
				LastName = "test"
			};
			this.sendConfirmationJVMarketerEmailUsingHtml(user, "C:\\data\\test.txt");
		}
	}
}