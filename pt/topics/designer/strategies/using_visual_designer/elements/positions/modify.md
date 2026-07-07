# Modificar posição

![Designer position modify 00](../../../../../../images/designer_position_modify_00.png)

O componente "Modify Position" é utilizado para alterar uma posição de negociação com base em condições especificadas.

## Sockets de entrada

- **Security**: O instrumento cuja posição será modificada.
- **Trigger**: Sinal para activar a modificação da posição.
- **Portfolio**: A carteira no âmbito da qual a operação ocorre.
- **Volume** (opcional): O volume para as operações "Increase" e "Decrease". Não é utilizado para "Reverse" e "Close Position".
- **Last Price** e **Last Volume**: Para os algoritmos "VWAP" e "Iceberg", são necessários dados sobre o último preço e o último volume da transacção.
- **Cancel**: Sinal para cancelar a configuração da posição, por exemplo, devido a um tempo limite.

## Sockets de saída

- **Order**: Informação sobre a ordem colocada.
- **Transaction**: Informação sobre a transacção realizada na ordem.
- **Balance**: Este socket transmite informação sobre a parte da posição que não foi realizada no fim da operação de modificação da posição. O valor devolvido pelo socket indica o resultado da operação:
  - `0` significa que o componente concluiu com êxito a operação de modificação da posição e que todas as acções planeadas foram executadas.
  - `-1` indica que o componente não iniciou a modificação da posição devido a uma incompatibilidade entre a posição actual e a condição especificada (por exemplo, se a posição actual for diferente de zero e a condição for "OpenPosition").
  - Qualquer valor superior a `0` assinala que o processo de modificação da posição foi interrompido antes da conclusão. Isto pode acontecer devido a cancelamento através da lógica do esquema ou a um erro durante o registo da ordem.

## Parâmetros

- **Condition**: Condições de modificação da posição:
  - `None`: Não executa acções.
  - `OpenPosition`: Abre uma posição na direcção especificada.
  - `ClosePosition`: Fecha a posição actual.
  - `Decrease`: Reduz o tamanho da posição actual.
  - `Increase`: Aumenta o tamanho da posição actual.
  - `Reverse`: Fecha a posição actual e abre uma nova na direcção oposta.
- **Direction**: Especifica a direcção para "OpenPosition" e "None", e serve como filtro opcional para outras condições.
- **Algorithm**: As opções incluem "Market Order", "VWAP", "Iceberg".
- **Part**: A fracção do volume total que será dividida em segmentos mais pequenos ao utilizar algoritmos como "VWAP" ou "Iceberg".

Se o componente receber um trigger quando já iniciou a alteração do volume, ignora o novo trigger. Se as condições de modificação forem incompatíveis com o estado actual da posição (por exemplo, tentar "OpenPosition" quando já existe uma posição aberta), o componente devolve imediatamente `-1` através do socket de saída **Balance**, indicando que a operação não é necessária e não foi iniciada.

## Nota

Para gestão de ordens de baixo nível, pode ser utilizado o componente [Registo de ordem](../orders/register.md). Para uma gestão de posições de nível superior, recomenda-se este componente "Modify Position".

## Ver também

- [Registo de ordem](../orders/register.md)
