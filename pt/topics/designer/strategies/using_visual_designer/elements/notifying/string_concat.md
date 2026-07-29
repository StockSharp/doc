# Concatenação de strings

![Captura de ecrã de Concatenação de strings](../../../../../../images/designer_string_concat_00.png)

O cubo concatena vários valores recebidos numa única string de texto de acordo com
um modelo com marcadores de posição entre chavetas. Cada nome de marcador de posição adiciona um conector
de entrada com o mesmo nome. Pode referenciar propriedades aninhadas através de pontos e
especificar a formatação depois de dois pontos.

### Conectores de entrada

Conectores de entrada

- Criados dinamicamente a partir dos nomes dos marcadores de posição. Cada conector aceita dados de qualquer tipo.

### Conectores de saída

Conectores de saída

- **Texto** - a string concatenada e formatada.

### Parâmetros

Parâmetros

- **Modelo** - modelo de concatenação e formatação de strings. A edição do modelo
  atualiza a lista de conectores de entrada.

### Exemplos

- O modelo `Price: {price:0.00}, Qty: {qty}` com `price = 10.5` e `qty = 2`
  produz `Price: 10.50, Qty: 2`.
- O modelo `{time:HH:mm:ss} - {trade.Price}` com os conectores `time` e `trade`
  (`trade.Price = 100`) produz `09:15:00 - 100`.
- O modelo `{side} {volume} @ {trade.Price}` com os conectores `side = Buy`,
  `volume = 1`, `trade.Price = 100` produz `Buy 1 @ 100`.

## Conteúdo recomendado

[Formatação de strings](string_format.md)
[Notificação](notification.md)

