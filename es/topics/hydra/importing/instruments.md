# Instrumentos

Para importar instrumentos, seleccione la pestaña **Importar \=\> Instrumentos**.

![hydra import securities](../../../images/hydra_import_securities.png)

## Proceso de importación.

1. **Configuración de importación**.

   Consulte la importación de [Velas](candles.md).
2. Configure los parámetros de importación para los campos de [S#](../../api.md).

   Consulte la importación de [Velas](candles.md).

   **Veamos un ejemplo de importación de un instrumento desde un archivo CSV:**
   - El archivo desde el que desea importar datos tiene la siguiente plantilla:

     ```none
     {SecurityId.SecurityCode};{SecurityId.BoardCode};{PriceStep};{SecurityType};{VolumeStep}

     ```

     Aquí los valores de {SecurityId.SecurityCode} y {SecurityId.BoardCode} corresponden a los valores de **Instrumento** y **Mercado**, respectivamente. Por lo tanto, en el campo **Orden de campos** asignamos los valores 0 y 1, respectivamente.
   - Para el campo {PriceStep}, seleccione el campo **Nominal** en la ventana **campo S#** y asígnele el valor 2.
   - Para el campo {SecurityType}, seleccione el campo **Tipo** en la ventana **campo S#**: el tipo de instrumento (acción, divisa, futuro, etc.). Asígnele el valor 3.
   - Para el campo {VolumeStep}, seleccione el campo **Volumen mín. (base)** en la ventana **campo S#**: el volumen base o mínimo del instrumento. Asígnele el valor 4.
   - La ventana de configuración de campos tendrá este aspecto:![hydra import prop securitiy](../../../images/hydra_import_prop_securitiy.png)

   El usuario puede configurar una gran cantidad de propiedades para los datos descargados. Basándose en la plantilla del archivo importado, debe especificar la propiedad y asignarle el número requerido en la secuencia.
3. Para previsualizar los datos, haga clic en el botón **Vista previa**.![hydra import preview securitiy](../../../images/hydra_import_preview_securitiy.png)
4. Haga clic en el botón **Importar**.
