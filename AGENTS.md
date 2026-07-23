# Documentation Workflow

## Adding Connector Documentation

When documenting a new connector, first compare it with a maintained connector in the same category (for US stock connectors, use Alpaca as the baseline).

Add the complete connector documentation set in every supported language:

- a short overview page at `<lang>/topics/api/connectors/<category>/<connector>.md`;
- `configuration_<connector>.md` in a matching child directory;
- `graphical_configuration_<connector>.md` in that child directory;
- `adapter_initialization_<connector>.md` in that child directory.

Keep the overview, configuration, graphical configuration, and adapter initialization responsibilities separate. Match the baseline connector's heading hierarchy, link layout, and navigation structure.

For every supported language:

- add the overview and all child pages to `<lang>/topics/toc.yml` as one nested entry;
- add the connector to `<lang>/topics/designer/connections_settings/connectors_settings.md`;
- add the connector to `<lang>/topics/hydra/data_sources.md`;
- localize visible labels and prose, while preserving exact API identifiers in code formatting when needed;
- use only real product screenshots. Copy an existing screenshot from the connector source repository when available; do not invent one or silently reuse another connector's image.

Verify the documented settings and capabilities against the connector source code. Run the documentation test suite (`dotnet test tests/StockSharp.Doc.Tests.csproj -c Release`) and `git diff --check` before committing. The same suite runs in CI (`.github/workflows/docs-validation.yml`) on every push and pull request.
