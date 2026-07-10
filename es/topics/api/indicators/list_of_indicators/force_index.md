# FI

**Force Index (FI)** es un indicador técnico desarrollado por el Dr. Alexander Elder que mide la fuerza de cada movimiento de precios en función de su dirección, magnitud y volumen de operaciones.

Para utilizar el indicador, debe utilizar la clase [ForceIndex](xref:StockSharp.Algo.Indicators.ForceIndex).

## Descripción

El Force Index es un oscilador que mide la fuerza de los "alcistas" (compradores) o "osos" (vendedores) en cada movimiento de precios. Combina tres elementos importantes de la información del mercado: dirección del movimiento de precios, magnitud del movimiento y volumen de operaciones.

La idea principal del indicador es que cuanto mayor sea el cambio de precio y mayor el volumen de operaciones, más fuerte será el movimiento del mercado. Los valores positivos de Force Index indican predominio del comprador (presión alcista), mientras que los valores negativos indican predominio del vendedor (presión bajista).

El Force Index es particularmente útil para:
- Determinando la fuerza de la tendencia actual
- Identificar posibles puntos de reversión
- Confirmando brotes
- Detectar divergencias entre el precio y la fuerza del movimiento.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de suavizado (valor predeterminado: 13)

## Cálculo

El cálculo de Force Index implica los siguientes pasos:

1. Calculando Force Index de un solo período:
   ```
   1-Period Force Index = (Close[current] - Close[previous]) * Volume[current]
   ```

2. Suavizado usando una media móvil exponencial (EMA):
   ```
   Force Index = EMA(1-Period Force Index, Length)
   ```

donde:
- Close - precio de cierre
- Volume - volumen de operaciones
- EMA - media móvil exponencial
- Length - período de suavizado

## Interpretación

El Force Index se puede interpretar de varias maneras:

1. **Cruces de línea cero**:
   - La transición de valores negativos a positivos indica una mayor presión alcista y puede verse como una señal de compra.
   - La transición de valores positivos a negativos indica una mayor presión bajista y puede verse como una señal de venta.

2. **Valores extremos**:
   - Los valores positivos de High indican una fuerte presión alcista que puede conducir a condiciones de sobrecompra en el mercado.
   - Los valores negativos de High indican una fuerte presión bajista que puede llevar a condiciones de sobreventa en el mercado.

3. **Divergencias**:
   - La divergencia alcista (el precio forma un nuevo mínimo, mientras que Force Index forma un mínimo más alto) puede indicar una posible reversión alcista
   - La divergencia bajista (el precio forma un nuevo máximo, mientras que Force Index forma un máximo más bajo) puede indicar una posible reversión a la baja

4. **Confirmación de tendencia**:
   - Los valores Force Index consistentemente positivos confirman la fuerza de una tendencia alcista
   - Los valores Force Index consistentemente negativos confirman la fuerza de una tendencia a la baja

5. **Triple uso** (según Anciano):
   - Short-term Force Index (2 días): para identificar oportunidades a corto plazo
   - Force Index de mediano plazo (13 días): para determinar tendencias y correcciones a mediano plazo
   - Force Index a largo plazo (100 días): para identificar la tendencia principal

6. **Correction Identification**:
   - En una tendencia alcista, los días con Force Index negativo pueden indicar correcciones temporales
   - En una tendencia a la baja, los días con Force Index positivo pueden indicar rebotes temporales

![indicator_force_index](../../../../images/indicator_force_index.png)

## Véase también

[EMA](ema.md)
[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
