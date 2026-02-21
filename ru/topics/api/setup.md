# Настройка окружения

## Требования

Для работы со StockSharp необходимы:

- **.NET 10** (SDK и Runtime) — [скачать](https://dotnet.microsoft.com/download/dotnet/10.0)
- **IDE** — Visual Studio 2022+, JetBrains Rider или VS Code
- **NuGet** — менеджер пакетов (встроен в Visual Studio и Rider)

Убедитесь, что SDK установлен:

```bash
dotnet --version
```

## Создание проекта

### Visual Studio 2022+

1. **File → New → Project**
2. Выберите шаблон **Console App** (консольное приложение) или **WPF Application** (GUI-приложение)
3. Укажите целевой фреймворк **.NET 10**

### JetBrains Rider

1. **File → New Solution**
2. Выберите **.NET / .NET Core → Console Application**
3. Укажите **Target Framework: net10.0**

### Командная строка (CLI)

```bash
# Консольное приложение
dotnet new console -n MyTradingApp --framework net10.0
cd MyTradingApp

# WPF-приложение (только Windows)
dotnet new wpf -n MyTradingGui --framework net10.0-windows
```

## Каталог NuGet-пакетов

StockSharp распространяется через NuGet. Ниже — полный каталог пакетов по категориям.

### Ядро

| Пакет | Описание |
|-------|----------|
| [StockSharp.Messages](https://www.nuget.org/packages/StockSharp.Messages/) | Базовые сообщения и контракты. Основа всего фреймворка |
| [StockSharp.BusinessEntities](https://www.nuget.org/packages/StockSharp.BusinessEntities/) | Торговые сущности: Security, Order, Trade, Portfolio и др. |
| [StockSharp.Algo](https://www.nuget.org/packages/StockSharp.Algo/) | Ядро алгоритмической торговли, Connector, подписки, свечи |
| [StockSharp.Configuration](https://www.nuget.org/packages/StockSharp.Configuration/) | Управление конфигурацией, настройка подключений |
| [StockSharp.Localization](https://www.nuget.org/packages/StockSharp.Localization/) | Система локализации (английский по умолчанию) |

### Стратегии и индикаторы

| Пакет | Описание |
|-------|----------|
| [StockSharp.Algo.Strategies](https://www.nuget.org/packages/StockSharp.Algo.Strategies/) | Фреймворк стратегий — базовый класс Strategy, позиции, PnL |
| [StockSharp.Algo.Indicators](https://www.nuget.org/packages/StockSharp.Algo.Indicators/) | 100+ технических индикаторов (SMA, EMA, RSI, MACD, Bollinger и др.) |

### Тестирование

| Пакет | Описание |
|-------|----------|
| [StockSharp.Algo.Testing](https://www.nuget.org/packages/StockSharp.Algo.Testing/) | Бэктестирование на исторических данных, эмуляция торговли |

### Хранение и данные

| Пакет | Описание |
|-------|----------|
| [StockSharp.Algo.Export](https://www.nuget.org/packages/StockSharp.Algo.Export/) | Экспорт рыночных данных (CSV, Excel, JSON и др.) |
| [StockSharp.Algo.Import](https://www.nuget.org/packages/StockSharp.Algo.Import/) | Импорт данных из внешних форматов |

### Аналитика и расчёты

| Пакет | Описание |
|-------|----------|
| [StockSharp.Algo.Analytics](https://www.nuget.org/packages/StockSharp.Algo.Analytics/) | Интерфейсы для аналитических скриптов |
| [StockSharp.Algo.Compilation](https://www.nuget.org/packages/StockSharp.Algo.Compilation/) | Компиляция кода во время выполнения |
| [StockSharp.Algo.Gpu](https://www.nuget.org/packages/StockSharp.Algo.Gpu/) | GPU-ускоренные вычисления индикаторов (CUDA через ILGPU) |

### GUI-компоненты (только Windows)

| Пакет | Описание |
|-------|----------|
| [StockSharp.Xaml](https://www.nuget.org/packages/StockSharp.Xaml/) | WPF-контролы: таблицы инструментов, портфелей, заявок |
| [StockSharp.Xaml.Charting](https://www.nuget.org/packages/StockSharp.Xaml.Charting/) | Графики свечей, индикаторов, эквити |
| [StockSharp.Charting.Interfaces](https://www.nuget.org/packages/StockSharp.Charting.Interfaces/) | Интерфейсы графических компонентов |
| [StockSharp.Alerts.Interfaces](https://www.nuget.org/packages/StockSharp.Alerts.Interfaces/) | Интерфейсы системы оповещений |
| [StockSharp.Diagram.Core](https://www.nuget.org/packages/StockSharp.Diagram.Core/) | Ядро визуального дизайнера стратегий |

### Коннекторы (биржи и брокеры)

Каждый коннектор — отдельный NuGet-пакет. Основные:

| Пакет | Биржа/Брокер |
|-------|-------------|
| `StockSharp.Binance` | Binance |
| `StockSharp.InteractiveBrokers` | Interactive Brokers |
| `StockSharp.Fix` | FIX-протокол (универсальный) |
| `StockSharp.Connectors.Tinkoff` | Tinkoff Invest |
| `StockSharp.Connectors.Coinbase` | Coinbase |
| `StockSharp.Connectors.BitStamp` | Bitstamp |
| `StockSharp.Connectors.Bittrex` | Bittrex |
| `StockSharp.Finam` | Финам |

> [!NOTE]
> Полный список коннекторов — в разделе [Коннекторы](connectors.md). Некоторые коннекторы доступны только через [приватный NuGet-сервер](#приватный-nuget-сервер).

### Локализация

Базовый пакет `StockSharp.Localization` включает английский язык. Дополнительные языки устанавливаются отдельными пакетами:

| Пакет | Язык |
|-------|------|
| `StockSharp.Localization.ru` | Русский |
| `StockSharp.Localization.zh` | Китайский |
| `StockSharp.Localization.de` | Немецкий |
| `StockSharp.Localization.es` | Испанский |
| `StockSharp.Localization.ja` | Японский |
| `StockSharp.Localization.ko` | Корейский |
| `StockSharp.Localization.All` | Все языки (мета-пакет) |

Доступны также: `ar`, `bn`, `ca`, `cs`, `da`, `el`, `fa`, `fi`, `fr`, `he`, `hi`, `hu`, `it`, `jv`, `ms`, `my`, `nl`, `no`, `pa`, `pl`, `pt`, `ro`, `sk`, `sr`, `sv`, `ta`, `th`, `tl`, `tr`, `uk`, `uz`, `vi`.

## Установка пакетов

### Через CLI (рекомендуется)

```bash
# Основные пакеты
dotnet add package StockSharp.Algo
dotnet add package StockSharp.Algo.Strategies

# Коннектор (пример — Binance)
dotnet add package StockSharp.Binance

# Индикаторы
dotnet add package StockSharp.Algo.Indicators

# Бэктестирование
dotnet add package StockSharp.Algo.Testing

# Локализация (русский)
dotnet add package StockSharp.Localization.ru
```

### Через Visual Studio

1. Правый клик на проекте → **Manage NuGet Packages...**
2. В поиске введите `StockSharp`
3. Выберите нужный пакет → **Install**

Все зависимости установятся автоматически.

### Через JetBrains Rider

1. Правый клик на проекте → **Manage NuGet Packages**
2. В поиске введите `StockSharp`
3. Выберите пакет → **Install**

### Через Package Manager Console (Visual Studio)

```powershell
Install-Package StockSharp.Algo
Install-Package StockSharp.Binance
Install-Package StockSharp.Algo.Strategies
```

## Приватный NuGet-сервер

Некоторые компоненты (криптовалютные коннекторы и др.) доступны только через приватный NuGet-сервер для зарегистрированных пользователей.

### Способ 1: Аутентификация через токен в адресе

1. Зарегистрируйтесь на сайте StockSharp.
2. Скопируйте токен из [личного кабинета](https://stocksharp.ru/profile/).
3. Добавьте источник пакетов:

**CLI:**

```bash
dotnet nuget add source "https://nuget.stocksharp.com/{ВАШ_ТОКЕН}/v3/index.json" --name StockSharpPrivate
```

**Visual Studio:** откройте **Tools → Options → NuGet Package Manager → Package Sources** и добавьте новый источник с адресом `https://nuget.stocksharp.com/{ВАШ_ТОКЕН}/v3/index.json`.

**Rider:** откройте **Settings → Build, Execution, Deployment → NuGet → Sources** и добавьте источник.

### Способ 2: Аутентификация через логин и пароль

1. Добавьте источник с адресом `https://nuget.stocksharp.com/x/v3/index.json`
2. При попытке использования появится окно ввода логина/пароля
3. Введите данные учётной записи StockSharp (или `x` в поле логина и токен в поле пароля)

**CLI:**

```bash
dotnet nuget add source "https://nuget.stocksharp.com/x/v3/index.json" --name StockSharpPrivate --username ВАШ_ЛОГИН --password ВАШ_ПАРОЛЬ --store-password-in-clear-text
```

> [!TIP]
> Чтобы сбросить сохранённые учётные данные в Windows, откройте **Панель управления → Учётные записи пользователей → Диспетчер учётных данных** и удалите записи, связанные с `nuget.stocksharp.com`.

## Обновление пакетов

### CLI

```bash
# Проверить доступные обновления
dotnet list package --outdated

# Обновить конкретный пакет
dotnet add package StockSharp.Algo
```

### Visual Studio

1. **Manage NuGet Packages...** → вкладка **Updates**
2. Выберите пакеты → **Update**

### Rider

1. **Manage NuGet Packages** → вкладка **Upgrades**
2. Выберите пакеты → **Upgrade**

## Типичные проблемы

### Несовместимость версий

Все пакеты StockSharp должны быть одной версии. Если возникают ошибки типов или методов, убедитесь что все `StockSharp.*` пакеты обновлены до одной версии.

### Отсутствие пакета в публичном NuGet

Некоторые коннекторы доступны только через [приватный сервер](#приватный-nuget-сервер). Убедитесь, что добавлен правильный источник.

### Проблемы с GUI на не-Windows

GUI-компоненты (`StockSharp.Xaml`, `StockSharp.Xaml.Charting`) работают только на Windows. На Linux/macOS используйте консольные приложения без GUI-пакетов.

### Ошибки восстановления пакетов

```bash
# Очистить кэш NuGet
dotnet nuget locals all --clear

# Повторить восстановление
dotnet restore
```

## Структура .csproj

### Минимальная (консольный робот)

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

### Расширенная (стратегия с индикаторами и тестированием)

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <!-- Ядро -->
    <PackageReference Include="StockSharp.Algo" Version="*" />
    <PackageReference Include="StockSharp.Configuration" Version="*" />

    <!-- Стратегии и индикаторы -->
    <PackageReference Include="StockSharp.Algo.Strategies" Version="*" />
    <PackageReference Include="StockSharp.Algo.Indicators" Version="*" />

    <!-- Коннектор -->
    <PackageReference Include="StockSharp.Binance" Version="*" />

    <!-- Бэктестирование -->
    <PackageReference Include="StockSharp.Algo.Testing" Version="*" />

    <!-- Локализация -->
    <PackageReference Include="StockSharp.Localization.ru" Version="*" />
  </ItemGroup>
</Project>
```

### WPF-приложение с графиками

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

## Проверка установки

Создайте минимальное приложение:

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

## Примеры

Готовые примеры использования StockSharp доступны в каталоге [Samples/](https://github.com/stocksharp/stocksharp/tree/master/Samples) репозитория. Они охватывают подключение к биржам, подписку на данные, построение свечей, индикаторы, стратегии и тестирование.
