# Protecção de posição

![Designer Protect positions 00](../../../../../../images/designer_protect_positions_00.png)

![Designer Protect positions 01](../../../../../../images/designer_protect_positions_01.png)

Este bloco é utilizado para proteger automaticamente negociações abertas com stop-loss e take-profit.

### Sockets de entrada

Sockets de entrada

- **Negócio próprio** - a negociação que precisa de ser protegida com stop-loss e take-profit.
- **Preço** - o preço actual (pode ser obtido a partir de uma vela, do último tick, etc.). Necessário para acompanhar o preço actual do instrumento e activar ordens de protecção.

### Sockets de saída

Sockets de saída

- **Take-profit** - uma ordem para fixar lucros.
- **Stop-loss** - uma ordem para limitar perdas.
- **Transação própria** - uma transacção criada por uma das ordens acima.

### Parâmetros

Parâmetros de Take e Stop

- **Valor** - o valor do take ou do stop.
- **Trailing** - indica se é utilizada protecção trailing.
- **Tempo limite** - o valor do tempo limite após o qual a protecção é accionada à força ao preço de mercado.
- **Ordens de mercado** - utilizar ordens de mercado (sem preço) para fechar rapidamente a posição.

![Designer Protect positions 02](../../../../../../images/designer_protect_positions_02.png)

> [!WARNING]
> As transacções de entrada NÃO PODEM ser transacções de toda a estratégia (o bloco [Strategy Trades](../common/trades_by_strategy.md)), pois isso levará ao cálculo incorrecto da posição actual: as transacções de protecção também se tornarão transacções da estratégia. O bloco **Proteção de posição** deve receber transacções do socket de saída **Transação** dos cubos [Registo de ordem](../orders/register.md) e [Modificar posição](modify.md), ou de componentes semelhantes que alterem directamente a posição.
