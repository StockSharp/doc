# Bear Power

**Bear Power** ist Teil des Elder-Ray-Systems von Alexander Elder und zeigt, wie stark die Verkäufer im Vergleich zu einem exponentiellen
gleitenden Durchschnitt (EMA) sind. Er misst, wie weit die Intraday-Tiefs unter den Durchschnittspreis fallen, und hebt Momente hervor, in denen die Bären die
Kontrolle verlieren.

Verwenden Sie die Klasse [BearPower](xref:StockSharp.Algo.Indicators.BearPower), um auf den Indikator zuzugreifen.

## Beschreibung

Der Indikator wird als Differenz zwischen dem Balkentief und dem EMA-Wert berechnet:

`Bear Power = Low - EMA`.

- Negative Werte bestätigen Verkaufsdruck.
- Steigende Werte in Richtung null oder über null deuten auf schwächer werdende Bären und eine mögliche bullische Umkehr hin.
- Tiefe Tiefpunkte gehen häufig Erholungen voraus, insbesondere bei panikartigen Abverkäufen.

## Parameter

Bear Power übernimmt die Konfiguration von [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage):

- **Length** - EMA-Periode.
- **Alpha** (optional) - Glättungskoeffizient, wenn die EMA auf diese Weise konfiguriert wird.

## Verwendung

- Achten Sie auf Umkehrungen, wenn Bear Power nach einem extremen Tief nach oben dreht, während die EMA zu steigen beginnt.
- Eine Kreuzung der Nulllinie kann eine Änderung des vorherrschenden Trends bestätigen.
- Kombinieren Sie Bear Power mit [Bull Power](bull_power.md) und der Preis-EMA, um den vollständigen Indikator [Elder Ray](elder_ray.md) aufzubauen.

![indicator_bear_power](../../../../images/indicator_bear_power.png)

## Siehe auch

[Bull Power](bull_power.md)
[Elder Ray](elder_ray.md)
[ExponentialMovingAverage](ema.md)
