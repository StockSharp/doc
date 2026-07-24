# Protocolo SBE

La compatibilidad con **SBE (Simple Binary Encoding)** proporciona un transporte binario compacto para datos de mercado y mensajes de negociación de baja latencia.

StockSharp incluye [SbeRecordSerializer](xref:StockSharp.Server.Sbe.SbeRecordSerializer) para codificar y decodificar registros, [SbeServer](xref:StockSharp.Server.Sbe.SbeServer) para aceptar conexiones de clientes y [StockSharpSBEMessageAdapter](xref:StockSharp.SBE.StockSharpSBEMessageAdapter) para las conexiones de clientes.

La implementación admite búsqueda de instrumentos, Level1, libros de órdenes, ticks, velas nativas opcionales, datos de carteras y posiciones, y operaciones con órdenes. Los identificadores y las versiones del esquema deben coincidir en el cliente y el servidor.

## Véase también

[Configuración de SBE](sbe_protocol/configuration_sbe.md)

[Inicialización del adaptador SBE](sbe_protocol/adapter_initialization_sbe.md)

[Protocolo FIX](../common/fix_protocol.md)

[Protocolo FAST](../common/fast_protocol.md)
