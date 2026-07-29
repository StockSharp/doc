# Modificar posição

![Designer Modificar posição 00](../../../../../../images/designer_position_modify_00.png)

O componente "Modificar posição" é utilizado para alterar uma posição de negociação com base em condições especificadas.

## Conectores de entrada

- **Instrumento**: O instrumento cuja posição será modificada.
- **Acionador**: Sinal para ativar a modificação da posição.
- **Carteira**: A carteira no âmbito da qual a operação ocorre.
- **Volume** (opcional): O volume para as operações "Aumentar" e "Reduzir". Não é utilizado para "Inverter" nem "Fechar posição".
- **Último preço** e **Último volume**: Para os algoritmos "VWAP" e "Iceberg", são necessários dados sobre o último preço e o último volume da transação.
- **Cancelar**: Sinal para cancelar a configuração da posição, por exemplo, devido a um tempo limite.

## Conectores de saída

- **Ordem**: Informação sobre a ordem colocada.
- **Transação**: Informação sobre a transação realizada na ordem.
- **Saldo**: Este conector transmite informação sobre a parte da posição que não foi realizada no fim da operação de modificação da posição. O valor devolvido pelo conector indica o resultado da operação:
  - `0` significa que o componente concluiu com êxito a operação de modificação da posição e que todas as ações planeadas foram executadas.
  - `-1` indica que o componente não iniciou a modificação da posição devido a uma incompatibilidade entre a posição atual e a condição especificada (por exemplo, se a posição atual for diferente de zero e a condição for "Abrir posição").
  - Qualquer valor superior a `0` assinala que o processo de modificação da posição foi interrompido antes da conclusão. Isto pode acontecer devido a cancelamento através da lógica do esquema ou a um erro durante o registo da ordem.

## Parâmetros

- **Condição**: Condições de modificação da posição:
  - `Nenhuma`: Não executa ações.
  - `Abrir posição`: Abre uma posição na direção especificada.
  - `Fechar posição`: Fecha a posição atual.
  - `Reduzir`: Reduz o tamanho da posição atual.
  - `Aumentar`: Aumenta o tamanho da posição atual.
  - `Inverter`: Fecha a posição atual e abre uma nova na direção oposta.
- **Direção**: Especifica a direção para "Abrir posição" e "Nenhuma", e serve como filtro opcional para outras condições.
- **Algoritmo**: As opções incluem "Ordem de mercado", "VWAP", "Iceberg".
- **Parte**: A fracção do volume total que será dividida em segmentos mais pequenos ao utilizar algoritmos como "VWAP" ou "Iceberg".

Se o componente receber um sinal de acionamento quando já iniciou a alteração do volume, ignora o novo sinal de acionamento. Se as condições de modificação forem incompatíveis com o estado atual da posição (por exemplo, tentar "Abrir posição" quando já existe uma posição aberta), o componente devolve imediatamente `-1` através do conector de saída **Saldo**, indicando que a operação não é necessária e não foi iniciada.

## Nota

Para gestão de ordens de baixo nível, pode ser utilizado o componente [Registo de ordem](../orders/register.md). Para uma gestão de posições de nível superior, recomenda-se este componente "Modificar posição".

## Ver também

- [Registo de ordem](../orders/register.md)
