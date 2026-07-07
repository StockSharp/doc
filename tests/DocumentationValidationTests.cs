using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;

using Ecng.UnitTesting;

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

	private static readonly string _repoRoot = FindRepoRoot();

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
	public void Localization_metadata_and_strings_are_consistent()
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
	public void Toc_files_reference_existing_markdown()
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
	public void Markdown_links_and_local_assets_resolve()
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
	public void Markdown_files_have_basic_document_structure()
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
}
