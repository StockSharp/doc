# Ticks

Para importar operaciones, seleccione la pestaña **Importar \=\> Ticks**.

![hydra importar operaciones](../../../images/hydra_import_trades.png)

## Proceso de importación.

1. **Configuración de importación**.

   Consulte la importación de [Velas](candles.md).
2. Configure los parámetros de importación para los campos de [S#](../../api.md).

   Consulte la importación de [Velas](candles.md).

   **Veamos un ejemplo de importación de operaciones (ticks) desde un archivo CSV:**
   - El archivo desde el que desea importar datos tiene la siguiente plantilla:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{TradeId};{TradePrice};{TradeVolume};{OriginSide}

     ```

     Aquí los valores de {SecurityId.SecurityCode} y {SecurityId.BoardCode} corresponden a los valores de **Instrumento** y **Mercado**, respectivamente. Por lo tanto, en el campo **Orden de campos** asignamos los valores 0 y 1, respectivamente.
   - Para los campos {ServerTime:default:yyyyMMdd} y {ServerTime:default:HH:mm:ss.ffffff}, seleccione los campos **Fecha** y **Hora** en la ventana **campo S#**, respectivamente. Asigne los valores 2 y 3.
   - Para el campo {TradeId}, seleccione el campo **Identificador** en la ventana **campo S#**: identificador o número de la operación. Asígnele el valor 4.
   - Para el campo {TradePrice}, seleccione el campo **Precio** en la ventana **campo S#**: precio de la operación. Asígnele el valor 5.
   - Para el campo {TradeVolume}, seleccione el campo **Volumen** en la ventana **campo S#**: volumen de la operación. Asígnele el valor 6.
   - Para el campo {OriginSide}, seleccione el campo **Iniciador** en la ventana **campo S#**: iniciador de la operación (vendedor o comprador). Asígnele el valor 7.
   - La ventana de configuración de campos tendrá este aspecto:![hydra import propiedades de operación](../../../images/hydra_import_prop_trade.png)

   El usuario puede configurar una gran cantidad de propiedades para los datos descargados. Basándose en la plantilla del archivo importado, debe especificar la propiedad y asignarle el número requerido en la secuencia.
3. Para previsualizar los datos, haga clic en el botón **Vista previa**.![hydra import vista previa de operación](../../../images/hydra_import_preview_trade.png)
4. Haga clic en el botón **Importar**.
