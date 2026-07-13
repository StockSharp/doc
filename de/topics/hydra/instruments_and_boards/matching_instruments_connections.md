# Instrumente-Verbindungen zuordnen

Dasselbe Instrument kann in verschiedenen Handelssystemen unterschiedlich bezeichnet werden. Sie können das Instrument und die Verbindungen zuordnen, über die dieses Instrument gehandelt wird, und festlegen, wie es im externen Handelssystem identifiziert wird.

Dadurch können Sie die empfangenen Daten organisieren und die Speicherung vereinfachen. Tatsächlich werden alle eingehenden Daten aus verschiedenen Quellen an einem Ort konsolidiert, und zwar nicht nach dem Quellnamen, sondern nach dem Instrumentennamen.

Dies ist auch nützlich, wenn dasselbe Instrument auf verschiedenen Handelsplätze oder über unterschiedliche Verbindungen (oder Broker) gehandelt wird. Außerdem können Daten über eine Verbindung empfangen und Trades über eine andere Verbindung ausgeführt werden.

Um Instrumente und Verbindungen zuzuordnen, gehen Sie wie folgt vor:

1. Wechseln Sie zur Registerkarte **Instrumente** und klicken Sie auf die Schaltfläche **Instrumente und Verbindungen**.![Designer Instrumentzuordnung 01 00](../../../images/designer_security_mapping_01_00.png)
2. Wählen Sie in der Verbindungsliste die erforderliche Verbindung aus.![Designer Instrumentzuordnung 01](../../../images/designer_security_mapping_01.png)
3. Füllen Sie alle Spalten aus.

   Beispiel:

   APPLE-Aktieninstrument.
   - Verbindung - **Interactive Brokers**. Klicken Sie auf die Schaltfläche ![Designer Schaltfläche Hinzufügen 00](../../../images/designer_creation_tool_00.png), danach wird eine neue Zeile hinzugefügt.
   - Geben Sie in den Spalten **Instrumentcode** und **Handelsplatzcode** den Instrumentcode und den Handelsplatzcode an. Geben Sie in den Spalten **Instrumentcode im Adapter** und **Handelsplatzcode im Adapter** den Instrumentcode und den Handelsplatzcode so an, wie sie im externen Handelssystem angegeben sind. Klicken Sie auf **OK** ![Designer Instrumentzuordnung 01 01](../../../images/designer_security_mapping_01_01.png)
   - Wiederholen Sie die Schritte für die Verbindungen **Interactive Brokers** und **CQG Continuum** auf die gleiche Weise.

   | **Interactive Brokers**                                                           | **CQG Continuum**                                                                 |
   | --------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- |
   | ![Designer Instrumentzuordnung 01 02](../../../images/designer_security_mapping_01_02.png) | ![Designer Instrumentzuordnung 01 03](../../../images/designer_security_mapping_01_03.png) |
4. Nun werden alle heruntergeladenen Daten, in unserem Fall für APPLE-Aktien, an einem Ort gespeichert.
