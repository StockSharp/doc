# Optimization

To switch to the strategy optimization mode, click the **Optimization** button on the **Emulation** tab.

![Designer Optimization 00](~/images/Designer_Optimization_00.png)

A tab will open in the workspace with the name Optimization + Strategy Name. The Optimization tab is separated into three areas, **Properties**, **Parameters for optimization**, **Optimization result**: 

- In the **Properties** area, you can change emulation settings, basic scheme parameters.
- The **Optimization result** area is a table, were each line is a result of strategy testing with unique parameters. Also, in the **Optimization result** area there is a progress\-bar, showing optimization progress, elapsed time and calculated time to the end of optimization.
- The **Parameters for optimization** consists of two tables. The upper table is a list of strategy parameters, which could be optimized. The lower table adjusts the optimization range and optimization increment for the parameter, selected from the upper table.

Further, optimization will be considered using the SMA strategy as an example. If in the SMA strategy, without preliminary preparation, click the **Optimization** button, there will be only one optimization parameter \- Volume. In order to add the moving average periods as optimization parameters it is necessary in the SMA Indicator Cube check the **Parameter** check box:

![Designer Optimization 01](~/images/Designer_Optimization_01.png)

After checking the **Parameter** check box in the required units and clicking the **Optimization** button, the **SMA Optimization** tab is opened:

![Designer Optimization 02](~/images/Designer_Optimization_02.png)

In the example, one optimization parameter is taken \- the length of short moving average, Short SMA, from 10 to 20 in increments of 1. As a result, we get 11 SMA strategies with different parameters.

## Recommended content

[Getting started](Designer_Example_of_backtesting.md)
