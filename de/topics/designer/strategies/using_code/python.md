# Python verwenden

Das Erstellen von Strategien aus Code richtet sich an Benutzer, die bevorzugt mit Python-Code arbeiten. Solche Strategien sind im Gegensatz zu Schemas nicht in ihren Möglichkeiten eingeschränkt, und jeder Algorithmus kann implementiert werden.

Der Prozess zum Erstellen einer Strategie findet direkt in [Designer](../../../designer.md) oder in einer **Python**-Entwicklungsumgebung statt (die beliebtesten Entwicklungsumgebungen sind **Visual Studio** und **JetBrains Rider**). Dabei wird eine Bibliothek für die professionelle Entwicklung von Handelsrobotern in **Python** und [API](../../../api.md) verwendet.

Sie können eine neue Strategie hinzufügen, indem Sie im Tab **General** auf die Schaltfläche **Add** ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png) klicken und **Strategy** auswählen. Alternativ klicken Sie mit der rechten Maustaste auf den Ordner **Strategies** im Panel **Schemes** und anschließend im Dropdown-Menü auf die Schaltfläche **Add** ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png):

![Designer The creation of a strategy 00](../../../../images/designer_creation_of_strategy_00.png)

Nach dem Klicken auf die Schaltfläche **Add** ![Designer Panel Circuits 01](../../../../images/designer_panel_circuits_01_button.png) erscheint ein Fenster, in dem der Inhaltstyp ausgewählt wird, auf dessen Grundlage die Strategie erstellt werden soll:

![Designer_Creation_of_element_containing_source_code_00](../../../../images/designer_python_create_strategy_00.png)

Um eine Strategie aus Python-Code zu erstellen, wählen Sie den zweiten Tab. Sie können außerdem eine Vorlage auswählen, die als Anfangscode verwendet wird.

Nach dem Klicken auf **OK** erscheint eine neue Strategie im Ordner **Strategies** des Panels **Schemes**, ähnlich wie beim Erstellen einer Strategie aus einem [Schema](../using_visual_designer.md). Auch die Aktionen zum Löschen oder Umbenennen der Strategie sind ähnlich.

Anstelle eines Schemas wird jedoch ein Python-Code-Editor angezeigt:

![Designer_Creation_of_element_containing_source_code_01](../../../../images/designer_python_create_strategy_01.png)

Der Code-Editor-Tab besteht aus den Panels **Source Code** und **Error List**. Das Panel **Source Code** enthält den eigentlichen Python-Code-Editor. Oben befindet sich eine Symbolleiste, in der Hervorhebungen wie **Current Line**, **Line Number** usw. ein- oder ausgeschaltet werden können. Um die Schriftgröße zu erhöhen, können Sie die Kombination CTRL+MouseWheel verwenden.

Das Panel **Error List** ist eine Tabelle mit einer Liste der Codefehler. Ein Doppelklick auf eine Zeile bewegt den Cursor im Panel **Source Code** automatisch an die Fehlerstelle.

Beim Bearbeiten des Codes erscheint in der rechten unteren Ecke des Panels **Error List** ein Symbol ![Designer The creation of the cube containing the source code 03](../../../../images/designer_creation_of_element_containing_source_code_03.png), das anzeigt, dass die Änderungsverfolgung begonnen hat. Die Codekompilierung erfolgt, sobald sich der Code nicht mehr ändert.

Das Ausführen der Strategie im [Backtest](../../backtesting/user_interface.md), im [Live-Betrieb](../../live_execution/getting_started.md) und andere Operationen funktionieren ähnlich wie bei Strategien, die aus Schemas erstellt wurden.

## Einschränkungen

> [!WARNING]
> [Designer](../../../designer.md) verwendet IronPython, das die folgenden Einschränkungen hat:
> - Kompatibilität mit Python-Version 3.4
> - Teilweise numpy-Unterstützung durch eine spezielle .NET-Implementierung (ein Verwendungsbeispiel finden Sie [hier](https://github.com/StockSharp/StockSharp/blob/master/Algo.Analytics.Python/pearson_correlation_script.py))
> - Keine Unterstützung für andere beliebte in C geschriebene Bibliotheken (pandas, scipy usw.)
> - Eingeschränkte Unterstützung für asynchrone Programmierung
> - Einige integrierte Python-Module können aufgrund ihrer Abhängigkeit von CPython-spezifischen Implementierungen nicht verwendet werden
> - Die Leistung kann bei einigen Operationen niedriger sein als bei CPython
>
> Es wird empfohlen, diese Einschränkungen bei der Entwicklung von Handelsstrategien in Python innerhalb von [Designer](../../../designer.md) zu berücksichtigen.

