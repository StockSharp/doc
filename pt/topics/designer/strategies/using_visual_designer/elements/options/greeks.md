# Greeks

![Designer Greek 00](../../../../../../images/designer_greek_00.png)

Este bloco é usado para calcular os principais "Gregos": Delta, Gamma, Vega, Theta, Rho no momento atual.

### Sockets de entrada

Sockets de entrada

- **Model** - o modelo de cálculo (por exemplo, Black-Scholes).
- **Price of the Underlying Asset** - o preço do ativo subjacente.
- **Maximum Deviation** - o desvio máximo.

### Sockets de saída

Sockets de saída

- **Result** - o resultado do cálculo dos principais "Gregos": Delta, Gamma, Vega, Theta, Rho no momento atual.

### Parâmetros

Parâmetros

- **Value** - pode assumir o tipo de "Grego" Delta, Gamma, Vega, Theta, Rho e determina que valor será emitido pelo bloco.

## Ver também

[Hedging](black_scholes.md)
