# Arbeitszeit

![Designer Arbeitszeit 00](../../../../../../images/designer_working_time_00.png)

Dieser Block wird verwendet, um die Arbeitszeit für die Strategie zu bestimmen. Zum Beispiel lässt sich damit festlegen, wann für ein bestimmtes Instrument gehandelt wird oder wann die Strategie handeln darf.
#### Eingehende Sockets

- **Beliebige Daten** - der Block akzeptiert jeden Wert, entnimmt ihm aber den Zeitstempel, der anschließend mit den Parametern des Blocks verglichen wird.
#### Ausgehende Sockets

- **Markierung** - ein Flag, das bestimmt, ob der Zeitstempel den Parametern des Blocks entspricht (true) oder nicht (false).
#### Parameter

- **Zeit von** - die Startzeit der Arbeitszeit.
- **Zeit bis** - die Endzeit der Arbeitszeit.

Der Block kann verwendet werden, um zu bestimmen, wann der Handel für mehrere Instrumente von verschiedenen Handelsplattformen durchgeführt wird.

![Designer Arbeitszeit 01](../../../../../../images/designer_working_time_01.png)

## Siehe auch

[Handel erlaubt](trade_allow.md)

