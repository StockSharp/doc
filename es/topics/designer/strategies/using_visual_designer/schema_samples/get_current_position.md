# Obtener posición actual

Para obtener el volumen necesario para girar la posición actual a la posición opuesta, puede usarse el esquema del ejemplo de estrategia SMA:

![Designer Determinación de la posición de volumen 00](../../../../../images/designer_determination_of_volume_position_00.png)

Para el cubo [Variable](../elements/data_sources/variable.md), se selecciona el tipo de dato **Instrumento**. Si el instrumento no se especifica, pero se establece la bandera **Parámetros** del grupo **Común**, se tomará de la estrategia y luego se pasará a [Posición](../elements/positions/current.md).

Para el cubo [Posición](../elements/positions/current.md), la propiedad de posición tampoco se especifica, pero se establece la bandera **Parámetros** del grupo **Común**, lo que significa que la posición se obtendrá para la cartera especificada en la configuración de la estrategia.

Después de pasar el instrumento y cambiar la posición, usando la función matemática con un argumento (abs(pos)), se calcula el valor absoluto y se da una señal al cubo de variable (2). Este cubo contiene un factor de 2, para pasar el valor almacenado por el parámetro de salida; después, usando una fórmula matemática con dos argumentos (abs(pos) \* 2), se calcula su producto. Luego, usando el cubo compuesto Operador condicional (pos \=\= 0 ? 1 : pos), se determina el valor real del volumen requerido, que puede diferir del valor de la posición actual multiplicado por 2. Por ejemplo, al iniciar la estrategia, cuando aún no se han ejecutado órdenes. En este caso, el elemento Instrucción condicional devuelve un valor predeterminado de 1. Como un parámetro de salida solo puede conectarse una vez con el parámetro de entrada de otro elemento, para pasar el mismo valor entre la fórmula y el operador condicional se añade un cubo **Combinación** adicional.

## Contenido recomendado

[Obtener nivel de precio del libro de órdenes](get_order_book_price_level.md)
