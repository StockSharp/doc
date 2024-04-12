# Подключение через FIX протокол

[Hydra](../../hydra.md) может использоваться в серверном режиме, в данном режиме можно удаленно подключиться к [Hydra](../../hydra.md) и получить имеющиеся данные в хранилище. Включение серверного режима [Hydra](../../hydra.md) описано в пункте [Настройки](settings.md).

Для подключения через [FIX протокол](../../api/connectors/common/fix_protocol.md) необходимо создать и настроить Fix подключение ([Инициализация адаптера FIX](../../api/connectors/common/fix_protocol/adapter_initialization_fix.md)).

```cs
...
private readonly Connector _connector = new Connector();
...
var marketDataAdapter = new FixMessageAdapter(Connector.TransactionIdGenerator)
{
    Address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5002),
    SenderCompId = "hydra_user",
    TargetCompId = "StockSharpHydraMD",
    Login = "hydra_user",
    Password = "qwerty".To<SecureString>(),
};
_connector.Adapter.InnerAdapters.Add(marketDataAdapter);
var transactionDataAdapter = new FixMessageAdapter(Connector.TransactionIdGenerator)
{
    Address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5002),
    SenderCompId = "hydra_user",
    TargetCompId = "StockSharpHydraMD",
    Login = "hydra_user",
    Password = "qwerty".To<SecureString>(),
};
_connector.Adapter.InnerAdapters.Add(transactionDataAdapter);
...
```

Подписаться на события получения данных и подключиться

```cs
...
_connector.Connected += () =>
{
    Console.WriteLine("_connector.Connected");
    _connector.LookupSecurities(new Security ());
    
    
};
_connector.Disconnected += () =>
{
    Console.WriteLine("_connector.Disconnected");
};
_connector.NewSecurity += security =>
{
    Console.Write("_connector.NewSecurity: ");
    Console.WriteLine(security);
    BuffeSecurity.Add(security);
    if (security.Id == seid)
    {
        _connector.SubscribeMarketDepth(security);
        _connector.SubscribeTrades(security);
        series.CandleType = typeof (TimeFrameCandle);
        series.Security = security;
        series.Arg = TimeSpan.FromMinutes(5);
        _connector.SubscribeCandles(series, DateTime.Today.Subtract(TimeSpan.FromDays(30)), DateTime.Now);
    }
};
_connector.NewTrade += trade =>
{
    Console.WriteLine("_connector.NewTrade");
    Console.WriteLine(trade);
};
_connector.MarketDepthChanged+= depth =>
{
    Console.WriteLine("_connector.MarketDepthChanged");
    Console.WriteLine(depth);
};
_connector.ConnectionError += error =>
{
    Console.WriteLine("_connector.ConnectionError");
    Console.WriteLine(error.Message);
};
_connector.Error += error =>
{
    Console.WriteLine("_connector.Error");
    Console.WriteLine(error.Message);
};
_connector.MarketDataSubscriptionFailed += (security, msg, error) =>
{
    Console.WriteLine("_connector.MarketDataSubscriptionFailed");
    Console.WriteLine(security);
    Console.WriteLine(msg);
    Console.WriteLine(error);
};
...
_connector.Connect();
...
```
