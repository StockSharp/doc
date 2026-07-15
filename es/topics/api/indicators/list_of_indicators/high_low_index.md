# HLI

**índice máximo-mínimo (HLI)** es un indicador técnico que mide la relación entre el número de acciones que alcanzan nuevos máximos y el número de acciones que alcanzan nuevos mínimos durante un período de tiempo específico.

Para utilizar el indicador, debe utilizar la clase [HighLowIndex](xref:StockSharp.Algo.Indicators.HighLowIndex).

## Descripción

El índice máximo-mínimo (HLI) es un indicador de amplitud del mercado que analiza la actividad general del mercado comparando la cantidad de instrumentos que alcanzan nuevos máximos con el número que alcanza nuevos mínimos. Esto permite evaluar la fortaleza o debilidad interna del mercado.

La idea principal del indicador es que un mercado saludable se caracteriza por que más valores alcanzan nuevos máximos que nuevos mínimos. Por el contrario, un mercado que se debilita hará que más valores alcancen nuevos mínimos.

HLI es particularmente útil para:
- Evaluación de la condición general del mercado
- Identificación de divergencias entre el índice y los instrumentos de mercado individuales.
- Determinación de posibles puntos de reversión del mercado
- Confirmación de señales de otros indicadores.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Longitud** - período de cálculo (valor predeterminado: 14)

## Cálculo

El cálculo de índice máximo-mínimo implica los siguientes pasos:

1. Cuente la cantidad de valores que alcanzaron nuevos máximos durante el período Length:
   ```
   nuevos máximos = número de instrumentos que alcanzan nuevos máximos durante el periodo Length
   ```

2. Cuente la cantidad de valores que alcanzaron nuevos mínimos durante el período Length:
   ```
   nuevos mínimos = número de instrumentos que alcanzan nuevos mínimos durante el periodo Length
   ```

3. Calcule índice máximo-mínimo como la relación entre la diferencia entre nuevos máximos y mínimos y su suma:
   ```
   HLI = ((nuevos máximos - nuevos mínimos) / (nuevos máximos + nuevos mínimos)) * 100
   ```

Nota: Si (Nuevos máximos + Nuevos mínimos) es igual a cero, HLI se establece en cero para evitar la división por cero.

## Interpretación

El índice máximo-mínimo se interpreta de la siguiente manera:

1. **Rango de valores**:
   - HLI oscila entre -100 y +100
   - Los valores positivos indican que más valores alcanzan nuevos máximos que nuevos mínimos.
   - Los valores negativos indican que más valores alcanzan nuevos mínimos que nuevos máximos.

2. **Cruces de línea cero**:
   - La transición de valores negativos a positivos puede verse como una señal alcista
   - La transición de valores positivos a negativos puede verse como una señal bajista.

3. **Valores extremos**:
   - Valores cercanos a +100 indican un mercado alcista fuerte (posible condición de sobrecompra)
   - Valores cercanos a -100 indican un mercado bajista fuerte (posible condición de sobreventa)

4. **Divergencias**:
   - Divergencia alcista: el índice de mercado alcanza un nuevo mínimo, pero HLI forma un mínimo más alto
   - Divergencia bajista: el índice de mercado alcanza un nuevo máximo, pero HLI forma un máximo más bajo

5. **Tendencias de HLI**:
   - El crecimiento sostenido de HLI indica el fortalecimiento de un mercado alcista
   - La caída sostenida de HLI indica el fortalecimiento de un mercado bajista

6. **Confirmación de la tendencia del mercado**:
   - Si el índice de mercado sube y HLI también sube, esto confirma la fuerza de una tendencia alcista.
   - Si el índice del mercado cae y HLI también cae, esto confirma la fuerza de una tendencia a la baja.

![Gráfico del indicador HLI](../../../../images/indicator_high_low_index.png)

## Véase también

[McClellanOscillator](mcclellan_oscillator.md)
