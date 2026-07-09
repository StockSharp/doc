# Cancelamento de ordem

![Designer Cancellations 00](../../../../../../images/designer_cancellations_00.png)

Este bloco é usado para cancelar uma ordem de um instrumento.

### Sockets de entrada

Sockets de entrada

- **Acionador** - o evento que aciona o cancelamento da ordem.
- **Ordem** - o sinal usado para determinar o momento em que uma ordem tem de ser cancelada.

### Sockets de saída

Sockets de saída

- **Ordem** - a ordem cancelada, que pode ser usada para obter transações para ela usando o elemento **Transações**, bem como para a apresentar no gráfico usando o bloco **Painel do gráfico**.
- **Erro** - um erro no cancelamento da ordem (por exemplo, a ordem já foi executada ou cancelada anteriormente).

## Ver também

[Cancelamento em massa de ordens](mass_cancel.md)
