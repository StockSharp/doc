
# Export aus Designer

**Runner** ermöglicht das Ausführen von Strategien, die in [Designer](../designer.md) erstellt wurden. Dies ist die bequemste Möglichkeit, **Runner** einzurichten, da alle Konfigurationen visuell vorgenommen werden.

So exportieren Sie eine Strategie aus [Designer](../designer.md):

- Wählen Sie die gewünschte Strategie in der Baumansicht aus, klicken Sie mit der rechten Maustaste darauf und wählen Sie den Menüpunkt **Runner**:

  ![Designer Runner-Export](../../images/designer_runner_1.png)

- Im angezeigten Fenster müssen Sie auswählen, welche Verbindungstypen nach **Runner** exportiert werden sollen, sowie die Einstellungen zur Verwaltung der Strategie über [Telegram](../telegram_services.md):

  ![Designer Runner-Export](../../images/designer_runner_2.png)

Die folgenden Dateien werden in das ausgewählte Exportverzeichnis kopiert:

- connector.json - eine Datei mit Verbindungseinstellungen
- params.json - eine Datei mit Strategieparametern
- start.bat - eine bat-Datei mit bereits eingetragener Befehlszeile für den schnellen Start von **Runner**
- strategy.json - eine Datei mit der Strategie
- connector.json - eine Datei mit Verbindungseinstellungen
- telegram.json - eine Datei mit Einstellungen für die Integration mit [Telegram](../telegram_services.md)
