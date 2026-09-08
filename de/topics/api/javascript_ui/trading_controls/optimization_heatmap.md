# Optimierungs-Heatmap

`OptimizationHeatmapWidget` zeichnet eine Kennzahl über zwei Parameter: Die Werte des ersten Parameters verlaufen waagerecht, die des zweiten senkrecht, und im Schnittpunkt liegt eine Zelle, eingefärbt nach dem, was dieser Parametersatz geliefert hat. Über die Optimierung selbst weiß das Steuerelement nichts — die Karte einer Kennzahl über zwei Achsen sieht gleich aus, womit auch immer die Paare berechnet wurden —, deshalb übergibt der Host die Paare zusammen mit den Bezeichnungen der Achsen und der Kennzahl.

![Optimierungs-Heatmap über zwei Parameter](../../../../images/javascript_controls_optimization_heatmap.png)

## Erstellen und aktualisieren

```ts
import {
  HeatDirections,
  OptimizationHeatmapWidget,
  type HeatCell,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const heatmap = OptimizationHeatmapWidget.create(
  document.querySelector<HTMLElement>('#heatmap')!,
  {},
  { host },
);

const cells: HeatCell[] = [
  { x: '10', y: '00:05:00', value: 12_400 },
  { x: '10', y: '00:15:00', value: 9_150 },
  { x: '20', y: '00:05:00', value: -1_800 },
  { x: '20', y: '00:15:00', value: 15_900 },
];

heatmap.update({
  xLabel: 'Length',
  yLabel: 'Timeframe',
  metricLabel: 'Net profit',
  betterWhen: HeatDirections.Higher,
  cells,
});
```

Die einzige Abhängigkeit ist `host`; die Schnittstelle `OptimizationHeatmapDeps` enthält keine weiteren Felder. Handler nimmt die Karte nicht entgegen: Eine Zelle ist der Mittelwert über die Läufe eines Paars und kein einzelner Lauf, deshalb gibt es per Klick nichts zu öffnen. Das zweite Argument von `create` ist der gespeicherte Instanzzustand; das Steuerelement liest ihn nicht und schreibt nichts hinein.

`update` ersetzt die Karte vollständig: Ein aus dem Satz ausgeschiedenes Paar existiert nicht mehr, und eine von ihm übrig gelassene Zelle würde von einem Lauf berichten, den es im Bericht nicht mehr gibt.

Das Argument von `update` ist ein Objekt `HeatmapData` mit den Feldern:

| Feld | Bedeutung |
|---|---|
| `xLabel` | Bezeichnung der waagerechten Achse, wird unter der Karte beschriftet. |
| `yLabel` | Bezeichnung der senkrechten Achse, wird über der Karte beschriftet. |
| `metricLabel` | Bezeichnung der Kennzahl, wird in der Panelüberschrift ausgegeben. |
| `betterWhen` | Wohin besser ist: `HeatDirections.Higher` (`'higher'`) oder `HeatDirections.Lower` (`'lower'`). Das Feld ist erforderlich — ohne es würde eine Drawdown-Karte die schlechteste Ecke in der Farbe des Sieges einfärben. |
| `cells` | Messungen vom Typ `HeatCell`: `x` und `y` sind die Achsenwerte als Zeichenfolgen, `value` ist eine Zahl. |

Der Typ `HeatmapData` ist im Modul des Steuerelements deklariert, und der Wurzelexport des Pakets exportiert ihn nicht erneut: Bei expliziter Typisierung importieren Sie ihn aus dem Unterpfad `@stocksharp/trading-controls/optimization-heatmap-widget`.

Die statische Eigenschaft `OptimizationHeatmapWidget.TYPE` entspricht `ControlTypes.OptimizationHeatmap` — der Kennung `optimizationHeatmap`.

## Daten und Darstellung

Die Achsen sind diskret, deshalb werden ihre Werte als Zeichenfolgen übergeben: `10`, `00:05:00` und `True` sind gleichberechtigte Positionen auf einer Achse. Die Reihenfolge der Werte ist numerisch, wenn jeder von ihnen eine Zahl ist (sonst stünde `10` vor `2`, und die Form der Karte wäre eine Folge der Zahlenschreibweise), und andernfalls textuell; bei Zeichenfolgen fester Breite, wie .NET Zeitintervalle schreibt, stimmt die textuelle Reihenfolge mit der chronologischen überein. Die erste Zeile des Gitters liegt am Fuß der Karte: Das ist ein Diagramm, und die Y-Achse wächst nach oben.

Mehrere Läufe auf demselben Paar werden zu einer Zelle zusammengefasst — dem Mittelwert samt Anzahl der Läufe; ein Eintrag mit nicht numerischer Kennzahl wird verworfen und verdirbt nicht die ganze Zelle. Die Farbe wird ausgehend von einem Bezugswert berechnet: Überschreiten die Messungen die Null, wird die Null zum Bezugswert, andernfalls die Mitte des Bereichs, denn eine Bindung an eine Null, die der Durchlauf gar nicht erreicht hat, ergäbe einen einzigen gleichmäßigen Fleck ohne Kontrast. Die Spanne bis zur vollen Farbe ist auf beiden Seiten gleich, deshalb bedeutet überall gleiche Sättigung auch eine gleiche Abweichung der Kennzahl. Die Richtung `betterWhen` steckt im Vorzeichen: Das beste Ergebnis wird immer in der Farbe des Anstiegs eingefärbt.

Zahlen werden nicht in die Zellen geschrieben: Bei einem Durchlauf von vierzig mal vierzig sind Ziffern in der Zelle unlesbar, und das Lesen der Farbe ist ja gerade der Sinn der Karte. Ein Paar, das niemand durchgerechnet hat, bleibt nicht leer, sondern wird diagonal durchgestrichen: Ein ungeprüftes Paar und ein Paar mit dem Ergebnis null sind verschiedene Tatsachen, die eine Skala mit der Null im neutralen Punkt gleich zeichnen würde. Die beste Zelle wird mit einem Rahmen in der Gitterfarbe umrandet — der einzigen richtungsneutralen Farbe der Palette.

Über der Karte wird eine Legende aus denselben zwei Farben mit drei Beschriftungen gezeichnet: unterer Rand, Bezugswert und oberer Rand. Die Achsenbeschriftungen werden ausgedünnt, wenn die Werte nicht mehr passen, doch der äußerste Wert einer Achse wird immer beschriftet. Die Zahlen werden über `formatStatistic` ausgegeben — dasselbe Format wie im Statistikpanel: Rundung auf zwei Stellen.

Beim Überfahren einer gemessenen Zelle erscheint ein Tooltip mit dem Wert beider Achsen und der Kennzahl. Die Anzahl der Läufe wird nur dann ergänzt, wenn über mehr als einen Lauf gemittelt wurde, und die Markierung `Best` nur bei der besten Zelle. Über einer durchgestrichenen Zelle gibt es keinen Tooltip: Sie ist bereits als ungeprüft erkennbar. Der Tooltip wird an die Ränder des Canvas gedrückt, damit er bei Randzellen nicht über den Rand hinausläuft.

Solange keine einzige Messung vorliegt, wird statt der Karte ein Platzhalter mit dem Text zum Schlüssel `NoOptimizationResults` angezeigt.

## Was das Steuerelement tut und was dem Host bleibt

Das Steuerelement baut das Markup des Panels selbst auf, passt das Canvas über einen `ResizeObserver` an den Container an und legt einen Puffer in physischen Pixeln gemäß `devicePixelRatio` an, sonst würde die Karte mit einem Gitter aus Haarlinien gezeichnet. Farben und Schrift fordert es bei jedem Zeichnen von `host.presentation.canvasPalette()` an: `up` und `down` sind die beiden Seiten der Skala, `grid` das Gitter, die Durchstreichungen, der Rahmen der besten Zelle und die Beschriftungen, `font` die Schrift des Textes auf dem Canvas. Eine eigene Palette wählt das Paket hier nicht, deshalb zeichnet ein Themenwechsel beim Host die Karte in neuen Farben neu. Die Transparenz ist das Einzige, worüber die Karte selbst verfügt.

Die Schaltfläche zum Schließen des Panels ruft `host.close()` auf, die Instanz registriert sich über `host.register` und meldet sich in `dispose` wieder ab. Der gesamte sichtbare Text wird über `host.t` angefordert: `OptimizationHeatmap`, `OptimizationHeatmapChart`, `ClosePanel`, `NoOptimizationResults`, `Runs`, `Best`. Eigene Einstellungen speichert das Steuerelement nicht in `host.preferences`, Schlüssel hat es keine.

Die Daten liefert der Host: Die Karte startet keinen Durchlauf, wählt und berechnet keine Kennzahl und errät die Richtung „besser“ nicht — gezeichnet wird, was an `update` übergeben wurde.

## Öffentliche Methoden

- `update(data)` — die Karte vollständig anzeigen.
- `dispose()` — den Größenbeobachter abschalten, `host.unregister` aufrufen und das Wurzelelement entfernen.

## Hilfsfunktionen

Die gesamte Geometrie der Karte ist in ein eigenes Modul ausgelagert und wird vom Paket exportiert — sie lässt sich ohne das Steuerelement verwenden:

- `layoutHeatmap(input)` — das Layout der Karte: Gitter, Lücken, Beschriftungen, Legende und Skala; `null`, wenn keine Messungen vorliegen.
- `hitHeatmap(layout, x, y)` — die Zelle unter einem Punkt oder `null`.
- `heatScale(buckets)` — Bezugswert, Spanne und Bereichsgrenzen.
- `tintOf(value, scale, betterWhen)` — die Sättigung von −1 bis 1, wobei positiv immer besser ist.
- `valueAt(tint, scale, betterWhen)` — die Umkehrung, für die Beschriftungen der Legende.
- `HeatDirections` — die Richtungen `Higher` und `Lower`.

## Siehe auch

- [JavaScript-Handelssteuerelemente](../trading_controls.md)
- [Optimierungsfläche](optimization_surface.md)
- [Statistik](statistics.md)
- [Equity-Kurve](equity.md)
