# Registo em estratégias

Em StockSharp, a classe [Strategy](xref:StockSharp.Algo.Strategies.Strategy) herda de [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver), o que permite usar ferramentas integradas para registar todas as ações e eventos que ocorrem durante a operação de uma estratégia de negociação.

## Níveis de registo

StockSharp suporta os seguintes níveis de registo (listados por ordem crescente de importância):

1. Verbose - o nível de registo mais detalhado para rastreamento
2. Debug - mensagens para depuração
3. Info - mensagens informativas regulares
4. Warning - avisos sobre problemas potenciais
5. Error - mensagens de erro

## Métodos de registo na estratégia

A estratégia fornece os seguintes métodos para escrever mensagens no log:

### LogVerbose

O método [LogVerbose](xref:Ecng.Logging.BaseLogReceiver.LogVerbose(System.String,System.Object[])) destina-se a registar mensagens detalhadas para rastreamento:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	LogVerbose("Estratégia iniciada com parâmetros: SMA longa={0}, SMA curta={1}", LongSmaLength, ShortSmaLength);

	// ...
}
```

### LogDebug

O método [LogDebug](xref:Ecng.Logging.BaseLogReceiver.LogDebug(System.String,System.Object[])) é usado para mensagens de depuração:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	LogDebug("Processando vela: {0}, Abertura={1}, Fechamento={2}, Máxima={3}, Mínima={4}, Volume={5}",
		candle.OpenTime, candle.OpenPrice, candle.ClosePrice, candle.HighPrice, candle.LowPrice, candle.TotalVolume);

	// ...
}
```

### LogInfo

O método [LogInfo](xref:Ecng.Logging.BaseLogReceiver.LogInfo(System.String,System.Object[])) é usado para mensagens informativas regulares:

```cs
private void CalculateSignal(decimal shortSma, decimal longSma)
{
	bool isShortGreaterThanLong = shortSma > longSma;

	LogInfo("Sinal: {0}, SMA curta={1}, SMA longa={2}",
		isShortGreaterThanLong ? "Buy" : "Sell", shortSma, longSma);

	// ...
}
```

### LogWarning

O método [LogWarning](xref:Ecng.Logging.BaseLogReceiver.LogWarning(System.String,System.Object[])) é usado para registar avisos:

```cs
public void RegisterOrder(Order order)
{
	if (order.Volume <= 0)
	{
		LogWarning("Tentativa de registrar uma ordem com volume inválido: {0}", order.Volume);
		return;
	}

	// ...
}
```

### LogError

O método [LogError](xref:Ecng.Logging.BaseLogReceiver.LogError(System.String,System.Object[])) é usado para registar mensagens de erro:

```cs
try
{
	// Algumas ações
}
catch (Exception ex)
{
	LogError("Erro ao executar a operação: {0}", ex.Message);
	Stop();
}
```

Existe também uma sobrecarga [LogError](xref:Ecng.Logging.BaseLogReceiver.LogError(System.Exception)) que aceita diretamente uma exceção:

```cs
try
{
	// Algumas ações
}
catch (Exception ex)
{
	LogError(ex);
	Stop();
}
```

## Configurar o nível de registo

A classe [Strategy](xref:StockSharp.Algo.Strategies.Strategy) contém uma propriedade [LogLevel](xref:Ecng.Logging.ILogSource.LogLevel) que determina que mensagens serão escritas no log:

```cs
// Definir o nível de log da estratégia
strategy.LogLevel = LogLevels.Info;
```

Com o nível de registo selecionado, apenas as mensagens desse nível e de níveis superiores serão registadas. Por exemplo, se `LogLevels.Info` estiver definido, as mensagens Verbose e Debug serão ignoradas.

## Parâmetro LogLevel

Para configurar convenientemente o nível de registo no construtor da estratégia, pode adicionar um parâmetro:

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<LogLevels> _logLevel;

	public SmaStrategy()
	{
		_logLevel = Param(nameof(LogLevel), LogLevels.Info)
					.SetDisplay("Nível de log", "Nível de detalhe das mensagens de log", "Configurações de log");
	}

	public override LogLevels LogLevel
	{
		get => _logLevel.Value;
		set => _logLevel.Value = value;
	}

	// ...
}
```

## Exemplos de Utilização numa Estratégia Real

### Registar o Início e a Paragem da Estratégia

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	LogInfo("Estratégia {0} iniciada em {1}. Instrumento: {2}, Portfólio: {3}",
		Name, time, Security?.Code, Portfolio?.Name);

	// ...
}

protected override void OnStopped()
{
	LogInfo("Estratégia {0} parada. Posição: {1}, P&L: {2}",
		Name, Position, PnL);

	base.OnStopped();
}
```

### Registar Negócios

```cs
protected override void OnNewMyTrade(MyTrade trade)
{
	LogInfo("{0} {1} {2} ao preço {3}. Volume: {4}",
		trade.Order.Direction == Sides.Buy ? "Comprado" : "Vendido",
		trade.Order.Security.Code,
		trade.Order.Type,
		trade.Trade.Price,
		trade.Trade.Volume);

	base.OnNewMyTrade(trade);
}
```

### Registar Erros de Registo de Ordens

```cs
protected override void OnOrderRegisterFailed(OrderFail fail, bool calcRisk)
{
	LogError("Erro de registro da ordem {0}: {1}",
		fail.Order.TransactionId, fail.Error.Message);

	base.OnOrderRegisterFailed(fail, calcRisk);
}
```

## Ligar ouvintes de registo

Para receber mensagens de uma estratégia, ligue ouvintes através de [LogManager](xref:Ecng.Logging.LogManager):

```cs
var logManager = new LogManager();

// Gravar em arquivo
var fileListener = new FileLogListener("{0}_{1:00}_{2:00}.txt".Put(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
logManager.Listeners.Add(fileListener);

// Enviar e-mail
var emailListener = new EmailLogListener("from@stocksharp.com", "to@stocksharp.com");
emailListener.Filters.Add(msg => msg.Level == LogLevels.Error);
logManager.Listeners.Add(emailListener);

// Adicionar a estratégia como fonte de log
logManager.Sources.Add(strategy);
```

## Visualizar registos

As mensagens escritas no log da estratégia podem ser visualizadas:

1. No programa [Designer](../../designer.md), no painel "Registos"
2. Em ficheiros de log, se [FileLogListener](xref:Ecng.Logging.FileLogListener) estiver configurado
3. Na interface de utilizador através de [LogControl](xref:StockSharp.Xaml.LogControl), se [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) for usado

## Ver Também

[Registo](../logging.md)
[Componente LogControl](../graphical_user_interface/logging/log_panel.md)
