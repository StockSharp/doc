# Debugging einer DLL mit Visual Studio

Visual Studio bietet einen Mechanismus, um sich mit dem Visual-Studio-Debugger an laufende Prozesse anzuhängen. Der Visual-Studio-Debugger wird in der Dokumentation [Attach to running processes](https://learn.microsoft.com/en-us/visualstudio/debugger/attach-to-running-processes-with-the-visual-studio-debugger?view=vs-2022) ausführlicher beschrieben. Der Debugging-Prozess wird am Beispiel einer Strategie gezeigt, die im Abschnitt [DLL verwenden](../using_dll.md) hinzugefügt wurde.

1. Um sich an einen Prozess anzuhängen und das Debugging einer DLL-Strategie zu starten, muss diese in den Speicher geladen werden. Die DLL wird nach dem [Hinzufügen der Strategie](../using_dll.md) in den Speicher geladen. Sobald die DLL im Speicher geladen ist, können Sie sich an den Prozess anhängen.

![Designer_Creation_Strategy_Dll_01](../../../../images/designer_creation_strategy_dll_01.png)

2. Wählen Sie in Visual Studio **Debug -> Attach to Process** aus.

![Designer Debugging eines DLL-Würfels mit Visual Studio 00](../../../../images/designer_debugging_dll_using_visual_studio_00.png)

3. Suchen Sie im Dialogfeld **An Prozess anhängen** in der Liste **Verfügbare Prozesse** den Prozess **Designer.exe**, an den Sie sich anhängen möchten.

![Designer Debugging eines DLL-Würfels mit Visual Studio 01](../../../../images/designer_debugging_dll_using_visual_studio_01.png)

Wenn der Prozess unter einem anderen Benutzerkonto ausgeführt wird, müssen Sie das Kontrollkästchen **Prozesse aller Benutzer anzeigen** aktivieren.

4. Wichtig ist, dass im Fenster **Anhängen an** der Codetyp angegeben ist, der debuggt werden soll. Der Standardparameter **Automatisch** versucht, den zu debuggenden Codetyp zu bestimmen, erkennt ihn aber nicht immer korrekt. Um den Codetyp manuell festzulegen, führen Sie die folgenden Schritte aus.

- Klicken Sie im Feld Attach to auf **Auswählen**.
- Klicken Sie im Dialogfeld **Codetyp auswählen** auf die Schaltfläche **Diese Codetypen debuggen** und wählen Sie die Typen für das Debugging aus.
- Klicken Sie auf OK.

![Designer Debugging eines DLL-Würfels mit Visual Studio 02](../../../../images/designer_debugging_dll_using_visual_studio_02.png)

5. Klicken Sie auf die Schaltfläche Attach.

6. Setzen Sie in Visual Studio Haltepunkte im Code. Wenn die Haltepunkte rot und rot ausgefüllt sind ![Designer Debugging eines DLL-Würfels mit Visual Studio 03](../../../../images/designer_debugging_dll_using_visual_studio_03.png) (und Studio sich im Debugging-Modus befindet), bedeutet dies, dass die exakte Version der DLL geladen wurde. Wenn die Haltepunkte rot und weiß ausgefüllt sind ![Designer Debugging eines DLL-Würfels mit Visual Studio 04](../../../../images/designer_debugging_dll_using_visual_studio_04.png) (und Studio sich im Debugging-Modus befindet), bedeutet dies, dass die falsche Version der DLL geladen wurde.

7. Im Beispiel wird der Haltepunkt in der ersten Zeile der Methode **public void ProcessCandle(Candle candle)** gesetzt. Wenn die Strategie in [Designer](../../../designer.md) läuft, hält Visual Studio am Haltepunkt an, sobald Kerzenwerte an die DLL übergeben werden. Von dort aus können Sie die Ausführung des Codes verfolgen:

![Designer Debugging eines DLL-Würfels mit Visual Studio 05](../../../../images/designer_debugging_dll_using_visual_studio_05.png)

> [!WARNING]
> Wenn der Code im Debugger angehalten ist, werden alle Prozesse im Programm **Designer** ausgesetzt. Wenn das Programm mit realem Handel verbunden ist, kommt es bei einem längeren Halt im Debugger zu Verbindungsabbrüchen.

## Siehe auch

[Strategien exportieren](../../export_import/export.md)

