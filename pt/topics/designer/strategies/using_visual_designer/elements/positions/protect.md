# Proteção de posição

![Designer proteção de posições 00](../../../../../../images/designer_protect_positions_00.png)

![Designer proteção de posições 01](../../../../../../images/designer_protect_positions_01.png)

Este bloco é utilizado para proteger automaticamente negócios abertos com stop-loss e take-profit.

### Conectores de entrada

Conectores de entrada

- **Negócio próprio** - o negócio que precisa de ser protegido com stop-loss e take-profit.
- **Preço** - o preço atual (pode ser obtido a partir de uma vela, do último tick, etc.). Necessário para acompanhar o preço atual do instrumento e ativar ordens de proteção.

### Conectores de saída

Conectores de saída

- **Realização de lucro** - uma ordem para fixar lucros.
- **Limitação de perdas** - uma ordem para limitar perdas.
- **Transação própria** - uma transação criada por uma das ordens acima.

### Parâmetros

Parâmetros de Take e Stop

- **Valor** - o valor do take ou do stop.
- **Dinâmica** - indica se é utilizada proteção trailing.
- **Tempo limite** - o valor do tempo limite após o qual a proteção é acionada à força ao preço de mercado.
- **Ordens de mercado** - utilizar ordens de mercado (sem preço) para fechar rapidamente a posição.

![Designer proteção de posições 02](../../../../../../images/designer_protect_positions_02.png)

> [!WARNING]
> As transações de entrada NÃO PODEM ser transações de toda a estratégia (o bloco [Negócios por estratégia](../common/trades_by_strategy.md)), pois isso levará ao cálculo incorreto da posição atual: as transações de proteção também se tornarão transações da estratégia. O bloco **Proteção de posição** deve receber transações do conector de saída **Transação** dos cubos [Registo de ordem](../orders/register.md) e [Modificar posição](modify.md), ou de componentes semelhantes que alterem diretamente a posição.
