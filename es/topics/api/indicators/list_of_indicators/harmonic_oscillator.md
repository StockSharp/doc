# HO

**oscilador armónico (HO)** es un indicador técnico basado en la teoría de la oscilación armónica que ayuda a identificar componentes cíclicos en el movimiento de precios.

Para utilizar el indicador, debe utilizar la clase [HarmonicOscillator](xref:StockSharp.Algo.Indicators.HarmonicOscillator).

## Descripción

El oscilador armónico (HO) es un indicador desarrollado para identificar la periodicidad y la naturaleza cíclica de los movimientos de los precios del mercado. Se basa en el principio de que muchos movimientos de precios contienen componentes armónicos (periódicos) que pueden aislarse y utilizarse para pronosticar movimientos futuros de precios.

El indicador aplica métodos de análisis espectral para descomponer la serie de precios en componentes armónicos, destacando los ciclos dominantes. Luego muestra estos componentes cíclicos como un oscilador que ayuda a los operadores a determinar cuándo el precio puede alcanzar máximos o mínimos locales dentro de los ciclos identificados.

HO es particularmente útil para:
- Determinar la naturaleza cíclica del mercado
- Identificar posibles puntos de reversión
- Filtrar el ruido del mercado
- Predecir momentos en los que el precio puede cambiar de dirección

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de análisis (valor predeterminado: 30)

## Cálculo

El cálculo de oscilador armónico implica los siguientes pasos:

1. Preprocesamiento de la serie de precios (eliminación de tendencia):
   ```
   Precio sin tendencia = Price - SMA(Price, Length)
   ```

2. Aplicar análisis espectral para identificar ciclos dominantes:
   ```
   Componentes espectrales = FFT(Precio sin tendencia)
   ```

3. Extracción de los componentes armónicos más significativos:
   ```
   Ciclos dominantes = extraer los N principales componentes espectrales según la amplitud
   ```

4. Sintetizando el oscilador armónico en base a ciclos dominantes:
   ```
   HO = reconstrucción de ciclos dominantes mediante FFT inversa
   ```

donde:
- Price - precio (normalmente precio de cierre)
- SMA - media móvil simple
- FFT - Transformada rápida de Fourier
- Length - período de análisis

## Interpretación

El oscilador armónico se puede interpretar de la siguiente manera:

1. **Cruces de línea cero**:
   - Cuando HO cruza la línea cero de abajo hacia arriba, puede verse como una señal alcista.
   - Cuando HO cruza la línea cero de arriba a abajo, puede verse como una señal bajista.

2. **Extremos del oscilador**:
   - Cuando HO alcanza un máximo local, puede indicar un posible pico de precio
   - Cuando HO alcanza un mínimo local, puede indicar un posible fondo de precio

3. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que HO forma un mínimo más alto
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que HO forma un máximo más bajo

4. **Cycle Projection**:
   - Los picos y valles regulares de HO se pueden utilizar para proyectar futuros puntos de inversión
   - Analizar la duración entre peaks/troughs puede ayudar a determinar la duración del ciclo dominante

5. **Cambios de amplitud**:
   - El aumento de la amplitud de oscilación HO puede indicar un fortalecimiento del componente cíclico
   - La amplitud de oscilación HO disminuida puede indicar una atenuación del componente cíclico

6. **Combinación con otros indicadores**:
   - HO funciona mejor en combinación con indicadores de tendencia
   - En mercados de tendencia, las señales HO se pueden utilizar para determinar puntos de entrada en la dirección de la tendencia.

![indicator_harmonic_oscillator](../../../../images/indicator_harmonic_oscillator.png)

## Véase también

[SineWave](sine_wave.md)
[CenterOfGravityOscillator](center_of_gravity_oscillator.md)
[FisherTransform](ehlers_fisher_transform.md)
[DetrendedSyntheticPrice](detrended_synthetic_price.md)
