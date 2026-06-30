# Converter

The task converts exchange data. For example, from the Order Logs to ticks or from ticks to candles, etc.

![hydra tasks converter](../../../images/hydra_tasks_converter.png)

**Converter**

- **Converter** - converter. 
- **From** - what data type will be converted. 
- **Data format** - converted data format. 
- **Start date** - from what date to start data conversion. 
- **Time offset** - the time offset in days from the date the task was started. This prevents converting an incomplete day. If real-time data conversion is configured, the update interval can leave the current day only partially converted. Use the time offset to avoid that.
- **Where** - the data directory where the converted data will be saved. 

**Order books**

- **Interval** - order book generation interval. 
- **Depth** - maximum depth of order book generation. 
- **Order log** - how to build order books from the order log. 

  Each exchange has its own **Order Log** format, the [Hydra](../../hydra.md) program supports three formats:
  - **By default** - is used in most cases.
  - **ITCH** - is used for the ITCH protocol (exchanges: LSE and Nasdaq).

**General**

- **Header** - Converter. 
- **Working hours** - setting up the board work schedule. ![hydra tasks backup desk](../../../images/hydra_tasks_backup_desk.png)
- **Interval of operation** - the interval of operation. 
- **Data directory** - data directory, from where the data for conversion will be received. 
- **Format** - the converted data format: BIN\/CSV. 
- **Max. errors** - the maximum number of errors, upon which the task will be stopped. By default, 0 - the number of errors is ignored. 
- **Dependency** - a task that must be performed before running the current one. 

**Logging**

- **Identifier** - the identifier. 
- **Logging level** - the logging level. 

Let's consider an example of data conversion.

1. Go to the **Converter** task. ![hydra tasks converter 00](../../../images/hydra_tasks_converter_00.png)
2. Select the instrument and in the window that appears, set the data type that we should receive during conversion, as well as the data type from which we should convert. For example, you need to convert Ticks into candles with a Time Frame of 15 minutes.

   > [!TIP]
> IMPORTANT\! The requested data period must match the period available for conversion; otherwise, the data will not be converted. In the settings, specify the correct source data format so that it matches the format of the data being converted.
3. Specify the required directories. Time offset. Operation interval. 
4. We start the conversion.![hydra tasks converter 01](../../../images/hydra_tasks_converter_01.png)

It can be seen that the data has been converted. [Let's review](../working_with_data/view_and_export.md) the resulting data. 

![hydra tasks converter 02](../../../images/hydra_tasks_converter_02.png)

This function is similar to [getting the required market data](../working_with_data/any_market_data_types.md) from another data type. 

**Watch [video tutorial](../videos/converter_task.md)**
