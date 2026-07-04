# Fortlaufende Futures

Das Programm [Hydra](../../hydra.md) ermoeglicht es dem Benutzer, verschiedene Typen von Marktdaten aus unterschiedlichen Kontrakten zu einem einzigen fortlaufenden Instrument zusammenzufassen.

Waehlen Sie dazu auf der Registerkarte **Common** den Punkt **Securities**, sodass die Registerkarte **All securities** angezeigt wird. Pruefen Sie vor dem Zusammenfassen der Daten, welche Marktdaten verfuegbar sind. Waehlen Sie den Pfad aus, in dem sich die Daten befinden, und pruefen Sie die Instrumente, die Sie zusammenfassen moechten. Wenn Luecken vorhanden sind, laden Sie die fehlenden Marktdaten herunter, zum Beispiel aus einer unterstuetzten Datenquelle.

![HydraGluingCheckData](../../../images/hydragluingcheckdata.png)

Als Beispiel betrachten wir das Zusammenfassen von E-mini S&P 500 Futures.

1. Um einen fortlaufenden Futures-Kontrakt zu erstellen, klicken Sie auf der Registerkarte **All securities** auf die Schaltflaeche **Create security \=\> Continuous security**.![Hydra Gluing Check Data 00](../../../images/hydragluingcheckdata_00.png)

   Danach erscheint das folgende Fenster:![HydraGluingWindow](../../../images/hydragluingwindow.png)
2. Um einen fortlaufenden Future zu erstellen, muessen Sie einen Namen angeben und Kontrakte hinzufuegen.

   Es gibt zwei Moeglichkeiten, Kontrakte hinzuzufuegen.
   - Manuell durch Klicken auf die Schaltflaeche ![hydra add](../../../images/hydra_add.png).![HydraGluingCSCustom](../../../images/hydragluingcscustom.png)
   - Wenn Sie die ersten beiden Buchstaben des Kontrakts als Namen festlegen, zum Beispiel RI, und auf die Schaltflaeche **Auto** klicken, werden alle in der Datenbank gefundenen Instrumente hinzugefuegt.![HydraGluingCSAuto](../../../images/hydragluingcsauto.png)
3. Waehlen Sie die erforderlichen Kontrakte aus und legen Sie deren Uebergangsdaten fest. ![Hydra GluingCSAuto 00](../../../images/hydragluingcsauto_00.png)
4. Weisen Sie anschliessend die Instrumentkennung **ES\_continuous@CME** zu und klicken Sie auf die Schaltflaeche **OK**. Danach wird ein neues Instrument erstellt.
5. Klicken Sie danach auf der Registerkarte **Common** auf die Schaltflaeche [Candles](../working_with_data/view_and_export/candles.md), waehlen Sie das resultierende Instrument und den Datenzeitraum aus, setzen Sie im Feld **Build from** den Wert **Composite element** und klicken Sie dann auf die Schaltflaeche ![hydra find](../../../images/hydra_find.png). ![HydraGluingTrades](../../../images/hydragluingtrades.png)

Die erzeugten Daten koennen in die Formate Excel, XML, JSON oder TXT exportiert werden. Der Export erfolgt ueber die Dropdown-Liste.

![hydra export](../../../images/hydra_export.png)
