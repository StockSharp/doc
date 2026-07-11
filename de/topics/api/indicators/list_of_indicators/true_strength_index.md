# Index der wahren Stärke

Der **Index der wahren Stärke (TSI)** ist ein von William Blau entwickelter Momentum-Oszillator. Er glättet die Differenz zwischen aufeinanderfolgenden Schlusskursen doppelt und hilft dadurch, Trends und Wendepunkte mit weniger Verzögerung als viele klassische Oszillatoren zu erkennen.

Verwenden Sie die Klasse [TrueStrengthIndex](xref:StockSharp.Algo.Indicators.TrueStrengthIndex), um auf den Indikator zuzugreifen.

## Berechnung

1. Berechnen Sie die Kursänderung `m = Close - PreviousClose`.
2. Wenden Sie zwei exponentielle gleitende Durchschnitte mit den Perioden `Length1` und `Length2` sowohl auf `m` als auch auf `|m|` an.
3. Berechnen Sie das Verhältnis des doppelt geglätteten Momentums zum doppelt geglätteten absoluten Momentum:
   `TSI = 100 * EMA(EMA(m, Length1), Length2) / EMA(EMA(|m|, Length1), Length2)`.
4. Optional kann eine Signallinie als EMA des TSI mit der Periode **Signal** abgeleitet werden.

## Parameter

- **Length1** - erste Glättungsperiode.
- **Length2** - zweite Glättungsperiode.
- **Signal** - Periode der Signallinie (optional).

## Interpretation

- **TSI > 0** - bullisches Momentum.
- **TSI < 0** - bärisches Momentum.
- **Kreuzungen der Signallinie** liefern Einstiegssignale.
- **Divergenzen** zwischen TSI und Kurs warnen vor möglichen Umkehrungen.

Durch die doppelte Glättung und Normalisierung filtert der Indikator Rauschen heraus und bleibt zugleich reaktionsfähiger als einfache Momentum-Berechnungen.

![Index der wahren Stärke Diagramm](../../../../images/indicator_true_strength_index.png)

## Siehe auch

[Impuls](momentum.md)
[MACD](macd.md)
[RSI](rsi.md)

