using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;

namespace Inview.Epi.EpiFund.Business
{
	public class AssetVideoServiceManager : IAssetVideoServiceManager
	{
		private EventLog _eventLog;

		private IEPIContextFactory _factory;

		private string _ffmpegPath;

		private Timer _timer;

		public AssetVideoServiceManager(IEPIContextFactory factory)
		{
			this._factory = factory;
		}

		private void _timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this._timer.Stop();
			this.ConvertVideos();
			this._timer.Start();
		}

		public void ConvertVideos()
		{
			string str;
			Exception exception;
			string[] directories;
			int i;
			string[] files;
			int j;
			try
			{
				this._ffmpegPath = ConfigurationManager.AppSettings["ffmpegPath"];
				List<string> list = (
					from x in this._factory.Create().AssetVideos
					select x.FilePath.ToLower()).ToList<string>();
				directories = Directory.GetDirectories(ConfigurationManager.AppSettings["VideoPath"]);
				for (i = 0; i < (int)directories.Length; i++)
				{
					files = Directory.GetFiles(directories[i]);
					for (j = 0; j < (int)files.Length; j++)
					{
						str = files[j];
						string lower = Path.GetFileName(Path.ChangeExtension(str, ".mp4")).ToLower();
						if (!list.Any<string>((string x) => x == lower))
						{
							try
							{
								File.Delete(str);
							}
							catch
							{
							}
						}
					}
				}
			}
			catch (Exception exception1)
			{
				exception = exception1;
				this.logServiceEvent(string.Concat("Error deleting stranded videos. Error: ", exception.Message), EventLogEntryType.Error);
			}
			try
			{
				this.KillProcesses();
				if (!Process.GetProcessesByName("ffmpeg").Any<Process>())
				{
					directories = Directory.GetDirectories(ConfigurationManager.AppSettings["VideoPath"]);
					for (i = 0; i < (int)directories.Length; i++)
					{
						string[] strArrays = Directory.GetFiles(directories[i]);
						files = strArrays;
						for (j = 0; j < (int)files.Length; j++)
						{
							str = files[j];
							if (str.ToLower().Contains(".mp4"))
							{
								string str1 = Path.ChangeExtension(str, "webm");
								if (!strArrays.Contains<string>(str1))
								{
									StringBuilder stringBuilder = new StringBuilder("-i ");
									stringBuilder.Append("\"");
									stringBuilder.Append(str);
									stringBuilder.Append("\"");
									stringBuilder.Append(" ");
									stringBuilder.Append("\"");
									stringBuilder.Append(str1);
									stringBuilder.Append("\"");
									ProcessStartInfo processStartInfo = new ProcessStartInfo();
									Process process = new Process();
									processStartInfo.UseShellExecute = false;
									processStartInfo.CreateNoWindow = true;
									processStartInfo.FileName = this._ffmpegPath;
									processStartInfo.Arguments = stringBuilder.ToString();
									Process process1 = Process.Start(processStartInfo);
									try
									{
										process1.WaitForExit();
									}
									finally
									{
										if (process1 != null)
										{
											((IDisposable)process1).Dispose();
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception exception2)
			{
				exception = exception2;
				this.logServiceEvent(string.Concat("Error creating webms. Error: ", exception.Message), EventLogEntryType.Error);
			}
		}

		private void KillProcesses()
		{
			Process[] processesByName = Process.GetProcessesByName("ffmpeg");
			if (processesByName.Any<Process>())
			{
				Process[] processArray = processesByName;
				for (int i = 0; i < (int)processArray.Length; i++)
				{
					Process process = processArray[i];
					try
					{
						process.Kill();
					}
					catch (Exception exception1)
					{
						Exception exception = exception1;
						this.logServiceEvent(string.Concat("Error stopping ffmpeg process. Error: ", exception.Message), EventLogEntryType.Error);
					}
				}
			}
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

		public void Start(EventLog log)
		{
			this.logServiceEvent("Inside Asset Video Service Start Method", EventLogEntryType.Information);
			this._eventLog = log;
			double num = 60000;
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
		}
	}
}