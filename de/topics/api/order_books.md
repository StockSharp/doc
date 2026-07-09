# Orderbuch

## Beschreibung

Das Orderbuch (auch Market Depth) enthält Informationen über aktuelle Kauf- und Verkaufsorders für ein bestimmtes Wertpapier, organisiert nach Preisniveaus. In StockSharp liefert das Orderbuch Daten zu Nachfrage und Angebot und ermöglicht Marktanalysen in Echtzeit.

## Struktur

Das [Orderbuch](xref:StockSharp.Messages.IOrderBookMessage) enthält zwei Orderlisten:

- Kauforders, absteigend nach Preis sortiert - [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids).
- Verkaufsorders, aufsteigend nach Preis sortiert - [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks).

Jede Order enthält Preis und Volumen.

## Verwendung

Orderbuchdaten werden verwendet für:

- Identifikation von Preisniveaus mit maximalen Ordervolumina, die auf mögliche Unterstützungs- oder Widerstandsniveaus hinweisen können.
- Einschätzung der Marktliquidität für ein Wertpapier.
- Entwicklung von Handelsstrategien auf Basis der Analyse von Änderungen im Orderbuch.

## Datenabruf

In StockSharp erfolgt das Abonnieren von Orderbuchdaten und der Empfang von Aktualisierungen über die entsprechenden [API-Methoden](order_books/subscriptions.md).
