# CM

**Zusammengesetztes Momentum (CM)** ist ein Indikator, der mehrere Methoden zur Messung der Preisdynamik kombiniert, um zuverlässigere Signale über Trendstärke und -richtung zu erhalten.

Um den Indikator verwenden zu können, müssen Sie die Klasse [CompositeMomentum](xref:StockSharp.Algo.Indicators.CompositeMomentum) verwenden.

## Beschreibung

Der Zusammengesetztes Momentum (CM)-Indikator ist ein umfassendes Tool, das verschiedene Aspekte der Preisbewegung integriert, darunter Preisänderungsrate, relative Stärke und andere Momentumkomponenten. Durch diesen kombinierten Ansatz liefert CM im Vergleich zu herkömmlichen eindimensionalen Momentumindikatoren ein vollständigeres Bild der aktuellen Marktdynamik.

CM ist wirksam für:
- Bestimmung der Stärke des aktuellen Trends
- Identifizieren potenzieller Umkehrpunkte
- Erkennen von Divergenzen zwischen Preis und Dynamik
- Herausfiltern falscher Signale anderer Indikatoren

Zusammengesetztes Momentum ist besonders nützlich in volatilen Märkten, wo traditionelle Momentum-Indikatoren zahlreiche falsche Signale erzeugen können.

## Berechnung

Die Zusammengesetztes Momentum-Berechnung umfasst mehrere Phasen und Komponenten:

1. Berechnung der Impulskomponenten:
   - Price-Änderung im Vergleich zu früheren Perioden
   - Verhältnis zwischen den jüngsten Höchst- und Tiefstständen
   - Volume-Analyse zur Begleitung der Preisbewegung

2. Normalisierung jeder Komponente, um sie auf vergleichbare Maßstäbe zu bringen.

3. Gewichtete Summation der Komponenten, um den endgültigen CM-Wert zu erhalten.

Der endgültige CM-Wert ist ein Oszillator, der sowohl im positiven als auch im negativen Bereich schwanken kann:
- Positive Werte deuten auf eine Aufwärtsdynamik hin
- Negative Werte deuten auf eine Abwärtsdynamik hin
- Die Größe des Werts (Absolutwert) gibt die Impulsstärke an

## Interpretation

- **Nulllinienüberschreitung**:
  - Der Übergang von der negativen zur positiven Zone kann als bullisches Signal angesehen werden
  - Der Übergang von der positiven zur negativen Zone kann als bärisches Signal angesehen werden

- **Extreme Werte**:
  - Sehr hohe positive Werte können auf überkaufte Marktbedingungen hinweisen
  - Sehr niedrige negative Werte können auf überverkaufte Marktbedingungen hinweisen

- **Abweichungen**:
  - Bullische Divergenz: Der Preis bildet ein neues Tief, während CM ein höheres Tief bildet
  - Bärische Divergenz: Der Preis bildet ein neues Hoch, während CM ein niedrigeres Hoch bildet

- **Trendbestätigung**:
  - Durchweg positive CM-Werte bestätigen die Stärke eines Aufwärtstrends
  - Durchweg negative CM-Werte bestätigen die Stärke eines Abwärtstrends

- **Momentumverlust**:
  - Ein Rückgang des absoluten Werts von CM in Trendrichtung kann auf einen Momentumverlust und eine mögliche Umkehr hinweisen

![indicator_composite_momentum](../../../../images/indicator_composite_momentum.png)

## Siehe auch

[Impuls](momentum.md)
[RoC](roc.md)
[RSI](rsi.md)
[MACD](macd.md)
