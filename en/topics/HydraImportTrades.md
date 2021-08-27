# Ticks

To import trades, select the **Import \=\> Ticks** tab.

![hydra import trades](../images/hydra_import_trades.png)

### The import process securities.

The import process securities.

1. **Import settings.**.

   See [Candles](HydraImportCandles.md) import.
2. Configure import parameters for [S\#](StockSharpAbout.md) fields.

   See [Candles](HydraImportCandles.md) import.

   **Let's consider an example of importing trades (ticks) from a CSV file:**
   - The file from which you want to import data has the following template:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{TradeId};{TradePrice};{TradeVolume};{OriginSide}
     	  				
     ```

     Here the values of {SecurityId.SecurityCode} and {SecurityId.BoardCode} correspond to the values of **Security** and **Board**, respectively. Therefore, in the **Field order** field we assign the values 0 and 1, respectively.
   - For the {ServerTime:default:yyyyMMdd} and {ServerTime:default:HH:mm:ss.ffffff} fields, select the **Date** and **Time** fields from the **S\# field** window, respectively. We assign the values 2 and 3.
   - For the {TradeId} field, select the **Identifier** field from the **S\# field** window \- the trade identifier or the trade number. We assign it the value 4.
   - For the {TradePrice} field, select the **Price** field \- the trade price from the **S\# field** window. We assign it the value 5.
   - For the {TradeVolume} field, select the **Volume** field from the **S\# field ** window \- the trade volume. We assign it the value 6.
   - For the {OriginSide} field, select the **Initiator** field from the **S\# field** window \- the trade initiator (Seller or Buyer). We assign it the value 7.
   - The field setting window will look like this:![hydra import prop trade](../images/hydra_import_prop_trade.png)

   The user can configure a large number of properties for the downloaded data. Based on the imported file template, you need to specify the property and assign it the required number in the sequence. 
3. To preview the data, click the **Preview** button.![hydra import preview trade](../images/hydra_import_preview_trade.png)
4. Click the **Import** button.
