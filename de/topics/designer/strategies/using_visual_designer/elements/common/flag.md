# Flag

![Designer Flagge](../../../../../../images/designer_flag_00.png)

Die Komponente "Flag" wird verwendet, um ein binäres Flag zu verwalten, das auf Grundlage eingehender Signale gesetzt oder zurückgesetzt werden kann.

## Eingabeanschlüsse

- **Auslöser**: Akzeptiert jeden Wert außer `False`. Setzt das Flag beim Empfang des ersten passenden Werts. Wenn das Flag bereits gesetzt ist, werden nachfolgende Signale ignoriert, bis es zurückgesetzt wird.
- **Zurücksetzen**: Akzeptiert jeden Wert außer `False`. Setzt das Flag zurück, sodass es auf nachfolgende Auslöser reagieren kann.

## Ausgabeanschlüsse

- **Signal**: Gibt ein Signal aus, wenn das Flag gesetzt wird.

