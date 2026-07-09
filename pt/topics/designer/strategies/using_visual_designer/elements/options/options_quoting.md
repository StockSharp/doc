# Cotação de opções

![Designer Quoting 00](../../../../../../images/designer_quoting_00.png)

O cubo é usado para cotar opções de acordo com os parâmetros especificados.

### Sockets de entrada

Sockets de entrada

- **Model** - o modelo de cálculo (por exemplo, Black-Scholes).
- **Volume** - o volume de cotação.

### Sockets de saída

Sockets de saída

- **Order** - a ordem registada que pode ser usada para obter negócios sobre ela usando o elemento **Trades** pela ordem e apresentá-la no gráfico usando o cubo **Chart panel**.

### Parâmetros

Parâmetros

- **Quoting** - o parâmetro pelo qual a cotação será executada. Pode assumir os valores **Volatility** (o volume de cotação seguirá os limites de volatilidade especificados) ou **Theoretical price** (o volume de cotação seguirá os limites especificados do preço teórico).
- **Direction** - a direção da cotação pode assumir os valores Purchase e Sell.
- **Minimum** - o valor mínimo da volatilidade ou do preço teórico.
- **Maximum** - o valor máximo da volatilidade ou do preço teórico.

## Conteúdo recomendado

[Preços de exercício](strikes.md)
