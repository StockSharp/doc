# Orderverwaltung

[S#](../api.md) bietet umfangreiche Funktionen für die effiziente Verwaltung von Handelsorders in allen Phasen ihres Lebenszyklus. Dieser Abschnitt behandelt die wichtigsten Aspekte der Arbeit mit Orders in Handelsanwendungen.

## Hauptfunktionen

- **Ordererstellung** - Bildung verschiedener Typen von Handelsorders (Market-, Limit-, Stop-Orders usw.)
- **Statusverfolgung** - Empfang aktueller Informationen über den aktuellen Status von Orders
- **Orderverwaltung** - Stornierung, Änderung und Ersetzung bestehender Orders
- **Ereignisverarbeitung** - Reaktion auf Ereignisse der Registrierung, Ausführung und Stornierung von Orders
- **Massenoperationen** - effiziente Arbeit mit Gruppen von Orders

## Orderlebenszyklus

Jede Order in S# durchläuft bestimmte Lebenszyklusphasen:

1. **Erstellung** - Bildung eines [Order](xref:StockSharp.BusinessEntities.Order)-Objekts mit den erforderlichen Parametern
2. **Registrierung** - Senden der Order an das Handelssystem
3. **Ausführung** - teilweise oder vollständige Ausführung der Order, Bildung von Trades
4. **Abschluss** - vollständige Ausführung, Stornierung oder Ablehnung der Order

Die API stellt detaillierte Informationen über den Zustand der Order in jeder Phase bereit, wodurch komplexe Handelsalgorithmen mit präziser Ausführungskontrolle erstellt werden können.

## Integration mit Handelsstrategien

Der Mechanismus zur Orderverwaltung ist eng mit Komponenten zur Entwicklung von Handelsstrategien [Strategy](xref:StockSharp.Algo.Strategies.Strategy) integriert. Dadurch können Sie:

- Orderverwaltungslogik innerhalb der Strategie kapseln
- Ereignisse der Orderregistrierung und -ausführung automatisch verfolgen und verarbeiten
- Einen einheitlichen Ansatz zur Orderverwaltung sowohl im realen Handel als auch beim Testen verwenden

## Siehe auch

[Create a New Order](orders_management/create_new_order.md)

[Create a New Stop Order](orders_management/create_new_stop_order.md)

[Order States](orders_management/orders_states.md)

[Order Cancellation](orders_management/order_cancel.md)

[Bulk Order Cancellation](orders_management/orders_mass_cancel.md)

[Order Replacement](orders_management/orders_replacement.md)

[Transaction Number](orders_management/transaction_number.md)

