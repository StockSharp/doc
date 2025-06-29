# Chart

![Designer Panel graphics 00](../../../../../../images/designer_panel_graphics_00.png)

The cube is designed to display data on the chart in the [Chart](../../../../user_interface/components/chart.md) panel. Each element corresponds to one area on the chart and contains a set of properties with which you can define a set of axes on the chart, a set of graphic elements and the parameters of their display. Adding any graphic element is done by pressing the ![Designer Panel graphics 01](../../../../../../images/designer_panel_graphics_01.png) button, deletion by pressing the ![Designer Panel graphics 02](../../../../../../images/designer_panel_graphics_02.png) button. When adding a graphic element (a series of candles, indicator, trades, etc.) in the settings, a special input parameter is created in the cube, through which the data to display will be passed to this cube, and as a result will be displayed in the [Chart](../../../../user_interface/components/chart.md) panel. When you click the ![Designer Panel graphics 03](../../../../../../images/designer_panel_graphics_03.png) button, the child graphic elements or their settings will be opened.

![Designer Panel graphics 04](../../../../../../images/designer_panel_graphics_04.png)

The [Chart](../../../../user_interface/components/chart.md) panel displays all the data received in the **Chart panel** cubes. For different series of candles, it is necessary to take different **Chart panel** cubes. At that, the charts will be placed one above the other, but only the corresponding graphic elements (candle series, indicator, trades, etc.) will be displayed on each one. The example shows that the top chart displays candles, two indicators and trades, which correspond to four input parameters of the top cube. Only candles are shown on the bottom chart. For more information about the [Chart](../../../../user_interface/components/chart.md) panel, see [Chart](../../../../user_interface/components/chart.md) section.

## Recommended content

[Crossing](crossing.md)
