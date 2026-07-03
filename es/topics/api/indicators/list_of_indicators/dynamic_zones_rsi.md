# DZRSI

**Dynamic Zones RSI (DZRSI)** es una modificación del clásico Relative Strength Index (RSI) que utiliza niveles de sobrecompra y sobreventa que cambian dinámicamente en lugar de niveles estáticos.

Para utilizar el indicador, debe utilizar la clase [DynamicZonesRSI](xref:StockSharp.Algo.Indicators.DynamicZonesRSI).

## Descripción

El Dynamic Zones RSI (DZRSI) se basa en el tradicional RSI, pero con una mejora importante: en lugar de utilizar niveles fijos de sobrecompra y sobreventa (normalmente 70 y 30), DZRSI adapta estos niveles a las condiciones actuales del mercado.

La idea principal de DZRSI es que diferentes condiciones de mercado requieren diferentes umbrales para determinar los estados de sobrecompra y sobreventa. En una fuerte tendencia alcista, RSI puede permanecer por encima del nivel de sobrecompra tradicional de 70 durante un período prolongado sin proporcionar señales precisas de entrada o salida. De manera similar, en una fuerte tendencia a la baja, RSI puede permanecer por debajo del nivel de sobreventa de 30 durante mucho tiempo.

DZRSI resuelve este problema ajustando dinámicamente estos niveles en función del comportamiento histórico del propio RSI, lo que hace que el indicador se adapte más a diversos regímenes de mercado.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período para calcular la base RSI (valor predeterminado: 14)
- **OverboughtLevel** - nivel de sobrecompra inicial (valor predeterminado: 70)
- **OversoldLevel** - nivel de sobreventa inicial (valor predeterminado: 30)

## Cálculo

El cálculo de DZRSI implica varios pasos:

1. Calcule el RSI estándar durante el período Length especificado:
   ```
   RSI = 100 - (100 / (1 + RS))
   RS = Average Positive Change / Average Negative Change
   ```

2. Determine el rango de oscilación RSI durante un período histórico específico.

3. Adapte los niveles de sobrecompra y sobreventa en función de este rango:
   ```
   Dynamic Overbought Level = Base Overbought Level + Adjustment Based on Historical Data
   Dynamic Oversold Level = Base Oversold Level - Adjustment Based on Historical Data
   ```

4. Ajuste las zonas dinámicas según la fuerza de la tendencia actual.

## Interpretación

DZRSI se interpreta de manera similar al RSI tradicional, pero teniendo en cuenta las zonas dinámicas:

1. **Señales de sobrecompra y sobreventa**:
   - Cuando DZRSI supera el nivel de sobrecompra dinámica actual, puede indicar condiciones de sobrecompra en el mercado.
   - Cuando DZRSI cae por debajo del nivel de sobreventa dinámico actual, puede indicar condiciones de sobreventa en el mercado.

2. **Señales de inversión**:
   - La reversión del nivel dinámico de sobrecompra a la baja puede verse como una señal de venta.
   - La reversión del nivel dinámico de sobreventa al alza puede verse como una señal de compra.

3. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que DZRSI forma un mínimo más alto
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que DZRSI forma un máximo más bajo

4. **Análisis de tendencias**:
   - En una tendencia alcista, el nivel dinámico de sobreventa puede ser más alto que el tradicional 30
   - En una tendencia a la baja, el nivel de sobrecompra dinámica puede ser inferior al tradicional 70

5. **Cruces de línea central (50)**:
   - Cruzar de abajo hacia arriba puede verse como una señal alcista
   - Cruzar de arriba a abajo puede verse como una señal bajista

![indicator_dynamic_zones_rsi](../../../../images/indicator_dynamic_zones_rsi.png)

## Véase también

[RSI](rsi.md)
[ConnorsRSI](connors_rsi.md)
[LRSI](laguerre_rsi.md)