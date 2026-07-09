# Configuración Aster

Para trabajar con el conector, genere una **clave API** y un **secreto** en la cuenta del exchange y especifíquelos en la configuración de conexión.

Configuraciones principales:

- **clave** y **secreto**.
- **Section**: `Spot` o `Derivatives`.
- **Modo de derivados**: `Legacy` o `V3 Agent`.
- Endpoints **Spot REST / Spot WS**.
- Endpoints **Derivatives REST / Derivatives WS**.
- Modo **Demo**.

Documentación oficial de la API:

- [Resumen de la API Spot](https://asterdex.github.io/aster-api-website/spot/spot-api-overview/)
- [API de cuenta y trading Spot](https://asterdex.github.io/aster-api-website/spot/spot-account-and-trading-api/)
- [Datos de mercado por websocket Spot](https://asterdex.github.io/aster-api-website/spot/websocket-market-data/)
- [Información de cuenta por websocket Spot](https://asterdex.github.io/aster-api-website/spot/websocket-account-info/)
- [Información general de Futures v3](https://asterdex.github.io/aster-api-website/futures-v3/general-info/)
- [Flujos de datos de usuario de Futures](https://asterdex.github.io/aster-api-website/futures/user-data-streams/)
- [Endpoints de Aster Code](https://asterdex.github.io/aster-api-website/asterCode/endpoints/)

> [!TIP]
> Los derivados de Aster tienen dos familias de protocolos. Seleccione el **Modo de derivados** correcto antes de habilitar el trading.
