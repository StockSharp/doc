# Optimierungsfläche

`SurfaceWidget` zeigt die Ergebnisse eines Parameterdurchlaufs als dreidimensionale Landschaft: eine Kennzahl über zwei diskreten Achsen, wobei der Wert der Kennzahl sowohl die Höhe als auch die Farbe bestimmt. Das Steuerelement nimmt dieselben Daten entgegen wie die [Optimierungs-Heatmap](optimization_heatmap.md), deshalb lässt sich ein Ergebnissatz als flache Karte, als Fläche oder als beides zugleich anzeigen.

![Optimierungsfläche: das Ergebnis als Landschaft über zwei Parameter](../../../../images/javascript_controls_optimization_surface.png)

## Erstellen und aktualisieren

```ts
import {
  HeatDirections,
  SurfaceWidget,
  type SurfaceData,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const surface = SurfaceWidget.create(
  document.querySelector<HTMLElement>('#surface')!,
  {},
  { host },
);

const sweep: SurfaceData = {
  xLabel: 'Fast',
  yLabel: 'Slow',
  metricLabel: 'Net profit',
  betterWhen: HeatDirections.Higher,
  cells: [
    { x: '10', y: '50', value: 1_250 },
    { x: '10', y: '80', value: -320 },
    { x: '20', y: '50', value: 2_480 },
    { x: '20', y: '80', value: 640 },
  ],
};

surface.update(sweep);
```

Die einzige Abhängigkeit ist `host`; die Schnittstelle `SurfaceDeps` enthält keine weiteren Felder. Das zweite Argument von `create` ist der gespeicherte Instanzzustand: Die Fläche liest ihn nicht und schreibt nichts hinein.

`update` ersetzt den gesamten Satz vollständig. Die Fläche ist ein einzelner Durchlauf, deshalb gibt es für sie keine Teilaktualisierung: die Hälfte eines Durchlaufs über der Hälfte eines anderen ergäbe eine Landschaft aus zwei verschiedenen Auswertungen.

Die statische Eigenschaft `SurfaceWidget.TYPE` entspricht `ControlTypes.OptimizationSurface` — der Kennung `optimizationSurface`.

## Daten

Eine Zelle `HeatCell` ist ein Paar von Achsenwerten und die gemessene Kennzahl: `{ x, y, value }`. Die Achsenwerte sind Zeichenfolgen, weil die Achse diskret ist: `10`, `00:05:00` und `True` sind gleichberechtigte Positionen auf ihr. Numerische Werte werden als Zahlen geordnet, die übrigen als Text, deshalb bleibt der Durchlauf 5, 8, 12, 40 eine Folge.

Das Feld `betterWhen` ist erforderlich und nimmt `HeatDirections.Higher` oder `HeatDirections.Lower` entgegen. Ohne es hübe eine Drawdown-Landschaft die schlechteste Ecke zum Gipfel.

Mehrere Läufe auf demselben Paar werden zum Mittelwert zusammengefasst — das ist eine Zelle. Ein Paar, das der Durchlauf nicht abgedeckt hat, bleibt ein Loch: Eine Fläche wird nur gezeichnet, wenn alle vier ihrer Ecken bekannt sind, und eine Lücke wird nicht interpoliert — ein fehlendes Ergebnis ist nicht dasselbe wie ein Ergebnis von null. Gibt es überhaupt keine Zellen oder auf mindestens einer Achse weniger als zwei verschiedene Werte, wird statt der Landschaft ein Platzhalter mit dem Text zum Schlüssel `NoOptimizationResults` angezeigt.

Die Beschriftungen `xLabel`, `yLabel` und `metricLabel` gehen in die Achsenbeschriftungen; `metricLabel` wird zusätzlich in der Panelüberschrift ausgegeben.

## Darstellung

Die Höhe einer Fläche ist die Lage des Wertes relativ zum Bezugspunkt der Skala, zusammengefaltet auf den Bereich vom Boden bis zur Oberkante: Der Bezugspunkt fällt auf die halbe Höhe, deshalb bedeutet der Boden nicht „schlechtestes Ergebnis“, sondern den unteren Rand der Skala. Die Farbe stammt aus `host.presentation.canvasPalette()`: `up` für Werte besser als der Bezugspunkt, `down` für schlechtere, wobei die Sättigung mit dem Abstand davon zunimmt. Die Flächen werden von den fernen zu den nahen gefüllt, deshalb verdeckt ein naher Grat das dahinter Liegende, und sie werden in der Farbe `grid` umrandet, damit das Gitter dort erkennbar bleibt, wo zwei benachbarte Flächen fast denselben Farbton haben.

Die Projektion ist orthografisch: Eine Fläche wird gelesen, indem man Höhen über das gesamte Feld hinweg vergleicht, und eine Perspektive würde die ferne Seite eines Grats gegenüber der nahen verkürzen.

Unter der Landschaft werden zwei Bodenkanten und die senkrechte Achse der Skala gezeichnet. Auf den Parameterachsen werden bis zu acht Marken ausgegeben: Solange die Werte passen, alle, danach jede zweite, jede dritte und so weiter, wobei die erste und die letzte immer beschriftet werden. Auf der senkrechten Achse stehen fünf Marken, beschriftet mit Werten der Kennzahl. Die Beschriftungen werden auf jene Bodenkanten verlegt, die dem Betrachter näher sind — beim Drehen wird das neu berechnet, damit die Zahlen nicht über dem Gitter liegen.

## Ansicht und Gesten

Alle Gesten kommen über Pointer-Ereignisse, deshalb gehen Maus, Stift und Finger denselben Weg:

- Ziehen mit einem Zeiger dreht die Fläche: waagerecht ändert sich `yaw`, senkrecht `pitch`;
- zwei Zeiger ändern den Maßstab über ihren Abstand; das Drehen bleibt dabei einem Zeiger vorbehalten;
- das Mausrad ändert ebenfalls den Maßstab. Das Delta wird unabhängig davon, ob der Browser es in Pixeln, Zeilen oder Seiten meldet, auf „Rasten“ zurückgeführt und auf zwei Rasten je Ereignis begrenzt, damit Maus und Trackpad einen vergleichbaren Schritt liefern.

Eine Geste über dem Canvas nimmt das Steuerelement für sich, sonst würden Ziehen auf dem Telefon und das Mausrad im Desktop-Browser die Seite statt der Landschaft scrollen.

Neigung und Maßstab sind begrenzt: `pitch` von `MIN_PITCH` (0,12) bis `MAX_PITCH` (1,45), der Maßstab von 0,4 bis 4. Bei einer Neigung von null würde jede Fläche zu einer Linie entarten, und im rechten Winkel würde die Fläche zu einer flachen Karte, also zu einem anderen Steuerelement. Die Drehung `yaw` ist nicht begrenzt, sondern schließt sich: Die Landschaft ganz herumzudrehen, um den Rückhang eines Grats zu betrachten, ist eine sinnvolle Geste. Die Anfangsansicht ist `DEFAULT_VIEW`; die Schaltfläche in der Panelüberschrift (`ResetView`) kehrt zu ihr zurück.

Wenn der Zeiger die Fläche nicht dreht, sucht das Steuerelement den nächstgelegenen gemessenen Punkt im Umkreis von 22 CSS-Pixeln. Der gefundene Punkt wird mit einem Ring in der Farbe `up` umrandet, und in der Leiste über dem Canvas erscheint eine Zeile: der Wert auf der Achse `xLabel`, der Wert auf der Achse `yLabel` und die Kennzahl. Die Leiste liegt über dem Canvas und nicht in der Panelüberschrift: Die Anzeige bezieht sich auf den Punkt unter dem Zeiger und muss neben ihm stehen. Ein Paar, das der Durchlauf nicht abgedeckt hat, wird dem Zeiger nicht angeboten; bei gleichem Abstand wird der Punkt gewählt, der dem Betrachter näher liegt.

## Was das Steuerelement tut und was dem Host bleibt

Das Steuerelement verarbeitet die Gesten selbst, verfolgt die Größe des Canvas über einen `ResizeObserver` und zeichnet die Landschaft für die aktuelle Größe und Pixeldichte des Bildschirms neu, bedient die Schaltfläche zum Zurücksetzen der Ansicht und die Schaltfläche zum Schließen des Panels, die `host.close()` aufruft, und registriert sich über `host.register`, um sich in `dispose` wieder abzumelden. Farben und Schrift des Canvas kommen aus `host.presentation.canvasPalette()`. Der gesamte sichtbare Text wird über `host.t` angefordert: `OptimizationSurface`, `ResetView`, `ClosePanel`, `OptimizationSurfaceChart`, `NoOptimizationResults`.

Die Daten liefert der Host: Das Steuerelement startet keine Optimierung, abonniert ihren Verlauf nicht und weiß nicht, wie die Ergebnisse zustande kamen — gezeichnet wird, was an `update` übergeben wurde. Einen Klick auf eine Fläche gibt es nicht: Eine Fläche entspricht einer Zelle und nicht einem einzelnen Lauf, deshalb gibt es über sie nichts zu öffnen — ein Druck dreht die Landschaft.

Eigene Einstellungen speichert das Steuerelement nicht in `host.preferences`, Schlüssel hat es keine, `host.persistState` ruft es nicht auf. Die aktuelle Ansicht liefert die Methode `view()` — soll sie zwischen Sitzungen wiederhergestellt werden, speichert und liefert der Host diese Werte selbst.

## Öffentliche Methoden

- `update(data)` — den Ergebnissatz vollständig anzeigen.
- `view()` — eine Kopie der aktuellen Ansicht zurückgeben (`yaw`, `pitch`, `zoom`).
- `resetView()` — die Ansicht auf `DEFAULT_VIEW` zurücksetzen.
- `dispose()` — den Größenbeobachter abschalten, `host.unregister` aufrufen und das Wurzelelement entfernen.

## Hilfsfunktionen

Die Geometrie ist aus dem Steuerelement in ein eigenes Modul ausgelagert und wird vom Paket exportiert — darauf lässt sich eine eigene Darstellung aufbauen:

- `surfaceLayout(input)` — die Zellen für die vorgegebenen Abmessungen und die vorgegebene Ansicht in Flächen, Achsen und Scheitelpunkte zerlegen; `null`, wenn es nichts zu zeichnen gibt.
- `project(nx, ny, nz, view, box)` — einen Punkt des Einheitswürfels auf das Canvas projizieren.
- `dragView(view, dx, dy)` — die Ansicht nach einem Ziehen um so viele Pixel.
- `zoomView(view, factor)` — die Ansicht nach einer Maßstabsänderung.
- `clampView(view)` — die auf die zulässigen Grenzen gebrachte Ansicht.
- `DEFAULT_VIEW`, `MIN_PITCH`, `MAX_PITCH` — die Anfangsansicht und die Grenzen der Neigung.

## Siehe auch

- [JavaScript-Handelssteuerelemente](../trading_controls.md)
- [Optimierungs-Heatmap](optimization_heatmap.md)
- [Statistik](statistics.md)
- [Equity-Kurve](equity.md)
