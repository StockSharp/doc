# RemoteManager

[S\#](StockSharpAbout.md) provides the ability to remotely manage strategies running on remote servers using the **RemoteManagerControl** module.

To use the module, it must be added as a WPF element to the screen form.

```none
...
xmlns:xamlRemote= "clr-namespace:StockSharp.RemoteManager;assembly=StockSharp.RemoteManager"	  				
...
<xamlRemote:RemoteManagerControl x:Name="RemoteManagerControl"/>
...
	  				
```

The module operates in two modes \- server and client, or in two modes simultaneously.

To initialize the client module, the InitRemoteManagerClient method must be called and the [Connector](xref:StockSharp.Algo.Connector) passed to it.

```cs
	...
RemoteManagerControl.InitRemoteManagerClient(Connector);
	...	
		
```

To initialize the server module, the **InitRemoteManagerServer** method must be called and passed to it the **ObservableDictionary** containing a list of available strategies, and the **IList** containing strategies launched for trading.

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

RemoteManagerControl is implemented in [Shell](Shell.md). For a detailed description of how to use it, see [RemoteManager](Shell_RemoteManager.md).

## Recommended content

[Shell](Shell.md)
