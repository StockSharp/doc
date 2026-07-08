# Notification

![Designer Notice 00](../../../../../../images/designer_notice_00.png)

O cubo envia uma notificação quando chegam dados ao seu socket de entrada. O valor recebido é convertido em texto através de `ToString`. Pode ligar uma [Variável](../data_sources/variable.md) para enviar texto fixo, anexar fluxos de negócios ou candles para ver os respetivos detalhes, ou usar os cubos [String format](string_format.md) e [String concat](string_concat.md) para preparar uma mensagem personalizada.

### Sockets de entrada

Sockets de entrada

- **Message** - dados a enviar. Qualquer valor é aceite e convertido numa string.

### Parâmetros

Parâmetros

- **Type** - tipo de mensagem (janela pop-up, e-mail, sms, etc.). Os tipos de notificações são descritos na secção [Notification settings](../../../../../terminal/notifications.md).
- **Telegram** - canal usado para notificações do Telegram.
- **Header** - o cabeçalho da mensagem.

## Conteúdo recomendado

[String format](string_format.md)
[String concat](string_concat.md)
[Notification settings](../../../../../terminal/notifications.md)
