# Cobertura

![Designer Hedging 00](../../../../../../images/designer_hedging_00.png)

O cubo é usado para cobrir posições sobre opções.

### Sockets de entrada

Sockets de entrada

- **Model** - o modelo de cálculo (por exemplo, Black-Scholes).
- **Instrument** - o instrumento, o ativo subjacente.
- **Volume** - o valor numérico do volume.
- **Position by underlying asset** - a posição pelo ativo subjacente.
- **Flag** - o sinal (flag) que inicia o processo de cobertura.

### Sockets de saída

Sockets de saída

- **Order** - a ordem registada que pode ser usada para obter negócios sobre ela usando o elemento Trades pela ordem e apresentá-la no gráfico usando o cubo Chart panel

### Parâmetros

Parâmetros

- **Hedging type** - o tipo de cobertura, que pode assumir os valores Delta, Gamma, Vega, Theta ou Rho.

## Conteúdo recomendado

[Cotação de opções](options_quoting.md)
