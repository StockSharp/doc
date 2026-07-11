# Statistikreferenz

Der [StatisticManager](xref:StockSharp.Algo.Statistics.StatisticManager) verwaltet eine Sammlung von [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter)-Instanzen. Jeder Parameter verfolgt während der Strategieausführung eine bestimmte Kennzahl. Alle verfügbaren Parameter werden über die [StatisticParameterRegistry](xref:StockSharp.Algo.Statistics.StatisticParameterRegistry) erstellt.

Eine allgemeine Übersicht zur Arbeit mit Strategiestatistiken finden Sie im Abschnitt [Statistiken](statistics.md).

## Schnittstellen

Das Statistiksystem basiert auf einer Hierarchie von Schnittstellen. Jede Schnittstelle definiert die Datenquelle für die Berechnung des Parameters:

| Schnittstelle | Beschreibung |
|-----------|-------------|
| [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter) | Basisschnittstelle: Eigenschaften `Name`, `Type`, `Value`, `DisplayName`, `Description`, `Category`, `Order`; Methode `Reset()` |
| [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter) | Parameter auf Basis von Gewinn/Verlust: Methode `Add(marketTime, pnl, commission)` |
| [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter) | Parameter auf Basis von Trades: Methode `Add(PnLInfo)` |
| [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) | Parameter auf Basis von Orders: Methoden `New(order)`, `Changed(order)`, `RegisterFailed(fail)`, `CancelFailed(fail)` |
| [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter) | Parameter auf Basis von Positionen: Methode `Add(marketTime, position)` |
| [IRiskFreeRateStatisticParameter](xref:StockSharp.Algo.Statistics.IRiskFreeRateStatisticParameter) | Parameter mit risikofreiem Zinssatz: Eigenschaft `RiskFreeRate` |
| [IBeginValueStatisticParameter](xref:StockSharp.Algo.Statistics.IBeginValueStatisticParameter) | Parameter mit Anfangswert: Eigenschaft `BeginValue` |

## Gewinn-und-Verlust-Parameter (P&L)

Alle Parameter in dieser Gruppe implementieren die Schnittstelle [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter) und erben von [BasePnLStatisticParameter](xref:StockSharp.Algo.Statistics.BasePnLStatisticParameter`1). Sie erhalten Daten bei jeder Aktualisierung des P&L-Werts der Strategie.

| Klasse | Beschreibung | Werttyp |
|-------|-------------|------------|
| [NetProfitParameter](xref:StockSharp.Algo.Statistics.NetProfitParameter) | Nettogewinn für den gesamten Zeitraum. Wird dem aktuellen P&L-Wert gleichgesetzt | `decimal` |
| [NetProfitPercentParameter](xref:StockSharp.Algo.Statistics.NetProfitPercentParameter) | Nettogewinn in Prozent. Erfordert das Setzen von `BeginValue` (Anfangskapital). Formel: `pnl * 100 / BeginValue` | `decimal` |
| [MaxProfitParameter](xref:StockSharp.Algo.Statistics.MaxProfitParameter) | Maximaler Gewinn (höchster P&L-Wert im gesamten Zeitraum) | `decimal` |
| [MaxProfitPercentParameter](xref:StockSharp.Algo.Statistics.MaxProfitPercentParameter) | Maximaler Gewinn in Prozent. Erfordert `BeginValue`. Formel: `MaxProfit * 100 / BeginValue` | `decimal` |
| [MaxProfitDateParameter](xref:StockSharp.Algo.Statistics.MaxProfitDateParameter) | Datum, an dem der maximale Gewinn erreicht wurde | `DateTime` |
| [MaxDrawdownParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownParameter) | Maximaler absoluter Drawdown. Differenz zwischen Hochpunkt und Tiefpunkt der Equity-Kurve | `decimal` |
| [MaxDrawdownPercentParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownPercentParameter) | Maximaler Drawdown in Prozent. Formel: `MaxDrawdown * 100 / MaxEquity` | `decimal` |
| [MaxDrawdownDateParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownDateParameter) | Datum des maximalen Drawdowns | `DateTime` |
| [MaxRelativeDrawdownParameter](xref:StockSharp.Algo.Statistics.MaxRelativeDrawdownParameter) | Maximaler relativer Drawdown. Wird als Verhältnis des Drawdowns zum Spitzenwert der Equity berechnet | `decimal` |
| [ReturnParameter](xref:StockSharp.Algo.Statistics.ReturnParameter) | Relative Rendite für den gesamten Zeitraum. Maximales Wachstum vom Tiefpunkt bis zum aktuellen Wert in relativen Größen | `decimal` |
| [CommissionParameter](xref:StockSharp.Algo.Statistics.CommissionParameter) | Gesamte gezahlte Kommission. Akkumuliert alle Kommissionswerte | `decimal` |
| [AverageDrawdownParameter](xref:StockSharp.Algo.Statistics.AverageDrawdownParameter) | Durchschnittlicher Drawdown. Arithmetischer Mittelwert aller abgeschlossenen und aktuellen Drawdowns | `decimal` |
| [RecoveryFactorParameter](xref:StockSharp.Algo.Statistics.RecoveryFactorParameter) | Recovery-Faktor. Formel: `NetProfit / MaxDrawdown` | `decimal` |
| [SharpeRatioParameter](xref:StockSharp.Algo.Statistics.SharpeRatioParameter) | Sharpe Ratio. Formel: `(annualized return - risk-free rate) / annualized standard deviation` | `decimal` |
| [SortinoRatioParameter](xref:StockSharp.Algo.Statistics.SortinoRatioParameter) | Sortino Ratio. Ähnlich wie Sharpe, berücksichtigt aber nur negative Abweichungen | `decimal` |
| [CalmarRatioParameter](xref:StockSharp.Algo.Statistics.CalmarRatioParameter) | Calmar Ratio. Formel: `NetProfit / MaxDrawdown` | `decimal` |
| [SterlingRatioParameter](xref:StockSharp.Algo.Statistics.SterlingRatioParameter) | Sterling Ratio. Formel: `NetProfit / AverageDrawdown` | `decimal` |

### Risikokoeffizienten

Der [SharpeRatioParameter](xref:StockSharp.Algo.Statistics.SharpeRatioParameter) und der [SortinoRatioParameter](xref:StockSharp.Algo.Statistics.SortinoRatioParameter) erben von der Basisklasse [RiskAdjustedRatioParameter](xref:StockSharp.Algo.Statistics.RiskAdjustedRatioParameter) und implementieren die Schnittstelle [IRiskFreeRateStatisticParameter](xref:StockSharp.Algo.Statistics.IRiskFreeRateStatisticParameter).

Sie unterstützen die folgenden Einstellungen:

- **RiskFreeRate** -- jährlicher risikofreier Zinssatz (z. B. `0.03m` = 3 %)
- **Period** -- Periode für die Renditeberechnung (Standardwert `TimeSpan.FromDays(1)`)

Der [CalmarRatioParameter](xref:StockSharp.Algo.Statistics.CalmarRatioParameter) und der [SterlingRatioParameter](xref:StockSharp.Algo.Statistics.SterlingRatioParameter) hängen von anderen Parametern ab (`NetProfitParameter`, `MaxDrawdownParameter`, `AverageDrawdownParameter`) und werden automatisch verknüpft, wenn sie über die [StatisticParameterRegistry](xref:StockSharp.Algo.Statistics.StatisticParameterRegistry) erstellt werden.

## Trade-Parameter

Alle Parameter in dieser Gruppe implementieren die Schnittstelle [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter). Sie erhalten Daten über das Objekt [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) für jeden ausgeführten Trade.

| Klasse | Beschreibung | Werttyp |
|-------|-------------|------------|
| [TradeCountParameter](xref:StockSharp.Algo.Statistics.TradeCountParameter) | Gesamtzahl der Trades (nur Trades mit `ClosedVolume > 0` werden gezählt) | `int` |
| [WinningTradesParameter](xref:StockSharp.Algo.Statistics.WinningTradesParameter) | Anzahl profitabler Trades (`ClosedVolume > 0` und `PnL > 0`) | `int` |
| [LossingTradesParameter](xref:StockSharp.Algo.Statistics.LossingTradesParameter) | Anzahl verlustbringender Trades (`ClosedVolume > 0` und `PnL < 0`) | `int` |
| [RoundtripCountParameter](xref:StockSharp.Algo.Statistics.RoundtripCountParameter) | Anzahl abgeschlossener Roundtrips (schließende Trades mit `ClosedVolume > 0`) | `int` |
| [AverageTradeProfitParameter](xref:StockSharp.Algo.Statistics.AverageTradeProfitParameter) | Durchschnittlicher Gewinn pro Trade. Formel: `SumPnL / Count` | `decimal` |
| [AverageWinTradeParameter](xref:StockSharp.Algo.Statistics.AverageWinTradeParameter) | Durchschnittlicher Gewinn profitabler Trades. Es werden nur Trades mit `PnL > 0` berücksichtigt | `decimal` |
| [AverageLossTradeParameter](xref:StockSharp.Algo.Statistics.AverageLossTradeParameter) | Durchschnittlicher Verlust verlustbringender Trades. Es werden nur Trades mit `PnL < 0` berücksichtigt | `decimal` |
| [ProfitFactorParameter](xref:StockSharp.Algo.Statistics.ProfitFactorParameter) | Profit-Faktor. Formel: `GrossProfit / GrossLoss` | `decimal` |
| [ExpectancyParameter](xref:StockSharp.Algo.Statistics.ExpectancyParameter) | Mathematische Erwartung. Formel: `P(win) * AvgWin + P(loss) * AvgLoss` | `decimal` |
| [PerMonthTradeParameter](xref:StockSharp.Algo.Statistics.PerMonthTradeParameter) | Durchschnittliche Anzahl von Trades pro Monat | `decimal` |
| [PerDayTradeParameter](xref:StockSharp.Algo.Statistics.PerDayTradeParameter) | Durchschnittliche Anzahl von Trades pro Tag | `decimal` |
| [GrossProfitParameter](xref:StockSharp.Algo.Statistics.GrossProfitParameter) | Bruttogewinn. Summe der P&L aller profitablen Trades (`PnL > 0`) | `decimal` |
| [GrossLossParameter](xref:StockSharp.Algo.Statistics.GrossLossParameter) | Bruttoverlust. Summe der P&L aller verlustbringenden Trades (`PnL < 0`, Wert ist negativ) | `decimal` |

## Positionsparameter

Parameter in dieser Gruppe implementieren die Schnittstelle [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter). Sie erhalten Daten bei jeder Positionsänderung.

| Klasse | Beschreibung | Werttyp |
|-------|-------------|------------|
| [MaxLongPositionParameter](xref:StockSharp.Algo.Statistics.MaxLongPositionParameter) | Maximale Long-Position. Höchster positiver Positionswert | `decimal` |
| [MaxShortPositionParameter](xref:StockSharp.Algo.Statistics.MaxShortPositionParameter) | Maximale Short-Position. Höchster absoluter negativer Positionswert | `decimal` |

## Order-Parameter

Alle Parameter in dieser Gruppe implementieren die Schnittstelle [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) und erben von [BaseOrderStatisticParameter](xref:StockSharp.Algo.Statistics.BaseOrderStatisticParameter`1). Sie erhalten Daten bei Order-Registrierung, Änderungen und Fehlern.

| Klasse | Beschreibung | Werttyp |
|-------|-------------|------------|
| [OrderCountParameter](xref:StockSharp.Algo.Statistics.OrderCountParameter) | Gesamtzahl registrierter Orders | `int` |
| [OrderRegisterErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderRegisterErrorCountParameter) | Anzahl von Fehlern bei der Order-Registrierung | `int` |
| [OrderInsufficientFundErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderInsufficientFundErrorCountParameter) | Anzahl von Fehlern wegen "insufficient funds" (Typ `InsufficientFundException`) | `int` |
| [OrderCancelErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderCancelErrorCountParameter) | Anzahl von Fehlern bei der Order-Stornierung | `int` |

## Latenzparameter

Latenzparameter implementieren ebenfalls [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter), verfolgen aber die Zeitcharakteristiken der Order-Verarbeitung.

| Klasse | Beschreibung | Werttyp |
|-------|-------------|------------|
| [MaxLatencyRegistrationParameter](xref:StockSharp.Algo.Statistics.MaxLatencyRegistrationParameter) | Maximale Latenz der Order-Registrierung (Eigenschaft `Order.LatencyRegistration`) | `TimeSpan` |
| [MinLatencyRegistrationParameter](xref:StockSharp.Algo.Statistics.MinLatencyRegistrationParameter) | Minimale Latenz der Order-Registrierung | `TimeSpan` |
| [MaxLatencyCancellationParameter](xref:StockSharp.Algo.Statistics.MaxLatencyCancellationParameter) | Maximale Latenz der Order-Stornierung (Eigenschaft `Order.LatencyCancellation`) | `TimeSpan` |
| [MinLatencyCancellationParameter](xref:StockSharp.Algo.Statistics.MinLatencyCancellationParameter) | Minimale Latenz der Order-Stornierung | `TimeSpan` |

## Verwendung

### Zugriff auf Strategiestatistiken

```cs
var strategy = new MyStrategy();

// Nach der Ausführung auf Statistiken zugreifen
foreach (var param in strategy.StatisticManager.Parameters)
{
    Console.WriteLine($"{param.DisplayName}: {param.Value}");
}
```

### Einen bestimmten Parameter abrufen

```cs
// Nettogewinnwert abrufen
var netProfit = strategy.StatisticManager.Parameters
    .OfType<NetProfitParameter>()
    .First();

Console.WriteLine($"Nettogewinn: {netProfit.Value}");
```

### Risikofreien Zinssatz für Koeffizienten konfigurieren

Für die korrekte Berechnung der Sharpe- und Sortino-Ratio muss der risikofreie Zinssatz gesetzt werden:

```cs
// Risikofreien Zinssatz von 3 % für alle Koeffizienten setzen
foreach (var param in strategy.StatisticManager.Parameters
    .OfType<IRiskFreeRateStatisticParameter>())
{
    param.RiskFreeRate = 0.03m;
}
```

### Anfangskapital für Prozentparameter konfigurieren

Die Parameter [NetProfitPercentParameter](xref:StockSharp.Algo.Statistics.NetProfitPercentParameter) und [MaxProfitPercentParameter](xref:StockSharp.Algo.Statistics.MaxProfitPercentParameter) erfordern das Setzen des Anfangskapitals:

```cs
// Anfangskapital für Prozentberechnungen setzen
foreach (var param in strategy.StatisticManager.Parameters
    .OfType<IBeginValueStatisticParameter>())
{
    param.BeginValue = 1_000_000m; // 1,000,000
}
```

### Statistiken zurücksetzen

```cs
// Alle Statistikparameter zurücksetzen
strategy.StatisticManager.Reset();
```

### Zustand speichern und laden

Alle Parameter unterstützen die Serialisierung über die Schnittstelle `IPersistable`:

```cs
// Speichern
var storage = new SettingsStorage();
strategy.StatisticManager.Save(storage);

// Laden
strategy.StatisticManager.Load(storage);
```

## Siehe auch

- [Strategiestatistiken](statistics.md)
- [Grafische Statistikkomponente](../graphical_user_interface/strategies/statistics.md)
