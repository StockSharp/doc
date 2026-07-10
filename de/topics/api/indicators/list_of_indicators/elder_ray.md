# Elder Ray

Der **Elder Ray Index** ist ein zusammengesetzter Indikator von Alexander Elder, der einen exponentiellen gleitenden Durchschnitt mit dem Bull Power kombiniert
und Bear Power-Oszillatoren. Es visualisiert das Gleichgewicht zwischen Käufern und Verkäufern und hilft zu erkennen, wann eine Seite die Kontrolle verliert.

Verwenden Sie die Klasse [ElderRay](xref:StockSharp.Algo.Indicators.ElderRay), um auf den Indikator zuzugreifen.

## Komponenten

Der Indikator gibt eine [ElderRayValue](xref:StockSharp.Algo.Indicators.ElderRayValue)-Struktur zurück, die Folgendes enthält:

- **EMA** – der exponentielle gleitende Basisdurchschnitt der Schlusskurse;
- **Bull Power** – der Abstand zwischen dem Balkenhoch und dem EMA;
- **Bear Power** – der Abstand zwischen dem Balkentief und dem EMA.

## Parameter

Elder Ray erbt die Einstellungen von [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage):

- **Length** — EMA-Periode;
- **Alpha** – Glättungskoeffizient, wenn direkt konfiguriert.

## Interpretation

- **Bull Power > 0** zusammen mit einem steigenden EMA bestätigt einen Aufwärtstrend.
- **Bear Power < 0** mit fallendem EMA bestätigt einen Abwärtstrend.
- Schrumpfender Bull Power bei steigenden Preisen oder steigender Bear Power bei fallenden Preisen bilden Divergenzen und warnen vor Umkehrungen.
- Nullliniendurchgänge von Bull Power oder Bear Power markieren die Verschiebung der Marktkontrolle.

Handelsentscheidungen werden durch die gleichzeitige Analyse des EMA und beider Oszillatoren getroffen. Eine Kaufgelegenheit ergibt sich beispielsweise, wenn
Der EMA steigt, der Bear Power erholt sich von einem neuen Tief und der Bull Power bricht über Null.

![indicator_elder_ray](../../../../images/indicator_elder_ray.png)

## Siehe auch

[Bull Power](bull_power.md)
[Bear Power](bear_power.md)
[Exponentieller gleitender Durchschnitt](ema.md)
