# Instrumente

In StockSharp werden Finanzinstrumente durch die Klasse [Security](xref:StockSharp.BusinessEntities.Security) dargestellt. Sie ist ein grundlegendes Element für die Arbeit mit Handelsdaten. Dieser Abschnitt behandelt die wichtigsten Aspekte der Arbeit mit Finanzinstrumenten innerhalb der Plattform.

## Basisklasse Security

[Security](xref:StockSharp.BusinessEntities.Security) stellt ein an einer Börse gehandeltes Finanzinstrument dar. Ein Instrument kann eine Aktie, ein Futures-Kontrakt, eine Option, ein Währungspaar, eine Kryptowährung oder ein anderes Asset sein. Die Klasse enthält alle notwendigen Informationen zur Identifikation und zum Handel des Instruments:

- **Identifikationsinformationen** - Code, ISIN, Name, Instrumentenklasse
- **Handelsparameter** - Preisschritt, Lotgröße, Mindestvolumen
- **Marktdaten** - aktuelle Werte von Preisen, Volumina, Orderbüchern usw.
- **Berechnete Werte** - Parameter für Derivate, Risikoberechnung usw.

## Instrumententypen

StockSharp unterstützt die Arbeit mit allen wichtigen Typen von Finanzinstrumenten:

- **Aktien** - Eigenkapitalinstrumente
- **Anleihen** - Schuldverschreibungen
- **Futures** - derivative Kontrakte auf einen Basiswert
- **Optionen** - Kontrakte, die das Recht, aber nicht die Pflicht, zum Kauf oder Verkauf eines Basiswerts geben
- **Währungspaare** - Instrumente für den Handel am Devisenmarkt
- **Kryptowährungen** - digitale Assets für den Handel an Kryptobörsen
- **ETFs** - börsengehandelte Fonds
- **Indizes** - berechnete Indikatoren für den Zustand eines Marktes oder Sektors

## Instrumentenkörbe

Zusätzlich zu regulären Instrumenten implementiert StockSharp spezielle Klassen für die Arbeit mit Gruppen von Instrumenten:

- [IndexSecurity](xref:StockSharp.Algo.IndexSecurity) - ein Instrument, das einen Index auf Basis zugrunde liegender Instrumente darstellt
- [WeightedIndexSecurity](xref:StockSharp.Algo.WeightedIndexSecurity) - ein Index mit Gewichtungskoeffizienten für jedes Instrument
- [ContinuousSecurity](xref:StockSharp.Algo.ContinuousSecurity) - ein kontinuierliches Instrument für die Arbeit mit einer Reihe von Futures-Kontrakten

Diese Klassen ermöglichen das Erstellen zusammengesetzter Instrumente und die Arbeit mit ihnen auf dieselbe Weise wie mit regulären Instrumenten: Sie können aggregierte Marktdaten empfangen, Statistiken berechnen und Handelsoperationen ausführen.

## Arbeiten mit Instrumenteninformationen

StockSharp stellt leistungsfähige Werkzeuge für die Arbeit mit Informationen zu Finanzinstrumenten bereit:

- **Instrumentensuche** - nach verschiedenen Kriterien (Code, Name, Klasse)
- **Filterung** - Auswahl von Instrumenten nach angegebenen Parametern
- **Speicherung** - Speichern von Instrumenteninformationen in lokalem oder entferntem Speicher
- **Abrufen von Börseninformationen** - Laden detaillierter Informationen von der Börse

## Instrumentenidentifikation

Jedes Instrument in StockSharp besitzt einen eindeutigen Bezeichner [SecurityId](xref:StockSharp.Messages.SecurityId), der zur eindeutigen Identifikation des Instruments im System verwendet wird. Der Bezeichner enthält:

- **Instrumentencode** - Börsencode des Instruments
- **Handelsplatzcode** - Code des Handelsplatzes
- **Bloomberg/Reuters/ISIN** und andere Codes - alternative Identifikationsmethoden

## Besonderheiten

- **Continuous Futures** - automatisches "Zusammenfügen" historischer Daten für eine Reihe von Futures-Kontrakten
- **Zusammengesetzte Instrumente** - Erstellung virtueller Instrumente auf Basis mehrerer realer Instrumente
- **Spezialbezeichner \*@ALL** - für die Arbeit mit allen Instrumenten einer bestimmten Klasse

## Siehe auch

[Instrumentenbezeichner](instruments/instrument_identifier.md)

[Identifier \*@ALL](instruments/identifier_@all.md)

[Fortlaufende Futures](instruments/continuous_futures.md)

[Index](instruments/index.md)

[Instrumentensuche](instruments/instrument_search.md)

