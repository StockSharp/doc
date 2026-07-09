# Notificação

![Designer Notice 00](../../../../../../images/designer_notice_00.png)

O cubo envia uma notificação quando chegam dados ao seu socket de entrada. O valor recebido é convertido em texto através de `ToString`. Pode ligar uma [Variável](../data_sources/variable.md) para enviar texto fixo, anexar fluxos de negócios ou candles para ver os respetivos detalhes, ou usar os cubos [Formatação de strings](string_format.md) e [Concatenação de strings](string_concat.md) para preparar uma mensagem personalizada.

### Sockets de entrada

Sockets de entrada

- **Mensagem** - dados a enviar. Qualquer valor é aceite e convertido numa string.

### Parâmetros

Parâmetros

- **Tipo** - tipo de mensagem (janela pop-up, e-mail, sms, etc.). Os tipos de notificações são descritos na secção [Notificações](../../../../../terminal/notifications.md).
- **Telegram** - canal usado para notificações do Telegram.
- **Cabeçalho** - o cabeçalho da mensagem.

## Conteúdo recomendado

[Formatação de strings](string_format.md)
[Concatenação de strings](string_concat.md)
[Notificações](../../../../../terminal/notifications.md)
