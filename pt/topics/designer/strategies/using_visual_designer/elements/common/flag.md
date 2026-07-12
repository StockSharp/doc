# Sinalizador

![Designer sinalizador](../../../../../../images/designer_flag_00.png)

O componente "Flag" é usado para gerir um sinalizador binário, que pode ser definido ou reposto com base nos sinais recebidos.

## Conectores de entrada

- **Acionador**: Aceita qualquer valor exceto `False`. Define o sinalizador ao receber o primeiro valor adequado. Se o sinalizador já estiver definido, os sinais seguintes são ignorados até que seja reposto.
- **Repor**: Aceita qualquer valor exceto `False`. Repõe o sinalizador, permitindo-lhe responder a disparadores posteriores.

## Conectores de saída

- **Sinal**: Emite um sinal quando o sinalizador é definido.
