# Index

With [S\#.Data](Hydra.md), you can create your own index.

To do this, on the **Common** tab, you need to select **Securities**, the **All Securities** tab will appear.

Before creating the **Index**, you need to check which market data is available. To do this, select the path where the data stored and in turn view the instruments that are supposed to participate in the calculation of the index. If there are omissions, then you need to download the necessary market data (for example, from Finam).

![HydraGluingCheckData](~/images/HydraGluingCheckData.png)

As an example, the instrument ratio index AAPL@NYSE\/GOOG@NYSE will be considered.

The first step is the creation of the **Index**. To do this, click on the **All Securities** tab, the **Create security\=\>Index** button, after that, the following window will appear:

![hydra index sec](~/images/hydra_index_sec.png)

To create the **Index** instrument, you must specify a name and add the mathematical formula of a combination of several instruments. Together with the standard mathematical operators, you can use the following functions:

- **abs(a)** \- Returns the absolute value of a number.
- **acos(a)** \- Returns the angle whose cosine is equal to the specified number.
- **asin(a)** \- Returns the angle whose sine is equal to the specified number.
- **atan(a)** \- Returns the angle whose tangent is equal to the specified number.
- **ceiling(a)** \- Returns the smallest integer that is greater than or equal to a specified number.
- **cos(a)** \- Returns the cosine of the specified angle.
- **exp(a)** \- Returns the value of e raised to the specified degree.
- **floor(a)** \- Returns the largest integer that is less than or equal to the specified number.
- **log(a)** \- Returns the natural logarithm (with base e) of the specified number.
- **log10(a)** \- Returns the logarithm with base 10 of the specified number.
- **max (a, b)** \- Returns the larger of two decimal numbers.
- **min(a, b)** \- Returns the smaller of two decimal numbers.
- **pow(a, b)** \- Returns the specified number raised to the specified power.
- **sign(a)** \- Returns an integer indicating the sign of the specified number.
- **sin(a)** \- Returns the sine of the specified angle.
- **sqrt (a)** \- Returns the square root of the specified number.
- **tan(a)** \- Returns the tangent of the specified angle.
- **truncat(a)** \- Calculates the integer part of the specified number.

Next, you need to click the [Candles](HydraExportCandles.md) button on the **Common** tab, select the obtained **Index** instrument, the data period, in the **Create From:** field set the **Composite Element** value. Then press the button ![hydra find](~/images/hydra_find.png).

![hydra index candle](~/images/hydra_index_candle.png)

The generated data can be exported to Excel, xml or txt formats. Export is done using the drop\-down list:

![hydra export](~/images/hydra_export.png)
