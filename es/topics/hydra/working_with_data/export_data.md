# Exportar datos

[Hydra](../../hydra.md) permite exportar los datos de mercado recibidos en distintos formatos, incluidos los [formatos de datos MetaStock](export_data/export_into_metastock.md).

Para exportar datos, se usan archivos en formatos [Excel](https://en.wikipedia.org/wiki/Excel), xml, bin, txt, Json o tablas SQL.

Para exportar, debe seleccionar el formato de archivo requerido en la lista desplegable:

![hydra export](../../../images/hydra_export.png)

Después debe seleccionar una carpeta y cambiar el nombre del archivo si es necesario.

Al exportar a archivos de texto (txt), aparece una ventana en la que puede especificar la plantilla de exportación con la forma:

**{OpenTime:default:yyyyMMdd};{OpenTime:default:HH:mm:ss};{OpenPrice};{HighPrice};{LowPrice};{ClosePrice};{TotalVolume}**

Aquí, entre llaves, se indican las propiedades que se exportarán y su orden, separadas por punto y coma.

Al hacer clic en el botón **Vista previa**, puede ver qué datos se guardarán en el archivo.

![hydra export TSLab Meta Stock 1](../../../images/hydra_export_tslab_metastock_1.png)

El usuario puede añadir propiedades adicionales, como el código del instrumento mediante la propiedad **{SecurityId.SecurityCode}**, o especificar un valor de marco temporal.

Puede añadir un encabezado que indique el nombre de las propiedades. En este caso, el registro tendrá este aspecto.

![hydra export TSLab Meta Stock 2](../../../images/hydra_export_tslab_metastock_2.png)

Si necesita exportar en un formato que usa dos puntos, debe especificar la palabra clave default como en el ejemplo anterior **{OpenTime:default:HH:mm:ss}**.

**Ver [tutorial en video](../videos/saving_format.md)**
