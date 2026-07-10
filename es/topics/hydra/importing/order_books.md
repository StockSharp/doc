# Libros de órdenes

Para importar libros de órdenes, seleccione **Importar \=\> Libros de órdenes** en el menú principal de la aplicación.

![hydra import depths](../../../images/hydra_import_depths.png)

## Proceso de importación.

1. **Configuración de importación**.

   Consulte la importación de [Velas](candles.md).
2. Configure los parámetros de importación para los campos de [S#](../../api.md).

   Consulte la importación de [Velas](candles.md).

   **Veamos un ejemplo de importación de un libro de órdenes desde un archivo CSV:**
   - El archivo desde el que desea importar datos tiene la siguiente plantilla:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{Quote.Price};{Quote.Volume};{Side}

     ```

     Aquí los valores de {SecurityId.SecurityCode} y {SecurityId.BoardCode} corresponden a los valores de **Instrumento** y **Mercado**, respectivamente. Por lo tanto, en el campo **Orden de campos** asignamos los valores 0 y 1, respectivamente.
   - Para los campos {ServerTime:default:yyyyMMdd} y {ServerTime:default:HH:mm:ss.ffffff}, seleccione los campos **Fecha** y **Hora** en la ventana **campo S#**, respectivamente. Asigne los valores 2 y 3.
   - Para el campo {Quote.Price}, seleccione el campo **Precio** en la ventana **campo S#**: precio de la cotización. Asígnele el valor 4.
   - Para el campo {Quote.Volume}, seleccione el campo **Volumen** en la ventana **campo S#**: volumen de la cotización. Asígnele el valor 5.
   - Para el campo {Side}, seleccione el campo **Dirección** en la ventana **campo S#**: dirección de la operación (compra o venta). Asígnele el valor 6.
   - La ventana de configuración de campos tendrá este aspecto:![hydra import propiedades de profundidad](../../../images/hydra_import_prop_depth.png)

   El usuario puede configurar una gran cantidad de propiedades para los datos descargados. Basándose en la plantilla del archivo importado, debe especificar la propiedad y asignarle el número requerido en la secuencia.
3. Para previsualizar los datos, haga clic en el botón **Vista previa**.![hydra import vista previa de profundidad](../../../images/hydra_import_preview_depth.png)
4. Haga clic en el botón **Importar**.
