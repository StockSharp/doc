# BOP

**Balance de poder (BOP)** es un indicador diseñado para medir la fuerza de los alcistas (compradores) en relación con los bajistas (vendedores) mediante la evaluación de la capacidad de los alcistas para elevar el precio desde el mínimo hasta el máximo.

Para utilizar el indicador, debe utilizar la clase [BalanceOfPower](xref:StockSharp.Algo.Indicators.BalanceOfPower).

## Descripción

El indicador Balance de poder (BOP) muestra el equilibrio de fuerzas entre compradores y vendedores en el mercado. Se basa en el supuesto de que en una tendencia, los compradores (alcistas) o vendedores (bajistas) pueden controlar el precio durante toda la sesión. Al comparar la diferencia entre los precios de cierre y apertura con el rango de precios completo (alto-mínimo), el indicador permite evaluar quién domina actualmente el mercado.

BOP ayuda a los operadores:
- Determinar la dirección y la fuerza de la tendencia actual.
- Identificar posibles puntos de reversión
- Detectar divergencias entre el precio y el indicador.
- Determinar los niveles de sobrecompra y sobreventa.

## Cálculo

La fórmula para calcular el indicador Balance de poder (BOP) es bastante simple:

```
BOP = (Close - Open) / (High - Low)
```

donde:
- Close - precio de cierre
- Open - precio de apertura
- High - precio más alto para el período
- Low - precio más bajo para el período

Si (High - Low) es cero, BOP se establece en cero para evitar la división por cero.

BOP a menudo se suaviza adicionalmente mediante una media móvil para reducir la volatilidad y mejorar la legibilidad de la señal.

## Interpretación

- **Valores positivos de BOP** (por encima de cero) indica que los compradores (alcistas) están controlando el mercado, lo que puede indicar una tendencia alcista.
- **Valores BOP negativos** (por debajo de cero) indica que los vendedores (bajistas) están controlando el mercado, lo que puede indicar una tendencia a la baja.
- **Cruzando la línea cero** puede considerarse una señal de un posible cambio de dirección de tendencia.
- **Valores extremos** (fuertemente positivo o fuertemente negativo) puede indicar condiciones de sobrecompra o sobreventa del mercado.
- **Divergencias** entre BOP y el precio puede indicar un posible cambio de tendencia:
  - Si el precio sube y BOP baja, esto puede ser una advertencia de que la tendencia alcista se está debilitando.
  - Si el precio cae y BOP aumenta, esto puede indicar un posible fin de la tendencia a la baja.

![indicator_balance_of_power](../../../../images/indicator_balance_of_power.png)

## Véase también

[BalanceOfMarketPower](balance_of_market_power.md)
[ForceIndex](force_index.md)
[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)