# Precios de ejercicio

![Designer Derivatives 00](../../../../../../images/designer_derivatives_00.png)

El cubo se usa para obtener una lista de opciones por un filtro especificado.

### Sockets de entrada

Sockets de entrada

- **Instrumento** – instrumento, el activo subyacente.

### Sockets de salida

Sockets de salida

- **Options** - lista de opciones por activo subyacente.

### Parámetros

Parámetros

- **Option type** – el tipo de opción puede ser Call option o Put option.
- **Expiry date** - fecha de vencimiento de la opción.
- **Strike (less)** – desplazamiento a la izquierda (menor) desde el strike central. Si el precio no está establecido, se usarán todos los strikes con un valor central inferior. El desplazamiento se calcula en pasos de strike; por ejemplo, si el paso de strike es 500 u.m., un desplazamiento igual a 3 será 1.500 u.m.
- **Strike (more)** – desplazamiento a la derecha (mayor) desde el strike central. Si el precio no está establecido, se usarán todos los strikes con un valor central mayor. El desplazamiento se calcula en pasos de strike; por ejemplo, si el paso de strike es 500 u.m., un desplazamiento igual a 3 será 1.500 u.m.

## Contenido recomendado

[Cruce](../common/crossing.md)
