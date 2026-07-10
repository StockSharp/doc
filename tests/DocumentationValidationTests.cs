using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;

using Ecng.UnitTesting;

using Markdig.Extensions.Tables;
using Markdig;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace StockSharp.Doc.Tests;

[TestClass]
public sealed class DocumentationValidationTests : BaseTestClass
{
	private const string DefaultLanguage = "en";
	private const int MaxReportedErrors = 200;
	private const string RussianSpecificPlaceholderMarker = "available only in the Russian version";
	private const string LocalizedAuditReportEnvironmentVariable = "DOC_WRITE_LOCALIZED_AUDIT_REPORT";
	private const string LocalizedAuditReportFileName = "LOCALIZED_AUDIT_REPORT.md";

	private static readonly string _repoRoot = FindRepoRoot();

	private static readonly string[] _textFileExtensions =
	[
		".json",
		".md",
		".txt",
		".yml",
		".yaml",
	];

	private static readonly string[] _mojibakeMarkers =
	[
		// UTF-8 punctuation decoded as Windows-1251.
		"\u0432\u201E", // в„
		"\u0432\u20AC", // в€
		"\u0432\u0402", // вЂ
		"\u0413\u2014", // Г—
		"\u0412\u0406", // ВІ
		"\u0412\u00B2", // В²
		"\u0412\u00B0", // В°
		"\u0412\u00AB", // В«
		"\u0412\u00BB", // В»
		"\u0412\u00B1", // В±
		"\u0412\u00B7", // В·

		// UTF-8 punctuation decoded as Latin-1.
		"\u00E2\u20AC", // â€
		"\u00E2\u201E", // â„
		"\u00E2\u02C6", // âˆ

		// UTF-8 accented Latin text decoded as Latin-1.
		// Do not check single Ã/Â characters: they are valid in words like NÃO and Ângulo.
		"\u00C3\u00A1", // Ã¡
		"\u00C3\u00A9", // Ã©
		"\u00C3\u00AD", // Ãí
		"\u00C3\u00B3", // Ã³
		"\u00C3\u00BA", // Ãº
		"\u00C3\u00B1", // Ãñ
		"\u00C3\u00A3", // Ã£
		"\u00C3\u00A7", // Ã§
		"\u00C3\u00BC", // Ãü
		"\u00C3\u00B6", // Ã¶
		"\u00C3\u00A4", // Ãä

		// UTF-8 Cyrillic decoded as Latin-1.
		"\u00D0\u00B0", // Ð°
		"\u00D0\u00B1", // Ð±
		"\u00D0\u00B2", // Ð²
		"\u00D0\u00B3", // Ð³
		"\u00D0\u00B4", // Ð´
		"\u00D0\u00B5", // Ðµ
		"\u00D0\u00B8", // Ð¸
		"\u00D0\u00B9", // Ð¹
		"\u00D0\u00BA", // Ðº
		"\u00D0\u00BB", // Ð»
		"\u00D0\u00BC", // Ð¼
		"\u00D0\u00BD", // Ð½
		"\u00D0\u00BE", // Ð¾
		"\u00D0\u00BF", // Ð¿
		"\u00D1\u0080", // Ñ€
		"\u00D1\u0081", // Ñ
		"\u00D1\u0082", // Ñ‚
		"\u00D1\u0083", // Ñƒ
		"\u00D1\u0087", // Ñ‡
		"\u00D1\u0088", // Ñˆ
		"\u00D1\u008F", // Ñ
	];

	private static readonly string[] _knownEnglishUiPhrases =
	[
		"Add button",
		"Add Designer strategy",
		"Apply changes",
		"Cloud panel",
		"Connect button",
		"Control Panel → User Accounts → Credential Manager",
		"File → New Solution",
		"File → New → Project",
		".NET / .NET Core → Console Application",
		"Manage NuGet Packages",
		"More info",
		"Open debug launch profiles UI",
		"Remote Manager",
		"Remote mode",
		"Run anyway",
		"send command",
		"Settings → Build, Execution, Deployment → NuGet → Sources",
		"Solution Explorer",
		"Tools → Options → NuGet Package Manager → Package Sources",
	];

	private static readonly (string Name, string Pattern)[] _knownEnglishCodeOutputPatterns =
	[
		("subscription lifecycle output", @"\bSubscription (?:started|completed|interrupted|online|switched to real-time mode)\b"),
		("adapter connection output", @"\bAdapter (?:connected|disconnected|connection error)\b"),
		("round-trip output", @"\bPosition closed:|\bMax volume:"),
		("ShrinkPrice output", @"\bOrder price:|\bOriginal price:|\bAfter ShrinkPrice:"),
		("tick price output", @"\bTick:.*\bPrice:"),
		("order book output", @"\bOrder Book:|\bBest Bid\b|\bBest Ask\b|\bMiddle of Spread\b|\bBid Price:|\bAsk Price:"),
		("field/value output", @"\bField:|\bValue:|\bBids:|\bAsks:"),
	];

	private static readonly HashSet<string> _translatableEnglishCodeOutputWords = new(StringComparer.OrdinalIgnoreCase)
	{
		"absolute",
		"accepted",
		"adapter",
		"adapters",
		"buy",
		"canceled",
		"cancelled",
		"candle",
		"candles",
		"commission",
		"completed",
		"configured",
		"connection",
		"connector",
		"created",
		"data",
		"delta",
		"direction",
		"error",
		"exchange",
		"exit",
		"failed",
		"found",
		"greater",
		"instrument",
		"instruments",
		"invalid",
		"large",
		"less",
		"loaded",
		"lost",
		"message",
		"mode",
		"not",
		"online",
		"operation",
		"order",
		"orders",
		"percentage",
		"price",
		"profit",
		"received",
		"register",
		"registration",
		"registered",
		"rule",
		"search",
		"sell",
		"signal",
		"spread",
		"successfully",
		"threshold",
		"trade",
		"transitioned",
		"type",
		"unsupported",
		"value",
		"values",
		"volume",
	};

	private static readonly HashSet<string> _allowedInvariantCodeOutputWords = new(StringComparer.OrdinalIgnoreCase)
	{
		"api",
		"csv",
		"delta",
		"fast",
		"fix",
		"http",
		"https",
		"json",
		"pnl",
		"rest",
		"signal",
		"signals",
		"sma",
		"stocksharp",
		"tcp",
		"udp",
		"ui",
		"xml",
	};

	private static readonly string[] _knownBrokenGermanEncodingFragments =
	[
		"anschlie?end",
		"?bertragen",
		"?bertragung",
		"?bersicht",
		"f?r",
		"m?glich",
		"w?hrend",
	];

	private static readonly HashSet<string> _allowedInvariantHeadingTexts = new(StringComparer.Ordinal)
	{
		"Backtesting/Emulation",
		"Backup",
		"Basket",
		"Black-Scholes",
		"Chart",
		"Debugging",
		"Designer",
		"Emulation",
		"Export",
		"Flag",
		"Hedging",
		"Hydra",
		"Identifier *@ALL",
		"Identifier \\*@ALL",
		"Import",
		"Index",
		"Indexer",
		"Installer",
		"Interface",
		"Level 1",
		"Level1",
		"MATLAB",
		"Orders",
		"Portfolios",
		"Position",
		"RemoteManager",
		"Runner",
		"Shell",
		"Simulator",
		"Strikes",
		"Terminal",
		"Ticks",
		"Trades",
		"UDP Dumper",
		"Variable",
	};

	private static readonly HashSet<string> _allowedInvariantIndicatorHeadingPaths = new(StringComparer.OrdinalIgnoreCase)
	{
		"topics/api/indicators/list_of_indicators/aroon_oscillator.md",
		"topics/api/indicators/list_of_indicators/bear_power.md",
		"topics/api/indicators/list_of_indicators/bollinger_bands.md",
		"topics/api/indicators/list_of_indicators/bull_power.md",
		"topics/api/indicators/list_of_indicators/elder_force_index.md",
		"topics/api/indicators/list_of_indicators/elder_ray.md",
		"topics/api/indicators/list_of_indicators/gator_oscillator.md",
		"topics/api/indicators/list_of_indicators/linear_regression_forecast.md",
		"topics/api/indicators/list_of_indicators/lrs.md",
		"topics/api/indicators/list_of_indicators/market_facilitation_index.md",
		"topics/api/indicators/list_of_indicators/mean_deviation.md",
		"topics/api/indicators/list_of_indicators/median.md",
		"topics/api/indicators/list_of_indicators/money_flow_index.md",
		"topics/api/indicators/list_of_indicators/optimal_tracking.md",
		"topics/api/indicators/list_of_indicators/parabolic_sar.md",
		"topics/api/indicators/list_of_indicators/pass_through.md",
		"topics/api/indicators/list_of_indicators/price_channels.md",
		"topics/api/indicators/list_of_indicators/rank_correlation_index.md",
		"topics/api/indicators/list_of_indicators/smoothed_ma.md",
		"topics/api/indicators/list_of_indicators/standard_deviation.md",
		"topics/api/indicators/list_of_indicators/standard_error.md",
		"topics/api/indicators/list_of_indicators/stochastic_oscillator.md",
		"topics/api/indicators/list_of_indicators/sum_n.md",
		"topics/api/indicators/list_of_indicators/true_range.md",
		"topics/api/indicators/list_of_indicators/true_strength_index.md",
		"topics/api/indicators/list_of_indicators/variable_moving_average.md",
		"topics/api/indicators/list_of_indicators/weighted_ma.md",
		"topics/api/indicators/list_of_indicators/wilder_ma.md",
	};

	private static readonly HashSet<string> _allowedInvariantLinkLabels = new(StringComparer.Ordinal)
	{
		"ALF",
		"ALMA",
		"Aroon",
		"ATR",
		"DEMA",
		"DeMarker",
		"Ichimoku",
		"KAMA",
		"Peak",
		"QStick",
		"RAVI",
		"RSI",
		"SuperTrend",
		"TRIX",
		"TWAP",
		"VIDYA",
		"VWAP",
		"Wilder MA",
		"ZLEMA",
	};

	private static readonly MarkdownPipeline _markdown = new MarkdownPipelineBuilder()
		.UseAdvancedExtensions()
		.UseAutoIdentifiers()
		.Build();

	private static readonly IDeserializer _yaml = new DeserializerBuilder()
		.WithNamingConvention(CamelCaseNamingConvention.Instance)
		.IgnoreUnmatchedProperties()
		.Build();

	private static readonly JsonSerializerOptions _json = new()
	{
		PropertyNameCaseInsensitive = true,
		ReadCommentHandling = JsonCommentHandling.Skip,
		AllowTrailingCommas = true,
	};

	[TestMethod]
	public void LocalizationMetadataAndStringsAreConsistent()
	{
		var errors = new List<string>();

		if (Directory.Exists(Path.Combine(_repoRoot, "i18n")))
			errors.Add("i18n folder is obsolete. Keep language metadata, strings, and flags under each language folder.");

		var languageDirs = Directory.EnumerateDirectories(_repoRoot)
			.Select(path => new { Path = path, Code = Path.GetFileName(path) })
			.Where(item => item.Code is not null && Regex.IsMatch(item.Code, "^[a-z]{2}$", RegexOptions.CultureInvariant))
			.OrderBy(item => item.Code, StringComparer.OrdinalIgnoreCase)
			.ToArray();

		var codes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		var orders = new Dictionary<int, string>();
		var defaults = 0;
		var metadataCount = 0;

		foreach (var dir in languageDirs)
		{
			var code = dir.Code;
			var metadataPath = Path.Combine(dir.Path, "language.json");

			if (!File.Exists(metadataPath))
			{
				if (HasContent(code))
					errors.Add($"{code}/language.json is missing.");

				continue;
			}

			metadataCount++;

			LanguageEntry entry;
			try
			{
				entry = JsonSerializer.Deserialize<LanguageEntry>(ReadAllText(metadataPath), _json);
				if (entry == null)
				{
					errors.Add($"{code}/language.json must contain a JSON object.");
					continue;
				}
			}
			catch (Exception ex)
			{
				errors.Add($"{code}/language.json is invalid JSON: {ex.Message}");
				continue;
			}

			var declaredCode = (entry.Code ?? string.Empty).Trim();

			if (!Regex.IsMatch(declaredCode, "^[a-z]{2}$", RegexOptions.CultureInvariant))
				errors.Add($"{code}/language.json has invalid language code '{entry.Code}'. Expected two lowercase ISO letters.");
			else if (!declaredCode.Equals(code, StringComparison.OrdinalIgnoreCase))
				errors.Add($"{code}/language.json code '{declaredCode}' must match its folder name.");
			else if (!codes.Add(declaredCode))
				errors.Add($"{code}/language.json has duplicate language code '{declaredCode}'.");

			if (string.IsNullOrWhiteSpace(entry.Name))
				errors.Add($"{code}/language.json has empty name.");

			if (entry.Order == int.MaxValue)
				errors.Add($"{code}/language.json must declare order.");
			else if (entry.Order < 0)
				errors.Add($"{code}/language.json order must be non-negative.");
			else if (orders.TryGetValue(entry.Order, out var previous))
				errors.Add($"{code}/language.json order {entry.Order} duplicates {previous}/language.json.");
			else
				orders[entry.Order] = code;

			if (entry.IsDefault)
				defaults++;

			ValidateLanguageFlag(code, dir.Path, entry.Flag, errors);
			ValidateLanguageStrings(code, Path.Combine(dir.Path, "strings.json"), errors);

			if (HasContent(code))
				ValidateContentLanguageEntryFiles(code, dir.Path, errors);
		}

		if (metadataCount == 0)
			errors.Add("At least one language.json file must be present.");

		if (defaults != 1)
			errors.Add($"Exactly one language.json file must declare default=true, found {defaults}.");

		if (!codes.Contains(DefaultLanguage))
			errors.Add($"Default content language '{DefaultLanguage}' must have language.json.");

		foreach (var lang in GetContentLanguages())
		{
			if (!codes.Contains(lang))
				errors.Add($"Content folder '{lang}' has no matching language.json.");
		}

		AssertNoErrors(errors);
	}

	private static void ValidateLanguageFlag(string code, string langRoot, string flagPath, List<string> errors)
	{
		if (string.IsNullOrWhiteSpace(flagPath))
		{
			errors.Add($"{code}/language.json has empty flag.");
			return;
		}

		var normalized = flagPath.Replace('\\', '/');
		if (normalized.Contains('/', StringComparison.Ordinal))
		{
			errors.Add($"{code}/language.json flag must be a file name inside the language folder.");
			return;
		}

		var flag = ResolveExistingPath(langRoot, normalized);
		if (!flag.Exists)
			errors.Add($"{code}/language.json references missing flag '{flagPath}'.");
		else if (!flag.ExactCase)
			errors.Add($"{code}/language.json references flag '{flagPath}' with wrong case; actual path is '{code}/{flag.ActualRelativePath}'.");
	}

	private static void ValidateLanguageStrings(string code, string stringsPath, List<string> errors)
	{
		if (!File.Exists(stringsPath))
		{
			errors.Add($"{code}/strings.json is missing.");
			return;
		}

		try
		{
			var strings = JsonSerializer.Deserialize<Dictionary<string, string>>(ReadAllText(stringsPath), _json);
			if (strings is null)
			{
				errors.Add($"{code}/strings.json must contain a JSON object.");
				return;
			}

			foreach (var (key, value) in strings)
			{
				if (string.IsNullOrWhiteSpace(key))
					errors.Add($"{code}/strings.json contains an empty localization key.");
				if (value is null)
					errors.Add($"{code}/strings.json key '{key}' has null value.");
			}
		}
		catch (Exception ex)
		{
			errors.Add($"{code}/strings.json is invalid JSON: {ex.Message}");
		}
	}

	private static void ValidateContentLanguageEntryFiles(string code, string langRoot, List<string> errors)
	{
		if (!File.Exists(Path.Combine(langRoot, "index.md")))
			errors.Add($"{code}/index.md is missing.");
		if (!File.Exists(Path.Combine(langRoot, "toc.yml")))
			errors.Add($"{code}/toc.yml is missing.");
	}

	[TestMethod]
	public void TocFilesReferenceExistingMarkdown()
	{
		var errors = new List<string>();

		foreach (var lang in GetContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);
			var tocPath = Path.Combine(langRoot, "toc.yml");
			var slugs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			ValidateTocFile(lang, langRoot, tocPath, errors, slugs, new HashSet<string>(StringComparer.OrdinalIgnoreCase));
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void ContentFileAndDirectoryNamesUseLowercase()
	{
		var errors = new List<string>();

		foreach (var lang in GetContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var path in Directory.EnumerateFileSystemEntries(langRoot, "*", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var relative = Path.GetRelativePath(langRoot, path).Replace('\\', '/');
				if (!relative.Any(char.IsUpper))
					continue;

				errors.Add($"{RelativeToRepo(path)}: content file and directory names must be lowercase.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void RussianSpecificTopicsStayPlaceholdersOutsideRussian()
	{
		var errors = new List<string>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);
		var languages = GetContentLanguages()
			.Where(lang => !lang.Equals("ru", StringComparison.OrdinalIgnoreCase))
			.ToArray();

		foreach (var defaultFile in Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			if (!IsRussianSpecificPlaceholder(ReadAllText(defaultFile)))
				continue;

			var relative = Path.GetRelativePath(defaultRoot, defaultFile).Replace('\\', '/');

			foreach (var lang in languages)
			{
				var langRoot = Path.Combine(_repoRoot, lang);
				var file = Path.Combine(langRoot, relative.Replace('/', Path.DirectorySeparatorChar));

				if (!File.Exists(file))
				{
					errors.Add($"{lang}/{relative}: Russian-specific placeholder page is missing.");
					continue;
				}

				ValidateRussianSpecificPlaceholderPage(file, ReadAllText(file), errors);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void MarkdownLinksAndLocalAssetsResolve()
	{
		var errors = new List<string>();

		foreach (var lang in GetContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				ValidateMarkdownFile(lang, langRoot, file, errors);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownAnchorReferencesMatchDefaultLanguage()
	{
		var errors = new List<string>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);

		foreach (var defaultFile in Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var relative = Path.GetRelativePath(defaultRoot, defaultFile).Replace('\\', '/');
			var expected = GetLocalAnchorReferences(defaultRoot, defaultFile);

			if (expected.Count == 0)
				continue;

			foreach (var lang in GetContentLanguages().Where(lang => !lang.Equals(DefaultLanguage, StringComparison.OrdinalIgnoreCase)))
			{
				var localizedRoot = Path.Combine(_repoRoot, lang);
				var localizedFile = Path.Combine(localizedRoot, relative.Replace('/', Path.DirectorySeparatorChar));

				if (!File.Exists(localizedFile))
					continue;

				var actual = GetLocalAnchorReferences(localizedRoot, localizedFile);
				if (!expected.SequenceEqual(actual, StringComparer.Ordinal))
					errors.Add($"{RelativeToRepo(localizedFile)}: local anchor references must keep the same stable fragments as {DefaultLanguage}/{relative}. Expected: {string.Join(", ", expected)}. Actual: {string.Join(", ", actual)}.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownStructureMatchesDefaultLanguage()
	{
		var errors = new List<string>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);
		var defaultFiles = Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories)
			.Order(StringComparer.OrdinalIgnoreCase)
			.ToArray();
		var defaultRelativeFiles = defaultFiles
			.Select(file => Path.GetRelativePath(defaultRoot, file).Replace('\\', '/'))
			.ToHashSet(StringComparer.OrdinalIgnoreCase);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);
			var localizedRelativeFiles = Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories)
				.Select(file => Path.GetRelativePath(langRoot, file).Replace('\\', '/'))
				.ToHashSet(StringComparer.OrdinalIgnoreCase);

			foreach (var relative in defaultRelativeFiles.Except(localizedRelativeFiles, StringComparer.OrdinalIgnoreCase).Order(StringComparer.OrdinalIgnoreCase))
				errors.Add($"{lang}/{relative}: localized Markdown file is missing.");

			foreach (var relative in localizedRelativeFiles.Except(defaultRelativeFiles, StringComparer.OrdinalIgnoreCase).Order(StringComparer.OrdinalIgnoreCase))
				errors.Add($"{lang}/{relative}: localized Markdown file has no matching {DefaultLanguage}/{relative} source file.");

			foreach (var defaultFile in defaultFiles)
			{
				var relative = Path.GetRelativePath(defaultRoot, defaultFile).Replace('\\', '/');
				var localizedFile = Path.Combine(langRoot, relative.Replace('/', Path.DirectorySeparatorChar));

				if (!File.Exists(localizedFile))
					continue;

				var expected = GetMarkdownStructure(defaultFile);
				var actual = GetMarkdownStructure(localizedFile);

				ValidateMarkdownStructure(relative, localizedFile, expected, actual, errors);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownHeadingsDoNotKeepEnglishTextUnexpectedly()
	{
		var errors = new List<string>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);

		foreach (var defaultFile in Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var relative = Path.GetRelativePath(defaultRoot, defaultFile).Replace('\\', '/');
			var defaultHeading = GetFirstHeadingText(defaultFile);
			if (defaultHeading.Length == 0)
				continue;

			foreach (var lang in GetTranslatedContentLanguages())
			{
				var langRoot = Path.Combine(_repoRoot, lang);
				var file = Path.Combine(langRoot, relative.Replace('/', Path.DirectorySeparatorChar));
				if (!File.Exists(file))
					continue;

				var localizedHeading = GetFirstHeadingText(file);
				if (!localizedHeading.Equals(defaultHeading, StringComparison.Ordinal))
					continue;

				if (IsAllowedInvariantHeading(relative, localizedHeading))
					continue;

				errors.Add($"{RelativeToRepo(file)}: H1 is identical to the English H1 '{localizedHeading}'. Localize it or add a deliberate allowlist entry.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownLinkLabelsDoNotKeepEnglishTargetHeadingsUnexpectedly()
	{
		var errors = new List<string>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);
				var document = Markdown.Parse(markdown, _markdown);
				var relative = Path.GetRelativePath(langRoot, file).Replace('\\', '/');
				var directory = Path.GetDirectoryName(relative);
				var relDir = directory == null ? string.Empty : directory.Replace('\\', '/');
				var lineStarts = GetLineStarts(markdown);

				foreach (var link in document.Descendants().OfType<LinkInline>())
				{
					if (link.IsImage)
						continue;

					var url = link.Url?.Trim();
					if (string.IsNullOrWhiteSpace(url)
						|| url[0] == '#'
						|| IsAbsoluteUrl(url)
						|| url.StartsWith("xref:", StringComparison.OrdinalIgnoreCase)
						|| url.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase))
					{
						continue;
					}

					var (path, _) = SplitPathQueryAndFragment(url);
					if (string.IsNullOrWhiteSpace(path))
						continue;

					var targetRelative = ResolveRelative(relDir, path);
					if (!targetRelative.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
						continue;

					var defaultTarget = Path.Combine(defaultRoot, targetRelative.Replace('/', Path.DirectorySeparatorChar));
					var localizedTarget = Path.Combine(langRoot, targetRelative.Replace('/', Path.DirectorySeparatorChar));
					if (!File.Exists(defaultTarget) || !File.Exists(localizedTarget))
						continue;

					var defaultHeading = GetFirstHeadingText(defaultTarget);
					var localizedHeading = GetFirstHeadingText(localizedTarget);
					var label = GetInlineText(link);
					if (label.Length == 0
						|| defaultHeading.Length == 0
						|| localizedHeading.Length == 0
						|| !label.Equals(defaultHeading, StringComparison.Ordinal)
						|| label.Equals(localizedHeading, StringComparison.Ordinal)
						|| IsAllowedInvariantLinkLabel(label))
					{
						continue;
					}

					var line = GetLineNumber(lineStarts, link.Span.Start);
					errors.Add($"{RelativeToRepo(file)}:{line}: link label '{label}' is still the English H1 for target '{targetRelative}', but localized target H1 is '{localizedHeading}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void MarkdownFilesHaveBasicDocumentStructure()
	{
		var errors = new List<string>();

		foreach (var lang in GetContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);
				var rel = RelativeToRepo(file);

				if (string.IsNullOrWhiteSpace(markdown))
					errors.Add($"{rel}: file is empty.");

				ValidateNoMergeConflictMarkers(rel, markdown, errors);
				ValidateFencedCodeBlocks(rel, markdown, errors);
				ValidateMarkdownParses(rel, markdown, errors);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownDoesNotContainKnownEnglishUiPhrases()
	{
		var errors = new List<string>();

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					foreach (var phrase in _knownEnglishUiPhrases)
					{
						if (!text.Contains(phrase, StringComparison.OrdinalIgnoreCase))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: contains known untranslated English UI phrase '{phrase}'.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCodeOutputStringsDoNotContainKnownEnglishPhrases()
	{
		var errors = new List<string>();

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var output in EnumerateCodeOutputStrings(ReadAllText(file)))
				{
					foreach (var (name, pattern) in _knownEnglishCodeOutputPatterns)
					{
						if (!Regex.IsMatch(output.Text, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{output.Line}: code output string contains known untranslated English {name}. String: {Truncate(output.Text, 180)}");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCodeOutputStringsAreTranslatedFromDefaultLanguage()
	{
		var errors = new List<string>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);

		foreach (var defaultFile in Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var relative = Path.GetRelativePath(defaultRoot, defaultFile).Replace('\\', '/');
			var defaultOutputs = EnumerateCodeOutputStrings(ReadAllText(defaultFile))
				.Select(output => NormalizeCodeOutputForTranslationCheck(output.Text))
				.Where(IsTranslatableEnglishCodeOutput)
				.ToHashSet(StringComparer.Ordinal);

			if (defaultOutputs.Count == 0)
				continue;

			foreach (var lang in GetTranslatedContentLanguages())
			{
				var langRoot = Path.Combine(_repoRoot, lang);
				var file = Path.Combine(langRoot, relative.Replace('/', Path.DirectorySeparatorChar));

				if (!File.Exists(file))
					continue;

				foreach (var output in EnumerateCodeOutputStrings(ReadAllText(file)))
				{
					var normalized = NormalizeCodeOutputForTranslationCheck(output.Text);
					if (!defaultOutputs.Contains(normalized))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{output.Line}: code output string is identical to the English source. Localize it or add a deliberate allowlist entry. String: {Truncate(output.Text, 180)}");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCodeOutputStringsDoNotLookLikeEnglish()
	{
		var errors = new List<string>();

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var output in EnumerateCodeOutputStrings(ReadAllText(file)))
				{
					var normalized = NormalizeCodeOutputForTranslationCheck(output.Text);
					if (!IsTranslatableEnglishCodeOutput(normalized))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{output.Line}: code output string looks like untranslated English. Localize it or add a deliberate allowlist entry. String: {Truncate(output.Text, 180)}");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void TextFilesDoNotContainRepeatedQuestionMarks()
	{
		var errors = new List<string>();

		foreach (var file in EnumerateContentTextFiles())
			ValidateNoQuestionMarkGarbling(file, errors);

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void TextFilesDoNotContainReplacementCharacters()
	{
		var errors = new List<string>();

		foreach (var file in EnumerateContentTextFiles())
			ValidateNoReplacementCharacters(file, errors);

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void TextFilesDoNotContainIntraWordQuestionMarkGarbling()
	{
		var errors = new List<string>();

		foreach (var file in EnumerateContentTextFiles())
			ValidateNoIntraWordQuestionMarkGarbling(file, errors);

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanTextDoesNotContainKnownBrokenEncodingFragments()
	{
		var errors = new List<string>();
		var germanRoot = Path.Combine(_repoRoot, "de");
		var extensions = new HashSet<string>(_textFileExtensions, StringComparer.OrdinalIgnoreCase);

		foreach (var file in Directory.EnumerateFiles(germanRoot, "*", SearchOption.AllDirectories)
			.Where(file => extensions.Contains(Path.GetExtension(file)))
			.Order(StringComparer.OrdinalIgnoreCase))
		{
			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				var fragment = _knownBrokenGermanEncodingFragments.FirstOrDefault(text.Contains);
				if (fragment is null)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: contains known broken German encoding fragment '{fragment}'. Line: {Truncate(text.Trim(), 180)}");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void TextFilesDoNotContainMojibakeMarkers()
	{
		var errors = new List<string>();

		foreach (var file in EnumerateContentTextFiles())
			ValidateNoMojibakeMarkers(file, errors);

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDocumentationAuditIsClean()
	{
		var audit = BuildLocalizedDocumentationAudit();

		if (ShouldWriteLocalizedAuditReport())
			WriteLocalizedAuditReport(audit);

		AssertNoErrors(audit.Issues.Select(issue => issue.ToString()).ToArray());
	}

	[TestMethod]
	public void LocalizedCodeCommentsAreTranslatedFromDefaultLanguage()
	{
		var errors = new List<string>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);

		foreach (var defaultFile in Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var relative = Path.GetRelativePath(defaultRoot, defaultFile).Replace('\\', '/');
			var defaultComments = EnumerateCodeComments(ReadAllText(defaultFile))
				.Select(comment => NormalizeCodeCommentForTranslationCheck(comment.Text))
				.Where(comment => comment.Length > 0)
				.ToHashSet(StringComparer.Ordinal);

			if (defaultComments.Count == 0)
				continue;

			foreach (var lang in GetContentLanguages().Where(lang => !lang.Equals(DefaultLanguage, StringComparison.OrdinalIgnoreCase)))
			{
				var langRoot = Path.Combine(_repoRoot, lang);
				var file = Path.Combine(langRoot, relative.Replace('/', Path.DirectorySeparatorChar));
				if (!File.Exists(file))
					continue;

				foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
				{
					var normalized = NormalizeCodeCommentForTranslationCheck(comment.Text);
					if (normalized.Length == 0 || !defaultComments.Contains(normalized))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: code comment is identical to the English source comment. Comment: {Truncate(comment.Text, 180)}");
				}
			}
		}

		AssertNoErrors(errors);
	}

	private static void ValidateTocFile(
		string lang,
		string langRoot,
		string tocPath,
		List<string> errors,
		Dictionary<string, string> slugs,
		HashSet<string> seenTocs)
	{
		var toc = ResolveExistingPath(langRoot, Path.GetRelativePath(langRoot, tocPath).Replace('\\', '/'));
		if (!toc.Exists)
		{
			errors.Add($"{RelativeToRepo(tocPath)} is missing.");
			return;
		}

		if (!toc.ExactCase)
		{
			errors.Add($"{RelativeToRepo(tocPath)} uses wrong case; actual path is '{lang}/{toc.ActualRelativePath}'.");
			return;
		}

		if (!seenTocs.Add(toc.FullPath))
		{
			errors.Add($"{RelativeToRepo(toc.FullPath)} recursively includes itself.");
			return;
		}

		TocEntry[] entries;
		try
		{
			entries = _yaml.Deserialize<TocEntry[]>(ReadAllText(toc.FullPath)) ?? Array.Empty<TocEntry>();
		}
		catch (Exception ex)
		{
			errors.Add($"{RelativeToRepo(toc.FullPath)} is invalid YAML: {ex.Message}");
			return;
		}

		if (entries.Length == 0)
			errors.Add($"{RelativeToRepo(toc.FullPath)} must contain at least one item.");

		var tocDir = Path.GetDirectoryName(toc.FullPath);
		if (tocDir == null)
		{
			errors.Add($"{RelativeToRepo(toc.FullPath)} directory cannot be resolved.");
			return;
		}

		foreach (var entry in entries)
			ValidateTocEntry(lang, langRoot, tocDir, toc.FullPath, entry, errors, slugs, seenTocs);

		seenTocs.Remove(toc.FullPath);
	}

	private static void ValidateTocEntry(
		string lang,
		string langRoot,
		string tocDir,
		string tocPath,
		TocEntry entry,
		List<string> errors,
		Dictionary<string, string> slugs,
		HashSet<string> seenTocs)
	{
		if (entry is null)
		{
			errors.Add($"{RelativeToRepo(tocPath)} contains an empty TOC item.");
			return;
		}

		if (string.IsNullOrWhiteSpace(entry.Name))
			errors.Add($"{RelativeToRepo(tocPath)} contains a TOC item with empty name.");

		if (!string.IsNullOrWhiteSpace(entry.Href))
			ValidateTocHref(lang, langRoot, tocDir, tocPath, entry.Href.Trim(), errors, slugs, seenTocs);

		if (entry.Items is not null)
		{
			foreach (var child in entry.Items)
				ValidateTocEntry(lang, langRoot, tocDir, tocPath, child, errors, slugs, seenTocs);
		}
	}

	private static void ValidateTocHref(
		string lang,
		string langRoot,
		string tocDir,
		string tocPath,
		string href,
		List<string> errors,
		Dictionary<string, string> slugs,
		HashSet<string> seenTocs)
	{
		var (path, fragment) = SplitPathQueryAndFragment(href);
		var normalizedHref = path.Replace('\\', '/');

		if (IsAbsoluteUrl(normalizedHref) || normalizedHref.StartsWith('#'))
		{
			errors.Add($"{RelativeToRepo(tocPath)}: TOC href '{href}' is not a renderable docs path.");
			return;
		}

		if (normalizedHref.Equals("api/toc.yml", StringComparison.OrdinalIgnoreCase))
			return;

		var absolute = Path.GetFullPath(Path.Combine(tocDir, normalizedHref.Replace('/', Path.DirectorySeparatorChar)));
		if (!IsUnderRoot(langRoot, absolute))
		{
			errors.Add($"{RelativeToRepo(tocPath)}: TOC href '{href}' escapes the language root.");
			return;
		}

		var relative = Path.GetRelativePath(langRoot, absolute).Replace('\\', '/');

		if (normalizedHref.EndsWith(".toc.yml", StringComparison.OrdinalIgnoreCase))
		{
			errors.Add($"{RelativeToRepo(tocPath)}: TOC href '{href}' looks like a typo; use 'toc.yml'.");
			return;
		}

		if (normalizedHref.EndsWith("toc.yml", StringComparison.OrdinalIgnoreCase))
		{
			var nestedToc = ResolveExistingPath(langRoot, relative);
			if (!nestedToc.Exists)
				errors.Add($"{RelativeToRepo(tocPath)}: TOC href '{href}' points to missing file '{lang}/{relative}'.");
			else if (!nestedToc.ExactCase)
				errors.Add($"{RelativeToRepo(tocPath)}: TOC href '{href}' has wrong case; actual path is '{lang}/{nestedToc.ActualRelativePath}'.");
			else
				ValidateTocFile(lang, langRoot, nestedToc.FullPath, errors, slugs, seenTocs);

			return;
		}

		if (!normalizedHref.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
		{
			errors.Add($"{RelativeToRepo(tocPath)}: TOC href '{href}' must point to .md, toc.yml, or api/toc.yml.");
			return;
		}

		var target = ResolveExistingPath(langRoot, relative);
		if (!target.Exists)
		{
			errors.Add($"{RelativeToRepo(tocPath)}: TOC href '{href}' points to missing file '{lang}/{relative}'.");
			return;
		}

		if (!target.ExactCase)
			errors.Add($"{RelativeToRepo(tocPath)}: TOC href '{href}' has wrong case; actual path is '{lang}/{target.ActualRelativePath}'.");

		if (!string.IsNullOrEmpty(fragment))
			ValidateAnchor(target.FullPath, fragment, $"{RelativeToRepo(tocPath)}: TOC href '{href}'", errors);

		var slug = target.ActualRelativePath.EndsWith(".md", StringComparison.OrdinalIgnoreCase)
			? target.ActualRelativePath[..^3]
			: target.ActualRelativePath;

		if (slugs.TryGetValue(slug, out var previous))
			errors.Add($"{RelativeToRepo(tocPath)}: duplicate TOC slug '{slug}' already referenced from {previous}.");
		else
			slugs[slug] = RelativeToRepo(tocPath);
	}

	private static void ValidateMarkdownFile(string lang, string langRoot, string file, List<string> errors)
	{
		var markdown = ReadAllText(file);
		MarkdownDocument document;

		try
		{
			document = Markdown.Parse(markdown, _markdown);
		}
		catch (Exception ex)
		{
			errors.Add($"{RelativeToRepo(file)}: Markdown parser failed: {ex.Message}");
			return;
		}

		var relative = Path.GetRelativePath(langRoot, file).Replace('\\', '/');
		var directory = Path.GetDirectoryName(relative);
		var relDir = directory == null ? string.Empty : directory.Replace('\\', '/');
		var lineStarts = GetLineStarts(markdown);

		foreach (var link in document.Descendants().OfType<LinkInline>())
			ValidateMarkdownLink(lang, file, relDir, link, lineStarts, errors);
	}

	private static void ValidateMarkdownLink(
		string lang,
		string file,
		string relDir,
		LinkInline link,
		int[] lineStarts,
		List<string> errors)
	{
		var line = GetLineNumber(lineStarts, link.Span.Start);
		var location = $"{RelativeToRepo(file)}:{line}";
		var rawUrl = link.Url;

		if (string.IsNullOrWhiteSpace(rawUrl))
		{
			errors.Add($"{location}: {(link.IsImage ? "image" : "link")} has an empty target.");
			return;
		}

		var url = rawUrl.Trim();

		if (url.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase))
		{
			errors.Add($"{location}: unsafe javascript URL '{rawUrl}'.");
			return;
		}

		if (url[0] == '#')
		{
			ValidateAnchor(file, url[1..], $"{location}: local anchor '{rawUrl}'", errors);
			return;
		}

		if (url.StartsWith("xref:", StringComparison.OrdinalIgnoreCase))
		{
			var uid = url["xref:".Length..].Trim();
			if (uid.Length == 0)
				errors.Add($"{location}: xref target is empty.");
			return;
		}

		if (IsAbsoluteUrl(url))
			return;

		var (path, fragment) = SplitPathQueryAndFragment(url);
		if (string.IsNullOrWhiteSpace(path))
		{
			if (!string.IsNullOrEmpty(fragment))
				ValidateAnchor(file, fragment, $"{location}: local anchor '{rawUrl}'", errors);
			else
				errors.Add($"{location}: {(link.IsImage ? "image" : "link")} has no local path.");

			return;
		}

		var resolved = ResolveRelative(relDir, path);
		if (resolved.Length == 0)
		{
			errors.Add($"{location}: target '{rawUrl}' resolves to the language root, not a file.");
			return;
		}

		if (!link.IsImage && resolved.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
		{
			var target = ResolveMarkdownPage(lang, resolved);
			if (!target.Exists)
			{
				errors.Add($"{location}: markdown link '{rawUrl}' points to missing page '{lang}/{resolved}' and fallback '{DefaultLanguage}/{resolved}' is missing too.");
				return;
			}

			if (!target.ExactCase)
			{
				errors.Add($"{location}: markdown link '{rawUrl}' has wrong case; actual path is '{target.Language}/{target.ActualRelativePath}'.");
				return;
			}

			if (!string.IsNullOrEmpty(fragment))
				ValidateAnchor(target.FullPath, fragment, $"{location}: markdown link '{rawUrl}'", errors);

			return;
		}

		var asset = ResolveExistingPath(Path.Combine(_repoRoot, lang), resolved);
		if (!asset.Exists)
		{
			errors.Add($"{location}: local {(link.IsImage ? "image" : "asset")} '{rawUrl}' points to missing file '{lang}/{resolved}'.");
			return;
		}

		if (!asset.ExactCase)
			errors.Add($"{location}: local {(link.IsImage ? "image" : "asset")} '{rawUrl}' has wrong case; actual path is '{lang}/{asset.ActualRelativePath}'.");
	}

	private static IReadOnlyList<string> GetLocalAnchorReferences(string langRoot, string file)
	{
		var markdown = ReadAllText(file);
		var document = Markdown.Parse(markdown, _markdown);
		var relative = Path.GetRelativePath(langRoot, file).Replace('\\', '/');
		var directory = Path.GetDirectoryName(relative);
		var relDir = directory == null ? string.Empty : directory.Replace('\\', '/');
		var references = new List<string>();

		foreach (var link in document.Descendants().OfType<LinkInline>())
		{
			var url = link.Url?.Trim();
			if (string.IsNullOrWhiteSpace(url)
				|| IsAbsoluteUrl(url)
				|| url.StartsWith("xref:", StringComparison.OrdinalIgnoreCase)
				|| url.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}

			var (path, fragment) = SplitPathQueryAndFragment(url);
			if (string.IsNullOrEmpty(fragment))
				continue;

			var target = string.IsNullOrWhiteSpace(path)
				? relative
				: ResolveRelative(relDir, path);

			references.Add($"{target}#{NormalizeAnchor(fragment)}");
		}

		return references;
	}

	private static MarkdownStructure GetMarkdownStructure(string file)
	{
		var markdown = ReadAllText(file);
		var document = Markdown.Parse(markdown, _markdown);

		return new MarkdownStructure(
			EnumerateHeadings(document).Select(heading => heading.Level).ToArray(),
			document.Descendants().OfType<CodeBlock>().Select(GetCodeBlockLanguage).ToArray(),
			document.Descendants().OfType<LinkInline>().Where(link => link.IsImage).Select(link => NormalizeStructureUrl(link.Url)).ToArray(),
			document.Descendants().OfType<Table>().Select(GetTableShape).ToArray(),
			EnumerateUserVisibleMarkdownLines(markdown)
				.Select(line => Regex.Match(line.Text, @"^\s*>\s*\[!(?<kind>NOTE|TIP|IMPORTANT|WARNING|CAUTION)\]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
				.Where(match => match.Success)
				.Select(match => match.Groups["kind"].Value.ToUpperInvariant())
				.ToArray());
	}

	private static void ValidateMarkdownStructure(
		string relative,
		string localizedFile,
		MarkdownStructure expected,
		MarkdownStructure actual,
		List<string> errors)
	{
		ValidateStructureSequence(relative, localizedFile, "heading levels", expected.HeadingLevels, actual.HeadingLevels, errors);
		ValidateStructureSequence(relative, localizedFile, "code block languages", expected.CodeBlockLanguages, actual.CodeBlockLanguages, errors);
		ValidateStructureSequence(relative, localizedFile, "image URLs", expected.ImageUrls, actual.ImageUrls, errors);
		ValidateStructureSequence(relative, localizedFile, "table shapes", expected.TableShapes, actual.TableShapes, errors);
		ValidateStructureSequence(relative, localizedFile, "admonition kinds", expected.AdmonitionKinds, actual.AdmonitionKinds, errors);
	}

	private static void ValidateStructureSequence<T>(
		string relative,
		string localizedFile,
		string name,
		IReadOnlyList<T> expected,
		IReadOnlyList<T> actual,
		List<string> errors)
	{
		if (expected.SequenceEqual(actual))
			return;

		errors.Add($"{RelativeToRepo(localizedFile)}: {name} must match {DefaultLanguage}/{relative}. Expected: {FormatStructureSequence(expected)}. Actual: {FormatStructureSequence(actual)}.");
	}

	private static string GetCodeBlockLanguage(CodeBlock block)
	{
		if (block is not FencedCodeBlock fenced)
			return "indented";

		var info = fenced.Info?.Trim();
		if (string.IsNullOrEmpty(info))
			return "none";

		var separator = info.IndexOfAny([' ', '\t']);
		if (separator >= 0)
			info = info[..separator];

		return info.ToLowerInvariant();
	}

	private static TableShape GetTableShape(Table table)
	{
		var rows = table.OfType<TableRow>().ToArray();
		var columnCounts = rows
			.Select(row => row.OfType<TableCell>().Count())
			.ToArray();

		return new TableShape(
			columnCounts.Length == 0 ? 0 : columnCounts.Max(),
			Math.Max(0, rows.Length - 1));
	}

	private static string NormalizeStructureUrl(string url)
		=> (url ?? string.Empty).Replace('\\', '/').Trim();

	private static string FormatStructureSequence<T>(IReadOnlyList<T> values)
		=> values.Count == 0
			? "<none>"
			: string.Join(", ", values);

	private static string GetFirstHeadingText(string file)
	{
		var document = Markdown.Parse(ReadAllText(file), _markdown);
		var heading = EnumerateHeadings(document).FirstOrDefault(heading => heading.Level == 1);

		return heading?.Inline is null
			? string.Empty
			: GetInlineText(heading.Inline);
	}

	private static string GetInlineText(ContainerInline inline)
	{
		var text = string.Concat(inline.Descendants<LiteralInline>().Select(literal => literal.Content.ToString()));

		return NormalizeHumanText(text);
	}

	private static string NormalizeHumanText(string text)
		=> Regex.Replace(text, @"\s+", " ", RegexOptions.CultureInvariant).Trim();

	private static bool IsAllowedInvariantHeading(string relativePath, string heading)
	{
		var normalizedPath = relativePath.Replace('\\', '/');

		if (normalizedPath.StartsWith("topics/api/connectors/", StringComparison.OrdinalIgnoreCase))
			return true;

		if (normalizedPath.StartsWith("topics/api/indicators/list_of_indicators/", StringComparison.OrdinalIgnoreCase))
		{
			if (IsShortInvariantName(heading) || IsSingleTokenIndicatorName(heading))
				return true;

			if (_allowedInvariantIndicatorHeadingPaths.Contains(normalizedPath))
				return true;
		}

		return _allowedInvariantHeadingTexts.Contains(heading) || IsShortInvariantName(heading);
	}

	private static bool IsAllowedInvariantLinkLabel(string label)
		=> _allowedInvariantLinkLabels.Contains(label) || IsShortInvariantName(label);

	private static bool IsShortInvariantName(string text)
	{
		var compact = text.Replace(" ", string.Empty, StringComparison.Ordinal);

		return compact.Length is > 0 and <= 12
			&& Regex.IsMatch(text, @"^[A-Z0-9%./+*() _-]+$", RegexOptions.CultureInvariant);
	}

	private static bool IsSingleTokenIndicatorName(string text)
		=> !text.Contains(' ')
			&& !text.Contains('-')
			&& Regex.IsMatch(text, @"^[A-Za-z][A-Za-z0-9]*$", RegexOptions.CultureInvariant);

	private static MarkdownTarget ResolveMarkdownPage(string lang, string relativePath)
	{
		var current = ResolveExistingPath(Path.Combine(_repoRoot, lang), relativePath);
		if (current.Exists)
			return new MarkdownTarget(lang, current.Exists, current.ExactCase, current.FullPath, current.ActualRelativePath);

		if (!lang.Equals(DefaultLanguage, StringComparison.OrdinalIgnoreCase))
		{
			var fallback = ResolveExistingPath(Path.Combine(_repoRoot, DefaultLanguage), relativePath);
			if (fallback.Exists)
				return new MarkdownTarget(DefaultLanguage, fallback.Exists, fallback.ExactCase, fallback.FullPath, fallback.ActualRelativePath);
		}

		return new MarkdownTarget(lang, false, true, string.Empty, relativePath);
	}

	private static void ValidateAnchor(string targetFile, string rawFragment, string location, List<string> errors)
	{
		var anchor = NormalizeAnchor(rawFragment);
		if (anchor.Length == 0 || anchor.StartsWith(":~:text=", StringComparison.OrdinalIgnoreCase))
			return;

		var anchors = GetAnchors(targetFile);
		if (!anchors.Contains(anchor))
			errors.Add($"{location} points to missing anchor '#{anchor}' in {RelativeToRepo(targetFile)}.");
	}

	private static HashSet<string> GetAnchors(string file)
	{
		var markdown = ReadAllText(file);
		var document = Markdown.Parse(markdown, _markdown);
		var anchors = new HashSet<string>(StringComparer.Ordinal);

		var generatedIds = new Dictionary<string, int>(StringComparer.Ordinal);

		foreach (var heading in EnumerateHeadings(document))
		{
			var id = heading.GetAttributes().Id;
			if (!string.IsNullOrWhiteSpace(id))
				anchors.Add(id);

			var fallbackId = CreateHeadingId(heading, generatedIds);
			if (!string.IsNullOrWhiteSpace(fallbackId))
				anchors.Add(fallbackId);
		}

		foreach (Match match in Regex.Matches(markdown, "\\b(?:id|name)\\s*=\\s*[\"'](?<id>[^\"']+)[\"']", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			var id = match.Groups["id"].Value;
			if (!string.IsNullOrWhiteSpace(id))
				anchors.Add(id);
		}

		return anchors;
	}

	private static string CreateHeadingId(HeadingBlock heading, Dictionary<string, int> generatedIds)
	{
		if (heading.Inline is null)
			return string.Empty;

		var text = string.Concat(heading.Inline.Descendants<LiteralInline>().Select(l => l.Content.ToString())).Trim();
		if (text.Length == 0)
			return string.Empty;

		var id = Regex.Replace(text.ToLowerInvariant(), @"[^\p{L}\p{Nd}_ -]+", string.Empty, RegexOptions.CultureInvariant);
		id = Regex.Replace(id, @"\s+", "-", RegexOptions.CultureInvariant).Trim('-');
		if (id.Length == 0)
			return string.Empty;

		generatedIds.TryGetValue(id, out var count);
		generatedIds[id] = count + 1;

		return count == 0 ? id : $"{id}-{count}";
	}

	private static void ValidateNoMergeConflictMarkers(string rel, string markdown, List<string> errors)
	{
		var line = 1;
		using var reader = new StringReader(markdown);

		for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
		{
			if (text.StartsWith("<<<<<<< ", StringComparison.Ordinal)
				|| text.StartsWith(">>>>>>> ", StringComparison.Ordinal)
				|| text.Equals("=======", StringComparison.Ordinal))
			{
				errors.Add($"{rel}:{line}: merge conflict marker is present.");
			}
		}
	}

	private static IEnumerable<HeadingBlock> EnumerateHeadings(ContainerBlock container)
	{
		foreach (var block in container)
		{
			if (block is HeadingBlock heading)
				yield return heading;

			if (block is ContainerBlock childContainer)
			{
				foreach (var childHeading in EnumerateHeadings(childContainer))
					yield return childHeading;
			}
		}
	}

	private static void ValidateFencedCodeBlocks(string rel, string markdown, List<string> errors)
	{
		var inFence = false;
		var fenceChar = '\0';
		var fenceLength = 0;
		var fenceStartLine = 0;
		var line = 1;

		using var reader = new StringReader(markdown);
		for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
		{
			var match = Regex.Match(text, "^[ ]{0,3}(?<fence>`{3,}|~{3,})", RegexOptions.CultureInvariant);
			if (!match.Success)
				continue;

			var fence = match.Groups["fence"].Value;
			if (!inFence)
			{
				inFence = true;
				fenceChar = fence[0];
				fenceLength = fence.Length;
				fenceStartLine = line;
				continue;
			}

			if (fence[0] == fenceChar && fence.Length >= fenceLength)
			{
				inFence = false;
				fenceChar = '\0';
				fenceLength = 0;
				fenceStartLine = 0;
			}
		}

		if (inFence)
			errors.Add($"{rel}:{fenceStartLine}: fenced code block is not closed.");
	}

	private static void ValidateMarkdownParses(string rel, string markdown, List<string> errors)
	{
		try
		{
			Markdown.Parse(markdown, _markdown);
		}
		catch (Exception ex)
		{
			errors.Add($"{rel}: Markdown parser failed: {ex.Message}");
		}
	}

	private static void ValidateNoQuestionMarkGarbling(string file, List<string> errors)
	{
		var line = 1;
		using var reader = new StringReader(ReadAllText(file));

		for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
		{
			var match = Regex.Match(text, @"\?{3,}", RegexOptions.CultureInvariant);
			if (!match.Success)
				continue;

			errors.Add($"{RelativeToRepo(file)}:{line}: contains '{match.Value}', which usually means translated Unicode text was corrupted. Line: {Truncate(text.Trim(), 180)}");
		}
	}

	private static void ValidateNoReplacementCharacters(string file, List<string> errors)
	{
		var line = 1;
		using var reader = new StringReader(ReadAllText(file));

		for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
		{
			if (!text.Contains('\uFFFD'))
				continue;

			errors.Add($"{RelativeToRepo(file)}:{line}: contains the Unicode replacement character '�', which usually means text was decoded with the wrong encoding. Line: {Truncate(text.Trim(), 180)}");
		}
	}

	private static void ValidateNoIntraWordQuestionMarkGarbling(string file, List<string> errors)
	{
		var line = 1;
		using var reader = new StringReader(ReadAllText(file));

		for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
		{
			var scanText = RemoveQuestionMarkSafeSegments(text);
			var match = Regex.Match(scanText, @"[A-Za-zÀ-ÖØ-öø-ÿ]\?{1,2}[A-Za-zÀ-ÖØ-öø-ÿ]", RegexOptions.CultureInvariant);
			if (!match.Success)
				continue;

			errors.Add($"{RelativeToRepo(file)}:{line}: contains suspicious '?' inside a word ('{match.Value}'), which usually means translated Unicode text was corrupted. Line: {Truncate(text.Trim(), 180)}");
		}
	}

	private static void ValidateNoMojibakeMarkers(string file, List<string> errors)
	{
		var line = 1;
		using var reader = new StringReader(ReadAllText(file));

		for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
		{
			var marker = _mojibakeMarkers.FirstOrDefault(text.Contains);
			if (marker is null)
				continue;

			errors.Add($"{RelativeToRepo(file)}:{line}: contains mojibake marker '{marker}', which usually means Unicode punctuation was decoded with the wrong encoding. Line: {Truncate(text.Trim(), 180)}");
		}
	}

	private static LocalizedDocumentationAudit BuildLocalizedDocumentationAudit()
	{
		var languages = GetTranslatedContentLanguages();
		var issues = new List<AuditIssue>();
		var stats = new List<LocalizedAuditLanguageStats>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);
		var defaultOutputsByRelativePath = BuildDefaultCodeOutputMap(defaultRoot);
		var defaultCommentsByRelativePath = BuildDefaultCodeCommentMap(defaultRoot);
		var textExtensions = new HashSet<string>(_textFileExtensions, StringComparer.OrdinalIgnoreCase);

		foreach (var lang in languages)
		{
			var langRoot = Path.Combine(_repoRoot, lang);
			var textFiles = Directory.EnumerateFiles(langRoot, "*", SearchOption.AllDirectories)
				.Where(file => textExtensions.Contains(Path.GetExtension(file)))
				.Order(StringComparer.OrdinalIgnoreCase)
				.ToArray();
			var markdownFiles = textFiles
				.Where(file => Path.GetExtension(file).Equals(".md", StringComparison.OrdinalIgnoreCase))
				.ToArray();

			var outputCount = 0;
			var commentCount = 0;

			foreach (var file in textFiles)
				CollectTextEncodingAuditIssues(file, issues);

			foreach (var file in markdownFiles)
			{
				var relative = Path.GetRelativePath(langRoot, file).Replace('\\', '/');
				var markdown = ReadAllText(file);
				defaultOutputsByRelativePath.TryGetValue(relative, out var defaultOutputs);
				defaultCommentsByRelativePath.TryGetValue(relative, out var defaultComments);

				foreach (var output in EnumerateCodeOutputStrings(markdown))
				{
					outputCount++;
					var normalized = NormalizeCodeOutputForTranslationCheck(output.Text);
					var exactEnglish = defaultOutputs is not null && defaultOutputs.Contains(normalized);

					foreach (var (name, pattern) in _knownEnglishCodeOutputPatterns)
					{
						if (!Regex.IsMatch(output.Text, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
							continue;

						issues.Add(new AuditIssue("code-output-known-english", RelativeToRepo(file), output.Line, $"contains known untranslated English {name}: {Truncate(output.Text, 180)}"));
					}

					if (exactEnglish)
					{
						issues.Add(new AuditIssue("code-output-exact-english", RelativeToRepo(file), output.Line, $"is identical to the English source output: {Truncate(output.Text, 180)}"));
						continue;
					}

					if (IsTranslatableEnglishCodeOutput(normalized))
						issues.Add(new AuditIssue("code-output-likely-english", RelativeToRepo(file), output.Line, $"looks like untranslated English: {Truncate(output.Text, 180)}"));
				}

				foreach (var comment in EnumerateCodeComments(markdown))
				{
					commentCount++;
					var normalized = NormalizeCodeCommentForTranslationCheck(comment.Text);
					if (normalized.Length == 0 || defaultComments is null || !defaultComments.Contains(normalized))
						continue;

					issues.Add(new AuditIssue("code-comment-exact-english", RelativeToRepo(file), comment.Line, $"is identical to the English source comment: {Truncate(comment.Text, 180)}"));
				}
			}

			stats.Add(new LocalizedAuditLanguageStats(lang, markdownFiles.Length, textFiles.Length, outputCount, commentCount));
		}

		return new LocalizedDocumentationAudit(languages, stats, issues);
	}

	private static Dictionary<string, HashSet<string>> BuildDefaultCodeOutputMap(string defaultRoot)
		=> Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories)
			.Order(StringComparer.OrdinalIgnoreCase)
			.Select(file => new
			{
				RelativePath = Path.GetRelativePath(defaultRoot, file).Replace('\\', '/'),
				Outputs = EnumerateCodeOutputStrings(ReadAllText(file))
					.Select(output => NormalizeCodeOutputForTranslationCheck(output.Text))
					.Where(IsTranslatableEnglishCodeOutput)
					.ToHashSet(StringComparer.Ordinal),
			})
			.Where(entry => entry.Outputs.Count > 0)
			.ToDictionary(entry => entry.RelativePath, entry => entry.Outputs, StringComparer.OrdinalIgnoreCase);

	private static Dictionary<string, HashSet<string>> BuildDefaultCodeCommentMap(string defaultRoot)
		=> Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories)
			.Order(StringComparer.OrdinalIgnoreCase)
			.Select(file => new
			{
				RelativePath = Path.GetRelativePath(defaultRoot, file).Replace('\\', '/'),
				Comments = EnumerateCodeComments(ReadAllText(file))
					.Select(comment => NormalizeCodeCommentForTranslationCheck(comment.Text))
					.Where(comment => comment.Length > 0)
					.ToHashSet(StringComparer.Ordinal),
			})
			.Where(entry => entry.Comments.Count > 0)
			.ToDictionary(entry => entry.RelativePath, entry => entry.Comments, StringComparer.OrdinalIgnoreCase);

	private static void CollectTextEncodingAuditIssues(string file, List<AuditIssue> issues)
	{
		var line = 1;
		using var reader = new StringReader(ReadAllText(file));

		for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
		{
			var repeatedQuestionMarks = Regex.Match(text, @"\?{3,}", RegexOptions.CultureInvariant);
			if (repeatedQuestionMarks.Success)
			{
				issues.Add(new AuditIssue("encoding-repeated-question-marks", RelativeToRepo(file), line, $"contains '{repeatedQuestionMarks.Value}': {Truncate(text.Trim(), 180)}"));
			}

			var scanText = RemoveQuestionMarkSafeSegments(text);
			var intraWordQuestionMark = Regex.Match(scanText, @"[A-Za-zÀ-ÖØ-öø-ÿ]\?{1,2}[A-Za-zÀ-ÖØ-öø-ÿ]", RegexOptions.CultureInvariant);
			if (intraWordQuestionMark.Success)
			{
				issues.Add(new AuditIssue("encoding-intra-word-question-mark", RelativeToRepo(file), line, $"contains suspicious '?' inside a word ('{intraWordQuestionMark.Value}'): {Truncate(text.Trim(), 180)}"));
			}

			if (text.Contains('\uFFFD'))
				issues.Add(new AuditIssue("encoding-replacement-character", RelativeToRepo(file), line, $"contains the Unicode replacement character '�': {Truncate(text.Trim(), 180)}"));

			var marker = _mojibakeMarkers.FirstOrDefault(text.Contains);
			if (marker is not null)
				issues.Add(new AuditIssue("encoding-mojibake-marker", RelativeToRepo(file), line, $"contains mojibake marker '{marker}': {Truncate(text.Trim(), 180)}"));
		}
	}

	private static bool ShouldWriteLocalizedAuditReport()
	{
		var value = Environment.GetEnvironmentVariable(LocalizedAuditReportEnvironmentVariable);
		return value is not null
			&& (value.Equals("1", StringComparison.Ordinal)
				|| value.Equals("true", StringComparison.OrdinalIgnoreCase)
				|| value.Equals("yes", StringComparison.OrdinalIgnoreCase));
	}

	private static void WriteLocalizedAuditReport(LocalizedDocumentationAudit audit)
	{
		var path = Path.Combine(_repoRoot, LocalizedAuditReportFileName);
		var report = new StringBuilder();

		report.AppendLine("# Localized Documentation Audit");
		report.AppendLine();
		report.AppendLine($"Generated: {DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss zzz}");
		report.AppendLine($"Languages: {string.Join(", ", audit.Languages)}");
		report.AppendLine();
		report.AppendLine("## Coverage");
		report.AppendLine();
		report.AppendLine("| Language | Markdown files | Text files | Code output strings | Code comments |");
		report.AppendLine("| --- | ---: | ---: | ---: | ---: |");

		foreach (var stat in audit.Stats)
			report.AppendLine($"| {stat.Language} | {stat.MarkdownFiles} | {stat.TextFiles} | {stat.CodeOutputStrings} | {stat.CodeComments} |");

		report.AppendLine();
		report.AppendLine("## Checks");
		report.AppendLine();
		report.AppendLine("- Encoding: repeated question marks, suspicious question marks inside Latin words, Unicode replacement characters, mojibake markers.");
		report.AppendLine("- Code output: known English phrases, likely English output strings, exact matches with English source output.");
		report.AppendLine("- Code comments: exact matches with English source comments.");
		report.AppendLine("- Markdown structure parity is covered by `LocalizedMarkdownStructureMatchesDefaultLanguage`.");
		report.AppendLine();
		report.AppendLine("## Issues");
		report.AppendLine();

		if (audit.Issues.Count == 0)
		{
			report.AppendLine("No issues found.");
		}
		else
		{
			report.AppendLine("| Category | Location | Details |");
			report.AppendLine("| --- | --- | --- |");

			foreach (var issue in audit.Issues)
				report.AppendLine($"| {EscapeMarkdownTableCell(issue.Category)} | {EscapeMarkdownTableCell(issue.Location)} | {EscapeMarkdownTableCell(issue.Message)} |");
		}

		File.WriteAllText(path, report.ToString(), Encoding.UTF8);
	}

	private static string EscapeMarkdownTableCell(string value)
		=> value.Replace("|", "\\|", StringComparison.Ordinal)
			.Replace("\r", " ", StringComparison.Ordinal)
			.Replace("\n", "<br>", StringComparison.Ordinal);

	private static string RemoveQuestionMarkSafeSegments(string text)
	{
		var withoutUrls = Regex.Replace(text, @"https?://[^\s)\]>""']+", " ", RegexOptions.CultureInvariant);
		return Regex.Replace(withoutUrls, @"(?:^|[\s(`])[\w./-]+\?[\w=&%{}./:+-]+", " ", RegexOptions.CultureInvariant);
	}

	private static IEnumerable<CodeComment> EnumerateCodeComments(string markdown)
	{
		foreach (Match block in Regex.Matches(markdown, "```[^\\r\\n]*\\r?\\n(?<code>.*?)```", RegexOptions.Singleline | RegexOptions.CultureInvariant))
		{
			var line = GetLineNumber(GetLineStarts(markdown), block.Groups["code"].Index);
			using var reader = new StringReader(block.Groups["code"].Value);

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				var trimmed = text.Trim();
				if (trimmed.StartsWith("///", StringComparison.Ordinal)
					|| trimmed.StartsWith("//", StringComparison.Ordinal)
					|| trimmed.StartsWith("#", StringComparison.Ordinal))
				{
					yield return new CodeComment(trimmed, line);
				}
			}
		}
	}

	private static IEnumerable<CodeOutputString> EnumerateCodeOutputStrings(string markdown)
	{
		foreach (Match block in Regex.Matches(markdown, "```[^\\r\\n]*\\r?\\n(?<code>.*?)```", RegexOptions.Singleline | RegexOptions.CultureInvariant))
		{
			var line = GetLineNumber(GetLineStarts(markdown), block.Groups["code"].Index);
			using var reader = new StringReader(block.Groups["code"].Value);

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				if (!Regex.IsMatch(text, @"\b(?:Console\.Write(?:Line)?|Add(?:Info|Debug|Warning|Error)Log|MessageBox\.Show|Trace\.Write(?:Line)?)\s*\(", RegexOptions.CultureInvariant))
					continue;

				foreach (Match literal in Regex.Matches(text, @"\$?""(?<value>[^""\\]*(?:\\.[^""\\]*)*)""", RegexOptions.CultureInvariant))
				{
					var value = literal.Groups["value"].Value;
					value = Regex.Replace(value, @"\{[^{}]*\}", " ", RegexOptions.CultureInvariant);
					value = Regex.Replace(value, @"\s+", " ", RegexOptions.CultureInvariant).Trim();

					if (value.Length > 0)
						yield return new CodeOutputString(value, line);
				}
			}
		}
	}

	private static string NormalizeCodeOutputForTranslationCheck(string text)
		=> Regex.Replace(text ?? string.Empty, @"\s+", " ", RegexOptions.CultureInvariant).Trim();

	private static bool IsTranslatableEnglishCodeOutput(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
			return false;

		var hits = 0;
		var words = 0;

		foreach (Match match in Regex.Matches(text, @"[A-Za-z][A-Za-z']+", RegexOptions.CultureInvariant))
		{
			var word = match.Value.Trim('\'');
			if (word.Length <= 2 || _allowedInvariantCodeOutputWords.Contains(word))
				continue;

			words++;

			if (_translatableEnglishCodeOutputWords.Contains(word))
				hits++;
		}

		return words >= 2 && hits >= 2;
	}

	private static string NormalizeCodeCommentForTranslationCheck(string comment)
	{
		if (Regex.IsMatch(comment, @"https?://|<see\s+cref=|nameof\(|StockSharp|^[#/\\\s-]*$|^//\s*[A-Z][A-Za-z0-9_.]+\s*=", RegexOptions.CultureInvariant))
			return string.Empty;

		var text = Regex.Replace(comment, @"^\s*(?:///?|#)\s*", string.Empty, RegexOptions.CultureInvariant);
		text = Regex.Replace(text, @"\s+", " ", RegexOptions.CultureInvariant).Trim();

		if (text.Length < 12)
			return string.Empty;

		// Skip XML doc boilerplate, commented-out code, identifiers, and compiler/preprocessor directives.
		if (Regex.IsMatch(text, @"^<[^>]+/?>$|^[A-Za-z_][A-Za-z0-9_.]*$|[;{}=()]|^(if|for|while|return|using|var|let|public|private|protected|class|new|await|yield|pragma|region|endregion|nullable|define|endif|else|elif)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
			return string.Empty;

		return text;
	}

	private static (string Path, string Fragment) SplitPathQueryAndFragment(string url)
	{
		var path = url;
		var fragment = string.Empty;

		var hash = path.IndexOf('#');
		if (hash >= 0)
		{
			fragment = path[(hash + 1)..];
			path = path[..hash];
		}

		var query = path.IndexOf('?');
		if (query >= 0)
			path = path[..query];

		if (fragment.Length > 0)
		{
			var fragmentQuery = fragment.IndexOf('?');
			if (fragmentQuery >= 0)
				fragment = fragment[..fragmentQuery];
		}

		return (path, fragment);
	}

	private static string NormalizeAnchor(string rawFragment)
	{
		var anchor = rawFragment.TrimStart('#').Trim();
		if (anchor.Length == 0)
			return string.Empty;

		try
		{
			return Uri.UnescapeDataString(anchor);
		}
		catch
		{
			return anchor;
		}
	}

	private static bool IsAbsoluteUrl(string url)
		=> url.StartsWith("//", StringComparison.Ordinal)
			|| url.Contains("://", StringComparison.Ordinal)
			|| url.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase)
			|| url.StartsWith("tel:", StringComparison.OrdinalIgnoreCase)
			|| url.StartsWith("data:", StringComparison.OrdinalIgnoreCase);

	private static string ResolveRelative(string baseDir, string relative)
	{
		var segments = new List<string>();

		if (!relative.StartsWith('/') && baseDir.Length > 0)
			segments.AddRange(baseDir.Split('/', StringSplitOptions.RemoveEmptyEntries));

		foreach (var raw in relative.Replace('\\', '/').Split('/', StringSplitOptions.RemoveEmptyEntries))
		{
			if (raw == ".")
				continue;

			if (raw == "..")
			{
				if (segments.Count > 0)
					segments.RemoveAt(segments.Count - 1);
				continue;
			}

			segments.Add(raw);
		}

		return string.Join('/', segments);
	}

	private static PathResolution ResolveExistingPath(string root, string relativePath)
	{
		root = Path.GetFullPath(root);
		relativePath = relativePath.Replace('\\', '/').TrimStart('/');

		if (relativePath.Length == 0)
			return new PathResolution(Directory.Exists(root), true, root, string.Empty);

		var current = root;
		var actualSegments = new List<string>();
		var exactCase = true;

		foreach (var segment in relativePath.Split('/', StringSplitOptions.RemoveEmptyEntries))
		{
			if (segment == ".")
				continue;

			if (segment == "..")
			{
				var parent = Directory.GetParent(current);
				if (parent is null || !IsUnderRoot(root, parent.FullName))
					return new PathResolution(false, exactCase, string.Empty, string.Join('/', actualSegments));

				current = parent.FullName;
				if (actualSegments.Count > 0)
					actualSegments.RemoveAt(actualSegments.Count - 1);
				continue;
			}

			if (!Directory.Exists(current))
				return new PathResolution(false, exactCase, string.Empty, string.Join('/', actualSegments.Append(segment)));

			var match = Directory.EnumerateFileSystemEntries(current)
				.Select(path => new { FullPath = path, Name = Path.GetFileName(path) })
				.FirstOrDefault(entry => entry.Name.Equals(segment, StringComparison.OrdinalIgnoreCase));

			if (match is null)
				return new PathResolution(false, exactCase, string.Empty, string.Join('/', actualSegments.Append(segment)));

			if (!match.Name.Equals(segment, StringComparison.Ordinal))
				exactCase = false;

			current = match.FullPath;
			actualSegments.Add(match.Name);
		}

		return new PathResolution(File.Exists(current) || Directory.Exists(current), exactCase, current, string.Join('/', actualSegments));
	}

	private static bool IsUnderRoot(string root, string path)
	{
		var fullRoot = Path.GetFullPath(root).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		var fullPath = Path.GetFullPath(path);

		return fullPath.Equals(fullRoot, StringComparison.OrdinalIgnoreCase)
			|| fullPath.StartsWith(fullRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
			|| fullPath.StartsWith(fullRoot + Path.AltDirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
	}

	private static bool IsRussianSpecificPlaceholder(string markdown)
		=> markdown.Contains(RussianSpecificPlaceholderMarker, StringComparison.OrdinalIgnoreCase);

	private static void ValidateRussianSpecificPlaceholderPage(string file, string markdown, List<string> errors)
	{
		var rel = RelativeToRepo(file);
		var nonEmptyLines = markdown.Split(["\r\n", "\n"], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

		if (nonEmptyLines.Length is < 2 or > 3)
			errors.Add($"{rel}: Russian-specific non-Russian page must stay a short placeholder, not full localized connector/source documentation.");

		if (nonEmptyLines.Length > 0 && !nonEmptyLines[0].StartsWith("# ", StringComparison.Ordinal))
			errors.Add($"{rel}: Russian-specific placeholder must start with an H1 heading.");

		if (Regex.IsMatch(markdown, @"```|~~~", RegexOptions.CultureInvariant))
			errors.Add($"{rel}: Russian-specific placeholder must not contain code blocks.");

		if (Regex.IsMatch(markdown, @"!\[[^\]]*\]\(|\[[^\]]+\]\([^)]+\)", RegexOptions.CultureInvariant))
			errors.Add($"{rel}: Russian-specific placeholder must not contain links or images.");

		if (Regex.IsMatch(markdown, @"^\s*(?:[-*+]\s+|\d+\.\s+|\|)", RegexOptions.Multiline | RegexOptions.CultureInvariant))
			errors.Add($"{rel}: Russian-specific placeholder must not contain lists or tables.");
	}

	private static IEnumerable<(string Text, int Line)> EnumerateUserVisibleMarkdownLines(string markdown)
	{
		var line = 1;
		var inFence = false;
		using var reader = new StringReader(markdown);

		for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
		{
			var trimmed = text.Trim();
			if (Regex.IsMatch(trimmed, @"^(```|~~~)", RegexOptions.CultureInvariant))
			{
				inFence = !inFence;
				continue;
			}

			if (inFence || trimmed.Length == 0 || trimmed.StartsWith("![", StringComparison.Ordinal))
				continue;

			yield return (text, line);
		}
	}

	private static IEnumerable<string> EnumerateContentTextFiles()
	{
		var extensions = new HashSet<string>(_textFileExtensions, StringComparer.OrdinalIgnoreCase);

		foreach (var lang in GetContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*", SearchOption.AllDirectories)
				.Where(file => extensions.Contains(Path.GetExtension(file)))
				.Order(StringComparer.OrdinalIgnoreCase))
			{
				yield return file;
			}
		}
	}

	private static int[] GetLineStarts(string text)
	{
		var starts = new List<int> { 0 };
		for (var i = 0; i < text.Length; i++)
		{
			if (text[i] == '\n')
				starts.Add(i + 1);
		}

		return starts.ToArray();
	}

	private static int GetLineNumber(int[] lineStarts, int offset)
	{
		if (offset < 0)
			return 1;

		var index = Array.BinarySearch(lineStarts, offset);
		if (index < 0)
			index = Math.Max(0, ~index - 2);

		return index + 1;
	}

	private static IReadOnlyList<string> GetContentLanguages()
		=> Directory.EnumerateDirectories(_repoRoot)
			.Select(Path.GetFileName)
			.Where(name => name is not null
				&& Regex.IsMatch(name, "^[a-z]{2}$", RegexOptions.CultureInvariant)
				&& HasContent(name))
			.Cast<string>()
			.Order(StringComparer.OrdinalIgnoreCase)
			.ToArray();

	private static IReadOnlyList<string> GetTranslatedContentLanguages()
		=> GetContentLanguages()
			.Where(lang => !lang.Equals(DefaultLanguage, StringComparison.OrdinalIgnoreCase)
				&& !lang.Equals("ru", StringComparison.OrdinalIgnoreCase))
			.ToArray();

	private static bool HasContent(string lang)
	{
		var root = Path.Combine(_repoRoot, lang);
		return File.Exists(Path.Combine(root, "index.md")) || Directory.Exists(Path.Combine(root, "topics"));
	}

	private static string FindRepoRoot()
	{
		for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
		{
			if (Directory.Exists(Path.Combine(dir.FullName, DefaultLanguage))
				&& File.Exists(Path.Combine(dir.FullName, DefaultLanguage, "language.json")))
				return dir.FullName;
		}

		throw new InvalidOperationException("Could not find doc repository root from test output directory.");
	}

	private static string ReadAllText(string path)
	{
		for (var attempt = 0; ; attempt++)
		{
			try
			{
				return File.ReadAllText(path);
			}
			catch (IOException) when (attempt < 3)
			{
				Thread.Sleep(100);
			}
		}
	}

	private static string RelativeToRepo(string path)
	{
		if (string.IsNullOrEmpty(path))
			return path;

		var fullPath = Path.GetFullPath(path);
		if (!IsUnderRoot(_repoRoot, fullPath))
			return fullPath.Replace('\\', '/');

		return Path.GetRelativePath(_repoRoot, fullPath).Replace('\\', '/');
	}

	private static string Truncate(string value, int maxLength)
		=> value.Length <= maxLength
			? value
			: value[..maxLength] + "…";

	private void AssertNoErrors(IReadOnlyList<string> errors)
	{
		if (errors.Count == 0)
			return;

		var sample = string.Join(Environment.NewLine, errors.Take(MaxReportedErrors));
		var suffix = errors.Count > MaxReportedErrors
			? $"{Environment.NewLine}... and {errors.Count - MaxReportedErrors} more error(s)."
			: string.Empty;

		Fail($"{errors.Count} documentation validation error(s):{Environment.NewLine}{sample}{suffix}");
	}

	private sealed class TocEntry
	{
		public string Name { get; set; } = string.Empty;
		public string Href { get; set; } = string.Empty;
		public List<TocEntry> Items { get; set; } = new List<TocEntry>();
	}

	private sealed class LanguageEntry
	{
		[JsonPropertyName("code")]
		public string Code { get; init; } = string.Empty;

		[JsonPropertyName("name")]
		public string Name { get; init; } = string.Empty;

		[JsonPropertyName("flag")]
		public string Flag { get; init; } = string.Empty;

		[JsonPropertyName("default")]
		public bool IsDefault { get; init; }

		[JsonPropertyName("order")]
		public int Order { get; init; } = int.MaxValue;
	}

	private readonly record struct PathResolution(bool Exists, bool ExactCase, string FullPath, string ActualRelativePath);

	private readonly record struct MarkdownTarget(string Language, bool Exists, bool ExactCase, string FullPath, string ActualRelativePath);

	private readonly record struct MarkdownStructure(
		IReadOnlyList<int> HeadingLevels,
		IReadOnlyList<string> CodeBlockLanguages,
		IReadOnlyList<string> ImageUrls,
		IReadOnlyList<TableShape> TableShapes,
		IReadOnlyList<string> AdmonitionKinds);

	private readonly record struct TableShape(int Columns, int BodyRows);

	private readonly record struct CodeComment(string Text, int Line);

	private readonly record struct CodeOutputString(string Text, int Line);

	private sealed record LocalizedDocumentationAudit(
		IReadOnlyList<string> Languages,
		IReadOnlyList<LocalizedAuditLanguageStats> Stats,
		IReadOnlyList<AuditIssue> Issues);

	private readonly record struct LocalizedAuditLanguageStats(
		string Language,
		int MarkdownFiles,
		int TextFiles,
		int CodeOutputStrings,
		int CodeComments);

	private readonly record struct AuditIssue(string Category, string File, int Line, string Message)
	{
		public string Location => Line > 0 ? $"{File}:{Line}" : File;

		public override string ToString()
			=> $"{Location}: [{Category}] {Message}";
	}
}
