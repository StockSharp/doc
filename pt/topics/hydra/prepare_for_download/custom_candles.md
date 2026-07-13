# Velas personalizadas

O utilizador pode selecionar um **tipo personalizado** de velas e escolher de forma independente quais velas serão construídas, sendo as velas construídas imediatamente.

![Hydra tipo de vela 00 00](../../../images/hydra_type_candle_00_00.png)

Vejamos um exemplo desta construção. A bolsa **Bitmex** não disponibiliza a possibilidade de receber velas com um período de 10 minutos.

![Hydra tipo de vela 00 01](../../../images/hydra_type_candle_00_01.png)

A sequência para obter essas velas:

1. Selecione velas **personalizados**
2. Nas definições, especificamos velas **TF** e um período de 10 minutos
3. Na fonte, especificamos a partir do que as velas serão construídas - **Registo de ordens** ![Hydra tipo de vela 00 02](../../../images/hydra_type_candle_00_02.png)
4. Definimos o período. Como pode ver, junto ao nome da vela apareceu a indicação **Gerados**.![Hydra tipo de vela 00 03](../../../images/hydra_type_candle_00_03.png)
5. Clique em Iniciar e os dados começam a ser descarregados.![Hydra tipo de vela 00 04](../../../images/hydra_type_candle_00_04.png)
6. Vamos à secção de velas e [ver os dados descarregados](../working_with_data/view_and_export.md).![Hydra tipo de vela 00 06](../../../images/hydra_type_candle_00_06.png)

Como pode ver, os dados foram recebidos com sucesso.

Considere um exemplo em que precisamos de obter uma [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage):

1. Selecione velas **personalizados**.
2. Nas definições, especifique as velas Range e o volume 10.
3. Na fonte, especificamos a partir do que as velas serão construídas - **Ticks**.![Hydra tipo de vela 00 07](../../../images/hydra_type_candle_00_07.png)
4. Definimos o período.
5. Clique em Iniciar e os dados começam a ser descarregados.![Hydra tipo de vela 00 08](../../../images/hydra_type_candle_00_08.png)
6. Vamos à secção de velas e [ver os dados descarregados](../working_with_data/view_and_export.md).![Hydra tipo de vela 00 09](../../../images/hydra_type_candle_00_09.png)
