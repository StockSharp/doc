# Google Finance

Along with using the [HistoryEmulationConnector](../api/StockSharp.Algo.Testing.HistoryEmulationConnector.html), data storage, the [IStorageRegistry](../api/StockSharp.Algo.Storages.IStorageRegistry.html), class provides an alternative mechanism for working with data sources for backtesting. This mechanism allows you to "directly" download data from the servers of historical information providers, in particular, from the Google server or use your own data sources. 

Consider the work of this mechanism on the example of getting data from the Google service. To work with Google, [S\#](StockSharpAbout.md) has a special class, [GoogleMessageAdapter](../api/StockSharp.Google.GoogleMessageAdapter.html), which allows you to receive candles, ticks and information about securities. 

### Testing with data downloaded from Google

Testing with data downloaded from Google

1. First you need to get information about the securities from the Google service. To do this, create a storage for securities (**GoogleSecurityStorage**) \- a class that implements the [ISecurityStorage](../api/StockSharp.Algo.Storages.ISecurityStorage.html). interface. The code for this class is in the *Samples\/Testing\/SampleHistoryTesting*. 
   1. Create security.

      ```none
      var security = new Security
      {
          Id = secid,
          Code = secCode,
          Board = board
      };
       
      ```
   2. Create an instance of the data loader class from Google:

      ```none
      // remove the old market data adapter and add a new one from 
      connector.Adapter.InnerAdapters.Remove(connector.MarketDataAdapter);
      connector.Adapter.InnerAdapters.Add(new CustomHistoryMessageAdapter(new GoogleMessageAdapter(connector.TransactionIdGenerator)));
      ```
