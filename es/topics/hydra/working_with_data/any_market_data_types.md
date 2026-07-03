# Cualquier tipo de dato de mercado

[Hydra](../../hydra.md) permite usar tipos de datos alternativos para obtener distintos tipos de datos de mercado.

Esto es necesario si la fuente no permite descargar los datos de mercado requeridos. Así, por ejemplo, se pueden usar varios tipos de datos de mercado a la vez para construir el **Order book**.

¡IMPORTANTE\! **Order book** puede construirse a partir de **Order Log** o **Level 1**, siempre que estos tipos de datos contengan los mejores precios.

Recuerde que los valores de **Level 1** se pueden descargar desde cualquier fuente que proporcione datos de mercado en tiempo real. **Level 1** también se puede recibir mediante [conversión](../tasks/converter.md) desde **Order book**.

Para construirlo, debe:

1. Seleccionar el período y el instrumento para los que desea obtener datos de mercado.![hydra LEVEL 1 build depth data](../../../images/hydra_level1_build_depth_data.png)
2. Seleccionar el campo **Build from** y elegir el tipo de datos requerido.![hydra type build data](../../../images/hydra_type_build_data.png)

   ¡IMPORTANTE\! Si se seleccionan **Order Book, Order Log, Level 1** como fuente para construir una vela, aparece una selección de parámetros adicionales.![hydra ext proper build data](../../../images/hydra_ext_proper_build_data.png)
3. Después de establecer los parámetros, haga clic en el botón ![hydra candles](../../../images/hydra_candles.png).![hydra LEVEL 1 build depth data result](../../../images/hydra_level1_build_depth_data_result.png)

Para construir **Candles**, también está disponible la opción de construir velas de un Time Frame mayor a partir de velas de un Time Frame menor.

Por ejemplo, si hay velas con un Time Frame de 1 minuto, puede construir velas con un Time Frame de 5 minutos a partir de ellas seleccionando el tipo correspondiente en la línea **Build from**.

**Ver [tutorial en video](../videos/building_order_books.md)**
