using Ecng.Common;
using Ecng.Collections;

using System.Text.RegularExpressions;

var rootDir = @"C:\StockSharp\doc\";
var enOnlyNames = new Regex(@"^[a-zA-Z0-9\s\p{P}]*$", RegexOptions.Singleline | RegexOptions.Compiled);

var enRedirs = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

void addRussiaRedir(params string[] names)
{
	foreach (var name in names)
		enRedirs.Add($"topics/api/connectors/russia/{name}");
}

addRussiaRedir("plaza.md", "tinkoff.md", "alor.md", "moexiss.md", "quik.md", "transaq.md");

foreach (var lang in new[] { "en", "ru" })
{
	Console.WriteLine(lang);

	var langDir = Path.Combine(rootDir, $@"{lang}\");
	var topicsDir = Path.Combine(langDir, @"topics\");
	var imagesDir = Path.Combine(langDir, @"images\");

	var toc = await File.ReadAllTextAsync(Path.Combine(topicsDir, "toc.yml"));

	var mdfiles = Directory
		.EnumerateFiles(topicsDir, "*.md", SearchOption.AllDirectories)
		.Select(f => GetRelativePath(langDir, f))
		.ToHashSet();

	var imgfiles = Directory
		.EnumerateFiles(imagesDir, "*.png", SearchOption.AllDirectories)
		.Select(Path.GetFileName)
		.ToHashSet();

	if (lang == "en")
		mdfiles.RemoveRange(enRedirs);

	Console.WriteLine("Non en names:");

	foreach (var f in imgfiles)
	{
		if (!enOnlyNames.IsMatch(f))
			Console.WriteLine(f);
	}

	async Task removeUsedImgs(string f)
	{
		var mdContent = await File.ReadAllTextAsync(f);

		imgfiles.RemoveRange(imgfiles.Where(mdContent.ContainsIgnoreCase).ToArray());
	}

	await removeUsedImgs(Path.Combine(langDir, "index.md"));

	foreach (var f in mdfiles)
	{
		if (!enOnlyNames.IsMatch(f))
			Console.WriteLine(f);

		await removeUsedImgs(Path.Combine(langDir, f.Replace('/', '\\')));
	}

	var unused = mdfiles.Where(f => !toc.ContainsIgnoreCase($"href: {f.Remove("topics/", true)}")).ToArray();

	Console.WriteLine("Unused md:");

	foreach (var f in unused)
		Console.WriteLine(f);

	Console.WriteLine("Unused img:");

	foreach (var f in imgfiles)
	{
		Console.WriteLine(f);
		//File.Delete(Path.Combine(imagesDir, f));
	}

	Console.WriteLine();
}

static string GetRelativePath(string basePath, string targetPath)
{
	var baseUri = new Uri(basePath);
	var targetUri = new Uri(targetPath);

	return baseUri.MakeRelativeUri(targetUri).ToString();
}