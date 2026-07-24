# SBE-Protokoll

Die Unterstützung für **SBE (Simple Binary Encoding)** stellt einen kompakten binären Transport für Marktdaten und Handelsnachrichten mit geringer Latenz bereit.

StockSharp enthält [SbeRecordSerializer](xref:StockSharp.Server.Sbe.SbeRecordSerializer) zum Kodieren und Dekodieren von Datensätzen, [SbeServer](xref:StockSharp.Server.Sbe.SbeServer) zum Annehmen von Clientverbindungen und [StockSharpSBEMessageAdapter](xref:StockSharp.SBE.StockSharpSBEMessageAdapter) für Clientverbindungen.

Die Implementierung unterstützt die Instrumentensuche, Level1, Orderbücher, Ticks, optionale native Kerzen, Portfolio- und Positionsdaten sowie Orderoperationen. Schema-IDs und Versionen von Client und Server müssen übereinstimmen.

## Siehe auch

[SBE-Konfiguration](sbe_protocol/configuration_sbe.md)

[Initialisierung des SBE-Adapters](sbe_protocol/adapter_initialization_sbe.md)

[FIX-Protokoll](../common/fix_protocol.md)

[FAST-Protokoll](../common/fast_protocol.md)
