# JavaScript-Diagramm

[StockSharp JS Diagram](https://github.com/StockSharp/Diagram) ist eine eigenständige, abhängigkeitsfreie Browser-Komponente, die das visuelle Strategieschema des [Designer](../../designer.md) — dasselbe Blockdiagramm aus verbundenen Elementen — auf einem HTML-`canvas` rendert. Sie wird auf npm als [@stocksharp/diagram](https://www.npmjs.com/package/@stocksharp/diagram) veröffentlicht und treibt die schreibgeschützten Strategiediagramme an, die auf den StockSharp-Websites angezeigt werden.

Eine Strategie wird als **Schema** beschrieben: eine Menge von *Knoten* (Elemente wie eine Kerzenquelle, ein Indikator, eine Bedingung oder eine Order), die über typisierte *Ports* miteinander verdrahtet sind. Die Komponente nimmt dieses Schema plus eine *Palette* (den Katalog der Elementtypen, ihrer Ports und Farben) und zeichnet es.

## Live-Demo

Das folgende Diagramm ist die echte Engine, die auf dieser Seite läuft — ein minimales Strategiegerüst „Datenquelle → Indikator → Chart“. Ziehen Sie die Leinwand, um zu verschieben, nutzen Sie das Mausrad zum Zoomen, und drücken Sie die Erweitern-Schaltfläche, um es im Vollbild zu öffnen.

```diagram-demo sma
```

Die drei Blöcke sind eine **Candles**-Quelle, die einen **Indicator** (einen einfachen gleitenden Durchschnitt) speist; sowohl die Kerzen als auch die Ausgabe des Indikators werden auf einem **Chart**-Element gezeichnet. Dies ist das kleinste vollständige Muster im Designer: Daten erzeugen, transformieren, visualisieren.

## Installation

Installieren Sie das Paket von npm:

```bash
npm install @stocksharp/diagram
```

Importieren Sie dann die ES-Module — `import { renderScheme } from '@stocksharp/diagram/embed'` für die schreibgeschützte Einbettung oder `import { StockSharpDiagram } from '@stocksharp/diagram'` für den [interaktiven Editor](javascript_diagram/editor.md).

## Ein Diagramm einbetten

Die Komponente stellt `renderScheme(host, paletteUrl, scheme)` über den Einstiegspunkt `@stocksharp/diagram/embed` bereit. Übergeben Sie ihr ein Host-Element, die URL einer Paletten-JSON und ein aus `nodes` und `links` aufgebautes Schema:

```js
import { renderScheme } from '@stocksharp/diagram/embed';

const scheme = {
  nodes: [
    { id: 'candles', typeId: 'CandleElement',    name: 'Candles', x: 60,  y: 130 },
    { id: 'sma',     typeId: 'IndicatorElement', name: 'SMA',     x: 340, y: 60  },
    { id: 'chart',   typeId: 'ChartElement',     name: 'Chart',   x: 620, y: 130 },
  ],
  links: [
    { from: 'candles', fromPort: 'Output', to: 'sma',   toPort: 'Input' },
    { from: 'sma',     fromPort: 'Output', to: 'chart', toPort: 'Input' },
    { from: 'candles', fromPort: 'Output', to: 'chart', toPort: 'Input' },
  ],
};

renderScheme(document.getElementById('diagram'), '/data/designer-palette.json', scheme);
```

Der `typeId` jedes Knotens muss in der Palette vorhanden sein; unbekannte Typen werden als Platzhalterblöcke gerendert. Ports werden über ihren `key` referenziert, und eine Verbindung ist gültig, wenn der Typ des Quellports mit dem Typ des Zielports kompatibel ist. `renderScheme` ist schreibgeschützt: Die Engine ordnet an, gestaltet das Theme (sie folgt der Hell-/Dunkel-Einstellung der Seite) und lässt den Betrachter verschieben, zoomen und erweitern, bearbeitet das Schema jedoch nicht.

Dieselbe Komponente läuft auch als vollwertiger **Editor** — Elemente aus einer Palette ziehen, Ports verbinden, Knoten bearbeiten und löschen, rückgängig machen/wiederherstellen. Siehe [Interaktiver Editor](javascript_diagram/editor.md) und [Ereignisse und API](javascript_diagram/events.md).

## Siehe auch

- [Interaktiver Editor](javascript_diagram/editor.md)
- [Ereignisse und API](javascript_diagram/events.md)
- [JavaScript-Charts](charts/javascript_charts.md)
- [Designer](../../designer.md) — der visuelle Desktop-Strategieeditor
- [Diagramm-Repository](https://github.com/StockSharp/Diagram)
