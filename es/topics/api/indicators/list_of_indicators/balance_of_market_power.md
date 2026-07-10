# BMP

**Balance of Market Power (BMP)** es un indicador que mide la fuerza de los compradores en relación con los vendedores, basándose en un análisis de los movimientos de precios y los volúmenes de negociación.

Para utilizar el indicador, debe utilizar la clase [BalanceOfMarketPower](xref:StockSharp.Algo.Indicators.BalanceOfMarketPower).

## Descripción

El indicador Balance of Market Power está diseñado para evaluar la distribución actual de fuerzas entre compradores y vendedores en el mercado. Analiza cuánto se desvía el precio de cierre de su rango (máximo-mínimo) y lo correlaciona con el volumen de operaciones.

BMP ayuda a los operadores:
- Determinar el lado dominante del mercado (compradores o vendedores)
- Identificar posibles cambios de tendencia
- Detectar divergencias entre el precio y el indicador.
- Encuentra niveles de sobrecompra y sobreventa

## Parámetros

El indicador tiene los siguientes parámetros:
- **Length** - período de suavizado (valor predeterminado: 14)

## Cálculo

El cálculo de BMP se produce en dos etapas:

1. Calculando BMP para cada vela individual:
   ```
   Raw BMP = ((Close Price - Open Price) / (High - Low)) * Volume
   ```
   Si (High - Low) es cero, el BMP sin formato se establece en cero.

2. Suavizado de BMP usando una media móvil simple (SMA):
   ```
   BMP = SMA(Raw BMP, Length)
   ```

donde:
- Close Price - precio de cierre de la vela actual
- Open Price - precio de apertura de la vela actual
- High - precio más alto de la vela actual
- Low - precio más bajo de la vela actual
- Volume - volumen de operaciones para el período de vela actual
- Length - período de suavizado seleccionado

## Interpretación

- **Valores positivos de BMP** indica dominio de los compradores (alcistas) en el mercado
- **Valores BMP negativos** indica dominio de los vendedores (osos) en el mercado
- **Cruzando la línea cero** puede considerarse una señal de cambio de tendencia
- **Valores extremos** (por encima o por debajo de ciertos niveles) puede indicar condiciones de sobrecompra o sobreventa del mercado.
- **Divergencias** entre BMP y el precio puede indicar un posible cambio de tendencia

![indicator_balance_of_market_power](../../../../images/indicator_balance_of_market_power.png)

## Véase también

[BalanceOfPower](balance_of_power.md)
[ForceIndex](force_index.md)
[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)
