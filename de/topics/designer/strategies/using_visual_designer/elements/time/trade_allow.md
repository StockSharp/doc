# Handel erlaubt

![Handel erlaubt Bildschirmfoto](../../../../../../images/designer_tradealloweddiagramelement_00.png)

Dieser Block wird verwendet, um zu prüfen, ob der Handel aktuell erlaubt ist. Die folgenden Bedingungen werden geprüft:

- Alle Strategie-Abonnements für Marktdaten müssen sich im Zustand [Online](../../../../../api/market_data/subscriptions.md) befinden (Empfang von Echtzeitdaten).
- Alle Indikatoren müssen [formed](../../../../../api/indicators.md) sein.
- Bei [Live-Handel](../../../../live_execution/getting_started.md) muss der eingehende Triggerwert einen Zeitstempel haben, der größer ist als die Startzeit der Strategie.

### Eingehende Sockets


- **Auslöser** - das Signal, das bestimmt, wann die Prüfung ausgeführt werden soll.

### Ausgehende Sockets


- **Markierung** - ein Flag, das bestimmt, ob die Handelssitzung aktiv ist.

## Siehe auch

[Aktuelle Zeit](current_time.md)

