# API Reference

This page is the entry point for the **API Reference** generated from the source code using DocFX metadata.

The API documentation is produced automatically from the project assemblies and will appear under `/api/` after running the metadata step and building the site.

To regenerate API docs locally:

```bash
# generate API metadata
docfx metadata docs/docfx.json
# build the site
docfx build docs/docfx.json --logLevel Warning
```

If the API pages are not present, run the commands above and then refresh the site.
