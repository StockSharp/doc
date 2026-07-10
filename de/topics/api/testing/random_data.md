# Zufallsdaten

Testing mit Zufallsdaten ist eine besondere Art des Testings. Es dient nicht dazu, optimale Parameter zu suchen. Stattdessen kann ein solches Testing Fehler im Code aufdecken, indem der Handelsalgorithmus unterschiedlichen Börsenszenarien ausgesetzt wird.

In der Algorithmusentwicklung wird in der Regel nur eine bestimmte Menge von Szenarien verwendet. Tritt deshalb eine spezielle Situation auf, kann der Algorithmus falsch reagieren oder Ausnahmen auslösen. Zum Beispiel können folgende Situationen auftreten:

- Die Strategie arbeitet mit Kerzen vom Typ [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) und erwartet bei jeder Iteration, dass für den angeforderten Zeitraum immer eine Kerze vorhanden ist. Ein Zeitraum beginnt jedoch, in dem kein einziger Trade stattgefunden hat, und es wird keine Kerze erstellt. Ohne korrekte Behandlung wird dadurch eine [NullReferenceException](xref:System.NullReferenceException) ausgelöst und die Strategie stoppt.
- Die Strategie arbeitet mit einem illiquiden Instrument und verwendet [IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage). Sie erwartet, dass das Orderbuch immer gefüllt ist. Zu einem bestimmten Zeitpunkt ist das Orderbuch nur teilweise gefüllt, zum Beispiel gibt es Gebote, aber keine Angebote. Wenn die Strategie diese Situation nicht berücksichtigt, registriert sie entweder eine Order falsch oder löst eine Ausnahme aus und stoppt.
- Die Strategie berechnet Preisniveaus. Der Code ist so geschrieben, dass die Strategie wartet, bis vorab gesetzte Niveaus durchbrochen werden. Wenn die Niveaus falsch berechnet oder gesetzt wurden, werden sie entweder nie durchbrochen oder es wird immer nur eines davon durchbrochen. Dadurch führt die Strategie entweder gar keine Trades aus oder diese Trades verursachen Verluste.

Für diese und viele andere Börsenszenarien, die sich im Voraus nicht vorhersagen lassen, stellt [S#](../../api.md) Testing mit Zufallsdaten bereit. Durch die gleichmäßige Einzigartigkeit der generierten Daten kann in kurzer Zeit eine maximale Anzahl von Bedingungen erzeugt werden.

## Testing einer Moving-Average-Strategie mit Zufallsdaten

1. Das Beispiel SampleRandomEmulation (*..Samples\/Testing\/SampleRandomEmulation*) ist durch die Verwendung der einheitlichen Klasse [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) fast identisch mit dem Beispiel SampleHistoryTesting. Dessen Beschreibung finden Sie im Abschnitt [Testing mit historischen Daten](historical_data.md). Im Unterschied zum [Testing mit historischen Daten](historical_data.md) werden beim Testing mit Zufallsdaten die Marktdaten jedoch nicht geladen, sondern "on the fly" erzeugt. Daher werden dem Beispiel zwei Zufallsdatengeneratoren hinzugefügt: einer für das Orderbuch und einer für Tick-Trades. In SampleHistoryTesting wird nur ein Generator verwendet, nämlich für das Orderbuch, da keine gespeicherte Historie vorhanden ist.

   ```cs
   _connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
   {
       IsSubscribe = true,
       Generator = new RandomWalkTradeGenerator(new SecurityId { SecurityCode = security.Code })
       {
           Interval = TimeSpan.FromSeconds(1),
           MaxVolume = maxVolume,
           MaxPriceStepCount = 3,
           GenerateOriginSide = true,
           MinVolume = minVolume,
           RandomArrayLength = 99,
       }
   });
   _connector.SubscribeMarketDepth(new TrendMarketDepthGenerator(_connector.GetSecurityId(security)) { GenerateDepthOnEachTrade = false });
   ```
2. Das Ergebnis des Beispiels sieht wie folgt aus: ![Beispiel Emulationstest](../../../images/sample_emulation_test.png)

