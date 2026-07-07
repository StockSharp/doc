# Registo de ordem

![Designer Position opening 00](../../../../../../images/designer_position_opening_00.png)

O componente "Order Registration" é utilizado para colocar ordens de negociação para um instrumento seleccionado.

## Sockets de entrada

- **Instrument** - O instrumento seleccionado para a ordem.
- **Price** - Especifica o preço de uma ordem limitada.
- **Trigger** - Sinal de activação da ordem; aceita qualquer valor excepto `False`.
- **Volume** - A quantidade de instrumentos para a ordem.
- **Portfolio** - A carteira no âmbito da qual a ordem será colocada.

## Sockets de saída

- **Order** - Informação sobre a ordem colocada.
- **Error** - Informação sobre qualquer erro durante o registo da ordem.
- **Transaction** - Informação sobre a transacção efectuada na ordem.
- **Cancellation** - Sinal de que a ordem foi cancelada.
- **Executed** - Sinal de que a ordem foi totalmente executada.
- **Completed** - Sinal que combina eventos de erro, cancelamento ou execução total da ordem.

## Parâmetros

- **Direction** - Determina se a ordem é de compra ou de venda.
- **Market Order** - Indica se a ordem é uma ordem de mercado.
- **Zero Price** - Se o preço for definido como zero, a ordem é registada como ordem de mercado.
- **Lifetime** - A duração durante a qual uma ordem limitada permanece activa.

## Configuração de ordem condicional

**Conditional Order** - Uma ordem com condições adicionais que determinam o momento de colocação no sistema de negociação com base na situação actual do mercado.

![Designer Conditional Application](../../../../../../images/designer_conditional_application.png)

- **Connection** - A ligação onde a ordem será colocada.
- **Stop Order Type** - O tipo de ordem stop.
- **Result** - O resultado da ordem stop executada.
- **Instrument Identifier** - O identificador do instrumento para ordens stop com condições relacionadas com outro instrumento.
- **Stop Price Condition** - A condição do preço stop. Utilizada para ordens como "Stop price for another instrument."
- **Stop Price** - O preço stop que define a condição de activação da ordem stop.
- **Stop-Limit Price** - Semelhante a Stop Price, mas utilizado apenas para ordens do tipo "Take-profit and stop-limit".
- **Stop-Limit at Market Price** - Indica se a ordem "Stop-Limit" é executada ao preço de mercado.
- **Condition Check Interval** - O intervalo de tempo para verificar as condições da ordem apenas dentro do período especificado (se for nulo, não há verificações). Utilizado para os tipos "Take-profit and stop-limit" e "Take-profit and stop-limit by order".
- **Conditional Order Execution Identifier** - O identificador da ordem condicional baseada na execução.
- **Direction of Conditional Order by Execution** - A direcção da ordem condicional baseada na execução.
- **Activation on Partial Execution** - A execução parcial da ordem é considerada. Uma ordem "on-execution" será activada após a execução parcial da ordem de condição.
- **Executed Volume** - Usa o volume executado da ordem como quantidade para colocar a ordem stop. A quantidade de títulos numa ordem "on-execution" é tomada como o volume executado da ordem de condição.
- **Price of Linked Order** - O preço da ordem limitada ligada.
- **Withdrawal on Partial Execution** - Indica a retirada da ordem stop após a execução parcial da ordem limitada ligada.
- **Offset from Maximum** - O valor de desvio face ao preço máximo (mínimo) da última transacção.
- **Protective Spread** - O tamanho do spread de protecção.
- **Take-Profit at Market Price** - Indica se a ordem "Take-Profit" é executada ao preço de mercado.

## Nota

Trabalhar com ordens é um método de baixo nível para gerir posições. Para uma gestão de nível superior, recomenda-se utilizar o componente "Modify Position", descrito em [Modificar posição](../positions/modify.md).

## Ver também

[Modificar posição](../positions/modify.md)
