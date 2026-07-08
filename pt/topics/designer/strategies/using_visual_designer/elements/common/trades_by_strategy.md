# Negócios por estratégia

![Designer The transaction strategy 00](../../../../../../images/designer_trades_strategy_00.png)

O cubo é usado para obter todos os negócios da estratégia. 

## Sockets de entrada

- **Instrument** - o instrumento para o qual precisa de obter negócios. Se o instrumento não for passado, então os negócios de todos os instrumentos da estratégia são transferidos para a saída.

## Sockets de saída

- **Trades** - negócios provenientes do instrumento passado. Podem ser usados tanto para apresentação no gráfico usando o elemento **Chart panel**, como para proteção de posição usando o elemento **Position protection**.

