# Sinalizador

![Designer Flag](../../../../../../images/designer_flag_00.png)

O componente "Flag" é usado para gerir uma flag binária, que pode ser definida ou reposta com base nos sinais recebidos.

## Sockets de entrada

- **Acionador**: Aceita qualquer valor exceto `False`. Define a flag ao receber o primeiro valor adequado. Se a flag já estiver definida, os sinais seguintes são ignorados até que seja reposta.
- **Repor**: Aceita qualquer valor exceto `False`. Repõe a flag, permitindo-lhe responder a disparadores posteriores.

## Sockets de saída

- **Sinal**: Emite um sinal quando a flag é definida.
