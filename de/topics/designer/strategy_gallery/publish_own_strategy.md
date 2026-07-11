# Eigene Strategie veröffentlichen

Sie können Ihre Strategie veröffentlichen, indem Sie im Panel [Schemata-Panel](../user_interface/schemas.md) mit der linken Maustaste auf die Strategie klicken und **Veröffentlichen** auswählen:

![Eigene Strategie veröffentlichen 00](../../../images/designer_publish_00.png)

Nach dem Klicken auf die Schaltfläche **Veröffentlichen** öffnet sich ein Fenster mit der Auswahl des Exporttyps. Weitere Details finden Sie im Abschnitt [Strategien exportieren](../export_import/export.md).

Nach der Auswahl des Exporttyps wird das Programm [Installer](../../installer.md) mit den Veröffentlichungsparametern aktiviert ([Installer](../../installer.md) muss zuvor gestartet worden sein):

![Eigene Strategie veröffentlichen 01](../../../images/designer_publish_01.png)

Auszufüllende Felder:

- Name
- Beschreibung
- NuGet-Paketkennung. Dieser Parameter wird benötigt, um den Link zum Produkt im Store festzulegen. Zum Beispiel wird in der Adresse https://stocksharp.com/store/runner/ das Wort **runner** über diesen Parameter angegeben.

Zugriff auf die Stufen **Kostenlos** oder **Kostenpflichtig** wird erst nach Kontaktaufnahme per E-Mail an [info@stocksharp.com](mailto:info@stocksharp.com) gewährt. Standardmäßig ist die Stufe **Privat** verfügbar; sie erlaubt das Veröffentlichen von Strategien nur in privatem Format (für ausgewählte Benutzer):

Nach dem Klicken auf die Schaltfläche **Speichern** wird die Strategie an den StockSharp-Server gesendet.

Beim Veröffentlichen von Updates müssen nicht alle Parameter erneut eingegeben werden. Statt der Eingabe von Produktparametern erscheint ein Fenster zur Eingabe einer Notiz für das Update:

![Eigene Strategie veröffentlichen 02](../../../images/designer_publish_02.png)

Nach dem Klicken auf die Schaltfläche **OK** erscheint ein Fenster, das ein erfolgreiches Update meldet:

![Eigene Strategie veröffentlichen 03](../../../images/designer_publish_03.png)

