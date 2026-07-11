# MP

**pinball de impulso (MP)** es un indicador técnico que analiza el impulso del precio y sus cambios para identificar posibles puntos de reversión y la fuerza de la tendencia en el mercado.

Para utilizar el indicador, debe utilizar la clase [MomentumPinball](xref:StockSharp.Algo.Indicators.MomentumPinball).

## Descripción

El indicador pinball de impulso (MP) es un oscilador especializado diseñado para rastrear el impulso del precio e identificar posibles puntos de reversión. El nombre "pinball" refleja la capacidad del indicador para identificar momentos en los que el precio, como una bola en una máquina de pinball, rebota en posiciones extremas.

MP analiza la relación entre el impulso actual y sus extremos históricos, determinando cuándo el mercado alcanza condiciones de sobrecompra o sobreventa. El indicador también ayuda a identificar momentos en los que el impulso comienza a debilitarse, lo que puede preceder a un cambio de tendencia.

La idea principal es que los valores de impulso extremos suelen ser inestables y, después de alcanzar tales extremos, suele seguir una corrección o reversión. MP ayuda a visualizar este proceso al rastrear tanto el impulso como sus cambios.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de cálculo (valor predeterminado: 14)

## Cálculo

El cálculo del indicador Pinball Momentum implica los siguientes pasos:

1. Calcule el impulso base como la diferencia entre el precio actual y el precio de hace N períodos:
   ```
   Momentum = Price[current] - Price[current - Length]
   ```

2. Determine el impulso histórico máximo y mínimo durante el período especificado:
   ```
   Max_Momentum = Maximum(Momentum) durante el periodo Length
   Min_Momentum = Minimum(Momentum) durante el periodo Length
   ```

3. Normalizar el impulso actual en relación con los extremos históricos:
   ```
   Normalized_Momentum = (Momentum - Min_Momentum) / (Max_Momentum - Min_Momentum)
   ```

4. Calcule la tasa de cambio para el impulso normalizado:
   ```
   Momentum_Change = Normalized_Momentum[current] - Normalized_Momentum[current - 1]
   ```

5. Cálculo final de MP como combinación del impulso normalizado y su cambio:
   ```
   MP = Normalized_Momentum + Momentum_Change
   ```

donde:
- Price - precio (normalmente precio de cierre)
- Length - período de cálculo
- Momentum - impulso base
- Normalized_Momentum - impulso normalizado
- Momentum_Change - cambio de impulso

## Interpretación

El indicador pinball de impulso se puede interpretar de la siguiente manera:

1. **Niveles extremos**:
   - Los valores superiores a 0,8 indican condiciones de sobrecompra en el mercado.
   - Valores por debajo de 0,2 indican condiciones de sobreventa del mercado
   - Cuando MP alcanza estos niveles, aumenta la probabilidad de una reversión o corrección.

2. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que MP forma un mínimo más alto
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que MP forma un máximo más bajo
   - Divergencias a menudo precede a cambios de tendencia significativos

3. **Cruces de línea central**:
   - MP cruzar el nivel 0,5 de abajo hacia arriba puede verse como una señal alcista
   - MP cruzar el nivel 0,5 de arriba a abajo puede verse como una señal bajista

4. **Rebotes desde los extremos**:
   - MP la reversión de los niveles de sobrecompra o sobreventa puede generar señales de entrada al mercado
   - Se forman señales particularmente fuertes cuando tales reversiones van acompañadas de divergencias.

5. **Análisis de tendencias**:
   - Los valores sostenidos de MP por encima de 0,5 confirman una tendencia alcista
   - Los valores sostenidos de MP por debajo de 0,5 confirman una tendencia a la baja
   - Las oscilaciones MP alrededor del nivel 0,5 indican una tendencia lateral o incertidumbre

6. **Fuerza del momentum**:
   - La pendiente pronunciada MP indica un fuerte impulso
   - La pendiente poco profunda de MP indica un impulso débil
   - La desaceleración en la subida o bajada de MP puede preceder a un cambio de tendencia

7. **Combinando con otros indicadores**:
   - MP se utiliza a menudo en combinación con indicadores de tendencia
   - Por ejemplo, los promedios móviles se pueden usar para determinar la dirección de la tendencia, mientras que MP se puede usar para los puntos de entrada y salida.

![indicator_momentum_pinball](../../../../images/indicator_momentum_pinball.png)

## Véase también

[Impulso](momentum.md)
[RSI](rsi.md)
[Oscilador estocástico](stochastic_oscillator.md)
[PrettyGoodOscillator](pretty_good_oscillator.md)
