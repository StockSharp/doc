# OpenFIGI

**OpenFIGI** conecta o StockSharp à OpenFIGI API v3 para pesquisa global de identificadores de instrumentos.

## Principais recursos

- Recursos verificados no código-fonte: pesquisa e filtragem por bolsa, MIC, moeda, setor de mercado e tipo de título. O adaptador retorna metadados, mas não fornece cotações nem negociação.
- Transportes, sessões e formatos específicos do provedor ficam ocultos atrás da API padrão do StockSharp.

## Uso típico

Use este conector para mapear identificadores para registros FIGI e criar um catálogo normalizado de instrumentos.

A cobertura, os limites e o acesso anônimo ou com chave são controlados pela OpenFIGI.

## Veja também

[Configuração do conector](openfigi/configuration_openfigi.md)

[Configuração gráfica](openfigi/graphical_configuration_openfigi.md)

[Inicialização do adaptador](openfigi/adapter_initialization_openfigi.md)
