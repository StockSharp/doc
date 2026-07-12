# Precios de ejercicio

![Captura de Precios de ejercicio](../../../../../../images/designer_derivatives_00.png)

El cubo se usa para obtener una lista de opciones por un filtro especificado.

### Conectores de entrada

Conectores de entrada

- **Instrumento** – instrumento, el activo subyacente.

### Conectores de salida

Conectores de salida

- **Opciones** - lista de opciones por activo subyacente.

### Parámetros

Parámetros

- **Tipo de opción** – el tipo de opción puede ser opción de compra (call) u opción de venta (put).
- **Fecha de vencimiento** - fecha de vencimiento de la opción.
- **Strike (menor)** – desplazamiento a la izquierda (menor) desde el strike central. Si el precio no está establecido, se usarán todos los strikes con un valor central inferior. El desplazamiento se calcula en pasos de strike; por ejemplo, si el paso de strike es 500 u.m., un desplazamiento igual a 3 será 1.500 u.m.
- **Strike (mayor)** – desplazamiento a la derecha (mayor) desde el strike central. Si el precio no está establecido, se usarán todos los strikes con un valor central mayor. El desplazamiento se calcula en pasos de strike; por ejemplo, si el paso de strike es 500 u.m., un desplazamiento igual a 3 será 1.500 u.m.

## Contenido recomendado

[Cruce](../common/crossing.md)
