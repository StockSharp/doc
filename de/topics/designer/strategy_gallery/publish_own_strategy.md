# Eigene Strategie veröffentlichen

Sie können Ihre Strategie veröffentlichen, indem Sie im Panel [Schemes](../user_interface/schemas.md) mit der linken Maustaste auf die Strategie klicken und **Publish** auswählen:

![Designer_publish_00](../../../images/designer_publish_00.png)

Nach dem Klicken auf die Schaltfläche **Publish** öffnet sich ein Fenster mit der Auswahl des Exporttyps. Weitere Details finden Sie im Abschnitt [Exporting Strategies](../export_import/export.md).

Nach der Auswahl des Exporttyps wird das Programm [Installer](../../installer.md) mit den Veröffentlichungsparametern aktiviert ([Installer](../../installer.md) muss zuvor gestartet worden sein):

![Designer_publish_01](../../../images/designer_publish_01.png)

Auszufüllende Felder:

- Name
- Description
- Nuget package identifier. Dieser Parameter wird benötigt, um den Link zum Produkt im Store festzulegen. Zum Beispiel wird in der Adresse https://stocksharp.com/store/runner/ das Wort **runner** über diesen Parameter angegeben.

Zugriff auf die Stufen **Free** oder **Paid** wird erst nach Kontaktaufnahme per E-Mail an [info@stocksharp.com](mailto:info@stocksharp.com) gewährt. Standardmäßig ist die Stufe **Private** verfügbar; sie erlaubt das Veröffentlichen von Strategien nur in privatem Format (für ausgewählte Benutzer):

Nach dem Klicken auf die Schaltfläche **Speichern** wird die Strategie an den StockSharp-Server gesendet.

Beim Veröffentlichen von Updates müssen nicht alle Parameter erneut eingegeben werden. Statt der Eingabe von Produktparametern erscheint ein Fenster zur Eingabe einer Notiz für das Update:

![Designer_publish_02](../../../images/designer_publish_02.png)

Nach dem Klicken auf die Schaltfläche **OK** erscheint ein Fenster, das ein erfolgreiches Update meldet:

![Designer_publish_03](../../../images/designer_publish_03.png)

