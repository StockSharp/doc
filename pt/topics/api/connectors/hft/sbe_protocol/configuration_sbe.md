# Configuração do SBE

Configure a conexão do cliente com os parâmetros fornecidos pelo administrador do servidor SBE:

- `Address` - endpoint TCP do servidor SBE.
- `SenderCompId` - identificador do cliente usado no login.
- `TargetCompId` - identificador do servidor de destino.
- `Password` - credencial de autenticação.
- `IsSupportNativeCandles` - habilita velas fornecidas pelo servidor. Quando desabilitado, as velas são formadas no cliente a partir dos dados de mercado.

O servidor usa [SbeServerSettings](xref:StockSharp.Server.Sbe.SbeServerSettings):

- `IsEnabled` - habilita o endpoint SBE.
- `Address` - endpoint de escuta. Valor padrão: `127.0.0.1:5002`.
- `HeartBeat` - intervalo de heartbeat da sessão. Valor padrão: 60 segundos.
- `TargetCompId` - identificador do servidor. Valor padrão: `StockSharp`.

Use a mesma versão do esquema SBE no cliente e no servidor.
