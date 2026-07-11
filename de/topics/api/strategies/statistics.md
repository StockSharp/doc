# Strategiestatistiken

## Überblick

Die StockSharp-Plattform stellt ein umfassendes System zur statistischen Analyse von Handelsstrategien bereit. Es hilft Tradern, die Effektivität zu bewerten, Parameter zu optimieren und fundierte Entscheidungen zu treffen. Das Statistiksystem sammelt und verarbeitet Daten aus verschiedenen Aspekten des Handels, einschließlich Orders, Trades, Positionen sowie Gewinn-/Verlustkennzahlen.

## Zweck und Vorteile

Statistische Analyse in Handelsstrategien erfüllt mehrere wichtige Funktionen:

1. **Leistungsmessung**: Quantitative Bewertung des Erfolgs Ihrer Strategie mit Kennzahlen wie Nettogewinn, maximalem Drawdown und Recovery-Faktor.

2. **Risikomanagement**: Verständnis des Risikoprofils Ihrer Strategie anhand von Kennzahlen wie maximalem Drawdown-Prozentsatz und Statistiken zur Positionsgröße.

3. **Optimierung**: Finden optimaler Strategieparameter durch Vergleich statistischer Kennzahlen über verschiedene Parametersätze hinweg.

4. **Analyse der Trade-Qualität**: Analyse der Trade-Verteilung, des Verhältnisses profitabler zu verlustbringenden Trades und des durchschnittlichen Gewinns pro Trade.

5. **Operative Kennzahlen**: Verfolgung operativer Kennzahlen wie Latenzstatistiken und Orderfehlerraten, um Ausführungsprobleme zu identifizieren.

## Verfügbare statistische Kennzahlen

Die Schnittstelle [IStatisticManager](xref:StockSharp.Algo.Statistics.IStatisticManager) in StockSharp bietet Zugriff auf zahlreiche statistische Parameter, die in mehrere Kategorien gegliedert sind:

### Gewinn- und Verluststatistiken

- Nettogewinn
- Nettogewinn (%)
- Maximaler Gewinn
- Maximaler Drawdown
- Maximaler Drawdown (%)
- Maximaler relativer Drawdown
- Recovery-Faktor

### Trade-Statistiken

- Anzahl profitabler Trades
- Anzahl verlustbringender Trades
- Gesamtzahl der Trades
- Durchschnittlicher Gewinn pro Trade
- Durchschnittlicher profitabler Trade
- Durchschnittlicher verlustbringender Trade
- Anzahl der Trades pro Monat/Tag

### Positionsstatistiken

- Maximale Long-Position
- Maximale Short-Position

### Orderstatistiken

- Anzahl der Orders
- Anzahl der Orderfehler
- Maximale/minimale Registrierungsverzögerung
- Maximale/minimale Stornierungsverzögerung

## Integration mit der Klasse Strategy

Die Klasse [Strategy](xref:StockSharp.Algo.Strategies.Strategy) sammelt und berechnet Statistiken während der Ausführung automatisch. Der Statistikmanager ist über die Eigenschaft `StatisticManager` verfügbar, die die Schnittstelle [IStatisticManager](xref:StockSharp.Algo.Statistics.IStatisticManager) implementiert.

Wichtige statistische Werte sind auch direkt als Eigenschaften der Klasse Strategy dargestellt:

- `PnL`: Gewinn- und Verlustwert
- `Commission`: Gesamte gezahlte Kommission
- `Slippage`: Gesamte Slippage
- `Latency`: Durchschnittliche Latenz von Orderoperationen

## Visualisierung

StockSharp stellt eine spezielle grafische Komponente zur Visualisierung von Strategiestatistiken bereit, die `StatisticParameterGrid` heißt und im Namespace `StockSharp.Xaml` verfügbar ist. Dieses Grid zeigt alle statistischen Parameter in einem benutzerfreundlichen Format an.

Weitere Informationen zur grafischen Komponente finden Sie in der Dokumentation zu [Statistiken](../graphical_user_interface/strategies/statistics.md).

## Verwendungsbeispiel

Hier ist ein Beispiel für die Arbeit mit Strategiestatistiken in Ihrem Code:

```csharp
// Strategie erstellen
var strategy = new SmaStrategy
{
	// Strategieparameter konfigurieren
	Security = security,
	Portfolio = portfolio,
	Volume = 1,
	// SMA-Parameter setzen
	LongSma = 200,
	ShortSma = 50,
};

// Strategie zur Visualisierung mit einem Chart verbinden
var chart = new ChartPanel();
strategy.SetChart(chart);

// Zugriff auf den Statistikmanager
var statisticManager = strategy.StatisticManager;

// Strategiestatistiken in der Benutzeroberfläche anzeigen
// Annahme: In XAML ist ein StatisticParameterGrid als 'StatisticsGrid' definiert
StatisticsGrid.Parameters.Clear();
StatisticsGrid.Parameters.AddRange(statisticManager.Parameters);

// Strategie starten
strategy.Start();

// Wenn Sie auf Änderungen der Statistik reagieren müssen
strategy.PnLChanged += () =>
{
	Console.WriteLine($"Aktuelles PnL: {strategy.PnL}");

	// Sie können auch auf einzelne statistische Parameter zugreifen
	var netProfit = statisticManager.Parameters
		.OfType<NetProfitParameter>()
		.FirstOrDefault();

	if (netProfit != null)
	{
		Console.WriteLine($"Nettogewinn: {netProfit.Value}");
	}
};

// Positionenstatistiken verfolgen
strategy.PositionChanged += () =>
{
	Console.WriteLine($"Aktuelle Position: {strategy.Position}");
};
```

## Benutzerdefinierte Statistiken

Sie können auch eigene statistische Parameter erstellen, indem Sie die entsprechenden Schnittstellen implementieren:

- [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter): Basisschnittstelle für alle statistischen Parameter
- [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter): Für Parameter im Zusammenhang mit Gewinn/Verlust
- [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter): Für Parameter im Zusammenhang mit Trades
- [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter): Für Parameter im Zusammenhang mit Positionen
- [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter): Für Parameter im Zusammenhang mit Orders

Hier ist ein einfaches Beispiel für einen benutzerdefinierten statistischen Parameter:

```csharp
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = "Mein benutzerdefinierter Indikator",
	Description = "Beschreibung meines benutzerdefinierten Indikators",
	GroupName = "Benutzerdefinierte Parameter",
	Order = 1000
)]
public class MyCustomParameter : BasePnLStatisticParameter<decimal>
{
	public MyCustomParameter()
		: base(StatisticParameterTypes.Custom)
	{
	}

	public override void Add(DateTimeOffset marketTime, decimal pnl, decimal? commission)
	{
		// Benutzerdefinierte Berechnungslogik
		Value = /* Ihre eigene Berechnung */;
	}
}

// Dann zu StatisticManager Ihrer Strategie hinzufügen
strategy.StatisticManager.Parameters.Add(new MyCustomParameter());
```

## Fazit

Das System zur statistischen Analyse in StockSharp bietet Tradern leistungsfähige Werkzeuge zur Bewertung und Optimierung ihrer Handelsstrategien. Durch die Verwendung dieser Statistiken können Sie wertvolle Einblicke in die Leistung Ihrer Strategie gewinnen, Verbesserungsbereiche identifizieren und datenbasierte Entscheidungen zur Verbesserung Ihrer Handelsergebnisse treffen.

