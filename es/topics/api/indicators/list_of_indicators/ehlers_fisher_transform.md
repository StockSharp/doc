# EFT

**Ehlers Fisher Transform (EFT)** es un indicador técnico desarrollado por John Ehlers que utiliza la transformación estadística de Fisher para convertir datos de precios a una forma distribuida normalmente.

Para utilizar el indicador, debe utilizar la clase [EhlersFisherTransform](xref:StockSharp.Algo.Indicators.EhlersFisherTransform).

## Descripción

El Ehlers Fisher Transform se basa en el concepto de que los precios de mercado no tienen una distribución normal (gaussiana). Más bien, a menudo demuestran distribuciones asimétricas. El indicador aplica una fórmula matemática de transformación de Fisher para convertir estas distribuciones asimétricas en valores distribuidos normalmente.

Esta transformación hace que los movimientos extremos de precios sean más notorios y ayuda a identificar más claramente los puntos de reversión del mercado. Cuando se aplica la transformación de Fisher, los valores máximos aumentan marcadamente, lo que hace que los extremos del comportamiento del mercado sean más obvios.

EFT es particularmente útil para:
- Determinación de posibles puntos de reversión del mercado
- Identificar condiciones de sobrecompra y sobreventa
- Detectar divergencias ocultas entre el precio y el indicador.
- Generando señales de entrada y salida más precisas

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de cálculo (valor predeterminado: 10)

## Cálculo

El cálculo de Ehlers Fisher Transform implica varios pasos:

1. Transforme los datos de precios a valores entre -1 y +1 (normalmente utilizando un rango de precios normalizado u otro oscilador):
   ```
   Value = (2 * ((Price - Min) / (Max - Min))) - 1
   ```
   donde Min y Max son los precios mínimo y máximo durante el período Length.

2. Aplicar la transformación de Fisher:
   ```
   If Value >= 0.999, then Value = 0.999
   If Value <= -0.999, then Value = -0.999
   
   Fisher = 0.5 * ln((1 + Value) / (1 - Value))
   ```
   donde ln es el logaritmo natural.

3. Suave para reducir el ruido:
   ```
   EFT = EMA(Fisher, Period)
   ```
   donde EMA es la media móvil exponencial.

## Interpretación

El Ehlers Fisher Transform se puede interpretar de la siguiente manera:

1. **Valores extremos**:
   - Los valores superiores a +2 suelen indicar condiciones de sobrecompra en el mercado.
   - Los valores por debajo de -2 a menudo indican condiciones de sobreventa del mercado.

2. **Cruces de línea cero**:
   - El cruce de la línea cero de abajo hacia arriba puede verse como una señal alcista
   - El cruce de la línea cero de arriba a abajo puede verse como una señal bajista

3. **Inversión del indicador**:
   - La reversión del indicador desde valores extremos a menudo precede a la reversión del precio

4. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que EFT forma un mínimo más alto
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que EFT forma un máximo más bajo

5. **Pendiente de la línea indicadora**:
   - Una pendiente ascendente pronunciada indica un fuerte impulso ascendente
   - Una pendiente descendente pronunciada indica un fuerte impulso bajista

El Ehlers Fisher Transform se diferencia de muchos otros osciladores en que puede alcanzar valores extremos y permanecer allí durante algún tiempo sin necesariamente revertirse inmediatamente. Esto lo hace útil para identificar fuertes movimientos de tendencia.

![indicator_ehlers_fisher_transform](../../../../images/indicator_ehlers_fisher_transform.png)

## Véase también

[CenterOfGravityOscillator](center_of_gravity_oscillator.md)
[SineWave](sine_wave.md)
[HarmonicOscillator](harmonic_oscillator.md)
[RSI](rsi.md)