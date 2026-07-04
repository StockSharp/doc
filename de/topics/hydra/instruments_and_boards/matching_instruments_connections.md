# Instrumente-Verbindungen zuordnen

Dasselbe Instrument kann in verschiedenen Handelssystemen unterschiedlich bezeichnet werden. Sie koennen das Instrument und die Verbindungen zuordnen, ueber die dieses Instrument gehandelt wird, und festlegen, wie es im externen Handelssystem identifiziert wird.

Dadurch koennen Sie die empfangenen Daten organisieren und die Speicherung vereinfachen. Tatsaechlich werden alle eingehenden Daten aus verschiedenen Quellen an einem Ort konsolidiert, und zwar nicht nach dem Quellnamen, sondern nach dem Instrumentennamen.

Dies ist auch nuetzlich, wenn dasselbe Instrument auf verschiedenen Trading Boards oder ueber unterschiedliche Verbindungen (oder Broker) gehandelt wird. Ausserdem koennen Daten ueber eine Verbindung empfangen und Trades ueber eine andere Verbindung ausgefuehrt werden.

Um Instrumente und Verbindungen zuzuordnen, gehen Sie wie folgt vor:

1. Wechseln Sie zur Registerkarte **Securities** und klicken Sie auf die Schaltflaeche **Securities and Connections**.![Designer Security mapping 01 00](../../../images/designer_security_mapping_01_00.png)
2. Waehlen Sie in der Verbindungsliste die erforderliche Verbindung aus.![Designer Security mapping 01](../../../images/designer_security_mapping_01.png)
3. Fuellen Sie alle Spalten aus.

   Beispiel:

   APPLE-Aktieninstrument.
   - Connection - **Interactive Brokers**. Klicken Sie auf die Schaltflaeche ![Designer Creation tool 00](../../../images/designer_creation_tool_00.png), danach wird eine neue Zeile hinzugefuegt.
   - Geben Sie in den Spalten **Security** code und **Board code** den Instrumentcode und den Board-Code an. Geben Sie in den Spalten **Security code in adapter** und **Board code in adapter** den Instrumentcode und den Board-Code so an, wie sie im externen Handelssystem angegeben sind. Klicken Sie auf **OK** ![Designer Security mapping 01 01](../../../images/designer_security_mapping_01_01.png)
   - Wiederholen Sie die Schritte fuer die Verbindungen **Interactive Brokers** und **CQG Continuum** auf die gleiche Weise.

   | **Interactive Brokers**                                                           | **CQG Continuum**                                                                 |
   | --------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- |
   | ![Designer Security mapping 01 02](../../../images/designer_security_mapping_01_02.png) | ![Designer Security mapping 01 03](../../../images/designer_security_mapping_01_03.png) |
4. Nun werden alle heruntergeladenen Daten, in unserem Fall fuer APPLE-Aktien, an einem Ort gespeichert.
