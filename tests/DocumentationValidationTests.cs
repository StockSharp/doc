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

	private static readonly string[] _translationArtifactMarkers =
	[
		"XZX",
	];

	private static readonly string[] _knownEnglishUiPhrases =
	[
		"Add button",
		"Add Designer strategy",
		"Account API",
		"Apply changes",
		"Aster Code endpoints",
		"Cloud panel",
		"Connect button",
		"Console App",
		"Connection type",
		"Control Panel → User Accounts → Credential Manager",
		"Ctrl + Left Mouse Button",
		"Ctrl + Right Mouse Button",
		"Dates format",
		"Auth path",
		"Check dates",
		"Check revocation",
		"Enable spot",
		"Exchange endpoint",
		"File -> Allow Remoting",
		"File log",
		"File → New Solution",
		"File → New → Project",
		"Group ID",
		"Getting started",
		"history plant",
		"Host name",
		"Info endpoint",
		"Log (address)",
		"Market data fields",
		".NET / .NET Core → Console Application",
		"Operating mode",
		"Order book channel",
		"Path to logs",
		"Point (admin)",
		"Point (data)",
		"Point (history)",
		"Point (positions)",
		"Point (transactions)",
		"Private websocket stream",
		"Price Step",
		"Manage NuGet Packages",
		"More info",
		"Open debug launch profiles UI",
		"Remote Manager",
		"Remote mode",
		"Run anyway",
		"send command",
		"Server type",
		"Settings file",
		"Settings → Build, Execution, Deployment → NuGet → Sources",
		"Solution Explorer",
		"Software ID",
		"Spot account and trading API",
		"Spot websocket account info",
		"Spot websocket market data",
		"Start strategy",
		"Target Framework",
		"Time Frame",
		"Time zone",
		"Tools → Options → NuGet Package Manager → Package Sources",
		"trading demo",
		"User name (hist)",
		"Validate remote",
		"Verification code",
		"Volume Step",
		"Websocket API docs",
		"Websocket channels",
		"Websocket introduction",
		"Work schedule",
		"WPF Application",
	];

	private static readonly HashSet<string> _knownEnglishBoldUiLabels = new(StringComparer.OrdinalIgnoreCase)
	{
		"Account index",
		"API Key",
		"API key index",
		"Balance",
		"Broker",
		"Candles",
		"Clearing account",
		"Demo",
		"Derivatives mode",
		"Expires after",
		"Info endpoint / Exchange endpoint / WS endpoint",
		"Indicator",
		"Key",
		"Licenses",
		"Market slippage",
		"Passphrase",
		"Private key",
		"Section",
		"Sections",
		"Secret",
		"Starknet account",
		"Starknet key",
		"Settings",
		"Strategies",
		"Strategy",
		"Testnet",
		"Vault address",
		"Wallet address",
		"WS read-only mode",
	};

	private static readonly Regex[] _knownEnglishLowercaseProseTerms =
	[
		new(@"\bpassphrases?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
	];

	private static readonly string[] _knownEnglishMarkdownCodeBlockListLabels =
	[
		"Logging",
	];

	private static readonly string[] _knownEnglishDesignerElementColorLabels =
	[
		"Black",
		"Dark green",
		"Dark cyan",
		"Cyan",
		"Orange red",
		"Dark goldenrod",
		"Olive",
		"Pale violet red",
		"Dark olive green",
		"Dodger blue",
		"Medium sea green",
		"Dark slate blue",
		"Brown",
		"Deep pink",
		"Dark khaki",
		"Dark blue",
		"Saddle brown",
		"Gainsboro",
		"Tan",
		"Purple",
	];

	private static readonly HashSet<string> _knownEnglishSectionLabels = new(StringComparer.OrdinalIgnoreCase)
	{
		"Description",
		"Example",
		"Examples",
		"Input",
		"Incoming Socket",
		"Incoming Sockets",
		"Inputs",
		"Next Steps",
		"Output",
		"Outgoing Socket",
		"Outgoing Sockets",
		"Outputs",
		"Overview",
		"Parameters",
		"Prerequisites",
		"Properties",
		"Result",
		"Results",
		"See Also",
		"Usage",
	};

	private static readonly string[] _knownEnglishNotificationDataTypeLabels =
	[
		"Customer code",
		"Client code",
		"Server time",
		"Data type",
		"Visible volume",
		"Message to order",
		"Order expiration time",
		"Execution condition",
		"Trade initiator",
		"Identifier (user)",
	];

	private static readonly string[] _knownEnglishNotificationFormLabels =
	[
		"Window",
		"Melody",
		"Music",
		"Speech",
		"Voice",
		"Log",
		"Disabled",
	];

	private static readonly HashSet<string> _cjkLanguageCodes = new(StringComparer.OrdinalIgnoreCase)
	{
		"ja",
		"zh",
	};

	private static readonly HashSet<string> _translatableEnglishCjkTocNames = new(StringComparer.OrdinalIgnoreCase)
	{
		"Backtesting",
		"Chart",
		"Debugging",
		"Export",
		"Import",
		"Installer",
		"Orders",
		"Portfolios",
		"Setup",
		"Synchronization",
		"Trades",
		"Tutorial",
		"Videos",
	};

	private static readonly HashSet<string> _allowedInvariantCjkTocNames = new(StringComparer.OrdinalIgnoreCase)
	{
		"API",
		"C#",
		"CSV",
		"Designer",
		"DEX",
		"F#",
		"FIX Server",
		"Hydra",
		"Level 1",
		"Level1",
		"MATLAB",
		"OAuth",
		"Python",
		"RemoteManager",
		"RSS",
		"Runner",
		"Shell",
		"Store",
		"Terminal",
		"UDP Dumper",
	};

	private static readonly HashSet<string> _knownEnglishCodeCommentLabels = new(StringComparer.OrdinalIgnoreCase)
	{
		"Backtesting",
		"Connector",
		"Core",
		"Indicators",
		"Localization",
		"Localization (Russian)",
		"parameters",
		"Strategies and indicators",
	};

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
		"average",
		"based",
		"buy",
		"buys",
		"canceled",
		"cancelled",
		"callable",
		"candle",
		"candles",
		"candlestick",
		"change",
		"commission",
		"completed",
		"configured",
		"connect",
		"connection",
		"connector",
		"created",
		"data",
		"delta",
		"description",
		"difference",
		"diff",
		"direction",
		"display",
		"error",
		"events",
		"exchange",
		"exit",
		"failed",
		"found",
		"general",
		"greater",
		"image",
		"imbalance",
		"index",
		"indicator",
		"input",
		"instrument",
		"instruments",
		"invalid",
		"invoked",
		"invokes",
		"large",
		"level",
		"length",
		"less",
		"logic",
		"loaded",
		"logging",
		"long",
		"lost",
		"main",
		"marked",
		"message",
		"method",
		"max",
		"maximum",
		"min",
		"minimum",
		"mode",
		"moving",
		"my",
		"not",
		"online",
		"operation",
		"order",
		"orders",
		"parameter",
		"parameters",
		"pattern",
		"patterns",
		"percentage",
		"period",
		"price",
		"processed",
		"processes",
		"profit",
		"rate",
		"received",
		"register",
		"registration",
		"registered",
		"representing",
		"required",
		"rule",
		"search",
		"save",
		"sell",
		"sells",
		"series",
		"setting",
		"settings",
		"short",
		"signal",
		"simple",
		"sockets",
		"spread",
		"subscribers",
		"subscribe",
		"successfully",
		"threshold",
		"total",
		"trade",
		"trades",
		"transitioned",
		"triggered",
		"type",
		"unsubscribe",
		"unsupported",
		"value",
		"values",
		"volume",
		"volatility",
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
		"C#",
		"Chart",
		"Debugging",
		"Designer",
		"Emulation",
		"Export",
		"F#",
		"Flag",
		"Hedging",
		"Hydra",
		"Identifier *@ALL",
		"Identifier \\*@ALL",
		"Import",
		"Index",
		"Indexer",
		"Installer",
		"Interpretation",
		"Interface",
		"JetBrains Rider",
		"Level 1",
		"Level1",
		"MATLAB",
		"Open Source",
		"Orders",
		"Portfolios",
		"Position",
		"RemoteManager",
		"Rider",
		"Runner",
		"Shell",
		"Simulator",
		"Start / Stop",
		"Strikes",
		"Terminal",
		"Testing",
		"Ticks",
		"Trades",
		"Trading",
		"UDP Dumper",
		"Variable",
		"Via JetBrains Rider",
		"Via Visual Studio",
		"Visual Studio",
		"Visual Studio 2022+",
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
		"Chaikin's Volatility",
		"DEMA",
		"DeMarker",
		"Fix Trading Community",
		"Force Index",
		"Fractal Adaptive Moving Average",
		"Ichimoku",
		"KAMA",
		"Kaufman Adaptive Moving Average",
		"Money Flow Index",
		"Moving Median",
		"Peak",
		"QStick",
		"RAVI",
		"RSI",
		"Smoothed Moving Average",
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

	private static Dictionary<string, string> ReadLanguageStrings(string stringsPath, List<string> errors)
	{
		try
		{
			var strings = JsonSerializer.Deserialize<Dictionary<string, string>>(ReadAllText(stringsPath), _json);
			if (strings is not null)
				return strings;

			errors.Add($"{RelativeToRepo(stringsPath)} must contain a JSON object.");
			return null;
		}
		catch (Exception ex)
		{
			errors.Add($"{RelativeToRepo(stringsPath)} is invalid JSON: {ex.Message}");
			return null;
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
	public void LocalizedTocFilesMatchDefaultStructure()
	{
		var errors = new List<string>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);

		foreach (var defaultToc in Directory.EnumerateFiles(defaultRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var relative = Path.GetRelativePath(defaultRoot, defaultToc).Replace('\\', '/');
			var expected = ReadTocEntries(defaultToc, errors);
			if (expected is null)
				continue;

			foreach (var lang in GetTranslatedContentLanguages())
			{
				var localizedToc = Path.Combine(_repoRoot, lang, relative.Replace('/', Path.DirectorySeparatorChar));
				if (!File.Exists(localizedToc))
				{
					errors.Add($"{lang}/{relative}: localized TOC file is missing.");
					continue;
				}

				var actual = ReadTocEntries(localizedToc, errors);
				if (actual is null)
					continue;

				ValidateTocStructureMatchesDefault(relative, localizedToc, expected, actual, errors, string.Empty);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCjkTocNamesDoNotLookLikeEnglish()
	{
		var errors = new List<string>();

		foreach (var lang in GetTranslatedContentLanguages().Where(_cjkLanguageCodes.Contains))
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var tocPath in Directory.EnumerateFiles(langRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var entries = ReadTocEntries(tocPath, errors);
				if (entries is null)
					continue;

				var flatEntries = FlattenTocEntries(entries).ToArray();
				var nameLines = EnumerateTocNameLines(tocPath).ToArray();
				var count = Math.Min(flatEntries.Length, nameLines.Length);

				for (var i = 0; i < count; i++)
				{
					var name = nameLines[i].Name;
					var href = flatEntries[i].Href;
					if (!IsLikelyUntranslatedEnglishCjkTocName(name, href))
						continue;

					errors.Add($"{RelativeToRepo(tocPath)}:{nameLines[i].Line}: TOC name looks like untranslated English for {lang}. Localize it or add a deliberate allowlist entry. Name: {name}");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCjkMarkdownHeadingsDoNotKeepKnownEnglishNames()
	{
		var errors = new List<string>();

		foreach (var lang in GetTranslatedContentLanguages().Where(_cjkLanguageCodes.Contains))
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var heading in EnumerateHeadingTexts(ReadAllText(file)))
				{
					if (!_translatableEnglishCjkTocNames.Contains(heading.Text))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{heading.Line}: markdown heading looks like untranslated English for {lang}. Localize it or add a deliberate allowlist entry. Heading: {heading.Text}");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedLanguageStringsMatchDefaultKeys()
	{
		var errors = new List<string>();
		var defaultPath = Path.Combine(_repoRoot, DefaultLanguage, "strings.json");
		var defaultStrings = ReadLanguageStrings(defaultPath, errors);
		if (defaultStrings is null)
		{
			AssertNoErrors(errors);
			return;
		}

		var defaultKeys = defaultStrings.Keys.ToHashSet(StringComparer.Ordinal);

		foreach (var lang in GetContentLanguages().Where(lang => !lang.Equals(DefaultLanguage, StringComparison.OrdinalIgnoreCase)))
		{
			var stringsPath = Path.Combine(_repoRoot, lang, "strings.json");
			var strings = ReadLanguageStrings(stringsPath, errors);
			if (strings is null)
				continue;

			foreach (var key in defaultKeys.Except(strings.Keys, StringComparer.Ordinal).Order(StringComparer.Ordinal))
				errors.Add($"{lang}/strings.json is missing localization key '{key}' from {DefaultLanguage}/strings.json.");

			foreach (var key in strings.Keys.Except(defaultKeys, StringComparer.Ordinal).Order(StringComparer.Ordinal))
				errors.Add($"{lang}/strings.json contains extra localization key '{key}' not present in {DefaultLanguage}/strings.json.");
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
			var defaultHeadings = EnumerateHeadingTexts(ReadAllText(defaultFile)).ToArray();
			if (defaultHeadings.Length == 0)
				continue;

			foreach (var lang in GetLocalizedContentQualityLanguages())
			{
				var langRoot = Path.Combine(_repoRoot, lang);
				var file = Path.Combine(langRoot, relative.Replace('/', Path.DirectorySeparatorChar));
				if (!File.Exists(file))
					continue;

				var localizedHeadings = EnumerateHeadingTexts(ReadAllText(file)).ToArray();
				var count = Math.Min(defaultHeadings.Length, localizedHeadings.Length);

				for (var i = 0; i < count; i++)
				{
					var defaultHeading = defaultHeadings[i].Text;
					var localizedHeading = localizedHeadings[i];
					if (!localizedHeading.Text.Equals(defaultHeading, StringComparison.Ordinal))
						continue;

					if (IsAllowedInvariantHeading(relative, localizedHeading.Text))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{localizedHeading.Line}: heading is identical to the English source heading '{localizedHeading.Text}'. Localize it or add a deliberate allowlist entry.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownLinkLabelsDoNotKeepEnglishTargetHeadingsUnexpectedly()
	{
		var errors = new List<string>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);

		foreach (var lang in GetLocalizedContentQualityLanguages())
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

		foreach (var lang in GetContentLanguages().Where(lang => !lang.Equals(DefaultLanguage, StringComparison.OrdinalIgnoreCase)))
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
	public void LocalizedNotificationDataTypeListsDoNotKeepEnglishLabels()
	{
		var errors = new List<string>();
		var relativePaths = new[]
		{
			"topics/api/graphical_user_interface/notification_settings_window.md",
			"topics/terminal/notifications/notifications_setup.md",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(langRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));

				if (!File.Exists(file))
					continue;

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					foreach (var label in _knownEnglishNotificationDataTypeLabels)
					{
						if (!ContainsStandaloneText(text, label))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: notification data type list keeps English label '{label}'. Localize the label in the target language.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedNotificationFormListsDoNotKeepEnglishLabels()
	{
		var errors = new List<string>();
		var relativePaths = new[]
		{
			"topics/api/graphical_user_interface/notification_settings_window.md",
			"topics/terminal/notifications.md",
			"topics/terminal/notifications/notifications_setup.md",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(langRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));

				if (!File.Exists(file))
					continue;

				foreach (var label in EnumerateMarkdownBoldTexts(ReadAllText(file)))
				{
					if (!_knownEnglishNotificationFormLabels.Contains(label.Text))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{label.Line}: notification form list keeps English label '{label.Text}'. Localize the label in the target language.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedConnectorDocsDoNotKeepEnglishCredentialDescriptions()
	{
		var errors = new List<string>();
		var credentialPattern = new Regex(@"^\s*[-*]\s+\*\*[^*]+\*\*\s*(?:-|—|:)\s*(?:Login|Password)\.\s*$|\b(?:Login|Password) adicional\b|\bZusätzliches Login\.", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var connectorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "connectors");

			if (!Directory.Exists(connectorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(connectorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var line = 1;
				using var reader = new StringReader(ReadAllText(file));

				for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
				{
					if (!credentialPattern.IsMatch(text))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: connector field description keeps untranslated English credential text. Localize 'Login' and 'Password' descriptions.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedBlackwoodFusionGraphicalConfigurationDoesNotKeepEnglishFieldLabels()
	{
		var errors = new List<string>();
		const string relativePath = "topics/api/connectors/stock_market/blackwood_fusion/graphical_configuration_blackwood_fusion.md";
		var fieldPattern = new Regex(@"\*\*(?:Market data|History|Transactions|Override)\*\*|!\[API GUI Settings Fusion(?: \(Blackwood\))?\]", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));

			if (!File.Exists(file))
				continue;

			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				var match = fieldPattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Blackwood Fusion graphical configuration keeps English UI label '{match.Value}'. Localize the visible field label.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownDoesNotKeepKnownEnglishBoldUiLabels()
	{
		var errors = new List<string>();
		var boldPattern = new Regex(@"\*\*(?<label>[^*`\r\n]+)\*\*", RegexOptions.CultureInvariant);

		foreach (var lang in GetContentLanguages().Where(lang => !lang.Equals(DefaultLanguage, StringComparison.OrdinalIgnoreCase)))
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					foreach (Match match in boldPattern.Matches(text))
					{
						var label = Regex.Replace(match.Groups["label"].Value.Trim(), @"\s+", " ", RegexOptions.CultureInvariant);
						if (!_knownEnglishBoldUiLabels.Contains(label))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: contains known untranslated English bold UI label '{label}'.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownCodeBlockListLabelsDoNotKeepKnownEnglishLabels()
	{
		var errors = new List<string>();

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var line in EnumerateMarkdownTextLikeCodeBlockLines(ReadAllText(file)))
				{
					foreach (var label in _knownEnglishMarkdownCodeBlockListLabels)
					{
						if (!Regex.IsMatch(line.Text, @"^\s*-\s+" + Regex.Escape(label) + @"\s*:", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line.Line}: markdown code block list keeps English label '{label}'. Localize the label in the target language.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDesignerElementDocsDoNotKeepEnglishColorLabels()
	{
		var errors = new List<string>();
		const string relativePath = "topics/designer/strategies/using_visual_designer/elements.md";

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			foreach (var label in EnumerateMarkdownBoldTexts(ReadAllText(file)))
			{
				foreach (var color in _knownEnglishDesignerElementColorLabels)
				{
					if (!ContainsStandaloneText(label.Text, color))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{label.Line}: designer element color legend keeps English color label '{color}'. Localize color names in the target language.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownDoesNotKeepKnownEnglishLowercaseProseTerms()
	{
		var errors = new List<string>();

		foreach (var lang in GetContentLanguages().Where(lang => !lang.Equals(DefaultLanguage, StringComparison.OrdinalIgnoreCase)))
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					foreach (var pattern in _knownEnglishLowercaseProseTerms)
					{
						var match = pattern.Match(text);
						if (!match.Success)
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: contains known untranslated English prose term '{match.Value}'.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownDoesNotKeepEnglishExampleAbbreviation()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\be\.g\.?", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized markdown keeps English example abbreviation '{match.Value}'. Localize it in prose.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownDoesNotKeepKnownEnglishSectionLabels()
	{
		var errors = new List<string>();

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var relative = Path.GetRelativePath(langRoot, file).Replace('\\', '/');

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var normalized = NormalizeMarkdownTextForTranslationCheck(text);
					if (!_knownEnglishSectionLabels.Contains(normalized) || IsAllowedInvariantMarkdownText(relative, text, normalized))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: markdown section label '{normalized}' is still English. Localize it or add a deliberate allowlist entry.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownTextIsTranslatedFromDefaultLanguage()
	{
		var errors = new List<string>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);
		var defaultTextByRelativePath = BuildDefaultMarkdownTextMap(defaultRoot);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var relative = Path.GetRelativePath(langRoot, file).Replace('\\', '/');
				if (!defaultTextByRelativePath.TryGetValue(relative, out var defaultTexts))
					continue;

				foreach (var line in EnumerateTranslatableMarkdownTextLines(ReadAllText(file), relative))
				{
					if (!defaultTexts.Contains(line.Text))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line.Line}: markdown text is identical to the English source. Localize it or add a deliberate allowlist entry. Text: {Truncate(line.Text, 180)}");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownTableCellsAreTranslatedFromDefaultLanguage()
	{
		var errors = new List<string>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);
		var defaultTableCellsByRelativePath = BuildDefaultMarkdownTableCellMap(defaultRoot);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var relative = Path.GetRelativePath(langRoot, file).Replace('\\', '/');
				if (!defaultTableCellsByRelativePath.TryGetValue(relative, out var defaultTableCells))
					continue;

				foreach (var cell in EnumerateMarkdownTableCellTexts(ReadAllText(file)))
				{
					var normalized = NormalizeMarkdownTextForTranslationCheck(cell.Text);
					if (normalized.Length == 0 || !defaultTableCells.Contains(normalized))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{cell.Line}: markdown table cell is identical to the English source. Localize it or add a deliberate allowlist entry. Cell: {Truncate(cell.Text, 180)}");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownPlainTextCodeBlocksAreTranslatedFromDefaultLanguage()
	{
		var errors = new List<string>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);
		var defaultPlainTextBlocksByRelativePath = BuildDefaultMarkdownPlainTextBlockMap(defaultRoot);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var relative = Path.GetRelativePath(langRoot, file).Replace('\\', '/');
				if (!defaultPlainTextBlocksByRelativePath.TryGetValue(relative, out var defaultPlainTextBlocks))
					continue;

				foreach (var block in EnumerateMarkdownPlainTextBlocks(ReadAllText(file)))
				{
					var normalized = NormalizeMarkdownPlainTextBlockForTranslationCheck(block.Text);
					if (normalized.Length == 0 || !defaultPlainTextBlocks.Contains(normalized))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{block.Line}: markdown plain text code block is identical to the English source. Localize it or add a deliberate allowlist entry. Text: {Truncate(block.Text, 180)}");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownTextDoesNotLookLikeEnglish()
	{
		var errors = new List<string>();

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var relative = Path.GetRelativePath(langRoot, file).Replace('\\', '/');

				foreach (var line in EnumerateLikelyEnglishMarkdownTextLines(ReadAllText(file), relative))
					errors.Add($"{RelativeToRepo(file)}:{line.Line}: markdown text looks like untranslated English. Localize it or add a deliberate allowlist entry. Text: {Truncate(line.Text, 180)}");
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
	public void LocalizedCodeUiStringsAreTranslatedFromDefaultLanguage()
	{
		var errors = new List<string>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);

		foreach (var defaultFile in Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var relative = Path.GetRelativePath(defaultRoot, defaultFile).Replace('\\', '/');
			var defaultUiStrings = EnumerateCodeUiStrings(ReadAllText(defaultFile))
				.Select(uiString => NormalizeCodeUiStringForTranslationCheck(uiString.Text))
				.Where(IsTranslatableEnglishCodeUiString)
				.ToHashSet(StringComparer.Ordinal);

			if (defaultUiStrings.Count == 0)
				continue;

			foreach (var lang in GetTranslatedContentLanguages())
			{
				var langRoot = Path.Combine(_repoRoot, lang);
				var file = Path.Combine(langRoot, relative.Replace('/', Path.DirectorySeparatorChar));

				if (!File.Exists(file))
					continue;

				foreach (var uiString in EnumerateCodeUiStrings(ReadAllText(file)))
				{
					var normalized = NormalizeCodeUiStringForTranslationCheck(uiString.Text);
					if (!defaultUiStrings.Contains(normalized))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{uiString.Line}: code UI string is identical to the English source. Localize it or add a deliberate allowlist entry. String: {Truncate(uiString.Text, 180)}");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCodeStringLiteralsAreTranslatedFromDefaultLanguage()
	{
		var errors = new List<string>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);

		foreach (var defaultFile in Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var relative = Path.GetRelativePath(defaultRoot, defaultFile).Replace('\\', '/');
			var defaultLiterals = EnumerateCodeStringLiterals(ReadAllText(defaultFile))
				.Select(literal => NormalizeCodeStringLiteralForTranslationCheck(literal.Text))
				.Where(IsTranslatableEnglishCodeStringLiteral)
				.ToHashSet(StringComparer.Ordinal);

			if (defaultLiterals.Count == 0)
				continue;

			foreach (var lang in GetTranslatedContentLanguages())
			{
				var langRoot = Path.Combine(_repoRoot, lang);
				var file = Path.Combine(langRoot, relative.Replace('/', Path.DirectorySeparatorChar));

				if (!File.Exists(file))
					continue;

				foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
				{
					var normalized = NormalizeCodeStringLiteralForTranslationCheck(literal.Text);
					if (!defaultLiterals.Contains(normalized))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{literal.Line}: code string literal is identical to the English source. Localize it or add a deliberate allowlist entry. String: {Truncate(literal.Text, 180)}");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCodeStringLiteralsDoNotLookLikeEnglish()
	{
		var errors = new List<string>();

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
				{
					var normalized = NormalizeCodeStringLiteralForTranslationCheck(literal.Text);
					if (!IsLikelyUntranslatedEnglishCodeStringLiteral(normalized))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{literal.Line}: code string literal looks like untranslated English. Localize it or add a deliberate allowlist entry. String: {Truncate(literal.Text, 180)}");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedAlertSystemSampleStringsDoNotKeepEnglishSignals()
	{
		var errors = new List<string>();
		var relativePath = "topics/api/strategies/alert_system.md";
		var patterns = new[]
		{
			new Regex(@"\bLevel breakout\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
			new Regex(@"\bTrading signal\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
			new Regex(@"\bupward!?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
			new Regex(@"\bdownward\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
			{
				foreach (var pattern in patterns)
				{
					if (!pattern.IsMatch(literal.Text))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{literal.Line}: alert-system code sample keeps English alert text '{literal.Text}'. Localize user-visible alert strings.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCodeCommentsDoNotKeepKnownEnglishSignalLabels()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:Buy|Sell) signal\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
				{
					if (!pattern.IsMatch(comment.Text))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: code comment keeps English signal label '{comment.Text}'. Localize the comment.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownImageAltTextsAreTranslatedFromDefaultLanguage()
	{
		var errors = new List<string>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);

		foreach (var defaultFile in Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var relative = Path.GetRelativePath(defaultRoot, defaultFile).Replace('\\', '/');
			var defaultAltTexts = EnumerateMarkdownImageAltTexts(ReadAllText(defaultFile))
				.Select(altText => NormalizeMarkdownImageAltTextForTranslationCheck(altText.Text))
				.Where(IsTranslatableEnglishMarkdownImageAltText)
				.ToHashSet(StringComparer.Ordinal);

			if (defaultAltTexts.Count == 0)
				continue;

			foreach (var lang in GetTranslatedContentLanguages())
			{
				var langRoot = Path.Combine(_repoRoot, lang);
				var file = Path.Combine(langRoot, relative.Replace('/', Path.DirectorySeparatorChar));

				if (!File.Exists(file))
					continue;

				foreach (var altText in EnumerateMarkdownImageAltTexts(ReadAllText(file)))
				{
					var normalized = NormalizeMarkdownImageAltTextForTranslationCheck(altText.Text);
					if (!defaultAltTexts.Contains(normalized))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: image alt text is identical to the English source. Localize it or add a deliberate allowlist entry. Alt text: {Truncate(altText.Text, 180)}");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownImageAltTextsDoNotUseEnglishSampleLabels()
	{
		var errors = new List<string>();
		var samplePattern = new Regex(@"^sample(?:\b|[a-z])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var altText in EnumerateMarkdownImageAltTexts(ReadAllText(file)))
				{
					if (!samplePattern.IsMatch(altText.Text))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: image alt text keeps English sample label '{altText.Text}'. Localize the sample description.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownImageAltTextsDoNotLookLikeRawFileNames()
	{
		var errors = new List<string>();
		var rawFileNamePattern = new Regex(@"_|(?:\.(?:png|jpe?g|gif|webp|svg))\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
				{
					if (!rawFileNamePattern.IsMatch(altText.Text))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: image alt text looks like a raw file name '{altText.Text}'. Use a localized description instead.");
				}

				var lineStarts = GetLineStarts(markdown);
				foreach (Match match in Regex.Matches(markdown, @"!\[\[(?<alt>[^\]\r\n]+)\]\]", RegexOptions.CultureInvariant))
				{
					var alt = NormalizeMarkdownImageAltTextForTranslationCheck(match.Groups["alt"].Value);
					if (!rawFileNamePattern.IsMatch(alt))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{GetLineNumber(lineStarts, match.Index)}: image alt text looks like a raw wiki-style file name '{alt}'. Use a standard markdown image with a localized description instead.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownImageAltTextsDoNotMirrorImageFileNames()
	{
		var errors = new List<string>();

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var altText in EnumerateMarkdownImageAltTexts(ReadAllText(file)))
				{
					var urlPath = altText.Url.Split('#', '?')[0].Replace('/', Path.DirectorySeparatorChar);
					var imageStem = Path.GetFileNameWithoutExtension(urlPath);
					if (string.IsNullOrWhiteSpace(imageStem))
						continue;

					if (!AreMarkdownImageAltTextAndFileStemEquivalent(altText.Text, imageStem))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: image alt text mirrors image file name '{altText.Text}'. Use a localized description instead.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownImageAltTextsAreUniqueWithinEachFile()
	{
		var errors = new List<string>();

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var duplicateGroups = EnumerateMarkdownImageAltTexts(ReadAllText(file))
					.GroupBy(altText => altText.Text, StringComparer.Ordinal)
					.Where(group => group.Select(altText => altText.Url).Distinct(StringComparer.OrdinalIgnoreCase).Count() > 1);

				foreach (var group in duplicateGroups)
				{
					var lines = string.Join(", ", group.Select(altText => altText.Line));
					errors.Add($"{RelativeToRepo(file)}:{group.First().Line}: image alt text '{group.Key}' is reused for different images on lines {lines}. Use distinct descriptions.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorMarkdownImageAltTextsUseLocalizedChartDescriptions()
	{
		var errors = new List<string>();

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			var requiredWord = GetLocalizedIndicatorChartDescriptionWord(lang);

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var altText in EnumerateMarkdownImageAltTexts(ReadAllText(file)))
				{
					if (altText.Text.IndexOf(requiredWord, StringComparison.OrdinalIgnoreCase) >= 0)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: indicator image alt text '{altText.Text}' does not describe the chart in the localized language. Include '{requiredWord}' in the description.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownImageAltTextsDoNotKeepEnglishScreenshotWord()
	{
		var errors = new List<string>();

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var altText in EnumerateMarkdownImageAltTexts(ReadAllText(file)))
				{
					if (!Regex.IsMatch(altText.Text, @"\bScreenshot\b", RegexOptions.CultureInvariant))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: image alt text '{altText.Text}' keeps the English word 'Screenshot'. Use localized image wording.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownImageAltTextsDoNotKeepKnownEnglishProductLabels()
	{
		var errors = new List<string>();
		var prefixes = new[]
		{
			"API GUI ConnectorWindow",
			"API GUI Thems",
			"Aws 3 Create Bucket Name",
			"Aws3 CreateBucket",
			"Designer Alert Bell",
			"Designer Black Basket",
			"Designer Components",
			"Designer Creation tool",
			"Designer_Creation_Strategy_Dll",
			"Designer_Creation_of_element_containing_source_code",
			"Designer Crossing",
			"Designer Debug",
			"Designer Delay",
			"Designer edit button",
			"Designer Edit Tool",
			"Designer Event model",
			"Designer Graph options positions",
			"Designer Options Board",
			"Designer Protect positions",
			"Designer Random",
			"Designer Schedule",
			"Designer Schedule PU",
			"Designer Security mapping",
			"Designer_Source_Code_Elem",
			"Designer_Source_Code_Indicator",
			"Designer_Source_Code_OrderBook",
			"Designer Stopping point",
			"Designer Sync",
			"Designer Tape",
			"Designer Working time",
			"Designer_Runner_1",
			"DesignerDeleteButton",
			"DesignerPlusButton",
			"GUI LogControl",
			"GUI SecurityPicker2",
			"Gui ClasterChart",
			"Hydra server",
			"HydraGluingCheckData",
			"HydraGluingCSCustom",
			"HydraGluingTrades",
			"MT Install",
			"OAuth Start",
			"Profile",
			"Shell Common",
			"Shell RemoteManager",
			"Shell custom strategy",
			"Shell run Designer strategy",
			"Terminal Graph options positions",
			"Terminal OrderPanel",
			"Terminal Tape",
			"Terminal news",
			"Terminal option desk",
			"Terminal orderlog",
			"Terminal securities",
			"hydra add",
			"hydra choose ITCH Plaza",
			"hydra choose securitiy",
			"hydra edit",
			"hydra export",
			"hydra export TSLab Meta Stock",
			"hydra find",
			"hydra securities choose all",
			"hydra securities edit",
			"hydra security edit",
			"hydra security full list",
			"hydra source add",
			"hydra source choose",
			"hydra tasks backup desk",
			"hydra tasks converter",
			"hydra tasks export",
		};
		var pattern = new Regex($"^(?:{string.Join("|", prefixes.OrderByDescending(prefix => prefix.Length).Select(Regex.Escape))})\\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var altText in EnumerateMarkdownImageAltTexts(ReadAllText(file)))
				{
					var match = pattern.Match(altText.Text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: image alt text keeps English product label '{match.Value}'. Localize the image description.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedConnectorDocsDoNotKeepEnglishApiGuiSettingsLabels()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"!\[API GUI Settings\b|^# .*?\b(?:Market data|Transactions)\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var connectorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "connectors");

			if (!Directory.Exists(connectorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(connectorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var line = 1;
				using var reader = new StringReader(ReadAllText(file));

				for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: connector documentation keeps English API GUI settings label '{match.Value}'. Localize image alt text and visible headings.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCjkConnectorDocsDoNotKeepKnownEnglishFieldLabels()
	{
		var errors = new List<string>();
		var labels = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"Active",
			"Board",
			"Client",
			"Compression",
			"Database",
			"Delay",
			"Duplicate",
			"encrypted channel",
			"Futures",
			"Initially",
			"Interval",
			"LMAX location",
			"Main",
			"message",
			"message adapter",
			"Recovery",
			"Reconnection",
			"Replay",
			"Securities",
			"Server",
			"Spot",
			"Swap",
			"Token",
			"Tokens",
			"Transactions only",
			"Timeout",
			"User",
		};

		foreach (var lang in new[] { "ja", "zh" })
		{
			var connectorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "connectors");

			if (!Directory.Exists(connectorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(connectorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var label in EnumerateMarkdownBoldTexts(ReadAllText(file)))
				{
					if (!labels.Contains(label.Text))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{label.Line}: CJK connector documentation keeps English label '{label.Text}'. Localize visible labels.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCjkConnectorDocsDoNotKeepEnglishMessageDirectionLabels()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\*(?:incoming|outgoing)\*", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in new[] { "ja", "zh" })
		{
			var connectorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "connectors");

			if (!Directory.Exists(connectorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(connectorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var line = 1;
				using var reader = new StringReader(ReadAllText(file));

				for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: CJK connector documentation keeps English message direction label '{match.Value}'. Localize visible direction labels.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMetaTraderDocsDoNotKeepEnglishMenuLabels()
	{
		var errors = new List<string>();
		const string relativePath = "topics/api/connectors/forex/metatrader.md";
		var pattern = new Regex(@"Tools(?:->|\\-\\>)Options|Experts Advisors|Allow DLL imports|\*\*Refresh\*\*|\*\*Attach to a chart\*\*|A также|%your_user_name%|%many_letters_and_numbers%|login-password", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));

			if (!File.Exists(file))
				continue;

			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				var match = pattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: MetaTrader instructions keep English menu label '{match.Value}'. Localize visible menu and action labels.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownLinkLabelsAreTranslatedFromDefaultLanguage()
	{
		var errors = new List<string>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);

		foreach (var defaultFile in Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var relative = Path.GetRelativePath(defaultRoot, defaultFile).Replace('\\', '/');
			var defaultLinkLabels = EnumerateMarkdownLinkLabels(ReadAllText(defaultFile))
				.Where(label => IsTranslatableEnglishMarkdownLinkLabel(label.Text, label.Url))
				.Select(label => NormalizeMarkdownLinkLabelForTranslationCheck(label.Text))
				.ToHashSet(StringComparer.Ordinal);

			if (defaultLinkLabels.Count == 0)
				continue;

			foreach (var lang in GetTranslatedContentLanguages())
			{
				var langRoot = Path.Combine(_repoRoot, lang);
				var file = Path.Combine(langRoot, relative.Replace('/', Path.DirectorySeparatorChar));

				if (!File.Exists(file))
					continue;

				foreach (var linkLabel in EnumerateMarkdownLinkLabels(ReadAllText(file)))
				{
					var normalized = NormalizeMarkdownLinkLabelForTranslationCheck(linkLabel.Text);
					if (!defaultLinkLabels.Contains(normalized))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{linkLabel.Line}: link label is identical to the English source. Localize it or add a deliberate allowlist entry. Link label: {Truncate(linkLabel.Text, 180)}");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorListDescriptionsDoNotStartWithEnglishPhrases()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "api", "indicators", "list_of_indicators.md");
		var defaultLeadsByUrl = ReadIndicatorListDescriptionLeads(Path.Combine(_repoRoot, DefaultLanguage, relative));

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);

			foreach (var entry in EnumerateIndicatorListDescriptionLeads(file))
			{
				if (!defaultLeadsByUrl.TryGetValue(entry.Url, out var defaultLead))
					continue;

				if (!entry.Lead.Equals(defaultLead, StringComparison.OrdinalIgnoreCase))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{entry.Line}: indicator list description starts with the English source phrase '{entry.Lead}'. Start the localized description in the target language; keep invariant indicator names in the link label if needed.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorExpansionTextDoesNotKeepEnglishPhrases()
	{
		var errors = new List<string>();
		var relativePaths = new[]
		{
			"topics/api/indicators/list_of_indicators/time_weighted_average_price.md",
			"topics/api/indicators/list_of_indicators/volume_weighted_average_price.md",
			"topics/api/indicators/list_of_indicators/volume_weighted_ma.md",
		};
		var phrases = new[]
		{
			"Time-Weighted Average Price",
			"Time-Weighted precio medio",
			"Volume Weighted Average Price",
			"Volume Weighted Moving Average",
			"Volume Weighted precio medio",
			"Volume Weighted MA",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(langRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));

				if (!File.Exists(file))
					continue;

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					foreach (var phrase in phrases)
					{
						if (!ContainsStandaloneText(text, phrase))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: indicator expansion keeps English phrase '{phrase}'. Localize the phrase and keep only the acronym as invariant.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorFormulaNotesDoNotKeepEnglishProseFragments()
	{
		var errors = new List<string>();
		var phrases = new[]
		{
			"Exponential Moving Average of price over Period",
			"Exponential Moving Average of ATR over Period",
			"Highest High value over Period",
			"Lowest Low value over Period",
			"Average TR value over Period",
			"Highest value of upper band over StopPeriod",
			"Lowest value of lower band over StopPeriod",
			"sum of all w(i)",
			"Money Flow Volume over Length period",
			"Volume over Length period",
			"Sum(TR(i)) for i from 1 to Length",
			"maximum High value over Length period",
			"minimum Low value over Length period",
			"Highest High over Length period",
			"Lowest Low over Length period",
			"Average Volume over period",
			"Cumulative Sum of Normalized Values",
			"Number of instruments reaching new highs over the Length period",
			"Number of instruments reaching new lows over the Length period",
			"Average True Range over Length period",
			"Number of non-sequential pairs",
			"Total number of pairs",
			"Ratio over last Length periods",
			"Number of rising periods over Length periods",
			"Number of Periods",
			"Sum of all Gains over Length period",
			"Sum of all Losses over Length period",
			"Maximum(Momentum) over Length period",
			"Minimum(Momentum) over Length period",
			"ATR = Average True Range",
			"Upper Band = ML + (D x Standard Deviation)",
			"Lower Band = ML - (D x Standard Deviation)",
			"Lower Bollinger Band = SMA - (StdDevMultiplier * Standard Deviation)",
			"Upper Bollinger Band = SMA + (StdDevMultiplier * Standard Deviation)",
			"StdDevMultiplier * Standard Deviation",
			"Standard Deviation -",
			"Standard Deviation (StdDev)",
			"Standard Deviation = StdDev(Close, Length)",
			"/ Standard Deviation",
			"Typical Price = (High + Low + Close) / 3",
			"Typical Price (TP) = (High + Low + Close) / 3",
			"Sum(Typical Price)",
			"ATR = Length 期間の Average True Range",
			"ATR = Length 周期内 Average True Range",
			"Price Change = Typical Price",
			"Volume-Weighted Price Change = Price Change",
			"Normalized Value = Volume-Weighted Price Change",
			"Price Volatility",
			"for i from (current - Length + 1) to current",
			"For first calculation:",
			"Extract Top N Spectral Components based on amplitude",
			"Reconstruction of Dominant Cycles through Inverse FFT",
			"Standard Deviation of Log Returns over ShortPeriod",
			"Standard Deviation of Log Returns over LongPeriod",
			"Trading Days Per Year",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var line = 1;
				using var reader = new StringReader(ReadAllText(file));

				for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
				{
					foreach (var phrase in phrases)
					{
						if (!text.Contains(phrase, StringComparison.OrdinalIgnoreCase))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: indicator formula note keeps English prose fragment '{phrase}'. Localize explanatory words around invariant formula terms.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepKnownEnglishGenericIndicatorPhrases()
	{
		var errors = new List<string>();
		var phrases = new[]
		{
			"Aroon Indicator",
			"Balance Volume indicator",
			"Lunar Phase indicator",
			"Momentum Pinball Indicator",
			"Vortex Indicator",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					foreach (var phrase in phrases)
					{
						if (!ContainsStandaloneText(text, phrase))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: indicator documentation keeps English generic phrase '{phrase}'. Localize generic words such as 'Indicator' while keeping invariant names or acronyms.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishIndicatorDocsDoNotKeepEnglishGenericIndicatorWord()
	{
		var errors = new List<string>();
		var indicatorRoot = Path.Combine(_repoRoot, "es", "topics", "api", "indicators", "list_of_indicators");

		foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				if (!ContainsStandaloneText(text, "indicator"))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Spanish indicator documentation keeps English generic word 'indicator'. Use 'indicador' unless it is an invariant identifier.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishBullishBearishWords()
	{
		var errors = new List<string>();
		var phrases = new[] { "bullish", "bearish", "bulls", "bears" };

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					foreach (var phrase in phrases)
					{
						if (!ContainsStandaloneText(text, phrase))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English market direction word '{phrase}'. Localize it for the target language.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishTrendDirectionPhrases()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:upward|downward) trend\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var line = 1;
				using var reader = new StringReader(ReadAllText(file));

				for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: indicator documentation keeps English trend direction phrase '{match.Value}'. Localize prose inside formula notes too.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishFormulaLabels()
	{
		var errors = new List<string>();
		var phrases = new[]
		{
			"Adjustment Based on Historical Data",
			"Actual Price",
			"Aroon Down",
			"Aroon Oscillator",
			"Aroon Up",
			"ATR Length",
			"Average Gain",
			"Average Loss",
			"Average Price",
			"Average Value",
			"Average Negative Change",
			"Average Positive Change",
			"Base Overbought Level",
			"Base Oversold Level",
			"Bear Power",
			"Bollinger Bands",
			"Box Ratio",
			"Bull Power",
			"Candle volume",
			"Centerline",
			"CLV - Close Location Value",
			"Close Price",
			"End Value",
			"Extreme Level",
			"Extreme Values",
			"Fast MA",
			"Force Index",
			"For each i from",
			"for each Period",
			"for all i from",
			"Current High",
			"Current Low",
			"High-Low",
			"Highest High",
			"Linear Regression Line",
			"Long Momentum",
			"Midpoint",
			"Midpoint Move",
			"Level 0%",
			"Level 23.6%",
			"Level 38.2%",
			"Level 50.0%",
			"Level 61.8%",
			"Level 78.6%",
			"Level 100%",
			"Long EMA",
			"Long MA",
			"Long-term group",
			"Long-Term Changes",
			"Long-term Volatility",
			"Lower Band",
			"Lower Stop",
			"Lowest Low",
			"MACD Histogram",
			"MACD Line",
			"Mean Deviation",
			"Middle Line",
			"Money Flow",
			"Money Flow Multiplier",
			"Money Flow Volume",
			"Net Advances",
			"New Highs",
			"New Lows",
			"Normalized Long Momentum",
			"Normalized Short Momentum",
			"Open Price",
			"PPO - Signal",
			"PPO Line",
			"PVO Line",
			"Previous BV",
			"Previous Close",
			"Previous High",
			"Previous Low",
			"Price Component",
			"Predicted Price",
			"Raw BMP",
			"Raw Demand",
			"Short EMA",
			"Short MA",
			"Short Momentum",
			"Short-term group",
			"Short-term Volatility",
			"Signal Line",
			"Slow MA",
			"Smoothed MA",
			"Smoothed Demand",
			"Smoothing Factor",
			"Start Value",
			"Std Dev",
			"Sum Gains",
			"Sum Losses",
			"Sum of TR",
			"Upper Band",
			"Upper Stop",
			"Volume Component",
			"Volume Force",
			"Dynamic Overbought Level",
			"Dynamic Oversold Level",
			"1-Period EMV",
			"1-Period Force Index",
			"Trend = +1, if",
			"Trend = -1, otherwise",
			"Weighted MA",
			"Wilder MA",
			"signal period",
			"where RS",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var line = 1;
				using var reader = new StringReader(ReadAllText(file));

				for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
				{
					foreach (var phrase in phrases)
					{
						if (!ContainsStandaloneText(text, phrase))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: indicator formula or label keeps English text '{phrase}'. Localize formula labels and explanatory parameter names.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishShortLongTerms()
	{
		var errors = new List<string>();
		var pattern = new Regex(
			@"\b(?:Short|Long)[ -]Position(?:en|s)?\b|\b(?:Short|Long)-\s+und\b|\bund\s+(?:Short|Long)-\b|\b(?:Short|Long)-(?!(?:term|Term)\b)\p{L}+\b|\b(?:posición|posiciones|posição|posições)\s+(?:short|long)\b",
			RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English short/long term '{match.Value}'. Localize it in prose and formula explanations.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishChannelLineLabels()
	{
		var errors = new List<string>();
		var pattern = new Regex(
			@"\b(?:Upper|Lower|Middle)(?:\s*=|\s+(?:Line|Band|banda|Bollinger-Band|ボリンジャーバンド|布林带|Gamma)|-(?:Linie|Kanalleitung|Band|Zeile))|\b(?:Edge to Middle|Middle to Edge)\b",
			RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English channel or band label '{match.Value}'. Localize visible formula labels and strategy names.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishDirectionVolatilityLabels()
	{
		var errors = new List<string>();
		var pattern = new Regex(
			@"\b(?:Direction|Volatility)\s*=|/\s*Volatility\b|\bVolatility\b(?=[^\r\n]*(?:zero|нул|ゼロ))|\bVolatility\s+(?:Changes|Measurement|Correlation)\b",
			RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English direction or volatility label '{match.Value}'. Localize visible formula labels and section headings.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishValueForecastLabels()
	{
		var errors = new List<string>();
		var pattern = new Regex(
			@"\b(?:Value|Forecast)\s*=|\bPrevious\s+(?:ADL\s+Value|WAD\s+value)\b|\bValue\b(?=[^\r\n]*(?:>=|<=|\bFisher\b))|\bForecast\b(?=[^\r\n]*(?:\)|/|\s+-))|\bValue Range\b",
			RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English value or forecast label '{match.Value}'. Localize visible formula labels and section headings.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishRawDetrendedLabels()
	{
		var errors = new List<string>();
		var pattern = new Regex(
			@"\bIII raw\b|\bdetrendedPrice\b|\bDetrended Price\b|\bSpectral Components\b|\bshifted \(Length/2\) \+ 1 periods back\b",
			RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English raw or detrended formula label '{match.Value}'. Localize visible formula labels and explanatory text.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishIndicatorHeadingFragments()
	{
		var errors = new List<string>();
		var pattern = new Regex(
			@"Price[-\s](?:Trend|RSI|Frequenzweichen|Beziehung)|Volume[-\s](?:Spikes|Indikatoren|Multiplier)|Volume\s+(?:und|y)\s+Price|Rebounds von extremen Levels|Amplitude Changes|False Signals|KER Changes|(?:DI|EMV|GAPO|HVR|HLI|III)\s+Trends|Risk Management|Potential Reversals|Cycle Projection|Parameter Selection|Group Positioning|Optimal Entry Points|Momentum Loss|Length Parameter(?: Tuning| Selection|optimierung|auswahl)|Parameter Tuning|Gamma Parameter Tuning|Failed Swings|Componente Streak RSI|Average-Frequenzweichen|Valores High CHOP|FVE Change Rate|Wave Structure|Pattern Formation|Change Precursor|Seasonal Patterns|Setting Thresholds|Main Pivot Point|Momentum Fuerza|Momentum surges",
			RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English heading fragment '{match.Value}'. Localize visible interpretation headings.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepKnownEnglishLongIndicatorNames()
	{
		var errors = new List<string>();
		var phrases = new[]
		{
			"Accumulation/Distribution Line",
			"Adaptive Laguerre Filter",
			"Adaptive Price Zone",
			"Approval Flow Index",
			"Aroon Down",
			"Aroon Oscillator",
			"Aroon Up",
			"Average Directional Index",
			"Awesome Oscillator",
			"Balance of Market Power",
			"Balance of Power",
			"Balance Volume",
			"Bear Power",
			"Bollinger Band",
			"Bollinger Bands",
			"Bollinger Percent",
			"Bull Power",
			"Chaikin Money Flow",
			"Chaikin Volatility",
			"Chande Kroll Stop",
			"Chande Momentum Oscillator",
			"Choppiness Index",
			"Center of Gravity Oscillator",
			"Commodity Channel Index",
			"Composite Momentum",
			"Constance Brown Composite Index",
			"Connors RSI",
			"Donchian Channels",
			"Demand Index",
			"Detrended Synthetic Price",
			"Detrended Price Oscillator",
			"Disparity Index",
			"Directional Movement Index",
			"Dynamic Zones RSI",
			"Ehlers Fisher Transform",
			"Elder's Force Index",
			"Elder Ray",
			"Ease of Movement",
			"Elliot Wave Oscillator",
			"Fibonacci Retracement",
			"Finite Volume Element",
			"Force Index",
			"Forecast Oscillator",
			"Fractal Dimension Index",
			"Gator Oscillator",
			"Gopalakrishnan Range Index",
			"Harmonic Oscillator",
			"High Low",
			"High Low Index",
			"Historical Volatility Ratio",
			"Hurst Exponent",
			"Intraday Intensity Index",
			"Intraday Momentum Index",
			"Kalman Filter",
			"Kase Peak Oscillator",
			"Kaufman Efficiency Ratio",
			"Keltner Channels",
			"Klinger Volume Oscillator",
			"Know Sure Thing",
			"Laguerre RSI",
			"Linear Regression Curve",
			"Linear Regression Slope",
			"Linear Regression Forecast",
			"Linear Regression R-Squared",
			"Lunar Phase",
			"MACD Signal",
			"Market Facilitation Index",
			"Market Meanness Index",
			"Mass Index",
			"McClellan Oscillator",
			"McClellan Summation Index",
			"McGinley Dynamic",
			"Mean Deviation",
			"Median Price",
			"Money Flow Index",
			"Momentum Pinball",
			"Moving Median",
			"Negative Volume Index",
			"Nick Rypock Trailing",
			"On Balance Volume",
			"On-Balance Volume",
			"Optimal Tracking",
			"Optimal Tracking Filter",
			"Parabolic SAR",
			"Pass-Through",
			"Pivot Points",
			"Positive Volume Index",
			"Percentile Rank",
			"Percent Range",
			"Price Volume Trend",
			"Price Channels",
			"Pretty Good Oscillator",
			"Psychological Line",
			"R-Squared in Linear Regression",
			"Rainbow Charts",
			"Rank Correlation Index",
			"Range Action Verification Index",
			"relative energy index",
			"Relative Strength Index",
			"Relative Momentum Index",
			"Relative Vigor Index",
			"Schaff Trend Cycle",
			"Sine Wave",
			"Standard Error",
			"Stochastic %K",
			"Stochastic Oscillator",
			"Twiggs Money Flow",
			"Typical Price",
			"True Strength Index",
			"Ultimate Oscillator",
			"Variable Index Dynamic Average",
			"Variable MA",
			"Vertical Horizontal Filter",
			"Wave Trend Oscillator",
			"Weighted Close Price",
			"Williams %R",
			"Williams Accumulation/Distribution",
			"Williams Percent Range",
			"Williams Variable Accumulation Distribution",
			"Woodies CCI",
			"Zig Zag",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					foreach (var phrase in phrases)
					{
						if (!ContainsStandaloneText(text, phrase))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English indicator name '{phrase}'. Localize long indicator names in headings, prose, and link labels while preserving abbreviations where useful.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishPivotPointStrategyLabels()
	{
		var errors = new List<string>();
		var labels = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"Bounce Trading",
			"Breakout Trading",
			"Range Trading",
			"Target Setting",
			"Stop-Loss Placement",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var label in EnumerateMarkdownBoldTexts(ReadAllText(file)))
				{
					if (!labels.Contains(label.Text))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{label.Line}: indicator documentation keeps English Pivot Points strategy label '{label.Text}'. Localize the trading label.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishStochasticSeeAlsoLabels()
	{
		var errors = new List<string>();
		var labels = new[]
		{
			"[StochasticOscillator](stochastic_oscillator.md)",
			"[StochasticK](stochastic_oscillator_k.md)",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var content = ReadAllText(file);

				foreach (var label in labels)
				{
					if (!content.Contains(label, StringComparison.Ordinal))
						continue;

					errors.Add($"{RelativeToRepo(file)}: localized indicator documentation keeps API-style see-also link label '{label}'. Localize the visible link label while preserving the target file.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedOptionGreeksDocsDoNotKeepEnglishGreeksText()
	{
		var errors = new List<string>();
		var relativePaths = new[]
		{
			"topics/api/options.md",
			"topics/api/options/greeks.md",
			"topics/api/graphical_user_interface/charts.md",
			"topics/api/graphical_user_interface/options.md",
			"topics/api/graphical_user_interface/options/option_desk.md",
			"topics/api/graphical_user_interface/options/position_chart.md",
			"topics/designer/strategies/using_visual_designer/elements/options/greeks.md",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
				if (!File.Exists(file))
					continue;

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var visibleText = Regex.Replace(text, @"\]\([^)]+\)", "]", RegexOptions.CultureInvariant);

					if (ContainsStandaloneText(visibleText, "Black-Scholes model"))
						errors.Add($"{RelativeToRepo(file)}:{line}: option Greeks documentation keeps English link label 'Black-Scholes model'. Localize the model label.");

					if (ContainsStandaloneText(visibleText, "Greeks"))
						errors.Add($"{RelativeToRepo(file)}:{line}: option Greeks documentation keeps English term 'Greeks'. Localize it in prose.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishBreakoutTerm()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bbreakouts?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: indicator documentation keeps English trading term '{match.Value}'. Localize it in prose and labels.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void PortugueseIndicatorDocsDoNotKeepEnglishRateOfChangePhrase()
	{
		var errors = new List<string>();
		var indicatorRoot = Path.Combine(_repoRoot, "pt", "topics", "api", "indicators", "list_of_indicators");

		foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				if (!ContainsStandaloneText(text, "rate of change"))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Portuguese indicator documentation keeps English phrase 'rate of change'. Use 'taxa de variacao' or another localized wording.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishRateOfChangePhrase()
	{
		var errors = new List<string>();

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					if (!ContainsStandaloneText(text, "Rate of Change"))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English phrase 'Rate of Change'. Localize it and keep only the RoC abbreviation where useful.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepKnownEnglishSentenceFragments()
	{
		var errors = new List<string>();
		var phrases = new[]
		{
			"Lines componen",
			"Abrupt changes in stop lines",
			"Correction Identification",
			"High MOMA",
			"Reverse Divergence",
			"Trend-line Break",
			"valores negativos de High",
			"valores positivos de High",
			"reversal hump",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					foreach (var phrase in phrases)
					{
						if (!ContainsStandaloneText(text, phrase))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English sentence fragment '{phrase}'. Localize the sentence text.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedStandardDeviationDocsDoNotKeepEnglishTitleText()
	{
		var errors = new List<string>();
		var relativePaths = new[]
		{
			"topics/api/indicators/list_of_indicators/standard_deviation.md",
			"topics/api/indicators/list_of_indicators/smoothed_ma.md",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(langRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));
				if (!File.Exists(file))
					continue;

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					if (!ContainsStandaloneText(text, "Standard Deviation"))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized Standard Deviation docs keep English title text. Localize the page title, lead text, and link label.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishTrueRangeTerm()
	{
		var errors = new List<string>();

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var content = ReadAllText(file);

				if (content.Contains("[TrueRange](true_range.md)", StringComparison.Ordinal))
					errors.Add($"{RelativeToRepo(file)}: localized indicator documentation keeps API-style link label '[TrueRange](true_range.md)'. Localize the link label.");

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(content))
				{
					if (!ContainsStandaloneText(text, "True Range"))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English term 'True Range'. Localize the term while keeping TR/ATR abbreviations where useful.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishMovingAverageTerms()
	{
		var errors = new List<string>();
		var phrases = new[]
		{
			"Exponential Moving Average",
			"Simple Moving Average",
			"Weighted Moving Average",
			"Smoothed Moving Average",
		};
		var apiStyleLocalLinks = new[]
		{
			"[ExponentialMovingAverage](ema.md)",
			"[SimpleMovingAverage](sma.md)",
			"[WeightedMovingAverage](weighted_ma.md)",
			"[SmoothedMovingAverage](smoothed_ma.md)",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var content = ReadAllText(file);

				foreach (var link in apiStyleLocalLinks)
				{
					if (content.Contains(link, StringComparison.Ordinal))
						errors.Add($"{RelativeToRepo(file)}: localized indicator documentation keeps API-style link label '{link}'. Localize the link label.");
				}

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(content))
				{
					foreach (var phrase in phrases)
					{
						if (!ContainsStandaloneText(text, phrase))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English moving-average term '{phrase}'. Localize the term while keeping MA/EMA/SMA abbreviations where useful.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownDoesNotKeepEnglishMovingAveragesPlural()
	{
		var errors = new List<string>();

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					if (!ContainsStandaloneText(text, "Moving Averages"))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized markdown keeps English plural term 'Moving Averages'. Localize the generic term or remove the English expansion.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishMacdExpansion()
	{
		var errors = new List<string>();

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					if (!ContainsStandaloneText(text, "Moving Average Convergence"))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English MACD expansion. Localize the expansion or use MACD without English prose.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishPercentageOscillatorNames()
	{
		var errors = new List<string>();
		var phrases = new[]
		{
			"Percentage Price Oscillator",
			"Percentage Volume Oscillator",
		};
		var apiStyleLocalLinks = new[]
		{
			"[Percentage Volume Oscillator](percentage_volume_oscillator.md)",
			"[Percentage Price Oscillator](percentage_price_oscillator.md)",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var content = ReadAllText(file);

				foreach (var link in apiStyleLocalLinks)
				{
					if (content.Contains(link, StringComparison.Ordinal))
						errors.Add($"{RelativeToRepo(file)}: localized indicator documentation keeps English percentage oscillator link label '{link}'. Localize the link label.");
				}

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(content))
				{
					foreach (var phrase in phrases)
					{
						if (!ContainsStandaloneText(text, phrase))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English percentage oscillator name '{phrase}'. Localize the name while keeping PPO/PVO abbreviations where useful.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedBollingerBandsDocsDoNotKeepEnglishFormulaLabels()
	{
		var errors = new List<string>();
		var phrases = new[]
		{
			"Moving Average",
			"Middle Line",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators", "bollinger_bands.md");
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				foreach (var phrase in phrases)
				{
					if (!ContainsStandaloneText(text, phrase))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized Bollinger Bands documentation keeps English formula label '{phrase}'. Localize the parameter and formula text.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDocsDoNotKeepEnglishBollingerBandLabels()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bBollinger Bands?\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var topicsRoot = Path.Combine(_repoRoot, lang, "topics");
			if (!Directory.Exists(topicsRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(topicsRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps English Bollinger band label '{match.Value}'. Localize visible indicator names in prose.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDocsDoNotKeepEnglishVolumeProfileLabels()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:Volume Profile|Intraday Volume)\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var topicsRoot = Path.Combine(_repoRoot, lang, "topics");
			if (!Directory.Exists(topicsRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(topicsRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps English volume-analysis label '{match.Value}'. Localize visible indicator and script names in prose.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishPossessiveIndicatorLabels()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:Welles Wilder's ADX|Elder Index)\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English indicator label '{match.Value}'. Localize visible indicator names in prose.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishSignalAndReferenceLabels()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:Chaikin's Volatility|Zero-line Reject|Granville's New Key to Stock Market Profits|Elder-ray|Reverse divergencia)\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English signal or reference label '{match.Value}'. Localize visible indicator prose.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepMachineTranslatedObvFragments()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:Flat OBV|Análisis técnico Patterns|XQX\d+|Volume en equilibrio|On-Balance-Volumen Mean|オンバランスボリューム Mean)\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps machine-translated OBV fragment '{match.Value}'. Localize or rewrite the visible prose.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedGenericIndicatorPagesDoNotKeepEnglishTitles()
	{
		var errors = new List<string>();
		var relativePaths = new[]
		{
			"topics/api/indicators/list_of_indicators/volume.md",
			"topics/api/indicators/list_of_indicators/peak.md",
			"topics/api/indicators/list_of_indicators/trough.md",
			"topics/api/indicators/list_of_indicators/shift.md",
			"topics/api/indicators/list_of_indicators/sum_n.md",
			"topics/api/indicators/list_of_indicators/highest.md",
			"topics/api/indicators/list_of_indicators/lowest.md",
			"topics/api/indicators/list_of_indicators/envelope.md",
			"topics/api/indicators/list_of_indicators/fractals.md",
			"topics/api/indicators/list_of_indicators/momentum.md",
		};

		var headingPattern = new Regex(@"^#\s*(?:Volume|Peak|Trough|Shift|Sum N|Highest|Lowest|Envelope|Fractals|Momentum)\s*$", RegexOptions.CultureInvariant);
		var boldPattern = new Regex(@"\*\*(?:Peak|Trough|Shift|Sum N|Envelope|Fractals|Momentum)\*\*|（Shift）|Фрактал \(Fractals\)|Моментум \(Momentum\)", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
				if (!File.Exists(file))
					continue;

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					if (!headingPattern.IsMatch(text) && !boldPattern.IsMatch(text))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized generic indicator page keeps English title text '{text}'. Localize visible page titles and bold indicator names.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishMovingAverageLocalLinkLabels()
	{
		var errors = new List<string>();

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var label in EnumerateMarkdownLinkLabels(ReadAllText(file)))
				{
					if (!label.Url.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
						continue;

					if (!ContainsStandaloneText(label.Text, "Moving Average"))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{label.Line}: localized indicator documentation keeps English moving-average local link label '{label.Text}'. Localize the link label.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCoreMovingAverageIndicatorDocsDoNotKeepEnglishNames()
	{
		var errors = new List<string>();
		var relativePaths = new[]
		{
			"topics/api/indicators/list_of_indicators/t3_moving_average.md",
			"topics/api/indicators/list_of_indicators/moving_average_crossover.md",
			"topics/api/indicators/list_of_indicators/moving_average_ribbon.md",
			"topics/api/indicators/list_of_indicators/momentum_of_moving_average.md",
			"topics/api/indicators/list_of_indicators/oscillator_of_moving_average.md",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(langRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));
				if (!File.Exists(file))
					continue;

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					if (!ContainsStandaloneText(text, "Moving Average"))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized moving-average indicator page keeps English name text. Localize the repeated indicator name in headings, prose, and formula descriptions.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedNamedMovingAverageIndicatorDocsDoNotKeepEnglishNames()
	{
		var errors = new List<string>();
		var relativePaths = new[]
		{
			"topics/api/indicators/list_of_indicators/hma.md",
			"topics/api/indicators/list_of_indicators/jma.md",
			"topics/api/indicators/list_of_indicators/variable_moving_average.md",
			"topics/api/indicators/list_of_indicators/guppy_multiple_moving_average.md",
			"topics/api/indicators/list_of_indicators/arnaud_legoux_moving_average.md",
			"topics/api/indicators/list_of_indicators/fractal_adaptive_moving_average.md",
			"topics/api/indicators/list_of_indicators/endpoint_moving_average.md",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(langRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));
				if (!File.Exists(file))
					continue;

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					if (!ContainsStandaloneText(text, "Moving Average"))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized named moving-average indicator page keeps English name text. Localize the repeated indicator name in headings and prose.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorDocsDoNotKeepEnglishMovingAverageText()
	{
		var errors = new List<string>();

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					if (!ContainsStandaloneText(text, "Moving Average"))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English moving-average text. Localize the visible term while preserving code and API identifiers.");
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
	public void TextFilesDoNotContainTranslationArtifactMarkers()
	{
		var errors = new List<string>();

		foreach (var file in EnumerateContentTextFiles())
			ValidateNoTranslationArtifactMarkers(file, errors);

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

	[TestMethod]
	public void LocalizedCodeCommentsDoNotLookLikeEnglish()
	{
		var errors = new List<string>();

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
				{
					var normalized = NormalizeCodeCommentForLikelyEnglishCheck(comment.Text);
					if (!IsLikelyUntranslatedEnglishCodeComment(normalized))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: code comment looks like untranslated English. Localize it or add a deliberate allowlist entry. Comment: {Truncate(comment.Text, 180)}");
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

	private static TocEntry[] ReadTocEntries(string tocPath, List<string> errors)
	{
		try
		{
			return _yaml.Deserialize<TocEntry[]>(ReadAllText(tocPath)) ?? Array.Empty<TocEntry>();
		}
		catch (Exception ex)
		{
			errors.Add($"{RelativeToRepo(tocPath)} is invalid YAML: {ex.Message}");
			return null;
		}
	}

	private static void ValidateTocStructureMatchesDefault(
		string relative,
		string localizedToc,
		IReadOnlyList<TocEntry> expected,
		IReadOnlyList<TocEntry> actual,
		List<string> errors,
		string path)
	{
		if (expected.Count != actual.Count)
		{
			errors.Add($"{RelativeToRepo(localizedToc)}{FormatTocPath(path)}: TOC item count must match {DefaultLanguage}/{relative}. Expected {expected.Count}, actual {actual.Count}.");
			return;
		}

		for (var i = 0; i < expected.Count; i++)
		{
			var expectedEntry = expected[i];
			var actualEntry = actual[i];
			var itemPath = string.IsNullOrEmpty(path)
				? $"[{i}]"
				: $"{path}/[{i}]";

			var expectedHref = NormalizeStructureUrl(expectedEntry.Href);
			var actualHref = NormalizeStructureUrl(actualEntry.Href);

			if (!actualHref.Equals(expectedHref, StringComparison.Ordinal))
			{
				errors.Add($"{RelativeToRepo(localizedToc)}{FormatTocPath(itemPath)}: TOC href must match {DefaultLanguage}/{relative}. Expected '{expectedHref}', actual '{actualHref}'.");
				continue;
			}

			ValidateTocStructureMatchesDefault(relative, localizedToc, expectedEntry.Items, actualEntry.Items, errors, itemPath);
		}
	}

	private static IEnumerable<TocEntryText> FlattenTocEntries(IEnumerable<TocEntry> entries)
	{
		foreach (var entry in entries)
		{
			yield return new TocEntryText(entry.Name?.Trim() ?? string.Empty, entry.Href?.Trim() ?? string.Empty);

			if (entry.Items is null)
				continue;

			foreach (var child in FlattenTocEntries(entry.Items))
				yield return child;
		}
	}

	private static IEnumerable<TocNameLine> EnumerateTocNameLines(string tocPath)
	{
		var line = 1;
		using var reader = new StringReader(ReadAllText(tocPath));

		for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
		{
			var match = Regex.Match(text, @"^\s*-\s*name:\s*(?<value>.*?)\s*$", RegexOptions.CultureInvariant);
			if (!match.Success)
				continue;

			var value = match.Groups["value"].Value.Trim();
			if (value.Length >= 2
				&& ((value[0] == '\'' && value[^1] == '\'')
					|| (value[0] == '"' && value[^1] == '"')))
			{
				value = value[1..^1].Trim();
			}

			yield return new TocNameLine(value, line);
		}
	}

	private static bool IsLikelyUntranslatedEnglishCjkTocName(string name, string href)
	{
		if (string.IsNullOrWhiteSpace(name) || _allowedInvariantCjkTocNames.Contains(name))
			return false;

		var normalizedHref = NormalizeStructureUrl(href);
		if (normalizedHref.StartsWith("api/connectors/", StringComparison.OrdinalIgnoreCase)
			|| normalizedHref.StartsWith("api/indicators/list_of_indicators/", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}

		return _translatableEnglishCjkTocNames.Contains(name);
	}

	private static string FormatTocPath(string path)
		=> string.IsNullOrEmpty(path) ? string.Empty : $" {path}";

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
			document.Descendants().OfType<LinkInline>().Where(link => !link.IsImage).Select(link => NormalizeStructureUrl(link.Url)).Order(StringComparer.Ordinal).ToArray(),
			document.Descendants().OfType<LinkInline>().Where(link => link.IsImage).Select(link => NormalizeStructureUrl(link.Url)).ToArray(),
			document.Descendants().OfType<Table>().Select(GetTableShape).ToArray(),
			GetListSummary(document),
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
		ValidateStructureSequence(relative, localizedFile, "link URLs", expected.LinkUrls, actual.LinkUrls, errors);
		ValidateStructureSequence(relative, localizedFile, "image URLs", expected.ImageUrls, actual.ImageUrls, errors);
		ValidateStructureSequence(relative, localizedFile, "table shapes", expected.TableShapes, actual.TableShapes, errors);
		ValidateStructureValue(relative, localizedFile, "list item counts", expected.ListSummary, actual.ListSummary, errors);
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

	private static void ValidateStructureValue<T>(
		string relative,
		string localizedFile,
		string name,
		T expected,
		T actual,
		List<string> errors)
	{
		if (EqualityComparer<T>.Default.Equals(expected, actual))
			return;

		errors.Add($"{RelativeToRepo(localizedFile)}: {name} must match {DefaultLanguage}/{relative}. Expected: {expected}. Actual: {actual}.");
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

	private static ListSummary GetListSummary(MarkdownDocument document)
	{
		var ordered = 0;
		var unordered = 0;

		foreach (var list in document.Descendants().OfType<ListBlock>())
		{
			var items = list.OfType<ListItemBlock>().Count();
			if (list.IsOrdered)
				ordered += items;
			else
				unordered += items;
		}

		return new ListSummary(ordered, unordered);
	}


	private static IEnumerable<MarkdownTableCellText> EnumerateMarkdownTableCellTexts(string markdown)
	{
		var document = Markdown.Parse(markdown, _markdown);
		var lineStarts = GetLineStarts(markdown);

		foreach (var cell in document.Descendants().OfType<TableCell>())
		{
			var text = string.Concat(cell.Descendants().OfType<LiteralInline>().Select(literal => literal.Content.ToString()));
			text = NormalizeHumanText(text);

			if (text.Length == 0)
				continue;

			yield return new MarkdownTableCellText(text, GetLineNumber(lineStarts, cell.Span.Start));
		}
	}

	private static IEnumerable<MarkdownPlainTextBlock> EnumerateMarkdownPlainTextBlocks(string markdown)
	{
		var lineStarts = GetLineStarts(markdown);

		foreach (Match match in Regex.Matches(markdown, @"(?m)^(?<fence>`{3,}|~{3,})(?<info>[^\r\n]*)\r?\n(?<content>.*?)(?m)^\k<fence>\s*$", RegexOptions.Singleline | RegexOptions.CultureInvariant))
		{
			var info = match.Groups["info"].Value.Trim();
			if (!IsPlainTextMarkdownFenceInfo(info))
				continue;

			var text = NormalizeMarkdownPlainTextBlockForTranslationCheck(match.Groups["content"].Value);
			if (text.Length == 0)
				continue;

			yield return new MarkdownPlainTextBlock(text, GetLineNumber(lineStarts, match.Groups["content"].Index));
		}
	}

	private static IEnumerable<MarkdownTextLine> EnumerateMarkdownTextLikeCodeBlockLines(string markdown)
	{
		var lineStarts = GetLineStarts(markdown);

		foreach (Match match in Regex.Matches(markdown, @"(?m)^(?<fence>`{3,}|~{3,})(?<info>[^\r\n]*)\r?\n(?<content>.*?)(?m)^\k<fence>\s*$", RegexOptions.Singleline | RegexOptions.CultureInvariant))
		{
			var info = match.Groups["info"].Value.Trim();
			if (!IsTextLikeMarkdownFenceInfo(info))
				continue;

			var line = GetLineNumber(lineStarts, match.Groups["content"].Index);
			using var reader = new StringReader(match.Groups["content"].Value);

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				if (!string.IsNullOrWhiteSpace(text))
					yield return new MarkdownTextLine(text, line);
			}
		}
	}

	private static bool IsPlainTextMarkdownFenceInfo(string info)
		=> string.IsNullOrWhiteSpace(info)
			|| info.Equals("text", StringComparison.OrdinalIgnoreCase)
			|| info.Equals("txt", StringComparison.OrdinalIgnoreCase)
			|| info.Equals("plain", StringComparison.OrdinalIgnoreCase);

	private static bool IsTextLikeMarkdownFenceInfo(string info)
		=> IsPlainTextMarkdownFenceInfo(info)
			|| info.Equals("markdown", StringComparison.OrdinalIgnoreCase)
			|| info.Equals("md", StringComparison.OrdinalIgnoreCase);

	private static string NormalizeMarkdownPlainTextBlockForTranslationCheck(string text)
	{
		var value = Regex.Replace(text ?? string.Empty, @"\r\n?", "\n", RegexOptions.CultureInvariant).Trim();
		value = Regex.Replace(value, @"[ \t]+", " ", RegexOptions.CultureInvariant);
		value = Regex.Replace(value, @"\n{3,}", "\n\n", RegexOptions.CultureInvariant);
		return value.Trim();
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

	private static IEnumerable<HeadingText> EnumerateHeadingTexts(string markdown)
	{
		var document = Markdown.Parse(markdown, _markdown);
		var lineStarts = GetLineStarts(markdown);

		foreach (var heading in EnumerateHeadings(document))
		{
			if (heading.Inline is null)
				continue;

			var text = GetInlineText(heading.Inline);
			if (text.Length == 0)
				continue;

			yield return new HeadingText(text, GetLineNumber(lineStarts, heading.Span.Start));
		}
	}

	private static string GetInlineText(ContainerInline inline)
	{
		var text = string.Concat(inline.Descendants<LiteralInline>().Select(literal => literal.Content.ToString()));

		return NormalizeHumanText(text);
	}

	private static string NormalizeHumanText(string text)
		=> Regex.Replace(text, @"\s+", " ", RegexOptions.CultureInvariant).Trim();

	private static bool ContainsStandaloneText(string text, string value)
		=> Regex.IsMatch(text, $@"(?<![A-Za-z]){Regex.Escape(value)}(?![A-Za-z])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

	private static bool IsAllowedInvariantHeading(string relativePath, string heading)
	{
		var normalizedPath = relativePath.Replace('\\', '/');

		if (IsCodeLikeHeading(heading))
			return true;

		if (normalizedPath.StartsWith("topics/api/connectors/", StringComparison.OrdinalIgnoreCase))
			return true;

		if (normalizedPath.StartsWith("topics/api/indicators/list_of_indicators/", StringComparison.OrdinalIgnoreCase))
		{
			if (IsShortInvariantName(heading) || IsSingleTokenIndicatorName(heading))
				return true;

			if (_allowedInvariantIndicatorHeadingPaths.Contains(normalizedPath))
				return true;
		}

		if (normalizedPath.StartsWith("topics/api/patterns/", StringComparison.OrdinalIgnoreCase)
			&& IsCandlePatternHeading(heading))
		{
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

	private static bool IsCodeLikeHeading(string text)
	{
		if (Regex.IsMatch(text, @"^[A-Z][A-Za-z0-9]*(?:\.[A-Z][A-Za-z0-9]*)*(?:\(\))?$", RegexOptions.CultureInvariant))
			return true;

		if (Regex.IsMatch(text, @"^I[A-Z][A-Za-z0-9]+$", RegexOptions.CultureInvariant))
			return true;

		return Regex.IsMatch(text, @"^[A-Z][A-Za-z0-9]+\s+(?:enum|class|interface)$", RegexOptions.CultureInvariant);
	}

	private static bool IsCandlePatternHeading(string text)
	{
		if (text.Length is < 4 or > 64)
			return false;

		if (!Regex.IsMatch(text, @"^[A-Z0-9][A-Za-z0-9' -]+$", RegexOptions.CultureInvariant))
			return false;

		var words = Regex.Matches(text, @"[A-Za-z]+", RegexOptions.CultureInvariant)
			.Select(match => match.Value)
			.ToArray();

		return words.Length > 0 && words.All(word => char.IsUpper(word[0]));
	}

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

	private static void ValidateNoTranslationArtifactMarkers(string file, List<string> errors)
	{
		var line = 1;
		using var reader = new StringReader(ReadAllText(file));

		for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
		{
			var marker = _translationArtifactMarkers.FirstOrDefault(text.Contains);
			if (marker is null)
				continue;

			errors.Add($"{RelativeToRepo(file)}:{line}: contains translation artifact marker '{marker}'. Line: {Truncate(text.Trim(), 180)}");
		}
	}

	private static LocalizedDocumentationAudit BuildLocalizedDocumentationAudit()
	{
		var languages = GetLocalizedContentQualityLanguages();
		var issues = new List<AuditIssue>();
		var stats = new List<LocalizedAuditLanguageStats>();
		var defaultRoot = Path.Combine(_repoRoot, DefaultLanguage);
		var defaultMarkdownTextByRelativePath = BuildDefaultMarkdownTextMap(defaultRoot);
		var defaultTableCellsByRelativePath = BuildDefaultMarkdownTableCellMap(defaultRoot);
		var defaultPlainTextBlocksByRelativePath = BuildDefaultMarkdownPlainTextBlockMap(defaultRoot);
		var defaultLinkLabelsByRelativePath = BuildDefaultMarkdownLinkLabelMap(defaultRoot);
		var defaultImageAltTextsByRelativePath = BuildDefaultMarkdownImageAltTextMap(defaultRoot);
		var defaultOutputsByRelativePath = BuildDefaultCodeOutputMap(defaultRoot);
		var defaultUiStringsByRelativePath = BuildDefaultCodeUiStringMap(defaultRoot);
		var defaultCodeLiteralsByRelativePath = BuildDefaultCodeStringLiteralMap(defaultRoot);
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
			var uiStringCount = 0;
			var codeLiteralCount = 0;
			var commentCount = 0;
			var markdownTextCount = 0;
			var tableCellCount = 0;
			var plainTextBlockCount = 0;
			var linkLabelCount = 0;
			var imageAltTextCount = 0;

			foreach (var file in textFiles)
				CollectTextEncodingAuditIssues(file, issues);

			foreach (var file in markdownFiles)
			{
				var relative = Path.GetRelativePath(langRoot, file).Replace('\\', '/');
				var markdown = ReadAllText(file);
				defaultMarkdownTextByRelativePath.TryGetValue(relative, out var defaultMarkdownTexts);
				defaultTableCellsByRelativePath.TryGetValue(relative, out var defaultTableCells);
				defaultPlainTextBlocksByRelativePath.TryGetValue(relative, out var defaultPlainTextBlocks);
				defaultLinkLabelsByRelativePath.TryGetValue(relative, out var defaultLinkLabels);
				defaultImageAltTextsByRelativePath.TryGetValue(relative, out var defaultImageAltTexts);
				defaultOutputsByRelativePath.TryGetValue(relative, out var defaultOutputs);
				defaultUiStringsByRelativePath.TryGetValue(relative, out var defaultUiStrings);
				defaultCodeLiteralsByRelativePath.TryGetValue(relative, out var defaultCodeLiterals);
				defaultCommentsByRelativePath.TryGetValue(relative, out var defaultComments);

				foreach (var textLine in EnumerateTranslatableMarkdownTextLines(markdown, relative))
				{
					markdownTextCount++;
					if (defaultMarkdownTexts is null || !defaultMarkdownTexts.Contains(textLine.Text))
						continue;

					issues.Add(new AuditIssue("markdown-text-exact-english", RelativeToRepo(file), textLine.Line, $"is identical to the English source text: {Truncate(textLine.Text, 180)}"));
				}

				foreach (var sectionLabel in EnumerateKnownEnglishSectionLabels(markdown, relative))
					issues.Add(new AuditIssue("markdown-section-label-english", RelativeToRepo(file), sectionLabel.Line, $"section label is still English: {sectionLabel.Text}"));

				foreach (var textLine in EnumerateLikelyEnglishMarkdownTextLines(markdown, relative))
					issues.Add(new AuditIssue("markdown-text-likely-english", RelativeToRepo(file), textLine.Line, $"looks like untranslated English: {Truncate(textLine.Text, 180)}"));

				foreach (var tableCell in EnumerateMarkdownTableCellTexts(markdown))
				{
					tableCellCount++;
					var normalized = NormalizeMarkdownTextForTranslationCheck(tableCell.Text);
					if (normalized.Length == 0 || defaultTableCells is null || !defaultTableCells.Contains(normalized))
						continue;

					issues.Add(new AuditIssue("markdown-table-cell-exact-english", RelativeToRepo(file), tableCell.Line, $"is identical to the English source table cell: {Truncate(tableCell.Text, 180)}"));
				}

				foreach (var plainTextBlock in EnumerateMarkdownPlainTextBlocks(markdown))
				{
					plainTextBlockCount++;
					var normalized = NormalizeMarkdownPlainTextBlockForTranslationCheck(plainTextBlock.Text);
					if (normalized.Length == 0 || defaultPlainTextBlocks is null || !defaultPlainTextBlocks.Contains(normalized))
						continue;

					issues.Add(new AuditIssue("markdown-plain-text-block-exact-english", RelativeToRepo(file), plainTextBlock.Line, $"is identical to the English source plain text code block: {Truncate(plainTextBlock.Text, 180)}"));
				}

				foreach (var linkLabel in EnumerateMarkdownLinkLabels(markdown))
				{
					linkLabelCount++;
					var normalized = NormalizeMarkdownLinkLabelForTranslationCheck(linkLabel.Text);
					if (normalized.Length == 0 || defaultLinkLabels is null || !defaultLinkLabels.Contains(normalized))
						continue;

					issues.Add(new AuditIssue("markdown-link-label-exact-english", RelativeToRepo(file), linkLabel.Line, $"is identical to the English source link label: {Truncate(linkLabel.Text, 180)}"));
				}

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
				{
					imageAltTextCount++;
					var normalized = NormalizeMarkdownImageAltTextForTranslationCheck(altText.Text);
					if (normalized.Length == 0 || defaultImageAltTexts is null || !defaultImageAltTexts.Contains(normalized))
						continue;

					issues.Add(new AuditIssue("markdown-image-alt-exact-english", RelativeToRepo(file), altText.Line, $"is identical to the English source image alt text: {Truncate(altText.Text, 180)}"));
				}

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

				foreach (var uiString in EnumerateCodeUiStrings(markdown))
				{
					uiStringCount++;
					var normalized = NormalizeCodeUiStringForTranslationCheck(uiString.Text);
					if (normalized.Length == 0 || defaultUiStrings is null || !defaultUiStrings.Contains(normalized))
						continue;

					issues.Add(new AuditIssue("code-ui-exact-english", RelativeToRepo(file), uiString.Line, $"is identical to the English source UI string: {Truncate(uiString.Text, 180)}"));
				}

				foreach (var literal in EnumerateCodeStringLiterals(markdown))
				{
					codeLiteralCount++;
					var normalized = NormalizeCodeStringLiteralForTranslationCheck(literal.Text);
					var exactEnglish = normalized.Length > 0 && defaultCodeLiterals is not null && defaultCodeLiterals.Contains(normalized);

					if (exactEnglish)
					{
						issues.Add(new AuditIssue("code-literal-exact-english", RelativeToRepo(file), literal.Line, $"is identical to the English source string literal: {Truncate(literal.Text, 180)}"));
						continue;
					}

					if (IsLikelyUntranslatedEnglishCodeStringLiteral(normalized))
						issues.Add(new AuditIssue("code-literal-likely-english", RelativeToRepo(file), literal.Line, $"looks like untranslated English: {Truncate(literal.Text, 180)}"));
				}

				foreach (var comment in EnumerateCodeComments(markdown))
				{
					commentCount++;
					var normalized = NormalizeCodeCommentForTranslationCheck(comment.Text);
					var exactEnglish = normalized.Length > 0 && defaultComments is not null && defaultComments.Contains(normalized);

					if (exactEnglish)
					{
						issues.Add(new AuditIssue("code-comment-exact-english", RelativeToRepo(file), comment.Line, $"is identical to the English source comment: {Truncate(comment.Text, 180)}"));
						continue;
					}

					var likelyEnglish = NormalizeCodeCommentForLikelyEnglishCheck(comment.Text);
					if (IsLikelyUntranslatedEnglishCodeComment(likelyEnglish))
						issues.Add(new AuditIssue("code-comment-likely-english", RelativeToRepo(file), comment.Line, $"looks like untranslated English: {Truncate(comment.Text, 180)}"));
				}
			}

			stats.Add(new LocalizedAuditLanguageStats(lang, markdownFiles.Length, textFiles.Length, markdownTextCount, tableCellCount, plainTextBlockCount, linkLabelCount, imageAltTextCount, outputCount, uiStringCount, codeLiteralCount, commentCount));
		}

		return new LocalizedDocumentationAudit(languages, stats, issues);
	}

	private static Dictionary<string, HashSet<string>> BuildDefaultMarkdownTextMap(string defaultRoot)
		=> Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories)
			.Order(StringComparer.OrdinalIgnoreCase)
			.Select(file =>
			{
				var relative = Path.GetRelativePath(defaultRoot, file).Replace('\\', '/');
				return new
				{
					RelativePath = relative,
					Texts = EnumerateTranslatableMarkdownTextLines(ReadAllText(file), relative)
						.Select(line => line.Text)
						.ToHashSet(StringComparer.Ordinal),
				};
			})
			.Where(entry => entry.Texts.Count > 0)
			.ToDictionary(entry => entry.RelativePath, entry => entry.Texts, StringComparer.OrdinalIgnoreCase);

	private static Dictionary<string, HashSet<string>> BuildDefaultMarkdownTableCellMap(string defaultRoot)
		=> Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories)
			.Order(StringComparer.OrdinalIgnoreCase)
			.Select(file =>
			{
				var relative = Path.GetRelativePath(defaultRoot, file).Replace('\\', '/');
				return new
				{
					RelativePath = relative,
					Texts = EnumerateMarkdownTableCellTexts(ReadAllText(file))
						.Select(cell => NormalizeMarkdownTextForTranslationCheck(cell.Text))
						.Where(text => IsTranslatableEnglishMarkdownText(relative, text, text))
						.ToHashSet(StringComparer.Ordinal),
				};
			})
			.Where(entry => entry.Texts.Count > 0)
			.ToDictionary(entry => entry.RelativePath, entry => entry.Texts, StringComparer.OrdinalIgnoreCase);

	private static Dictionary<string, HashSet<string>> BuildDefaultMarkdownPlainTextBlockMap(string defaultRoot)
		=> Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories)
			.Order(StringComparer.OrdinalIgnoreCase)
			.Select(file =>
			{
				var relative = Path.GetRelativePath(defaultRoot, file).Replace('\\', '/');
				return new
				{
					RelativePath = relative,
					Blocks = EnumerateMarkdownPlainTextBlocks(ReadAllText(file))
						.Select(block => NormalizeMarkdownPlainTextBlockForTranslationCheck(block.Text))
						.Where(text => IsTranslatableEnglishMarkdownPlainTextBlock(relative, text))
						.ToHashSet(StringComparer.Ordinal),
				};
			})
			.Where(entry => entry.Blocks.Count > 0)
			.ToDictionary(entry => entry.RelativePath, entry => entry.Blocks, StringComparer.OrdinalIgnoreCase);

	private static Dictionary<string, HashSet<string>> BuildDefaultMarkdownLinkLabelMap(string defaultRoot)
		=> Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories)
			.Order(StringComparer.OrdinalIgnoreCase)
			.Select(file => new
			{
				RelativePath = Path.GetRelativePath(defaultRoot, file).Replace('\\', '/'),
				Labels = EnumerateMarkdownLinkLabels(ReadAllText(file))
					.Where(label => IsTranslatableEnglishMarkdownLinkLabel(label.Text, label.Url))
					.Select(label => NormalizeMarkdownLinkLabelForTranslationCheck(label.Text))
					.ToHashSet(StringComparer.Ordinal),
			})
			.Where(entry => entry.Labels.Count > 0)
			.ToDictionary(entry => entry.RelativePath, entry => entry.Labels, StringComparer.OrdinalIgnoreCase);

	private static Dictionary<string, HashSet<string>> BuildDefaultMarkdownImageAltTextMap(string defaultRoot)
		=> Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories)
			.Order(StringComparer.OrdinalIgnoreCase)
			.Select(file => new
			{
				RelativePath = Path.GetRelativePath(defaultRoot, file).Replace('\\', '/'),
				AltTexts = EnumerateMarkdownImageAltTexts(ReadAllText(file))
					.Select(altText => NormalizeMarkdownImageAltTextForTranslationCheck(altText.Text))
					.Where(IsTranslatableEnglishMarkdownImageAltText)
					.ToHashSet(StringComparer.Ordinal),
			})
			.Where(entry => entry.AltTexts.Count > 0)
			.ToDictionary(entry => entry.RelativePath, entry => entry.AltTexts, StringComparer.OrdinalIgnoreCase);

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

	private static Dictionary<string, HashSet<string>> BuildDefaultCodeUiStringMap(string defaultRoot)
		=> Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories)
			.Order(StringComparer.OrdinalIgnoreCase)
			.Select(file => new
			{
				RelativePath = Path.GetRelativePath(defaultRoot, file).Replace('\\', '/'),
				UiStrings = EnumerateCodeUiStrings(ReadAllText(file))
					.Select(uiString => NormalizeCodeUiStringForTranslationCheck(uiString.Text))
					.Where(IsTranslatableEnglishCodeUiString)
					.ToHashSet(StringComparer.Ordinal),
			})
			.Where(entry => entry.UiStrings.Count > 0)
			.ToDictionary(entry => entry.RelativePath, entry => entry.UiStrings, StringComparer.OrdinalIgnoreCase);

	private static Dictionary<string, HashSet<string>> BuildDefaultCodeStringLiteralMap(string defaultRoot)
		=> Directory.EnumerateFiles(defaultRoot, "*.md", SearchOption.AllDirectories)
			.Order(StringComparer.OrdinalIgnoreCase)
			.Select(file => new
			{
				RelativePath = Path.GetRelativePath(defaultRoot, file).Replace('\\', '/'),
				Literals = EnumerateCodeStringLiterals(ReadAllText(file))
					.Select(literal => NormalizeCodeStringLiteralForTranslationCheck(literal.Text))
					.Where(IsTranslatableEnglishCodeStringLiteral)
					.ToHashSet(StringComparer.Ordinal),
			})
			.Where(entry => entry.Literals.Count > 0)
			.ToDictionary(entry => entry.RelativePath, entry => entry.Literals, StringComparer.OrdinalIgnoreCase);

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
		report.AppendLine("| Language | Markdown files | Text files | Markdown text candidates | Markdown table cells | Markdown plain text blocks | Markdown link labels | Markdown image alt texts | Code output strings | Code UI strings | Code string literals | Code comments |");
		report.AppendLine("| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |");

		foreach (var stat in audit.Stats)
			report.AppendLine($"| {stat.Language} | {stat.MarkdownFiles} | {stat.TextFiles} | {stat.MarkdownTextCandidates} | {stat.MarkdownTableCells} | {stat.MarkdownPlainTextBlocks} | {stat.MarkdownLinkLabels} | {stat.MarkdownImageAltTexts} | {stat.CodeOutputStrings} | {stat.CodeUiStrings} | {stat.CodeStringLiterals} | {stat.CodeComments} |");

		report.AppendLine();
		report.AppendLine("## Checks");
		report.AppendLine();
		report.AppendLine("- Markdown text: full normalized English-like visible text candidates must not be identical to the English source.");
		report.AppendLine("- Markdown text: short known section labels must not remain in English.");
		report.AppendLine("- Markdown text: visible localized text must not have a high ratio of English stop words.");
		report.AppendLine("- Markdown table cells: exact matches with English source table cells for translatable text candidates.");
		report.AppendLine("- Markdown plain text code blocks: exact matches with English source plain text blocks for translatable prompt/text examples.");
		report.AppendLine("- Markdown link labels: exact matches with English source link labels for translatable label candidates.");
		report.AppendLine("- Markdown image alt text: exact matches with English source alt text for translatable alt candidates.");
		report.AppendLine("- Encoding: repeated question marks, suspicious question marks inside Latin words, Unicode replacement characters, mojibake markers.");
		report.AppendLine("- Code output: known English phrases, likely English output strings, exact matches with English source output.");
		report.AppendLine("- Code UI strings: exact matches with English source UI strings in code samples.");
		report.AppendLine("- Code string literals: exact matches with English source string literals and literals that look like untranslated English.");
		report.AppendLine("- Code comments: exact matches with English source comments and comments that look like untranslated English.");
		report.AppendLine("- TOC structure, CJK TOC/heading localization, and language string key parity are covered by dedicated tests.");
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

	private static IEnumerable<MarkdownTextLine> EnumerateTranslatableMarkdownTextLines(string markdown, string relativePath)
	{
		foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
		{
			var normalized = NormalizeMarkdownTextForTranslationCheck(text);
			if (!IsTranslatableEnglishMarkdownText(relativePath, text, normalized))
				continue;

			yield return new MarkdownTextLine(normalized, line);
		}
	}

	private static IEnumerable<MarkdownTextLine> EnumerateKnownEnglishSectionLabels(string markdown, string relativePath)
	{
		foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
		{
			var normalized = NormalizeMarkdownTextForTranslationCheck(text);
			if (!_knownEnglishSectionLabels.Contains(normalized) || IsAllowedInvariantMarkdownText(relativePath, text, normalized))
				continue;

			yield return new MarkdownTextLine(normalized, line);
		}
	}

	private static IEnumerable<MarkdownTextLine> EnumerateLikelyEnglishMarkdownTextLines(string markdown, string relativePath)
	{
		foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
		{
			var normalized = NormalizeMarkdownTextForTranslationCheck(text);
			if (!IsLikelyUntranslatedEnglishMarkdownText(relativePath, text, normalized))
				continue;

			yield return new MarkdownTextLine(normalized, line);
		}
	}

	private static string NormalizeMarkdownTextForTranslationCheck(string text)
	{
		var value = text ?? string.Empty;

		if (Regex.IsMatch(value.Trim(), @"^\|?\s*:?-{3,}:?\s*(?:\|\s*:?-{3,}:?\s*)+\|?$", RegexOptions.CultureInvariant))
			return string.Empty;

		value = Regex.Replace(value, @"^\s*>\s*\[!(?:NOTE|TIP|IMPORTANT|WARNING|CAUTION)\]\s*", string.Empty, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		value = Regex.Replace(value, @"^\s*>+\s*", string.Empty, RegexOptions.CultureInvariant);
		value = Regex.Replace(value, @"^\s{0,3}#{1,6}\s*", string.Empty, RegexOptions.CultureInvariant);
		value = Regex.Replace(value, @"^\s*(?:[-*+]|\d+\.)\s+", string.Empty, RegexOptions.CultureInvariant);
		value = Regex.Replace(value, @"!\[[^\]]*\]\([^)]+\)", " ", RegexOptions.CultureInvariant);
		value = Regex.Replace(value, @"\[(?<label>[^\]]+)\]\([^)]+\)", "${label}", RegexOptions.CultureInvariant);
		value = Regex.Replace(value, @"<[^>]+>", " ", RegexOptions.CultureInvariant);
		value = Regex.Replace(value, @"[（(][^）)]*[A-Za-z][^）)]*[）)]", " ", RegexOptions.CultureInvariant);
		value = value.Replace('|', ' ');
		value = Regex.Replace(value, @"\\([\\`*_{}\[\]()#+\-.!|])", "$1", RegexOptions.CultureInvariant);
		value = Regex.Replace(value, @"[`*_~]+", string.Empty, RegexOptions.CultureInvariant);
		value = Regex.Replace(value, @"\s+", " ", RegexOptions.CultureInvariant).Trim();

		return value;
	}

	private static bool IsTranslatableEnglishMarkdownText(string relativePath, string rawText, string normalizedText)
	{
		if (normalizedText.Length < 24 || normalizedText.Contains(RussianSpecificPlaceholderMarker, StringComparison.OrdinalIgnoreCase))
			return false;

		if (IsAllowedInvariantMarkdownText(relativePath, rawText, normalizedText))
			return false;

		var words = Regex.Matches(normalizedText, @"[A-Za-z][A-Za-z']+", RegexOptions.CultureInvariant)
			.Select(match => match.Value.Trim('\''))
			.Where(word => word.Length > 1)
			.ToArray();

		if (words.Length < 4)
			return false;

		var commonWords = words.Count(IsCommonEnglishMarkdownWord);
		return commonWords >= 2;
	}

	private static bool IsLikelyUntranslatedEnglishMarkdownText(string relativePath, string rawText, string normalizedText)
	{
		if (normalizedText.Length < 32 || normalizedText.Contains(RussianSpecificPlaceholderMarker, StringComparison.OrdinalIgnoreCase))
			return false;

		if (IsAllowedInvariantMarkdownText(relativePath, rawText, normalizedText))
			return false;

		var words = Regex.Matches(normalizedText, @"[A-Za-z][A-Za-z']+", RegexOptions.CultureInvariant)
			.Select(match => match.Value.Trim('\''))
			.Where(word => word.Length > 1)
			.ToArray();

		if (words.Length < 5)
			return false;

		var commonWords = words.Count(IsCommonEnglishMarkdownWord);
		return commonWords >= 3 && (double)commonWords / words.Length >= 0.30;
	}

	private static bool IsTranslatableEnglishMarkdownPlainTextBlock(string relativePath, string text)
	{
		if (text.Length < 24 || text.Contains(RussianSpecificPlaceholderMarker, StringComparison.OrdinalIgnoreCase))
			return false;

		if (IsAllowedInvariantMarkdownText(relativePath, text, text))
			return false;

		var words = Regex.Matches(text, @"[A-Za-z][A-Za-z']+", RegexOptions.CultureInvariant)
			.Select(match => match.Value.Trim('\''))
			.Where(word => word.Length > 1)
			.ToArray();

		if (words.Length < 4)
			return false;

		var commonWords = words.Count(IsCommonEnglishMarkdownWord);
		return commonWords >= 2;
	}

	private static bool IsAllowedInvariantMarkdownText(string relativePath, string rawText, string normalizedText)
	{
		if (string.IsNullOrWhiteSpace(normalizedText))
			return true;

		if (normalizedText.StartsWith("xref:", StringComparison.OrdinalIgnoreCase)
			|| normalizedText.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
			|| normalizedText.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}

		if (Regex.IsMatch(normalizedText, @"^[\p{P}\p{S}\p{N}\s]+$", RegexOptions.CultureInvariant)
			|| Regex.IsMatch(normalizedText, @"^[A-Za-z0-9_.:/#?=&%{}+-]+$", RegexOptions.CultureInvariant))
		{
			return true;
		}

		if (IsShortInvariantName(normalizedText))
			return true;

		var trimmedRaw = rawText.Trim();
		if (trimmedRaw.StartsWith("#", StringComparison.Ordinal))
			return IsAllowedInvariantHeading(relativePath, normalizedText);

		return _allowedInvariantHeadingTexts.Contains(normalizedText)
			|| _allowedInvariantLinkLabels.Contains(normalizedText);
	}

	private static bool IsCommonEnglishMarkdownWord(string word)
		=> !Regex.IsMatch(word, @"^[A-Z]{2,}$", RegexOptions.CultureInvariant)
			&& (word.Equals("a", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("an", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("and", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("are", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("as", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("be", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("by", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("can", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("for", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("from", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("has", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("in", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("into", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("is", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("it", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("of", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("on", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("or", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("that", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("the", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("this", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("to", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("using", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("when", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("which", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("will", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("with", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("you", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("your", StringComparison.OrdinalIgnoreCase));

	private static IEnumerable<MarkdownLinkLabel> EnumerateMarkdownLinkLabels(string markdown)
	{
		var lineStarts = GetLineStarts(markdown);

		foreach (Match match in Regex.Matches(markdown, @"(?<!!)\[(?<label>(?:\\.|[^\]\\])*)\]\((?<url>[^\r\n)]*)\)", RegexOptions.CultureInvariant))
		{
			var text = NormalizeMarkdownLinkLabelForTranslationCheck(match.Groups["label"].Value);
			if (text.Length == 0)
				continue;

			yield return new MarkdownLinkLabel(text, match.Groups["url"].Value.Trim(), GetLineNumber(lineStarts, match.Index));
		}
	}

	private static string NormalizeMarkdownLinkLabelForTranslationCheck(string text)
	{
		var value = Regex.Replace(text ?? string.Empty, @"\\([\\`*_{}\[\]()#+\-.!|])", "$1", RegexOptions.CultureInvariant);
		return Regex.Replace(value, @"\s+", " ", RegexOptions.CultureInvariant).Trim();
	}

	private static bool IsTranslatableEnglishMarkdownLinkLabel(string text, string url)
	{
		if (string.IsNullOrWhiteSpace(text) || text.Length < 8)
			return false;

		var normalizedUrl = (url ?? string.Empty).Replace('\\', '/');
		if (text.Contains(RussianSpecificPlaceholderMarker, StringComparison.OrdinalIgnoreCase)
			|| text.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
			|| text.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
			|| normalizedUrl.Contains("list_of_indicators/", StringComparison.OrdinalIgnoreCase)
			|| IsCodeLikeMarkdownLinkLabel(text)
			|| Regex.IsMatch(text, @"^[\p{P}\p{S}\p{N}\s]+$", RegexOptions.CultureInvariant)
			|| Regex.IsMatch(text, @"^[A-Za-z0-9_.:/#?=&%{}+-]+$", RegexOptions.CultureInvariant)
			|| IsAllowedInvariantLinkLabel(text))
		{
			return false;
		}

		var words = Regex.Matches(text, @"[A-Za-z][A-Za-z']+", RegexOptions.CultureInvariant)
			.Select(match => match.Value.Trim('\''))
			.Where(word => word.Length > 1 && !_allowedInvariantCodeOutputWords.Contains(word))
			.ToArray();

		if (words.Length < 2)
			return false;

		var hits = words.Count(word => _translatableEnglishCodeOutputWords.Contains(word)
			|| IsCommonEnglishMarkdownWord(word)
			|| IsTranslatableEnglishMarkdownImageAltWord(word)
			|| IsTranslatableEnglishMarkdownLinkLabelWord(word));

		return hits >= 1;
	}

	private static bool IsCodeLikeMarkdownLinkLabel(string text)
	{
		if (text.Contains('<', StringComparison.Ordinal)
			|| text.Contains('>', StringComparison.Ordinal)
			|| Regex.IsMatch(text, @"\b(?:Ecng|StockSharp|System)\.", RegexOptions.CultureInvariant))
		{
			return true;
		}

		return Regex.IsMatch(text, @"^[A-Z][A-Za-z0-9_]*(?:\.[A-Z][A-Za-z0-9_]*)+(?:\(\))?$", RegexOptions.CultureInvariant);
	}

	private static bool IsTranslatableEnglishMarkdownLinkLabelWord(string word)
		=> word.Equals("account", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("basic", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("create", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("ecng", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("formats", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("futures", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("metadata", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("repository", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("streams", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("websocket", StringComparison.OrdinalIgnoreCase);

	private static IEnumerable<MarkdownImageAltText> EnumerateMarkdownImageAltTexts(string markdown)
	{
		var lineStarts = GetLineStarts(markdown);

		foreach (Match match in Regex.Matches(markdown, @"!\[(?<alt>(?:\\.|[^\]\\])*)\]\((?<url>[^\r\n)]*)\)", RegexOptions.CultureInvariant))
		{
			var text = NormalizeMarkdownImageAltTextForTranslationCheck(match.Groups["alt"].Value);
			if (text.Length == 0)
				continue;

			yield return new MarkdownImageAltText(text, match.Groups["url"].Value.Trim(), GetLineNumber(lineStarts, match.Index));
		}
	}

	private static string NormalizeMarkdownImageAltTextForTranslationCheck(string text)
	{
		var value = Regex.Replace(text ?? string.Empty, @"\\([\\`*_{}\[\]()#+\-.!|])", "$1", RegexOptions.CultureInvariant);
		return Regex.Replace(value, @"\s+", " ", RegexOptions.CultureInvariant).Trim();
	}

	private static bool AreMarkdownImageAltTextAndFileStemEquivalent(string altText, string fileStem)
	{
		var normalizedAltText = NormalizeMarkdownImageAltTextForFileNameComparison(altText);
		var normalizedFileStem = NormalizeMarkdownImageAltTextForFileNameComparison(fileStem);

		return normalizedAltText.Equals(normalizedFileStem, StringComparison.OrdinalIgnoreCase)
			|| normalizedAltText.Equals(TrimTrailingDigits(normalizedFileStem), StringComparison.OrdinalIgnoreCase);
	}

	private static string NormalizeMarkdownImageAltTextForFileNameComparison(string text)
		=> Regex.Replace(NormalizeMarkdownImageAltTextForTranslationCheck(text), @"[^\p{L}\p{Nd}]+", "", RegexOptions.CultureInvariant);

	private static string TrimTrailingDigits(string text)
		=> Regex.Replace(text, @"\d+$", string.Empty, RegexOptions.CultureInvariant);

	private static string GetLocalizedIndicatorChartDescriptionWord(string lang)
		=> lang switch
		{
			"de" => "Diagramm",
			"es" => "Gráfico",
			"ja" => "チャート",
			"pt" => "Gráfico",
			"ru" => "График",
			"zh" => "图表",
			_ => throw new InvalidOperationException($"Unexpected language '{lang}'."),
		};

	private static bool IsTranslatableEnglishMarkdownImageAltText(string text)
	{
		if (string.IsNullOrWhiteSpace(text) || text.Length < 8)
			return false;

		if (text.Contains(RussianSpecificPlaceholderMarker, StringComparison.OrdinalIgnoreCase)
			|| text.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
			|| text.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
			|| Regex.IsMatch(text, @"^[\p{P}\p{S}\p{N}\s]+$", RegexOptions.CultureInvariant)
			|| Regex.IsMatch(text, @"^[A-Za-z0-9_.:/#?=&%{}+-]+$", RegexOptions.CultureInvariant))
		{
			return false;
		}

		var words = Regex.Matches(text, @"[A-Za-z][A-Za-z']+", RegexOptions.CultureInvariant)
			.Select(match => match.Value.Trim('\''))
			.Where(word => word.Length > 1 && !_allowedInvariantCodeOutputWords.Contains(word))
			.ToArray();

		if (words.Length < 2)
			return false;

		var hits = words.Count(word => _translatableEnglishCodeOutputWords.Contains(word)
			|| IsCommonEnglishMarkdownWord(word)
			|| IsTranslatableEnglishMarkdownImageAltWord(word));

		return hits >= 1;
	}

	private static bool IsTranslatableEnglishMarkdownImageAltWord(string word)
		=> word.Equals("activating", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("activation", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("administrator", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("book", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("bot", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("channel", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("chart", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("choosing", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("circuits", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("cloud", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("composite", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("cube", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("cubes", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("depth", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("download", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("emulation", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("graphics", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("historical", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("installer", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("installerzip", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("installation", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("interface", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("live", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("login", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("logs", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("market", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("notifications", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("optimization", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("panel", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("portfolio", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("portfolios", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("position", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("properties", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("quick", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("remote", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("repository", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("strategies", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("table", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("telegram", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("tools", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("trading", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("visual", StringComparison.OrdinalIgnoreCase);

	private static IEnumerable<CodeComment> EnumerateCodeComments(string markdown)
	{
		foreach (Match block in Regex.Matches(markdown, "```(?<info>[^\\r\\n]*)\\r?\\n(?<code>.*?)```", RegexOptions.Singleline | RegexOptions.CultureInvariant))
		{
			var info = block.Groups["info"].Value.Trim();
			var line = GetLineNumber(GetLineStarts(markdown), block.Groups["code"].Index);
			using var reader = new StringReader(block.Groups["code"].Value);

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				var trimmed = text.Trim();
				var fullLineComment = false;

				foreach (Match xmlComment in Regex.Matches(trimmed, @"<!--\s*.*?\s*-->", RegexOptions.CultureInvariant))
				{
					fullLineComment = true;
					yield return new CodeComment(xmlComment.Value, line);
				}

				foreach (Match blockComment in Regex.Matches(trimmed, @"/\*\s*.*?\s*\*/", RegexOptions.CultureInvariant))
				{
					fullLineComment = true;
					yield return new CodeComment(blockComment.Value, line);
				}

				if (trimmed.StartsWith("///", StringComparison.Ordinal)
					|| trimmed.StartsWith("//", StringComparison.Ordinal)
					|| trimmed.StartsWith("#", StringComparison.Ordinal))
				{
					fullLineComment = true;
					yield return new CodeComment(trimmed, line);
				}

				if (fullLineComment)
					continue;

				var slashComment = FindInlineLineCommentStart(text, "//");
				if (slashComment > 0)
					yield return new CodeComment(text[slashComment..].Trim(), line);

				if (IsPythonCodeBlock(info))
				{
					var hashComment = FindInlineLineCommentStart(text, "#");
					if (hashComment > 0)
						yield return new CodeComment(text[hashComment..].Trim(), line);
				}
			}
		}
	}

	private static bool IsPythonCodeBlock(string info)
		=> info.Equals("python", StringComparison.OrdinalIgnoreCase)
			|| info.Equals("py", StringComparison.OrdinalIgnoreCase);

	private static int FindInlineLineCommentStart(string text, string marker)
	{
		var inSingleQuotedString = false;
		var inDoubleQuotedString = false;

		for (var i = 0; i <= text.Length - marker.Length; i++)
		{
			var ch = text[i];
			if (ch == '\\')
			{
				i++;
				continue;
			}

			if (!inDoubleQuotedString && ch == '\'')
			{
				inSingleQuotedString = !inSingleQuotedString;
				continue;
			}

			if (!inSingleQuotedString && ch == '"')
			{
				inDoubleQuotedString = !inDoubleQuotedString;
				continue;
			}

			if (inSingleQuotedString || inDoubleQuotedString)
				continue;

			if (!text.AsSpan(i).StartsWith(marker, StringComparison.Ordinal))
				continue;

			if (marker == "//" && i > 0 && text[i - 1] == ':')
				continue;

			if (text[..i].Trim().Length == 0)
				continue;

			return i;
		}

		return -1;
	}

	private static IEnumerable<CodeOutputString> EnumerateCodeOutputStrings(string markdown)
	{
		foreach (Match block in Regex.Matches(markdown, "```[^\\r\\n]*\\r?\\n(?<code>.*?)```", RegexOptions.Singleline | RegexOptions.CultureInvariant))
		{
			var line = GetLineNumber(GetLineStarts(markdown), block.Groups["code"].Index);
			var inOutputInvocation = false;
			using var reader = new StringReader(block.Groups["code"].Value);

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				if (!inOutputInvocation && IsCodeOutputInvocationStart(text))
					inOutputInvocation = true;

				if (!inOutputInvocation)
					continue;

				foreach (Match literal in Regex.Matches(text, @"\$?""(?<value>[^""\\]*(?:\\.[^""\\]*)*)""", RegexOptions.CultureInvariant))
				{
					var value = literal.Groups["value"].Value;
					value = Regex.Replace(value, @"\{[^{}]*\}", " ", RegexOptions.CultureInvariant);
					value = Regex.Replace(value, @"\s+", " ", RegexOptions.CultureInvariant).Trim();

					if (value.Length > 0)
						yield return new CodeOutputString(value, line);
				}

				if (IsCodeOutputInvocationEnd(text))
					inOutputInvocation = false;
			}
		}
	}

	private static bool IsCodeOutputInvocationStart(string text)
		=> Regex.IsMatch(text, @"\b(?:Console\.Write(?:Line)?|Add(?:Info|Debug|Warning|Error)Log|AlertLog|MessageBox\.Show|Trace\.Write(?:Line)?)\s*\(", RegexOptions.CultureInvariant);

	private static bool IsCodeOutputInvocationEnd(string text)
		=> Regex.IsMatch(text, @"\)\s*;", RegexOptions.CultureInvariant);

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

	private static IEnumerable<CodeUiString> EnumerateCodeUiStrings(string markdown)
	{
		foreach (Match block in Regex.Matches(markdown, "```[^\\r\\n]*\\r?\\n(?<code>.*?)```", RegexOptions.Singleline | RegexOptions.CultureInvariant))
		{
			var line = GetLineNumber(GetLineStarts(markdown), block.Groups["code"].Index);
			var inUiInvocation = false;
			using var reader = new StringReader(block.Groups["code"].Value);

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				foreach (var value in EnumerateCodeUiAttributeStrings(text))
					yield return new CodeUiString(value, line);

				foreach (var value in EnumerateCodeUiAssignmentStrings(text))
					yield return new CodeUiString(value, line);

				if (!inUiInvocation && IsCodeUiInvocationStart(text))
					inUiInvocation = true;

				if (!inUiInvocation)
					continue;

				foreach (Match literal in Regex.Matches(text, @"\$?""(?<value>[^""\\]*(?:\\.[^""\\]*)*)""", RegexOptions.CultureInvariant))
				{
					var value = NormalizeCodeUiLiteral(literal.Groups["value"].Value);

					if (value.Length > 0)
						yield return new CodeUiString(value, line);
				}

				if (IsCodeUiInvocationEnd(text))
					inUiInvocation = false;
			}
		}
	}

	private static IEnumerable<string> EnumerateCodeUiAttributeStrings(string text)
	{
		foreach (Match match in Regex.Matches(text, @"\[(?:DisplayName|Description|Category)\(\s*""(?<value>[^""\\]*(?:\\.[^""\\]*)*)""", RegexOptions.CultureInvariant))
		{
			var value = NormalizeCodeUiLiteral(match.Groups["value"].Value);
			if (value.Length > 0)
				yield return value;
		}
	}

	private static IEnumerable<string> EnumerateCodeUiAssignmentStrings(string text)
	{
		foreach (Match match in Regex.Matches(text, @"\b(?:FullTitle|Title|Content|Header|Caption|Filter|GroupName)\s*=\s*\$?""(?<value>[^""\\]*(?:\\.[^""\\]*)*)""", RegexOptions.CultureInvariant))
		{
			var rawValue = match.Groups["value"].Value;
			if (rawValue.TrimStart().StartsWith("{", StringComparison.Ordinal))
				continue;

			var value = NormalizeCodeUiLiteral(rawValue);
			if (value.Length > 0)
				yield return value;
		}
	}

	private static bool IsCodeUiInvocationStart(string text)
		=> Regex.IsMatch(text, @"\bSetDisplay\s*\(", RegexOptions.CultureInvariant);

	private static bool IsCodeUiInvocationEnd(string text)
		=> Regex.IsMatch(text, @"\)\s*;?\s*$", RegexOptions.CultureInvariant);

	private static string NormalizeCodeUiStringForTranslationCheck(string text)
		=> Regex.Replace(text ?? string.Empty, @"\s+", " ", RegexOptions.CultureInvariant).Trim();

	private static string NormalizeCodeUiLiteral(string text)
	{
		var value = Regex.Replace(text ?? string.Empty, @"\{[^{}]*\}", " ", RegexOptions.CultureInvariant);
		return NormalizeCodeUiStringForTranslationCheck(value);
	}

	private static bool IsTranslatableEnglishCodeUiString(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
			return false;

		if (Regex.IsMatch(text, @"^(?:[A-Z0-9_./:+#-]+|[a-zA-Z_][a-zA-Z0-9_.]*\(\))$", RegexOptions.CultureInvariant))
			return false;

		if (_knownEnglishSectionLabels.Contains(text))
			return true;

		var words = Regex.Matches(text, @"[A-Za-z][A-Za-z']+", RegexOptions.CultureInvariant)
			.Select(match => match.Value.Trim('\''))
			.Where(word => word.Length > 2 && !_allowedInvariantCodeOutputWords.Contains(word))
			.ToArray();

		if (words.Length == 0)
			return false;

		var hits = words.Count(word => _translatableEnglishCodeOutputWords.Contains(word));
		return words.Length == 1 ? hits == 1 : hits >= 1;
	}

	private static IEnumerable<CodeStringLiteral> EnumerateCodeStringLiterals(string markdown)
	{
		foreach (Match block in Regex.Matches(markdown, "```[^\\r\\n]*\\r?\\n(?<code>.*?)```", RegexOptions.Singleline | RegexOptions.CultureInvariant))
		{
			var markdownLineStarts = GetLineStarts(markdown);
			var code = block.Groups["code"].Value;

			foreach (Match literal in Regex.Matches(code, "(?<quote>\"\"\"|''')(?<value>.*?)(?:\\k<quote>)", RegexOptions.Singleline | RegexOptions.CultureInvariant))
			{
				var value = NormalizeCodeStringLiteralForTranslationCheck(literal.Groups["value"].Value);
				if (value.Length > 0)
					yield return new CodeStringLiteral(value, GetLineNumber(markdownLineStarts, block.Groups["code"].Index + literal.Groups["value"].Index));
			}

			var line = GetLineNumber(markdownLineStarts, block.Groups["code"].Index);
			using var reader = new StringReader(code);

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				foreach (Match literal in Regex.Matches(text, @"(?:\$?@?""(?<double>[^""\\]*(?:\\.[^""\\]*)*)""|\$?'(?<single>[^'\\]*(?:\\.[^'\\]*)*)')", RegexOptions.CultureInvariant))
				{
					var value = literal.Groups["double"].Success
						? literal.Groups["double"].Value
						: literal.Groups["single"].Value;

					value = NormalizeCodeStringLiteralForTranslationCheck(value);
					if (value.Length > 0)
						yield return new CodeStringLiteral(value, line);
				}
			}
		}
	}

	private static string NormalizeCodeStringLiteralForTranslationCheck(string text)
	{
		var value = Regex.Replace(text ?? string.Empty, @"\\[rnt]", " ", RegexOptions.CultureInvariant);
		value = Regex.Replace(value, @"\{[^{}]*\}", " ", RegexOptions.CultureInvariant);
		value = Regex.Replace(value, @"\s+", " ", RegexOptions.CultureInvariant).Trim();
		return value;
	}

	private static bool IsTranslatableEnglishCodeStringLiteral(string text)
	{
		if (string.IsNullOrWhiteSpace(text) || text.Length < 6)
			return false;

		if (IsAllowedInvariantCodeStringLiteral(text))
			return false;

		var words = Regex.Matches(text, @"[A-Za-z][A-Za-z']+", RegexOptions.CultureInvariant)
			.Select(match => match.Value.Trim('\''))
			.Where(word => word.Length > 1 && !_allowedInvariantCodeOutputWords.Contains(word))
			.ToArray();

		if (words.Length < 2)
			return false;

		var hits = words.Count(word => _translatableEnglishCodeOutputWords.Contains(word) || IsCommonEnglishMarkdownWord(word));
		return hits >= 2;
	}

	private static bool IsLikelyUntranslatedEnglishCodeStringLiteral(string text)
	{
		if (string.IsNullOrWhiteSpace(text) || text.Length < 6 || IsAllowedInvariantCodeStringLiteral(text))
			return false;

		var value = Regex.Replace(text, @"https?://[^\s)\]>""']+", " ", RegexOptions.CultureInvariant);
		value = Regex.Replace(value, @"\b(?:param|return|returns)\b:?", " ", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		value = Regex.Replace(value, @"\s+", " ", RegexOptions.CultureInvariant).Trim();

		var words = Regex.Matches(value, @"[A-Za-z][A-Za-z']+", RegexOptions.CultureInvariant)
			.Select(match => match.Value.Trim('\''))
			.Where(word => word.Length > 1
				&& !Regex.IsMatch(word, @"^[A-Z]{2,}$", RegexOptions.CultureInvariant)
				&& !_allowedInvariantCodeOutputWords.Contains(word))
			.ToArray();

		if (words.Length < 2)
			return false;

		var commonWords = words.Count(IsCommonEnglishMarkdownWord);
		var translatableWords = words.Count(IsLikelyEnglishCodeStringLiteralWord);

		if (words.Length <= 4)
			return translatableWords >= 2 && (commonWords > 0 || words.Any(IsShortEnglishCodeStringLiteralMarkerWord));

		return commonWords >= 1
			&& translatableWords >= 4
			&& (double)translatableWords / words.Length >= 0.35;
	}

	private static bool IsLikelyEnglishCodeStringLiteralWord(string word)
		=> IsCommonEnglishMarkdownWord(word)
			|| _translatableEnglishCodeOutputWords.Contains(word)
			|| word.Equals("called", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("change", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("changes", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("crossing", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("demonstrating", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("diagram", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("element", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("executes", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("finished", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("incoming", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("loading", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("logs", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("persistent", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("random", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("resets", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("sample", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("saves", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("saving", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("settings", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("starts", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("stops", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("strategy", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("usage", StringComparison.OrdinalIgnoreCase);

	private static bool IsShortEnglishCodeStringLiteralMarkerWord(string word)
		=> word.Equals("created", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("custom", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("my", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("pattern", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("profit", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("required", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("special", StringComparison.OrdinalIgnoreCase);

	private static bool IsAllowedInvariantCodeStringLiteral(string text)
	{
		var value = text.Trim();

		if (value.StartsWith("{", StringComparison.Ordinal)
			|| value.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
			|| value.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
			|| value.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase)
			|| value.StartsWith("pack://", StringComparison.OrdinalIgnoreCase)
			|| value.StartsWith("clr-namespace:", StringComparison.OrdinalIgnoreCase)
			|| value.StartsWith("xmlns", StringComparison.OrdinalIgnoreCase)
			|| value.StartsWith("xref:", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}

		if (Regex.IsMatch(value, @"^[\p{P}\p{S}\p{N}\s]+$", RegexOptions.CultureInvariant)
			|| Regex.IsMatch(value, @"^[A-Z0-9_./:+#?=&%{}*|@,-]+$", RegexOptions.CultureInvariant)
			|| Regex.IsMatch(value, @"^[A-Za-z]:\\|^[/\\]|^\.\.?[/\\]", RegexOptions.CultureInvariant)
			|| Regex.IsMatch(value, @"^[A-Za-z_][A-Za-z0-9_.]*$", RegexOptions.CultureInvariant)
			|| Regex.IsMatch(value, @"^[A-Za-z_][A-Za-z0-9_.]*(?:\.[A-Za-z_][A-Za-z0-9_.]*)+$", RegexOptions.CultureInvariant)
			|| Regex.IsMatch(value, @"^[A-Za-z0-9_./-]+\.(?:dll|exe|html|json|xml|csv|txt|md|png|jpg|jpeg|bmp|gif|svg|cs|fs|py|xaml|sln|csproj|fsproj)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			return true;
		}

		return false;
	}

	private static string NormalizeCodeCommentForTranslationCheck(string comment)
	{
		if (Regex.IsMatch(comment, @"https?://|nameof\(|^[#/\\\s-]*$|^//\s*[A-Z][A-Za-z0-9_.]+\s*=", RegexOptions.CultureInvariant))
			return string.Empty;

		var text = Regex.Replace(comment, @"^\s*(?:///?|#|<!--|/\*)\s*", string.Empty, RegexOptions.CultureInvariant);
		text = Regex.Replace(text, @"\s*-->\s*$", string.Empty, RegexOptions.CultureInvariant);
		text = Regex.Replace(text, @"\s*\*/\s*$", string.Empty, RegexOptions.CultureInvariant);
		text = Regex.Replace(text, @"<[^>]+>", " ", RegexOptions.CultureInvariant);
		text = Regex.Replace(text, @"\s+", " ", RegexOptions.CultureInvariant).Trim();

		if (text.Length < 12 && !_knownEnglishCodeCommentLabels.Contains(text))
			return string.Empty;

		if (_knownEnglishCodeCommentLabels.Contains(text))
			return text;

		if (IsCodeLikeCommentText(text))
			return string.Empty;

		return text;
	}

	private static bool IsCodeLikeCommentText(string text)
	{
		if (Regex.IsMatch(text, @"^<[^>]+/?>$|^[A-Za-z_][A-Za-z0-9_.]*$|^[A-Za-z_][A-Za-z0-9_]*\s*-\s*StockSharp(?:\.[A-Za-z_][A-Za-z0-9_]*)+$|[;{}=]", RegexOptions.CultureInvariant))
			return true;

		// Skip commented-out code and compiler/preprocessor directives without hiding prose comments
		// such as "if no historical data..." or "Class for analyzing...".
		return Regex.IsMatch(text, @"^[+\-*/]\s*[A-Za-z_][A-Za-z0-9_.]*(?:\.|\()", RegexOptions.CultureInvariant)
			|| Regex.IsMatch(text, @"^(?:if|for|while)\s*\(", RegexOptions.CultureInvariant)
			|| Regex.IsMatch(text, @"^return\s+(?:true|false|null|default|new\b|[A-Za-z_][A-Za-z0-9_.]*(?:\([^)]*\))?)$", RegexOptions.CultureInvariant)
			|| Regex.IsMatch(text, @"^(?:using|var|let|public|private|protected|await|yield|pragma|region|endregion|nullable|define|endif|else|elif|def|with)\b", RegexOptions.CultureInvariant)
			|| Regex.IsMatch(text, @"^(?:class|new)\s+[A-Za-z_][A-Za-z0-9_.]*(?:\b|[<(])", RegexOptions.CultureInvariant);
	}

	private static string NormalizeCodeCommentForLikelyEnglishCheck(string comment)
	{
		if (Regex.IsMatch(comment, @"https?://|nameof\(|^[#/\\\s-]*$|^//\s*[A-Z][A-Za-z0-9_.]+\s*=", RegexOptions.CultureInvariant))
			return string.Empty;

		var text = Regex.Replace(comment, @"^\s*(?:///?|#|<!--|/\*)\s*", string.Empty, RegexOptions.CultureInvariant);
		text = Regex.Replace(text, @"\s*-->\s*$", string.Empty, RegexOptions.CultureInvariant);
		text = Regex.Replace(text, @"\s*\*/\s*$", string.Empty, RegexOptions.CultureInvariant);
		text = Regex.Replace(text, @"<[^>]+>", " ", RegexOptions.CultureInvariant);
		text = Regex.Replace(text, @"\s+", " ", RegexOptions.CultureInvariant).Trim();

		if (text.Length < 20)
			return string.Empty;

		// Unlike exact-comment matching, keep parentheses: translated comments often use them
		// for explanatory prose, and several previous misses were English comments in parentheses.
		if (IsCodeLikeCommentText(text))
			return string.Empty;

		return text;
	}

	private static bool IsLikelyUntranslatedEnglishCodeComment(string comment)
	{
		if (string.IsNullOrWhiteSpace(comment))
			return false;

		var words = Regex.Matches(comment, @"[A-Za-z][A-Za-z']+", RegexOptions.CultureInvariant)
			.Select(match => match.Value.Trim('\''))
			.Where(word => word.Length > 1
				&& !Regex.IsMatch(word, @"^[A-Z]{2,}$", RegexOptions.CultureInvariant)
				&& !IsAllowedInvariantCodeCommentWord(word))
			.ToArray();

		if (words.Length < 4)
			return false;

		var hits = words.Count(IsTranslatableEnglishCodeCommentWord);
		return hits >= 3 && (double)hits / words.Length >= 0.45;
	}

	private static bool IsAllowedInvariantCodeCommentWord(string word)
		=> _allowedInvariantCodeOutputWords.Contains(word)
			|| word.Equals("api", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("board", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("candlemessage", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("csharp", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("ecng", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("executionmessage", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("fsharp", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("securityid", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("system", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("timespan", StringComparison.OrdinalIgnoreCase);

	private static bool IsTranslatableEnglishCodeCommentWord(string word)
		=> IsCommonEnglishMarkdownWord(word)
			|| _translatableEnglishCodeOutputWords.Contains(word)
			|| word.Equals("automatically", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("built", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("building", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("custom", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("descending", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("disable", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("discrete", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("display", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("example", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("explicit", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("filter", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("form", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("generate", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("generated", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("gateway", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("here", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("history", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("initialize", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("list", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("loads", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("mechanism", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("normally", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("objects", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("own", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("protective", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("regular", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("search", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("selected", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("server", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("sorting", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("subscription", StringComparison.OrdinalIgnoreCase)
			|| word.Equals("transaction", StringComparison.OrdinalIgnoreCase);

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

	private static IEnumerable<MarkdownTextLine> EnumerateMarkdownBoldTexts(string markdown)
	{
		var boldPattern = new Regex(@"\*\*(?<label>[^*`\r\n]+)\*\*", RegexOptions.CultureInvariant);

		foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
		{
			foreach (Match match in boldPattern.Matches(text))
			{
				var label = NormalizeHumanText(match.Groups["label"].Value);

				if (label.Length > 0)
					yield return new MarkdownTextLine(label, line);
			}
		}
	}

	private static Dictionary<string, string> ReadIndicatorListDescriptionLeads(string file)
		=> EnumerateIndicatorListDescriptionLeads(file)
			.ToDictionary(entry => entry.Url, entry => entry.Lead, StringComparer.OrdinalIgnoreCase);

	private static IEnumerable<IndicatorListDescriptionLead> EnumerateIndicatorListDescriptionLeads(string file)
	{
		var pattern = new Regex(@"^\s*-\s+\[[^\]\r\n]+\]\((?<url>[^\)\r\n]+)\)\s+-\s+(?<lead>.+?)(?:,|\u3001|\u3002|\uFF0C|\uFF1B|;|\.)", RegexOptions.CultureInvariant);

		foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
		{
			var match = pattern.Match(text);
			if (!match.Success)
				continue;

			var url = match.Groups["url"].Value.Trim();
			var lead = NormalizeHumanText(match.Groups["lead"].Value);

			if (url.Length > 0 && lead.Length > 0)
				yield return new IndicatorListDescriptionLead(url, lead, line);
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

	private static IReadOnlyList<string> GetLocalizedContentQualityLanguages()
		=> GetContentLanguages()
			.Where(lang => !lang.Equals(DefaultLanguage, StringComparison.OrdinalIgnoreCase))
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

	private readonly record struct TocEntryText(string Name, string Href);

	private readonly record struct TocNameLine(string Name, int Line);

	private readonly record struct MarkdownTarget(string Language, bool Exists, bool ExactCase, string FullPath, string ActualRelativePath);

	private readonly record struct MarkdownStructure(
		IReadOnlyList<int> HeadingLevels,
		IReadOnlyList<string> CodeBlockLanguages,
		IReadOnlyList<string> LinkUrls,
		IReadOnlyList<string> ImageUrls,
		IReadOnlyList<TableShape> TableShapes,
		ListSummary ListSummary,
		IReadOnlyList<string> AdmonitionKinds);

	private readonly record struct TableShape(int Columns, int BodyRows);

	private readonly record struct ListSummary(int OrderedItems, int UnorderedItems);

	private readonly record struct HeadingText(string Text, int Line);

	private readonly record struct MarkdownTextLine(string Text, int Line);

	private readonly record struct MarkdownTableCellText(string Text, int Line);

	private readonly record struct MarkdownPlainTextBlock(string Text, int Line);

	private readonly record struct MarkdownLinkLabel(string Text, string Url, int Line);

	private readonly record struct MarkdownImageAltText(string Text, string Url, int Line);

	private readonly record struct IndicatorListDescriptionLead(string Url, string Lead, int Line);

	private readonly record struct CodeComment(string Text, int Line);

	private readonly record struct CodeOutputString(string Text, int Line);

	private readonly record struct CodeUiString(string Text, int Line);

	private readonly record struct CodeStringLiteral(string Text, int Line);

	private sealed record LocalizedDocumentationAudit(
		IReadOnlyList<string> Languages,
		IReadOnlyList<LocalizedAuditLanguageStats> Stats,
		IReadOnlyList<AuditIssue> Issues);

	private readonly record struct LocalizedAuditLanguageStats(
		string Language,
		int MarkdownFiles,
		int TextFiles,
		int MarkdownTextCandidates,
		int MarkdownTableCells,
		int MarkdownPlainTextBlocks,
		int MarkdownLinkLabels,
		int MarkdownImageAltTexts,
		int CodeOutputStrings,
		int CodeUiStrings,
		int CodeStringLiterals,
		int CodeComments);

	private readonly record struct AuditIssue(string Category, string File, int Line, string Message)
	{
		public string Location => Line > 0 ? $"{File}:{Line}" : File;

		public override string ToString()
			=> $"{Location}: [{Category}] {Message}";
	}
}
