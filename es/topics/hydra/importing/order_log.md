# Registro de órdenes

Para importar el registro de órdenes, seleccione **Import \=\> Order log** en el menú principal de la aplicación.

![hydra import orderlog](../../../images/hydra_import_orderlog.png)

## Proceso de importación.

1. **Configuración de importación**.

   Consulte la importación de [Velas](candles.md).
2. Configure los parámetros de importación para los campos de [S#](../../api.md).

   Consulte la importación de [Velas](candles.md).

   **Veamos un ejemplo de importación de Order Log desde un archivo CSV:**
   - El archivo desde el que desea importar datos tiene la siguiente plantilla:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{OrderId};{OrderPrice};{OrderVolume};{Side};{OrderState};{TimeInForce};{TradeId};{TradePrice}

     ```

     Aquí los valores de {SecurityId.SecurityCode} y {SecurityId.BoardCode} corresponden a los valores de **Security** y **Board**, respectivamente. Por lo tanto, en el campo **Field order** asignamos los valores 0 y 1, respectivamente.
   - Para los campos {ServerTime:default:yyyyMMdd} y {ServerTime:default:HH:mm:ss.ffffff}, seleccione los campos **Date** y **Time** en la ventana **S# field**, respectivamente. Asigne los valores 2 y 3.
   - Para el campo {OrderId}, seleccione el campo **ID** en la ventana **S# field**: ID de la orden. Asígnele el valor 4.
   - Para el campo {OrderPrice}, seleccione el campo **Price** en la ventana **S# field**: precio de la orden. Asígnele el valor 5.
   - Para el campo {OrderVolume}, seleccione el campo **Volume** en la ventana **S# field**: volumen de la orden. Asígnele el valor 6.
   - Para el campo {Side}, seleccione el campo **Direction** en la ventana **S# field**: dirección de la orden (compra o venta). Asígnele el valor 7.
   - Para el campo {OrderState}, seleccione el campo **Action** en la ventana **S# field**: estado de la orden (activa, inactiva o con error). Asígnele el valor 8.
   - Para el campo {TimeInForce}, seleccione **Time** in force en la ventana **S# field**: una condición de ejecución de una orden limitada. Asígnele el valor 9.
   - Para el campo {TradeId}, seleccione el campo **ID (trade)** en la ventana **S# field**: identificador de la operación. Asígnele el valor 10.
   - Para el campo {TradePrice}, seleccione el campo **Price (trade)** en la ventana **S# field**: precio de la operación. Asígnele el valor 11.
   - La ventana de configuración de campos tendrá este aspecto:![hydra import prop orderlog](../../../images/hydra_import_prop_orderlog.png)

   El usuario puede configurar una gran cantidad de propiedades para los datos descargados. Basándose en la plantilla del archivo importado, debe especificar la propiedad y asignarle el número requerido en la secuencia.
3. Para previsualizar los datos, haga clic en el botón **Vista previa**.![hydra import preview orderlog](../../../images/hydra_import_preview_orderlog.png)
4. Haga clic en el botón **Importar**.
