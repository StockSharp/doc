# Shell

**Shell** es un framework gráfico preparado para un terminal de negociación, con el código fuente completamente abierto en C#. Está construido con [Avalonia](https://avaloniaui.net/) y funciona en Windows, Linux y macOS.

No necesita dedicar tiempo a crear una GUI (interfaz gráfica de usuario): la conexión a fuentes de datos, las tablas de carteras, instrumentos, órdenes y operaciones, los gráficos, las pruebas sobre historial, la optimización de parámetros y el registro de trabajo ya vienen incluidos. Solo queda escribir la propia estrategia.

![Captura de Shell](../images/shell_title_00.png)

## Características principales

1. Código fuente completo. Sirve tanto para robots por encargo como para soluciones propias.
2. **Soporte para más de 70 conexiones** a bolsas y brókeres mediante [Conectores](api/connectors.md).
3. Cuatro áreas de trabajo, entre las que se cambia con la barra lateral: [Común](shell/user_interface/common.md), [Tiempo real](shell/user_interface/real_time.md), [Emulación](shell/user_interface/emulation.md) y [Registros](shell/user_interface/log.md).
4. Interfaz flexible: todos los paneles de un área de trabajo se pueden arrastrar, dividir, desacoplar en una ventana aparte y volver a acoplar. La disposición se guarda entre inicios.
5. Prueba de estrategias sobre historial con estadísticas, gráfico de operaciones y curva de rentabilidad.
6. [Optimización](shell/user_interface/optimization.md): búsqueda de los parámetros de la estrategia sobre el mismo historial.
7. Guardado y restauración de la configuración de la estrategia, incluidas las reglas de riesgo.
8. Ejecución simultánea de varias estrategias: cada una funciona por su cuenta y tiene sus propias órdenes, operaciones y estadísticas.
9. Ejecución de estrategias creadas en [Designer](designer.md) sin escribir código.
10. [Control remoto](shell/remote_control.md): inicio y detención de estrategias desde el sitio web o desde el bot de Telegram.

## Estrategias incluidas

Shell incluye cuatro estrategias que se pueden ejecutar nada más iniciarlo:

| Estrategia | Qué hace |
|---|---|
| **SMA strategy** | cruce de las medias móviles corta y larga |
| **SMA Index strategy** | lo mismo, pero con un instrumento sintético definido por una expresión |
| **StairsTrend strategy** | entrada a favor de la tendencia tras varias velas seguidas en la misma dirección |
| **StairsCountertrend strategy** | entrada contra la tendencia en la misma situación |

## Contenido recomendado

[Instalación de Shell](shell/installing_shell.md)

[Inicio rápido](shell/quick_start.md)
