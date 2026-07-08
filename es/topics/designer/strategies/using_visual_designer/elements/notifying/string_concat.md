# Concatenación de cadenas

![Designer String Concat 00](../../../../../../images/designer_string_concat_00.png)

El cubo concatena varios valores entrantes en una sola cadena de texto según
una plantilla con marcadores de posición entre llaves. Cada nombre de marcador añade un socket de entrada
con el mismo nombre. Puede referirse a propiedades anidadas mediante puntos y
especificar formato después de dos puntos.

### Sockets de entrada

Sockets de entrada

- Se crean dinámicamente a partir de nombres de marcadores. Cada socket acepta datos de cualquier tipo.

### Sockets de salida

Sockets de salida

- **Text** – cadena concatenada y formateada.

### Parámetros

Parámetros

- **Template** – plantilla de concatenación y formateo de cadenas. Editar la plantilla
  actualiza la lista de sockets de entrada.

### Ejemplos

- La plantilla `Price: {price:0.00}, Qty: {qty}` con `price = 10.5` y `qty = 2`
  produce `Price: 10.50, Qty: 2`.
- La plantilla `{time:HH:mm:ss} - {trade.Price}` con sockets `time` y `trade`
  (`trade.Price = 100`) produce `09:15:00 - 100`.
- La plantilla `{side} {volume} @ {trade.Price}` con sockets `side = Buy`,
  `volume = 1`, `trade.Price = 100` produce `Buy 1 @ 100`.

## Contenido recomendado

[Formato de cadena](string_format.md)
[Notificación](notification.md)

