# Configuración de SBE

Configure la conexión del cliente con los parámetros facilitados por el administrador del servidor SBE:

- `Address` - extremo TCP del servidor SBE.
- `SenderCompId` - identificador del cliente utilizado durante el inicio de sesión.
- `TargetCompId` - identificador del servidor de destino.
- `Password` - credencial de autenticación.
- `IsSupportNativeCandles` - habilita las velas proporcionadas por el servidor. Si está deshabilitado, las velas se forman en el cliente a partir de los datos de mercado.

El servidor utiliza [SbeServerSettings](xref:StockSharp.Server.Sbe.SbeServerSettings):

- `IsEnabled` - habilita el extremo SBE.
- `Address` - extremo de escucha. Valor predeterminado: `127.0.0.1:5002`.
- `HeartBeat` - intervalo de latido de la sesión. Valor predeterminado: 60 segundos.
- `TargetCompId` - identificador del servidor. Valor predeterminado: `StockSharp`.

Utilice la misma versión del esquema SBE en el cliente y el servidor.
