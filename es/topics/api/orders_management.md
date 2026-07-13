# Gestión de órdenes

[S#](../api.md) proporciona una amplia funcionalidad para gestionar eficientemente órdenes de negociación en todas las etapas de su ciclo de vida. Esta sección cubre los aspectos clave del trabajo con órdenes en aplicaciones de negociación.

## Funciones principales

- **Creación de órdenes** - formación de varios tipos de órdenes de negociación (mercado, límite, stop, etc.)
- **Seguimiento de estado** - recepción de información actualizada sobre el estado actual de las órdenes
- **Gestión de órdenes** - cancelación, modificación y reemplazo de órdenes existentes
- **Manejo de eventos** - respuesta a eventos de registro, ejecución y cancelación de órdenes
- **Operaciones masivas** - trabajo eficiente con grupos de órdenes

## Ciclo de vida de una orden

Cada orden en S# pasa por determinadas etapas de ciclo de vida:

1. **Creación** - formación de un objeto [Order](xref:StockSharp.BusinessEntities.Order) con los parámetros necesarios
2. **Registro** - envío de la orden al sistema de negociación
3. **Ejecución** - ejecución parcial o completa de la orden, formación de operaciones
4. **Finalización** - ejecución completa, cancelación o rechazo de la orden

La API proporciona información detallada sobre el estado de la orden en cada etapa, lo que permite construir algoritmos de negociación complejos con control preciso de ejecución.

## Integración con estrategias de negociación

El mecanismo de gestión de órdenes está estrechamente integrado con los componentes para desarrollar estrategias de negociación [Strategy](xref:StockSharp.Algo.Strategies.Strategy), lo que permite:

- Encapsular la lógica de gestión de órdenes dentro de la estrategia
- Realizar seguimiento y procesamiento automático de eventos de registro y ejecución de órdenes
- Usar un enfoque unificado para la gestión de órdenes tanto en negociación real como durante pruebas

## Véase también

[Crear una nueva orden](orders_management/create_new_order.md)

[Crear una nueva orden stop](orders_management/create_new_stop_order.md)

[Estados de órdenes](orders_management/orders_states.md)

[Cancelación de órdenes](orders_management/order_cancel.md)

[Cancelación masiva de órdenes](orders_management/orders_mass_cancel.md)

[Reemplazo de órdenes](orders_management/orders_replacement.md)

[Número de transacción](orders_management/transaction_number.md)
