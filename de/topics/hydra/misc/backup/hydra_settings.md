# Hydra-Einstellungen

Im Folgenden wird beschrieben, wie Sie eine Backup-Aufgabe erstellen und konfigurieren.

1. Um eine Aufgabe zu erstellen, klicken Sie auf die Schaltfläche **Add tasks...**, wählen im geoeffneten Fenster den Eintrag **Backup** aus und klicken auf die Schaltfläche **OK**.![hydra tasks backup add](../../../../images/hydra_tasks_backup_add.png)
2. Danach müssen Sie die Aufgabe konfigurieren.![hydra tasks backup](../../../../images/hydra_tasks_backup.png)

   **Backup**
   - **Service** - die Service-Adresse.
  - **Address** - die Regionsadresse. Die Adresse der Region, die in den Bucket-Einstellungen angegeben ist. Siehe [Regions and Endpoints](https://docs.aws.amazon.com/general/latest/gr/rande.html#s3_region).
   - **Storage** - der Bucket-Name.
   - **Login** - Login. Access Key ID.
   - **Password** - Passwort. **Secret Access Key**.
   - **Start date** - ab welchem Datum das Backup gestartet werden soll.
   - **Time offset** - ein Offset in Tagen ab dem aktuellen Datum.

   **General**
   - **Header** - Converter.
   - **Working hours** - Einrichtung des Arbeitszeitplans des Boards. ![hydra tasks backup desk](../../../../images/hydra_tasks_backup_desk.png)
   - **Interval of operation** - das Ausfuehrungsintervall.
  - **Data directory** - Datenverzeichnis, aus dem die Daten für die Konvertierung gelesen werden.
   - **Format** - Format der konvertierten Daten: BIN\/CSV.
   - **Max. errors** - maximale Anzahl von Fehlern, bei deren Erreichen die Aufgabe gestoppt wird. Standardmaessig 0, die Anzahl der Fehler wird ignoriert.
   - **Dependency** - eine Aufgabe, die vor dem Start der aktuellen Aufgabe ausgeführt werden muss.

   **Logging**
   - **Identifier** - die Kennung.
   - **Logging level** - der Logging-Level.
3. Nachdem Sie die Aufgabe eingerichtet haben, fügen Sie die Instrumente hinzu, die im Backup-Speicher gespeichert werden sollen, und klicken Sie auf die Schaltfläche **Starten**.

## Empfohlene Inhalte

[Konto erstellen und konfigurieren](setup.md)
