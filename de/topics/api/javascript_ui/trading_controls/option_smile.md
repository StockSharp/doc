# Volatilitäts-Smile

`OptionSmileWidget` zeichnet den Volatilitäts-Smile einer Optionsserie: die implizite Volatilität von Calls und Puts über die Strikes, zwei Linien auf einer Skala. Das Diagramm baut die Engine aus dem Paket `@stocksharp/chart` auf, das als Peer-Abhängigkeit deklariert ist.

![Volatilitäts-Smile über die Strikes einer Optionskette](../../../../images/javascript_controls_option_smile.png)

## Erstellen und aktualisieren

```ts
import {
  OptionSmileWidget,
  type OptionStrike,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const smile = OptionSmileWidget.create(
  document.querySelector<HTMLElement>('#smile')!,
  {},
  { host },
);

const chain: OptionStrike[] = [
  { strike: 67_000, call: { ivLast: 0.52 }, put: { ivBid: 0.55, ivAsk: 0.57 } },
  { strike: 67_500, call: { ivLast: 0.48 }, put: { ivLast: 0.50 } },
  { strike: 68_000, call: { ivBid: 0.45, ivAsk: 0.47 }, put: { ivLast: 0.47 } },
  { strike: 69_000, call: { ivLast: 0.49 }, put: {} },
];

smile.update(chain, { assetPrice: 68_120 });
```

Die einzige Abhängigkeit ist `host`; die Schnittstelle `OptionSmileDeps` enthält keine weiteren Felder. Das zweite Argument von `create` ist der gespeicherte Instanzzustand, den der Smile weder liest noch beschreibt.

`update` ersetzt die gesamte Kette vollständig. Der zweite Parameter `context` ist optional und standardmäßig ein leeres Objekt; er wird mit demselben Typ `OptionChainContext` übergeben wie beim Optionsdesk, doch der Smile benötigt daraus nur den Kurs des Basiswerts `assetPrice`.

Die statische Eigenschaft `OptionSmileWidget.TYPE` entspricht `ControlTypes.OptionSmile` — der Kennung `optionSmile`.

## Daten und Darstellung

Die Strikes werden aufsteigend sortiert, Einträge mit nicht numerischem `strike` werden verworfen. Die Volatilität einer Seite stammt aus `ivLast`, und wenn es keine Abschlüsse gab, als Mittel aus `ivBid` und `ivAsk`; berücksichtigt werden nur endliche positive Werte. Fehlt jeder davon, wird der Punkt nicht gezeichnet: Der Strike bleibt auf der Achse, und die Linie bricht ab — so ist ein Strike sichtbar, der nur von einer Seite quotiert wird. Im Diagramm werden die Werte als Prozentwerte ausgegeben.

Die Strike-Achse arbeitet im Modus `ordinal`: Die Schritte sind über die Auflistung des Listings gleichmäßig und nicht über den Abstand zwischen den Zahlen, deshalb wird die Lücke zwischen 67_500 und 68_000 nicht zu einem Loch. Die Achsenbeschriftungen und die Beschriftung des Fadenkreuzes erzeugt derselbe Kursformatierer.

Der Kurs des Basiswerts wird nicht als Linie gezeichnet — auf einer ordinalen Achse gibt es für ihn keine Stufe —, sondern als Text in der Legende neben den Schlüsseln `Call` und `Put` ausgegeben. Beim Überfahren des Diagramms erscheint eine Zeile mit dem Strike und den Werten beider Seiten an dieser Stelle: Der Smile liest sich über den Abstand zwischen den Kurven, deshalb werden beide gezeigt.

Solange kein einziger Strike quotiert ist, wird statt des Diagramms ein Platzhalter mit dem Text zum Schlüssel `NoOptions` angezeigt.

## Was das Steuerelement tut und was dem Host bleibt

Das Steuerelement erzeugt das Diagramm bei den ersten Daten selbst, bezieht Farben, Schrift und Gitterfarbe aus `host.presentation.canvasPalette()` (`up` — Call, `down` — Put), verfolgt die Größe des Containers über einen `ResizeObserver` und passt das Canvas an, behandelt die Schaltfläche zum Zurücksetzen des Zooms sowie die Schaltfläche zum Schließen des Panels, die `host.close()` aufruft. Der gesamte sichtbare Text wird über `host.t` angefordert: `OptionSmile`, `ResetView`, `ClosePanel`, `ImpliedVolatility`, `Call`, `Put`, `OptionChain`, `NoOptions`, `Underlying`.

Die Daten liefert der Host: Der Smile schließt keine Marktabonnements ab, berechnet keine Volatilität und unterscheidet eine Serie nicht von einer anderen — gezeichnet wird, was an `update` übergeben wurde. Eigene Einstellungen speichert das Steuerelement nicht in `host.preferences`, Schlüssel hat es keine.

## Öffentliche Methoden

- `update(strikes, context)` — die Kette und den Kontext anzeigen, in dem sie aufgenommen wurde.
- `resetZoom()` — nach dem Zoomen zur Übersicht über die gesamte Kette zurückkehren.
- `chart()` — das Diagrammobjekt (`IChartApi`) zurückgeben oder `null`, wenn das Diagramm noch nicht erzeugt wurde; wird von einem Host benötigt, der auf demselben Canvas eine zweite Serie oder eine Markierung ergänzt.
- `dispose()` — den Größenbeobachter abschalten, das Diagramm entfernen, `host.unregister` aufrufen und das Wurzelelement entfernen.

## Hilfsfunktionen

Das Paket exportiert auch die Funktionen, auf denen die Darstellung aufbaut — sie lassen sich separat verwenden:

- `sideVolatility(side)` — die Volatilität einer Seite oder `null`, wenn sie unbekannt ist.
- `sortedChain(strikes)` — die Kette in Zeichenreihenfolge: aufsteigend nach Strike, ohne fehlerhafte Einträge.
- `toSmileSeries(chain, put)` — eine Seite als Punktmenge für das Diagramm.

## Siehe auch

- [JavaScript-Handelssteuerelemente](../trading_controls.md)
- [Optionsdesk](option_desk.md)
- [Equity-Kurve](equity.md)
- [Orderbuch](order_book.md)
