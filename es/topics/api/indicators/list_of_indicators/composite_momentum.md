# CM

**impulso compuesto (CM)** es un indicador que combina múltiples métodos de medir el impulso del precio para obtener señales más confiables sobre la fuerza y dirección de la tendencia.

Para utilizar el indicador, debe utilizar la clase [CompositeMomentum](xref:StockSharp.Algo.Indicators.CompositeMomentum).

## Descripción

El indicador impulso compuesto (CM) es una herramienta integral que integra varios aspectos del movimiento de precios, incluida la tasa de cambio de precios, la fuerza relativa y otros componentes del impulso. A través de este enfoque combinado, CM proporciona una imagen más completa del impulso actual del mercado en comparación con los indicadores de impulso unidimensionales tradicionales.

CM es eficaz para:
- Determinando la fuerza de la tendencia actual
- Identificar posibles puntos de reversión
- Detectar divergencias entre precio e impulso
- Filtrar señales falsas de otros indicadores

impulso compuesto es particularmente útil en mercados volátiles donde los indicadores de impulso tradicionales pueden generar numerosas señales falsas.

## Cálculo

El cálculo de impulso compuesto implica varias etapas y componentes:

1. Calcular los componentes del impulso:
   - El precio cambia con respecto a períodos anteriores
   - Relación entre máximos y mínimos recientes
   - Análisis de volumen que acompaña al movimiento de precios.

2. Normalizar cada componente para llevarlos a escalas comparables.

3. Suma ponderada de componentes para obtener el valor final CM.

El valor final CM es un oscilador que puede fluctuar tanto en áreas positivas como negativas:
- Los valores positivos indican un impulso alcista
- Los valores negativos indican un impulso a la baja.
- La magnitud del valor (valor absoluto) indica la fuerza del impulso.

## Interpretación

- **Cruce de línea cero**:
  - La transición de la zona negativa a la positiva puede verse como una señal alcista
  - La transición de la zona positiva a la negativa puede verse como una señal bajista

- **Valores extremos**:
  - Valores positivos muy altos pueden indicar condiciones de sobrecompra en el mercado
  - Valores negativos muy bajos pueden indicar condiciones de sobreventa del mercado.

- **Divergencias**:
  - Divergencia alcista: el precio forma un nuevo mínimo, mientras que CM forma un mínimo más alto
  - Divergencia bajista: el precio forma un nuevo máximo, mientras que CM forma un máximo más bajo

- **Confirmación de tendencia**:
  - Los valores CM consistentemente positivos confirman la fuerza de una tendencia alcista
  - Los valores CM consistentemente negativos confirman la fuerza de una tendencia a la baja

- **Pérdida de momentum**:
  - La disminución del valor absoluto de CM en la dirección de la tendencia puede indicar una pérdida de impulso y una posible reversión

![Gráfico del indicador CM](../../../../images/indicator_composite_momentum.png)

## Véase también

[Impulso](momentum.md)
[RoC](roc.md)
[RSI](rsi.md)
[MACD](macd.md)
