# Registro de órdenes

Para importar el registro de órdenes, seleccione **Importar \=\> Registro de órdenes** en el menú principal de la aplicación.

![Captura de Registro de órdenes 1](../../../images/hydra_import_orderlog.png)

## Proceso de importación.

1. **Configuración de importación**.

   Consulte la importación de [Velas](candles.md).
2. Configure los parámetros de importación para los campos de [S#](../../api.md).

   Consulte la importación de [Velas](candles.md).

   **Veamos un ejemplo de importación del registro de órdenes desde un archivo CSV:**
   - El archivo desde el que desea importar datos tiene la siguiente plantilla:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{OrderId};{OrderPrice};{OrderVolume};{Side};{OrderState};{TimeInForce};{TradeId};{TradePrice}

     ```

     Aquí los valores de {SecurityId.SecurityCode} y {SecurityId.BoardCode} corresponden a los valores de **Instrumento** y **Mercado**, respectivamente. Por lo tanto, en el campo **Orden de campos** asignamos los valores 0 y 1, respectivamente.
   - Para los campos {ServerTime:default:yyyyMMdd} y {ServerTime:default:HH:mm:ss.ffffff}, seleccione los campos **Fecha** y **Hora** en la ventana **campo S#**, respectivamente. Asigne los valores 2 y 3.
   - Para el campo {OrderId}, seleccione el campo **ID** en la ventana **campo S#**: ID de la orden. Asígnele el valor 4.
   - Para el campo {OrderPrice}, seleccione el campo **Precio** en la ventana **campo S#**: precio de la orden. Asígnele el valor 5.
   - Para el campo {OrderVolume}, seleccione el campo **Volumen** en la ventana **campo S#**: volumen de la orden. Asígnele el valor 6.
   - Para el campo {Side}, seleccione el campo **Dirección** en la ventana **campo S#**: dirección de la orden (compra o venta). Asígnele el valor 7.
   - Para el campo {OrderState}, seleccione el campo **Acción** en la ventana **campo S#**: estado de la orden (activa, inactiva o con error). Asígnele el valor 8.
   - Para el campo {TimeInForce}, seleccione **Vigencia** en la ventana **campo S#**: una condición de ejecución de una orden limitada. Asígnele el valor 9.
   - Para el campo {TradeId}, seleccione el campo **ID (operación)** en la ventana **campo S#**: identificador de la operación. Asígnele el valor 10.
   - Para el campo {TradePrice}, seleccione el campo **Precio (operación)** en la ventana **campo S#**: precio de la operación. Asígnele el valor 11.
   - La ventana de configuración de campos tendrá este aspecto:![Captura de Registro de órdenes 2](../../../images/hydra_import_prop_orderlog.png)

   El usuario puede configurar una gran cantidad de propiedades para los datos descargados. Basándose en la plantilla del archivo importado, debe especificar la propiedad y asignarle el número requerido en la secuencia.
3. Para previsualizar los datos, haga clic en el botón **Vista previa**.![Captura de Registro de órdenes 3](../../../images/hydra_import_preview_orderlog.png)
4. Haga clic en el botón **Importar**.
