# Preisrundung

## Einführung

Die Methode [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) in StockSharp ist ein wesentliches Werkzeug, um Preise gemäß Marktanforderungen korrekt zu runden. Dadurch wird sichergestellt, dass gesendete Orders den Regeln der Börse oder des Brokers entsprechen.

## Zweck

Das Hauptziel von [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) besteht darin, Preise auf zulässige Werte zu runden, wobei Folgendes berücksichtigt wird:
1. Der Preisschritt des Instruments ([Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep))
2. Die Anzahl der Dezimalstellen ([Security.Decimals](xref:StockSharp.BusinessEntities.Security.Decimals))

## Bedeutung der Verwendung

Die Verwendung von [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) ist entscheidend für:
- Die Vermeidung von Orderablehnungen durch Börse oder Broker aufgrund falscher Preise.
- Die Sicherstellung der Genauigkeit in Berechnungen und Handelsoperationen.
- Die Einhaltung der Regeln und Beschränkungen bestimmter Märkte oder Instrumente.

## Funktionsprinzip

1. Wenn [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep) gesetzt ist:
   - Der Preis wird auf den nächstgelegenen Wert gerundet, der ein Vielfaches des Preisschritts ist.
2. Wenn [Security.Decimals](xref:StockSharp.BusinessEntities.Security.Decimals) gesetzt ist:
   - Der Preis wird auf die angegebene Anzahl von Dezimalstellen gerundet.
3. Wenn beide Parameter gesetzt sind:
   - Es wird die strengere Rundung angewendet, normalerweise auf den Preisschritt.

## Verwendungsbeispiel

```cs
// Security-Objekt mit angegebenen Parametern erstellen
var security = new Security
{
	PriceStep = 0.01m,  // Preisschritt von 0.01
	Decimals = 2        // Zwei Dezimalstellen
};

// Beispiele für die Verwendung von ShrinkPrice

// Beispiel 1: Rundung auf den Preisschritt
decimal price1 = 10.234m;
decimal shrunkPrice1 = price1.ShrinkPrice(security);
Console.WriteLine($"Ursprünglicher Preis: {price1}, nach ShrinkPrice: {shrunkPrice1}");
// Ausgabe: ursprünglicher Preis: 10.234, nach ShrinkPrice: 10.23

// Beispiel 2: Rundung eines Preises, der bereits dem Schritt entspricht
decimal price2 = 10.22m;
decimal shrunkPrice2 = price2.ShrinkPrice(security);
Console.WriteLine($"Ursprünglicher Preis: {price2}, nach ShrinkPrice: {shrunkPrice2}");
// Ausgabe: ursprünglicher Preis: 10.22, nach ShrinkPrice: 10.22

// Beispiel 3: Rundung eines Preises mit mehr Dezimalstellen
decimal price3 = 10.2345678m;
decimal shrunkPrice3 = price3.ShrinkPrice(security);
Console.WriteLine($"Ursprünglicher Preis: {price3}, nach ShrinkPrice: {shrunkPrice3}");
// Ausgabe: ursprünglicher Preis: 10.2345678, nach ShrinkPrice: 10.23

// Beispiel 4: Verwendung von ShrinkPrice beim Erstellen einer Order
var order = new Order
{
	Security = security,
	Price = 10.237m.ShrinkPrice(security)  // Preis vor dem Erstellen der Order runden
};
Console.WriteLine($"Auftragspreis: {order.Price}");
// Ausgabe: Auftragspreis: 10.24
```

## Anwendung

[ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) sollte vor dem Senden von Orders oder vor Berechnungen verwendet werden, die eine genaue Preisübereinstimmung mit Marktbedingungen erfordern.

## Fazit

Die korrekte Verwendung von [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)) hilft, Fehler beim Platzieren von Orders zu vermeiden, und stellt sicher, dass Handelsalgorithmen gemäß den Marktanforderungen korrekt arbeiten.

