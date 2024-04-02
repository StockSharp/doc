using Ecng.Common;
using Ecng.Collections;

using System.Text.RegularExpressions;
using System.Linq;

var rootDir = @"C:\StockSharp\doc\";
var enOnlyNames = new Regex(@"^[a-zA-Z0-9\s\p{P}]*$", RegexOptions.Singleline | RegexOptions.Compiled);

var enRedirs = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
{
	"Plaza.md", "Tinkoff.md", "Alor.md", "MoexISS.md", "Quik.md", "Transaq.md"
};

foreach (var lang in new[] { "en", "ru" })
{
	Console.WriteLine(lang);

	var langDir = Path.Combine(rootDir, $@"{lang}\");
	var topicsDir = Path.Combine(langDir, @"topics\");
	var imagesDir = Path.Combine(langDir, @"images\");

	var toc = await File.ReadAllTextAsync(Path.Combine(topicsDir, "toc.yml"));

	var mdfiles = Directory
		.EnumerateFiles(topicsDir, "*.md", SearchOption.AllDirectories)
		.Select(f => Path.GetFileName(f))
		.ToHashSet();

	var imgfiles = Directory
		.EnumerateFiles(imagesDir, "*.png", SearchOption.AllDirectories)
		.Select(f => Path.GetFileName(f))
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

		await removeUsedImgs(Path.Combine(topicsDir, f));
	}

	var unused = mdfiles.Where(f => !toc.ContainsIgnoreCase(f)).ToArray();

	Console.WriteLine("Unused md:");

	foreach (var f in unused)
		Console.WriteLine(f);

	Console.WriteLine("Unused img:");

	foreach (var f in imgfiles)
	{
		Console.WriteLine(f);
		File.Delete(Path.Combine(imagesDir, f));
	}

	Console.WriteLine();
}