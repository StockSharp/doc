# Инициализация адаптера Quik Lua

Код ниже демонстрирует как инициализировать [LuaFixMarketDataMessageAdapter](xref:StockSharp.Quik.Lua.LuaFixMarketDataMessageAdapter) и [LuaFixTransactionMessageAdapter](xref:StockSharp.Quik.Lua.LuaFixTransactionMessageAdapter) и передать их в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var luaFixMarketDataMessageAdapter = new LuaFixMarketDataMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "localhost:5001".To<EndPoint>(),
    Login = "quik",
    Password = "quik".To<SecureString>(),
};
var luaFixTransactionMessageAdapter  = new LuaFixTransactionMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "localhost:5001".To<EndPoint>(),
    Login = "quik",
    Password = "quik".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(luaFixMarketDataMessageAdapter);
Connector.Adapter.InnerAdapters.Add(luaFixTransactionMessageAdapter);
...	
							
```

## См. также

[Окно настройки подключений](API_UI_ConnectorWindow.md)
