# Настройка окружения

## Требования

Для работы со StockSharp необходимы:

- **.NET 10** (SDK и Runtime)
- **Visual Studio 2022+** (рекомендуется последняя версия) или **JetBrains Rider**
- **NuGet** -- менеджер пакетов (встроен в Visual Studio)

## Установка .NET 10

Скачайте и установите .NET 10 SDK с официального сайта Microsoft:

```
https://dotnet.microsoft.com/download/dotnet/10.0
```

Убедитесь, что SDK установлен корректно:

```bash
dotnet --version
```

## Создание проекта

### Через Visual Studio 2022+

1. Откройте Visual Studio 2022 (или новее)
2. Создайте новый проект: **File -> New -> Project**
3. Выберите шаблон **Console App** или **WPF Application**
4. Убедитесь, что выбран целевой фреймворк **.NET 10**

### Через командную строку

```bash
dotnet new console -n MyTradingApp --framework net10.0
cd MyTradingApp
```

## Подключение NuGet-пакетов

StockSharp распространяется через NuGet. Добавьте необходимые пакеты в проект:

```bash
# Основной пакет с алгоритмами
dotnet add package StockSharp.Algo

# Коннектор (пример -- Binance)
dotnet add package StockSharp.Binance

# Для хранения рыночных данных
dotnet add package StockSharp.Algo.Storages
```

Или через **Package Manager Console** в Visual Studio:

```powershell
Install-Package StockSharp.Algo
Install-Package StockSharp.Binance
```

## Структура проекта

Минимальный файл проекта (`.csproj`) для работы со StockSharp:

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

## Проверка установки

Создайте минимальное приложение для проверки того, что все настроено корректно:

```csharp
using StockSharp.Algo;
using StockSharp.BusinessEntities;

Console.WriteLine("StockSharp successfully configured!");

var connector = new Connector();
Console.WriteLine($"Connector created: {connector}");
```

Скомпилируйте и запустите:

```bash
dotnet run
```

## Примеры

Готовые примеры использования StockSharp доступны в каталоге `Samples/` репозитория. Они охватывают подключение к биржам, подписку на данные, построение свечей, индикаторы, стратегии и тестирование.
