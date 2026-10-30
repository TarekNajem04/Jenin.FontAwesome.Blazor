# Security Policy

## Supported Versions

- 2.0.x (latest)

## Reporting a Vulnerability

If you discover a security vulnerability in this project, please report it via [GitHub Issues](https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/issues). Do not disclose security issues publicly until they have been addressed.

## Scope

This is a client-side Blazor component library with no server-side processing and no network calls initiated by the library. Security concerns are primarily limited to build, packaging, and supply-chain integrity.

## Security Considerations

- **No runtime dependencies**: The library ships no third-party NuGet dependencies at runtime (only Microsoft.AspNetCore.* references).
- **Minimal packaged assets**: Only two JavaScript configuration files are packaged (ontawesome-config-nest.js, ontawesome-config-webfont.js); no Font Awesome fonts, CSS, or JS are included.
- **Deterministic builds**: Builds use global.json for SDK pinning; CI validates restore, build, format, and packaging.
- **Sample assets**: Sample applications include Font Awesome Free assets for demonstration only (not packaged in the library). Those assets are distributed under Font Awesome Free license terms.
