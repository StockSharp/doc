# III

**índice de intensidad intradía (III)** es un indicador técnico desarrollado por David Bostian que evalúa la relación entre el precio de cierre, el rango de precios y el volumen de negociación dentro de un día de negociación.

Para utilizar el indicador, debe utilizar la clase [IntradayIntensityIndex](xref:StockSharp.Algo.Indicators.IntradayIntensityIndex).

## Descripción

El índice de intensidad intradía (III) combina información sobre el movimiento de precios y el volumen de negociación para evaluar la intensidad de la presión de compra o venta dentro de un día de negociación. El indicador se basa en el supuesto de que la posición del precio de cierre en relación con el rango de precios del día, combinado con el volumen, puede indicar la dirección y la fuerza del movimiento del mercado.

III es particularmente útil para identificar cambios intradía en el sentimiento del mercado y determinar posibles puntos de reversión. Los valores positivos del indicador indican presión de compra (precio de cierre más cercano al máximo del día), mientras que los valores negativos indican presión de venta (precio de cierre más cercano al mínimo del día).

El índice de intensidad intradía es especialmente eficaz para:
- Identificar cambios intradía en el sentimiento del mercado
- Determinación de posibles puntos de reversión
- Confirmación de señales de otros indicadores.
- Evaluación de la fuerza de la tendencia actual

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de suavizado (valor predeterminado: 14)

## Cálculo

El cálculo de índice de intensidad intradía implica los siguientes pasos:

1. Calcule el valor III individual para cada período:
   ```
   III bruto = ((2 * Close - High - Low) / ((High - Low) * Volume)) * Volume
   ```

2. Suave usando una media móvil simple:
   ```
   III = SMA(III bruto, Length)
   ```

donde:
- Close - precio de cierre
- High - precio más alto del período
- Low - precio más bajo del período
- Volume - volumen de operaciones
- SMA - media móvil simple
- Length - período de suavizado

## Interpretación

El índice de intensidad intradía se puede interpretar de la siguiente manera:

1. **Cruces de línea cero**:
   - La transición de valores negativos a positivos puede verse como una señal alcista, que indica una mayor presión de compra.
   - La transición de valores positivos a negativos puede verse como una señal bajista, lo que indica una mayor presión de venta.

2. **Valores extremos**:
   - Los valores positivos altos indican una fuerte presión de compra
   - Los valores negativos extremos indican una fuerte presión de venta
   - Valores extremos puede indicar condiciones de sobrecompra o sobreventa del mercado

3. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que III forma un mínimo más alto
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que III forma un máximo más bajo

4. **Tendencias de III**:
   - Los valores III consistentemente positivos confirman una tendencia alcista
   - Los valores III consistentemente negativos confirman una tendencia a la baja
   - Las oscilaciones alrededor de la línea cero pueden indicar una tendencia lateral o incertidumbre

5. **Combinando con otros indicadores**:
   - III se utiliza a menudo en combinación con otros indicadores técnicos para confirmar señales
   - Particularmente eficaz cuando se combina con indicadores de tendencia y volumen.

6. **Cambios en los valores**:
   - El rápido cambio de valores negativos a positivos puede indicar un cambio brusco en el sentimiento del mercado
   - La convergencia gradual hacia la línea cero puede indicar un debilitamiento del impulso actual

![III](../../../../images/indicator_intraday_intensity_index.png)

## Véase también

[IntradayMomentumIndex](intraday_momentum_index.md)
[BalanceOfPower](balance_of_power.md)
[ForceIndex](force_index.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
