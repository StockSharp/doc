# Order Cancellation

![Designer Cancellations 00](../../../../../../images/designer_cancellations_00.png)

Este bloco é usado para cancelar uma ordem de um instrumento.

### Sockets de entrada

Sockets de entrada

- **Trigger** - o evento que aciona o cancelamento da ordem.
- **Order** - o sinal usado para determinar o momento em que uma ordem tem de ser cancelada.

### Sockets de saída

Sockets de saída

- **Order** - a ordem cancelada, que pode ser usada para obter transações para ela usando o elemento **Transactions**, bem como para a apresentar no gráfico usando o bloco **Chart Panel**.
- **Error** - um erro no cancelamento da ordem (por exemplo, a ordem já foi executada ou cancelada anteriormente).

## Ver também

[Mass Orders Cancel](mass_cancel.md)
