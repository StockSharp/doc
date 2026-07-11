# Elder-Ray

Der **Elder-Ray Index** ist ein zusammengesetzter Indikator von Alexander Elder, der einen exponentiellen gleitenden Durchschnitt mit dem Bullenstärke kombiniert
und Bärenstärke-Oszillatoren. Es visualisiert das Gleichgewicht zwischen Käufern und Verkäufern und hilft zu erkennen, wann eine Seite die Kontrolle verliert.

Verwenden Sie die Klasse [ElderRay](xref:StockSharp.Algo.Indicators.ElderRay), um auf den Indikator zuzugreifen.

## Komponenten

Der Indikator gibt eine [ElderRayValue](xref:StockSharp.Algo.Indicators.ElderRayValue)-Struktur zurück, die Folgendes enthält:

- **EMA** – der exponentielle gleitende Basisdurchschnitt der Schlusskurse;
- **Bullenstärke** – der Abstand zwischen dem Balkenhoch und dem EMA;
- **Bärenstärke** – der Abstand zwischen dem Balkentief und dem EMA.

## Parameter

Elder-Ray erbt die Einstellungen von [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage):

- **Length** — EMA-Periode;
- **Alpha** – Glättungskoeffizient, wenn direkt konfiguriert.

## Interpretation

- **Bullenstärke > 0** zusammen mit einem steigenden EMA bestätigt einen Aufwärtstrend.
- **Bärenstärke < 0** mit fallendem EMA bestätigt einen Abwärtstrend.
- Schrumpfender Bullenstärke bei steigenden Preisen oder steigender Bärenstärke bei fallenden Preisen bilden Divergenzen und warnen vor Umkehrungen.
- Nullliniendurchgänge von Bullenstärke oder Bärenstärke markieren die Verschiebung der Marktkontrolle.

Handelsentscheidungen werden durch die gleichzeitige Analyse des EMA und beider Oszillatoren getroffen. Eine Kaufgelegenheit ergibt sich beispielsweise, wenn
Der EMA steigt, der Bärenstärke erholt sich von einem neuen Tief und der Bullenstärke bricht über Null.

![indicator_elder_ray](../../../../images/indicator_elder_ray.png)

## Siehe auch

[Bullenstärke](bull_power.md)
[Bärenstärke](bear_power.md)
[Exponentieller gleitender Durchschnitt](ema.md)
