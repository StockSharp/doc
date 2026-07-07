# Configuração do Ambiente

## Requisitos

Para trabalhar com StockSharp precisa de:

- **.NET 10** (SDK e Runtime) - [transferir](https://dotnet.microsoft.com/download/dotnet/10.0)
- **IDE** - Visual Studio 2022+, JetBrains Rider ou VS Code
- **NuGet** - gestor de pacotes (integrado no Visual Studio e no Rider)

Verifique se o SDK está instalado:

```bash
dotnet --version
```

## Criar um Projeto

### Visual Studio 2022+

1. **File → New → Project**
2. Selecione o template **Console App** ou **WPF Application**
3. Defina a framework de destino como **.NET 10**

### JetBrains Rider

1. **File → New Solution**
2. Selecione **.NET / .NET Core → Console Application**
3. Defina **Target Framework: net10.0**

### Linha de Comandos (CLI)

```bash
# Aplicação de consola
dotnet new console -n MyTradingApp --framework net10.0
cd MyTradingApp

# Aplicação WPF (apenas Windows)
dotnet new wpf -n MyTradingGui --framework net10.0-windows
```

## Catálogo de Pacotes NuGet

StockSharp é distribuído via NuGet. Abaixo está o catálogo completo de pacotes organizado por categoria.

### Core

| Pacote | Descrição |
|---------|-------------|
| [StockSharp.Messages](https://www.nuget.org/packages/StockSharp.Messages/) | Mensagens e contratos base. Fundação de toda a framework |
| [StockSharp.BusinessEntities](https://www.nuget.org/packages/StockSharp.BusinessEntities/) | Entidades de negociação: Security, Order, Trade, Portfolio, etc. |
| [StockSharp.Algo](https://www.nuget.org/packages/StockSharp.Algo/) | Negociação algorítmica core, Connector, subscrições, candles |
| [StockSharp.Configuration](https://www.nuget.org/packages/StockSharp.Configuration/) | Gestão de configuração, definições de ligação |
| [StockSharp.Localization](https://www.nuget.org/packages/StockSharp.Localization/) | Sistema de localização (inglês por predefinição) |

### Estratégias e Indicadores

| Pacote | Descrição |
|---------|-------------|
| [StockSharp.Algo.Strategies](https://www.nuget.org/packages/StockSharp.Algo.Strategies/) | Framework de estratégias - classe Strategy base, posições, PnL |
| [StockSharp.Algo.Indicators](https://www.nuget.org/packages/StockSharp.Algo.Indicators/) | Mais de 100 indicadores técnicos (SMA, EMA, RSI, MACD, Bollinger, etc.) |

### Testes

| Pacote | Descrição |
|---------|-------------|
| [StockSharp.Algo.Testing](https://www.nuget.org/packages/StockSharp.Algo.Testing/) | Backtesting em dados históricos, emulação de negociação |

### Armazenamento e Dados

| Pacote | Descrição |
|---------|-------------|
| [StockSharp.Algo.Export](https://www.nuget.org/packages/StockSharp.Algo.Export/) | Exportação de dados de mercado (CSV, Excel, JSON, etc.) |
| [StockSharp.Algo.Import](https://www.nuget.org/packages/StockSharp.Algo.Import/) | Importação de dados a partir de formatos externos |

### Análise e Computação

| Pacote | Descrição |
|---------|-------------|
| [StockSharp.Algo.Analytics](https://www.nuget.org/packages/StockSharp.Algo.Analytics/) | Interfaces de scripts analíticos |
| [StockSharp.Algo.Compilation](https://www.nuget.org/packages/StockSharp.Algo.Compilation/) | Compilação de código em runtime |
| [StockSharp.Algo.Gpu](https://www.nuget.org/packages/StockSharp.Algo.Gpu/) | Cálculos de indicadores acelerados por GPU (CUDA via ILGPU) |

### Componentes GUI (Apenas Windows)

| Pacote | Descrição |
|---------|-------------|
| [StockSharp.Xaml](https://www.nuget.org/packages/StockSharp.Xaml/) | Controlos WPF: tabelas de instrumentos, portefólios e ordens |
| [StockSharp.Xaml.Charting](https://www.nuget.org/packages/StockSharp.Xaml.Charting/) | Gráficos de velas, indicadores, curvas de equity |
| [StockSharp.Charting.Interfaces](https://www.nuget.org/packages/StockSharp.Charting.Interfaces/) | Interfaces de componentes de gráficos |
| [StockSharp.Alerts.Interfaces](https://www.nuget.org/packages/StockSharp.Alerts.Interfaces/) | Interfaces do sistema de alertas |
| [StockSharp.Diagram.Core](https://www.nuget.org/packages/StockSharp.Diagram.Core/) | Core do designer visual de estratégias |

### Conectores (Bolsas e Brokers)

Cada conector é um pacote NuGet separado. Principais conectores:

| Pacote | Bolsa/Broker |
|---------|----------------|
| `StockSharp.Binance` | Binance |
| `StockSharp.InteractiveBrokers` | Interactive Brokers |
| `StockSharp.Fix` | Protocolo FIX (universal) |
| `StockSharp.Connectors.Coinbase` | Coinbase |
| `StockSharp.Connectors.BitStamp` | Bitstamp |
| `StockSharp.Connectors.Bittrex` | Bittrex |

> [!NOTE]
> Para a lista completa de conectores, consulte a secção [Conectores](connectors.md). Alguns conectores só estão disponíveis através do [servidor NuGet privado](#servidor-nuget-privado).

### Localização

O pacote base `StockSharp.Localization` inclui inglês. Idiomas adicionais são instalados como pacotes separados:

| Pacote | Idioma |
|---------|----------|
| `StockSharp.Localization.ru` | Russo |
| `StockSharp.Localization.zh` | Chinês |
| `StockSharp.Localization.de` | Alemão |
| `StockSharp.Localization.es` | Espanhol |
| `StockSharp.Localization.ja` | Japonês |
| `StockSharp.Localization.ko` | Coreano |
| `StockSharp.Localization.All` | Todos os idiomas (meta-pacote) |

Também disponíveis: `ar`, `bn`, `ca`, `cs`, `da`, `el`, `fa`, `fi`, `fr`, `he`, `hi`, `hu`, `it`, `jv`, `ms`, `my`, `nl`, `no`, `pa`, `pl`, `pt`, `ro`, `sk`, `sr`, `sv`, `ta`, `th`, `tl`, `tr`, `uk`, `uz`, `vi`.

## Instalar Pacotes

### Via CLI (Recomendado)

```bash
# Pacotes principais
dotnet add package StockSharp.Algo
dotnet add package StockSharp.Algo.Strategies

# Conector (exemplo - Binance)
dotnet add package StockSharp.Binance

# Indicadores
dotnet add package StockSharp.Algo.Indicators

# Backtesting
dotnet add package StockSharp.Algo.Testing

# Localização (russo)
dotnet add package StockSharp.Localization.ru
```

### Via Visual Studio

1. Clique com o botão direito no projeto → **Manage NuGet Packages...**
2. Pesquise por `StockSharp`
3. Selecione o pacote pretendido → **Install**

Todas as dependências são instaladas automaticamente.

### Via JetBrains Rider

1. Clique com o botão direito no projeto → **Manage NuGet Packages**
2. Pesquise por `StockSharp`
3. Selecione o pacote → **Install**

### Via Package Manager Console (Visual Studio)

```powershell
Install-Package StockSharp.Algo
Install-Package StockSharp.Binance
Install-Package StockSharp.Algo.Strategies
```

## Servidor NuGet Privado

Alguns componentes (conectores cripto, etc.) só estão disponíveis através do servidor NuGet privado para utilizadores registados.

### Método 1: Autenticação via Token no URL

1. Registe-se no site da StockSharp.
2. Copie o token da sua [conta pessoal](https://stocksharp.ru/profile/).
3. Adicione a origem do pacote:

**CLI:**

```bash
dotnet nuget add source "https://nuget.stocksharp.com/{YOUR_TOKEN}/v3/index.json" --name StockSharpPrivate
```

**Visual Studio:** abra **Tools → Options → NuGet Package Manager → Package Sources** e adicione uma nova origem com o URL `https://nuget.stocksharp.com/{YOUR_TOKEN}/v3/index.json`.

**Rider:** abra **Settings → Build, Execution, Deployment → NuGet → Sources** e adicione a origem.

### Método 2: Autenticação via Nome de Utilizador e Palavra-passe

1. Adicione uma origem de pacotes com o URL `https://nuget.stocksharp.com/x/v3/index.json`
2. Ao tentar usar esta origem, aparecerá um pedido de login
3. Introduza as credenciais da sua conta StockSharp (ou `x` como nome de utilizador e o seu token como palavra-passe)

**CLI:**

```bash
dotnet nuget add source "https://nuget.stocksharp.com/x/v3/index.json" --name StockSharpPrivate --username YOUR_LOGIN --password YOUR_PASSWORD --store-password-in-clear-text
```

> [!TIP]
> Para reiniciar credenciais guardadas no Windows, abra **Control Panel → User Accounts → Credential Manager** e elimine as entradas relacionadas com `nuget.stocksharp.com`.

## Atualizar Pacotes

### CLI

```bash
# Verificar atualizações disponíveis
dotnet list package --outdated

# Atualizar um pacote específico
dotnet add package StockSharp.Algo
```

### Visual Studio

1. **Manage NuGet Packages...** → separador **Updates**
2. Selecione pacotes → **Update**

### Rider

1. **Manage NuGet Packages** → separador **Upgrades**
2. Selecione pacotes → **Upgrade**

## Resolução de Problemas

### Incompatibilidade de Versões

Todos os pacotes StockSharp têm de ter a mesma versão. Se encontrar erros de tipo ou método, certifique-se de que todos os pacotes `StockSharp.*` estão atualizados para a mesma versão.

### Pacote Não Encontrado no NuGet Público

Alguns conectores só estão disponíveis através do [servidor privado](#servidor-nuget-privado). Certifique-se de que a origem correta foi adicionada.

### Problemas de GUI em Sistemas Não Windows

Os componentes GUI (`StockSharp.Xaml`, `StockSharp.Xaml.Charting`) funcionam apenas no Windows. Em Linux/macOS, use aplicações de consola sem pacotes GUI.

### Erros de Restauro de Pacotes

```bash
# Limpar a cache do NuGet
dotnet nuget locals all --clear

# Tentar restaurar novamente
dotnet restore
```

## Estrutura do Ficheiro de Projeto (.csproj)

### Mínimo (Bot de Negociação de Consola)

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="StockSharp.Algo" Version="*" />
    <PackageReference Include="StockSharp.Binance" Version="*" />
  </ItemGroup>
</Project>
```

### Alargado (Estratégia com Indicadores e Testes)

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <!-- Principal -->
    <PackageReference Include="StockSharp.Algo" Version="*" />
    <PackageReference Include="StockSharp.Configuration" Version="*" />

    <!-- Estratégias e indicadores -->
    <PackageReference Include="StockSharp.Algo.Strategies" Version="*" />
    <PackageReference Include="StockSharp.Algo.Indicators" Version="*" />

    <!-- Conector -->
    <PackageReference Include="StockSharp.Binance" Version="*" />

    <!-- Backtesting -->
    <PackageReference Include="StockSharp.Algo.Testing" Version="*" />

    <!-- Localização -->
    <PackageReference Include="StockSharp.Localization.ru" Version="*" />
  </ItemGroup>
</Project>
```

### Aplicação WPF com Gráficos

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net10.0-windows</TargetFramework>
    <UseWPF>true</UseWPF>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="StockSharp.Algo" Version="*" />
    <PackageReference Include="StockSharp.Algo.Strategies" Version="*" />
    <PackageReference Include="StockSharp.Xaml.Charting" Version="*" />
    <PackageReference Include="StockSharp.Binance" Version="*" />
  </ItemGroup>
</Project>
```

## Verificar a Instalação

Crie uma aplicação mínima:

```csharp
using StockSharp.Algo;
using StockSharp.BusinessEntities;

Console.WriteLine("StockSharp successfully configured!");

var connector = new Connector();
Console.WriteLine($"Connector created: {connector}");
```

```bash
dotnet run
```

## Exemplos

Exemplos prontos de utilização do StockSharp estão disponíveis no diretório [Samples/](https://github.com/stocksharp/stocksharp/tree/master/Samples) do repositório. Abrangem ligação a bolsas, subscrição de dados, construção de candles, indicadores, estratégias e testes.
