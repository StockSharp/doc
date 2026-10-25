# Ejecutar estrategias desde Designer

[Shell](../shell.md) puede ejecutar estrategias creadas en [Designer](../designer.md) y exportadas mediante [Exportación de estrategias](../designer/export_import/export.md). Para ello no hace falta escribir código: el esquema lo ejecuta el mismo motor que en Designer.

## Importación

Abra el área [Tiempo real](user_interface/real_time.md) o [Emulación](user_interface/emulation.md), pulse **Añadir** y, en la ventana que se abre, **Importar**.

![Shell selección de estrategia](../../images/shell_add_strategy_00.png)

Seleccione el archivo del esquema exportado desde Designer. Aparecerá en la lista junto a las estrategias incluidas, con el nombre que tenía en Designer. Junto a la lista se muestra cómo ha terminado la importación: esquema importado, importación cancelada o importación fallida con la causa.

Los esquemas importados se guardan en la carpeta `Data` junto a la aplicación y siguen en la lista después de reiniciar.

## Inicio

A partir de ahí, se trabaja con el esquema igual que con una estrategia incluida: selecciónelo, pulse **OK**, rellene el instrumento, la cartera, el volumen y los parámetros del esquema en el panel **Estrategia** y pulse **Iniciar**.

En el área [Emulación](user_interface/emulation.md), el esquema se ejecuta sobre historial y da las mismas estadísticas, el mismo gráfico y las mismas listas de órdenes y operaciones que cualquier otra estrategia.

## Contenido recomendado

[Crear estrategia](create_strategy.md)

[Tiempo real](user_interface/real_time.md)
