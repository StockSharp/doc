# Configuración del conector: MetaApi

Cree un token de MetaApi, despliegue la cuenta de negociación e indique los parámetros de conexión.

- `Token` - token de acceso de MetaApi.
- `AccountId` - identificador de la cuenta desplegada en MetaApi.
- `Region` - región de la cuenta. Los tokens de API la resuelven automáticamente, por lo que solo debe indicarse explícitamente para los tokens asociados a una cuenta.
- `Domain` - dominio de MetaApi. Valor predeterminado: `agiliumtrade.agiliumtrade.ai`.
- `SynchronizationTimeout` - tiempo de espera de la sincronización del terminal en el servidor. Valor predeterminado: 2 minutos; los valores menores se elevan a 10 segundos.

La conexión finaliza cuando MetaApi informa de que el estado del terminal está sincronizado, por lo que el tiempo de espera debe cubrir la sincronización inicial del historial de la cuenta.

Los parámetros de las órdenes pendientes y de protección se transmiten a través de [MetaApiOrderCondition](xref:StockSharp.MetaApi.MetaApiOrderCondition): el precio de activación, los precios de stop-loss y take-profit, y el número mágico, el comentario y el identificador de cliente que se guardan con la posición.

## Véase también

[Documentación oficial de MetaApi](https://metaapi.cloud/docs/client/)
