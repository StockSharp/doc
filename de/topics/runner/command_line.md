
# Befehlszeile

**Runner** ist eine Konsolenanwendung und kann daher durch Angabe von Parametern in der Befehlszeile in verschiedenen Modi gestartet werden. Wird das Programm ohne Parameter gestartet, erscheint eine Hilfemeldung mit den verfügbaren Parametern:

![Befehlszeile 1](../../images/runner_command_line_1.png)

Start von **Runner** für Tests auf historischen Daten:

```cmd
b -s SmaStrategy.cs -h "C:\Storage" --hf 20200401 --ht 20200430 --sec AAPL@NASDAQ -r json
```

Verfügbare Parameter:

- -s - Pfad zur Strategiedatei (mit der Erweiterung cs, json oder dll).
- -t - (optional) wenn eine dll-Datei ausgewählt ist und die Assembly mehr als eine Strategieklasse enthält, muss über diesen Parameter der erforderliche Typ angegeben werden.
- -h - Pfad zum Verzeichnis mit historischen Daten. Bei Verwendung des Servermodus [server](../hydra_server.md) kann dies eine Netzwerkadresse sein.
- --hl - (optional) Login, der im Servermodus [server](../hydra_server.md) verwendet wird.
- --hp - (optional) Passwort, das im Servermodus [server](../hydra_server.md) verwendet wird.
- --hf - Startdatum für den Test im Format YYYYMMDD.
- --ht - Enddatum für den Test im Format YYYYMMDD.
- -f - (optional) Speicherformat (Binary oder Csv).
- --sec - (optional) [Instrumentenkennung](../api/instruments/instrument_identifier.md).
- -r - (optional) Format des Testergebnisberichts (json, xml, csv).
- --tm - (optional) Strategie-Timeout.
- --memory - (optional) maximale Speichergröße (in Megabyte).
- --cpu - (optional) Prozessormaske.
- -l - (optional) Protokollierungsstufe (Info, Debug, Error, Warning, Verbose).

Start von **Runner** für die Optimierung:

```cmd
o -s SmaStrategy.cs -h "C:\Storage" --hf 20200401 --ht 20200430 --sec AAPL@NASDAQ -r json -p sma_optimization.json
```
Alle Parameter aus dem Modus für historische Tests sowie zusätzliche Parameter:

- -p - Pfad zur Parameterdatei.
- --ol - (optional) maximale Anzahl von Iterationen.
- --ob - (optional) Anzahl gleichzeitig getesteter Strategien.

Format der Parameterdatei:

```json
[
	{
	"Name": "SMA_80",
	"Value": "200,201"
	},
	{
	"Name": "SMA_30",
	"From": "40",
	"To": "50",
	"Step": "1"
	},
	{
	"Name": "Security",
	"Value": "AAPL@NASDAQ,MSFT@NASDAQ"
	}
]
```

Start von **Runner** für den Live-Handel:

```cmd
l -s SmaStrategy.cs -c connector.json --tg telegram.json
```

- -c - Datei mit Verbindungseinstellungen.
- --tg - Datei mit Einstellungen für die Telegram-Integration.
