# RemoteManager

Auf der Registerkarte **RemoteManager** können Sie den Fernsteuerungsmodus aktivieren. Um diesen Modus zu aktivieren, müssen Sie das Menü zur Benutzerkonfiguration öffnen.

![RemoteManager Screenshot 1](../../../images/shell_remotemanager_00.png)

Legen Sie im angezeigten Fenster Ihren **Benutzernamen** und Ihr **Passwort** fest.

![RemoteManager Screenshot 2](../../../images/shell_remotemanager_01.png)

Danach müssen Sie den **Servermodus** aktivieren.

![RemoteManager Screenshot 3](../../../images/shell_remotemanager_02.png)

Nun können Sie sich von einer anderen Shell aus mit Shell verbinden.

Dazu müssen Sie **eine weitere Shell** starten. Öffnen Sie darin die Verbindungseinstellungen.

![RemoteManager Screenshot 4](../../../images/shell_remotemanager_03.png)

Richten Sie im geöffneten Fenster die FIX-Verbindung ein.

![RemoteManager Screenshot 5](../../../images/shell_remotemanager_04.png)

Drücken Sie anschließend die Schaltfläche Verbinden.

![RemoteManager Screenshot 6](../../../images/shell_remotemanager_05.png)

Nach der Verbindung sind alle vorhandenen Strategien auf dem Shell-Server im Shell-Client verfügbar.

![RemoteManager Screenshot 7](../../../images/shell_remotemanager_06.png)

Durch Klicken auf die Schaltfläche Hinzufügen können Sie eine weitere Strategie für den Handel hinzufügen.

![RemoteManager Screenshot 8](../../../images/shell_remotemanager_07.png)

Da der Shell-Client mehrere Server unterstützt, müssen Sie beim Hinzufügen einer Strategie links den Server auswählen. Rechts werden alle auf dem Server verfügbaren Strategien angezeigt.

![RemoteManager Screenshot 9](../../../images/shell_remotemanager_08.png)

Nach dem Hinzufügen einer Strategie erscheint sie in der Strategieliste.

![RemoteManager Screenshot 10](../../../images/shell_remotemanager_09.png)

Wenn Sie eine Strategie auswählen, werden rechts Registerkarten mit den Strategieeinstellungen sowie ihren Statistiken angezeigt.

Klicken Sie nach dem Ändern der Strategieeinstellungen unbedingt auf die Schaltfläche Änderungen übernehmen, da die Änderungen sonst nicht auf die Strategie angewendet werden.

![RemoteManager Screenshot 11](../../../images/shell_remotemanager_10.png)

Wenn die Strategie einen anderen Befehl als Start\/Stop besitzt, müssen Sie ihn zum Anwenden im nächsten Feld festlegen.

![RemoteManager Screenshot 12](../../../images/shell_remotemanager_11.png)

Klicken Sie anschließend auf die Schaltfläche zum Senden des Befehls.

Um Ihren Befehl in der Strategie festzulegen, müssen Sie die Methode [Strategy.ApplyCommand](xref:StockSharp.Algo.Strategies.Strategy.ApplyCommand(StockSharp.Messages.CommandMessage))**(**[StockSharp.Messages.CommandMessage](xref:StockSharp.Messages.CommandMessage) cmdMsg **)** überschreiben.

```cs
public virtual void ApplyCommand(CommandMessage cmdMsg)

```

Die Basisklasse [Strategy](xref:StockSharp.Algo.Strategies.Strategy) steuert nur Start und Stopp der Strategie.

## Empfohlene Inhalte

[Verbindungseinstellungen](../connections_settings.md)
