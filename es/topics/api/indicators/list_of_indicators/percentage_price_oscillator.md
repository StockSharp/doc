# PPO

**Oscilador porcentual de precio (PPO)** es un indicador técnico similar a MACD, pero expresa la diferencia entre dos medias móviles exponenciales como porcentaje en lugar de valores absolutos.

Para utilizar el indicador, debe utilizar la clase [PercentagePriceOscillator](xref:StockSharp.Algo.Indicators.PercentagePriceOscillator).

## Descripción

El oscilador porcentual de precio (PPO) es una variación del más conocido indicador MACD (Convergencia/Divergencia de Medias Móviles). La principal diferencia es que PPO expresa la diferencia entre dos medias móviles exponenciales como porcentaje, en lugar de en unidades absolutas. Esto hace que PPO sea particularmente útil al comparar diferentes instrumentos con diferentes niveles de precios o al analizar un solo instrumento durante un largo período cuando su precio ha cambiado significativamente.

PPO consta de tres componentes:
1. **Línea PPO** - diferencia entre EMA rápido y lento, expresada como porcentaje
2. **Línea de señal** - EMA de la línea PPO
3. **Histograma** - diferencia entre la línea PPO y la línea de señal

El indicador PPO oscila alrededor de la línea cero, donde los valores positivos indican un sentimiento de mercado alcista y los valores negativos indican un sentimiento bajista. La magnitud de la desviación respecto de cero refleja la fuerza de la tendencia actual.

## Parámetros

El indicador tiene los siguientes parámetros:
- **ShortPeriod** - período para calcular EMA corto (valor predeterminado: 12)
- **LongPeriod** - período para calcular EMA largo (valor predeterminado: 26)

## Cálculo

El cálculo del PPO implica los siguientes pasos:

1. Calcule medias móviles exponenciales cortas y largas:
   ```
   EMA corta = EMA(Precio, ShortPeriod)
   EMA larga = EMA(Precio, LongPeriod)
   ```

2. Calcule la línea PPO como diferencia porcentual entre EMA corto y largo:
   ```
   Línea PPO = ((EMA corta - EMA larga) / EMA larga) * 100
   ```

3. Calcule la línea de señal (normalmente EMA de 9 períodos de la línea PPO):
   ```
   Línea de señal = EMA(Línea PPO, 9)
   ```

4. Calcular histograma:
   ```
   Histograma = Línea PPO - Línea de señal
   ```

donde:
- Price - precio (normalmente precio de cierre)
- EMA - media móvil exponencial
- ShortPeriod - período corto EMA
- LongPeriod - período largo EMA

## Interpretación

El PPO se puede interpretar de la siguiente manera:

1. **Cruces de línea cero**:
   - Que la línea PPO cruce la línea cero de abajo hacia arriba puede verse como una señal alcista
   - Que la línea PPO cruce la línea cero de arriba a abajo puede verse como una señal bajista

2. **Cruces de línea de señal**:
   - Que la línea PPO cruce la línea de señal de abajo hacia arriba puede verse como una señal alcista
   - Que la línea PPO cruce la línea de señal de arriba a abajo puede verse como una señal bajista

3. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que PPO forma un mínimo más alto
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que PPO forma un máximo más bajo

4. **Sobrecompra/Sobreventa**:
   - Los valores PPO positivos extremadamente altos pueden indicar condiciones de sobrecompra en el mercado
   - Los valores PPO negativos extremadamente bajos pueden indicar condiciones de sobreventa del mercado

5. **Análisis del histograma**:
   - La expansión del histograma indica un fortalecimiento de la tendencia actual
   - La contracción del histograma indica un debilitamiento de la tendencia actual
   - El cambio en el color (o signo) del histograma indica un cambio en el impulso a corto plazo

6. **Comparación de instrumentos**:
   - A diferencia de MACD, PPO se puede utilizar para comparar directamente diferentes instrumentos
   - Los valores PPO más altos para un instrumento en comparación con otro pueden indicar un impulso relativo más fuerte

7. **Filtrado de señal**:
   - Las señales de cruce de la línea de señal son más confiables cuando PPO está en línea con la tendencia principal
   - Por ejemplo, las señales alcistas son más confiables cuando PPO es positivo y las señales bajistas son más confiables cuando PPO es negativo.

![PPO](../../../../images/indicator_percentage_price_oscillator.png)

## Véase también

[MACD](macd.md)
[EMA](ema.md)
[Señal del PPO](percentage_price_oscillator_signal.md)
[Histograma del PPO](percentage_price_oscillator_histogram.md)
[Oscilador porcentual de volumen](percentage_volume_oscillator.md)
[TRIX](trix.md)
