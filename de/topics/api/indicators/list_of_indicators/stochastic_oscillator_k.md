# Stochastischer Oszillator %K

**Stochastic Oscillator %K** ist eine Komponente des stochastischen Oszillators, die die aktuelle Schlusspreisposition im Verhältnis zur Preisspanne über den ausgewählten Zeitraum anzeigt. Der Indikator wurde Ende der 1950er Jahre von George Lane entwickelt.

Um den Indikator zu verwenden, verwenden Sie die Klasse [StochasticK](xref:StockSharp.Algo.Indicators.StochasticK).

## Beschreibung

Stochastic Oscillator %K basiert auf der Beobachtung, dass sich die Schlusskurse bei Aufwärtstrends normalerweise näher an der oberen Grenze der Preisspanne konzentrieren, während sie sich bei Abwärtstrends eher an der unteren Grenze konzentrieren.

%K ist die „schnelle“ Linie des stochastischen Oszillators und die Hauptkomponente zur Berechnung der %D-Linie, die ein gleitender Durchschnitt von %K ist.

Der Oszillator reicht von 0 bis 100:

- Werte über 80 deuten normalerweise auf einen überkauften Markt hin.
- Werte unter 20 deuten auf einen überverkauften Markt hin.
- Kreuzungen der %K- und %D-Leitungen können als Ein- oder Ausstiegssignale verwendet werden.

## Parameter

- **Length** – Zeitraum zur Berechnung der Preisspanne, also der Höchst- und Tiefststände. Der übliche Standardwert ist 14.

## Berechnung

Formel zur Berechnung von %K:

```
%K = 100 * ((Close - Low(Length)) / (High(Length) - Low(Length)))
```

Dabei gilt:

- Close – aktueller Schlusskurs.
- Low(Length) – Mindestpreis im Length-Zeitraum.
- High(Length) – Höchstpreis im Length-Zeitraum.

Im vollstochastischen Oszillator wird die %D-Linie als einfacher gleitender Durchschnitt von %K über den angegebenen Zeitraum, normalerweise 3, berechnet:

```
%D = SMA(%K, 3)
```

![IndicatorStochasticK](../../../../images/indicatorstochastick.png)

## Siehe auch

[Stochastic Oscillator](stochastic_oscillator.md)
