# Kerzenerzeugung

[Hydra](../../hydra.md) ermoeglicht die Erzeugung verschiedener Kerzentypen auf Basis heruntergeladener Trades. Diese koennen anschliessend in die Formate [Excel](https://en.wikipedia.org/wiki/Excel), XML, SQL, BIN, JSON oder TXT exportiert werden.

Dadurch koennen Sie die erzeugten Daten in beliebigen Programmen fuer technische Analyse verwenden (WealthLab, AmiBroker usw.).

## Prozess der Kerzenerzeugung

1. Klicken Sie auf der Registerkarte **General** auf die Schaltflaeche **Candles**. Das folgende Fenster wird geoeffnet:

   ![hydra candles main](../../../images/hydra_candles_main.png)

2. Im geoeffneten Fenster muessen Sie die Parameter fuer die Kerzenerzeugung konfigurieren:

   - Waehlen Sie den gewuenschten Kerzentyp aus der Dropdown-Liste aus (alle [standard candle types](../../api/candles.md) werden unterstuetzt).
   - Geben Sie die erforderlichen Parameter fuer den ausgewaehlten Kerzentyp an:
     - Fuer [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) - **Timeframe** auswaehlen.
     - Fuer [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage) - **Volume** angeben.
     - Fuer [TickCandleMessage](xref:StockSharp.Messages.TickCandleMessage) - **Number of ticks** angeben.
     - Fuer [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage) - **Range** angeben.
     - Fuer [RenkoCandleMessage](xref:StockSharp.Messages.RenkoCandleMessage) - **Block size** angeben.
     - Fuer [PnFCandleMessage](xref:StockSharp.Messages.PnFCandleMessage) - **P&F Parameters** angeben.
   - Waehlen Sie das Instrument aus, fuer das Kerzen erzeugt werden sollen.
   - Geben Sie bei Bedarf einen Zeitbereich an.
   - Klicken Sie auf die Schaltflaeche ![hydra find](../../../images/hydra_find.png), um die Erzeugung zu starten.

### Beispiel fuer die Timeframe-Kerzenerzeugung

Um 5-Minuten-Kerzen fuer das Instrument AAPL@NASDAQ zu erzeugen:

1. Waehlen Sie den Kerzentyp [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage).
2. Setzen Sie **Timeframe** = 5 min.
3. Waehlen Sie das Instrument AAPL@NASDAQ aus.
4. Klicken Sie auf die Suchschaltflaeche.

Nach der Datenerzeugung sehen Sie das Ergebnis:

![hydra candles tf](../../../images/hydra_candles_tf.png)

### Beispiel fuer die Volume-Kerzenerzeugung

Um Volume-Kerzen zu erzeugen:

1. Waehlen Sie den Kerzentyp [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage).
2. Geben Sie das Volumen an, zum Beispiel 100.
3. Waehlen Sie das Instrument aus.
4. Waehlen Sie im Feld **Build from** den Wert **Ticks**.
5. Klicken Sie auf die Suchschaltflaeche.

Ergebnis der Erzeugung:

![hydra candles volume](../../../images/hydra_candles_volume.png)

## Datenquellen fuer die Kerzenerstellung

Wenn Marktdaten nicht direkt aus der Quelle abgerufen werden konnten, koennen Sie Kerzen erzeugen, indem Sie im Feld [**Build from**](any_market_data_types.md) den Datentyp auswaehlen, aus dem sie erstellt werden:

- **Ticks** - Kerzen aus Tick-Daten erstellen.
- **Order Books** - Kerzen aus Order-Book-Daten erstellen.
- **Level1** - Kerzen aus Level1-Daten erstellen.
- **Smaller Timeframe** - Kerzen mit groesserem Timeframe aus Kerzen mit kleinerem Timeframe erstellen.

### Beispiele verschiedener Erstellungsoptionen:

- 10-Minuten-Kerzen aus Ticks:

  ![hydra candles tf 10](../../../images/hydra_candles_tf_10.png)

- 30-Minuten-Kerzen aus 5-Minuten-Kerzen:

  ![hydra candles tf 01](../../../images/hydra_candles_tf_01.png)

> [!TIP]
> Wenn Sie im Feld **Build from** den Wert **don't build** auswaehlen, werden nur fertige Kerzen gesucht, die direkt ueber die Datenquelle heruntergeladen wurden.

## Visualisierung erzeugter Kerzen

Zur grafischen Anzeige erzeugter Kerzen:

1. Klicken Sie auf die Schaltflaeche ![hydra candles](../../../images/hydra_candles.png).
2. Ein Chart mit den erstellten Kerzen wird geoeffnet:

   ![hydra candles tf chart](../../../images/hydra_candles_tf_chart.png)

   ![hydra candles volume chart](../../../images/hydra_candles_volume_chart.png)

## Indikatoren zum Chart hinzufuegen

Technische Indikatoren koennen zum Kerzenchart hinzugefuegt werden:

1. Oeffnen Sie das Kontextmenue durch Rechtsklick auf das Chartpanel.
2. Waehlen Sie den Eintrag **Indicator** und den gewuenschten Indikator aus der Liste aus.
3. Um den Indikator in einem separaten Panel anzuzeigen:
   - Fuegen Sie ueber die Schaltflaeche ![hydra add](../../../images/hydra_add.png) ein neues Panel hinzu.
   - Waehlen Sie den gewuenschten Indikator im Kontextmenue aus.

Beispiel fuer ein Chart mit hinzugefuegten Indikatoren:

![hydra candles ind chart](../../../images/hydra_candles_ind_chart.png)

## Datenexport

Die erhaltenen Kerzenwerte koennen [in verschiedene Formate exportiert](export_data.md) werden, um sie in anderen Programmen zu verwenden.

**Siehe auch das [Video-Tutorial](../videos/building_candles.md) zum Erstellen verschiedener Kerzentypen**
