# Candles personalizados

O utilizador pode selecionar um **Custom type** de candles e escolher de forma independente que candles serão construídos, sendo os candles construídos "on the fly", ou seja, imediatamente.

![hydra type candle 00 00](../../../images/hydra_type_candle_00_00.png)

Vejamos um exemplo desta construção. A bolsa **Bitmex** não disponibiliza a possibilidade de receber candles com um Time Frame de 10 minutos.

![hydra type candle 00 01](../../../images/hydra_type_candle_00_01.png)

A sequência para obter esses candles:

1. Selecione candles **Custom**
2. Nas definições, especificamos candles **TF** e um período de 10 minutos
3. Na fonte, especificamos a partir do que os candles serão construídos - **Order Log** ![hydra type candle 00 02](../../../images/hydra_type_candle_00_02.png)
4. Definimos o período. Como pode ver, junto ao nome do candle apareceu a indicação **Generated**.![hydra type candle 00 03](../../../images/hydra_type_candle_00_03.png)
5. Clique em Start e os dados começam a ser descarregados.![hydra type candle 00 04](../../../images/hydra_type_candle_00_04.png)
6. Vamos à secção de candles e [ver os dados descarregados](../working_with_data/view_and_export.md).![hydra type candle 00 06](../../../images/hydra_type_candle_00_06.png)

Como pode ver, os dados foram recebidos com sucesso.

Considere um exemplo em que precisamos de obter uma [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage):

1. Selecione candles **Custom**.
2. Nas definições, especifique os candles Range e o volume 10.
3. Na fonte, especificamos a partir do que os candles serão construídos - **Ticks**.![hydra type candle 00 07](../../../images/hydra_type_candle_00_07.png)
4. Definimos o período.
5. Clique em Start e os dados começam a ser descarregados.![hydra type candle 00 08](../../../images/hydra_type_candle_00_08.png)
6. Vamos à secção de candles e [ver os dados descarregados](../working_with_data/view_and_export.md).![hydra type candle 00 09](../../../images/hydra_type_candle_00_09.png)
