# Bedienfeld

Ein Dienst zur Verwaltung von Handelsstrategien und Robotern über einen Telegram-Bot.

Gehen Sie für die Einrichtung vorher durch den [Autorisierungsprozess für den Bot](authorization.md).

Danach ist der Bot einsatzbereit. Damit der Bot Ihre Strategien sehen kann, müssen Sie anschließend:

 - Bei Verwendung des Programms [Designer](../designer.md) im Cloud-Panel den Fernmodus aktivieren:

  ![Bedienfeld 01](../../images/designerribbon.png)

  Alle Strategien, die im Live-Modus laufen, werden automatisch an den Telegram-Bot übertragen, und Sie können sie von Ihrem Telefon aus steuern.

  Wählen Sie in [StockSharpBot](https://t.me/StockSharpBot) den Befehl /apps aus, um eine Liste aller Programme zu sehen:

  ![Bedienfeld 02](../../images/telegramcontrolapps.png)

  Nachdem das gewünschte Programm ausgewählt wurde, können Sie die Strategien und ihre Steuerelemente sehen:

  ![Bedienfeld 03](../../images/telegramcontrolapp.png)

  ![Bedienfeld 04](../../images/telegramcontrolstrategies.png)

  ![Bedienfeld 05](../../images/telegramcontrolstrategy.png)

 - Bei Verwendung von [Shell](../shell.md) öffnen Sie das Panel RemoteManager und konfigurieren die Einstellungen ähnlich wie in [Designer](../designer.md).
 - Bei Verwendung von [Hydra](../hydra.md) führen Sie ähnliche Aktionen wie in [Designer](../designer.md) aus. Die Integration mit [Hydra](../hydra.md) ermöglicht das Verwalten des Marktdaten-Downloads und das Überwachen quantitativer Statistiken.

  ![Bedienfeld 06](../../images/telegramhydra.png)
  ![Bedienfeld 07](../../images/telegramhydrastat.png)

 - Bei Verwendung von [S#](../api.md) können Sie die Integration mithilfe des Codes aus [Shell](../shell.md) vornehmen. Da [S#](../api.md) plattformübergreifend ist, können Ihre Roboter auf jedem Betriebssystem laufen.
