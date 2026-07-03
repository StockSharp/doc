# Level 1

Para importar datos Level 1, seleccione **Import \=\> Level 1** en el menú principal de la aplicación.

![hydra import level1](../../../images/hydra_import_level1.png)

## Proceso de importación.

1. **Import settings.**.

   Consulte la importación de [Velas](candles.md).
2. Configure los parámetros de importación para los campos de [S#](../../api.md).

   Consulte la importación de [Velas](candles.md).

   **Veamos un ejemplo de importación de Level 1 desde un archivo CSV:**
   - El archivo desde el que desea importar datos tiene la siguiente plantilla:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{ServerTime:default:yyyyMMdd};{ServerTime:default:HH:mm:ss.ffffff};{Changes:{BestBidPrice};{BestBidVolume};{BestAskPrice};{BestAskVolume};{LastTradeTime};{LastTradePrice};{LastTradeVolume}}
     	  				
     ```

     Aquí los valores de {SecurityId.SecurityCode} y {SecurityId.BoardCode} corresponden a los valores de **Security** y **Board**, respectivamente. Por lo tanto, en el campo **Field order** asignamos los valores 0 y 1, respectivamente.
   - Para los campos {ServerTime:default:yyyyMMdd} y {ServerTime:default:HH:mm:ss.ffffff}, seleccione los campos Date y **Time** en la ventana **S# field**, respectivamente. Asigne los valores 2 y 3.
   - Para el campo {BestBidPrice}, seleccione el campo **Best buy price** en la ventana **S# field**. Asígnele el valor 4.
   - Para el campo {BestBidVolume}, seleccione el campo **Best buy volume** en la ventana **S# field**. Asígnele el valor 5.
   - Para el campo {BestAskPrice}, seleccione el campo **Best sale price** en la ventana **S# field**. Asígnele el valor 6.
   - Para el campo {BestAskVolume}, seleccione el campo **Best sale volume** en la ventana **S# field**. Asígnele el valor 7.
   - Para el campo {LastTradeTime}, seleccione el campo **Last trade time** en la ventana **S# field**. Asígnele el valor 8.
   - Para el campo {LastTradePrice}, seleccione el campo **Last trade price** en la ventana **S# field**. Asígnele el valor 9.
   - Para el campo {LastTradeVolume}, seleccione el campo **Last trade volume** en la ventana **S# field**. Asígnele el valor 10.
   - La ventana de configuración de campos tendrá este aspecto:![hydra import prop level 1](../../../images/hydra_import_prop_level1.png)

   El usuario puede configurar una gran cantidad de propiedades para los datos descargados. Basándose en la plantilla del archivo importado, debe especificar la propiedad y asignarle el número requerido en la secuencia.
3. Para previsualizar los datos, haga clic en el botón **Preview**.![hydra import preview level 1](../../../images/hydra_import_preview_level1.png)
4. Haga clic en el botón **Import**.
