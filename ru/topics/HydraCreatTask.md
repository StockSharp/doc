# Создание утилит

Программа [S\#.Data](Hydra.md) построена с возможностью создания собственных расширений. Для добавления собственного расширения необходимо скопировать dll в подпапку **Plugins** где установлена [S\#.Data](Hydra.md). Ниже представлен процесс создания утилиты резервирования имеющихся данных (она входит стандартно и представлена в качестве обучающего материала). 

Каждая утилита должна реализовать интерфейс [IHydraTask](../api/StockSharp.Hydra.Core.IHydraTask.html) (или наследоваться от классов [BaseHydraTask](../api/StockSharp.Hydra.Core.BaseHydraTask.html) или [ConnectorHydraTask\`1](../api/StockSharp.Hydra.Core.ConnectorHydraTask`1.html)): 

```cs
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.IO;
	using System.Linq;
	using System.Security;
	using Amazon;
	using Ecng.Backup;
	using Ecng.Backup.Amazon;
	using Ecng.Backup.Yandex;
	using Ecng.Common;
	using Ecng.ComponentModel;
	using Ecng.Serialization;
	using StockSharp.Algo;
	using StockSharp.Algo.Storages;
	using StockSharp.Hydra.Core;
	using StockSharp.Localization;
	using StockSharp.Logging;
	using StockSharp.Messages;
	using DataType = StockSharp.Algo.DataType;
	enum BackupServices
	{
		AwsS3,
		AwsGlacier,
		Yandex,
	}
	[DisplayNameLoc(LocalizedStrings.BackupKey)]
	[DescriptionLoc(LocalizedStrings.BackupDescriptionKey)]
	[Doc("5a056352-64c7-41ea-87a8-2e112935e3b9.htm")]
	[Icon("backup_logo.png")]
	[MessageAdapterCategory(MessageAdapterCategories.Tool)]
	class BackupTask : BaseHydraTask
	{
		private const string _sourceName = LocalizedStrings.BackupKey;
		public BackupTask()
		{
			Offset = 1;
			StartFrom = new DateTime(2000, 1, 1);
			Interval = TimeSpan.FromDays(1);
			Service = BackupServices.AwsS3;
			Address = RegionEndpoint.USEast1.SystemName;
			ServiceRepo = "stocksharp";
			Login = string.Empty;
			Password = new SecureString();
		}
		[Display(
			ResourceType = typeof(LocalizedStrings),
			Name = LocalizedStrings.Str3427Key,
			Description = LocalizedStrings.Str3427Key + LocalizedStrings.Dot,
			GroupName = _sourceName,
			Order = 0)]
		public BackupServices Service { get; set; }
		[Display(
			ResourceType = typeof(LocalizedStrings),
			Name = LocalizedStrings.AddressKey,
			Description = LocalizedStrings.ServerAddressKey + LocalizedStrings.Dot,
			GroupName = _sourceName,
			Order = 1)]
		public string Address { get; set; }
		[Display(
			ResourceType = typeof(LocalizedStrings),
			Name = LocalizedStrings.Str1405Key,
			Description = LocalizedStrings.Str1405Key + LocalizedStrings.Dot,
			GroupName = _sourceName,
			Order = 2)]
		public string ServiceRepo { get; set; }
		[Display(
			ResourceType = typeof(LocalizedStrings),
			Name = LocalizedStrings.LoginKey,
			Description = LocalizedStrings.LoginKey + LocalizedStrings.Dot,
			GroupName = _sourceName,
			Order = 3)]
		public string Login { get; set; }
		[Display(
			ResourceType = typeof(LocalizedStrings),
			Name = LocalizedStrings.PasswordKey,
			Description = LocalizedStrings.PasswordKey + LocalizedStrings.Dot,
			GroupName = _sourceName,
			Order = 4)]
		public SecureString Password { get; set; }
		[Display(
			ResourceType = typeof(LocalizedStrings),
			Name = LocalizedStrings.Str2282Key,
			Description = LocalizedStrings.Str3779Key,
			GroupName = _sourceName,
			Order = 5)]
		public DateTime StartFrom { get; set; }
		[Display(
			ResourceType = typeof(LocalizedStrings),
			Name = LocalizedStrings.Str2284Key,
			Description = LocalizedStrings.Str3778Key,
			GroupName = _sourceName,
			Order = 6)]
		public int Offset { get; set; }
		public override void Save(SettingsStorage storage)
		{
			base.Save(storage);
			storage.SetValue(nameof(Service), Service);
			storage.SetValue(nameof(Address), Address);
			storage.SetValue(nameof(ServiceRepo), ServiceRepo);
			storage.SetValue(nameof(Login), Login);
			storage.SetValue(nameof(Password), Password);
			storage.SetValue(nameof(StartFrom), StartFrom);
			storage.SetValue(nameof(Offset), Offset);
		}
		public override void Load(SettingsStorage storage)
		{
			base.Load(storage);
			Service = storage.GetValue(nameof(Service), Service);
			Address = storage.GetValue(nameof(Address), Address);
			ServiceRepo = storage.GetValue(nameof(ServiceRepo), ServiceRepo);
			Login = storage.GetValue(nameof(Login), Login);
			Password = storage.GetValue(nameof(Password), Password);
			StartFrom = storage.GetValue(nameof(StartFrom), StartFrom);
			Offset = storage.GetValue(nameof(Offset), Offset);
		}
		public override IEnumerable<DataType> SupportedDataTypes => Enumerable.Empty<DataType>();
```

Важным этапом создания утилиты является реализация метода [BaseHydraTask.OnProcess](../api/StockSharp.Hydra.Core.BaseHydraTask.OnProcess.html), в котором реализуется логика утилиты. В случае реализации резервирования, вся логика находится внутри [S\#.API](StockSharpAbout.md): 

```cs
	[	
		protected override TimeSpan OnProcess()
		{
			using (var service = CreateService())
			{
				var hasSecurities = false;
				this.AddInfoLog(LocalizedStrings.Str2306Params.Put(StartFrom));
				var startDate = StartFrom;
				var endDate = DateTime.Today - TimeSpan.FromDays(Offset);
				var allDates = startDate.Range(endDate, TimeSpan.FromDays(1)).ToArray();
				var pathEntry = ToEntry(new DirectoryInfo(Drive.Path));
				var workingSecurities = GetWorkingSecurities().ToArray();
				foreach (var date in allDates)
				{
					foreach (var security in workingSecurities)
					{
						hasSecurities = true;
						if (!CanProcess())
							break;
						var dateEntry = new BackupEntry
						{
							Name = LocalMarketDataDrive.GetDirName(date),
							Parent = new BackupEntry
							{
								Parent = new BackupEntry
								{
									Name = security.Security.Id.Substring(0, 1),
									Parent = pathEntry
								},
								Name = security.Security.Id,
							}
						};
						var dataTypes = Drive.GetAvailableDataTypes(security.Security.ToSecurityId(), StorageFormat);
						foreach (var dataType in dataTypes)
						{
							var storage = StorageRegistry.GetStorage(security.Security, dataType.MessageType, dataType.Arg, Drive, StorageFormat);
							var drive = storage.Drive;
							var stream = drive.LoadStream(date);
							if (stream == Stream.Null)
								continue;
							var entry = new BackupEntry
							{
								Name = LocalMarketDataDrive.GetFileName(dataType.MessageType, dataType.Arg, StorageFormat), // + LocalMarketDataDrive.GetExtension(StorageFormats.Binary),
								Parent = dateEntry,
							};
							var token = service.Upload(entry, stream, p => { });
							if (token.IsCancellationRequested)
							{
								this.AddWarningLog(LocalizedStrings.Cancelling);
								return TimeSpan.MaxValue;
							}
							this.AddInfoLog(LocalizedStrings.Str1580Params, GetPath(entry));
						}
					}
					if (CanProcess())
					{
						StartFrom += TimeSpan.FromDays(1);
						this.SaveSettings();
					}
				}
				if (!hasSecurities)
				{
					this.AddWarningLog(LocalizedStrings.Str2292);
					return TimeSpan.MaxValue;
				}
				if (CanProcess())
					this.AddInfoLog(LocalizedStrings.Str2300);
				return base.OnProcess();
			}
		}
		private static string GetPath(BackupEntry entry)
		{
			if (entry == null)
				return null;
			return GetPath(entry.Parent) + "/" + entry.Name;
		}
		private static BackupEntry ToEntry(DirectoryInfo di)
		{
			// is a disk
			if (di.Parent == null)
				return null;
			return new BackupEntry
			{
				Name = di.Name,
				Parent = di.Parent != null ? ToEntry(di.Parent) : null
			};
		}
		public override bool CanTestConnect => true;
		public override void TestConnect(Action<Exception> connectionChanged)
		{
			if (connectionChanged == null)
				throw new ArgumentNullException(nameof(connectionChanged));
			try
			{
				using (var service = CreateService())
					service.Find(null, string.Empty);
				connectionChanged(null);
			}
			catch (Exception ex)
			{
				connectionChanged(ex);
			}
		}
		private IBackupService CreateService()
		{
			switch (Service)
			{
				case BackupServices.AwsS3:
					return new AmazonS3Service(Address, ServiceRepo, Login, Password.To<string>());
				case BackupServices.AwsGlacier:
					return new AmazonGlacierService(Address, ServiceRepo, Login, Password.To<string>());
				case BackupServices.Yandex:
					return new YandexDiskService();
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
```
