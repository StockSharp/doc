# Deslocação de ordem

![Designer Mover aplicativos 00](../../../../../../images/designer_moving_applications_00.png)

Este bloco é utilizado para modificar uma ordem de um instrumento.

### Conectores de entrada

Conectores de entrada

- **Acionador** - o sinal que determina quando deslocar uma ordem.
- **Ordem** - a ordem que será modificada.
- **Preço** - valor numérico do novo preço.
- **Volume** - valor numérico do novo volume.

### Conectores de saída

Conectores de saída

- **Ordem** - a ordem modificada, que pode ser utilizada para obter transacções correspondentes através do elemento **Transações por ordem** e para apresentação no gráfico através do bloco **Painel do gráfico**.
- **Erro** - um erro ao deslocar a ordem.
- **Negócio** - a negociação da ordem colocada.

Parâmetros

- **Preço zero** - um preço zero regista uma ordem de mercado.

## Ver também

[Cancelar ordem](cancel.md)
