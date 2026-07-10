# Configuración del entorno

## Requisitos

Para trabajar con StockSharp necesita:

- **.NET 10** (SDK y Runtime) — [descargar](https://dotnet.microsoft.com/download/dotnet/10.0)
- **IDE** — Visual Studio 2022+, JetBrains Rider o VS Code
- **NuGet** — gestor de paquetes (integrado en Visual Studio y Rider)

Verifique que el SDK esté instalado:

```bash
dotnet --version
```

## Creación de un proyecto

### Visual Studio 2022+

1. **Archivo → Nuevo → Proyecto**
2. Seleccione la plantilla **Aplicación de consola** o **Aplicación WPF**
3. Establezca el framework de destino en **.NET 10**

### JetBrains Rider

1. **Archivo → Nueva solución**
2. Seleccione **.NET / .NET Core → Aplicación de consola**
3. Establezca **Marco de destino: net10.0**

### Línea de comandos (CLI)

```bash
# Aplicación de consola
dotnet new console -n MyTradingApp --framework net10.0
cd MyTradingApp

# Aplicación WPF (solo Windows)
dotnet new wpf -n MyTradingGui --framework net10.0-windows
```

## Catálogo de paquetes NuGet

StockSharp se distribuye mediante NuGet. A continuación se muestra el catálogo completo de paquetes organizado por categoría.

### Core

| Paquete | Descripción |
|---------|-------------|
| [StockSharp.Messages](https://www.nuget.org/packages/StockSharp.Messages/) | Mensajes y contratos base. Base de todo el framework |
| [StockSharp.BusinessEntities](https://www.nuget.org/packages/StockSharp.BusinessEntities/) | Entidades de trading: Security, Order, Trade, Portfolio, etc. |
| [StockSharp.Algo](https://www.nuget.org/packages/StockSharp.Algo/) | Trading algorítmico core, Connector, suscripciones, velas |
| [StockSharp.Configuration](https://www.nuget.org/packages/StockSharp.Configuration/) | Gestión de configuración, ajustes de conexión |
| [StockSharp.Localization](https://www.nuget.org/packages/StockSharp.Localization/) | Sistema de localización (inglés por defecto) |

### Estrategias e indicadores

| Paquete | Descripción |
|---------|-------------|
| [StockSharp.Algo.Strategies](https://www.nuget.org/packages/StockSharp.Algo.Strategies/) | Framework de estrategias — clase base Strategy, posiciones, PnL |
| [StockSharp.Algo.Indicators](https://www.nuget.org/packages/StockSharp.Algo.Indicators/) | Más de 100 indicadores técnicos (SMA, EMA, RSI, MACD, Bollinger, etc.) |

### Pruebas

| Paquete | Descripción |
|---------|-------------|
| [StockSharp.Algo.Testing](https://www.nuget.org/packages/StockSharp.Algo.Testing/) | Backtesting con datos históricos, emulación de trades |

### Almacenamiento y datos

| Paquete | Descripción |
|---------|-------------|
| [StockSharp.Algo.Export](https://www.nuget.org/packages/StockSharp.Algo.Export/) | Exportación de datos de mercado (CSV, Excel, JSON, etc.) |
| [StockSharp.Algo.Import](https://www.nuget.org/packages/StockSharp.Algo.Import/) | Importación de datos desde formatos externos |

### Analítica y cálculo

| Paquete | Descripción |
|---------|-------------|
| [StockSharp.Algo.Analytics](https://www.nuget.org/packages/StockSharp.Algo.Analytics/) | Interfaces de scripts analíticos |
| [StockSharp.Algo.Compilation](https://www.nuget.org/packages/StockSharp.Algo.Compilation/) | Compilación de código en tiempo de ejecución |
| [StockSharp.Algo.Gpu](https://www.nuget.org/packages/StockSharp.Algo.Gpu/) | Cálculos de indicadores acelerados por GPU (CUDA mediante ILGPU) |

### Componentes GUI (solo Windows)

| Paquete | Descripción |
|---------|-------------|
| [StockSharp.Xaml](https://www.nuget.org/packages/StockSharp.Xaml/) | Controles WPF: tablas de instrumentos, carteras y órdenes |
| [StockSharp.Xaml.Charting](https://www.nuget.org/packages/StockSharp.Xaml.Charting/) | Gráficos de velas, indicadores, curvas de equity |
| [StockSharp.Charting.Interfaces](https://www.nuget.org/packages/StockSharp.Charting.Interfaces/) | Interfaces de componentes de gráficos |
| [StockSharp.Alerts.Interfaces](https://www.nuget.org/packages/StockSharp.Alerts.Interfaces/) | Interfaces del sistema de alertas |
| [StockSharp.Diagram.Core](https://www.nuget.org/packages/StockSharp.Diagram.Core/) | Núcleo del diseñador visual de estrategias |

### Conectores (exchanges y brokers)

Cada conector es un paquete NuGet separado. Conectores principales:

| Paquete | Exchange/Broker |
|---------|----------------|
| `StockSharp.Binance` | Binance |
| `StockSharp.InteractiveBrokers` | Interactive Brokers |
| `StockSharp.Fix` | Protocolo FIX (universal) |
| `StockSharp.Connectors.Coinbase` | Coinbase |
| `StockSharp.Connectors.BitStamp` | Bitstamp |
| `StockSharp.Connectors.Bittrex` | Bittrex |

> [!NOTE]
> Para la lista completa de conectores, consulte la sección [Conectores](connectors.md). Algunos conectores solo están disponibles mediante el [servidor NuGet privado](#private-nuget-server).

### Localización

El paquete base `StockSharp.Localization` incluye inglés. Los idiomas adicionales se instalan como paquetes separados:

| Paquete | Idioma |
|---------|----------|
| `StockSharp.Localization.ru` | Ruso |
| `StockSharp.Localization.zh` | Chino |
| `StockSharp.Localization.de` | Alemán |
| `StockSharp.Localization.es` | Español |
| `StockSharp.Localization.ja` | Japonés |
| `StockSharp.Localization.ko` | Coreano |
| `StockSharp.Localization.All` | Todos los idiomas (metapaquete) |

También están disponibles: `ar`, `bn`, `ca`, `cs`, `da`, `el`, `fa`, `fi`, `fr`, `he`, `hi`, `hu`, `it`, `jv`, `ms`, `my`, `nl`, `no`, `pa`, `pl`, `pt`, `ro`, `sk`, `sr`, `sv`, `ta`, `th`, `tl`, `tr`, `uk`, `uz`, `vi`.

## Instalación de paquetes

### Mediante CLI (recomendado)

```bash
# Paquetes core
dotnet add package StockSharp.Algo
dotnet add package StockSharp.Algo.Strategies

# Conector (ejemplo — Binance)
dotnet add package StockSharp.Binance

# Indicadores
dotnet add package StockSharp.Algo.Indicators

# Backtesting
dotnet add package StockSharp.Algo.Testing

# Localización (ruso)
dotnet add package StockSharp.Localization.ru
```

### Mediante Visual Studio

1. Haga clic con el botón derecho en el proyecto → **Administrar paquetes NuGet...**
2. Busque `StockSharp`
3. Seleccione el paquete deseado → **Instalar**

Todas las dependencias se instalan automáticamente.

### Mediante JetBrains Rider

1. Haga clic con el botón derecho en el proyecto → **Administrar paquetes NuGet**
2. Busque `StockSharp`
3. Seleccione el paquete → **Instalar**

### Mediante la Consola del Administrador de paquetes (Visual Studio)

```powershell
Install-Package StockSharp.Algo
Install-Package StockSharp.Binance
Install-Package StockSharp.Algo.Strategies
```

## Servidor NuGet privado {#private-nuget-server}

Algunos componentes (conectores cripto, etc.) solo están disponibles mediante el servidor NuGet privado para usuarios registrados.

### Método 1: autenticación mediante token en la URL

1. Regístrese en el sitio web de StockSharp.
2. Copie el token desde su [cuenta personal](https://stocksharp.ru/profile/).
3. Agregue el origen de paquetes:

**CLI:**

```bash
dotnet nuget add source "https://nuget.stocksharp.com/{YOUR_TOKEN}/v3/index.json" --name StockSharpPrivate
```

**Visual Studio:** abra **Herramientas → Opciones → Administrador de paquetes NuGet → Orígenes de paquetes** y agregue un nuevo origen con la URL `https://nuget.stocksharp.com/{YOUR_TOKEN}/v3/index.json`.

**Rider:** abra **Configuración → Compilación, ejecución, implementación → NuGet → Orígenes** y agregue el origen.

### Método 2: autenticación mediante usuario y contraseña

1. Agregue un origen de paquetes con la URL `https://nuget.stocksharp.com/x/v3/index.json`
2. Cuando intente usar este origen, aparecerá una solicitud de inicio de sesión
3. Introduzca las credenciales de su cuenta StockSharp (o `x` como usuario y su token como contraseña)

**CLI:**

```bash
dotnet nuget add source "https://nuget.stocksharp.com/x/v3/index.json" --name StockSharpPrivate --username YOUR_LOGIN --password YOUR_PASSWORD --store-password-in-clear-text
```

> [!TIP]
> Para restablecer las credenciales guardadas en Windows, abra **Panel de control → Cuentas de usuario → Administrador de credenciales** y elimine las entradas relacionadas con `nuget.stocksharp.com`.

## Actualización de paquetes

### CLI

```bash
# Comprobar actualizaciones disponibles
dotnet list package --outdated

# Actualizar un paquete específico
dotnet add package StockSharp.Algo
```

### Visual Studio

1. **Administrar paquetes NuGet...** → pestaña **Actualizaciones**
2. Seleccione paquetes → **Actualizar**

### Rider

1. **Administrar paquetes NuGet** → pestaña **Actualizaciones**
2. Seleccione paquetes → **Actualizar**

## Solución de problemas

### Incompatibilidad de versiones

Todos los paquetes StockSharp deben tener la misma versión. Si encuentra errores de tipos o métodos, asegúrese de que todos los paquetes `StockSharp.*` estén actualizados a la misma versión.

### Paquete no encontrado en NuGet público

Algunos conectores solo están disponibles mediante el [servidor privado](#private-nuget-server). Asegúrese de que se haya agregado el origen correcto.

### Problemas GUI en sistemas no Windows

Los componentes GUI (`StockSharp.Xaml`, `StockSharp.Xaml.Charting`) funcionan solo en Windows. En Linux/macOS, use aplicaciones de consola sin paquetes GUI.

### Errores de restauración de paquetes

```bash
# Limpiar caché de NuGet
dotnet nuget locals all --clear

# Reintentar restauración
dotnet restore
```

## Estructura de archivo del proyecto (.csproj)

### Mínimo (bot de trading de consola)

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

### Extendido (estrategia con indicadores y pruebas)

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <!-- Núcleo -->
    <PackageReference Include="StockSharp.Algo" Version="*" />
    <PackageReference Include="StockSharp.Configuration" Version="*" />

    <!-- Estrategias e indicadores -->
    <PackageReference Include="StockSharp.Algo.Strategies" Version="*" />
    <PackageReference Include="StockSharp.Algo.Indicators" Version="*" />

    <!-- Conector -->
    <PackageReference Include="StockSharp.Binance" Version="*" />

    <!-- Pruebas retrospectivas -->
    <PackageReference Include="StockSharp.Algo.Testing" Version="*" />

    <!-- Localización -->
    <PackageReference Include="StockSharp.Localization.ru" Version="*" />
  </ItemGroup>
</Project>
```

### Aplicación WPF con gráficos

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

## Verificación de la instalación

Cree una aplicación mínima:

```csharp
using StockSharp.Algo;
using StockSharp.BusinessEntities;

Console.WriteLine("StockSharp configurado correctamente!");

var connector = new Connector();
Console.WriteLine($"Conector creado: {connector}");
```

```bash
dotnet run
```

## Ejemplos

Los ejemplos preparados de uso de StockSharp están disponibles en el directorio [Samples/](https://github.com/stocksharp/stocksharp/tree/master/Samples) del repositorio. Cubren conexión a exchanges, suscripción a datos, construcción de velas, indicadores, estrategias y pruebas.
