# Documentación de la API

## Descripción general

La API de StockSharp, también conocida como S# API, es un kit de desarrollo de software (SDK) para crear aplicaciones de trading como [Designer](designer.md), [Terminal](terminal.md) y otras herramientas de trading personalizadas. Proporciona la infraestructura principal para datos de mercado, enrutamiento de órdenes, ejecución de estrategias, pruebas y componentes de interfaz de usuario de trading.

## Características

- **Scripting de estrategias**: la API de StockSharp permite a los usuarios escribir y ejecutar estrategias de trading directamente en [Designer](designer/strategies/using_code.md). Las estrategias pueden desarrollarse, probarse e implementarse usando C#, F# o Python.

- **Herramientas de análisis**: la API se integra con Hydra para un [análisis detallado de datos de mercado](hydra/analytics.md). Admite procesamiento de datos, almacenamiento y flujos de trabajo analíticos personalizados.

- **Desarrollo de aplicaciones personalizadas**: los desarrolladores pueden usar la API de StockSharp para crear [soluciones de trading](api/examples.md) independientes en lugar de depender únicamente del scripting integrado de la aplicación.

- **Conectores y controles gráficos**: la API incluye muchos [Conectores](api/connectors.md) para el acceso a datos de mercado en tiempo real y operaciones de trading. También ofrece componentes de [Interfaz gráfica de usuario](api/graphical_user_interface.md) personalizables para crear plataformas de trading profesionales.

## Arquitectura

La API de StockSharp está construida en torno a la modularidad y la [extensibilidad](api/connectors/creating_own_connector.md). Los desarrolladores pueden extenderla con plugins y módulos adicionales sin modificar el sistema principal. Esta arquitectura ayuda a crear aplicaciones de trading escalables y mantenibles.

## Código abierto

El núcleo de la API de StockSharp es de código abierto. El código fuente está disponible en GitHub, de modo que los desarrolladores pueden estudiarlo, modificarlo y contribuir con mejoras.

## Repositorio de GitHub

El código fuente oficial de la API de StockSharp está disponible en el repositorio de GitHub:

[Repositorio de StockSharp en GitHub](https://github.com/stocksharp/stocksharp)
