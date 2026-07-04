# Lines

Die Strategie im Designer ist ein Schema aus einer Gruppe von Elementen und Verknüpfungen zwischen ihnen, den sogenannten Verbindungen. Jede Verbindung führt vom Ausgabeparameter eines Würfels zum Eingabeparameter eines anderen Würfels. Normalerweise sind alle Verbindungslinien grau, aber wenn Sie auf den Würfel zeigen, zu dem sie gehören, werden die Linien schwarz dargestellt.

![Designer Line 00](../../../../images/designer_line_00.png)

Jede Verbindung kann hervorgehoben werden, indem Sie darauf zeigen und mit der linken Maustaste klicken. Die ausgewählte Verbindung wird an den Linienenden mit Kreisen markiert; wenn Sie diese greifen, können Sie die Linie umleiten. Wenn Sie bei ausgewählter Linie die Taste Del drücken, wird sie gelöscht.

Sie können Parameter gleicher Farbe (also gleicher Datentypen) miteinander verbinden, mit Ausnahme der folgenden Parametertypen:

- Der Parameter **black** kann beliebige Daten akzeptieren. Meist werden diese Parameter verwendet, um Signale für Aktionen innerhalb des Elements zu übergeben. Zum Beispiel speichert das Element [Variable](elements/data_sources/variable.md) einen Wert und gibt ihn an den Ausgang weiter, wenn es ein Signal empfängt.
- Der Parameter **green** kann verschiedene vergleichbare Datentypen akzeptieren. Zum Beispiel numerische Werte, Indikatorwerte, Zeichenfolgen usw.

## Empfohlene Inhalte

[Event model](event_model.md)

