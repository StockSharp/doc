# Hydra-Einstellungen

Im Folgenden wird beschrieben, wie Sie eine Backup-Aufgabe erstellen und konfigurieren.

1. Um eine Aufgabe zu erstellen, klicken Sie auf die Schaltfläche **Aufgaben hinzufügen...**, wählen im geöffneten Fenster den Eintrag **Sicherung** aus und klicken auf die Schaltfläche **OK**.![hydra tasks backup add](../../../../images/hydra_tasks_backup_add.png)
2. Danach müssen Sie die Aufgabe konfigurieren.![hydra tasks backup](../../../../images/hydra_tasks_backup.png)

   **Sicherung**
   - **Dienst** - die Dienstadresse.
   - **Adresse** - die Regionsadresse. Die Adresse der Region, die in den Bucket-Einstellungen angegeben ist. Siehe [Regionen und Endpunkte](https://docs.aws.amazon.com/general/latest/gr/rande.html#s3_region).
   - **Speicher** - der Bucket-Name.
   - **Anmeldung** - Anmeldung. **Access Key ID**.
   - **Passwort** - Passwort. **Secret Access Key**.
   - **Startdatum** - ab welchem Datum das Backup gestartet werden soll.
   - **Zeitversatz** - ein Offset in Tagen ab dem aktuellen Datum.

   **Allgemein**
   - **Kopfzeile** - Aufgabentitel.
   - **Arbeitszeiten** - Einrichtung des Arbeitszeitplans des Boards. ![hydra tasks backup desk](../../../../images/hydra_tasks_backup_desk.png)
   - **Betriebsintervall** - das Ausführungsintervall.
   - **Datenverzeichnis** - Datenverzeichnis, aus dem die Daten für die Konvertierung gelesen werden.
   - **Format** - Format der konvertierten Daten: BIN\/CSV.
   - **Max. Fehler** - maximale Anzahl von Fehlern, bei deren Erreichen die Aufgabe gestoppt wird. Standardmäßig 0, die Anzahl der Fehler wird ignoriert.
   - **Abhängigkeit** - eine Aufgabe, die vor dem Start der aktuellen Aufgabe ausgeführt werden muss.

   **Protokollierung**
   - **Kennung** - die Kennung.
   - **Protokollierungsstufe** - die Protokollierungsstufe.
3. Nachdem Sie die Aufgabe eingerichtet haben, fügen Sie die Instrumente hinzu, die im Backup-Speicher gespeichert werden sollen, und klicken Sie auf die Schaltfläche **Starten**.

## Empfohlene Inhalte

[Konto erstellen und konfigurieren](setup.md)
