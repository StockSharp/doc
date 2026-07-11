# Export nach MetaStock

Um Daten in Dateien im MetaStock-Format zu exportieren, wählen Sie in der Dropdown-Liste das Format Txt aus:

![Hydra Datenexport](../../../../images/hydra_export.png)

Beim Export in Dateien im Textformat (Txt) erscheint ein Fenster:

![Hydra MetaStock-Export 2](../../../../images/hydra_export_tslab_metastock_2.png)

Geben Sie in diesem Fenster die Exportvorlage an. Die geschweiften Klammern kennzeichnen die zu exportierenden Eigenschaften und deren Reihenfolge:

```none
{SecurityId.SecurityCode},5,{OpenTime:yyyyMMdd},{OpenTime:HHmmss},{OpenPrice},{HighPrice},{LowPrice},{ClosePrice},{TotalVolume}

```

Im Beispiel ist der Zeitrahmen der Fünf-Minuten-Kerze an zweiter Position angegeben.

Außerdem sollte in der Datei die erste Zeile (Header) gesetzt werden:

```none
<TICKER>,<PER>,<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>

```
