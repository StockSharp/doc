# Benachrichtigungen

[Terminal](../terminal.md) meldet wichtige Ereignisse mit Popup-Fenstern in der Bildschirmecke: Eine Verbindung wurde hergestellt oder getrennt, ein eigener Trade wurde ausgeführt, eine Order konnte nicht storniert werden. Ein Verbindungsfehler wird in einem Fenster mit einer Warnung angezeigt.

Über Ereignisse von Orders und Verbindungen kann Terminal auch auf andere Weise informieren. Diese Möglichkeiten werden im Fenster [Einstellungen](settings/common_settings.md) (**Einstellungen → Einstellungen**) in der Gruppe **Benachrichtigung** für jedes Ereignis einzeln ausgewählt:

- **Orderänderung** — eine Order hat sich geändert;
- **Matching** — eine Order wurde ausgeführt;
- **Fehler** — eine Order wurde nicht angenommen;
- **Verloren** und **Wiederhergestellt** — die Verbindung ist verloren gegangen und wurde wiederhergestellt.

Für jedes Ereignis können Sie wählen, wie darüber informiert wird:

- **Ton** - ein Tonsignal wird abgespielt.
- **Popup-Fenster** - in der Bildschirmecke erscheint ein kleines Fenster mit einer Nachricht.
- **Protokolldatei** - die Nachricht wird in das Protokoll geschrieben, das im Panel [Protokolle](user_interface/logs.md) zu sehen ist.
- **Telegram** - die Nachricht wird an den Telegram-Kanal gesendet, der in derselben Einstellungsgruppe angegeben wird. Die Kanäle werden aus dem StockSharp-Konto genommen, deshalb müssen Sie sich für diese Möglichkeit auf der Website anmelden.

Wenn keine Möglichkeit ausgewählt ist, wird über das Ereignis nicht informiert.

## Siehe auch

[Allgemeine Einstellungen](settings/common_settings.md)

[Hauptmenü](user_interface/ribbon.md)
