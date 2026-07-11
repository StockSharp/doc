# RemoteManager

Auf der Registerkarte **RemoteManager** können Sie den Fernsteuerungsmodus aktivieren. Um diesen Modus zu aktivieren, müssen Sie das Menü zur Benutzerkonfiguration öffnen.

![Shell Remote-Manager 00](../../../images/shell_remotemanager_00.png)

Legen Sie im angezeigten Fenster Ihren **Benutzernamen** und Ihr **Passwort** fest.

![Shell Remote-Manager 01](../../../images/shell_remotemanager_01.png)

Danach müssen Sie den **Servermodus** aktivieren.

![Shell Remote-Manager 02](../../../images/shell_remotemanager_02.png)

Nun können Sie sich von einer anderen Shell aus mit Shell verbinden.

Dazu müssen Sie **eine weitere Shell** starten. Öffnen Sie darin die Verbindungseinstellungen.

![Shell Remote-Manager 03](../../../images/shell_remotemanager_03.png)

Richten Sie im geöffneten Fenster die FIX-Verbindung ein.

![Shell Remote-Manager 04](../../../images/shell_remotemanager_04.png)

Drücken Sie anschließend die Schaltfläche Verbinden.

![Shell Remote-Manager 05](../../../images/shell_remotemanager_05.png)

Nach der Verbindung sind alle vorhandenen Strategien auf dem Shell-Server im Shell-Client verfügbar.

![Shell Remote-Manager 06](../../../images/shell_remotemanager_06.png)

Durch Klicken auf die Schaltfläche Hinzufügen können Sie eine weitere Strategie für den Handel hinzufügen.

![Shell Remote-Manager 07](../../../images/shell_remotemanager_07.png)

Da der Shell-Client mehrere Server unterstützt, müssen Sie beim Hinzufügen einer Strategie links den Server auswählen. Rechts werden alle auf dem Server verfügbaren Strategien angezeigt.

![Shell Remote-Manager 08](../../../images/shell_remotemanager_08.png)

Nach dem Hinzufügen einer Strategie erscheint sie in der Strategieliste.

![Shell Remote-Manager 09](../../../images/shell_remotemanager_09.png)

Wenn Sie eine Strategie auswählen, werden rechts Registerkarten mit den Strategieeinstellungen sowie ihren Statistiken angezeigt.

Klicken Sie nach dem Ändern der Strategieeinstellungen unbedingt auf die Schaltfläche Änderungen übernehmen, da die Änderungen sonst nicht auf die Strategie angewendet werden.

![Shell Remote-Manager 10](../../../images/shell_remotemanager_10.png)

Wenn die Strategie einen anderen Befehl als Start\/Stop besitzt, müssen Sie ihn zum Anwenden im nächsten Feld festlegen.

![Shell Remote-Manager 11](../../../images/shell_remotemanager_11.png)

Klicken Sie anschließend auf die Schaltfläche zum Senden des Befehls.

Um Ihren Befehl in der Strategie festzulegen, müssen Sie die Methode [Strategy.ApplyCommand](xref:StockSharp.Algo.Strategies.Strategy.ApplyCommand(StockSharp.Messages.CommandMessage))**(**[StockSharp.Messages.CommandMessage](xref:StockSharp.Messages.CommandMessage) cmdMsg **)** überschreiben.

```cs
public virtual void ApplyCommand(CommandMessage cmdMsg)

```

Die Basisklasse [Strategy](xref:StockSharp.Algo.Strategies.Strategy) steuert nur Start und Stopp der Strategie.

## Empfohlene Inhalte

[Verbindungseinstellungen](../connections_settings.md)
