# Bandera

![Designer bandera](../../../../../../images/designer_flag_00.png)

El componente "Flag" se usa para gestionar una bandera binaria, que puede establecerse o restablecerse según señales entrantes.

## Conectores de entrada

- **Activador**: acepta cualquier valor excepto `False`. Establece la bandera al recibir el primer valor adecuado. Si la bandera ya está establecida, las señales posteriores se ignoran hasta que se restablezca.
- **Restablecer**: acepta cualquier valor excepto `False`. Restablece la bandera, permitiéndole responder a activaciones posteriores.

## Conectores de salida

- **Señal**: emite una señal cuando la bandera está establecida.
