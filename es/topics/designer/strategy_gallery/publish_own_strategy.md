# Publicación de su estrategia

Puede publicar su estrategia haciendo clic con el botón izquierdo del ratón en la estrategia en el panel [Panel Esquemas](../user_interface/schemas.md) y seleccionando **Publicar**:

![Designer_publish_00](../../../images/designer_publish_00.png)

Después de hacer clic en el botón **Publicar**, se abrirá una ventana con la elección del tipo de exportación. Para más detalles, vea la sección [Exportación de estrategias](../export_import/export.md).

Después de elegir el tipo de exportación, se activa el programa [Installer](../../installer.md) con los parámetros de publicación ([Installer](../../installer.md) debe estar iniciado de antemano):

![Designer_publish_01](../../../images/designer_publish_01.png)

Campos que deben rellenarse:

- Nombre
- Descripción
- Identificador de paquete Nuget. Este parámetro es necesario para establecer el enlace al producto en la tienda. Por ejemplo, en la dirección https://stocksharp.com/store/runner/, la palabra **runner** se especifica mediante este parámetro.

El acceso al nivel **Free** o **Paid** se concede solo después de contactar por correo electrónico [info@stocksharp.com](mailto:info@stocksharp.com). De forma predeterminada, está disponible el nivel **Private**, que permite publicar estrategias solo en formato privado (para usuarios seleccionados):

Después de hacer clic en el botón **Guardar**, la estrategia se enviará al servidor de StockSharp.

Al publicar actualizaciones, no es necesario introducir todos los parámetros de nuevo. En lugar de introducir parámetros del producto, aparecerá una ventana para introducir una nota de la actualización:

![Designer_publish_02](../../../images/designer_publish_02.png)

Después de hacer clic en el botón **OK**, aparecerá una ventana indicando que la actualización se realizó correctamente:

![Designer_publish_03](../../../images/designer_publish_03.png)
