# Formatação de strings

![Designer String Format 00](../../../../../../images/designer_string_format_00.png)

O cubo converte um valor recebido de qualquer tipo numa string de texto. A
conversão é executada de acordo com um modelo com placeholders entre chavetas.
Cada placeholder refere-se ao valor inteiro (`{0}`) ou a uma das suas
propriedades (`{Price}`, `{Trade.Price}`, etc.). Pode especificar um formato depois de
dois pontos para controlar como números, datas ou outros objetos aparecem no texto.

### Sockets de entrada

Sockets de entrada

- **Input** - o valor a formatar. O socket aceita dados de qualquer tipo.

### Sockets de saída

Sockets de saída

- **Text** - o resultado da aplicação do modelo ao valor recebido.

### Parâmetros

Parâmetros

- **Template** - modelo de formatação de strings aplicado ao valor recebido. O
  modelo predefinido é `{0}`, o que significa que o valor é inserido sem formatação
  adicional. Os placeholders podem conter nomes de propriedades e strings de formato, por
  exemplo `Price: {0:0.00}` ou `{Price:0.00}`.

### Exemplos

- O modelo `Price: {0:0.00}` com entrada `10.5` produz `Price: 10.50`.
- O modelo `{Price} - {Volume}` com o objeto de negócio de entrada `{ Price = 100,
  Volume = 2 }` produz `100 - 2`.
- O modelo `{Trade.Price} - {Trade.Volume}` com o objeto de entrada
  `{ Trade = { Price = 100, Volume = 2 } }` produz `100 - 2`.
- O modelo `Time: {Time:HH:mm:ss}` com um objeto de entrada com `Time =
  2024-05-01T09:15:00` produz `Time: 09:15:00`.

## Conteúdo recomendado

[Concatenação de strings](string_concat.md)
[Notificação](notification.md)
