# FVE

**Finite Volume Element (FVE)** es un indicador técnico desarrollado para analizar la relación entre precio y volumen, ayudando a evaluar la presión del comprador y del vendedor en el mercado.

Para utilizar el indicador, debe utilizar la clase [FiniteVolumeElement](xref:StockSharp.Algo.Indicators.FiniteVolumeElement).

## Descripción

El Finite Volume Element (FVE) analiza la relación entre los cambios de precios y los volúmenes de negociación para determinar la fuerza potencial del movimiento de precios. Se basa en el supuesto de que los cambios de precios son más significativos cuando son confirmados por los volúmenes correspondientes.

El indicador FVE convierte el cambio de precio, ponderado por volumen, en un oscilador que ayuda a determinar el equilibrio relativo entre compradores y vendedores en el mercado. Los valores positivos de FVE indican predominio del comprador, mientras que los valores negativos indican predominio del vendedor.

FVE es particularmente útil para:
- Evaluación de la fuerza y sostenibilidad de la tendencia actual
- Identificar posibles puntos de reversión
- Determinación de divergencias entre precio y volumen.
- Confirmación de señales de otros indicadores.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de suavizado (valor predeterminado: 22)

## Cálculo

El cálculo del indicador FVE implica varios pasos:

1. Calcule el precio típico para los períodos actual y anterior:
   ```
   Precio típico = (High + Low + Close) / 3
   ```

2. Calcule el cambio de precio típico:
   ```
   Cambio de precio = precio típico[current] - precio típico[previous]
   ```

3. Calcule el cambio de precio ponderado por volumen:
   ```
   Cambio de precio ponderado por volumen = cambio de precio * Volume[current]
   ```

4. Normalizar para tener en cuenta la escala del mercado:
   ```
   Valor normalizado = cambio de precio ponderado por volumen / (volumen promedio durante el periodo * volatilidad del precio)
   ```

5. Suma acumulativa y suavizado:
   ```
   FVE = SMA(suma acumulada de valores normalizados, Length)
   ```

donde:
- High, Low, Close: precios más alto, más bajo y de cierre
- Volume - volumen de operaciones
- SMA - media móvil simple
- Length - período de suavizado

## Interpretación

El indicador FVE se puede interpretar de la siguiente manera:

1. **Cruces de línea cero**:
   - La transición de valores negativos a positivos indica una mayor presión del comprador y puede verse como una señal alcista.
   - La transición de valores positivos a negativos indica una mayor presión del vendedor y puede verse como una señal bajista.

2. **Valores extremos**:
   - Los valores positivos altos (por encima de +3) pueden indicar condiciones de sobrecompra del mercado
   - Los valores negativos extremos (por debajo de -3) pueden indicar condiciones de sobreventa del mercado

3. **Divergencias**:
   - La divergencia alcista (el precio forma un nuevo mínimo, mientras que FVE forma un mínimo más alto) puede indicar una posible reversión alcista
   - La divergencia bajista (el precio forma un nuevo máximo, mientras que FVE forma un máximo más bajo) puede indicar una posible reversión a la baja

4. **Confirmación de tendencia**:
   - Los valores FVE consistentemente positivos confirman la fuerza de una tendencia alcista
   - Los valores FVE consistentemente negativos confirman la fuerza de una tendencia a la baja

5. **FVE Change Rate**:
   - El rápido aumento o disminución de los valores de FVE puede indicar un fuerte impulso de movimiento de precios
   - La desaceleración de los cambios de valor de FVE puede indicar una posible desaceleración del impulso

6. **Niveles de soporte y resistencia**:
   - Los puntos de reversión históricos en el gráfico FVE pueden servir como guía para futuras reversiones.

![indicator_finite_volume_element](../../../../images/indicator_finite_volume_element.png)

## Véase también

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[ForceIndex](force_index.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)