# Protocolo SBE

O suporte a **SBE (Simple Binary Encoding)** fornece um transporte binário compacto para dados de mercado e mensagens de negociação de baixa latência.

O StockSharp inclui [SbeRecordSerializer](xref:StockSharp.Server.Sbe.SbeRecordSerializer) para codificar e decodificar registros, [SbeServer](xref:StockSharp.Server.Sbe.SbeServer) para aceitar conexões de clientes e [StockSharpSBEMessageAdapter](xref:StockSharp.SBE.StockSharpSBEMessageAdapter) para conexões de clientes.

A implementação oferece busca de instrumentos, Level1, livros de ofertas, ticks, velas nativas opcionais, dados de carteiras e posições e operações com ordens. Os identificadores e as versões do esquema devem coincidir no cliente e no servidor.

## Veja também

[Configuração do SBE](sbe_protocol/configuration_sbe.md)

[Inicialização do adaptador SBE](sbe_protocol/adapter_initialization_sbe.md)

[Protocolo FIX](../common/fix_protocol.md)

[Protocolo FAST](../common/fast_protocol.md)
