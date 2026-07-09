# Deslocação de ordem

![Designer Moving applications 00](../../../../../../images/designer_moving_applications_00.png)

Este bloco é utilizado para modificar uma ordem de um instrumento.

### Sockets de entrada

Sockets de entrada

- **Acionador** - o sinal que determina quando deslocar uma ordem.
- **Ordem** - a ordem que será modificada.
- **Preço** - valor numérico do novo preço.
- **Volume** - valor numérico do novo volume.

### Sockets de saída

Sockets de saída

- **Ordem** - a ordem modificada, que pode ser utilizada para obter transacções correspondentes através do elemento **Transações por ordem** e para apresentação no gráfico através do bloco **Painel do gráfico**.
- **Erro** - um erro ao deslocar a ordem.
- **Negócio** - a negociação da ordem colocada.

Parâmetros

- **Preço zero** - um preço zero regista uma ordem de mercado.

## Ver também

[Cancelar ordem](cancel.md)
