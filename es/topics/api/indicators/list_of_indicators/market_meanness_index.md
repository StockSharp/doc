# MMI

**Índice de comportamiento adverso del mercado (MMI)** es un indicador técnico desarrollado para determinar si el mercado se encuentra en un estado de tendencia o lateral (caótico).

Para utilizar el indicador, debe utilizar la clase [MarketMeannessIndex](xref:StockSharp.Algo.Indicators.MarketMeannessIndex).

## Descripción

El índice de “maldad” del mercado (MMI) es una herramienta que ayuda a los operadores a determinar la naturaleza del mercado actual, ya sea en tendencia o lateral. El nombre “maldad” refleja la idea de que el mercado a veces se comporta de forma “malvada” o impredecible con los operadores, especialmente durante los movimientos laterales.

MMI se basa en contar el número de pares precio-valor (normalmente precios de cierre) que no siguen un patrón lineal simple, y su relación con el número total de pares analizados. El indicador mide el "caos" o la "aleatoriedad" del movimiento de precios durante un período específico.

El índice oscila de 0 a 100:
- Los valores bajos (generalmente por debajo de 50) indican predominio del movimiento tendencial
- Los valores altos (normalmente superiores a 50) indican predominio del movimiento lateral o caótico

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de cálculo (valor predeterminado: 20)

## Cálculo

El cálculo del índice de “maldad” del mercado implica los siguientes pasos:

1. Cree un conjunto de pares de precios de cierre consecutivos (Close) dentro del período Length determinado.

2. Cuente el número de pares "no secuenciales". Un par se considera no secuencial si no sigue el patrón lineal típico de una tendencia. Si dos pares consecutivos (P1, P2) y (P2, P3) tienen direcciones opuestas (diferentes signos de diferencia), el par se considera no secuencial.

3. Calcule MMI como porcentaje:
   ```
   MMI = (Número de pares no secuenciales / Número total de pares) * 100
   ```

Formalmente, esto se puede representar como:
1. Para cada trío de precios consecutivos (Close[i-2], Close[i-1], Close[i]), verifique:
   - Si (Close[i-1] - Close[i-2]) * (Close[i] - Close[i-1]) < 0, el par se considera no secuencial
   - Cuente el número total de dichos pares.

2. MMI = (Número de pares no secuenciales / (Length - 2)) * 100

## Interpretación

El índice de “maldad” del mercado se puede interpretar de la siguiente manera:

1. **Niveles del indicador**:
   - MMI > 50: El mercado está en un estado lateral o caótico
   - MMI < 50: el mercado está en un estado de tendencia
   - Cuanto más cerca esté MMI de 100, más caótico será el mercado
   - Cuanto más cerca esté MMI de 0, más pronunciada será la tendencia

2. **Aplicación en estrategias de negociación**:
   - Cuando MMI es alto (>50), utilice estrategias orientadas al mercado lateral (por ejemplo, operativa en rango u osciladores).
   - Cuando MMI es bajo (<50), utilice estrategias tendenciales (por ejemplo, seguimiento de tendencia)

3. **Dinámica de los cambios**:
   - La disminución de MMI desde niveles altos puede indicar la formación de una nueva tendencia
   - El aumento de MMI desde niveles bajos puede indicar la finalización de la tendencia y la transición a la consolidación

4. **Valores extremos**:
   - Valores muy bajos (MMI < 20) pueden indicar una tendencia fuerte, pero también posibles condiciones de sobrecompra/sobreventa
   - Valores muy altos (MMI > 80) indican un mercado extremadamente caótico donde es difícil aplicar estrategias

5. **Filtrado de señal**:
   - MMI se utiliza a menudo como filtro para otros indicadores:
     - Las señales del indicador de tendencia (MA, MACD) son más confiables en niveles bajos de MMI
     - Las señales del oscilador (RSI, estocástico) son más confiables en MMI alto

6. **Combinación con otros indicadores**:
   - MMI funciona bien en combinación con ADX (índice direccional promedio)
   - Un MMI bajo y un ADX alto confirman una fuerte tendencia
   - Un MMI alto y un ADX bajo confirman un mercado lateral

7. **Marcos temporales**:
   - MMI se puede utilizar en diferentes períodos de tiempo para determinar el carácter del mercado.
   - MMI a largo plazo ayuda a determinar el estado primario del mercado
   - MMI a corto plazo ayuda a elegir una estrategia adecuada para las condiciones actuales

![Gráfico del indicador MMI](../../../../images/indicator_market_meanness_index.png)

## Véase también

[ChoppinessIndex](choppiness_index.md)
[ADX](adx.md)
[VHF](vhf.md)
[BalanceOfPower](balance_of_power.md)
