# Работа с удаленным хранилищем

## Введение

В дополнение к локальному хранилищу, API предоставляет возможность работы с удаленным хранилищем маркет-данных. Это особенно полезно при использовании Hydra в [серверном режиме](../../hydra/server_mode/settings.md) или при подключении к [Hydra серверу](../../hydra_server.md).

## Подключение к удаленному хранилищу

Для работы с удаленным хранилищем используется класс [RemoteMarketDataDrive](xref:StockSharp.Algo.Storages.RemoteMarketDataDrive). 

```cs
// Создание RemoteMarketDataDrive
var remoteDrive = new RemoteMarketDataDrive(RemoteMarketDataDrive.DefaultAddress, new FixMessageAdapter(new IncrementalIdGenerator()))
{
	Credentials = { Email = "hydra_user", Password = "hydra_user".To<SecureString>() }
};

// Этот код создает экземпляр RemoteMarketDataDrive для подключения к удаленному хранилищу.
// Используется адрес по умолчанию и FixMessageAdapter для коммуникации.
// Также устанавливаются учетные данные для аутентификации.
```

## Загрузка информации о инструментах

Перед загрузкой маркет-данных, необходимо получить информацию о доступных инструментах.

```cs
// Загрузка информации о инструментах
var exchangeInfoProvider = new InMemoryExchangeInfoProvider();
remoteDrive.LookupSecurities(Extensions.LookupAllCriteriaMessage, registry.Securities,
	s => securityStorage.Save(s.ToSecurity(exchangeInfoProvider), false), () => false,
	(c, t) => Console.WriteLine($"Downloaded [{c}]/[{t}]"));

var securities = securityStorage.LookupAll();

// Этот код загружает информацию о всех доступных инструментах с удаленного хранилища.
// Загруженные инструменты сохраняются в локальное хранилище и выводятся в консоль.
```

## Загрузка маркет-данных

После получения информации об инструментах, можно приступить к загрузке маркет-данных.

```cs
// Загрузка маркет-данных
foreach (var dataType in remoteDrive.GetAvailableDataTypes(secId, format))
{
	var localStorage = storageRegistry.GetStorage(secId, dataType.MessageType, dataType.Arg, localDrive, format);
	var remoteStorage = remoteDrive.GetStorageDrive(secId, dataType, format);

	// ... (код загрузки данных)
}

// Этот цикл перебирает все доступные типы данных для указанного инструмента.
// Для каждого типа данных создается локальное хранилище и получается удаленное хранилище.
```

## Сохранение данных локально

Загруженные данные могут быть сохранены локально для дальнейшего использования.

```cs
// Сохранение данных локально
foreach (var dateTime in dates)
{
	using (var stream = remoteStorage.LoadStream(dateTime))
	{
		if (stream == Stream.Null)
			continue;

		localStorage.Drive.SaveStream(dateTime, stream);
	}

	// ... (код вывода данных)
}

// Этот код загружает данные за каждую дату из удаленного хранилища и сохраняет их в локальное хранилище.
```

## Использование данных для тестирования

Загруженные и сохраненные локально данные могут быть использованы для тестирования торговых стратегий с помощью [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector).

```cs
var connector = new HistoryEmulationConnector(secProvider, new[] { pf }, new StorageRegistry { DefaultDrive = remoteDrive });
```

## Получение диапазонов доступных дат

```cs
// Работа с различными типами данных
foreach (var dataType in remoteDrive.GetAvailableDataTypes(secId, format))
{
	// ... (код обработки каждого типа данных)

	// Обработка ошибок и логирование
	Console.WriteLine($"Remote {dataType}: {remoteStorage.Dates.FirstOrDefault()}-{remoteStorage.Dates.LastOrDefault()}");
	Console.WriteLine($"{dataType}={dateTime}");
}
```

## Заключение

Работа с удаленным хранилищем маркет-данных в API предоставляет гибкие возможности для получения и использования исторических данных. Это позволяет эффективно тестировать торговые стратегии и проводить анализ рынка, используя обширные наборы данных, доступные через [Hydra сервер](../../hydra_server.md).