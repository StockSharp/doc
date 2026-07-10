# KC

**Keltner Channels (KC)** ist ein technischer Indikator, der aus einer Reihe von Volatilitätsbändern besteht, der einen exponentiellen gleitenden Durchschnitt (EMA) als Mittellinie und den durchschnittlichen wahren Bereich (ATR) verwendet, um die Kanalbreite zu bestimmen.

Um den Indikator verwenden zu können, müssen Sie die Klasse [KeltnerChannels](xref:StockSharp.Algo.Indicators.KeltnerChannels) verwenden.

## Beschreibung

Keltner Channels besteht aus drei Zeilen:
1. **Mittellinie**: typischerweise dargestellt durch einen 20-Perioden-EMA
2. **Upper-Band**: Mittellinie plus ein Multiplikator von ATR
3. **Lower-Band**: Mittellinie minus dem gleichen ATR-Multiplikator

Der Indikator wurde in den 1960er Jahren von Chester Keltner entwickelt und später von Linda Raschke modifiziert, die den einfachen gleitenden Durchschnitt (SMA) durch einen exponentiellen gleitenden Durchschnitt (EMA) ersetzte und begann, ATR anstelle des Bereichs High-Low zur Berechnung der Kanalbreite zu verwenden.

Keltner Channels hilft Händlern dabei, die Trendrichtung sowie potenzielle Unterstützungs- und Widerstandsniveaus zu bestimmen. Sie werden auch verwendet, um überkaufte und überverkaufte Bedingungen zu identifizieren, wenn der Preis das obere bzw. untere Band berührt oder durchbricht.

## Parameter

Der Indikator hat die folgenden Parameter:
- **Length** – Zeitraum zur Berechnung von EMA und ATR (Standardwert: 20)
- **Multiplier** – Multiplikator für ATR, der die Kanalbreite bestimmt (Standardwert: 2,0)

## Berechnung

Die Keltner Channels-Berechnung umfasst die folgenden Schritte:

1. Berechnen Sie den exponentiellen gleitenden Durchschnitt:
   ```
   Middle Line = EMA(Price, Length)
   ```

2. Berechnen Sie die durchschnittliche wahre Reichweite:
   ```
   ATR = Average True Range über Length-Periode
   ```

3. Berechnen Sie die oberen und unteren Bänder:
   ```
   Upper Band = Middle Line + (Multiplier * ATR)
   Lower Band = Middle Line - (Multiplier * ATR)
   ```

Dabei gilt:
- Price – normalerweise Schlusskurs
- EMA – exponentieller gleitender Durchschnitt
- ATR – durchschnittliche wahre Reichweite
- Length – Zeitraum für die Berechnung von EMA und ATR
- Multiplier – Multiplikator zur Bestimmung der Kanalbreite

## Interpretation

Keltner Channels kann wie folgt interpretiert werden:

1. **Trendrichtung**:
   - Wenn alle drei Linien nach oben zeigen, deutet dies auf einen Aufwärtstrend hin
   - Wenn alle drei Linien nach unten zeigen, deutet dies auf einen Abwärtstrend hin
   - Die horizontale Linienbewegung weist auf einen Seitwärtstrend hin

2. **Ausbrüche**:
   - Ein Ausbruch von Price über das obere Band könnte auf eine starke Aufwärtsdynamik hinweisen
   - Wenn Price unter das untere Band fällt, kann dies auf eine starke Abwärtsdynamik hinweisen
   - Ausbrüche werden häufig als Einstiegssignale in Richtung des Ausbruchs verwendet

3. **Rückkehr zur Middle-Zeile**:
   - Nach dem Durchbrechen des oberen oder unteren Bandes kehrt der Preis häufig zur Mittellinie zurück
   - Die Mittellinie kann als Unterstützungs- oder Widerstandsniveau dienen

4. **Überkaufte und überverkaufte Bedingungen**:
   - Ein Preis nahe oder jenseits des oberen Bandes kann auf überkaufte Bedingungen hinweisen
   - Ein Preis nahe oder jenseits des unteren Bandes kann auf überverkaufte Bedingungen hinweisen
   - In Trendmärkten kann der Preis über längere Zeiträume in „extremen“ Zonen bleiben

5. **Kanalverkleinerung und -erweiterung**:
   - Eine Kanalverengung (verringernder Abstand zwischen den Bändern) weist auf eine verringerte Volatilität hin, die häufig einer starken Preisbewegung vorausgeht
   - Die Kanalausweitung weist auf eine erhöhte Volatilität hin

6. **Handelsstrategien**:
   - „Edge to Middle“-Strategie: Eröffnen einer Position, wenn der Preis das obere oder untere Band berührt, wobei die Mittellinie angestrebt wird
   - Breakout-Strategie: Eröffnen einer Position, wenn der Preis das obere oder untere Band durchbricht, in Erwartung einer weiteren Bewegung in die gleiche Richtung
   - „Middle to Edge“-Strategie: Eröffnen einer Position, wenn der Preis von der Mittellinie abprallt, wobei das obere oder untere Band angestrebt wird

![indicator_keltner_channels](../../../../images/indicator_keltner_channels.png)

## Siehe auch

[BollingerBands](bollinger_bands.md)
[DonchianChannels](donchian_channels.md)
[EMA](ema.md)
[ATR](atr.md)
