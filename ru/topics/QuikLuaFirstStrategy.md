# Инициализация адаптера Quik Lua

Код ниже демонстрирует как инициализировать [LuaFixMarketDataMessageAdapter](../api/StockSharp.Quik.Lua.LuaFixMarketDataMessageAdapter.html) и [LuaFixTransactionMessageAdapter](../api/StockSharp.Quik.Lua.LuaFixTransactionMessageAdapter.html) и передать их в [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var luaFixMarketDataMessageAdapter \= new LuaFixMarketDataMessageAdapter(Connector.TransactionIdGenerator)
{
	Address \= "localhost:5001".To\<EndPoint\>(),
    Login \= "quik",
    Password \= "quik".To\<SecureString\>(),
};
var luaFixTransactionMessageAdapter  \= new LuaFixTransactionMessageAdapter(Connector.TransactionIdGenerator)
{
	Address \= "localhost:5001".To\<EndPoint\>(),
    Login \= "quik",
    Password \= "quik".To\<SecureString\>(),
};
Connector.Adapter.InnerAdapters.Add(luaFixMarketDataMessageAdapter);
Connector.Adapter.InnerAdapters.Add(luaFixTransactionMessageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
