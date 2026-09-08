# Optionsdesk

`OptionDeskWidget` zeigt eine Serie einer Optionskette: links die Calls, rechts spiegelbildlich die Puts, dazwischen den Strike und den inneren Wert. Volumen, Open Interest und Volatilitäten werden nicht nur als Zahl, sondern auch als Balken ausgegeben, deshalb liest sich die Kette über ihre Form und nicht allein über die Werte.

![Optionsdesk mit Call- und Put-Seite rund um die Strikes](../../../../images/javascript_controls_option_desk.png)

## Erstellen und aktualisieren

```ts
import {
  OptionDeskWidget,
  type TradingHost,
} from '@stocksharp/trading-controls';

declare const host: TradingHost;

const desk = OptionDeskWidget.create(
  document.querySelector<HTMLElement>('#option-desk')!,
  {},
  { host },
);

desk.update(
  [{
    strike: 68_000,
    call: {
      symbol: 'BTC-68000-C',
      bid: 1_240,
      ask: 1_265,
      last: 1_250,
      theoretical: 1_248,
      volume: 320,
      openInterest: 1_480,
      ivBid: 0.42,
      ivAsk: 0.44,
      ivLast: 0.43,
      historicalVolatility: 0.39,
    },
    put: {
      symbol: 'BTC-68000-P',
      bid: 820,
      ask: 845,
      volume: 210,
      openInterest: 960,
      ivLast: 0.47,
    },
  }],
  {
    assetPrice: 68_420,
    timeToExpiry: 0.08,
    riskFree: 0.05,
    dividend: 0,
  },
);
```

Die einzige Abhängigkeit ist `host`; optionale Abhängigkeiten hat das Steuerelement nicht. Das zweite Argument von `create` ist der Panelzustand, den das Steuerelement nicht verwendet.

`update(strikes, context)` ersetzt die gesamte Kette. Beide Argumente werden gemeinsam übergeben: Die Kette und der Kurs des Basiswerts sind eine einzige Beobachtung, und ein getrenntes Aktualisieren würde Griechen zeigen, die zu einem bereits verschobenen Kurs berechnet wurden. `context` ist optional und standardmäßig leer.

## Kontext der Kette

`OptionChainContext` beschreibt, wogegen die Serie bewertet wird: `assetPrice` ist der Kurs des Basiswerts, `timeToExpiry` die Restlaufzeit in Jahren, `riskFree` und `dividend` sind Zinssätze als Anteile (`0.05` bedeutet fünf Prozent). Ohne `assetPrice` zeigt das Desk weiterhin Quotierungen, der innere Wert ist jedoch null, und die Zeilen werden nicht in „im Geld“ und „aus dem Geld“ unterteilt. Ohne `assetPrice` oder `timeToExpiry` werden die Griechen nicht berechnet, und die Zelle bleibt leer statt null.

## Griechen

Die Griechen kommen auf einem von zwei Wegen. Berechnet der Host sie selbst, übergibt er ein fertiges Objekt `greeks` in der Seite des Strikes, und das Desk zeigt, was es bekommen hat. Sendet der Host die Volatilität, werden die Griechen an Ort und Stelle nach Black-Scholes aus dem ersten verfügbaren Wert in der Reihenfolge `ivLast`, `ivBid`, `ivAsk`, `historicalVolatility` berechnet. Keine der beiden Varianten ist ein Ersatz für die andere — es sind zwei Formen des Hosts.

Die Zahl der Nachkommastellen richtet sich nach den Daten: vier signifikante Ziffern für den kleinsten Wert der Spalte, jedoch nicht weniger als zwei und nicht mehr als acht Stellen. Es wird einmal je Spalte gezählt und gemeinsam für beide Seiten, deshalb wird das Delta nicht zu `0.0000`, und Gamma sowie seine Spiegelspalte werden gleich geschrieben.

## Spalten und Darstellung

Die Spaltenreihenfolge verläuft vom Strike nach außen: Volatilitäten und Quotierungen näher zur Mitte, die Griechen an den Rändern; die Put-Seite ist dasselbe in umgekehrter Reihenfolge. Sortiert wird aufsteigend nach Strike: Die Kette liest sich als Leiter.

Standardmäßig ausgeblendet sind die Spalten `callRho`, `callTheta`, `callHv`, `callTheor` und ihre Spiegelspalten `putRho`, `putTheta`, `putHv`, `putTheor` — zurück bringt sie das Tabellenkontextmenü.

Die Balken werden unterschiedlich skaliert. Volumen und Open Interest je Seite getrennt, weil Calls und Puts in unterschiedlichen Größen gehandelt werden. Die Volatilitäten mit einer einzigen Skala über beide Seiten hinweg, sonst verschwände die Schieflage zwischen den Seiten. Der Balken wird über die Breite des Elements gezeichnet, ohne Canvas.

Der Zeile wird die Klasse `option-itm-call` für Strikes unterhalb des Kurses des Basiswerts und `option-itm-put` für die übrigen zugewiesen; fehlt `assetPrice`, nur `option-row`. Volatilitäten werden als Prozentwerte mit zwei Stellen ausgegeben, Kurse im allgemeinen Kursformat des Pakets.

Einstellungen speichert das Desk nicht: Zugriffe auf `host.preferences` und `host.cache` hat es keine, der Spaltensatz und die Sortierung leben in der aktuellen Instanz.

## Was der Host tut

Der gesamte sichtbare Text stammt aus `host.t` — die Panelüberschrift, die Spaltenbeschriftungen, der Hinweis der leeren Tabelle, die Einträge des Kontextmenüs. Die Schaltfläche zum Schließen ruft `host.close()` auf: Das Panel entfernt sich nicht selbst. Beim Erstellen ruft das Steuerelement `host.register(this)` auf, bei `dispose()` `host.unregister(this)`. Eine Schaltfläche in der Seitenleiste gibt die Kette als XLSX aus.

Das Steuerelement abonniert keine Daten und sendet keine Aufträge: Die Kette und den Kontext übergibt ihm der Host mit der Methode `update`.

## Öffentliche Methoden

- `OptionDeskWidget.create(hostEl, state, deps)` — das Panel aufbauen und in den Container einfügen.
- `update(strikes, context)` — die Kette und den Bewertungskontext ersetzen.
- `rows()` — die Zeilen in der Form zurückgeben, in der das Desk sie hält: mit den berechneten Balkenskalen und dem inneren Wert.
- `dispose()` — Ressourcen freigeben und die Registrierung im Host aufheben.
- `OptionDeskWidget.TYPE` — die Kennung des Steuerelementtyps, `ControlTypes.OptionDesk`.

## Exportierte Funktionen

Der Rechenteil ist unabhängig vom Panel verfügbar:

- `scaleChain(strikes, context)` — berechnet in einem Durchlauf über die Kette die Maxima für die Balken und den inneren Wert jedes Strikes.
- `sideGreeks(row, which, context)` — die Griechen einer Seite des Strikes: die vom Host übergebenen oder die aus seiner Volatilität berechneten; `null`, wenn weder das eine noch das andere möglich ist.
- `greekPlaces(values)` — die Zahl der Nachkommastellen für eine Wertespalte.
- `greekScales(rows, context)` — die Zahl der Stellen für jeden Griechen, gemessen über beide Seiten der Kette zugleich.

## Siehe auch

- [JavaScript-Handelssteuerelemente](../trading_controls.md)
- [Instrumentenliste](watchlist.md)
- [Orderbuch](order_book.md)
- [Positionen](positions.md)
