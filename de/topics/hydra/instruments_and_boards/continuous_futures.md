# Fortlaufende Futures

Das Programm [Hydra](../../hydra.md) ermöglicht es dem Benutzer, verschiedene Typen von Marktdaten aus unterschiedlichen Kontrakten zu einem einzigen fortlaufenden Instrument zusammenzufassen.

Wählen Sie dazu auf der Registerkarte **Allgemein** den Punkt **Instrumente**, sodass die Registerkarte **Alle Instrumente** angezeigt wird. Prüfen Sie vor dem Zusammenfassen der Daten, welche Marktdaten verfügbar sind. Wählen Sie den Pfad aus, in dem sich die Daten befinden, und prüfen Sie die Instrumente, die Sie zusammenfassen möchten. Wenn Lücken vorhanden sind, laden Sie die fehlenden Marktdaten herunter, zum Beispiel aus einer unterstützten Datenquelle.

![Hydra Prüfung der Continuous-Futures-Daten](../../../images/hydragluingcheckdata.png)

Als Beispiel betrachten wir das Zusammenfassen von E-mini S&P 500 Futures.

1. Um einen fortlaufenden Futures-Kontrakt zu erstellen, klicken Sie auf der Registerkarte **Alle Instrumente** auf die Schaltfläche **Instrument erstellen \=\> Fortlaufendes Instrument**.![Hydra Datenprüfung beim Zusammenfügen 00](../../../images/hydragluingcheckdata_00.png)

   Danach erscheint das folgende Fenster:![Fortlaufende Futures Bildschirmfoto 1](../../../images/hydragluingwindow.png)
2. Um einen fortlaufenden Future zu erstellen, müssen Sie einen Namen angeben und Kontrakte hinzufügen.

   Es gibt zwei Möglichkeiten, Kontrakte hinzuzufügen.
   - Manuell durch Klicken auf die Schaltfläche ![Hydra Schaltfläche Hinzufügen](../../../images/hydra_add.png).![Hydra benutzerdefinierter Continuous Future](../../../images/hydragluingcscustom.png)
   - Wenn Sie die ersten beiden Buchstaben des Kontrakts als Namen festlegen, zum Beispiel RI, und auf die Schaltfläche **Auto** klicken, werden alle in der Datenbank gefundenen Instrumente hinzugefügt.![Fortlaufende Futures Bildschirmfoto 2](../../../images/hydragluingcsauto.png)
3. Wählen Sie die erforderlichen Kontrakte aus und legen Sie deren Übergangsdaten fest. ![Fortlaufende Futures Bildschirmfoto 3](../../../images/hydragluingcsauto_00.png)
4. Weisen Sie anschließend die Instrumentkennung **ES\_continuous@CME** zu und klicken Sie auf die Schaltfläche **OK**. Danach wird ein neues Instrument erstellt.
5. Klicken Sie danach auf der Registerkarte **Allgemein** auf die Schaltfläche [Kerzen](../working_with_data/view_and_export/candles.md), wählen Sie das resultierende Instrument und den Datenzeitraum aus, setzen Sie im Feld **Erstellen aus** den Wert **Zusammengesetztes Element** und klicken Sie dann auf die Schaltfläche ![Hydra Suchschaltfläche](../../../images/hydra_find.png). ![Hydra Trades für Continuous Futures](../../../images/hydragluingtrades.png)

Die erzeugten Daten können in die Formate Excel, XML, JSON oder TXT exportiert werden. Der Export erfolgt über die Dropdown-Liste.

![Hydra Datenexport](../../../images/hydra_export.png)
