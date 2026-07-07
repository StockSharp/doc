# Deslocação de ordem

![Designer Moving applications 00](../../../../../../images/designer_moving_applications_00.png)

Este bloco é utilizado para modificar uma ordem de um instrumento.

### Sockets de entrada

Sockets de entrada

- **Trigger** - o sinal que determina quando deslocar uma ordem.
- **Order** - a ordem que será modificada.
- **Price** - valor numérico do novo preço.
- **Volume** - valor numérico do novo volume.

### Sockets de saída

Sockets de saída

- **Order** - a ordem modificada, que pode ser utilizada para obter transacções correspondentes através do elemento **Transactions by Order** e para apresentação no gráfico através do bloco **Chart Panel**.
- **Error** - um erro ao deslocar a ordem.
- **Trade** - a negociação da ordem colocada.

Parâmetros

- **Zero Price** - um preço zero regista uma ordem de mercado.

## Ver também

[Cancelar ordem](cancel.md)
