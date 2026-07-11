# PGO

**Pretty Good Oscillator (PGO)** es un indicador técnico desarrollado por Mark Johnson que compara el precio de cierre actual con precios anteriores, considerando la volatilidad, para determinar las condiciones de sobrecompra o sobreventa del mercado.

Para utilizar el indicador, debe utilizar la clase [PrettyGoodOscillator](xref:StockSharp.Algo.Indicators.PrettyGoodOscillator).

## Descripción

El Pretty Good Oscillator (PGO) es un indicador que evalúa la fortaleza del precio de cierre actual en relación con sus valores históricos durante un período específico. PGO tiene en cuenta no sólo la posición del precio actual en el rango histórico sino también la volatilidad de este rango, lo que lo hace más adaptable a las condiciones cambiantes del mercado.

El nombre "Pretty Good Oscillator" refleja el enfoque pragmático de su creador: el indicador no pretende ser una herramienta perfecta, pero ofrece una forma "bastante buena" de evaluar la situación actual del mercado.

PGO es particularmente útil para identificar condiciones de sobrecompra y sobreventa, así como para detectar divergencias que pueden preceder a los cambios de tendencia.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de cálculo (valor predeterminado: 14)

## Cálculo

El cálculo de Pretty Good Oscillator implica los siguientes pasos:

1. Determine el máximo más alto (Highest High) y el mínimo más bajo (Lowest Low) durante el período especificado:
   ```
   Highest High = Highest(High, Length)
   Lowest Low = Lowest(Low, Length)
   ```

2. Calcule la desviación estándar de los precios de cierre durante el período especificado:
   ```
   Desviación estándar = StdDev(Close, Length)
   ```

3. Calcule el Pretty Good Oscillator:
   ```
   PGO = (Close - (Highest High + Lowest Low) / 2) / desviación estándar
   ```

donde:
- Close - precio de cierre actual
- High - precio más alto
- Low - precio más bajo
- Length - período de cálculo
- StdDev - desviación estándar

## Interpretación

El Pretty Good Oscillator se puede interpretar de la siguiente manera:

1. **Niveles de sobrecompra y sobreventa**:
   - Los valores superiores a +2 suelen indicar condiciones de sobrecompra en el mercado.
   - Los valores por debajo de -2 a menudo indican condiciones de sobreventa del mercado.
   - Valores extremos (+3/-3 y por encima/por debajo) pueden indicar condiciones significativas de sobrecompra/sobreventa y una posible reversión

2. **Cruces de línea cero**:
   - PGO cruzar la línea cero de abajo hacia arriba puede verse como una señal alcista
   - PGO cruzar la línea cero de arriba a abajo puede verse como una señal bajista

3. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que PGO forma un mínimo más alto
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que PGO forma un máximo más bajo
   - Divergencias a menudo precede a cambios de tendencia significativos

4. **Confirmación de tendencia**:
   - Los valores positivos de PGO indican que el precio está por encima del rango promedio, característico de una tendencia alcista.
   - Los valores negativos de PGO indican que el precio está por debajo del rango promedio, característico de una tendencia a la baja.

5. **Evaluación de la fuerza de la tendencia**:
   - Cuanto más lejos esté el valor PGO de cero, más fuerte será la tendencia actual
   - PGO converge con la línea cero puede indicar un debilitamiento de la tendencia

6. **Filtrado de señal**:
   - PGO se puede utilizar para filtrar señales de otros indicadores
   - Por ejemplo, considere solo señales alcistas cuando PGO sea positivo y solo señales bajistas cuando PGO sea negativo.

7. **Salidas de posición**:
   - Los valores extremos de PGO se pueden utilizar como señales para obtener ganancias
   - Por ejemplo, salga de posiciones largas cuando PGO supere +2 y de posiciones cortas cuando PGO caiga por debajo de -2

![indicator_pretty_good_oscillator](../../../../images/indicator_pretty_good_oscillator.png)

## Véase también

[RSI](rsi.md)
[Oscilador estocástico](stochastic_oscillator.md)
[CCI](cci.md)
[StandardDeviation](standard_deviation.md)