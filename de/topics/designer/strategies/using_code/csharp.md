# C# verwenden

Das Erstellen von Strategien aus Code richtet sich an Benutzer, die bevorzugt mit C#-Code arbeiten. Solche Strategien sind im Gegensatz zu Diagrammen nicht in ihren Möglichkeiten eingeschränkt, und jeder Algorithmus kann beschrieben werden.

Der Prozess zum Erstellen einer Strategie findet direkt in [Designer](../../../designer.md) oder in einer **C#**-Entwicklungsumgebung statt (die beliebtesten sind **Visual Studio** und **JetBrains Rider**). Dabei wird eine Bibliothek für die professionelle Entwicklung von Handelsrobotern in **C#** und [API](../../../api.md) verwendet.

Sie können eine neue Strategie hinzufügen, indem Sie im Tab **Common** auf die Schaltfläche **Add** ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png) klicken und **Strategy** auswählen. Alternativ klicken Sie mit der rechten Maustaste auf den Ordner **Strategies** im Panel **Scheme** und anschließend im Dropdown-Menü auf die Schaltfläche **Add** ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png):

![Designer The creation of a strategy 00](../../../../images/designer_creation_of_strategy_00.png)

Nach dem Klicken auf die Schaltfläche **Add** ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png) erscheint ein Fenster, in dem der Inhaltstyp ausgewählt wird, auf dessen Grundlage die Strategie erstellt werden soll:

![Designer_Creation_of_element_containing_source_code_00](../../../../images/designer_creation_of_element_containing_source_code_00.png)

Um eine Strategie aus C#-Code zu erstellen, müssen Sie den zweiten Tab auswählen. Sie können außerdem eine Vorlage auswählen, die als Anfangscode verwendet wird.

Nach dem Klicken auf **OK** erscheint eine neue Strategie im Ordner **Strategies** des Panels **Scheme**, ähnlich wie beim Erstellen einer Strategie aus [Diagrammen](../using_visual_designer.md). Auch die Aktionen zum Löschen oder Umbenennen der Strategie sind ähnlich.

Anstelle eines Diagramms wird jedoch ein C#-Code-Editor angezeigt:

![Designer_Creation_of_element_containing_source_code_01](../../../../images/designer_creation_of_element_containing_source_code_01.png)

Der Code-Editor-Tab besteht aus den Panels **Source Code** und **Error List**. Das Panel **Source Code** enthält den eigentlichen C#-Code-Editor. Oben befindet sich eine Symbolleiste, in der Hervorhebungen wie **Current Line**, **Line Number** usw. ein- oder ausgeschaltet werden können. Um die Schriftgröße zu erhöhen, können Sie die Kombination CTRL+MouseWheel verwenden.

Das Panel **Error List** ist eine Tabelle mit einer Liste der Fehler im Code. Ein Doppelklick auf eine Zeile bewegt den Cursor im Panel **Source Code** automatisch an die Fehlerstelle.

Beim Bearbeiten des Codes erscheint in der rechten unteren Ecke des Panels **Error List** ein Symbol ![Designer The creation of the cube containing the source code 03](../../../../images/designer_creation_of_element_containing_source_code_03.png), das anzeigt, dass die Änderungsverfolgung begonnen hat. Der Code wird kompiliert, sobald sich der Code nicht mehr ändert.

Das Ausführen der Strategie im [Backtest](../../backtesting/user_interface.md), im [Live-Betrieb](../../live_execution/getting_started.md) und andere Operationen funktionieren ähnlich wie bei einer Strategie aus Diagrammen.

