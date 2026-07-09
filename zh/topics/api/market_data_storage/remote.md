# 使用远程存储

## 介绍

除了本地存储，API 还提供使用远程市场数据存储的功能。当在[服务器模式](../../hydra/server_mode/settings.md)下使用 Hydra 或连接到 [Hydra 服务器](../../hydra_server.md) 时，这尤其有用。

## 连接到远程存储

要使用远程存储，请使用 [RemoteMarketDataDrive](xref:StockSharp.Algo.Storages.RemoteMarketDataDrive) 类。

```cs
// 创建 RemoteMarketDataDrive
var remoteDrive = new RemoteMarketDataDrive(RemoteMarketDataDrive.DefaultAddress, new FixMessageAdapter(new IncrementalIdGenerator()))
{
	Credentials = { Email = "hydra_user", Password = "hydra_user".To<SecureString>() }
};

// 此代码创建 RemoteMarketDataDrive 实例以连接远程存储。
// 它使用默认地址和 FixMessageAdapter 进行通信。
// 设置凭据用于认证。
```

## 正在加载交易品种信息

在加载市场数据之前，您需要获取可用工具的信息。

```cs
// 加载交易品种信息
var exchangeInfoProvider = new InMemoryExchangeInfoProvider();
remoteDrive.LookupSecurities(Extensions.LookupAllCriteriaMessage, registry.Securities,
	s => securityStorage.Save(s.ToSecurity(exchangeInfoProvider), false), () => false,
	(c, t) => Console.WriteLine($"Downloaded [{c}]/[{t}]"));

var securities = securityStorage.LookupAll();

// 此代码从远程存储加载所有可用交易品种的信息。
// 加载的交易品种会保存到本地存储并输出到控制台。
```

## 正在加载市场数据

在获取了交易品种信息后，您可以继续加载市场数据。

```cs
// 加载市场数据
foreach (var dataType in remoteDrive.GetAvailableDataTypes(secId, format))
{
	var localStorage = storageRegistry.GetStorage(secId, dataType.MessageType, dataType.Arg, localDrive, format);
	var remoteStorage = remoteDrive.GetStorageDrive(secId, dataType, format);

	// ... (data loading code)
}

// 此循环遍历指定交易品种的所有可用数据类型。
// For each data type, a local storage is created and remote storage is accessed.
```

## 本地保存数据

加载的数据可以本地保存以便进一步使用。

```cs
// 本地保存数据
foreach (var dateTime in dates)
{
	using (var stream = remoteStorage.LoadStream(dateTime))
	{
		if (stream == Stream.Null)
			continue;

		localStorage.Drive.SaveStream(dateTime, stream);
	}

	// ... (data output code)
}

// 此代码从远程存储加载每个日期的数据并保存到本地存储。
```

## 使用数据进行测试

已加载并本地保存的数据可以用于使用 [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) 测试交易策略。

```cs
// 使用 HistoryEmulationConnector
var connector = new HistoryEmulationConnector(secProvider, new[] { pf }, new StorageRegistry { DefaultDrive = remoteDrive });
```

## 获取可用日期范围

```cs
// 处理各种数据类型
foreach (var dataType in remoteDrive.GetAvailableDataTypes(secId, format))
{
	// ... (data processing code)

	// 错误处理和日志记录
	Console.WriteLine($"Remote {dataType}: {remoteStorage.Dates.FirstOrDefault()}-{remoteStorage.Dates.LastOrDefault()}");
	Console.WriteLine($"{dataType}={dateTime}");
}
```

## 结论

该API的远程市场数据存储功能提供了获取和使用历史数据的灵活能力。这使得可以使用通过[Hydra服务器](../../hydra_server.md)提供的广泛数据集进行交易策略的高效测试和市场分析。
