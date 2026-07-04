
# Verbindung einrichten

Zusätzlich zum [Exportieren von Einstellungen über Designer](export_from_designer.md) bietet **Runner** die Möglichkeit, das Programm über seine Konsolenoberfläche zu konfigurieren. Führen Sie dazu das Programm mit dem Befehl **setup** aus:

```cmd
stocksharp.studio.runner setup
```

Ein Menü erscheint:

![runner_setup_1](../../images/runner_setup_1.png)

Wenn Sie den Menüpunkt Connections auswählen, wechselt das Programm in den Modus zur Einrichtung des Connectors:

![runner_setup_2](../../images/runner_setup_2.png)

Hier können Sie entweder eine zuvor gespeicherte Verbindung bearbeiten oder eine neue erstellen:

![runner_setup_3](../../images/runner_setup_3.png)

Nach Auswahl des erforderlichen Typs der neuen Verbindung wechselt das Programm in das Bearbeitungsmenü für deren Einstellungen:

![runner_setup_4](../../images/runner_setup_4.png)

Für [Binance](../api/connectors/crypto_exchanges/binance.md) müssen die Haupteinstellungen eingegeben werden:

![runner_setup_5](../../images/runner_setup_5.png)

![runner_setup_6](../../images/runner_setup_6.png)

![runner_setup_7](../../images/runner_setup_7.png)

Um die Richtigkeit der eingegebenen Daten zu prüfen, wählen Sie **Check**:

![runner_setup_8](../../images/runner_setup_8.png)

Die Verbindungsprüfung wird gestartet:

![runner_setup_9](../../images/runner_setup_9.png)

Bei Erfolg wird eine Meldung angezeigt:

![runner_setup_10](../../images/runner_setup_10.png)

Nachdem alle Einstellungen eingegeben und geprüft wurden, müssen Sie **Save** drücken:

![runner_setup_11](../../images/runner_setup_11.png)

Im Ordner Data wird eine Datei **connector.json** erstellt (falls sie nicht bereits vorhanden war), die die gespeicherten Einstellungen enthält.

Um die Integration mit [Telegram](../telegram_services.md) einzurichten, wählen Sie den Menüpunkt:

![runner_telegram_1](../../images/runner_telegram_1.png)

Und authentifizieren Sie sich mit einer geeigneten Methode:

![runner_telegram_2](../../images/runner_telegram_2.png)

Für die Authentifizierung per Token geben Sie das Token von [https://stocksharp.ru/profile/](https://stocksharp.ru/profile/) ein:

![Profile](../../images/profile.png)

Bei Erfolg zeigt das Programm die verfügbaren Telegram-Optionen an:

![runner_telegram_3](../../images/runner_telegram_3.png)
