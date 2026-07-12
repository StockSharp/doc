# Velas personalizadas

El usuario puede seleccionar un **tipo personalizado** de velas y elegir de forma independiente qué velas se construirán; las velas se construirán "sobre la marcha", es decir, inmediatamente.

![Hydra tipo de vela 00 00](../../../images/hydra_type_candle_00_00.png)

Consideremos un ejemplo de esta construcción. La bolsa **Bitmex** no proporciona la posibilidad de recibir velas con un marco temporal de 10 minutos.

![Hydra tipo de vela 00 01](../../../images/hydra_type_candle_00_01.png)

Secuencia para obtener dichas velas:

1. Seleccione velas **personalizadas**.
2. En la configuración, especifique velas **TF** y un período de 10 minutos.
3. En la fuente, especifique a partir de qué se construirán las velas: **Registro de órdenes** ![Hydra tipo de vela 00 02](../../../images/hydra_type_candle_00_02.png)
4. Establezca el período. Como puede ver, junto al nombre de la vela apareció la indicación **Generadas**.![Hydra tipo de vela 00 03](../../../images/hydra_type_candle_00_03.png)
5. Haga clic en Iniciar y los datos empezarán a descargarse.![Hydra tipo de vela 00 04](../../../images/hydra_type_candle_00_04.png)
6. Vaya a la sección de velas y [vea los datos descargados](../working_with_data/view_and_export.md).![Hydra tipo de vela 00 06](../../../images/hydra_type_candle_00_06.png)

Como puede ver, los datos se han recibido correctamente.

Consideremos un ejemplo en el que necesitamos obtener un [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage):

1. Seleccione velas **personalizadas**.
2. En la configuración, especifique velas Range y el volumen 10.
3. En la fuente, especifique a partir de qué se construirán las velas: **Ticks**.![Hydra tipo de vela 00 07](../../../images/hydra_type_candle_00_07.png)
4. Establezca el período.
5. Haga clic en Iniciar y los datos empezarán a descargarse.![Hydra tipo de vela 00 08](../../../images/hydra_type_candle_00_08.png)
6. Vaya a la sección de velas y [vea los datos descargados](../working_with_data/view_and_export.md).![Hydra tipo de vela 00 09](../../../images/hydra_type_candle_00_09.png)
