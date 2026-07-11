# Logging in Strategien

In StockSharp erbt die Klasse [Strategy](xref:StockSharp.Algo.Strategies.Strategy) von [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver). Dadurch können integrierte Werkzeuge verwendet werden, um alle Aktionen und Ereignisse zu protokollieren, die während der Ausführung einer Handelsstrategie auftreten.

## Logging-Stufen

StockSharp unterstützt die folgenden Logging-Stufen, aufgelistet in aufsteigender Wichtigkeit:

1. Verbose - die detaillierteste Logging-Stufe für Tracing
2. Debug - Meldungen für die Fehlersuche
3. Info - normale Informationsmeldungen
4. Warning - Warnungen vor potenziellen Problemen
5. Error - Fehlermeldungen

## Logging-Methoden in Strategy

Die Strategie stellt die folgenden Methoden zum Schreiben von Meldungen in das Log bereit:

### LogVerbose

Die Methode [LogVerbose](xref:Ecng.Logging.BaseLogReceiver.LogVerbose(System.String,System.Object[])) dient zum Aufzeichnen detaillierter Meldungen für Tracing:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	LogVerbose("Strategie mit Parametern gestartet: lange SMA={0}, kurze SMA={1}", LongSmaLength, ShortSmaLength);

	// ...
}
```

### LogDebug

Die Methode [LogDebug](xref:Ecng.Logging.BaseLogReceiver.LogDebug(System.String,System.Object[])) wird für Debug-Meldungen verwendet:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	LogDebug("Kerze wird verarbeitet: {0}, Open={1}, Close={2}, High={3}, Low={4}, Volumen={5}",
		candle.OpenTime, candle.OpenPrice, candle.ClosePrice, candle.HighPrice, candle.LowPrice, candle.TotalVolume);

	// ...
}
```

### LogInfo

Die Methode [LogInfo](xref:Ecng.Logging.BaseLogReceiver.LogInfo(System.String,System.Object[])) wird für normale Informationsmeldungen verwendet:

```cs
private void CalculateSignal(decimal shortSma, decimal longSma)
{
	bool isShortGreaterThanLong = shortSma > longSma;

	LogInfo("Signal: {0}, kurze SMA={1}, lange SMA={2}",
		isShortGreaterThanLong ? "Kauf" : "Verkauf", shortSma, longSma);

	// ...
}
```

### LogWarning

Die Methode [LogWarning](xref:Ecng.Logging.BaseLogReceiver.LogWarning(System.String,System.Object[])) wird zum Aufzeichnen von Warnungen verwendet:

```cs
public void RegisterOrder(Order order)
{
	if (order.Volume <= 0)
	{
		LogWarning("Versuch, eine Order mit ungültigem Volumen zu registrieren: {0}", order.Volume);
		return;
	}

	// ...
}
```

### LogError

Die Methode [LogError](xref:Ecng.Logging.BaseLogReceiver.LogError(System.String,System.Object[])) wird zum Aufzeichnen von Fehlermeldungen verwendet:

```cs
try
{
	// Einige Aktionen
}
catch (Exception ex)
{
	LogError("Fehler beim Ausführen der Operation: {0}", ex.Message);
	Stop();
}
```

Es gibt außerdem eine Überladung [LogError](xref:Ecng.Logging.BaseLogReceiver.LogError(System.Exception)), die direkt eine Ausnahme akzeptiert:

```cs
try
{
	// Einige Aktionen
}
catch (Exception ex)
{
	LogError(ex);
	Stop();
}
```

## Konfigurieren der Logging-Stufe

Die Klasse [Strategy](xref:StockSharp.Algo.Strategies.Strategy) enthält die Eigenschaft [LogLevel](xref:Ecng.Logging.ILogSource.LogLevel), die bestimmt, welche Meldungen in das Log geschrieben werden:

```cs
// Logging-Stufe für die Strategie festlegen
strategy.LogLevel = LogLevels.Info;
```

Bei der ausgewählten Logging-Stufe werden nur Meldungen dieser Stufe und höherer Stufen aufgezeichnet. Wenn beispielsweise `LogLevels.Info` gesetzt ist, werden Verbose- und Debug-Meldungen ignoriert.

## LogLevel-Parameter

Für eine bequeme Konfiguration der Logging-Stufe im Strategiekonstruktor können Sie einen Parameter hinzufügen:

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<LogLevels> _logLevel;

	public SmaStrategy()
	{
		_logLevel = Param(nameof(LogLevel), LogLevels.Info)
					.SetDisplay("Logging-Stufe", "Detailstufe der Logmeldung", "Logging-Einstellungen");
	}

	public override LogLevels LogLevel
	{
		get => _logLevel.Value;
		set => _logLevel.Value = value;
	}

	// ...
}
```

## Verwendungsbeispiele in einer realen Strategie

### Start und Stopp der Strategie protokollieren

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	LogInfo("Strategie {0} gestartet um {1}. Wertpapier: {2}, Portfolio: {3}",
		Name, time, Security?.Code, Portfolio?.Name);

	// ...
}

protected override void OnStopped()
{
	LogInfo("Strategie {0} gestoppt. Position: {1}, P&L: {2}",
		Name, Position, PnL);

	base.OnStopped();
}
```

### Trades protokollieren

```cs
protected override void OnNewMyTrade(MyTrade trade)
{
	LogInfo("{0} {1} {2} zum Preis {3}. Volumen: {4}",
		trade.Order.Direction == Sides.Buy ? "Gekauft" : "Verkauft",
		trade.Order.Security.Code,
		trade.Order.Type,
		trade.Trade.Price,
		trade.Trade.Volume);

	base.OnNewMyTrade(trade);
}
```

### Fehler bei der Orderregistrierung protokollieren

```cs
protected override void OnOrderRegisterFailed(OrderFail fail, bool calcRisk)
{
	LogError("Fehler bei der Orderregistrierung {0}: {1}",
		fail.Order.TransactionId, fail.Error.Message);

	base.OnOrderRegisterFailed(fail, calcRisk);
}
```

## Log-Listener verbinden

Um Meldungen von einer Strategie zu empfangen, verbinden Sie Listener über [LogManager](xref:Ecng.Logging.LogManager):

```cs
var logManager = new LogManager();

// In Datei schreiben
var fileListener = new FileLogListener("{0}_{1:00}_{2:00}.txt".Put(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
logManager.Listeners.Add(fileListener);

// E-Mail senden
var emailListener = new EmailLogListener("from@stocksharp.com", "to@stocksharp.com");
emailListener.Filters.Add(msg => msg.Level == LogLevels.Error);
logManager.Listeners.Add(emailListener);

// Strategie als Log-Quelle hinzufügen
logManager.Sources.Add(strategy);
```

## Logs anzeigen

Meldungen, die in das Strategielog geschrieben wurden, können angezeigt werden:

1. Im Programm [Designer](../../designer.md) im Bereich "Logs"
2. In Logdateien, wenn [FileLogListener](xref:Ecng.Logging.FileLogListener) konfiguriert ist
3. In der Benutzeroberfläche über [LogControl](xref:StockSharp.Xaml.LogControl), wenn [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener) verwendet wird

## Siehe auch

[Protokollierung](../logging.md)
[LogControl-Komponente](../graphical_user_interface/logging/log_panel.md)
