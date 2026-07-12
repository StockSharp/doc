# Testes com dados de mercado em tempo real

Os testes com dados de mercado em tempo real envolvem negociação com uma ligação real à bolsa (cotações em tempo real), mas sem colocar ordens reais na bolsa. Todas as ordens registadas são intercetadas, e a sua execução é emulada com base nos livros de ordens de mercado. Este tipo de teste pode ser útil, por exemplo, ao desenvolver um simulador de negociação ou ao verificar um algoritmo de negociação durante um curto período com cotações reais.

Para emular a negociação com dados reais, é necessário usar [RealTimeEmulationTrader\<TAdapter\>](xref:StockSharp.Algo.Testing.RealTimeEmulationTrader`1), que encapsula um conector de sistema de negociação específico ([Binance](../connectors/crypto_exchanges/binance.md), [Interactive Brokers](../connectors/stock_market/interactive_brokers.md), etc.).

## Criar um conector de emulação

Para criar um conector de emulação, crie primeiro um conector normal para receber dados de mercado e, em seguida, crie um conector de emulação baseado nele:

```csharp
// Criar um conector normal para receber dados de mercado
private readonly Connector _realConnector = new();

// Criar um conector de emulação
_emuConnector = new RealTimeEmulationTrader<IMessageAdapter>(_realConnector.Adapter, _realConnector, _emuPf, false);

// Configurar parâmetros de emulação
var settings = _emuConnector.EmulationAdapter.Emulator.Settings;
settings.TimeZone = TimeHelper.Est;
settings.ConvertTime = true;
```

Para emulação de transações, é necessário usar uma carteira especial:

```csharp
private readonly Portfolio _emuPf = Portfolio.CreateSimulator();
```

## Subscrever eventos

Tal como um conector normal, o conector de emulação gera eventos ao receber dados de mercado e executar transações:

```csharp
// Subscrever eventos do conector
_emuConnector.Connected += () =>
{
	// atualizar etiquetas da gui
	this.GuiAsync(() => { ChangeConnectStatus(true); });
};

_emuConnector.Disconnected += () =>
{
	// atualizar etiquetas da gui
	this.GuiAsync(() => { ChangeConnectStatus(false); });
};

_emuConnector.ConnectionError += error => this.GuiAsync(() =>
{
	// atualizar etiquetas da gui
	ChangeConnectStatus(false);
	MessageBox.Show(this, error.ToString(), LocalizedStrings.ErrorConnection);
});

_emuConnector.OrderBookReceived += OnDepth;
_emuConnector.PositionReceived += (sub, p) => PortfolioGrid.Positions.TryAdd(p);
_emuConnector.OwnTradeReceived += (s, t) => TradeGrid.Trades.TryAdd(t);
_emuConnector.OrderReceived += (s, o) =>
{
	if (!_fistTimeOrders.Add(o))
		return;

	_bufferOrders.Add(o);
	OrderGrid.Orders.Add(o);
};

// Subscrever erros de registo de ordens
_emuConnector.OrderRegisterFailReceived += (s, f) => OrderGrid.AddRegistrationFail(f);

_emuConnector.CandleReceived += (s, candle) =>
{
	if (s == _candlesSubscription)
		_buffer.Add(candle);
};
```

## Subscrever dados de mercado

Para trabalhar com dados de mercado, é necessário subscrever os tipos de dados adequados:

```csharp
// Subscrever livros de ordens, ticks e Level1 para o conector de emulação
_emuConnector.Subscribe(new(DataType.MarketDepth, security));
_emuConnector.Subscribe(new(DataType.Ticks, security));
_emuConnector.Subscribe(new(DataType.Level1, security));

// Subscrever livros de ordens para o conector real (necessário para emulação)
_realConnector.Subscribe(new(DataType.MarketDepth, security));

// Subscrever candles
_candlesSubscription = new(CandleDataTypeEdit.DataType, security)
{
	From = DateTimeOffset.UtcNow - TimeSpan.FromDays(10),
};
_emuConnector.Subscribe(_candlesSubscription);
```

## Registo e gestão de ordens

As ordens são registadas através do conector de emulação de forma semelhante a um conector normal:

```csharp
// Registo de ordem
_emuConnector.RegisterOrder(order);

// Cancelamento de ordem
_emuConnector.CancelOrder(order);

// Substituição de ordem
_emuConnector.ReRegisterOrder(order, newPrice, order.Balance);
```

## Configurar parâmetros de emulação

Pode usar a propriedade [MarketEmulatorSettings](xref:StockSharp.Algo.Testing.MarketEmulatorSettings) para configurar parâmetros de emulação:

```csharp
var settings = _emuConnector.EmulationAdapter.Emulator.Settings;

// Definir fuso horário
settings.TimeZone = TimeHelper.Est;

// Converter hora
settings.ConvertTime = true;

// Executar ordens ao tocar no preço
settings.MatchOnTouch = false;

// Emular latência de execução de ordens
settings.Latency = TimeSpan.FromMilliseconds(100);
```

## Exemplo de interface

O exemplo SampleRealTimeEmulation demonstra a capacidade de apresentar simultaneamente dados do conector real e do conector de emulação:

![Exemplo de emulação em tempo real](../../../images/sample_realtime_emulation.png)

A interface da aplicação contém os seguintes elementos:
- Gráficos para apresentar candles e ordens
- Tabelas de ordens e transações próprias
- Livros de ordens reais de mercado e de emulação
- Controlos para criar e cancelar ordens

## Vantagens e limitações

Os testes com dados de mercado em tempo real têm as seguintes vantagens:
- Uso de dados de mercado reais sem riscos financeiros
- Teste de algoritmos em condições muito próximas da negociação real
- Possibilidade de comparar resultados com o mercado real em tempo real

Limitações:
- A velocidade de teste é limitada pela taxa dos dados reais
- Impossibilidade de testar em períodos históricos
- Dependência da qualidade e completude dos dados de mercado recebidos
