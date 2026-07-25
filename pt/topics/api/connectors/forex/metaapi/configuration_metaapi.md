# Configuração do conector: MetaApi

Crie um token do MetaApi, implante a conta de negociação e informe os parâmetros de conexão.

- `Token` - token de acesso do MetaApi.
- `AccountId` - identificador da conta MetaApi implantada.
- `Region` - região da conta. Os tokens de API a determinam automaticamente, portanto informe-a explicitamente apenas para tokens restritos a uma conta.
- `Domain` - domínio do MetaApi. O padrão é `agiliumtrade.agiliumtrade.ai`.
- `SynchronizationTimeout` - tempo de espera pela sincronização do terminal no servidor. O padrão é 2 minutos, e valores menores são elevados para 10 segundos.

A conexão é concluída assim que o MetaApi informa que o estado do terminal está sincronizado, portanto o tempo limite deve cobrir a sincronização inicial do histórico da conta.

Os parâmetros de ordens pendentes e de proteção são informados por meio de [MetaApiOrderCondition](xref:StockSharp.MetaApi.MetaApiOrderCondition): o preço de ativação, os preços de stop-loss e take-profit e o número mágico, o comentário e o identificador do cliente armazenados com a posição.

## Veja também

[Documentação oficial do MetaApi](https://metaapi.cloud/docs/client/)
