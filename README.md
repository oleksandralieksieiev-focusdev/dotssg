# Static site generation from Markdown

basically a clone of `md2html`

## Running
```bash
dotnet build -c Release # build executable to ./bin/
dotnet run -c Release # start a dev version
```

## Usage
Flags:
- `--src` - source directory containing Markdown files (default: `./content`)
- `--out` - output directory with HTML files (default: `./www`)
- `--watch` - start watching and reacting to file changes
- `--razor` - use RazorLight template engine
- `--layout` - file containing HTML layout

## Tech stack
- C# / Dotnet 8.0
- Markdig
- RazorLight
