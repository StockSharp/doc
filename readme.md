# StockSharp Documentation Hub

## Documentation Links

| Language | URL |
| --- | --- |
| English | https://doc.stocksharp.com/en |
| Русский | https://doc.stocksharp.com/ru |
| 中文 | https://doc.stocksharp.com/zh |
| Deutsch | https://doc.stocksharp.com/de |
| Español | https://doc.stocksharp.com/es |
| 日本語 | https://doc.stocksharp.com/ja |
| Português | https://doc.stocksharp.com/pt |

## English

This repository contains the source Markdown files for the StockSharp documentation site. The ASP.NET Core documentation app in `D:\stocksharp\web\Docs` renders the site dynamically from this repository.

The site uses `doc.stocksharp.com` as the canonical host. The first URL segment selects the documentation language, for example `/en/...`, `/ru/...`, `/zh/...`, `/de/...`, `/es/...`, `/ja/...`, and `/pt/...`.

Run the documentation validation tests locally:

```powershell
dotnet restore tests\StockSharp.Doc.Tests.csproj
dotnet test tests\StockSharp.Doc.Tests.csproj --configuration Release --no-restore --logger "console;verbosity=normal"
```

The GitHub Actions workflow is `.github/workflows/docs-validation.yml`.

## Русский

Этот репозиторий содержит исходные Markdown-файлы документации StockSharp. ASP.NET Core приложение документации из `D:\stocksharp\web\Docs` динамически рендерит сайт из этого репозитория.

Канонический хост сайта - `doc.stocksharp.com`. Первый сегмент URL выбирает язык документации: `/en/...`, `/ru/...`, `/zh/...`, `/de/...`, `/es/...`, `/ja/...`, `/pt/...`.

Локальный запуск проверки документации:

```powershell
dotnet restore tests\StockSharp.Doc.Tests.csproj
dotnet test tests\StockSharp.Doc.Tests.csproj --configuration Release --no-restore --logger "console;verbosity=normal"
```

GitHub Actions workflow находится в `.github/workflows/docs-validation.yml`.

## 中文

此仓库包含 StockSharp 文档站点的 Markdown 源文件。`D:\stocksharp\web\Docs` 中的 ASP.NET Core 文档应用会从此仓库动态渲染站点。

站点使用 `doc.stocksharp.com` 作为规范主机。URL 的第一个路径段用于选择文档语言，例如 `/en/...`、`/ru/...`、`/zh/...`、`/de/...`、`/es/...`、`/ja/...`、`/pt/...`。

在本地运行文档验证测试：

```powershell
dotnet restore tests\StockSharp.Doc.Tests.csproj
dotnet test tests\StockSharp.Doc.Tests.csproj --configuration Release --no-restore --logger "console;verbosity=normal"
```

GitHub Actions workflow 位于 `.github/workflows/docs-validation.yml`。

## Deutsch

Dieses Repository enthält die Markdown-Quelldateien für die StockSharp-Dokumentationswebsite. Die ASP.NET Core-Dokumentationsanwendung in `D:\stocksharp\web\Docs` rendert die Website dynamisch aus diesem Repository.

Die Website verwendet `doc.stocksharp.com` als kanonischen Host. Das erste URL-Segment wählt die Sprache der Dokumentation aus, zum Beispiel `/en/...`, `/ru/...`, `/zh/...`, `/de/...`, `/es/...`, `/ja/...` und `/pt/...`.

Dokumentationsprüfungen lokal ausführen:

```powershell
dotnet restore tests\StockSharp.Doc.Tests.csproj
dotnet test tests\StockSharp.Doc.Tests.csproj --configuration Release --no-restore --logger "console;verbosity=normal"
```

Der GitHub Actions workflow liegt unter `.github/workflows/docs-validation.yml`.

## Español

Este repositorio contiene los archivos Markdown fuente del sitio de documentación de StockSharp. La aplicación de documentación ASP.NET Core en `D:\stocksharp\web\Docs` renderiza el sitio dinámicamente desde este repositorio.

El sitio usa `doc.stocksharp.com` como host canónico. El primer segmento de la URL selecciona el idioma de la documentación, por ejemplo `/en/...`, `/ru/...`, `/zh/...`, `/de/...`, `/es/...`, `/ja/...` y `/pt/...`.

Ejecutar las pruebas de validación de la documentación localmente:

```powershell
dotnet restore tests\StockSharp.Doc.Tests.csproj
dotnet test tests\StockSharp.Doc.Tests.csproj --configuration Release --no-restore --logger "console;verbosity=normal"
```

El workflow de GitHub Actions está en `.github/workflows/docs-validation.yml`.

## 日本語

このリポジトリには StockSharp ドキュメントサイトの Markdown ソースファイルが含まれています。`D:\stocksharp\web\Docs` の ASP.NET Core ドキュメントアプリが、このリポジトリからサイトを動的にレンダリングします。

サイトの正規ホストは `doc.stocksharp.com` です。URL の最初のセグメントでドキュメントの言語を選択します。例: `/en/...`、`/ru/...`、`/zh/...`、`/de/...`、`/es/...`、`/ja/...`、`/pt/...`。

ローカルでドキュメント検証テストを実行します:

```powershell
dotnet restore tests\StockSharp.Doc.Tests.csproj
dotnet test tests\StockSharp.Doc.Tests.csproj --configuration Release --no-restore --logger "console;verbosity=normal"
```

GitHub Actions workflow は `.github/workflows/docs-validation.yml` にあります。

## Português

Este repositório contém os arquivos Markdown de origem do site de documentação do StockSharp. O aplicativo de documentação ASP.NET Core em `D:\stocksharp\web\Docs` renderiza o site dinamicamente a partir deste repositório.

O site usa `doc.stocksharp.com` como host canônico. O primeiro segmento da URL seleciona o idioma da documentação, por exemplo `/en/...`, `/ru/...`, `/zh/...`, `/de/...`, `/es/...`, `/ja/...` e `/pt/...`.

Execute os testes de validação da documentação localmente:

```powershell
dotnet restore tests\StockSharp.Doc.Tests.csproj
dotnet test tests\StockSharp.Doc.Tests.csproj --configuration Release --no-restore --logger "console;verbosity=normal"
```

O workflow do GitHub Actions está em `.github/workflows/docs-validation.yml`.

## Repository Structure

Each localized documentation tree lives in a two-letter language folder: `en`, `ru`, `zh`, `de`, `es`, `ja`, and `pt`.

Every content language is expected to have:

- `index.md`
- `toc.yml`
- Markdown files under `topics/`

Shared UI localization files live under `i18n/`.

The validation checks cover language manifest consistency, required localized entry files, `toc.yml` references, duplicate TOC slugs, Markdown links, local assets, anchors, parser errors, code fences, and merge conflict markers.
