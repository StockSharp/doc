# Schemes Panel

Um das Panel **Schemes** zu offnen, klicken Sie auf der Registerkarte **Common** auf die Schaltflache **Schemes**. Das Panel **Schemes** enthalt einen Baum von Skripten, die nach Zweck in Ordner gruppiert sind. Strategieschemata und benutzerdefinierte Blocke unterscheiden sich nicht. Sie werden mit einem gemeinsamen Editor, dem [Strategie-Designer](../strategies/using_visual_designer/diagram_panel.md), bearbeitet. Um Verwechslungen zu vermeiden, sind sie jedoch in zwei unabhangige Listen aufgeteilt und werden in unterschiedlichen Ordnern gespeichert (Strategien im Ordner **Backtest**, benutzerdefinierte Blocke im Ordner **Custom Blocks**). Die Auswahl eines Schemas zur Bearbeitung erfolgt durch Doppelklick auf den erforderlichen Eintrag in der Liste. Das ausgewahlte Schema wird anschliessend im Designer zur Anzeige und Bearbeitung geoffnet. Nachfolgend finden Sie eine Beschreibung der Ordner im Panel **Schemes**:

![Designer Panel Circuits 00](../../../images/designer_panel_circuits_00.png)

1. Der Ordner **Backtest** enthalt Handelsstrategien, die sowohl als Schemata aus Elementen und deren Verbindungen als auch aus Code erstellt wurden. Sie konnen eine neue Strategie hinzufugen, indem Sie auf der Registerkarte **Common** auf die Schaltflache **Add** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) klicken und **Strategy** auswahlen. Alternativ klicken Sie im Panel **Schemes** mit der rechten Maustaste auf den Ordner **Backtest** und wahlen im Dropdown-Menu die Schaltflache **Add** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png). Wahlen Sie im geoffneten Fenster aus, wie genau Sie eine Strategie erstellen mochten.
   
    ![Designer Panel Circuits 04](../../../images/designer_panel_circuits_04.png)
   
    Strategien konnen mit einem visuellen Designer ohne Programmierung oder mit dem integrierten Quellcode-Editor erstellt werden. Zusatzlich konnen externe DLL-Dateien mit in Microsoft Visual Studio geschriebenen Strategien eingebunden werden. Ausfuhrliche Informationen zu **Strategies** finden Sie im Abschnitt [Using Blocks](../strategies/using_visual_designer.md).

2. Der Ordner **Own elements** enthalt Elemente, die eine abgeschlossene Funktionalitat darstellen und in verschiedenen Schemata oder mehrfach in einem Schema mit unterschiedlichen Eigenschaftswerten verwendet werden konnen. Solche Elementgruppen konnen in einen separaten Block ausgelagert werden, der anschliessend wie jedes Standardelement verwendet wird. Ein **Custom block** ist ein normales Schema, das wie jedes Strategieschema gespeichert, geladen und bearbeitet wird. Fugen Sie ein neues zusammengesetztes Element hinzu, indem Sie auf der Registerkarte **Common** auf die Schaltflache **Add** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png) klicken und **Custom Blocks** auswahlen. Alternativ klicken Sie im Panel **Schemes** mit der rechten Maustaste auf den Ordner **Custom Blocks** und wahlen im Dropdown-Menu die Schaltflache **Add** ![Designer Panel Circuits 01](../../../images/designer_panel_circuits_01_button.png). Neue benutzerdefinierte Blocke werden automatisch zur **Element Palette** in der Gruppe **Custom Blocks** hinzugefugt und konnen beim Erstellen anderer Strategieschemata und benutzerdefinierter Blocke verwendet werden. Ausfuhrliche Informationen zu **Custom Blocks** finden Sie im Abschnitt [Creating composite elements](../strategies/using_visual_designer/composite_elements.md).

3. Der Ordner **Live** enthalt Strategien, die fur den Handel hinzugefugt wurden. Gestartete Strategien sind mit dem Symbol ![Designer Panel Circuits 02](../../../images/designer_panel_circuits_02.png) markiert, gestoppte Strategien mit dem Symbol ![Designer Panel Circuits 03](../../../images/designer_panel_circuits_03.png). Wie Strategien zum Ordner **Live** hinzugefugt und gestartet werden, ist im Abschnitt [Live trading](../live_execution/getting_started.md) beschrieben.

4. Der Ordner **Indicators** enthalt Ihre eigenen Indikatoren fur Handelsstrategien, die Sie selbst geschrieben haben. Neue Indikatoren konnen nicht mit Schemata erstellt werden; verfugbar sind nur Code und externe DLL-Dateien. Die Verwendung eigener Indikatoren in Schemata ist uber den Block [Indicator](../strategies/using_visual_designer/elements/common/indicator.md) bei Auswahl des Indikatortyps moglich.

5. Der Ordner **Remote** enthalt Strategien, die sich auf einem Remote-Server befinden.

## Siehe auch

[Logs Panel](logs.md)
