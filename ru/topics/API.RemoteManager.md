# S\#.RemoteManager

[S\#](StockSharpAbout.md) предоставляет возможность удаленного управления стратегиями, запущенных на удаленных серверах, с помощью модуля **RemoteManagerControl**.

Для использования модуля его необходимо добавить как WPF элемент на экранную форму.

```none
...
xmlns:xamlRemote= "clr-namespace:StockSharp.RemoteManager;assembly=StockSharp.RemoteManager"	  				
...
<xamlRemote:RemoteManagerControl x:Name="RemoteManagerControl"/>
...
	  				
```

Модуль работает в двух режимах \- серверный и клиентский, или в двух одновременно.

Для инициализации клиентского модуля необходимо вызвать метод **InitRemoteManagerClient** и передать в него [Connector](xref:StockSharp.Algo.Connector).

```cs
	...
RemoteManagerControl.InitRemoteManagerClient(Connector);
	...	
		
```

Для инициализации серверного модуля необходимо вызвать метод **InitRemoteManagerServer** и передать в него **ObservableDictionary**, содержащий список доступных стратегий, и **IList** содержащий запущенные на торговлю стратегии.

```cs
	...
	//---------------------------------------------------------------------
	DictionaryStrategies = new ObservableDictionary<Guid, Strategy>
	{
		{ new SmaStrategy().GetTypeId(), new SmaStrategy() },
		{ new StairsTrendStrategy().GetTypeId(), new StairsTrendStrategy() },
		{ new StairsCountertrendStrategy().GetTypeId(), new StairsCountertrendStrategy() }
	};
	//---------------------------------------------------------------------
	...	
	RemoteManagerControl.InitRemoteManagerServer(DictionaryStrategies, RealtimeLayoutGroup.Strategies, LogManager);
		
```

RemoteManagerControl реализован в [S\#.Shell](Shell.md). Подробное описание как им пользоваться можно посмотреть в пункте [RemoteManager](Shell_RemoteManager.md).

## См. также

[S\#.Shell](Shell.md)
