# Erster Start

Beim ersten Start erscheint das folgende Fenster zur Auswahl von Datenquellen. Sie konnen dieses Fenster auch auf der Registerkarte **Common** offnen, indem Sie **Add \=\> Sources** auswahlen.

![hydra source add](../../images/hydra_source_add.png)

Markieren Sie im Fenster die erforderlichen Quellen. Sie konnen Filter nach Region, Board, Datentyp, Zahlungsart, Echtzeit oder nicht verwenden. Wenn die Auswahl abgeschlossen ist, klicken Sie auf **OK**. Danach bietet das Programm an, die Hilfsprogramme zu aktivieren. Weitere Details zur Arbeit mit Hilfsprogrammen finden Sie im Abschnitt [Utilities](tasks.md). Klicken Sie auf **OK**.

![hydra first started utilities 00](../../images/hydra_first_started_utilities00.png)

Danach werden die Quellen zum linken Panel des Hauptfensters der Anwendung hinzugefugt. 

![hydra Quick start 01](../../images/hydra_quick_start_01.png)

## Instrument zum Hochladen von Marktdaten herunterladen:

Bevor Sie mit dem Herunterladen von Marktdaten beginnen, mussen Sie die Instrumente einrichten, fur die Marktdaten abgerufen werden sollen.

Nach dem Hinzufugen von Marktdatenquellen werden im zentralen Bereich Panels der hinzugefugten Quellen geoffnet, in denen eine Liste der Instrumente angezeigt wird. Wenn das Panel geschlossen ist, offnen Sie es durch Doppelklick auf das Quellenlogo in der Liste links im Programm.

Laden Sie zum Beispiel das Instrument AAPL@NASDAQ aus einer unterstutzten Datenquelle herunter.

![hydra choose market data](../../images/hydra_choose_market_data.png)

> [!TIP]
> WICHTIG\! Daten werden nur fur die Instrumente heruntergeladen, die der Instrumentenliste hinzugefugt wurden

1. Instrumente hinzufugen.

   Beim ersten Start bietet das Programm an, alle Instrumente fur die ausgewahlte Quelle auf einmal herunterzuladen. Danach ladt der Benutzer die Instrumente selbst herunter. Anfanglich ist die Instrumentendatenbank in [Hydra](../hydra.md) leer; es gibt nur das Hilfsinstrument **ALL@ALL**. Wenn dieses Instrument ausgewahlt ist, werden Daten fur alle fur diese Quelle verfugbaren Instrumente heruntergeladen. 

   Um ein Instrument hinzuzufugen, klicken Sie auf die Schaltflache **Add** ![hydra add](../../images/hydra_add.png). Danach offnet sich ein Fenster zum Herunterladen des Instruments. ![hydra securities](../../images/hydra_securities.png)

   Um die Instrumente herunterzuladen, klicken Sie auf die entsprechende Schaltflache **Download securities**.

   Danach erscheint auf dem Bildschirm ein Menu, in dem der Benutzer **Download all securities** auswahlen kann.![hydra securities choose all](../../images/hydra_securities_choose_all.png)

   Oder Sie konnen fur eine Reihe von Quellen die Instrumente [konfigurieren](prepare_for_download/instruments_list.md), die heruntergeladen werden sollen. 

   Nachdem die Instrumente empfangen wurden, sieht das Fenster wie folgt aus.![hydra security full list](../../images/hydra_security_full_list.png)

   Es listet alle Instrumente auf, die zum Hinzufugen verfugbar sind. Fur eine schnelle Suche konnen Sie den Namen in das entsprechende Feld eingeben. 

   Um ein Instrument auszuwahlen, doppelklicken Sie darauf, und es wird auf die rechte Seite der Liste verschoben.![hydra security full list 00](../../images/hydra_security_full_list_00.png)

   Anschliessend wird es auf die rechte Seite der Tabelle verschoben.![hydra security full list 01](../../images/hydra_security_full_list_01.png)

   Die ausgewahlten Instrumente werden in der Tabelle **Securities** angezeigt, die baumartig strukturiert ist. Das Hauptelement ist das Instrument, die zusatzlichen Elemente sind die Marktdatentypen, die fur dieses Instrument empfangen werden.
2. Fur jedes ausgewahlte Instrument sollten Sie die Marktdatentypen auswahlen, die fur den Download erforderlich sind.

   Wenn nicht alle erforderlichen Instrumentparameter gesetzt sind, erscheint in der linken Spalte der Instrumentzeile das Symbol ![hydra zero](../../images/hydra_zero.png). ![hydra type market data choose](../../images/hydra_type_market_data_choose.png)

   Wahlen wir fur den Download **Ticks** und **Candles Time Frame 5** aus.

   Am unteren Rand des Quellenfensters befindet sich ein Panel mit Schaltflachen zur Einrichtung der zu empfangenden Daten und Instrumente. ![hydra Quick start 02 00](../../images/hydra_quick_start_02_00.png)

   In diesem Panel konnen die folgenden Operationen ausgefuhrt werden: 
   - Konfigurieren der Menge der empfangenen Informationen mit den Schaltflachen: **Trades, Order Books, Candles, Order Log, Level 1, Own Transactions**. Die Listen verfugbarer Marktdatentypen unterscheiden sich je nach Quelle. 
   - Den erforderlichen Time Frame fur die geladenen Kerzen angeben. Der Time Frame der empfangenen Kerzen unterscheidet sich je nach Quelle.![hydra Quick start 02](../../images/hydra_quick_start_02.png)
   - Den erforderlichen Zeitraum fur das Herunterladen von Marktdaten festlegen. Der Zeitraum kann auch direkt im Marktdatenfenster konfiguriert werden. Dazu wahlen Sie den Beginn und das Ende des Zeitraums aus.

     Wenn der Benutzer kein Enddatum fur den Zeitraum angibt, ladt das Programm alle fur das aktuelle Datum verfugbaren Daten herunter. Wenn die Quelle die Ubertragung von Marktdaten in Echtzeit unterstutzt, werden die Marktdaten bei fehlendem Enddatum fur den Zeitraum in Echtzeit heruntergeladen. 

     Legen wir den Zeitraum fest, fur den die Marktdaten heruntergeladen werden sollen.![hydra Quick start 02 01](../../images/hydra_quick_start_02_01.png)
   - Angeben, woraus die Marktdaten erstellt werden sollen. Wenn dieser Parameter nicht angegeben ist, werden die in der Quelle verfugbaren Kerzen empfangen. Wenn der Benutzer den Marktdatentyp angibt, werden Kerzen aus dem angegebenen Marktdatentyp erstellt. Zum Beispiel konnen Kerzen aus dem letzten Handelspreis, dem Order-Book-Spread (ublicherweise fur den Forex-Markt), der Volatilitat oder dem besten Preis erstellt werden. 

     Diese Funktion ist praktisch, wenn die Quelle keine Daten fur die Kerzendarstellung bereitstellt. In diesem Fall werden Kerzen auf Basis gemittelter Datenwerte gezeichnet.![hydra candle build type](../../images/hydra_candle_build_type.png)

     Der Benutzer hat ausserdem die Moglichkeit, einen [Custom type](prepare_for_download/custom_candles.md) von Kerzen auszuwahlen, um die empfangenen Daten anzupassen.
   - Nach Auswahl eines Instruments, eines Marktdatentyps und Festlegung des Zeitraums klicken Sie auf die Schaltflache **Start**. Danach beginnt der Download der Marktdaten.

   Der Arbeitsprozess kann auf der speziellen Registerkarte **Logs** beobachtet werden, die am unteren Rand des Programms fixiert ist. Zusatzlich werden Logs in Dateien im lokalen Ordner gespeichert.

![hydra main start](../../images/hydra_main_start.png)

Ausserdem kann der Benutzer [zusatzliche Quellen](data_sources/select_source.md) hinzufugen.

Nachdem die Marktdaten heruntergeladen wurden, kann der Benutzer [Marktdaten anzeigen](working_with_data/view_and_export.md), [Kerzen zeichnen](working_with_data/candles_generation.md), speichern oder [in verschiedene Formate exportieren](working_with_data/export_data.md).

**Sehen Sie sich das [Video-Tutorial](videos/first_start.md) an**.

