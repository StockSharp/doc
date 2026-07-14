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

	private static readonly string[] _stockSharpSiteLanguages =
	[
		"en",
		"ru",
		"de",
		"es",
		"pt",
		"ja",
		"zh",
	];

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
		"\"Logs\" パネル",
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
		"Equity P&L",
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
		"Profit/Loss",
		"Profit\\/Loss",
		"Manage NuGet Packages",
		"More info",
		"Open debug launch profiles UI",
		"Own Volume",
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
		"Credentials",
		"Demo",
		"Derivatives mode",
		"Expires after",
		"History",
		"Info endpoint / Exchange endpoint / WS endpoint",
		"Indicator",
		"Key",
		"Licenses",
		"Market slippage",
		"Orders",
		"Passphrase",
		"P/L realized",
		"P/L unrealized",
		"Private key",
		"Section",
		"Sections",
		"Secret",
		"Security mapping",
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
		new(@"\bauto[ -]?scroll\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
		new(@"\bauto[ -]?zoom\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
		new(@"\bbox charts?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
		new(@"\bcombo boxes?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
		new(@"\b[Gg]r[aá]fico box\b", RegexOptions.CultureInvariant),
		new(@"\bpassphrases?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
	];

	private static readonly string[] _knownEnglishMarkdownCodeBlockListLabels =
	[
		"Logging",
	];

	private static readonly string[] _knownEnglishDesignerElementColorLabels =
	[
		"Black",
		"Green",
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

	private static readonly string[] _knownEnglishCandlePatternNames =
	[
		"3 Black Crows and 3 White Soldiers",
		"3 Inside Down and 3 Inside Up",
		"3 Outside Down and 3 Outside Up",
		"Flat (Neutral) Candle",
		"Flat Candle",
		"Falling Three Methods",
		"Rising Three Methods",
		"Flat Candles",
		"Black Candles",
		"White Candles",
		"Three Black Crows",
		"Three White Soldiers",
		"Bearish Engulfing",
		"Bullish Engulfing",
		"Bearish Harami",
		"Bullish Harami",
		"Black Marubozu",
		"White Marubozu",
		"Bearish Candle",
		"Bullish Candle",
		"Black Candle",
		"White Candle",
		"Evening Star",
		"Evening Doji Star",
		"Morning Star",
		"Shooting Star",
		"Morning Doji Star",
		"Harami Cross",
		"Inverted Hammer",
		"Hanging Man",
		"Spinning Top",
		"Tweezer Bottom",
		"Tweezer Top",
		"3 Black Crows",
		"3 White Soldiers",
		"3 Inside Down",
		"3 Inside Up",
		"3 Outside Down",
		"3 Outside Up",
		"On-Neck",
		"Dragonfly",
		"Gravestone",
		"Hammer",
		"Piercing",
	];

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
		"Open Interest",
		"Identifier (user)",
		"Market Maker",
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
		"AverageDirectionalIndex indicator value implementation.",
		"Backtesting",
		"Connector",
		"Core",
		"create delta hedge strategy (requires BasketBlackScholes model)",
		"fill series",
		"Gets the value.",
		"Indicators",
		"If there is no position, use Volume; otherwise, double",
		"is a disk",
		"Localization",
		"Localization (Russian)",
		"parameters",
		"Rate of change.",
		"show DOM",
		"Strategies and indicators",
	};

	private static readonly (string Name, string Pattern)[] _knownEnglishCodeOutputPatterns =
	[
		("subscription lifecycle output", @"\bSubscription (?:started|completed|interrupted|online|switched to real-time mode)\b"),
		("adapter connection output", @"\bAdapter (?:connected|disconnected|connection error)\b"),
		("order execution output", @"\bOrder\s+executed\b|\bOrder fully executed\b|\bOrder successfully (?:registered|canceled)\b|\bOrder not accepted by the exchange\b|\bOrder #1 (?:registered|not registered)\b|\bOrder №[12] RegisterFailed\b"),
		("rule output", @"\bRule WhenOrderBookReceived\b"),
		("strategy timer output", @"\bChecking market conditions at\b|\bCurrent position:|\bCurrent PnL:|\bPosition hold time expired, closing\b|\bTarget position reached:"),
		("strategy logging output", @"\bStrategy\s+started at\b|\bStrategy\s+stopped\. Position:"),
		("strategy state output", @"\bCurrent state\b.*\benter spread\b"),
		("market data output", @"\bLast price\b|\bDrive created:|\bInstrument:"),
		("round-trip output", @"\bPosition closed:|\bMax volume:"),
		("round-trip details output", @"\bRound-trip completed:|\bOpened:|\bClosed:"),
		("mixed round-trip output", @"\bRound-trip\s+заверш"),
		("PnL summary output", @"\bRealized PnL:|\bUnrealized PnL:|\bTotal PnL:"),
		("optimization result output", @"\bIteration complete:|\bBest result:"),
		("optimization parameter output", @"\bLongSma=|\bShortSma="),
		("statistics output", @"\bNet Profit:|\bNet profit:|\bTotal iterations:"),
		("latency output", @"\bLatency:|\bRegistration latency:|\bCancellation latency:"),
		("slippage summary output", @"\bTotal slippage:"),
		("ShrinkPrice output", @"\bOrder price:|\bOriginal price:|\bAfter ShrinkPrice:"),
		("candle OHLC output", @"\b(?:Open|Close|High|Low)="),
		("tick price output", @"\bTick:.*\bPrice:"),
		("order book output", @"\bOrder Book:|\bBest Bid\b|\bBest Ask\b|\bMiddle of Spread\b|\bBid Price:|\bAsk Price:"),
		("field/value output", @"\bField:|\bValue:|\bBids:|\bAsks:"),
		("event rule output", @"\bCandle closed or time expired\b|\bLast trade price is in the range from\b"),
		("test source error output", @"\bError \(source\)!!!"),
		("Portuguese candle output", @"\bCandle (?:recebido|histórico|fechada|de)\b"),
		("stairs countertrend candle output", @"\b(?:Bullish|Bearish) candle detected\. Streak:"),
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
	public void LocalizedTocFilesDoNotKeepKnownEnglishLabels()
	{
		var errors = new List<string>();
		var checks = new[]
		{
			(
				Lang: "de",
				RelativePath: "topics/toc.yml",
				Labels: new[] { "Brute Force" }
			),
		};

		foreach (var check in checks)
		{
			var tocPath = Path.Combine(_repoRoot, check.Lang, check.RelativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(tocPath))
				continue;

			foreach (var nameLine in EnumerateTocNameLines(tocPath))
			{
				foreach (var label in check.Labels)
				{
					if (!nameLine.Name.Equals(label, StringComparison.OrdinalIgnoreCase))
						continue;

					errors.Add($"{RelativeToRepo(tocPath)}:{nameLine.Line}: localized TOC keeps English label '{label}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void RussianIndicatorTocDoesNotKeepEnglishGenericNames()
	{
		var errors = new List<string>();
		var tocPath = Path.Combine(_repoRoot, "ru", "topics", "toc.yml");
		var entries = ReadTocEntries(tocPath, errors);
		var pattern = new Regex(@"\b(?:Accumulation/Distribution|Adaptive|Approval|Average|Balance|Bands|Crossover|Divergence|Histogram|Index|Line|Market|Momentum|Moving|Oscillator|Price|Range|Ribbon|Signal|Strength|Trend|Volume|Weighted)\b", RegexOptions.CultureInvariant);

		if (entries is not null)
		{
			foreach (var entry in FlattenTocEntries(entries))
			{
				var href = NormalizeStructureUrl(entry.Href);
				if (!href.StartsWith("api/indicators/list_of_indicators/", StringComparison.OrdinalIgnoreCase))
					continue;

				var match = pattern.Match(entry.Name);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(tocPath)}: Russian indicator TOC item '{entry.Name}' for '{href}' keeps English generic word '{match.Value}'. Localize the visible name while preserving indicator acronyms.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void RussianTocDoesNotKeepKnownEnglishNavigationFragments()
	{
		var errors = new List<string>();
		var tocPath = Path.Combine(_repoRoot, "ru", "topics", "toc.yml");
		var fragments = new[]
		{
			"Market data",
			"Transactions",
			"Latency",
			"Логирование Strategy",
			"box chart",
			"Outside Down",
			"Inside Down",
			"Black Crows",
			"White Soldiers",
			"On-Neck",
			"Bearish",
			"Bullish",
			"Dragonfly",
			"Evening Star",
			"Falling Three",
			"Flat Candle",
			"Gravestone",
			"Hanging Man",
			"Inverted Hammer",
			"Morning Star",
			"Rising Three",
			"Shooting Star",
			"Spinning Top",
			"Tweezer",
			"White Candle",
			"Black Candle",
			"Candle パターン",
			"Candle 模式",
		};

		foreach (var nameLine in EnumerateTocNameLines(tocPath))
		{
			foreach (var fragment in fragments)
			{
				if (!nameLine.Name.Contains(fragment, StringComparison.OrdinalIgnoreCase))
					continue;

				errors.Add($"{RelativeToRepo(tocPath)}:{nameLine.Line}: Russian TOC keeps English navigation fragment '{fragment}' in '{nameLine.Name}'.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedPatternTocNamesDoNotKeepEnglishCandlestickFragments()
	{
		var errors = new List<string>();
		var fragments = new[]
		{
			"Outside Down",
			"Inside Down",
			"Black Crows",
			"White Soldiers",
			"On-Neck",
			"Piercing",
			"Bearish",
			"Bullish",
			"Dragonfly",
			"Evening Star",
			"Falling Three",
			"Flat Candle",
			"Gravestone",
			"Hanging Man",
			"Inverted Hammer",
			"Morning Star",
			"Rising Three",
			"Shooting Star",
			"Spinning Top",
			"Tweezer",
			"White Candle",
			"Black Candle",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var tocPath = Path.Combine(_repoRoot, lang, "topics", "toc.yml");
			if (!File.Exists(tocPath))
				continue;

			var entries = ReadTocEntries(tocPath, errors);
			if (entries is null)
				continue;

			foreach (var entry in FlattenTocEntries(entries))
			{
				var href = NormalizeStructureUrl(entry.Href);
				if (!href.StartsWith("api/patterns/", StringComparison.OrdinalIgnoreCase))
					continue;

				foreach (var fragment in fragments)
				{
					if (!entry.Name.Contains(fragment, StringComparison.OrdinalIgnoreCase))
						continue;

					errors.Add($"{RelativeToRepo(tocPath)}: localized pattern TOC item '{entry.Name}' for '{href}' keeps English candlestick fragment '{fragment}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedTocDoesNotKeepKnownEnglishGenericNavigationFragments()
	{
		var errors = new List<string>();
		var fragments = new (string Name, Regex Pattern)[]
		{
			("Logging", new Regex(@"\bLogging\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)),
			("Backtesting", new Regex(@"\bBacktesting\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)),
			("Configuración live", new Regex(@"\bConfiguración live\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)),
			("Exemplo de execução em Live", new Regex(@"\bExemplo de execução em Live\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)),
			("Tarea Import", new Regex(@"\bTarea Import\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)),
			("Tarea Export", new Regex(@"\bTarea Export\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)),
			("Tarea Converter", new Regex(@"\bTarea Converter\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)),
			("Buy/Sell", new Regex(@"Buy\\?/Sell", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)),
			("Equity P&L", new Regex(@"\bEquity\s+P&L\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)),
			("Standard Error", new Regex(@"\bStandard\s+[Ee]rror\b", RegexOptions.CultureInvariant)),
			("Schemes", new Regex(@"\bSchemes\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)),
			("Logs", new Regex(@"\bLogs\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)),
			("Live 設定", new Regex(@"\bLive\s+設定\b", RegexOptions.CultureInvariant)),
			("Live 実行", new Regex(@"\bLive\s+実行", RegexOptions.CultureInvariant)),
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var tocPath = Path.Combine(_repoRoot, lang, "topics", "toc.yml");
			if (!File.Exists(tocPath))
				continue;

			foreach (var nameLine in EnumerateTocNameLines(tocPath))
			{
				foreach (var fragment in fragments)
				{
					if (!fragment.Pattern.IsMatch(nameLine.Name))
						continue;

					errors.Add($"{RelativeToRepo(tocPath)}:{nameLine.Line}: localized TOC keeps English navigation fragment '{fragment.Name}' in '{nameLine.Name}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void WesternLocalizedIndicatorTocNamesMatchLocalizedHeadings()
	{
		var errors = new List<string>();

		foreach (var lang in new[] { "de", "es", "pt", "ja" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);
			var tocPath = Path.Combine(langRoot, "topics", "toc.yml");
			if (!File.Exists(tocPath))
				continue;

			var entries = ReadTocEntries(tocPath, errors);
			if (entries is null)
				continue;

			var flatEntries = FlattenTocEntries(entries).ToArray();
			var nameLines = EnumerateTocNameLines(tocPath).ToArray();
			var count = Math.Min(flatEntries.Length, nameLines.Length);

			for (var i = 0; i < count; i++)
			{
				var href = NormalizeStructureUrl(flatEntries[i].Href);
				if (!href.StartsWith("api/indicators/list_of_indicators/", StringComparison.OrdinalIgnoreCase)
					|| !href.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
					continue;

				var target = Path.Combine(langRoot, "topics", href.Replace('/', Path.DirectorySeparatorChar));
				if (!File.Exists(target))
					continue;

				var heading = GetFirstHeadingText(target);
				if (heading.Length == 0 || nameLines[i].Name.Equals(heading, StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(tocPath)}:{nameLines[i].Line}: indicator TOC name '{nameLines[i].Name}' must match localized page heading '{heading}' for '{href}'.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void ChineseIndicatorTocDoesNotKeepEnglishDescriptiveAliases()
	{
		var errors = new List<string>();
		var tocPath = Path.Combine(_repoRoot, "zh", "topics", "toc.yml");
		var forbidden = new Regex(@"\b(?:Highest|Lowest|R-squared|MeanDev|Median|Momentum|OptimalTracking|Parabolic SAR|Stub|Std Dev|Stochastic %K|Sum|UltimateOsc)\b", RegexOptions.CultureInvariant);

		foreach (var nameLine in EnumerateTocNameLines(tocPath))
		{
			var match = forbidden.Match(nameLine.Name);
			if (!match.Success)
				continue;

			errors.Add($"{RelativeToRepo(tocPath)}:{nameLine.Line}: Chinese indicator TOC keeps English descriptive alias '{match.Value}' in '{nameLine.Name}'. Use the localized indicator heading or a deliberate acronym.");
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIndicatorOverviewDoesNotKeepEnglishInternalAliases()
	{
		var errors = new List<string>();
		var forbidden = new Regex(@"\[(?:Bollinger|Gator|MedPr|Stub|Peak|Sum)\]\(list_of_indicators/|\b(?:MeanDev|OptimalTracking|UltimateOsc)\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages().Where(lang => !lang.Equals("ru", StringComparison.OrdinalIgnoreCase)))
		{
			var file = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators.md");
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var match = forbidden.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator overview keeps English internal alias '{match.Value}'. Use the localized indicator label in visible overview text.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedTradingDocsDoNotKeepEnglishTrailingStopTerm()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\btrailing[-\s]stop\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages().Where(lang => !lang.Equals("ru", StringComparison.OrdinalIgnoreCase)))
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (match.Success)
						errors.Add($"{RelativeToRepo(file)}:{line}: localized trading prose keeps English trailing-stop term '{match.Value}'. Use localized stop wording.");
				}

				foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
				{
					var normalized = NormalizeCodeCommentForTranslationCheck(comment.Text);
					var match = pattern.Match(normalized);
					if (match.Success)
						errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized code comment keeps English trailing-stop term '{match.Value}'. Use localized stop wording.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanPortugueseDocsDoNotKeepEnglishDownloadTermInVisibleText()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bdownload(?:ed|s|ing)?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		void AddErrorIfMatched(string file, int line, string scope, string text)
		{
			if (text.Contains("Download Key File", StringComparison.Ordinal)
				|| text.Contains("products/download", StringComparison.OrdinalIgnoreCase)
				|| text.Contains("download_installer.png", StringComparison.OrdinalIgnoreCase)
				|| text.Contains("dotnet.microsoft.com/download", StringComparison.OrdinalIgnoreCase))
				return;

			var match = pattern.Match(text);
			if (!match.Success)
				return;

			errors.Add($"{RelativeToRepo(file)}:{line}: localized {scope} keeps English download term '{match.Value}'. Use localized download wording.");
		}

		foreach (var lang in new[] { "de", "pt" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
					AddErrorIfMatched(file, line, "visible text", text);

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
					AddErrorIfMatched(file, altText.Line, "image alt text", altText.Text);

				foreach (var comment in EnumerateCodeComments(markdown))
					AddErrorIfMatched(file, comment.Line, "code comment", NormalizeCodeCommentForTranslationCheck(comment.Text));

				foreach (var literal in EnumerateCodeStringLiterals(markdown))
					AddErrorIfMatched(file, literal.Line, "code string", NormalizeCodeStringLiteralForTranslationCheck(literal.Text));
			}

			foreach (var tocPath in Directory.EnumerateFiles(langRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var nameLine in EnumerateTocNameLines(tocPath))
					AddErrorIfMatched(tocPath, nameLine.Line, "TOC name", nameLine.Name);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotKeepEnglishQuotingTermsInVisibleText()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"(?<![A-Za-z0-9_./:\\-])(?:quoting|quotes?)(?![A-Za-z0-9_./:\\-])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		void AddErrorIfMatched(string file, int line, string scope, string text)
		{
			if (text.Contains("Quote API", StringComparison.Ordinal))
				return;

			var match = pattern.Match(text);
			if (!match.Success)
				return;

			errors.Add($"{RelativeToRepo(file)}:{line}: localized {scope} keeps English quoting/quote term '{match.Value}'. Use localized cotización/cotação wording.");
		}

		foreach (var lang in new[] { "es", "pt" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
					AddErrorIfMatched(file, line, "visible text", text);

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
					AddErrorIfMatched(file, altText.Line, "image alt text", altText.Text);

				foreach (var comment in EnumerateCodeComments(markdown))
					AddErrorIfMatched(file, comment.Line, "code comment", NormalizeCodeCommentForTranslationCheck(comment.Text));

				foreach (var literal in EnumerateCodeStringLiterals(markdown))
					AddErrorIfMatched(file, literal.Line, "code string", NormalizeCodeStringLiteralForTranslationCheck(literal.Text));
			}

			foreach (var tocPath in Directory.EnumerateFiles(langRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var nameLine in EnumerateTocNameLines(tocPath))
					AddErrorIfMatched(tocPath, nameLine.Line, "TOC name", nameLine.Name);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotKeepEnglishSlippageTermInVisibleText()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"(?<![A-Za-z0-9_./:\\-])slippage(?![A-Za-z0-9_./:\\-])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		void AddErrorIfMatched(string file, int line, string scope, string text)
		{
			if (text.Contains("`Slippage`", StringComparison.Ordinal)
				|| text.Contains("**Slippage**", StringComparison.Ordinal))
				return;

			var match = pattern.Match(text);
			if (!match.Success)
				return;

			errors.Add($"{RelativeToRepo(file)}:{line}: localized {scope} keeps English slippage term '{match.Value}'. Use localized deslizamiento/deslizamento wording.");
		}

		foreach (var lang in new[] { "es", "pt" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
					AddErrorIfMatched(file, line, "visible text", text);

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
					AddErrorIfMatched(file, altText.Line, "image alt text", altText.Text);

				foreach (var comment in EnumerateCodeComments(markdown))
					AddErrorIfMatched(file, comment.Line, "code comment", NormalizeCodeCommentForTranslationCheck(comment.Text));

				foreach (var literal in EnumerateCodeStringLiterals(markdown))
					AddErrorIfMatched(file, literal.Line, "code string", NormalizeCodeStringLiteralForTranslationCheck(literal.Text));
			}

			foreach (var tocPath in Directory.EnumerateFiles(langRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var nameLine in EnumerateTocNameLines(tocPath))
					AddErrorIfMatched(tocPath, nameLine.Line, "TOC name", nameLine.Name);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotKeepEnglishTradeTermInVisibleText()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"(?<![A-Za-z0-9_./:\\*-])trades?(?![A-Za-z0-9_./:\\-])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		void AddErrorIfMatched(string file, int line, string scope, string text)
		{
			if (text.Contains("E*TRADE", StringComparison.Ordinal)
				|| text.Contains(@"E\*TRADE", StringComparison.Ordinal)
				|| text.Contains("E TRADE", StringComparison.Ordinal)
				|| text.Contains("Security, Order, Trade, Portfolio", StringComparison.Ordinal)
				|| text.Contains("{ Trade =", StringComparison.Ordinal)
				|| text.Contains("`trade", StringComparison.Ordinal)
				|| text.Contains("trade.Price", StringComparison.Ordinal))
				return;

			var match = pattern.Match(text);
			if (!match.Success)
				return;

			errors.Add($"{RelativeToRepo(file)}:{line}: localized {scope} keeps English trade term '{match.Value}'. Use localized operación/negócio wording.");
		}

		foreach (var lang in new[] { "es", "pt" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
					AddErrorIfMatched(file, line, "visible text", text);

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
					AddErrorIfMatched(file, altText.Line, "image alt text", altText.Text);

				foreach (var comment in EnumerateCodeComments(markdown))
					AddErrorIfMatched(file, comment.Line, "code comment", NormalizeCodeCommentForTranslationCheck(comment.Text));

				foreach (var literal in EnumerateCodeStringLiterals(markdown))
					AddErrorIfMatched(file, literal.Line, "code string", NormalizeCodeStringLiteralForTranslationCheck(literal.Text));
			}

			foreach (var tocPath in Directory.EnumerateFiles(langRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var nameLine in EnumerateTocNameLines(tocPath))
					AddErrorIfMatched(tocPath, nameLine.Line, "TOC name", nameLine.Name);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDocsDoNotKeepEnglishTradingTermInVisibleText()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\btrading\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var markdownLinkTargetPattern = new Regex(@"\]\([^)]+\)", RegexOptions.CultureInvariant);
		var rawUrlPattern = new Regex(@"https?://\S+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		void AddErrorIfMatched(string file, int line, string scope, string text)
		{
			if (text.Contains("Fix Trading Community", StringComparison.Ordinal)
				|| text.Contains("FIX Trading Community", StringComparison.Ordinal))
				return;

			var normalized = markdownLinkTargetPattern.Replace(text, "]");
			normalized = rawUrlPattern.Replace(normalized, " ");

			var match = pattern.Match(normalized);
			if (!match.Success)
				return;

			errors.Add($"{RelativeToRepo(file)}:{line}: localized {scope} keeps English trading term '{match.Value}'. Use localized trading/market-activity wording.");
		}

		foreach (var lang in new[] { "de", "es", "pt", "ja", "zh" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
					AddErrorIfMatched(file, line, "visible text", text);

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
					AddErrorIfMatched(file, altText.Line, "image alt text", altText.Text);

				foreach (var comment in EnumerateCodeComments(markdown))
					AddErrorIfMatched(file, comment.Line, "code comment", NormalizeCodeCommentForTranslationCheck(comment.Text));

				foreach (var literal in EnumerateCodeStringLiterals(markdown))
					AddErrorIfMatched(file, literal.Line, "code string", NormalizeCodeStringLiteralForTranslationCheck(literal.Text));
			}

			foreach (var tocPath in Directory.EnumerateFiles(langRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var nameLine in EnumerateTocNameLines(tocPath))
					AddErrorIfMatched(tocPath, nameLine.Line, "TOC name", nameLine.Name);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishDocsDoNotKeepBrokenNegociacionAgreement()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:el|del|al|un)\s+negociación\b|\bdla\s+negociación\b|\bNegociación\s+permitido\b|\bnegociación\s+(?:algorítmico|exitoso)\b|\bnegociación\s+está\s+(?:completamente\s+)?(?:deshabilitado|permitido|prohibido)\b|\bpara\s+la\s+negociación\s+completo\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		void AddErrorIfMatched(string file, int line, string scope, string text)
		{
			var match = pattern.Match(text);
			if (!match.Success)
				return;

			errors.Add($"{RelativeToRepo(file)}:{line}: Spanish {scope} has broken negociación agreement '{match.Value}'.");
		}

		var langRoot = Path.Combine(_repoRoot, "es");

		foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var markdown = ReadAllText(file);

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				AddErrorIfMatched(file, line, "visible text", text);

			foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
				AddErrorIfMatched(file, altText.Line, "image alt text", altText.Text);

			foreach (var comment in EnumerateCodeComments(markdown))
				AddErrorIfMatched(file, comment.Line, "code comment", NormalizeCodeCommentForTranslationCheck(comment.Text));

			foreach (var literal in EnumerateCodeStringLiterals(markdown))
				AddErrorIfMatched(file, literal.Line, "code string", NormalizeCodeStringLiteralForTranslationCheck(literal.Text));
		}

		foreach (var tocPath in Directory.EnumerateFiles(langRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			foreach (var nameLine in EnumerateTocNameLines(tocPath))
				AddErrorIfMatched(tocPath, nameLine.Line, "TOC name", nameLine.Name);
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotKeepEnglishTraderTermInVisibleText()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\btraders?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var markdownLinkTargetPattern = new Regex(@"\]\([^)]+\)", RegexOptions.CultureInvariant);
		var rawUrlPattern = new Regex(@"https?://\S+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		void AddErrorIfMatched(string file, int line, string scope, string text)
		{
			if (text.Contains("IB Trader Workstation", StringComparison.Ordinal)
				|| text.Contains("Sterling Trader Pro", StringComparison.Ordinal)
				|| text.Contains("OEC Trader", StringComparison.Ordinal)
				|| text.Contains("Trader.RemotingRequired", StringComparison.Ordinal))
				return;

			var normalized = markdownLinkTargetPattern.Replace(text, "]");
			normalized = rawUrlPattern.Replace(normalized, " ");

			var match = pattern.Match(normalized);
			if (!match.Success)
				return;

			errors.Add($"{RelativeToRepo(file)}:{line}: localized {scope} keeps English trader term '{match.Value}'. Use localized operador wording.");
		}

		foreach (var lang in new[] { "es", "pt" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
					AddErrorIfMatched(file, line, "visible text", text);

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
					AddErrorIfMatched(file, altText.Line, "image alt text", altText.Text);

				foreach (var comment in EnumerateCodeComments(markdown))
					AddErrorIfMatched(file, comment.Line, "code comment", NormalizeCodeCommentForTranslationCheck(comment.Text));

				foreach (var literal in EnumerateCodeStringLiterals(markdown))
					AddErrorIfMatched(file, literal.Line, "code string", NormalizeCodeStringLiteralForTranslationCheck(literal.Text));
			}

			foreach (var tocPath in Directory.EnumerateFiles(langRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var nameLine in EnumerateTocNameLines(tocPath))
					AddErrorIfMatched(tocPath, nameLine.Line, "TOC name", nameLine.Name);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotKeepEnglishExchangeTermInVisibleText()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bexchanges?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var markdownLinkTargetPattern = new Regex(@"\]\([^)]+\)", RegexOptions.CultureInvariant);
		var rawUrlPattern = new Regex(@"https?://\S+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		void AddErrorIfMatched(string file, int line, string scope, string text)
		{
			if (text.Contains("LMAX Exchange", StringComparison.Ordinal)
				|| text.Contains("Investors Exchange", StringComparison.Ordinal)
				|| text.Contains("[Exchange](xref:", StringComparison.Ordinal)
				|| text.Contains("BusinessEntities.Exchange", StringComparison.Ordinal)
				|| text.Contains("exchange.csv", StringComparison.Ordinal)
				|| text.Contains("exchangeboard.csv", StringComparison.Ordinal)
				|| text.Contains("exchangeInfoProvider", StringComparison.Ordinal))
				return;

			var normalized = markdownLinkTargetPattern.Replace(text, "]");
			normalized = rawUrlPattern.Replace(normalized, " ");

			var match = pattern.Match(normalized);
			if (!match.Success)
				return;

			errors.Add($"{RelativeToRepo(file)}:{line}: localized {scope} keeps English exchange term '{match.Value}'. Use localized bolsa wording.");
		}

		foreach (var lang in new[] { "es", "pt" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
					AddErrorIfMatched(file, line, "visible text", text);

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
					AddErrorIfMatched(file, altText.Line, "image alt text", altText.Text);

				foreach (var comment in EnumerateCodeComments(markdown))
					AddErrorIfMatched(file, comment.Line, "code comment", NormalizeCodeCommentForTranslationCheck(comment.Text));

				foreach (var literal in EnumerateCodeStringLiterals(markdown))
					AddErrorIfMatched(file, literal.Line, "code string", NormalizeCodeStringLiteralForTranslationCheck(literal.Text));
			}

			foreach (var tocPath in Directory.EnumerateFiles(langRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var nameLine in EnumerateTocNameLines(tocPath))
					AddErrorIfMatched(tocPath, nameLine.Line, "TOC name", nameLine.Name);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotKeepBrokenBolsaAgreement()
	{
		var errors = new List<string>();
		var patterns = new Dictionary<string, Regex>
		{
			["es"] = new(@"\b(?:el|los|del|al|un|unos|este|ese|propio|propios|algunos|varios|muchos|todos|estos|esos)\s+bolsas?\b|\btodos\s+las\s+bolsas\b|\bbolsas?\s+(?:espec(?:i|\u00ED)fico|espec(?:i|\u00ED)ficos|cerrado|cerrados|utilizado|utilizados|modernos|internacional)\b|\bbolsa\s+en\s+el\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
			["pt"] = new(@"\b(?:o|os|do|dos|ao|aos|um|uns|este|esse|pr(?:o|\u00F3)prio|pr(?:o|\u00F3)prios|alguns|v(?:a|\u00E1)rios|muitos|todos|estes|esses)\s+bolsas?\b|\btodos\s+as\s+bolsas\b|\bbolsas?\s+(?:espec(?:i|\u00ED)fico|espec(?:i|\u00ED)ficos|utilizado|utilizados|fechado|fechados|modernos|internacional)\b|\bbolsa\s+no\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
		};

		void AddErrorIfMatched(string lang, string file, int line, string scope, string text)
		{
			var match = patterns[lang].Match(text);
			if (!match.Success)
				return;

			errors.Add($"{RelativeToRepo(file)}:{line}: {lang} {scope} has broken bolsa agreement '{match.Value}'.");
		}

		foreach (var lang in new[] { "es", "pt" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
					AddErrorIfMatched(lang, file, line, "visible text", text);

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
					AddErrorIfMatched(lang, file, altText.Line, "image alt text", altText.Text);

				foreach (var comment in EnumerateCodeComments(markdown))
					AddErrorIfMatched(lang, file, comment.Line, "code comment", NormalizeCodeCommentForTranslationCheck(comment.Text));

				foreach (var literal in EnumerateCodeStringLiterals(markdown))
					AddErrorIfMatched(lang, file, literal.Line, "code string", NormalizeCodeStringLiteralForTranslationCheck(literal.Text));
			}

			foreach (var tocPath in Directory.EnumerateFiles(langRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var nameLine in EnumerateTocNameLines(tocPath))
					AddErrorIfMatched(lang, tocPath, nameLine.Line, "TOC name", nameLine.Name);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotKeepEnglishBoardTermInVisibleText()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bboards?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var markdownLinkTargetPattern = new Regex(@"\]\([^)]+\)", RegexOptions.CultureInvariant);
		var rawUrlPattern = new Regex(@"https?://\S+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		void AddErrorIfMatched(string file, int line, string scope, string text)
		{
			if (text.Contains("ExchangeBoard", StringComparison.Ordinal)
				|| text.Contains("BoardCode", StringComparison.Ordinal)
				|| text.Contains("BoardLookup", StringComparison.Ordinal)
				|| text.Contains("BoardStates", StringComparison.Ordinal)
				|| text.Contains("BoardMessage", StringComparison.Ordinal)
				|| text.Contains("CommissionBoardCodeRule", StringComparison.Ordinal)
				|| text.Contains("CODE--BOARD", StringComparison.Ordinal)
				|| text.Contains("`Board`", StringComparison.Ordinal)
				|| text.Contains("\"Board\"", StringComparison.Ordinal)
				|| text.Equals("Board", StringComparison.Ordinal))
				return;

			var normalized = markdownLinkTargetPattern.Replace(text, "]");
			normalized = rawUrlPattern.Replace(normalized, " ");

			var match = pattern.Match(normalized);
			if (!match.Success)
				return;

			errors.Add($"{RelativeToRepo(file)}:{line}: localized {scope} keeps English board term '{match.Value}'. Use localized mercado wording.");
		}

		foreach (var lang in new[] { "es", "pt" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
					AddErrorIfMatched(file, line, "visible text", text);

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
					AddErrorIfMatched(file, altText.Line, "image alt text", altText.Text);

				foreach (var comment in EnumerateCodeComments(markdown))
					AddErrorIfMatched(file, comment.Line, "code comment", NormalizeCodeCommentForTranslationCheck(comment.Text));

				foreach (var literal in EnumerateCodeStringLiterals(markdown))
					AddErrorIfMatched(file, literal.Line, "code string", NormalizeCodeStringLiteralForTranslationCheck(literal.Text));
			}

			foreach (var tocPath in Directory.EnumerateFiles(langRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var nameLine in EnumerateTocNameLines(tocPath))
					AddErrorIfMatched(tocPath, nameLine.Line, "TOC name", nameLine.Name);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotTranslateBoardApiIdentifier()
	{
		var errors = new List<string>();
		var assignmentPattern = new Regex(@"\bBoard\s*=\s*(?:mercado|mercados)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var signaturePattern = new Regex(@"ExchangeBoard[^\r\n]*\)\s+(?:mercado|mercados)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in new[] { "es", "pt" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var lines = ReadAllText(file).Replace("\r\n", "\n").Split('\n');

				for (var i = 0; i < lines.Length; i++)
				{
					var line = lines[i];
					var match = assignmentPattern.Match(line);
					if (!match.Success)
						match = signaturePattern.Match(line);

					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{i + 1}: board API identifier appears translated as '{match.Value}'. Keep API names and parameter names in code/signatures.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotKeepEnglishStopOrderTermInVisibleText()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bstop\s+orders?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var markdownLinkTargetPattern = new Regex(@"\]\([^)]+\)", RegexOptions.CultureInvariant);
		var rawUrlPattern = new Regex(@"https?://\S+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		void AddErrorIfMatched(string file, int line, string scope, string text)
		{
			var normalized = markdownLinkTargetPattern.Replace(text, "]");
			normalized = rawUrlPattern.Replace(normalized, " ");

			var match = pattern.Match(normalized);
			if (!match.Success)
				return;

			errors.Add($"{RelativeToRepo(file)}:{line}: localized {scope} keeps English stop order term '{match.Value}'. Use localized orden/ordem stop wording.");
		}

		foreach (var lang in new[] { "es", "pt" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
					AddErrorIfMatched(file, line, "visible text", text);

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
					AddErrorIfMatched(file, altText.Line, "image alt text", altText.Text);

				foreach (var comment in EnumerateCodeComments(markdown))
					AddErrorIfMatched(file, comment.Line, "code comment", NormalizeCodeCommentForTranslationCheck(comment.Text));

				foreach (var literal in EnumerateCodeStringLiterals(markdown))
					AddErrorIfMatched(file, literal.Line, "code string", NormalizeCodeStringLiteralForTranslationCheck(literal.Text));
			}

			foreach (var tocPath in Directory.EnumerateFiles(langRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var nameLine in EnumerateTocNameLines(tocPath))
					AddErrorIfMatched(tocPath, nameLine.Line, "TOC name", nameLine.Name);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotKeepEnglishSmileTermInVisibleText()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bsmiles?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var markdownLinkTargetPattern = new Regex(@"\]\([^)]+\)", RegexOptions.CultureInvariant);
		var rawUrlPattern = new Regex(@"https?://\S+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		void AddErrorIfMatched(string file, int line, string scope, string text)
		{
			var normalized = markdownLinkTargetPattern.Replace(text, "]");
			normalized = rawUrlPattern.Replace(normalized, " ");

			var match = pattern.Match(normalized);
			if (!match.Success)
				return;

			errors.Add($"{RelativeToRepo(file)}:{line}: localized {scope} keeps English volatility-smile term '{match.Value}'. Use localized sonrisa/sorriso wording.");
		}

		foreach (var lang in new[] { "es", "pt" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
					AddErrorIfMatched(file, line, "visible text", text);

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
					AddErrorIfMatched(file, altText.Line, "image alt text", altText.Text);
			}

			foreach (var tocPath in Directory.EnumerateFiles(langRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var nameLine in EnumerateTocNameLines(tocPath))
					AddErrorIfMatched(tocPath, nameLine.Line, "TOC name", nameLine.Name);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotKeepEnglishDumpModePhraseInVisibleText()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:modo\s+dump|dump\s+mode|m[eé]todo\s+de\s+dump)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var markdownLinkTargetPattern = new Regex(@"\]\([^)]+\)", RegexOptions.CultureInvariant);
		var rawUrlPattern = new Regex(@"https?://\S+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		void AddErrorIfMatched(string file, int line, string scope, string text)
		{
			var normalized = markdownLinkTargetPattern.Replace(text, "]");
			normalized = rawUrlPattern.Replace(normalized, " ");

			var match = pattern.Match(normalized);
			if (!match.Success)
				return;

			errors.Add($"{RelativeToRepo(file)}:{line}: localized {scope} keeps English dump-mode phrase '{match.Value}'. Use localized volcado/despejo wording while preserving API identifiers.");
		}

		foreach (var lang in new[] { "es", "pt" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
					AddErrorIfMatched(file, line, "visible text", text);

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
					AddErrorIfMatched(file, altText.Line, "image alt text", altText.Text);
			}

			foreach (var tocPath in Directory.EnumerateFiles(langRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var nameLine in EnumerateTocNameLines(tocPath))
					AddErrorIfMatched(tocPath, nameLine.Line, "TOC name", nameLine.Name);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotKeepEnglishFeedTermInVisibleText()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bfeeds?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var iqFeedPattern = new Regex(@"\bIQ\s+Feed(?:\s+Client)?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var markdownLinkTargetPattern = new Regex(@"\]\([^)]+\)", RegexOptions.CultureInvariant);
		var rawUrlPattern = new Regex(@"https?://\S+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		void AddErrorIfMatched(string file, int line, string scope, string text)
		{
			var normalized = markdownLinkTargetPattern.Replace(text, "]");
			normalized = rawUrlPattern.Replace(normalized, " ");
			normalized = iqFeedPattern.Replace(normalized, "IQFeed");

			var match = pattern.Match(normalized);
			if (!match.Success)
				return;

			errors.Add($"{RelativeToRepo(file)}:{line}: localized {scope} keeps English feed term '{match.Value}'. Use localized fuente/fonte/flujo/fluxo wording while preserving product names.");
		}

		foreach (var lang in new[] { "es", "pt" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
					AddErrorIfMatched(file, line, "visible text", text);

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
					AddErrorIfMatched(file, altText.Line, "image alt text", altText.Text);
			}

			foreach (var tocPath in Directory.EnumerateFiles(langRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var nameLine in EnumerateTocNameLines(tocPath))
					AddErrorIfMatched(tocPath, nameLine.Line, "TOC name", nameLine.Name);
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotKeepEnglishOrderLogPhraseInVisibleText()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:log de (?:[óo]rdenes|ordens)|elemento de log|item do log)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var markdownLinkTargetPattern = new Regex(@"\]\([^)]+\)", RegexOptions.CultureInvariant);
		var rawUrlPattern = new Regex(@"https?://\S+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		void AddErrorIfMatched(string file, int line, string scope, string text)
		{
			var normalized = markdownLinkTargetPattern.Replace(text, "]");
			normalized = rawUrlPattern.Replace(normalized, " ");

			var match = pattern.Match(normalized);
			if (!match.Success)
				return;

			errors.Add($"{RelativeToRepo(file)}:{line}: localized {scope} keeps English order-log phrase '{match.Value}'. Use localized registro/registo de ordenes/ordens wording while preserving OrderLog API identifiers.");
		}

		foreach (var lang in new[] { "es", "pt" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
					AddErrorIfMatched(file, line, "visible text", text);

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
					AddErrorIfMatched(file, altText.Line, "image alt text", altText.Text);

				foreach (var comment in EnumerateCodeComments(markdown))
					AddErrorIfMatched(file, comment.Line, "code comment", NormalizeCodeCommentForTranslationCheck(comment.Text));
			}

			foreach (var tocPath in Directory.EnumerateFiles(langRoot, "toc.yml", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var nameLine in EnumerateTocNameLines(tocPath))
					AddErrorIfMatched(tocPath, nameLine.Line, "TOC name", nameLine.Name);
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
	public void StockSharpSiteLinksUseLocalizedRoutes()
	{
		var errors = new List<string>();
		var siteLinkPattern = new Regex(@"https?:(?://|\\/\\/)(?:www\.)?stocksharp\.(?<domain>ru|com)(?<suffix>[^\s\)\]\}>""'<]*)?", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var checkedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			".json",
			".md",
			".yml",
			".yaml",
		};

		foreach (var lang in GetContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.*", SearchOption.AllDirectories)
				.Where(file => checkedExtensions.Contains(Path.GetExtension(file)))
				.Order(StringComparer.OrdinalIgnoreCase))
			{
				var text = ReadAllText(file);
				var matches = siteLinkPattern.Matches(text);
				if (matches.Count == 0)
					continue;

				var lineStarts = GetLineStarts(text);

				foreach (Match match in matches)
				{
					var location = $"{RelativeToRepo(file)}:{GetLineNumber(lineStarts, match.Index)}";
					var domain = match.Groups["domain"].Value;

					if (domain.Equals("ru", StringComparison.OrdinalIgnoreCase))
					{
						errors.Add($"{location}: StockSharp site link '{match.Value}' uses obsolete stocksharp.ru; use https://stocksharp.com/{lang} for the {lang} docs.");
						continue;
					}

					var suffix = NormalizeStockSharpSiteSuffix(match.Groups["suffix"].Value);
					if (HasStockSharpSiteLanguagePrefix(suffix, lang))
						continue;

					var actualLang = GetStockSharpSiteLanguagePrefix(suffix);
					if (actualLang.Length > 0)
						errors.Add($"{location}: StockSharp site link '{match.Value}' uses '/{actualLang}' but this is the {lang} docs.");
					else
						errors.Add($"{location}: StockSharp site link '{match.Value}' must start with https://stocksharp.com/{lang} for the {lang} docs.");
				}
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
	public void LocalizedIndexPagesDoNotKeepEnglishLogTerms()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:order-logs|logs)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, "index.md");
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var match = pattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: localized index page keeps English log term '{match.Value}'. Localize visible product summary text.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanHydraFirstStartDoesNotKeepEnglishUtilitiesLinkLabel()
	{
		var errors = new List<string>();
		var file = Path.Combine(_repoRoot, "de", "topics", "hydra", "first_start.md");

		foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
		{
			if (!text.Contains("[Utilities](tasks.md)", StringComparison.Ordinal))
				continue;

			errors.Add($"{RelativeToRepo(file)}:{line}: German Hydra first-start documentation keeps the English Utilities link label. Use the localized section title.");
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void HydraIndicatorViewDocsDoNotKeepColonArtifactsOrEnglishFragments()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "hydra", "working_with_data", "view_and_export", "indicators.md");
		var colonArtifact = new Regex(@":\s+:", RegexOptions.CultureInvariant);

		foreach (var lang in GetContentLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);
			if (!File.Exists(file))
				continue;

			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				if (!colonArtifact.IsMatch(text))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Hydra indicator-view documentation keeps a duplicated colon artifact.");
			}
		}

		var germanFile = Path.Combine(_repoRoot, "de", relative);
		foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(germanFile)))
		{
			if (!text.Contains("[indicator](../../../api/indicators/list_of_indicators.md)", StringComparison.Ordinal))
				continue;

			errors.Add($"{RelativeToRepo(germanFile)}:{line}: German Hydra indicator-view documentation keeps the English indicator link label.");
		}

		var russianFile = Path.Combine(_repoRoot, "ru", relative);
		foreach (var altText in EnumerateMarkdownImageAltTexts(ReadAllText(russianFile)))
		{
			if (!altText.Text.Contains(" view", StringComparison.Ordinal))
				continue;

			errors.Add($"{RelativeToRepo(russianFile)}:{altText.Line}: Russian Hydra indicator-view image alt text keeps the English word 'view'.");
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void MarkdownBangBangCalloutsUseBalancedSpacing()
	{
		var errors = new List<string>();
		var badPattern = new Regex(@"!!\s*[^\s!][^!\r\n]*[^\s!]!!", RegexOptions.CultureInvariant);

		foreach (var lang in GetContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var line = 1;
				using var reader = new StringReader(ReadAllText(file));

				for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
				{
					var match = badPattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: bang-bang emphasis marker is missing whitespace before the closing marker: '{match.Value}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void JapaneseDesignerCodeDocsDoNotKeepEnglishAlgoTradingFolderLinkLabel()
	{
		var errors = new List<string>();
		var badLabel = "[AlgoTrading API folder]";
		var langRoot = Path.Combine(_repoRoot, "ja");

		foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				if (!text.Contains(badLabel, StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Japanese documentation keeps the English AlgoTrading API folder link label.");
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
		var credentialPattern = new Regex(@"^\s*[-*]\s+\*\*[^*]+\*\*\s*(?:-|—|–|:|：)\s*(?:Login|Password)[.。．]\s*$|\b(?:Login|Password) adicional\b|\bZusätzliches Login\.", RegexOptions.CultureInvariant);

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
	public void LocalizedConnectorDocsDoNotKeepKnownEnglishCredentialPhrases()
	{
		var errors = new List<string>();
		var patterns = new[]
		{
			new Regex(@"\bLogin\b.*\bPassword\b|\bPassword\b.*\bLogin\b", RegexOptions.CultureInvariant),
			new Regex(@"\blogin\s+(?:e|y)\s+password\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
			new Regex(@"\bPassword\s+do\b", RegexOptions.CultureInvariant),
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var connectorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "connectors");

			if (!Directory.Exists(connectorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(connectorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					foreach (var pattern in patterns)
					{
						var match = pattern.Match(text);
						if (!match.Success)
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: connector documentation keeps untranslated English credential phrase '{match.Value}'.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDesignerDocsDoNotKeepKnownEnglishUiLabels()
	{
		var errors = new List<string>();
		var checks = new[]
		{
			(
				RelativePath: "topics/designer/strategies/using_visual_designer/diagram_panel.md",
				Fragments: new[] { "*basic settings*", "*advanced settings*", "Panel Palette", "Painel Palette", "Panel Designer", "Palette パネル", "Palette 面板" }
			),
			(
				RelativePath: "topics/designer/user_interface/risk_management.md",
				Fragments: new[] { "Risk Rules" }
			),
			(
				RelativePath: "topics/designer/optimization/portfolio_optimization.md",
				Fragments: new[] { "[Optimization]" }
			),
			(
				RelativePath: "topics/designer/live_execution/getting_started.md",
				Fragments: new[] { "Live trade", "Live-trade", "carpeta Trade", "pasta Trade", "Ordner Trade" }
			),
			(
				RelativePath: "topics/designer/optimization/brute_force.md",
				Fragments: new[] { "Optimization +" }
			),
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			foreach (var check in checks)
			{
				var file = Path.Combine(_repoRoot, lang, check.RelativePath.Replace('/', Path.DirectorySeparatorChar));

				if (!File.Exists(file))
					continue;

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					foreach (var fragment in check.Fragments)
					{
						if (!text.Contains(fragment, StringComparison.Ordinal))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: localized Designer documentation keeps English UI label '{fragment}'.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedApiDocsDoNotKeepKnownEnglishShortLabels()
	{
		var errors = new List<string>();
		var checks = new[]
		{
			(
				RelativePath: "topics/api/options/graphic_components.md",
				Fragments: new[] { "Graphic components" }
			),
			(
				RelativePath: "topics/api/market_data/getting_news_data.md",
				Fragments: new[] { "Graphical Components" }
			),
			(
				RelativePath: "topics/api/market_data_storage.md",
				Fragments: new[] { "Remote Storage" }
			),
			(
				RelativePath: "topics/api/market_data_storage/remote.md",
				Fragments: new[] { "Remote Storage" }
			),
			(
				RelativePath: "topics/api/market_data_storage/drives.md",
				Fragments: new[] { "Remote Storage" }
			),
			(
				RelativePath: "topics/api/setup.md",
				Fragments: new[] { "Exchange/Broker" }
			),
			(
				RelativePath: "topics/api/strategies/trading_modes.md",
				Fragments: new[] { "\\ required" }
			),
			(
				RelativePath: "topics/api/graphical_user_interface/charts/candle_chart.md",
				Fragments: new[] { "кнопки **Connect**", "кнопки **ShowChart**" }
			),
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			foreach (var check in checks)
			{
				var file = Path.Combine(_repoRoot, lang, check.RelativePath.Replace('/', Path.DirectorySeparatorChar));

				if (!File.Exists(file))
					continue;

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					foreach (var fragment in check.Fragments)
					{
						if (!text.Contains(fragment, StringComparison.Ordinal))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: localized API documentation keeps English label '{fragment}'.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanSpanishPortugueseDocsDoNotKeepEnglishMarketDataStoragePhrases()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:remote storage|storage registry)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in new[] { "de", "es", "pt" })
		{
			var indexFile = Path.Combine(_repoRoot, lang, "topics", "api", "market_data_storage.md");

			if (File.Exists(indexFile))
				AddLocalizedEnglishTermErrors(indexFile, ReadAllText(indexFile), pattern, "market data storage phrase", errors);

			var storageRoot = Path.Combine(_repoRoot, lang, "topics", "api", "market_data_storage");

			if (Directory.Exists(storageRoot))
			{
				foreach (var file in Directory.EnumerateFiles(storageRoot, "*.md", SearchOption.TopDirectoryOnly).Order(StringComparer.OrdinalIgnoreCase))
					AddLocalizedEnglishTermErrors(file, ReadAllText(file), pattern, "market data storage phrase", errors);
			}

			var tocPath = Path.Combine(_repoRoot, lang, "topics", "toc.yml");

			foreach (var nameLine in EnumerateTocNameLines(tocPath))
			{
				if (pattern.IsMatch(nameLine.Name))
					errors.Add($"{RelativeToRepo(tocPath)}:{nameLine.Line}: localized TOC name keeps English market data storage phrase '{nameLine.Name}'.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedApiDocsDoNotKeepEnglishStockSharpRepositoryLabels()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bStockSharp (?:Samples|repository)\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var root = Path.Combine(_repoRoot, lang, "topics", "api");

			if (!Directory.Exists(root))
				continue;

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
				AddLocalizedEnglishTermErrors(file, ReadAllText(file), pattern, "StockSharp repository label", errors);
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedApiDocsDoNotKeepEnglishSamplesFolderLinkLabels()
	{
		var errors = new List<string>();

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var root = Path.Combine(_repoRoot, lang, "topics", "api");

			if (!Directory.Exists(root))
				continue;

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var label in EnumerateMarkdownLinkLabels(ReadAllText(file)))
				{
					if ((label.Text.Equals("Samples", StringComparison.Ordinal) || label.Text.Equals("Samples/", StringComparison.Ordinal))
						&& label.Url.Contains("/Samples", StringComparison.OrdinalIgnoreCase))
					{
						errors.Add($"{RelativeToRepo(file)}:{label.Line}: localized API documentation keeps English Samples folder link label. Localize the visible link text while preserving the URL.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedHydraServerDocsDoNotKeepEnglishWindowsServicePhrase()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bWindows service\b|\bWindows Service\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var file = Path.Combine(_repoRoot, lang, "topics", "hydra_server.md");

			if (File.Exists(file))
				AddLocalizedEnglishTermErrors(file, ReadAllText(file), pattern, "Windows service", errors);
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCommonDocsDoNotKeepEnglishTelegramChatLinkLabel()
	{
		var errors = new List<string>();

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var file = Path.Combine(_repoRoot, lang, "topics", "common", "reference_materials.md");

			if (!File.Exists(file))
				continue;

			foreach (var label in EnumerateMarkdownLinkLabels(ReadAllText(file)))
			{
				if (label.Text.Equals("Chat", StringComparison.Ordinal)
					&& label.Url.Contains("t.me/stocksharpchat", StringComparison.OrdinalIgnoreCase))
				{
					errors.Add($"{RelativeToRepo(file)}:{label.Line}: localized common reference keeps English Telegram chat link label.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedInstallerDocsDoNotKeepEnglishStoreLinkLabel()
	{
		var errors = new List<string>();

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var file = Path.Combine(_repoRoot, lang, "topics", "installer", "console.md");

			if (!File.Exists(file))
				continue;

			foreach (var label in EnumerateMarkdownLinkLabels(ReadAllText(file)))
			{
				if (label.Text.Equals("Store", StringComparison.Ordinal)
					&& NormalizeStockSharpSiteLanguageRouteForStructure(label.Url).Equals("https://stocksharp.com/{lang}/store/", StringComparison.OrdinalIgnoreCase))
				{
					errors.Add($"{RelativeToRepo(file)}:{label.Line}: localized installer documentation keeps English Store link label.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedRunnerDocsDoNotKeepEnglishServerModeLinkLabel()
	{
		var errors = new List<string>();

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var file = Path.Combine(_repoRoot, lang, "topics", "runner", "command_line.md");

			if (!File.Exists(file))
				continue;

			foreach (var label in EnumerateMarkdownLinkLabels(ReadAllText(file)))
			{
				if (label.Text.Equals("server", StringComparison.Ordinal)
					&& label.Url.Equals("../hydra_server.md", StringComparison.OrdinalIgnoreCase))
				{
					errors.Add($"{RelativeToRepo(file)}:{label.Line}: localized Runner command-line documentation keeps English server-mode link label.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDesignerDocsDoNotKeepEnglishFormedIndicatorLinkLabel()
	{
		var errors = new List<string>();

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var root = Path.Combine(_repoRoot, lang, "topics", "designer");

			if (!Directory.Exists(root))
				continue;

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var label in EnumerateMarkdownLinkLabels(ReadAllText(file)))
				{
					if (label.Text.Equals("formed", StringComparison.Ordinal)
						&& label.Url.EndsWith("/api/indicators.md", StringComparison.OrdinalIgnoreCase))
					{
						errors.Add($"{RelativeToRepo(file)}:{label.Line}: localized Designer documentation keeps English formed-indicator link label.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDocsUseCanonicalHydraServerCasing()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bHydra server\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var root = Path.Combine(_repoRoot, lang, "topics");

			if (!Directory.Exists(root))
				continue;

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
				AddLocalizedEnglishTermErrors(file, ReadAllText(file), pattern, "Hydra Server", errors);
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
	public void DesignerPnlStrategyDocsDoNotKeepRemoteManagerPlaceholder()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "designer", "strategies", "using_visual_designer", "elements", "common", "pnl_strategy.md");

		foreach (var lang in GetContentLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);

			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				if (!text.Contains("RemoteManager", StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Strategy P/L element documentation keeps an unrelated RemoteManager placeholder.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void IqFeedLevel1DataDescriptionDoesNotUseTranslationTerminology()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "api", "connectors", "stock_market", "iqfeed", "graphical_configuration_iqfeed.md");
		var badPattern = new Regex(@"have to be translated|\u8F6C\u6362\u4E3A\s+Level1", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetContentLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);

			if (!File.Exists(file))
				continue;

			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				if (!badPattern.IsMatch(text))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: IQFeed Level1 data description uses translation/conversion terminology instead of transmission terminology.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedReportingSampleParameterLiteralsAreLocalized()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "api", "strategies", "reporting.md");
		var badPattern = new Regex(@"AddParameter\(""Timeframe"",\s*""5 minutes""\)|AddStatisticParameter\(""Sharpe Ratio""", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);

			if (!File.Exists(file))
				continue;

			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				if (!badPattern.IsMatch(text))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: reporting sample keeps an English display literal. Localize report parameter display names and values.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedSetupDocsDoNotKeepEnglishNuGetPlaceholders()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "api", "setup.md");
		var badPattern = new Regex(@"\bYOUR_(?:TOKEN|LOGIN|PASSWORD)\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);

			if (!File.Exists(file))
				continue;

			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				if (!badPattern.IsMatch(text))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: setup documentation keeps an English NuGet placeholder. Localize example placeholder names.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCodePlaceholdersDoNotKeepEnglishYourPrompts()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"<Your [^>]+>", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var line = 1;
				using var reader = new StringReader(ReadAllText(file));

				for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized code placeholder keeps English prompt '{match.Value}'. Localize user-provided placeholder text.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedRestClientSamplesDoNotKeepEnglishUserAgentPlaceholder()
	{
		var errors = new List<string>();
		var relativePath = Path.Combine("topics", "api", "connectors", "creating_own_connector", "best_practices", "rest_client.md");

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath);
			if (!File.Exists(file))
				continue;

			foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
			{
				if (!literal.Text.Equals("YourAppName/1.0", StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{literal.Line}: localized REST client sample keeps the English User-Agent placeholder 'YourAppName/1.0'.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedHydraCustomCandleInstructionsDoNotKeepEnglishStartButtonText()
	{
		var errors = new List<string>();
		var languages = new[] { "es", "pt", "ja", "zh" };
		var relative = Path.Combine("topics", "hydra", "prepare_for_download", "custom_candles.md");
		var badPattern = new Regex(@"\bStart\b|\bstart\s+を", RegexOptions.CultureInvariant);

		foreach (var lang in languages)
		{
			var file = Path.Combine(_repoRoot, lang, relative);

			if (!File.Exists(file))
				continue;

			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				if (!badPattern.IsMatch(text))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Hydra custom candle instruction keeps English Start button text. Localize the visible action text.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedHydraAnalyticsDocsDoNotKeepEnglishScriptNames()
	{
		var errors = new List<string>();
		var relativePaths = new[]
		{
			Path.Combine("topics", "hydra", "analytics", "examples", "normalization.md"),
			Path.Combine("topics", "hydra", "analytics", "examples", "largest_candles.md"),
		};
		var pattern = new Regex(@"\b(?:Closing Price Normalization|Largest Candles)\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(_repoRoot, lang, relativePath);
				if (!File.Exists(file))
					continue;

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized Hydra analytics docs keep English script name '{match.Value}'. Use the localized script title in visible text.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedHydraAnalyticsUiDocsDoNotKeepEnglishPanelLabels()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "hydra", "analytics", "running_a_script.md");
		var pattern = new Regex(@"\b(?:Navigation Tree|Code Window|Parameter Panel|Error List)\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var match = pattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: localized Hydra analytics UI docs keep English panel label '{match.Value}'. Localize the visible panel name.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedHydraAnalyticsDocsDoNotKeepEnglishAnalyticsFeatureLabels()
	{
		var errors = new List<string>();
		var relativePaths = new[]
		{
			Path.Combine("topics", "hydra", "analytics.md"),
			Path.Combine("topics", "hydra", "analytics", "running_a_script.md"),
			Path.Combine("topics", "hydra", "analytics", "create_own_script.md"),
		};
		var pattern = new Regex(
			@"\*\*Analytics\*\*|Analytics-Funktion|Analytics-Skript|Analytics\s+機能|Analytics\s+功能|script(?:s)?\s+de\s+analytics|fun(?:ç|c)ão\s+Analytics|función\s+Analytics",
			RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages().Where(lang => !lang.Equals("ru", StringComparison.Ordinal)))
		{
			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(_repoRoot, lang, relativePath);
				if (!File.Exists(file))
					continue;

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized Hydra analytics documentation keeps English Analytics feature label '{match.Value}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void ChineseLiveDocsDoNotKeepEnglishLiveModeLabel()
	{
		var errors = new List<string>();
		var relativePaths = new[]
		{
			Path.Combine("topics", "designer", "live_execution", "live_execution_sample.md"),
			Path.Combine("topics", "telegram_services", "control_panel.md"),
		};
		var pattern = new Regex(@"\bLive\s+模式", RegexOptions.CultureInvariant);

		foreach (var relativePath in relativePaths)
		{
			var file = Path.Combine(_repoRoot, "zh", relativePath);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var match = pattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Chinese live documentation keeps English Live mode label '{match.Value}'. Use the localized live-trading term.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void JapaneseApiDocsDoNotKeepKnownEnglishUiPhrases()
	{
		var errors = new List<string>();
		var checks = new[]
		{
			(
				RelativePath: Path.Combine("topics", "api", "graphical_user_interface", "notification_settings_window.md"),
				Pattern: new Regex(@"\bDesigner Logs window\b", RegexOptions.CultureInvariant),
				Description: "notification log-window label"
			),
			(
				RelativePath: Path.Combine("topics", "api", "strategies", "quoting.md"),
				Pattern: new Regex(@"\bQuoting Behavior\b", RegexOptions.CultureInvariant),
				Description: "quoting behavior heading"
			),
		};

		foreach (var check in checks)
		{
			var file = Path.Combine(_repoRoot, "ja", check.RelativePath);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var match = check.Pattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Japanese API docs keep English {check.Description} '{match.Value}'. Localize the visible UI phrase.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedAiDevelopmentDocsDoNotKeepEnglishPromptFragments()
	{
		var errors = new List<string>();
		var aiRootRelative = Path.Combine("topics", "api", "ai_development");
		var badPattern = new Regex(
			@"Framework:\s+StockSharp|subscribe to channel|Interval mapping|Parse bids/asks into|Parse into ExecutionMessage|Order registration|Order cancellation|Portfolio retrieval|WebSocket for order updates|Parse order status updates|Return ExecutionMessage with|with params:|edge cases|demo mode",
			RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var aiRoot = Path.Combine(_repoRoot, lang, aiRootRelative);

			if (!Directory.Exists(aiRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(aiRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var line = 1;
				using var reader = new StringReader(ReadAllText(file));

				for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
				{
					var match = badPattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: AI development documentation keeps English prompt fragment '{match.Value}'. Localize visible prompt text.");
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
		var relativePaths = new[]
		{
			"topics/designer/strategies/using_visual_designer/elements.md",
			"topics/designer/strategies/using_visual_designer/lines.md",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			foreach (var relativePath in relativePaths)
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
	public void PortugueseMarkdownDoesNotKeepEnglishCandleTerms()
	{
		var errors = new List<string>();
		var langRoot = Path.Combine(_repoRoot, "pt");
		var pattern = new Regex(@"(?<![A-Za-z0-9_./\\])candles?(?![A-Za-z0-9_./\\])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var relative = Path.GetRelativePath(langRoot, file).Replace('\\', '/');
			var markdown = ReadAllText(file);

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
			{
				var normalized = NormalizeMarkdownTextForLocalizedLoggingTermCheck(text);

				if (IsAllowedLocalizedCandleTermLine(relative, normalized))
					continue;

				var match = pattern.Match(normalized);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Portuguese markdown keeps English candle term '{match.Value}'. Use 'vela'/'velas' in visible prose.");
			}

			foreach (var comment in EnumerateCodeComments(markdown))
			{
				var normalized = NormalizeMarkdownTextForLocalizedLoggingTermCheck(NormalizeCodeCommentForTranslationCheck(comment.Text));

				if (IsAllowedLocalizedCandleTermLine(relative, normalized))
					continue;

				var match = pattern.Match(normalized);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{comment.Line}: Portuguese code comment keeps English candle term '{match.Value}'. Use 'vela'/'velas' in explanatory comments.");
			}

			foreach (var literal in EnumerateCodeStringLiterals(markdown))
			{
				var normalized = NormalizeMarkdownTextForLocalizedLoggingTermCheck(NormalizeCodeStringLiteralForTranslationCheck(literal.Text));
				normalized = Regex.Replace(normalized, @"(?::param|@param)\s+[A-Za-z_]\w*\s*:?", " ", RegexOptions.CultureInvariant);

				if (IsAllowedLocalizedCandleTermLine(relative, normalized))
					continue;

				var match = pattern.Match(normalized);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{literal.Line}: Portuguese code string keeps English candle term '{match.Value}'. Use 'vela'/'velas' in user-facing strings.");
			}
		}

		var tocPath = Path.Combine(langRoot, "topics", "toc.yml");
		foreach (var nameLine in EnumerateTocNameLines(tocPath))
		{
			var match = pattern.Match(nameLine.Name);
			if (!match.Success)
				continue;

			errors.Add($"{RelativeToRepo(tocPath)}:{nameLine.Line}: Portuguese TOC keeps English candle term '{match.Value}' in '{nameLine.Name}'.");
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanMarkdownDoesNotKeepEnglishCandleTerms()
	{
		var errors = new List<string>();
		var langRoot = Path.Combine(_repoRoot, "de");
		var pattern = new Regex(@"(?<![A-Za-z0-9_./\\])candles?(?![A-Za-z0-9_./\\])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var relative = Path.GetRelativePath(langRoot, file).Replace('\\', '/');

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var normalized = NormalizeMarkdownTextForLocalizedLoggingTermCheck(text);

				if (IsAllowedLocalizedCandleTermLine(relative, normalized))
					continue;

				var match = pattern.Match(normalized);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: German markdown keeps English candle term '{match.Value}'. Use 'Kerze'/'Kerzen' in visible prose.");
			}

			foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
			{
				var normalized = NormalizeMarkdownTextForLocalizedLoggingTermCheck(NormalizeCodeCommentForTranslationCheck(comment.Text));

				if (IsAllowedLocalizedCandleTermLine(relative, normalized))
					continue;

				var match = pattern.Match(normalized);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{comment.Line}: German code comment keeps English candle term '{match.Value}'. Use 'Kerze'/'Kerzen' in explanatory comments.");
			}
		}

		var tocPath = Path.Combine(langRoot, "topics", "toc.yml");
		foreach (var nameLine in EnumerateTocNameLines(tocPath))
		{
			var match = pattern.Match(nameLine.Name);
			if (!match.Success)
				continue;

			errors.Add($"{RelativeToRepo(tocPath)}:{nameLine.Line}: German TOC keeps English candle term '{match.Value}' in '{nameLine.Name}'.");
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownDoesNotKeepEnglishCandleTerms()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"(?<![A-Za-z0-9_./\\])candles?(?![A-Za-z0-9_./\\])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var localizedTerms = new Dictionary<string, (string Singular, string Plural)>(StringComparer.OrdinalIgnoreCase)
		{
			["de"] = ("Kerze", "Kerzen"),
			["es"] = ("vela", "velas"),
			["ja"] = ("ローソク足", "ローソク足"),
			["pt"] = ("vela", "velas"),
			["zh"] = ("K线", "K线")
		};

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			if (!localizedTerms.TryGetValue(lang, out var term))
				continue;

			var langRoot = Path.Combine(_repoRoot, lang);
			var replacement = term.Singular.Equals(term.Plural, StringComparison.Ordinal)
				? term.Singular
				: $"{term.Singular}/{term.Plural}";

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var relative = Path.GetRelativePath(langRoot, file).Replace('\\', '/');
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
					AddLocalizedCandleTermError(file, relative, line, "markdown", NormalizeTextForLocalizedCandleTermCheck(text), replacement, pattern, errors);

				foreach (var textLine in EnumerateMarkdownTextLikeCodeBlockLines(markdown))
					AddLocalizedCandleTermError(file, relative, textLine.Line, "markdown text block", NormalizeTextForLocalizedCandleTermCheck(textLine.Text), replacement, pattern, errors);

				foreach (var comment in EnumerateCodeComments(markdown))
					AddLocalizedCandleTermError(file, relative, comment.Line, "code comment", NormalizeTextForLocalizedCandleTermCheck(NormalizeCodeCommentForTranslationCheck(comment.Text)), replacement, pattern, errors);

				foreach (var literal in EnumerateCodeStringLiterals(markdown))
					AddLocalizedCandleTermError(file, relative, literal.Line, "code string", NormalizeTextForLocalizedCandleTermCheck(NormalizeCodeStringLiteralForTranslationCheck(literal.Text)), replacement, pattern, errors);
			}

			var tocPath = Path.Combine(langRoot, "topics", "toc.yml");
			foreach (var nameLine in EnumerateTocNameLines(tocPath))
				AddLocalizedCandleTermError(tocPath, string.Empty, nameLine.Line, "TOC", NormalizeTextForLocalizedCandleTermCheck(nameLine.Name), replacement, pattern, errors);
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void JapaneseChineseMarkdownDoesNotKeepLowercaseEnglishChartTerm()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"(?<![A-Za-z0-9_./\\])charts?(?![A-Za-z0-9_./\\])", RegexOptions.CultureInvariant);

		foreach (var lang in new[] { "ja", "zh" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					var normalized = NormalizeMarkdownTextForLocalizedLoggingTermCheck(text);
					var match = pattern.Match(normalized);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: {lang} markdown keeps lowercase English chart term '{match.Value}'. Use localized chart wording in prose.");
				}

				foreach (var textLine in EnumerateMarkdownTextLikeCodeBlockLines(markdown))
				{
					var normalized = NormalizeMarkdownTextForLocalizedLoggingTermCheck(textLine.Text);
					var match = pattern.Match(normalized);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{textLine.Line}: {lang} markdown text block keeps lowercase English chart term '{match.Value}'. Use localized chart wording in prose.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void JapaneseMarkdownUsesConsistentCandlestickTerm()
	{
		var errors = new List<string>();
		var japaneseRoot = Path.Combine(_repoRoot, "ja");

		foreach (var file in Directory.EnumerateFiles(japaneseRoot, "*.*", SearchOption.AllDirectories)
			.Where(file => file.EndsWith(".md", StringComparison.OrdinalIgnoreCase) || file.EndsWith(".yml", StringComparison.OrdinalIgnoreCase))
			.Order(StringComparer.OrdinalIgnoreCase))
		{
			var lines = ReadAllText(file).Split('\n');

			for (var i = 0; i < lines.Length; i++)
			{
				if (!lines[i].Contains("キャンドル", StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{i + 1}: Japanese documentation uses 'キャンドル'. Use the consistent candlestick term 'ローソク足'.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void JapaneseChineseMarkdownDoesNotKeepEnglishTickTerm()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"(?<![A-Za-z0-9_./\\])ticks?(?![A-Za-z0-9_./\\])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in new[] { "ja", "zh" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
					AddLocalizedTickTermError(file, line, lang, "markdown", NormalizeMarkdownTextForLocalizedLoggingTermCheck(text), pattern, errors);

				foreach (var textLine in EnumerateMarkdownTextLikeCodeBlockLines(markdown))
					AddLocalizedTickTermError(file, textLine.Line, lang, "markdown text block", NormalizeMarkdownTextForLocalizedLoggingTermCheck(textLine.Text), pattern, errors);
			}

			var tocPath = Path.Combine(langRoot, "topics", "toc.yml");
			foreach (var nameLine in EnumerateTocNameLines(tocPath))
				AddLocalizedTickTermError(tocPath, nameLine.Line, lang, "TOC", NormalizeMarkdownTextForLocalizedLoggingTermCheck(nameLine.Name), pattern, errors);
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
	public void LocalizedCandlePatternDocsDoNotKeepEnglishPatternNames()
	{
		var errors = new List<string>();
		var adjectivePattern = new Regex(@"\b(?:bullish|bearish)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var patternRoot = Path.Combine(_repoRoot, lang, "topics", "api", "patterns");
			if (!Directory.Exists(patternRoot))
				continue;

			var patternNames = _knownEnglishCandlePatternNames
				.Where(name => !lang.Equals("de", StringComparison.OrdinalIgnoreCase) || !name.Equals("Hammer", StringComparison.Ordinal))
				.ToArray();

			foreach (var file in Directory.EnumerateFiles(patternRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var textWithoutInlineCode = Regex.Replace(text, @"`[^`\r\n]*`", " ", RegexOptions.CultureInvariant);
					var normalized = NormalizeMarkdownTextForTranslationCheck(textWithoutInlineCode);

					foreach (var patternName in patternNames)
					{
						if (!ContainsStandaloneText(normalized, patternName))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: localized candle pattern documentation keeps English pattern name '{patternName}'. Localize visible pattern prose and link labels.");
					}

					var adjectiveMatch = adjectivePattern.Match(normalized);
					if (adjectiveMatch.Success)
						errors.Add($"{RelativeToRepo(file)}:{line}: localized candle pattern documentation keeps English adjective '{adjectiveMatch.Value}'. Localize visible pattern prose.");
				}

				foreach (var altText in EnumerateMarkdownImageAltTexts(ReadAllText(file)))
				{
					var normalized = NormalizeHumanText(altText.Text);

					foreach (var patternName in patternNames)
					{
						if (!ContainsStandaloneText(normalized, patternName))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{altText.Line}: localized candle pattern image alt text keeps English pattern name '{patternName}'. Localize the image description.");
					}

					var adjectiveMatch = adjectivePattern.Match(normalized);
					if (adjectiveMatch.Success)
						errors.Add($"{RelativeToRepo(file)}:{altText.Line}: localized candle pattern image alt text keeps English adjective '{adjectiveMatch.Value}'. Localize the image description.");
				}

				foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
				{
					var normalized = NormalizeHumanText(comment.Text);

					foreach (var patternName in patternNames)
					{
						if (!ContainsStandaloneText(normalized, patternName))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized candle pattern code comment keeps English pattern name '{patternName}'. Localize the code comment.");
					}

					var adjectiveMatch = adjectivePattern.Match(normalized);
					if (adjectiveMatch.Success)
						errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized candle pattern code comment keeps English adjective '{adjectiveMatch.Value}'. Localize the code comment.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedComplexPatternSampleDoesNotKeepEnglishCustomName()
	{
		var errors = new List<string>();
		var relativePath = Path.Combine("topics", "api", "patterns", "complex_patterns.md");

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath);
			if (!File.Exists(file))
				continue;

			foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
			{
				if (!literal.Text.Equals("Reversal Up", StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{literal.Line}: localized complex pattern sample keeps the English custom pattern name 'Reversal Up'. Localize the sample string literal.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedAdvancedStrategyDocsDoNotKeepEnglishSampleStrategyName()
	{
		var errors = new List<string>();
		var relativePath = Path.Combine("topics", "api", "strategies", "advanced_features.md");

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				if (!text.Contains("SMA Crossover", StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: localized advanced strategy documentation keeps the English sample strategy name 'SMA Crossover'. Localize the visible example name.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedTradingOperationSamplesDoNotKeepEnglishCustomOrderComment()
	{
		var errors = new List<string>();
		var relativePath = Path.Combine("topics", "api", "strategies", "trading_operations.md");

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath);
			if (!File.Exists(file))
				continue;

			foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
			{
				if (!literal.Text.Equals("Custom order", StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{literal.Line}: localized trading operation sample keeps the English order comment 'Custom order'. Localize the sample string literal.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedShortSampleStringsDoNotKeepKnownEnglishLiterals()
	{
		var errors = new List<string>();
		var phrasesByRelativePath = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
		{
			["topics/api/strategies/parameters.md"] = ["SMA strategy"],
			["topics/api/indicators.md"] = ["SMA strategy"],
			["topics/api/graphical_user_interface/charts/annotations.md"] = ["New annotation"],
			["topics/api/basket_routing.md"] = ["All adapters connected"],
			["topics/hydra/server_mode/fix_fast_connectivity.md"] = ["Connection established"],
			["topics/api/instruments/instrument_search.md"] = ["Enter a search criterion"],
			["topics/api/graphical_user_interface/logging/log_panel.md"] = ["Warning test message"],
			["topics/api/testing/historical_data.md"] = ["test account"],
			["topics/api/logging/other_logs_sources.md"] = ["Warning (source)!!!", "Warning (trace)!!!", "Error (trace)!!!", "{0} (source)!!!", "{0} (trace)!!!"],
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var (relative, phrases) in phrasesByRelativePath)
			{
				var file = Path.Combine(langRoot, relative.Replace('/', Path.DirectorySeparatorChar));
				if (!File.Exists(file))
					continue;

				foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
				{
					foreach (var phrase in phrases)
					{
						if (!literal.Text.Equals(phrase, StringComparison.Ordinal))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{literal.Line}: localized sample keeps the English string literal '{phrase}'. Localize the sample string literal.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedOAuthConnectorDocsDoNotKeepEnglishActionButtonLabels()
	{
		var errors = new List<string>();
		var relativePaths = new[]
		{
			Path.Combine("topics", "api", "connectors", "forex", "ctrader", "graphical_configuration_ctrader.md"),
			Path.Combine("topics", "api", "connectors", "stock_market", "tradier", "graphical_configuration_tradier.md"),
		};
		var phrases = new[] { "\"Check\"", "\"Start\"" };

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(_repoRoot, lang, relativePath);
				if (!File.Exists(file))
					continue;

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					foreach (var phrase in phrases)
					{
						if (!text.Contains(phrase, StringComparison.Ordinal))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: localized OAuth connector documentation keeps English button label {phrase}.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedConnectionSettingsDocsDoNotKeepEnglishCheckButtonLabel()
	{
		var errors = new List<string>();
		var relativePath = Path.Combine("topics", "api", "graphical_user_interface", "connection_settings_window.md");

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				if (!text.Contains("**Check**", StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: localized connection settings documentation keeps the English button label '**Check**'.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedHydraInstrumentListDoesNotKeepEnglishGeneralTabLabel()
	{
		var errors = new List<string>();
		var relativePath = Path.Combine("topics", "hydra", "instruments_and_boards", "instruments_list.md");
		var languages = new[] { "de", "ja", "pt", "zh" };

		foreach (var lang in languages)
		{
			var file = Path.Combine(_repoRoot, lang, relativePath);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				if (!ContainsStandaloneText(text, "General"))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: localized Hydra instrument list keeps the English 'General' tab label.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedGeneralDocsDoNotKeepEnglishExplanatoryPhrases()
	{
		var errors = new List<string>();
		var checks = new[]
		{
			(
				Lang: "de",
				RelativePath: "topics/shell.md",
				Fragments: new[] { "Graphical User Interface" }
			),
			(
				Lang: "es",
				RelativePath: "topics/shell.md",
				Fragments: new[] { "Graphical User Interface" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/shell.md",
				Fragments: new[] { "Graphical User Interface" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/shell.md",
				Fragments: new[] { "Graphical User Interface" }
			),
			(
				Lang: "de",
				RelativePath: "topics/api/market_data_storage.md",
				Fragments: new[] { "Data Mining" }
			),
			(
				Lang: "de",
				RelativePath: "topics/api.md",
				Fragments: new[] { "Open Source" }
			),
			(
				Lang: "es",
				RelativePath: "topics/common/source_codes.md",
				Fragments: new[] { "Open Source" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/common/source_codes.md",
				Fragments: new[] { "Open Source" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/common/source_codes.md",
				Fragments: new[] { "Open Source" }
			),
			(
				Lang: "de",
				RelativePath: "topics/designer/user_interface/components/chart.md",
				Fragments: new[] { "Toolbar", "Auto-Scroll", "Auto-Zoom", "Levels" }
			),
			(
				Lang: "de",
				RelativePath: "topics/terminal/user_interface/components/chart.md",
				Fragments: new[] { "Toolbar", "Auto-Scroll", "Auto-Zoom", "Levels" }
			),
			(
				Lang: "de",
				RelativePath: "topics/runner/connection_setup.md",
				Fragments: new[] { "Connections" }
			),
			(
				Lang: "es",
				RelativePath: "topics/runner/connection_setup.md",
				Fragments: new[] { "Connections" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/runner/connection_setup.md",
				Fragments: new[] { "Connections" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/runner/connection_setup.md",
				Fragments: new[] { "Connections" }
			),
			(
				Lang: "zh",
				RelativePath: "topics/runner/connection_setup.md",
				Fragments: new[] { "Connections" }
			),
			(
				Lang: "de",
				RelativePath: "topics/hydra/server_mode/settings.md",
				Fragments: new[] { "FIX/FAST connectivity" }
			),
			(
				Lang: "de",
				RelativePath: "topics/hydra/server_mode/settings.md",
				Fragments: new[] { "Dialect des", "Sender und Recipient", "Subscriptions beim" }
			),
			(
				Lang: "de",
				RelativePath: "topics/hydra/server_mode/hydra_client_connectivity.md",
				Fragments: new[] { "FIX protocol" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/hydra/server_mode/settings.md",
				Fragments: new[] { "Dialect", "Sender", "Recipient", "Data format" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/hydra/server_mode/settings.md",
				Fragments: new[] { "Sender e Recipient" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/api/testing/optimization.md",
				Fragments: new[] { "Brute force", "Genetic algorithm" }
			),
			(
				Lang: "de",
				RelativePath: "topics/api/testing/optimization.md",
				Fragments: new[] { "Brute Force" }
			),
			(
				Lang: "de",
				RelativePath: "topics/designer/optimization/brute_force.md",
				Fragments: new[] { "Brute Force" }
			),
			(
				Lang: "zh",
				RelativePath: "topics/api/testing/optimization.md",
				Fragments: new[] { "Brute force", "Genetic algorithm" }
			),
			(
				Lang: "zh",
				RelativePath: "topics/hydra/importing/order_log.md",
				Fragments: new[] { "Time in force" }
			),
			(
				Lang: "de",
				RelativePath: "topics/hydra/prepare_for_download/custom_candles.md",
				Fragments: new[] { "Custom", "Generated" }
			),
			(
				Lang: "es",
				RelativePath: "topics/hydra/prepare_for_download/custom_candles.md",
				Fragments: new[] { "Custom", "Generated" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/hydra/prepare_for_download/custom_candles.md",
				Fragments: new[] { "Custom", "Generated", "Ticks" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/hydra/prepare_for_download/custom_candles.md",
				Fragments: new[] { "Custom", "Generated" }
			),
			(
				Lang: "zh",
				RelativePath: "topics/hydra/prepare_for_download/custom_candles.md",
				Fragments: new[] { "Custom", "Generated", "Ticks" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/hydra/importing/ticks.md",
				Fragments: new[] { "Ticks" }
			),
			(
				Lang: "zh",
				RelativePath: "topics/hydra/server_mode/emulation_setup.md",
				Fragments: new[] { "Re-registration" }
			),
			(
				Lang: "es",
				RelativePath: "topics/hydra/server_mode/settings.md",
				Fragments: new[] { "Simulator" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/hydra/server_mode/settings.md",
				Fragments: new[] { "Simulator" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/hydra/server_mode/settings.md",
				Fragments: new[] { "Simulator" }
			),
			(
				Lang: "zh",
				RelativePath: "topics/hydra/server_mode/settings.md",
				Fragments: new[] { "Simulator" }
			),
			(
				Lang: "de",
				RelativePath: "topics/hydra/tasks/export_auto.md",
				Fragments: new[] { "Split" }
			),
			(
				Lang: "es",
				RelativePath: "topics/hydra/tasks/export_auto.md",
				Fragments: new[] { "Split" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/hydra/tasks/export_auto.md",
				Fragments: new[] { "Split" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/hydra/tasks/export_auto.md",
				Fragments: new[] { "Split" }
			),
			(
				Lang: "zh",
				RelativePath: "topics/hydra/tasks/export_auto.md",
				Fragments: new[] { "Split" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/hydra/analytics/running_a_script.md",
				Fragments: new[] { "Instrument" }
			),
			(
				Lang: "zh",
				RelativePath: "topics/hydra/analytics/running_a_script.md",
				Fragments: new[] { "Instrument" }
			),
			(
				Lang: "de",
				RelativePath: "topics/designer/backtesting/debugging/break_points.md",
				Fragments: new[] { "True" }
			),
			(
				Lang: "es",
				RelativePath: "topics/designer/backtesting/debugging/break_points.md",
				Fragments: new[] { "True" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/designer/backtesting/debugging/break_points.md",
				Fragments: new[] { "True" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/designer/backtesting/debugging/break_points.md",
				Fragments: new[] { "True" }
			),
			(
				Lang: "zh",
				RelativePath: "topics/designer/backtesting/debugging/break_points.md",
				Fragments: new[] { "True" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/designer/backtesting/debugging.md",
				Fragments: new[] { "breakpoint", "breakpoints" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/designer/backtesting/debugging/break_points.md",
				Fragments: new[] { "breakpoint", "breakpoints" }
			),
			(
				Lang: "de",
				RelativePath: "topics/designer/optimization/optimization_parameters.md",
				Fragments: new[] { "True-False" }
			),
			(
				Lang: "es",
				RelativePath: "topics/designer/optimization/optimization_parameters.md",
				Fragments: new[] { "True-False" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/designer/optimization/optimization_parameters.md",
				Fragments: new[] { "True-False" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/designer/optimization/optimization_parameters.md",
				Fragments: new[] { "True-False" }
			),
			(
				Lang: "zh",
				RelativePath: "topics/designer/optimization/optimization_parameters.md",
				Fragments: new[] { "True-False" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/designer/strategies/using_visual_designer/elements.md",
				Fragments: new[] { "up（true）", "down（false）" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/designer/strategies/using_visual_designer/elements/common/logical_condition.md",
				Fragments: new[] { "raised（true）", "lowered（false）" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/designer/strategies/using_visual_designer/elements/common/comparison.md",
				Fragments: new[] { "up（true）", "down（false）" }
			),
			(
				Lang: "es",
				RelativePath: "topics/api/import.md",
				Fragments: new[] { "order log" }
			),
			(
				Lang: "zh",
				RelativePath: "topics/api/import.md",
				Fragments: new[] { "ticks", "candles", "order books", "order log", "transactions", "instruments", "positions" }
			),
			(
				Lang: "de",
				RelativePath: "topics/api/instruments/instrument_identifier.md",
				Fragments: new[] { "instrument code", "board code" }
			),
			(
				Lang: "de",
				RelativePath: "topics/designer/user_interface/boards.md",
				Fragments: new[] { "board code" }
			),
			(
				Lang: "de",
				RelativePath: "topics/designer/market_data_storage/create_instrument.md",
				Fragments: new[] { "security code", "board code" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/api/instruments/instrument_identifier.md",
				Fragments: new[] { "instrument code", "board code" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/designer/market_data_storage/create_instrument.md",
				Fragments: new[] { "security code", "board code" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/designer/market_data_storage/create_instrument.md",
				Fragments: new[] { "security code", "board code" }
			),
			(
				Lang: "zh",
				RelativePath: "topics/api/instruments/instrument_identifier.md",
				Fragments: new[] { "instrument code", "board code" }
			),
			(
				Lang: "zh",
				RelativePath: "topics/designer/user_interface/boards.md",
				Fragments: new[] { "board code" }
			),
			(
				Lang: "de",
				RelativePath: "topics/designer/strategies/using_visual_designer/elements/options/strikes.md",
				Fragments: new[] { "Call option", "Put option" }
			),
			(
				Lang: "es",
				RelativePath: "topics/designer/strategies/using_visual_designer/elements/options/strikes.md",
				Fragments: new[] { "Call option", "Put option" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/designer/strategies/using_visual_designer/elements/options/strikes.md",
				Fragments: new[] { "Call option", "Put option" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/designer/strategies/using_visual_designer/elements/options/strikes.md",
				Fragments: new[] { "Call option", "Put option" }
			),
			(
				Lang: "zh",
				RelativePath: "topics/designer/strategies/using_visual_designer/elements/options/strikes.md",
				Fragments: new[] { "Call option", "Put option" }
			),
			(
				Lang: "zh",
				RelativePath: "topics/api/indicators/list_of_indicators/oscillator_of_moving_average.md",
				Fragments: new[] { "oscillates" }
			),
			(
				Lang: "de",
				RelativePath: "topics/api/strategies/trading_modes.md",
				Fragments: new[] { "trading is prohibited", "| yes |", "| no |" }
			),
			(
				Lang: "es",
				RelativePath: "topics/api/strategies/trading_modes.md",
				Fragments: new[] { "trading is prohibited" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/api/strategies/trading_modes.md",
				Fragments: new[] { "trading is prohibited" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/api/strategies/trading_modes.md",
				Fragments: new[] { "trading is prohibited" }
			),
			(
				Lang: "de",
				RelativePath: "topics/api/strategies/statistics_reference.md",
				Fragments: new[] { "insufficient funds" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/api/strategies/statistics_reference.md",
				Fragments: new[] { "insufficient funds" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/runner/integration_with_visual_studio.md",
				Fragments: new[] { "breakpoint", "breakpoints" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/api/testing/historical_data.md",
				Fragments: new[] { "order log" }
			),
			(
				Lang: "es",
				RelativePath: "topics/api/pnl.md",
				Fragments: new[] { "order log" }
			),
			(
				Lang: "es",
				RelativePath: "topics/api/export.md",
				Fragments: new[] { "order log" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/api/connectors/creating_own_connector/market_data.md",
				Fragments: new[] { "order log" }
			),
			(
				Lang: "de",
				RelativePath: "topics/api/order_books.md",
				Fragments: new[] { "Market Depth" }
			),
			(
				Lang: "de",
				RelativePath: "topics/api/strategies/event_model/rules_using.md",
				Fragments: new[] { "Market Depth" }
			),
			(
				Lang: "es",
				RelativePath: "topics/api/order_books/display.md",
				Fragments: new[] { "Market Depth" }
			),
			(
				Lang: "es",
				RelativePath: "topics/api/strategies/event_model/rules_using.md",
				Fragments: new[] { "Market Depth" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/api/strategies/event_model/rules_using.md",
				Fragments: new[] { "Market Depth" }
			),
			(
				Lang: "es",
				RelativePath: "topics/api/indicators/list_of_indicators/forecast_oscillator.md",
				Fragments: new[] { "Return to Zero" }
			),
			(
				Lang: "es",
				RelativePath: "topics/api/indicators/list_of_indicators/fractal_dimension.md",
				Fragments: new[] { "FDI closer to 2" }
			),
			(
				Lang: "de",
				RelativePath: "topics/api/testing/random_data.md",
				Fragments: new[] { "on the fly" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/api/testing/random_data.md",
				Fragments: new[] { "on the fly" }
			),
			(
				Lang: "de",
				RelativePath: "topics/api/instruments/instrument_search.md",
				Fragments: new[] { "Button-Click-Handler" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/api/pnl.md",
				Fragments: new[] { "via Adapter" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/api/slippage.md",
				Fragments: new[] { "via Adapter" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/api/market_data/getting_historical_data.md",
				Fragments: new[] { "via Connector" }
			),
			(
				Lang: "es",
				RelativePath: "topics/api/connectors/forex/metatrader.md",
				Fragments: new[] { "use Installer" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/api/connectors/creating_own_connector/instrument_lookup.md",
				Fragments: new[] { "exchange via API" }
			),
			(
				Lang: "de",
				RelativePath: "topics/designer/connections_settings/simulator.md",
				Fragments: new[] { "in Batches" }
			),
			(
				Lang: "de",
				RelativePath: "topics/hydra/misc/backup/hydra_settings.md",
				Fragments: new[] { "Offset in Tagen" }
			),
			(
				Lang: "de",
				RelativePath: "topics/api/graphical_user_interface/notification_settings_window.md",
				Fragments: new[] { "Trade-Initiator", "Market-Maker" }
			),
			(
				Lang: "de",
				RelativePath: "topics/terminal/notifications/notifications_setup.md",
				Fragments: new[] { "Trade-Initiator", "Market-Maker" }
			),
			(
				Lang: "de",
				RelativePath: "topics/hydra/importing/ticks.md",
				Fragments: new[] { "Trade-Initiator" }
			),
			(
				Lang: "de",
				RelativePath: "topics/api/strategies.md",
				Fragments: new[] { "Logging in Strategien", "Logging-Mechanismus" }
			),
			(
				Lang: "de",
				RelativePath: "topics/api/logging/strategy_logging.md",
				Fragments: new[] { "Logging in eine Testdatei", "Logging in das LogWindow", "Strategie-Logging", "Visuelle Logging-Komponenten" }
			),
			(
				Lang: "de",
				RelativePath: "topics/api/strategies/logging.md",
				Fragments: new[] { "Logging in Strategien", "Logging-Stufen", "Logging-Methoden", "Logging-Stufe", "Logging-Einstellungen" }
			),
			(
				Lang: "de",
				RelativePath: "topics/api/graphical_user_interface/logging.md",
				Fragments: new[] { "visueller Logging-Komponenten", "Um Logging", "Logs in MainWindow" }
			),
			(
				Lang: "de",
				RelativePath: "topics/hydra/analytics/create_own_script.md",
				Fragments: new[] { "für das Logging" }
			),
			(
				Lang: "ru",
				RelativePath: "topics/designer/backtesting/debugging.md",
				Fragments: new[] { "Step to out" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/api/indicators/list_of_indicators/detrended_synthetic_price.md",
				Fragments: new[] { "head and shoulders", "double bottom" }
			),
			(
				Lang: "de",
				RelativePath: "topics/hydra/videos/converter_task.md",
				Fragments: new[] { "Converter-Aufgabe" }
			),
			(
				Lang: "es",
				RelativePath: "topics/hydra/videos/converter_task.md",
				Fragments: new[] { "Tarea Converter" }
			),
		};

		foreach (var check in checks)
		{
			var file = Path.Combine(_repoRoot, check.Lang, check.RelativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				foreach (var fragment in check.Fragments)
				{
					if (!text.Contains(fragment, StringComparison.OrdinalIgnoreCase))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps English explanatory phrase '{fragment}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void HydraCommonTaskSettingsDoNotKeepConverterHeaderInNonConverterTasks()
	{
		var errors = new List<string>();
		var languages = new[] { "en", "de", "es", "ja", "pt", "ru", "zh" };
		var relativePaths = new[]
		{
			"topics/hydra/tasks/import_auto.md",
			"topics/hydra/tasks/export_auto.md",
			"topics/hydra/misc/backup/hydra_settings.md",
		};

		foreach (var lang in languages)
		{
			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
				if (!File.Exists(file))
					continue;

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					if (!text.Contains("Converter", StringComparison.Ordinal))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: non-converter Hydra task common settings keep 'Converter' as the task header.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotKeepEnglishLoggingProse()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\blogging\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var checks = new[]
		{
			(Lang: "es", LocalizedTerm: "registro"),
			(Lang: "pt", LocalizedTerm: "registo"),
		};

		foreach (var check in checks)
		{
			var root = Path.Combine(_repoRoot, check.Lang, "topics");

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
						errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps English 'logging' prose. Use '{check.LocalizedTerm}' outside API identifiers.");
				}

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(altText.Text)))
						errors.Add($"{RelativeToRepo(file)}:{altText.Line}: localized image alt text keeps English 'logging' prose. Use '{check.LocalizedTerm}'.");
				}

				foreach (var comment in EnumerateCodeComments(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(comment.Text)))
						errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized code comment keeps English 'logging' prose. Use '{check.LocalizedTerm}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotKeepEnglishLoggerListenerProse()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:logger|listeners?)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var checks = new[]
		{
			(Lang: "es", LoggerTerm: "registrador", ListenerTerm: "receptor"),
			(Lang: "pt", LoggerTerm: "registador", ListenerTerm: "ouvinte"),
		};

		foreach (var check in checks)
		{
			var root = Path.Combine(_repoRoot, check.Lang, "topics");

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
						errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps English logger/listener prose. Use '{check.LoggerTerm}' or '{check.ListenerTerm}' outside API identifiers.");
				}

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(altText.Text)))
						errors.Add($"{RelativeToRepo(file)}:{altText.Line}: localized image alt text keeps English logger/listener prose. Use localized wording.");
				}

				foreach (var comment in EnumerateCodeComments(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(comment.Text)))
						errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized code comment keeps English logger/listener prose. Use localized wording.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotKeepEnglishLogsProse()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\blogs\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var checks = new[]
		{
			(Lang: "es", LocalizedTerm: "registros"),
			(Lang: "pt", LocalizedTerm: "registos"),
		};

		foreach (var check in checks)
		{
			var root = Path.Combine(_repoRoot, check.Lang, "topics");

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
						errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps English 'logs' prose. Use '{check.LocalizedTerm}' outside API identifiers, paths, and filenames.");
				}

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(altText.Text)))
						errors.Add($"{RelativeToRepo(file)}:{altText.Line}: localized image alt text keeps English 'logs' prose. Use '{check.LocalizedTerm}'.");
				}

				foreach (var comment in EnumerateCodeComments(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(comment.Text)))
						errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized code comment keeps English 'logs' prose. Use '{check.LocalizedTerm}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishPortugueseDocsDoNotKeepEnglishLogTermAsLoggingProse()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:panel de log|painel de log(?: alargado)?|ventana de log|janela de log|archivos? de log|archivo de log|arquivo de log|ficheiros de log|mensajes de log|mensagens de log|fuentes? de log|fontes? de log|fonte de log|origen de log|nivel de log|nível de log|configuraci[oó]n(?:es)? de log|configurações de log|log de la estrategia|log da estratégia|log del programa|log do programa|log da aplicação|entradas de log|entrada en archivo de log|entrada em ficheiro de log|notificación al log|notificação para o log|escritos en el log|escritas no log|se registra en el log|são gravados no log|aparecerá en el log|aparecerá no log|agrega al log|adicionad[ao] ao log|escribir mensajes en el log|escrever mensagens no log|sonido,\s*log,\s*telegram|som,\s*log,\s*telegram|en el log|no log|al log|ao log)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var checks = new[]
		{
			(Lang: "es", LocalizedTerm: "registro"),
			(Lang: "pt", LocalizedTerm: "registo"),
		};

		foreach (var check in checks)
		{
			var root = Path.Combine(_repoRoot, check.Lang, "topics");
			var tocPath = Path.Combine(root, "toc.yml");
			var files = Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories)
				.Concat(File.Exists(tocPath) ? [tocPath] : [])
				.Order(StringComparer.OrdinalIgnoreCase);

			foreach (var file in files)
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
						errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps English 'log' as logging prose. Use '{check.LocalizedTerm}' outside API identifiers, formulas, paths, and filenames.");
				}

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(altText.Text)))
						errors.Add($"{RelativeToRepo(file)}:{altText.Line}: localized image alt text keeps English 'log' as logging prose. Use '{check.LocalizedTerm}'.");
				}

				foreach (var comment in EnumerateCodeComments(markdown))
				{
					var normalized = NormalizeMarkdownTextForLocalizedLoggingTermCheck(NormalizeCodeCommentForTranslationCheck(comment.Text));
					if (pattern.IsMatch(normalized))
						errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized code comment keeps English 'log' as logging prose. Use '{check.LocalizedTerm}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanSpanishPortugueseDocsDoNotKeepEnglishSnapshotDriveProse()
	{
		var errors = new List<string>();
		var snapshotPattern = new Regex(@"\bsnapshots?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var portugueseDrivePattern = new Regex(@"\bdrives?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in new[] { "de", "es", "pt" })
		{
			var root = Path.Combine(_repoRoot, lang, "topics");
			var tocPath = Path.Combine(root, "toc.yml");

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);
				AddLocalizedEnglishTermErrors(file, markdown, snapshotPattern, "snapshot", errors);

				if (lang == "pt")
					AddLocalizedEnglishTermErrors(file, markdown, portugueseDrivePattern, "drive", errors);
			}

			foreach (var nameLine in EnumerateTocNameLines(tocPath))
			{
				if (snapshotPattern.IsMatch(nameLine.Name))
					errors.Add($"{RelativeToRepo(tocPath)}:{nameLine.Line}: localized TOC name keeps English 'snapshot' prose.");

				if (lang == "pt" && portugueseDrivePattern.IsMatch(nameLine.Name))
					errors.Add($"{RelativeToRepo(tocPath)}:{nameLine.Line}: Portuguese TOC name keeps English 'drive' prose.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanSpanishPortugueseDocsDoNotKeepEnglishStreamingTerms()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bstreaming\b|\bdata[- ]feed\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in new[] { "de", "es", "pt" })
		{
			var root = Path.Combine(_repoRoot, lang, "topics");

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
						errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps English streaming/data-feed wording.");
				}

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(altText.Text)))
						errors.Add($"{RelativeToRepo(file)}:{altText.Line}: localized image alt text keeps English streaming/data-feed wording.");
				}

				foreach (var comment in EnumerateCodeComments(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(comment.Text)))
						errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized code comment keeps English streaming/data-feed wording.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedChoppinessDocsDoNotKeepEnglishProseFragments()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"Setting threshold levels|\b(?:High|Low)\s+[""„]|\bHigh CHOP\b|[""„]choppiness[""“]|\bRange-Trading\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var relativePaths = new[]
		{
			"topics/api/indicators/list_of_indicators/choppiness_index.md",
		};

		foreach (var lang in new[] { "de", "es", "pt" })
		{
			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
						errors.Add($"{RelativeToRepo(file)}:{line}: localized CHOP documentation keeps an English prose fragment.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedOptimizationCodeCommentsDoNotKeepEnglishPhrases()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"maximum iterations|number of parallel threads|default = CPU|commission|with a step of|use the range from SetOptimize", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		const string relativePath = "topics/api/testing/optimization.md";

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
			{
				if (pattern.IsMatch(comment.Text))
					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized optimization code comment keeps an English phrase.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedConnectorCodeCommentsDoNotKeepEnglishRealtimeUpdatePhrase()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"since this is|real-time update|not a response to|specific request", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var root = Path.Combine(_repoRoot, lang, "topics");

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
				{
					if (pattern.IsMatch(comment.Text))
						errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized code comment keeps an English real-time update phrase.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCodeCommentsDoNotKeepEnglishOrThisPhrase()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bor this\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var root = Path.Combine(_repoRoot, lang, "topics");

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
				{
					if (pattern.IsMatch(comment.Text))
						errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized code comment keeps English 'or this' wording.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void PortugueseDocsDoNotKeepEnglishStatusPhrases()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bstatus\s+(?:online|d[ae]\s+ordem)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var root = Path.Combine(_repoRoot, "pt", "topics");

		foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var markdown = ReadAllText(file);

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
					errors.Add($"{RelativeToRepo(file)}:{line}: Portuguese documentation keeps English 'status' wording.");
			}

			foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(altText.Text)))
					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: Portuguese image alt text keeps English 'status' wording.");
			}

			foreach (var comment in EnumerateCodeComments(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(comment.Text)))
					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: Portuguese code comment keeps English 'status' wording.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void JapaneseDocsDoNotKeepEnglishCompleteTerms()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bcomplete(?:d)?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var root = Path.Combine(_repoRoot, "ja", "topics");

		foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var markdown = ReadAllText(file);

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
					errors.Add($"{RelativeToRepo(file)}:{line}: Japanese documentation keeps English complete/completed wording.");
			}

			foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(altText.Text)))
					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: Japanese image alt text keeps English complete/completed wording.");
			}

			foreach (var comment in EnumerateCodeComments(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(comment.Text)))
					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: Japanese code comment keeps English complete/completed wording.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCandleCodeCommentsDoNotKeepEnglishPointAndFigure()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bPoint and Figure\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, "topics", "api", "candles.md");
			if (!File.Exists(file))
				continue;

			foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
			{
				if (!pattern.IsMatch(comment.Text))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized candle code comment keeps English Point and Figure wording.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCandleGluingSampleStatusStringsAreTranslated()
	{
		var errors = new List<string>();
		var relativePath = "topics/api/candles/gluing_candles_history_real_time.md";
		var forbidden = new HashSet<string>(StringComparer.Ordinal)
		{
			"Real-time",
			"History",
		};

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
			{
				if (forbidden.Contains(literal.Text))
					errors.Add($"{RelativeToRepo(file)}:{literal.Line}: localized candle gluing sample keeps an English status string literal.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCustomIndicatorSummaryDoesNotKeepEnglishSimpleMovingAverage()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bSimple moving average\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var relativePath = "topics/api/indicators/custom_indicator.md";

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
			{
				if (!pattern.IsMatch(comment.Text))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized custom indicator XML summary keeps English Simple moving average text.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCustomIndicatorXmlCommentsDoNotKeepEnglishParamText()
	{
		var errors = new List<string>();
		var pattern = new Regex(@">\s*(?:Indicator|Value time)\.\s*</param>", RegexOptions.CultureInvariant);
		var relativePath = "topics/api/indicators/custom_indicator.md";

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
			{
				if (pattern.IsMatch(comment.Text))
					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized XML doc comment keeps English param text.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDesignerCodeCommentsDoNotKeepEnglishDocumentationLabels()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bSee more examples\b|^Doc\s+https?://", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var rootNames = new[] { "designer", "topics/designer" };

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			foreach (var rootName in rootNames)
			{
				var root = Path.Combine(_repoRoot, lang, rootName.Replace('/', Path.DirectorySeparatorChar));
				if (!Directory.Exists(root))
					continue;

				foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
				{
					var markdown = ReadAllText(file);

					foreach (var comment in EnumerateCodeComments(markdown))
					{
						if (pattern.IsMatch(comment.Text))
							errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized Designer code comment keeps an English documentation label.");
					}

					var lines = markdown.Replace("\r\n", "\n").Split('\n');
					for (var i = 0; i < lines.Length; i++)
					{
						var text = Regex.Replace(lines[i].Trim(), @"^(?:///|//|#)\s*", string.Empty, RegexOptions.CultureInvariant);
						if (pattern.IsMatch(text))
							errors.Add($"{RelativeToRepo(file)}:{i + 1}: localized Designer documentation string keeps an English documentation label.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void PortugueseStrategyIndicatorCommentsDoNotKeepEnglishIncorrectLabel()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bincorrect\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var file = Path.Combine(_repoRoot, "pt", "topics", "api", "strategies", "indicators.md");

		foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
		{
			if (pattern.IsMatch(comment.Text))
				errors.Add($"{RelativeToRepo(file)}:{comment.Line}: Portuguese strategy indicator comment keeps English 'incorrect' label.");
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedIFileSystemDocsDoNotKeepEnglishObsoleteMessage()
	{
		var errors = new List<string>();
		const string relativePath = "topics/api/connectors/ifile_system_and_paths.md";
		const string forbidden = "Use IFileSystem overload.";

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
			{
				if (literal.Text.Equals(forbidden, StringComparison.Ordinal))
					errors.Add($"{RelativeToRepo(file)}:{literal.Line}: localized IFileSystem documentation keeps an English obsolete message.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedNewsExamplesDoNotKeepEnglishConsoleLabels()
	{
		var errors = new List<string>();
		var relativePaths = new[]
		{
			"topics/api/market_data/getting_news_data.md",
			"topics/api/graphical_user_interface/market_data/news.md",
		};
		var forbiddenLabels = new HashSet<string>(StringComparer.Ordinal)
		{
			"News:",
			"Headline:",
			"Source:",
			"Time:",
			"URL:",
			"Story:",
			"New news on topic: {news.Headline}",
		};

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
				if (!File.Exists(file))
					continue;

				foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
				{
					if (forbiddenLabels.Contains(literal.Text) || (lang != "de" && literal.Text.Equals("Text:", StringComparison.Ordinal)))
						errors.Add($"{RelativeToRepo(file)}:{literal.Line}: localized news example keeps an English console label '{literal.Text}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedImportExamplesDoNotKeepEnglishConsoleLabels()
	{
		var errors = new List<string>();
		const string relativePath = "topics/api/import.md";
		var forbiddenLabels = new HashSet<string>(StringComparer.Ordinal)
		{
			"Progress: {p}%",
			"Imported {count} records, last: {lastTime}",
		};

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
			{
				if (forbiddenLabels.Contains(literal.Text))
					errors.Add($"{RelativeToRepo(file)}:{literal.Line}: localized import example keeps an English console label '{literal.Text}'.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedOptimizationAdvancedExamplesDoNotKeepEnglishProgressLabels()
	{
		var errors = new List<string>();
		const string relativePath = "topics/api/strategies/optimization_advanced.md";
		var forbiddenLabels = new HashSet<string>(StringComparer.Ordinal)
		{
			"Progress: {tracker.TotalProgress:F1}%, ",
			@"Remaining: {tracker.Remaining:hh\\:mm\\:ss}",
		};

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
			{
				if (forbiddenLabels.Contains(literal.Text))
					errors.Add($"{RelativeToRepo(file)}:{literal.Line}: localized advanced optimization example keeps an English progress label '{literal.Text}'.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedEventRuleNamesDoNotKeepEnglishMoneyIncreaseLabel()
	{
		var errors = new List<string>();
		const string relativePath = "topics/api/strategies/event_model/rules_create.md";
		const string forbidden = "Money increase of portfolio {0} above {1}";

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
			{
				if (literal.Text.Equals(forbidden, StringComparison.Ordinal))
					errors.Add($"{RelativeToRepo(file)}:{literal.Line}: localized event rule keeps an English rule name.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedArbitrageSamplesDoNotKeepEnglishProfitLabel()
	{
		var errors = new List<string>();
		const string relativePath = "topics/api/strategies/samples/arbitrage.md";
		const string forbidden = "Profit: {_profit}";

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
			{
				if (literal.Text.Equals(forbidden, StringComparison.Ordinal))
					errors.Add($"{RelativeToRepo(file)}:{literal.Line}: localized arbitrage sample keeps an English profit label.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDesignerCustomIndicatorSamplesDoNotKeepEnglishChangeLabel()
	{
		var errors = new List<string>();
		var relativePaths = new[]
		{
			"topics/designer/strategies/using_code/csharp/create_own_indicator.md",
			"topics/designer/strategies/using_code/fsharp/create_own_indicator.md",
			"topics/designer/strategies/using_code/python/create_own_indicator.md",
		};
		var forbiddenLabels = new HashSet<string>(StringComparer.Ordinal)
		{
			"Change: {Change}",
			"Change: %d",
			"Change: {self.Change}",
		};

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
				if (!File.Exists(file))
					continue;

				foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
				{
					if (forbiddenLabels.Contains(literal.Text))
						errors.Add($"{RelativeToRepo(file)}:{literal.Line}: localized Designer custom indicator sample keeps an English change label '{literal.Text}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanApiTablesDoNotKeepKnownEnglishHeaderAndDescriptionLabels()
	{
		var errors = new List<string>();
		var checks = new Dictionary<string, string[]>
		{
			["topics/api/market_data_storage/snapshots.md"] =
			[
				"| Serializer | Message Type | Zweck |",
			],
			["topics/api/strategies/reporting.md"] =
			[
				"| `Orders` | `IEnumerable<ReportOrder>` | Orders |",
			],
		};

		foreach (var (relativePath, forbiddenRows) in checks)
		{
			var file = Path.Combine(_repoRoot, "de", relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			var lines = ReadAllText(file).Replace("\r\n", "\n").Split('\n');
			for (var i = 0; i < lines.Length; i++)
			{
				foreach (var forbiddenRow in forbiddenRows)
				{
					if (lines[i].Equals(forbiddenRow, StringComparison.Ordinal))
						errors.Add($"{RelativeToRepo(file)}:{i + 1}: German API table keeps English label row '{forbiddenRow}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedApiHeadingsDoNotKeepKnownEnglishLabels()
	{
		var errors = new List<string>();
		var forbiddenHeadingPattern = new Regex(@"^#{2,6}\s+(?:OrderStates enum|Enum OrderStates|Enumeration StrategyCommentModes|1\.\s+Usar Event Handlers|1\.\s+Veraltete Events|Arbeiten mit Aufträgen über Subscriptions)\s*$", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var lines = ReadAllText(file).Replace("\r\n", "\n").Split('\n');
				for (var i = 0; i < lines.Length; i++)
				{
					if (forbiddenHeadingPattern.IsMatch(lines[i]))
						errors.Add($"{RelativeToRepo(file)}:{i + 1}: localized API heading keeps a known English label.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedAiStrategyPromptDoesNotKeepEnglishFastSlowTerms()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bfast\b|\bslow\b|\bperiod\s+\d+\b", RegexOptions.CultureInvariant);
		const string relativePath = "topics/api/ai_development/strategy_with_ai.md";

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			var lines = ReadAllText(file).Replace("\r\n", "\n").Split('\n');
			for (var i = 0; i < lines.Length; i++)
			{
				if (pattern.IsMatch(lines[i]))
					errors.Add($"{RelativeToRepo(file)}:{i + 1}: localized AI strategy prompt keeps English fast/slow/period wording.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedFastProtocolDocsDoNotKeepEnglishProtocolLinkLabel()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\[FAST protocol\]", RegexOptions.CultureInvariant);
		const string relativePath = "topics/api/connectors/common/fast_protocol.md";

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			var markdown = ReadAllText(file);
			if (pattern.IsMatch(markdown))
				errors.Add($"{RelativeToRepo(file)}: localized FAST protocol documentation keeps English link label '[FAST protocol]'.");
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedSaveLoadHeadingsDoNotKeepEnglishLabels()
	{
		var errors = new List<string>();
		var headingPattern = new Regex(@"^#{2,6}\s+.*\b(?:Save|Load)\b", RegexOptions.CultureInvariant);
		var relativePaths = new[]
		{
			"topics/api/indicators/custom_indicator.md",
			"topics/api/strategies/compatibility.md",
		};

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
				if (!File.Exists(file))
					continue;

				var lines = ReadAllText(file).Replace("\r\n", "\n").Split('\n');
				for (var i = 0; i < lines.Length; i++)
				{
					if (!headingPattern.IsMatch(lines[i]))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{i + 1}: localized heading keeps English Save/Load label.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedHighLevelSubscriptionDocsDoNotKeepEnglishStartStopHeading()
	{
		var errors = new List<string>();
		var headingPattern = new Regex(@"^#{2,6}\s+Start\s*/\s*Stop\s*$", RegexOptions.CultureInvariant);
		const string relativePath = "topics/api/strategies/high_level_subscriptions.md";

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			var lines = ReadAllText(file).Replace("\r\n", "\n").Split('\n');
			for (var i = 0; i < lines.Length; i++)
			{
				if (!headingPattern.IsMatch(lines[i]))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{i + 1}: localized subscription documentation keeps English Start/Stop heading.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedTradingModePermissionTableDoesNotKeepEnglishLabels()
	{
		var errors = new List<string>();
		const string relativePath = "topics/api/strategies/trading_modes.md";
		var englishYesPattern = new Regex(@"^\|\s*`(?:Full|CancelOrdersOnly|ReducePositionOnly|LongOnly)`\s*\|.*\byes\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			var lines = ReadAllText(file).Replace("\r\n", "\n").Split('\n');
			for (var i = 0; i < lines.Length; i++)
			{
				var line = lines[i];
				if (line.Contains("| Current TradingMode \\ required |", StringComparison.Ordinal))
					errors.Add($"{RelativeToRepo(file)}:{i + 1}: localized trading-mode table keeps English Current/required header.");

				if (englishYesPattern.IsMatch(line))
					errors.Add($"{RelativeToRepo(file)}:{i + 1}: localized trading-mode permission table keeps English 'yes' value.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedConnectorConfigurationDoesNotKeepEnglishPlaceholders()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"<Path to certificate file>|\*\*Active\*\*", RegexOptions.CultureInvariant);

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var root = Path.Combine(_repoRoot, lang, "topics", "api", "connectors");

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					if (pattern.IsMatch(text))
						errors.Add($"{RelativeToRepo(file)}:{line}: localized connector configuration keeps an English placeholder or UI label.");
				}

				foreach (var literal in EnumerateCodeStringLiterals(markdown))
				{
					if (pattern.IsMatch(literal.Text))
						errors.Add($"{RelativeToRepo(file)}:{literal.Line}: localized connector code string keeps an English placeholder.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDocsDoNotKeepEnglishCommissionExamples()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bbrokerage\b|\bexchange,\s*etc\.", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var root = Path.Combine(_repoRoot, lang, "topics");

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
						errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps an English commission example.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDocsDoNotKeepEnglishWrapperTerm()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bwrapper\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var root = Path.Combine(_repoRoot, lang, "topics");

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
						errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps English 'wrapper' wording.");
				}

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(altText.Text)))
						errors.Add($"{RelativeToRepo(file)}:{altText.Line}: localized image alt text keeps English 'wrapper' wording.");
				}

				foreach (var comment in EnumerateCodeComments(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(comment.Text)))
						errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized code comment keeps English 'wrapper' wording.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDocsDoNotKeepEnglishBacktestingTerm()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bbacktesting\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var root = Path.Combine(_repoRoot, lang, "topics");

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
						errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps English 'backtesting' wording.");
				}

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(altText.Text)))
						errors.Add($"{RelativeToRepo(file)}:{altText.Line}: localized image alt text keeps English 'backtesting' wording.");
				}

				foreach (var comment in EnumerateCodeComments(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(comment.Text)))
						errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized code comment keeps English 'backtesting' wording.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDocsDoNotKeepEnglishBacktestTerm()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bbacktests?\b|\bbacktester\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var root = Path.Combine(_repoRoot, lang, "topics");

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
						errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps English 'backtest' wording.");
				}

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(altText.Text)))
						errors.Add($"{RelativeToRepo(file)}:{altText.Line}: localized image alt text keeps English 'backtest' wording.");
				}

				foreach (var comment in EnumerateCodeComments(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(comment.Text)))
						errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized code comment keeps English 'backtest' wording.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDocsDoNotKeepEnglishPlaceholderTerm()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bplaceholders?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetContentLanguages().Where(lang => lang != DefaultLanguage && lang != "ru"))
		{
			var root = Path.Combine(_repoRoot, lang, "topics");

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
						errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps English placeholder wording.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanSpanishPortugueseDesignerDocsDoNotKeepEnglishSocketTerm()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bsockets?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in new[] { "de", "es", "pt" })
		{
			var root = Path.Combine(_repoRoot, lang, "topics", "designer");

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
						errors.Add($"{RelativeToRepo(file)}:{line}: localized Designer documentation keeps English 'socket' wording.");
				}

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(altText.Text)))
						errors.Add($"{RelativeToRepo(file)}:{altText.Line}: localized Designer image alt text keeps English 'socket' wording.");
				}

				foreach (var comment in EnumerateCodeComments(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(comment.Text)))
						errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized Designer code comment keeps English 'socket' wording.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanPortugueseDesignerDocsDoNotKeepEnglishTriggerTerm()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\btriggers?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in new[] { "de", "pt" })
		{
			var root = Path.Combine(_repoRoot, lang, "topics", "designer");

			foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
						errors.Add($"{RelativeToRepo(file)}:{line}: localized Designer documentation keeps English 'trigger' wording.");
				}

				foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(altText.Text)))
						errors.Add($"{RelativeToRepo(file)}:{altText.Line}: localized Designer image alt text keeps English 'trigger' wording.");
				}

				foreach (var comment in EnumerateCodeComments(markdown))
				{
					if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(comment.Text)))
						errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized Designer code comment keeps English 'trigger' wording.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void PortugueseDesignerDocsDoNotKeepEnglishFlagTerm()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bflag\b", RegexOptions.CultureInvariant);
		var root = Path.Combine(_repoRoot, "pt", "topics", "designer");

		foreach (var file in Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var markdown = ReadAllText(file);

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
					errors.Add($"{RelativeToRepo(file)}:{line}: Portuguese Designer documentation keeps English lowercase 'flag' wording.");
			}

			foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(altText.Text)))
					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: Portuguese Designer image alt text keeps English lowercase 'flag' wording.");
			}

			foreach (var comment in EnumerateCodeComments(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(comment.Text)))
					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: Portuguese Designer code comment keeps English lowercase 'flag' wording.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanDocsDoNotKeepEnglishTestingTerm()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bTesting\b", RegexOptions.CultureInvariant);
		var germanRoot = Path.Combine(_repoRoot, "de", "topics");

		foreach (var file in Directory.EnumerateFiles(germanRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var markdown = ReadAllText(file);

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForGermanTestingTermCheck(text)))
					errors.Add($"{RelativeToRepo(file)}:{line}: German documentation keeps English 'Testing' wording. Use 'Tests' or 'Testen' outside API identifiers and sample paths.");
			}

			foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForGermanTestingTermCheck(altText.Text)))
					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: German image alt text keeps English 'Testing' wording. Use localized wording.");
			}

			foreach (var comment in EnumerateCodeComments(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForGermanTestingTermCheck(comment.Text)))
					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: German code comment keeps English 'Testing' wording. Use localized wording.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanDocsDoNotKeepEnglishSubscriptionAndEventProse()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:Subscriptions|Events)\b|\bEvent-Handlers?\b|\bSubscription(?:s)?\s+(?:auf|starten)|\b(?:unsere|unserer|unserem|eine|einer)\s+Subscription\b|\bSubscription-(?:Typ|Objekt)\b", RegexOptions.CultureInvariant);
		var germanRoot = Path.Combine(_repoRoot, "de", "topics");

		foreach (var file in Directory.EnumerateFiles(germanRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var markdown = ReadAllText(file);

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
			{
				var withoutApiReferences = Regex.Replace(text, @"`[^`]*`|\[[^\]]+\]\([^)]*\)", " ", RegexOptions.CultureInvariant);
				if (pattern.IsMatch(withoutApiReferences))
					errors.Add($"{RelativeToRepo(file)}:{line}: German documentation keeps English subscription/event prose. Use 'Abonnement' or 'Ereignis' wording outside API identifiers.");
			}

			foreach (var comment in EnumerateCodeComments(markdown))
			{
				if (pattern.IsMatch(comment.Text))
					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: German code comment keeps English subscription/event prose. Use localized wording outside identifiers.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanDocsDoNotKeepEnglishTradingConceptTerms()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bMoving-Average\b|\bRange-Trading\b|\bMean-Reversion\b|\bPairs?[- ]Trading\b", RegexOptions.CultureInvariant);
		var germanRoot = Path.Combine(_repoRoot, "de", "topics");

		foreach (var file in Directory.EnumerateFiles(germanRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var markdown = ReadAllText(file);

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
					errors.Add($"{RelativeToRepo(file)}:{line}: German documentation keeps an English trading concept term. Use localized wording such as 'gleitender Durchschnitt', 'Rückkehr zum Mittelwert', 'Seitwärtshandel', or 'Paarhandel'.");
			}

			foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(altText.Text)))
					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: German image alt text keeps an English trading concept term.");
			}

			foreach (var comment in EnumerateCodeComments(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(comment.Text)))
					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: German code comment keeps an English trading concept term.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanDocsDoNotKeepEnglishLevelTerminology()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:High-Level|Low-Level)\b", RegexOptions.CultureInvariant);
		var germanRoot = Path.Combine(_repoRoot, "de", "topics");

		foreach (var file in Directory.EnumerateFiles(germanRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var markdown = ReadAllText(file);

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
					errors.Add($"{RelativeToRepo(file)}:{line}: German documentation keeps English High-Level/Low-Level wording. Use localized wording such as 'auf höherer Ebene' or 'auf niedriger Ebene'.");
			}

			foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(altText.Text)))
					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: German image alt text keeps English High-Level/Low-Level wording.");
			}

			foreach (var comment in EnumerateCodeComments(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(comment.Text)))
					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: German code comment keeps English High-Level/Low-Level wording.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanIndicatorDocsDoNotKeepEnglishHighLowLabels()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:High|Low)\s+(?:GAPO|HVR|KER|R²|MMI)\b", RegexOptions.CultureInvariant);
		var germanIndicatorRoot = Path.Combine(_repoRoot, "de", "topics", "api", "indicators");

		foreach (var file in Directory.EnumerateFiles(germanIndicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
					errors.Add($"{RelativeToRepo(file)}:{line}: German indicator documentation keeps English High/Low labels in prose. Use localized labels such as 'Hohe', 'Hoher', or 'Niedriger'.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanIndicatorDocsDoNotKeepEnglishPriceVolumeProse()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bFrequenzweichen\b|\b(?:Price|Volume)-(?:Änderung|Analyse|Serie)\b|\*\*Price\*\*|\bPrice\s+(?:über|unter)|\b(?:Tatsächlicher|Vorhergesagter)\s+Price\b|\bPrice,\s+das\s+MGD\b|\b(?:Short|Long)\s+MGD\b", RegexOptions.CultureInvariant);
		var germanIndicatorRoot = Path.Combine(_repoRoot, "de", "topics", "api", "indicators");

		foreach (var file in Directory.EnumerateFiles(germanIndicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var withoutCode = Regex.Replace(text, @"`[^`]*`", " ", RegexOptions.CultureInvariant);
				var match = pattern.Match(withoutCode);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: German indicator documentation keeps English price/volume prose fragment '{match.Value}'. Localize visible prose while preserving formula variables.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanDocsDoNotKeepEnglishOrderLogTerm()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bOrder-Logs?\b|\bOrderlogs?\b", RegexOptions.CultureInvariant);
		var germanRoot = Path.Combine(_repoRoot, "de", "topics");

		foreach (var file in Directory.EnumerateFiles(germanRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var withoutCode = Regex.Replace(text, @"`[^`]*`", " ", RegexOptions.CultureInvariant);
				if (pattern.IsMatch(withoutCode))
					errors.Add($"{RelativeToRepo(file)}:{line}: German documentation keeps English order-log wording. Use 'Orderprotokoll' in visible text.");
			}

			foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
			{
				if (pattern.IsMatch(comment.Text))
					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: German code comment keeps English order-log wording. Use 'Orderprotokoll' in explanatory comments.");
			}
		}

		foreach (var nameLine in EnumerateTocNameLines(Path.Combine(germanRoot, "toc.yml")))
		{
			if (pattern.IsMatch(nameLine.Name))
				errors.Add($"{RelativeToRepo(Path.Combine(germanRoot, "toc.yml"))}:{nameLine.Line}: German TOC keeps English order-log wording. Use 'Orderprotokoll'.");
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanDocsDoNotKeepEnglishBoardCodeAndOrderBookHyphenTerms()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bBoard-Codes?\b|\bOrder-Book\b", RegexOptions.CultureInvariant);
		var germanRoot = Path.Combine(_repoRoot, "de", "topics");

		foreach (var file in Directory.EnumerateFiles(germanRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var withoutCode = Regex.Replace(text, @"`[^`]*`", " ", RegexOptions.CultureInvariant);
				if (pattern.IsMatch(withoutCode))
					errors.Add($"{RelativeToRepo(file)}:{line}: German documentation keeps English hyphenated board/order-book wording. Use localized visible terminology.");
			}

			foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
			{
				if (pattern.IsMatch(comment.Text))
					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: German code comment keeps English hyphenated board/order-book wording. Use localized explanatory comments.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanDocsDoNotKeepBareDesignerPreposition()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:in|aus) Designer\b", RegexOptions.CultureInvariant);
		var germanRoot = Path.Combine(_repoRoot, "de", "topics");

		foreach (var file in Directory.EnumerateFiles(germanRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var markdown = ReadAllText(file);

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
			{
				var normalized = Regex.Replace(text, @"\[(?<label>[^\]]+)\]\([^)]+\)", "${label}", RegexOptions.CultureInvariant);
				if (pattern.IsMatch(normalized))
					errors.Add($"{RelativeToRepo(file)}:{line}: German documentation uses a bare preposition before Designer. Use 'im Designer' or 'aus dem Designer'.");
			}

			foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
			{
				if (pattern.IsMatch(altText.Text))
					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: German image alt text uses a bare preposition before Designer. Use 'im Designer' or 'aus dem Designer'.");
			}

			foreach (var comment in EnumerateCodeComments(markdown))
			{
				if (pattern.IsMatch(comment.Text))
					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: German code comment uses a bare preposition before Designer. Use 'im Designer' or 'aus dem Designer'.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanDocsDoNotKeepEnglishLoggingPhrases()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:Strategie-Logging|IConnector-Logging|Andere Logquellen|Logging-Beispiel|Logging in|Logging-Stufen?|Logging-Methoden|Logging-Mechanismus|Logging-Einstellungen|Logging-Nachrichten|Logging-Level|Detailliertes Logging|detailliertes Logging|Logquellen?|Loglisteners?|Log-Listeners?|Logger|Listeners?|Logs|Log-Panel|ILogListener creating|Visuelle Logging-Komponenten|Um Logging|Logs in MainWindow|für das Logging|für Logging|und Logging|Streaming-Synchronisierung)\b", RegexOptions.CultureInvariant);
		var germanRoot = Path.Combine(_repoRoot, "de", "topics");

		foreach (var file in Directory.EnumerateFiles(germanRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var markdown = ReadAllText(file);

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(text)))
					errors.Add($"{RelativeToRepo(file)}:{line}: German documentation keeps English logging wording. Use 'Protokollierung' in visible prose.");
			}

			foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(altText.Text)))
					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: German image alt text keeps English logging wording. Use 'Protokollierung'.");
			}

			foreach (var comment in EnumerateCodeComments(markdown))
			{
				if (pattern.IsMatch(NormalizeMarkdownTextForLocalizedLoggingTermCheck(comment.Text)))
					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: German code comment keeps English logging wording. Use 'Protokollierung'.");
			}
		}

		foreach (var nameLine in EnumerateTocNameLines(Path.Combine(germanRoot, "toc.yml")))
		{
			if (pattern.IsMatch(nameLine.Name))
				errors.Add($"{RelativeToRepo(Path.Combine(germanRoot, "toc.yml"))}:{nameLine.Line}: German TOC keeps English logging wording. Use 'Protokollierung'.");
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedOandaGraphicalConfigurationDoesNotKeepEnglishFieldLabels()
	{
		var errors = new List<string>();
		var checks = new[]
		{
			(
				Lang: "de",
				Fragments: new[] { "**Compression**", "**Transactions only**", "Board-Code" }
			),
			(
				Lang: "es",
				Fragments: new[] { "**Server**", "**Compression**", "**Transactions only**", "Código de board" }
			),
			(
				Lang: "pt",
				Fragments: new[] { "**Server**", "**Compression**", "**Transactions only**", "Código de board" }
			),
		};

		foreach (var check in checks)
		{
			var file = Path.Combine(_repoRoot, check.Lang, "topics", "api", "connectors", "forex", "oanda", "graphical_configuration_oanda.md");
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				foreach (var fragment in check.Fragments)
				{
					if (text.Contains(fragment, StringComparison.Ordinal))
						errors.Add($"{RelativeToRepo(file)}:{line}: Oanda graphical configuration keeps English UI label '{fragment}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedHydraCredentialDocsDoNotKeepEnglishLoginText()
	{
		var errors = new List<string>();
		var checks = new[]
		{
			(
				Lang: "de",
				RelativePath: "topics/hydra/tasks/export_auto.md",
				Fragments: new[] { "**Login**", "Login für" }
			),
			(
				Lang: "de",
				RelativePath: "topics/hydra/server_mode/hydra_client_connectivity.md",
				Fragments: new[] { "- **Anmeldung** - Login" }
			),
			(
				Lang: "es",
				RelativePath: "topics/hydra/tasks/export_auto.md",
				Fragments: new[] { "login para" }
			),
			(
				Lang: "es",
				RelativePath: "topics/hydra/misc/backup/setup.md",
				Fragments: new[] { "como login" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/hydra/tasks/export_auto.md",
				Fragments: new[] { "**Login**", "login para" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/hydra/server_mode/hydra_client_connectivity.md",
				Fragments: new[] { "- **Início de sessão** - login" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/hydra/misc/backup/setup.md",
				Fragments: new[] { "como login" }
			),
		};

		foreach (var check in checks)
		{
			var file = Path.Combine(_repoRoot, check.Lang, check.RelativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				foreach (var fragment in check.Fragments)
				{
					if (!text.Contains(fragment, StringComparison.OrdinalIgnoreCase))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized Hydra credential docs keep English login text '{fragment}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedVisualStudioDllDebugDocsDoNotKeepEnglishAttachLabels()
	{
		var errors = new List<string>();
		var relativePath = Path.Combine("topics", "designer", "strategies", "using_dll", "debug_dll_in_visual_studio.md");
		var phrases = new[]
		{
			"Debug -> Attach to Process",
			"Feld Attach to",
			"campo Attach to",
			"Attach to フィールド",
			"Attach to 字段",
			"Schaltfläche Attach",
			"botón Attach",
			"botão Attach",
			"Attach ボタン",
			"Attach 按钮",
		};

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				foreach (var phrase in phrases)
				{
					if (!text.Contains(phrase, StringComparison.Ordinal))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized Visual Studio DLL debugging documentation keeps English attach UI label '{phrase}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedTradierOAuthDocsDoNotKeepEnglishPermissionsImageAltText()
	{
		var errors = new List<string>();
		var relativePath = Path.Combine("topics", "api", "connectors", "stock_market", "tradier", "graphical_configuration_tradier.md");

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath);
			if (!File.Exists(file))
				continue;

			foreach (var altText in EnumerateMarkdownImageAltTexts(ReadAllText(file)))
			{
				if (!altText.Text.Equals("Tradier Permissions", StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{altText.Line}: localized Tradier OAuth documentation keeps the English image alt text 'Tradier Permissions'.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedHydraImageAltTextsDoNotKeepEnglishExportAndMenuWords()
	{
		var errors = new List<string>();
		var checks = new[]
		{
			(
				Lang: "ja",
				RelativePath: Path.Combine("topics", "hydra", "working_with_data", "view_and_export", "transactions.md"),
				Fragments: new[] { "executions" }
			),
			(
				Lang: "zh",
				RelativePath: Path.Combine("topics", "hydra", "working_with_data", "view_and_export", "transactions.md"),
				Fragments: new[] { "executions" }
			),
			(
				Lang: "ja",
				RelativePath: Path.Combine("topics", "hydra", "working_with_data", "view_and_export", "news.md"),
				Fragments: new[] { "news" }
			),
			(
				Lang: "zh",
				RelativePath: Path.Combine("topics", "hydra", "working_with_data", "view_and_export", "news.md"),
				Fragments: new[] { "news" }
			),
			(
				Lang: "ja",
				RelativePath: Path.Combine("topics", "hydra", "server_mode", "settings.md"),
				Fragments: new[] { "menu" }
			),
			(
				Lang: "zh",
				RelativePath: Path.Combine("topics", "hydra", "server_mode", "settings.md"),
				Fragments: new[] { "menu" }
			),
		};

		foreach (var check in checks)
		{
			var file = Path.Combine(_repoRoot, check.Lang, check.RelativePath);
			if (!File.Exists(file))
				continue;

			foreach (var altText in EnumerateMarkdownImageAltTexts(ReadAllText(file)))
			{
				foreach (var fragment in check.Fragments)
				{
					if (!altText.Text.Contains(fragment, StringComparison.OrdinalIgnoreCase))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: image alt text keeps English word '{fragment}'. Localize the visible image description.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void RussianAlorOAuthDocsDoNotKeepEnglishLoginAndPermissionsImageAltText()
	{
		var errors = new List<string>();
		var file = Path.Combine(_repoRoot, "ru", "topics", "api", "connectors", "russia", "alor", "ui_settings.md");
		var phrases = new[] { "Alor Login", "Alor Permissions" };

		foreach (var altText in EnumerateMarkdownImageAltTexts(ReadAllText(file)))
		{
			foreach (var phrase in phrases)
			{
				if (!altText.Text.Equals(phrase, StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{altText.Line}: Russian Alor OAuth documentation keeps the English image alt text '{phrase}'.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void RussianMarkdownDoesNotKeepKnownEnglishNavigationFragments()
	{
		var errors = new List<string>();
		var fragments = new[]
		{
			"Патерн",
			"Логирование Strategy",
			"Графики box chart",
			"3 Black Crows",
			"Outside Down",
			"Outside Up",
		};
		var russianRoot = Path.Combine(_repoRoot, "ru", "topics");

		foreach (var file in Directory.EnumerateFiles(russianRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var inFence = false;
			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				var trimmed = text.Trim();
				if (Regex.IsMatch(trimmed, @"^(```|~~~)", RegexOptions.CultureInvariant))
				{
					inFence = !inFence;
					continue;
				}

				if (inFence)
					continue;

				foreach (var fragment in fragments)
				{
					if (!text.Contains(fragment, StringComparison.OrdinalIgnoreCase))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: Russian markdown keeps English/navigation fragment '{fragment}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedImageAltTextsDoNotKeepKnownMixedEnglishFragments()
	{
		var errors = new List<string>();
		var startPattern = new Regex(@"\bstart\b", RegexOptions.CultureInvariant);
		var mixedDesignerPattern = new Regex(@"\bin Designer\b", RegexOptions.CultureInvariant);
		var portuguesePattern = new Regex(@"\bLogin\s+(?:cTrader|Sterling|Tradier)\b|^(?:download|login)\b", RegexOptions.CultureInvariant);
		var nonPortugueseVolumePattern = new Regex(@"\bvolume\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var altText in EnumerateMarkdownImageAltTexts(ReadAllText(file)))
				{
					if (startPattern.IsMatch(altText.Text))
					{
						errors.Add($"{RelativeToRepo(file)}:{altText.Line}: image alt text keeps lowercase English 'start' fragment '{altText.Text}'. Localize the screenshot description.");
						continue;
					}

					if ((lang == "es" || lang == "pt" || lang == "zh") && mixedDesignerPattern.IsMatch(altText.Text))
					{
						errors.Add($"{RelativeToRepo(file)}:{altText.Line}: image alt text mixes a localized label with English 'in Designer': '{altText.Text}'. Localize the relationship wording.");
						continue;
					}

					if (lang == "pt" && portuguesePattern.IsMatch(altText.Text))
						errors.Add($"{RelativeToRepo(file)}:{altText.Line}: Portuguese image alt text keeps an English login/download label '{altText.Text}'. Localize the visible description.");

					if (lang != "pt" && nonPortugueseVolumePattern.IsMatch(altText.Text))
						errors.Add($"{RelativeToRepo(file)}:{altText.Line}: image alt text keeps lowercase English 'volume' fragment '{altText.Text}'. Localize the screenshot description.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void BlackScholesImageAltTextUsesModelName()
	{
		var errors = new List<string>();
		const string relativePath = "topics/designer/strategies/using_visual_designer/elements/options/black_scholes.md";
		const string badAltText = "Designer Black Sols 00";

		foreach (var lang in GetContentLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			foreach (var altText in EnumerateMarkdownImageAltTexts(ReadAllText(file)))
			{
				if (altText.Text.Equals(badAltText, StringComparison.Ordinal))
					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: Black-Scholes screenshot alt text keeps misspelled model name '{badAltText}'.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void SpanishHydraVideoDocsDoNotKeepEnglishImportExportHeadings()
	{
		var errors = new List<string>();
		var relativeRoot = Path.Combine("topics", "hydra", "videos");
		var pattern = new Regex(@"^#\s+Tarea\s+(?:Import|Export)\s*$", RegexOptions.CultureInvariant);

		foreach (var file in Directory.EnumerateFiles(Path.Combine(_repoRoot, "es", relativeRoot), "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				if (!pattern.IsMatch(text))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Spanish Hydra video heading keeps an English Import/Export task label '{text}'. Localize the task title.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedAroonDocsDoNotKeepEnglishCalculationLabels()
	{
		var errors = new List<string>();
		var relativePath = Path.Combine("topics", "api", "indicators", "list_of_indicators", "aroon.md");
		var phrases = new[] { "Periods since high", "Periods since low" };

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath);
			if (!File.Exists(file))
				continue;

			var markdown = ReadAllText(file);
			foreach (var phrase in phrases)
			{
				if (!markdown.Contains(phrase, StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}: localized Aroon documentation keeps the English calculation label '{phrase}'. Localize the formula label and its explanation.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedBackupDocsDoNotKeepEnglishBackupTaskLabel()
	{
		var errors = new List<string>();
		var relativePath = Path.Combine("topics", "hydra", "misc", "backup.md");

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				if (!text.Contains("Backup task", StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: localized backup documentation keeps the English label 'Backup task'. Localize the visible task label.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedHydraAnalyticsSamplesDoNotKeepEnglishDiagnosticMessages()
	{
		var errors = new List<string>();
		var phrases = new[] { "No instruments.", "no data" };

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var analyticsRoot = Path.Combine(_repoRoot, lang, "topics", "hydra", "analytics");
			if (!Directory.Exists(analyticsRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(analyticsRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
				{
					foreach (var phrase in phrases)
					{
						if (!literal.Text.Equals(phrase, StringComparison.Ordinal))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{literal.Line}: localized Hydra analytics sample keeps the English diagnostic string '{phrase}'. Localize the sample string literal.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedHydraAnalyticsSamplesDoNotKeepEnglishChartLabels()
	{
		var errors = new List<string>();
		var phrases = new[] { "\"prices\"", "\"Instruments\"", "\"Hours\"", "\"Time\"", "\"Volume\"", "(close)", "(vol)" };

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var analyticsRoot = Path.Combine(_repoRoot, lang, "topics", "hydra", "analytics");
			if (!Directory.Exists(analyticsRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(analyticsRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var phrase in phrases)
				{
					if (!markdown.Contains(phrase, StringComparison.Ordinal))
						continue;

					errors.Add($"{RelativeToRepo(file)}: localized Hydra analytics sample keeps the English chart label {phrase}. Localize visible chart and grid labels.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedHydraAnalyticsIndicatorDocsDoNotKeepEnglishSampleScriptName()
	{
		var errors = new List<string>();
		var relativePath = Path.Combine("topics", "hydra", "analytics", "examples", "indicators.md");

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				if (!text.Contains("Indicator", StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: localized Hydra analytics indicator documentation keeps the English sample script name 'Indicator'.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanHydraAnalyticsDocsDoNotKeepKnownAsciiUmlautFallbacks()
	{
		var errors = new List<string>();
		var analyticsRoot = Path.Combine(_repoRoot, "de", "topics", "hydra", "analytics");
		var phrases = new[]
		{
			"Grosste",
			"ausgewahl",
			"nutzlich",
			"Einschatzung",
			"Starke",
			"Abschwachung",
			"Anderung",
			"Handelsaktivitat",
			"verandert",
			"lasst",
			"Trendanderungen",
			"Unterstutzungs",
			"erhohter",
			"Liquiditat",
			"leistungsfahig",
			"Berucksichtigung",
			"Funktionalitat",
			"Zeitraume",
			"Oberflach",
			"einschliesslich",
			"Marktaktivitat",
			"Traderaktivitat",
			"hochsten",
			"stutzen",
		};

		foreach (var file in Directory.EnumerateFiles(analyticsRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				foreach (var phrase in phrases)
				{
					if (!text.Contains(phrase, StringComparison.Ordinal))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: German Hydra analytics documentation keeps ASCII umlaut fallback '{phrase}'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanDocsDoNotKeepKnownAsciiUmlautFallbacks()
	{
		var errors = new List<string>();
		var germanRoot = Path.Combine(_repoRoot, "de");
		var fragments = new[]
		{
			"ausserdem",
			"einschliesslich",
			"einschliessen",
			"schliessen",
			"Anschliessend",
			"anschliessende",
			"Ubernimmt",
			"ubergebene",
			"Befullen",
			"befullen",
			"Eroffnungs",
			"Eroffnungszeit",
			"Verstandnis",
			"daruber",
			"eroffnet",
			"Abhangigkeit",
			"Kurzung",
			"einfugen",
			"hinzufugen",
			"Prufen",
			"Ausmass",
			"Preisanderungen",
			"uberkaufte",
			"uberverkaufte",
			"nachste",
			"gekurzt",
			"ubergeordnete",
			"Fensteroberflache",
			"ahnlich",
			"gedruckt",
			"Randern",
			"reprasentieren",
			"Funktionalitat",
			"Qualitat",
			"Wahrung",
			"gewahlten",
			"glatten",
			"Kerzenkorperlange",
			"Fugen",
			"Positionsgrosse",
			"Haufigkeit",
			"Hohe von",
			"Hohe der",
			"Hohe des",
			"Hohe oder",
			"uberwacht",
		};

		foreach (var file in Directory.EnumerateFiles(germanRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var markdown = ReadAllText(file);

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				AddAsciiFallbackErrors(file, line, text, "visible text");

			foreach (var comment in EnumerateCodeComments(markdown))
				AddAsciiFallbackErrors(file, comment.Line, comment.Text, "code comment");
		}

		AssertNoErrors(errors);

		void AddAsciiFallbackErrors(string file, int line, string text, string source)
		{
			foreach (var fragment in fragments)
			{
				if (!text.Contains(fragment, StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: German {source} keeps ASCII umlaut fallback '{fragment}'.");
			}
		}
	}

	[TestMethod]
	public void PortugueseHydraAnalyticsDocsDoNotKeepEnglishSecurityTerms()
	{
		var errors = new List<string>();
		var analyticsRoot = Path.Combine(_repoRoot, "pt", "topics", "hydra", "analytics");
		var pattern = new Regex(@"\bsecurit(?:y|ies)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var file in Directory.EnumerateFiles(analyticsRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var markdown = ReadAllText(file);

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
			{
				var textWithoutInlineCode = Regex.Replace(text, @"`[^`\r\n]*`", " ", RegexOptions.CultureInvariant);
				var match = pattern.Match(textWithoutInlineCode);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Portuguese Hydra analytics documentation keeps English term '{match.Value}' in visible text.");
			}

			foreach (var comment in EnumerateCodeComments(markdown))
			{
				var match = pattern.Match(comment.Text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{comment.Line}: Portuguese Hydra analytics code comment keeps English term '{match.Value}'.");
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

		foreach (var lang in GetLocalizedContentQualityLanguages())
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
	public void LocalizedStairsCountertrendQuotingLogsDoNotKeepEnglishCandleMessages()
	{
		var errors = new List<string>();
		var relativePath = Path.Combine("topics", "api", "strategies", "samples", "stairs_countertrend_quoting.md");
		var pattern = new Regex(@"\b(?:Bullish|Bearish) candle detected\. Streak:", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath);
			if (!File.Exists(file))
				continue;

			foreach (var output in EnumerateCodeOutputStrings(ReadAllText(file)))
			{
				if (!pattern.IsMatch(output.Text))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{output.Line}: localized Stairs countertrend quoting log keeps English candle message '{output.Text}'. Localize the log string.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedTradeExecutionLogsDoNotUseEnglishAtPriceSeparator()
	{
		var errors = new List<string>();
		var relativePaths = new[]
		{
			Path.Combine("topics", "api", "strategies", "samples", "stairs_countertrend_quoting.md"),
			Path.Combine("topics", "api", "strategies", "samples", "mq_spread.md"),
			Path.Combine("topics", "api", "strategies", "samples", "mq.md"),
			Path.Combine("topics", "api", "strategies", "quoting.md"),
		};
		var pattern = new Regex(@"\bat\s+\{trade\.Trade\.Price\}", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(_repoRoot, lang, relativePath);
				if (!File.Exists(file))
					continue;

				var markdown = ReadAllText(file);
				foreach (Match match in pattern.Matches(markdown))
					errors.Add($"{RelativeToRepo(file)}:{GetLineNumber(GetLineStarts(markdown), match.Index)}: localized trade execution log keeps English price separator 'at'. Localize the log string.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedLoggingSamplesDoNotKeepEnglishDemoMessages()
	{
		var errors = new List<string>();
		var relativePaths = new[]
		{
			Path.Combine("topics", "api", "graphical_user_interface", "logging", "log_panel.md"),
			Path.Combine("topics", "api", "logging", "other_logs_sources.md"),
		};
		var patterns = new[]
		{
			new Regex(@"\bInfo\s+test message\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
			new Regex(@"\bWarning test message\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
			new Regex(@"\bError test message\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
			new Regex(@"^(?:\{0\}\s+)?\(source\)!!!$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
			new Regex(@"^(?:\{0\}\s+)?\(trace\)!!!$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
			new Regex(@"\b(?:Warning|Error)\s+\((?:source|trace)\)!!!", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant),
		};

		foreach (var lang in GetTranslatedContentLanguages())
		{
			foreach (var relativePath in relativePaths)
			{
				var file = Path.Combine(_repoRoot, lang, relativePath);
				if (!File.Exists(file))
					continue;

				foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
				{
					foreach (var pattern in patterns)
					{
						if (!pattern.IsMatch(literal.Text))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{literal.Line}: localized logging sample keeps English demo log text '{literal.Text}'. Localize user-visible log strings.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCandleChartDocsDoNotKeepEnglishButtonLabels()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "api", "graphical_user_interface", "charts", "candle_chart.md");
		var labels = new HashSet<string>(StringComparer.Ordinal)
		{
			"Connect",
			"ShowChart",
		};

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);
			if (!File.Exists(file))
				continue;

			foreach (var label in EnumerateMarkdownBoldTexts(ReadAllText(file)))
			{
				if (!labels.Contains(label.Text))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{label.Line}: localized candle-chart docs keep English button label '{label.Text}'. Localize the visible button text.");
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
	public void LocalizedCodeCommentsDoNotKeepKnownEnglishChartTerms()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bopen/close\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
				{
					var match = pattern.Match(comment.Text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: code comment keeps English chart term '{match.Value}'. Localize the comment.");
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
	public void LocalizedCjkMarkdownImageAltTextsDoNotKeepKnownEnglishUiWords()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bCircuits\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages().Where(_cjkLanguageCodes.Contains))
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var altText in EnumerateMarkdownImageAltTexts(ReadAllText(file)))
				{
					var match = pattern.Match(altText.Text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: CJK image alt text '{altText.Text}' keeps the English UI word '{match.Value}'. Localize the UI description.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownImageAltTextsDoNotKeepRawHydraImportPrefix()
	{
		var errors = new List<string>();

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var altText in EnumerateMarkdownImageAltTexts(ReadAllText(file)))
				{
					if (!altText.Text.StartsWith("hydra import", StringComparison.Ordinal))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: image alt text '{altText.Text}' keeps the raw 'hydra import' filename prefix. Use a localized Hydra import description.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedMarkdownImageAltTextsUseCanonicalProductCasing()
	{
		var errors = new List<string>();
		var rawPrefixes = new[] { "hydra", "multiconnection", "ib", "etrade" };

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var altText in EnumerateMarkdownImageAltTexts(ReadAllText(file)))
				{
					var rawPrefix = rawPrefixes.FirstOrDefault(prefix => altText.Text.StartsWith($"{prefix} ", StringComparison.Ordinal));
					if (rawPrefix is null)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{altText.Line}: image alt text '{altText.Text}' starts with raw product prefix '{rawPrefix}'. Use canonical product casing and a localized description.");
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
	public void LocalizedConnectorGraphicalConfigsDoNotKeepEnglishFieldDescriptions()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"-\s+\*\*[^*\r\n]+\*\*\s*(?:-|–|—)\s*(?:Client ID|Client-ID|Websocket ID|WebSocket ID|RequestWithdrawAccounts|IsPrime|(?:API-)?Secret)[.。]?\s*$|-\s+\*\*Board\*\*\s*(?:-|–|—)", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var connectorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "connectors");

			if (!Directory.Exists(connectorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(connectorRoot, "graphical_configuration*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var line = 1;
				using var reader = new StringReader(ReadAllText(file));

				for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: connector graphical configuration keeps English field description '{match.Value}'. Localize the visible field description.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedTradingSideDocsDoNotKeepEnglishBuySellPairs()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b[Bb]uy\s*/\s*[Ss]ell\b|\b[Bb]uy\s+(?:or|oder|e|y|または|或|或者)\s+[Ss]ell\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps English trading side pair '{match.Value}'. Localize visible buy/sell wording.");
				}

				foreach (var comment in EnumerateCodeComments(markdown))
				{
					var match = pattern.Match(comment.Text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized code comment keeps English trading side pair '{match.Value}'. Localize visible buy/sell wording.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedOwnTradeDocsDoNotKeepEnglishLabels()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b[Oo]wn trades?\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps English own-trade label '{match.Value}'. Localize the visible own-trade wording.");
				}

				foreach (var comment in EnumerateCodeComments(markdown))
				{
					var match = pattern.Match(comment.Text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized code comment keeps English own-trade label '{match.Value}'. Localize the own-trade wording.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCodeCommentsDoNotKeepShortEnglishLabels()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"^\s*(?:Start|Stop|Saving|Loading|Counter|Clear|Connect|Disconnect|Chart setup|Tick trades|Orders|Positions|Portfolios|Securities|Trades|Candles|Indicators|Timer|Subscribe|Unsubscribe)\s*$", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
				{
					var match = pattern.Match(comment.Text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized code comment keeps short English label '{match.Value}'. Localize the comment label.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDesignerPositionModifyDocsDoNotKeepEnglishOperationLabels()
	{
		var errors = new List<string>();
		var fileParts = new[] { "topics", "designer", "strategies", "using_visual_designer", "elements", "positions", "modify.md" };
		var pattern = new Regex(@"\b(?:None|OpenPosition|ClosePosition|Decrease|Increase|Reverse|Close Position|Market Order)\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var file = Path.Combine(new[] { _repoRoot, lang }.Concat(fileParts).ToArray());
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				foreach (Match match in pattern.Matches(text))
				{
					errors.Add($"{RelativeToRepo(file)}:{line}: localized Designer position-modify docs keep English operation label '{match.Value}'. Localize visible operation labels.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDesignerStatisticsDocsDoNotKeepEnglishCategoryLabels()
	{
		var errors = new List<string>();
		var files = new[]
		{
			Path.Combine("topics", "designer", "user_interface", "components.md"),
			Path.Combine("topics", "designer", "user_interface", "components", "statistics.md"),
		};
		var categoryLinePattern = new Regex(@"categor|Kategor|カテゴリ|类别|分類|分类", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		var englishLabelPattern = new Regex(@"\b(?:Trades|Positions|Orders)\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var relative in files)
			{
				var file = Path.Combine(langRoot, relative);
				if (!File.Exists(file))
					continue;

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					if (!categoryLinePattern.IsMatch(text))
						continue;

					foreach (Match match in englishLabelPattern.Matches(text))
					{
						errors.Add($"{RelativeToRepo(file)}:{line}: localized Designer statistics docs keep English category label '{match.Value}'. Localize visible result-category labels.");
					}
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDesignerResultDocsDoNotKeepEnglishResultComponentLabels()
	{
		var errors = new List<string>();
		var files = new[]
		{
			Path.Combine("topics", "designer", "backtesting", "getting_started.md"),
			Path.Combine("topics", "designer", "quick_start.md"),
		};
		var labelPattern = new Regex(@"\b(?:Chart|Orders|Trades|Positions|Statistics)\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var relative in files)
			{
				var file = Path.Combine(langRoot, relative);
				if (!File.Exists(file))
					continue;

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var matches = labelPattern.Matches(text);
					if (matches.Count < 2)
						continue;

					var values = string.Join(", ", matches.Cast<Match>().Select(match => match.Value).Distinct(StringComparer.Ordinal));
					errors.Add($"{RelativeToRepo(file)}:{line}: localized Designer result docs keep English result component labels '{values}'. Localize visible result-tab/table labels.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDesignerComponentsOverviewDoesNotKeepEnglishComponentLabels()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "designer", "user_interface", "components.md");
		var pattern = new Regex(@"\[(?:Source Code|Depth)\]\(", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var match = pattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: localized Designer components overview keeps English component link label '{match.Value}'. Localize the visible component label.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDesignerCurrentPositionSampleDoesNotKeepEnglishCubeLabels()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "designer", "strategies", "using_visual_designer", "schema_samples", "get_current_position.md");
		var pattern = new Regex(@"\b(?:Conditional operator|Conditional statement|Variable\s*模块)\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				foreach (Match match in pattern.Matches(text))
				{
					errors.Add($"{RelativeToRepo(file)}:{line}: localized Designer current-position sample keeps English cube label '{match.Value}'. Localize visible cube labels.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDesignerOptimizationDocsDoNotKeepEnglishChartTabLabel()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "designer", "optimization", "3d_chart.md");
		var pattern = new Regex(@"\bTab\s+Chart\b|\bpestaña\s+a\s+Chart\b|\bseparador\s+para\s+Chart\b|タブ[^\r\n]{0,12}\bChart\b|\bChart\s*选项卡", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var match = pattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: localized Designer optimization docs keep English Chart tab label. Localize the visible tab name.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCjkDesignerLiveExecutionDocsDoNotKeepEnglishTradeFolderLabel()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "designer", "live_execution", "getting_started.md");
		var pattern = new Regex(@"\bTrade\s*(?:フォルダー|文件夹)", RegexOptions.CultureInvariant);

		foreach (var lang in new[] { "ja", "zh" })
		{
			var file = Path.Combine(_repoRoot, lang, relative);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var match = pattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: CJK Designer live-execution docs keep English Trade folder label '{match.Value}'. Localize the visible folder name.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDesignerMaterialsDocsDoNotKeepEnglishStrategyGalleryLabel()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "designer", "materials.md");
		var pattern = new Regex(@"\bStrategy Gallery\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var match = pattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: localized Designer materials docs keep English Strategy Gallery label. Localize the visible gallery name.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDesignerStrategyGalleryDocsDoNotKeepEnglishDownloadButtonLabel()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "designer", "strategy_gallery.md");
		var pattern = new Regex(@"\bDownload\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var match = pattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: localized Designer strategy-gallery docs keep English Download button label. Localize the visible button text.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDesignerRibbonDocsDoNotKeepEnglishCloudTasksLabel()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "designer", "user_interface", "ribbon.md");
		var pattern = new Regex(@"\bCloud Tasks\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var match = pattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: localized Designer ribbon docs keep English Cloud Tasks label. Localize the visible cloud task label.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDesignerDocsDoNotKeepEnglishStrategyNamePlaceholder()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bStrategy Name\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var designerRoot = Path.Combine(_repoRoot, lang, "topics", "designer");
			if (!Directory.Exists(designerRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(designerRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized Designer docs keep English strategy-name placeholder '{match.Value}'. Localize the placeholder text.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedDesignerLiveExecutionDocsDoNotKeepEnglishLiveTabTitle()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "designer", "live_execution", "user_interface.md");
		var pattern = new Regex(@"\bLive\s*\[", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var match = pattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: localized Designer live-execution docs keep English Live tab title prefix. Localize the visible live tab title.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedLiveDocsDoNotKeepBareEnglishLiveLabels()
	{
		var errors = new List<string>();
		var checks = new[]
		{
			(
				Lang: "es",
				RelativePath: "topics/telegram_services/control_panel.md",
				Fragments: new[] { "modo Live" }
			),
			(
				Lang: "es",
				RelativePath: "topics/designer/user_interface/risk_management.md",
				Fragments: new[] { "Configuración live" }
			),
			(
				Lang: "es",
				RelativePath: "topics/designer/user_interface/components.md",
				Fragments: new[] { "Configuración live" }
			),
			(
				Lang: "es",
				RelativePath: "topics/designer/user_interface/components/live_settings.md",
				Fragments: new[] { "Configuración live", "configuración Live" }
			),
			(
				Lang: "es",
				RelativePath: "topics/api/testing/simulator.md",
				Fragments: new[] { "cotizaciones \"live\"" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/telegram_services/control_panel.md",
				Fragments: new[] { "modo Live" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/designer/live_execution/live_execution_sample.md",
				Fragments: new[] { "em Live" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/designer/live_execution/strategies_dashboard.md",
				Fragments: new[] { "em Live" }
			),
			(
				Lang: "pt",
				RelativePath: "topics/api/testing/simulator.md",
				Fragments: new[] { "cotações \"live\"" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/telegram_services/control_panel.md",
				Fragments: new[] { "Live モード" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/designer/user_interface/components.md",
				Fragments: new[] { "Live 設定" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/designer/user_interface/components/live_settings.md",
				Fragments: new[] { "Live 設定" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/designer/user_interface/components/backtesting_settings.md",
				Fragments: new[] { "Live 取引" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/designer/live_execution/live_execution_sample.md",
				Fragments: new[] { "Live 実行" }
			),
			(
				Lang: "ja",
				RelativePath: "topics/designer/live_execution/strategies_dashboard.md",
				Fragments: new[] { "Live 実行" }
			),
		};

		foreach (var check in checks)
		{
			var file = Path.Combine(_repoRoot, check.Lang, check.RelativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				foreach (var fragment in check.Fragments)
				{
					if (!text.Contains(fragment, StringComparison.Ordinal))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized live documentation keeps bare English live label '{fragment}'.");
				}
			}
		}

		foreach (var lang in new[] { "es", "pt" })
		{
			var designerRoot = Path.Combine(_repoRoot, lang, "topics", "designer", "strategies");
			if (!Directory.Exists(designerRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(designerRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					if (!text.Contains("[live]", StringComparison.Ordinal))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized strategy docs keep English live link label '[live]'.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void JapanesePositionProtectionDocsDoNotKeepEnglishTakeStopParameterLabels()
	{
		var errors = new List<string>();
		var file = Path.Combine(_repoRoot, "ja", "topics", "designer", "strategies", "using_visual_designer", "elements", "positions", "protect.md");
		var pattern = new Regex(@"\b(?:Take|Stop)\b|テイクまたはストップ", RegexOptions.CultureInvariant);

		if (File.Exists(file))
		{
			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
			{
				var match = pattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Japanese position-protection docs keep English take/stop parameter label '{match.Value}'. Use localized protection terminology.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedStrategyLoggingCodeLiteralsDoNotKeepEnglishTradeActionLabels()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "api", "strategies", "logging.md");
		var pattern = new Regex(@"\b(?:Bought|Sold)\b", RegexOptions.CultureInvariant);

		foreach (var lang in GetTranslatedContentLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);
			if (!File.Exists(file))
				continue;

			foreach (var literal in EnumerateCodeStringLiterals(ReadAllText(file)))
			{
				var match = pattern.Match(literal.Text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{literal.Line}: localized strategy logging example keeps English trade action label '{match.Value}'. Localize visible log output.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void GermanTradingOrderDocsDoNotKeepEnglishOrdersLabels()
	{
		var errors = new List<string>();
		var files = new[]
		{
			Path.Combine("topics", "api", "ai_development", "strategy_with_ai.md"),
			Path.Combine("topics", "api", "graphical_user_interface", "trading.md"),
			Path.Combine("topics", "api", "graphical_user_interface", "trading", "orders.md"),
		};
		var pattern = new Regex(@"(?<![A-Za-z.])Orders(?![A-Za-z])", RegexOptions.CultureInvariant);

		foreach (var relative in files)
		{
			var file = Path.Combine(_repoRoot, "de", relative);
			if (!File.Exists(file))
				continue;

			var markdown = ReadAllText(file);

			foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
			{
				if (text.Contains("Samples", StringComparison.Ordinal))
					continue;

				var match = pattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: German trading docs keep English order label '{match.Value}'. Use localized visible order wording.");
			}

			foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
			{
				var match = pattern.Match(altText.Text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{altText.Line}: German trading docs keep English order label '{match.Value}' in image alt text. Localize the image description.");
			}

			foreach (var comment in EnumerateCodeComments(markdown))
			{
				var match = pattern.Match(comment.Text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{comment.Line}: German trading docs keep English order label '{match.Value}' in a code comment. Localize the comment.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedCjkTradingProtectionDocsDoNotKeepEnglishTakeProfitStopLossTerms()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\b(?:take[- ]profit|stop[- ]loss)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in new[] { "ja", "zh" })
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var line = 1;
				using var reader = new StringReader(ReadAllText(file));

				for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: CJK documentation keeps English protection term '{match.Value}'. Localize visible take-profit/stop-loss wording.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void ChineseReportingDocsUsePositionRoundTripTerminology()
	{
		var errors = new List<string>();
		var file = Path.Combine(_repoRoot, "zh", "topics", "api", "strategies", "reporting.md");
		var badPattern = new Regex(@"位置回程|往返行程|位置反转|来回交易", RegexOptions.CultureInvariant);

		if (File.Exists(file))
		{
			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				var match = badPattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Chinese reporting documentation uses an incorrect position round-trip term '{match.Value}'. Use 持仓往返交易 terminology.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void ChineseTradingDocsUsePositionTerminologyForHoldings()
	{
		var errors = new List<string>();
		var checks = new Dictionary<string, string[]>
		{
			["topics/api/graphical_user_interface/portfolios/table.md"] = ["选定的位置", "已选择的位置"],
			["topics/api/orders_management/orders_states.md"] = ["位置信息"],
			["topics/api/strategies/trading_operations.md"] = ["只允许位置减操作"],
			["topics/api/strategies/target_position_management.md"] = ["当前位置", "目标位置", "位置变动算法"],
			["topics/api/strategies/take_profit_and_stop_loss.md"] = ["特定位置", "更新位置信息", "跟踪位置"],
			["topics/api/strategies/statistics_reference.md"] = ["基于位置的参数", "位置参数"],
			["topics/api/strategies/samples/mq.md"] = ["当前位置确定", "位置变化"],
		};

		foreach (var (relativePath, badPhrases) in checks)
		{
			var file = Path.Combine(_repoRoot, "zh", relativePath.Replace('/', Path.DirectorySeparatorChar));

			if (!File.Exists(file))
				continue;

			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				foreach (var phrase in badPhrases)
				{
					if (!text.Contains(phrase, StringComparison.Ordinal))
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: Chinese trading documentation uses '{phrase}' for a trading position. Use 持仓 terminology.");
				}
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void ChineseDocsDoNotUseCommerceTermForTradingTrades()
	{
		var errors = new List<string>();
		var zhRoot = Path.Combine(_repoRoot, "zh");
		var badPattern = new Regex(@"贸易|own trade", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var file in Directory.EnumerateFiles(zhRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				var match = badPattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Chinese documentation uses '{match.Value}' for trading trade terminology. Use 交易 or 成交 wording.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void ChineseDocsDoNotUseShoppingOrderTermForTradingOrders()
	{
		var errors = new List<string>();
		var zhRoot = Path.Combine(_repoRoot, "zh");

		foreach (var file in Directory.EnumerateFiles(zhRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				if (!text.Contains("订购", StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Chinese documentation uses shopping-order term '订购' in trading documentation. Use 订单 or 订阅 depending on context.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void ChineseDocsUseStrategyTermForStockSharpStrategies()
	{
		var errors = new List<string>();
		var zhRoot = Path.Combine(_repoRoot, "zh");

		foreach (var file in Directory.EnumerateFiles(zhRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				if (!text.Contains("战略", StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Chinese documentation uses '战略' for strategy documentation. Use 策略 terminology.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void ChineseDocsDoNotUseSwapTermForExchangeTradingContext()
	{
		var errors = new List<string>();
		var zhRoot = Path.Combine(_repoRoot, "zh");
		var badPattern = new Regex(@"交换工作|交换端点", RegexOptions.CultureInvariant);

		foreach (var file in Directory.EnumerateFiles(zhRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				var match = badPattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Chinese documentation uses swap term '{match.Value}' for exchange/trading context. Use 交易所 or 交易 wording.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void ChineseDocsUseSecurityTerminologyForTradingInstruments()
	{
		var errors = new List<string>();
		var zhRoot = Path.Combine(_repoRoot, "zh");
		var badPattern = new Regex(@"器械|非流动性工具|基础工具|所选的工具|多个工具|第一个工具|第二个工具|工具标识符|工具类型|搜索工具|接收工具", RegexOptions.CultureInvariant);

		foreach (var file in Directory.EnumerateFiles(zhRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
		{
			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				var match = badPattern.Match(text);
				if (!match.Success)
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: Chinese documentation uses generic instrument wording '{match.Value}'. Use 交易品种 terminology in trading contexts.");
			}
		}

		AssertNoErrors(errors);
	}

	[TestMethod]
	public void LocalizedRoundTripDocsDoNotKeepEnglishOrTravelTerms()
	{
		var errors = new List<string>();
		var badPattern = new Regex(@"\b(?:[Rr]ound[- ][Tt]rips?|[Rr]oundtrips?)\b|раундтрип|往返旅行|往返行程|位置回程|来回交易|往返持仓|（往返）", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var langRoot = Path.Combine(_repoRoot, lang);

			foreach (var file in Directory.EnumerateFiles(langRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var line = 1;
				using var reader = new StringReader(ReadAllText(file));

				for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
				{
					var match = badPattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps incorrect round-trip terminology '{match.Value}'. Localize the visible term consistently.");
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
	public void LocalizedStatisticsReferenceClassLinkLabelsMatchXrefTypeNames()
	{
		var errors = new List<string>();
		var relative = Path.Combine("topics", "api", "strategies", "statistics_reference.md");
		var linkPattern = new Regex(@"^\|\s*\[(?<label>[^\]]+)\]\(xref:StockSharp\.Algo\.Statistics\.(?<type>[A-Za-z0-9_]+)\)\s*\|", RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relative);

			if (!File.Exists(file))
				continue;

			var line = 1;
			using var reader = new StringReader(ReadAllText(file));

			for (var text = reader.ReadLine(); text is not null; text = reader.ReadLine(), line++)
			{
				var match = linkPattern.Match(text);
				if (!match.Success)
					continue;

				var label = match.Groups["label"].Value;
				var typeName = match.Groups["type"].Value;

				if (label.Equals(typeName, StringComparison.Ordinal))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{line}: statistics reference class link label '{label}' hides API type '{typeName}'. Keep the class column as the API type name and localize the description column.");
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
	public void LocalizedStrategySampleDocsDoNotKeepEnglishBullishBearishWords()
	{
		var errors = new List<string>();
		var phrases = new[] { "bullish", "bearish" };

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var samplesRoot = Path.Combine(_repoRoot, lang, "topics", "api", "strategies", "samples");
			if (!Directory.Exists(samplesRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(samplesRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					foreach (var phrase in phrases)
					{
						if (!ContainsStandaloneText(text, phrase))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{line}: localized strategy sample documentation keeps English market direction word '{phrase}'. Localize it for the target language.");
					}
				}

				foreach (var comment in EnumerateCodeComments(markdown))
				{
					foreach (var phrase in phrases)
					{
						if (!ContainsStandaloneText(comment.Text, phrase))
							continue;

						errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized strategy sample code comment keeps English market direction word '{phrase}'. Localize it for the target language.");
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
	public void LocalizedIndicatorDocsDoNotKeepEnglishCrossoverTerms()
	{
		var errors = new List<string>();
		var pattern = new Regex(@"\bCrossovers?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					var withoutCode = Regex.Replace(text, @"`[^`]*`", " ", RegexOptions.CultureInvariant);
					var match = pattern.Match(withoutCode);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English crossover term '{match.Value}'. Localize visible prose while preserving API identifiers.");
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
	public void LocalizedIndicatorDocsDoNotKeepEnglishHighLowSignalFragments()
	{
		var errors = new List<string>();
		var highLowPattern = new Regex(@"\b(?:High|Low)\s+[A-Z]{2,8}\b", RegexOptions.CultureInvariant);
		var peakTroughPattern = new Regex(@"\bpeaks?/troughs?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var indicatorRoot = Path.Combine(_repoRoot, lang, "topics", "api", "indicators", "list_of_indicators");
			if (!Directory.Exists(indicatorRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(indicatorRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(ReadAllText(file)))
				{
					foreach (Match match in highLowPattern.Matches(text))
						errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English high/low descriptor '{match.Value}'. Localize the descriptor while preserving the indicator abbreviation.");

					foreach (Match match in peakTroughPattern.Matches(text))
						errors.Add($"{RelativeToRepo(file)}:{line}: localized indicator documentation keeps English peak/trough pair '{match.Value}'. Localize the phrase.");
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
		var pattern = new Regex(@"\bBollinger Bands?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var topicsRoot = Path.Combine(_repoRoot, lang, "topics");
			if (!Directory.Exists(topicsRoot))
				continue;

			foreach (var file in Directory.EnumerateFiles(topicsRoot, "*.md", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
			{
				var markdown = ReadAllText(file);

				foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
				{
					var match = pattern.Match(text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps English Bollinger band label '{match.Value}'. Localize visible indicator names in prose.");
				}

				foreach (var comment in EnumerateCodeComments(markdown))
				{
					var match = pattern.Match(comment.Text);
					if (!match.Success)
						continue;

					errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized code comment keeps English Bollinger band label '{match.Value}'. Localize visible indicator names in comments.");
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
	public void LocalizedHydraTaskCodeCommentsDoNotKeepEnglishDiskComment()
	{
		var errors = new List<string>();
		var relativePath = "topics/hydra/create_new_task.md";

		foreach (var lang in GetLocalizedContentQualityLanguages())
		{
			var file = Path.Combine(_repoRoot, lang, relativePath.Replace('/', Path.DirectorySeparatorChar));
			if (!File.Exists(file))
				continue;

			foreach (var comment in EnumerateCodeComments(ReadAllText(file)))
			{
				if (!comment.Text.Contains("is a disk", StringComparison.OrdinalIgnoreCase))
					continue;

				errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized Hydra task sample keeps the English code comment 'is a disk'.");
			}
		}

		AssertNoErrors(errors);
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
	{
		var normalized = (url ?? string.Empty).Replace('\\', '/').Trim();
		return NormalizeStockSharpSiteLanguageRouteForStructure(normalized);
	}

	private static string NormalizeStockSharpSiteLanguageRouteForStructure(string url)
		=> Regex.Replace(
			url,
			@"^(?<scheme>https?://)(?:www\.)?stocksharp\.com/(?<lang>en|ru|de|es|pt|ja|zh)(?=$|[/?#])",
			match => $"{match.Groups["scheme"].Value}stocksharp.com/{{lang}}",
			RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

	private static string NormalizeStockSharpSiteSuffix(string suffix)
	{
		var value = (suffix ?? string.Empty).Replace(@"\/", "/");

		while (value.Length > 0 && ".,;:".IndexOf(value[^1]) >= 0)
			value = value[..^1];

		return value;
	}

	private static bool HasStockSharpSiteLanguagePrefix(string suffix, string lang)
	{
		var prefix = "/" + lang;

		return suffix.Equals(prefix, StringComparison.OrdinalIgnoreCase)
			|| suffix.StartsWith(prefix + "/", StringComparison.OrdinalIgnoreCase)
			|| suffix.StartsWith(prefix + "?", StringComparison.OrdinalIgnoreCase)
			|| suffix.StartsWith(prefix + "#", StringComparison.OrdinalIgnoreCase);
	}

	private static string GetStockSharpSiteLanguagePrefix(string suffix)
	{
		foreach (var lang in _stockSharpSiteLanguages)
		{
			if (HasStockSharpSiteLanguagePrefix(suffix, lang))
				return lang;
		}

		return string.Empty;
	}

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

	private static string NormalizeMarkdownTextForLocalizedLoggingTermCheck(string text)
	{
		var normalized = Regex.Replace(text, @"!\[(?<label>[^\]\r\n]*)\]\([^\)\r\n]*\)", "${label}", RegexOptions.CultureInvariant);
		normalized = Regex.Replace(normalized, @"\[(?<label>[^\]\r\n]*)\]\([^\)\r\n]*\)", "${label}", RegexOptions.CultureInvariant);
		normalized = Regex.Replace(normalized, @"`[^`\r\n]*`", " ", RegexOptions.CultureInvariant);
		normalized = Regex.Replace(normalized, @"\*\*[A-Za-z_]\w*\*\*", " ", RegexOptions.CultureInvariant);
		normalized = Regex.Replace(normalized, @"\blogs\.[A-Za-z0-9]+\b", " ", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		normalized = Regex.Replace(normalized, @"\b[A-Za-z_]\w*(?:\.[A-Za-z_]\w*)+\b", " ", RegexOptions.CultureInvariant);

		return normalized;
	}

	private static void AddLocalizedEnglishTermErrors(string file, string markdown, Regex pattern, string term, List<string> errors)
	{
		foreach (var (text, line) in EnumerateUserVisibleMarkdownLines(markdown))
		{
			if (pattern.IsMatch(NormalizeTextForLocalizedCandleTermCheck(text)))
				errors.Add($"{RelativeToRepo(file)}:{line}: localized documentation keeps English '{term}' prose outside API identifiers, paths, and code.");
		}

		foreach (var altText in EnumerateMarkdownImageAltTexts(markdown))
		{
			if (pattern.IsMatch(NormalizeTextForLocalizedCandleTermCheck(altText.Text)))
				errors.Add($"{RelativeToRepo(file)}:{altText.Line}: localized image alt text keeps English '{term}' prose.");
		}

		foreach (var comment in EnumerateCodeComments(markdown))
		{
			var normalized = NormalizeTextForLocalizedCandleTermCheck(NormalizeCodeCommentForTranslationCheck(comment.Text));
			if (pattern.IsMatch(normalized))
				errors.Add($"{RelativeToRepo(file)}:{comment.Line}: localized code comment keeps English '{term}' prose.");
		}
	}

	private static string NormalizeTextForLocalizedCandleTermCheck(string text)
	{
		var normalized = Regex.Replace(text ?? string.Empty, @"\b[A-Za-z_]\w*(?:\.[A-Za-z_]\w*)*\([^()\r\n]*\)", " ", RegexOptions.CultureInvariant);
		normalized = NormalizeMarkdownTextForLocalizedLoggingTermCheck(normalized);
		normalized = Regex.Replace(normalized, @"(?::param|@param)\s+[A-Za-z_]\w*\s*:?", " ", RegexOptions.CultureInvariant);

		return normalized;
	}

	private static string NormalizeMarkdownTextForGermanTestingTermCheck(string text)
	{
		var normalized = NormalizeMarkdownTextForLocalizedLoggingTermCheck(text).Replace("\\/", "/", StringComparison.Ordinal);
		normalized = Regex.Replace(normalized, @"(?:\.\.)?Samples/Testing/[A-Za-z0-9_./-]+", " ", RegexOptions.CultureInvariant);
		normalized = Regex.Replace(normalized, @"\bSample[A-Za-z0-9_]*Testing[A-Za-z0-9_]*\b", " ", RegexOptions.CultureInvariant);

		return normalized;
	}

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

	private static bool IsAllowedLocalizedCandleTermLine(string relativePath, string normalizedText)
		=> relativePath.Equals("topics/designer/strategies/using_dll/debug_dll_in_visual_studio.md", StringComparison.OrdinalIgnoreCase)
			&& Regex.IsMatch(normalizedText, @"\bProcessCandle\s*\(\s*Candle\s+candle\s*\)", RegexOptions.CultureInvariant);

	private static void AddLocalizedCandleTermError(
		string file,
		string relativePath,
		int line,
		string source,
		string normalizedText,
		string replacement,
		Regex pattern,
		List<string> errors)
	{
		if (string.IsNullOrWhiteSpace(normalizedText) || IsAllowedLocalizedCandleTermLine(relativePath, normalizedText))
			return;

		var match = pattern.Match(normalizedText);
		if (!match.Success)
			return;

		errors.Add($"{RelativeToRepo(file)}:{line}: localized {source} keeps English candle term '{match.Value}'. Use '{replacement}' in user-facing text.");
	}

	private static void AddLocalizedTickTermError(
		string file,
		int line,
		string lang,
		string source,
		string normalizedText,
		Regex pattern,
		List<string> errors)
	{
		var match = pattern.Match(normalizedText);
		if (!match.Success)
			return;

		var replacement = lang.Equals("ja", StringComparison.OrdinalIgnoreCase) ? "ティック" : "逐笔成交";
		errors.Add($"{RelativeToRepo(file)}:{line}: {lang} {source} keeps English tick term '{match.Value}'. Use '{replacement}' in user-facing text.");
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
		if (string.IsNullOrWhiteSpace(text))
			return false;

		var normalizedUrl = (url ?? string.Empty).Replace('\\', '/');
		var isKnownShortTranslatableLabel = IsKnownShortTranslatableEnglishMarkdownLinkLabel(text, normalizedUrl);

		if (text.Length < 8 && !isKnownShortTranslatableLabel)
			return false;

		if (text.Contains(RussianSpecificPlaceholderMarker, StringComparison.OrdinalIgnoreCase)
			|| text.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
			|| text.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
			|| normalizedUrl.Contains("list_of_indicators/", StringComparison.OrdinalIgnoreCase)
			|| IsCodeLikeMarkdownLinkLabel(text)
			|| Regex.IsMatch(text, @"^[\p{P}\p{S}\p{N}\s]+$", RegexOptions.CultureInvariant)
			|| (!isKnownShortTranslatableLabel && Regex.IsMatch(text, @"^[A-Za-z0-9_.:/#?=&%{}+-]+$", RegexOptions.CultureInvariant))
			|| (!isKnownShortTranslatableLabel && IsAllowedInvariantLinkLabel(text)))
		{
			return false;
		}

		if (isKnownShortTranslatableLabel)
			return true;

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

	private static bool IsKnownShortTranslatableEnglishMarkdownLinkLabel(string text, string normalizedUrl)
	{
		if (text.Equals("Chat", StringComparison.Ordinal))
			return normalizedUrl.Contains("t.me/stocksharpchat", StringComparison.OrdinalIgnoreCase);

		if (text.Equals("formed", StringComparison.Ordinal))
			return normalizedUrl.EndsWith("/api/indicators.md", StringComparison.OrdinalIgnoreCase);

		if (text.Equals("Samples", StringComparison.Ordinal)
			|| text.Equals("Samples/", StringComparison.Ordinal))
		{
			return normalizedUrl.Contains("/Samples", StringComparison.OrdinalIgnoreCase);
		}

		if (text.Equals("server", StringComparison.Ordinal))
			return normalizedUrl.Equals("../hydra_server.md", StringComparison.OrdinalIgnoreCase);

		if (text.Equals("Store", StringComparison.Ordinal))
			return NormalizeStockSharpSiteLanguageRouteForStructure(normalizedUrl).Equals("https://stocksharp.com/{lang}/store/", StringComparison.OrdinalIgnoreCase);

		return false;
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
		=> Regex.IsMatch(text, @"\b(?:Console\.Write(?:Line)?|Add(?:Info|Debug|Warning|Error)Log|Log(?:Verbose|Info|Debug|Warning|Error)|AlertLog|MessageBox\.Show|Trace\.Write(?:Line)?)\s*\(", RegexOptions.CultureInvariant);

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
