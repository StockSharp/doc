# LRSI

**RSI de Laguerre (LRSI)** es un indicador técnico basado en los principios matemáticos del filtro de Laguerre, desarrollado por John Ehlers como una versión avanzada del tradicional índice de fuerza relativa (RSI).

Para utilizar el indicador, debe utilizar la clase [LaguerreRSI](xref:StockSharp.Algo.Indicators.LaguerreRSI).

## Descripción

RSI de Laguerre (LRSI) es un oscilador innovador que utiliza matemáticas de filtro Laguerre para crear un indicador más sensible y menos retrasado en comparación con el RSI tradicional. John Ehlers desarrolló este indicador para abordar el problema de retraso de la señal inherente a muchos indicadores técnicos.

LRSI combina los principios polinomiales de Laguerre con el concepto índice de fuerza relativa. Esto permite que el indicador responda más rápidamente a los cambios de tendencia y genere señales de trading más claras. Al igual que el RSI tradicional, el LRSI oscila entre 0 y 1 (o de 0 a 100 cuando se multiplica por 100), pero tiene una estructura menos ruidosa y giros más distintos.

La principal ventaja de LRSI es su capacidad para identificar rápidamente cambios de tendencia manteniendo la estabilidad de la señal. Esto lo hace particularmente útil para operaciones a corto plazo y para determinar puntos de entrada y salida.

## Parámetros

El indicador tiene los siguientes parámetros:
- **Gamma** - coeficiente de filtrado (valor predeterminado: 0,4, rango de 0,1 a 0,9)

El parámetro Gamma determina el grado de filtrado y afecta la sensibilidad del indicador. Los valores más bajos de Gamma dan como resultado un indicador más suave y menos sensible, mientras que los valores más altos hacen que el indicador sea más sensible a los cambios de precios, pero potencialmente más ruidoso.

## Cálculo

El cálculo de RSI de Laguerre implica varios pasos:

1. Inicialice cuatro valores de filtro Laguerre (L0, L1, L2, L3) en la primera ejecución:
   ```
   L0 = L1 = L2 = L3 = 0
   ```

2. Actualice los valores del filtro para cada nuevo precio:
   ```
   L0_new = (1 - Gamma) * Price + Gamma * L0_old
   L1_new = -Gamma * L0_new + L0_old + Gamma * L1_old
   L2_new = -Gamma * L1_new + L1_old + Gamma * L2_old
   L3_new = -Gamma * L2_new + L2_old + Gamma * L3_old
   ```

3. Calcule la "multiplicación acumulativa" de valores filtrados:
   ```
   CU = (L0_new + L1_new + L2_new + L3_new) / 4
   ```

4. Separe en componentes "arriba" y "abajo":
   ```
   Si CU >= CU_old, entonces:
     UP = CU - CU_old
     DN = 0
   Otherwise:
     UP = 0
     DN = CU_old - CU
   ```

5. Cálculo final de LRSI:
   ```
   LRSI = UP / (UP + DN)
   ```

   Si (UP + DN) es cero, LRSI se establece en el valor anterior.

donde:
- Price - precio de entrada (normalmente precio de cierre)
- Gamma - parámetro de filtrado
- CU - "multiplicación acumulativa"

## Interpretación

RSI de Laguerre se interpreta de manera similar al RSI tradicional, pero con mayor sensibilidad:

1. **Niveles de sobrecompra y sobreventa**:
   - Los valores superiores a 0,8 (u 80) normalmente se consideran sobrecomprados.
   - Los valores por debajo de 0,2 (o 20) normalmente se consideran sobrevendidos.
   - Debido a las características del filtro Laguerre, estos niveles pueden ajustarse en función de la volatilidad del mercado.

2. **Cruces de línea central**:
   - Cruzar el nivel 0,5 (o 50) de abajo hacia arriba puede verse como una señal alcista.
   - Cruzar el nivel 0,5 (o 50) de arriba a abajo puede verse como una señal bajista.

3. **Divergencias**:
   - Divergencia alcista: el precio forma un nuevo mínimo, mientras que LRSI forma un mínimo más alto
   - Divergencia bajista: el precio forma un nuevo máximo, mientras que LRSI forma un máximo más bajo

4. **Rebotes desde los extremos**:
   - La reversión LRSI desde niveles de sobrecompra o sobreventa puede servir como señal de entrada al mercado

5. **Confirmación de tendencia**:
   - Los valores LRSI superiores a 0,5 confirman una tendencia alcista
   - Los valores de LRSI por debajo de 0,5 confirman una tendencia a la baja

6. **Ajuste del parámetro gamma**:
   - Para señales más rápidas: aumente Gamma (más cerca de 0,9)
   - Para señales más suaves: disminuya Gamma (más cerca de 0,1)

![indicator_laguerre_rsi](../../../../images/indicator_laguerre_rsi.png)

## Véase también

[RSI](rsi.md)
[AdaptiveLaguerreFilter](adaptive_laguerre_filter.md)
[ConnorsRSI](connors_rsi.md)
