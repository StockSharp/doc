## Sincronização

![Designer Sync 00](../../../../../../images/designer_sync_00.png)

O bloco Synchronization foi concebido para acumular e sincronizar dados de várias fontes (por exemplo, velas de diferentes instrumentos, diferentes timeframes, combinações de velas e transações) e, posteriormente, emiti-los quando uma determinada quantidade for acumulada. Este bloco é útil para criar índices personalizados ou arbitragem.

## Sockets de entrada

- **Entrada**: Quando uma nova fonte de dados é ligada, são criados automaticamente um socket de saída correspondente e um novo socket de entrada. O número de valores de entrada é ilimitado.

## Parâmetros

- **Intervalo**: Define o tempo após o qual os dados têm de ser atualizados ou eliminados. Se chegar um valor de entrada com uma hora que exceda o valor anterior mais o intervalo, os dados antigos são limpos e começa uma nova série de acumulação de dados.
- **Limpar itens**: Se esta opção estiver ativada, os dados são limpos após a sua acumulação para todos os sockets de entrada ligados, ou os dados são acumulados até aparecerem dados do intervalo de tempo seguinte.

## Exemplos de utilização

1. Criar um índice personalizado para várias ações, quando é necessário considerar diferentes séries temporais de várias fontes de dados.
2. Arbitragem entre diferentes mercados usando dados temporais sincronizados para identificar diferenças temporais de preços.

![Designer Sync 01](../../../../../../images/designer_sync_01.png)
