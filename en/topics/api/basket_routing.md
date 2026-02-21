# Adapter Routing

StockSharp supports simultaneous connections to multiple exchanges and brokers. The routing system (basket routing) manages which messages are directed to which adapters, ensuring transparent operation with multiple connections.

## General Architecture

When using multiple adapters, the connector automatically creates a basket that combines all connections. The router determines which adapter each specific message should be directed to -- market data subscriptions, transactions, portfolio requests, etc.

## AdapterRouter

The [IAdapterRouter](xref:StockSharp.Algo.IAdapterRouter) interface defines the logic for routing messages between adapters.

### Main Methods

| Method | Description |
|--------|-------------|
| `GetAdapters` | Returns a list of adapters suitable for processing the given message |
| `GetSubscriptionAdaptersAsync` | Asynchronously determines adapters for market data subscriptions |
| `GetPortfolioAdapter` | Returns the adapter bound to a specific portfolio |
| `TryGetOrderAdapter` | Finds the adapter through which an order was registered |
| `SetSecurityAdapter` | Binds an instrument to a specific adapter |
| `SetPortfolioAdapter` | Binds a portfolio to a specific adapter |

### Routing Priorities

The system determines the target adapter in the following priority order:

1. **Explicit specification** -- if an adapter is specified in the message via the `message.Adapter` property, that adapter is used.
2. **Instrument binding** -- mapping set via `SetSecurityAdapter`.
3. **Data type binding** -- adapters registered for a specific message type.
4. **Supported type filtering** -- adapters that support the given message type are selected.

### Configuring Routing

```cs
var router = connector.Adapter.InnerAdapters;

// Bind instrument to adapter for receiving tick data
router.SetSecurityAdapter(
    secId,
    DataType.Ticks,
    binanceAdapter
);

// Bind portfolio to adapter for transactions
router.SetPortfolioAdapter(
    "MyPortfolio",
    interactiveBrokersAdapter
);
```

## Managing Connections

### Connection States

Each adapter in the basket goes through standard connection states:

- **Disconnected** -- disconnected
- **Connecting** -- connection in progress
- **Connected** -- connected
- **Disconnecting** -- disconnection in progress

The basket aggregates the states of all nested adapters.

### Aggregation Parameters

The `ConnectDisconnectEventOnFirstAdapter` property determines when the basket is considered connected:

- `true` -- the connection event fires when the **first** adapter connects (default). Allows starting work without waiting for all connections.
- `false` -- the event fires only after **all** adapters have connected.

```cs
// Wait for all adapters to connect
connector.Adapter.InnerAdapters.ConnectDisconnectEventOnFirstAdapter = false;

connector.Connected += () =>
{
    Console.WriteLine("All adapters connected");
};

connector.Connect();
```

## Parent and Child Subscriptions

When working with multiple adapters, a single subscription can be split into multiple child subscriptions, each directed to its own adapter. The system automatically:

- Creates child subscriptions for each suitable adapter
- Aggregates responses before notifying the parent subscription
- Handles partial errors (if one adapter fails to subscribe, the others continue working)

### Multi-Exchange Example

```cs
// Adding adapters
connector.Adapter.InnerAdapters.Add(binanceAdapter);
connector.Adapter.InnerAdapters.Add(bybitAdapter);

connector.Connect();

// Subscribing to ticks -- will be automatically routed
// to all adapters supporting the given instrument
var subscription = new Subscription(DataType.Ticks, security);
connector.Subscribe(subscription);
```

## Pending Message Queue

If no adapter is connected when a message is sent, the message is placed in a pending queue (`IPendingMessageState`). When an adapter connects, all accumulated messages are automatically sent.

```cs
// Registering an order before connecting -- the order will be sent
// automatically after the connection is established
connector.RegisterOrder(order);
connector.Connect();
```

## Configuring Multiple Connections

### Programmatic Configuration

```cs
// Creating adapters
var binance = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
    Key = "<API_KEY>",
    Secret = "<API_SECRET>".Secure(),
};

var ib = new InteractiveBrokersMessageAdapter(connector.TransactionIdGenerator)
{
    Address = InteractiveBrokersMessageAdapter.DefaultAddress,
};

// Adding to the basket
connector.Adapter.InnerAdapters.Add(binance);
connector.Adapter.InnerAdapters.Add(ib);

// Configuring routing
connector.Adapter.InnerAdapters.SetPortfolioAdapter("BinancePortfolio", binance);
connector.Adapter.InnerAdapters.SetPortfolioAdapter("IBPortfolio", ib);

connector.Connect();
```

### Graphical Configuration

For visual connection configuration, use the graphical configuration component. See the [Graphical Configuration](connectors/graphical_configuration.md) section for details.

## Order Tracking

The router automatically tracks which adapter was used to register each order. When receiving order updates (state changes, trades), the system routes them through the same adapter:

```cs
// The order will be registered through the adapter bound to the portfolio
var order = new Order
{
    Security = security,
    Portfolio = portfolio,
    Side = Sides.Buy,
    Price = price,
    Volume = volume,
};

connector.RegisterOrder(order);

// Cancellation will go through the same adapter automatically
connector.CancelOrder(order);
```

## See Also

- [Connectors](connectors.md)
- [Graphical Configuration](connectors/graphical_configuration.md)
- [Position Management](positions.md)
