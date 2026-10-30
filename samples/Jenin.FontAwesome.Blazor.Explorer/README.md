<!--
//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
-->
# Interactive Icon Explorer

An interactive, developer-facing icon explorer for Jenin.FontAwesome.Blazor, in the spirit of
<https://fontawesome.com/search>. It lets you search the Font Awesome Free catalogue, filter by style,
preview an icon with the full range of library features (style, typeface, size, colour, rotation,
animations, transformations, pull/border, and duotone), inspect the generated Razor / preset / HTML
markup, and copy a snippet — with the entire view addressable through the URL so it can be shared and
bookmarked.

> This is a **sample** Razor class library (`samples/Jenin.FontAwesome.Blazor.Explorer`), not a shipped
> package. It targets **`net10.0`** and is the only place in the repository that is allowed to use
> JavaScript (for measuring the grid and for keyboard/theme interop); the shipped library packages stay
> interop-free.

## Running it

The explorer is hosted by the Blazor Web App server sample, which references this project and serves
the route at `/explorer`:

```bash
dotnet run --project samples/Jenin.FontAwesome.Blazor.Sample
```

Then open `http://localhost:5000/explorer` (the port comes from the sample's launch settings) or use
the **Icon explorer** entry in the left-hand navigation under **Reference**. The page needs an
interactive render mode (Interactive Server is used); the virtualized grid renders its cards once the
circuit connects.

## Features

- **Search** over all 1,992 Font Awesome Free icons by name, alias, or keyword.
- **Style facets** — `All` (1,992), `Solid` (1,422), `Regular` (169), and `Brands` (572), each with
  its count.
- **Virtualized result grid** — only the visible rows are rendered, so the full catalogue scrolls
  smoothly; the column count follows the measured container width.
- **Live preview** driven by controls for style, typeface, size and width, foreground/background
  colour, rotation and flip, all 14 animation families (with delay/duration/iteration/direction/
  timing), power transforms, pull, bordered/inverse, and duotone primary/secondary colour and opacity.
  Controls that need a Font Awesome Pro asset kit show an `UnsupportedNotice` instead of rendering
  nothing.
- **Snippets** — copy the selected configuration as Razor (`<Icon … />`), as a per-icon preset
  component (`<HouseIcon />`), or as plain HTML.
- **Deep-linkable state** — the query, style facet, and selected icon are written to the URL.
- **Dark/light theme** with a persisted preference, and full keyboard support (see below).

## URL schema

The explorer keeps a deliberately small query-string schema. Defaults are omitted and unknown keys are
ignored, so a URL with no state is simply `/explorer`.

| Key | Meaning | Example |
|---|---|---|
| `q` | Search query | `/explorer?q=home` |
| `style` | Style facet: `solid`, `regular`, or `brands` | `/explorer?q=home&style=solid` |
| `icon` | Selected icon name (without the `fa-` prefix is accepted) | `/explorer?icon=house` |

Typing updates the URL with a history *replace* (after a short debounce) so the back button is not
cluttered; changing the facet or selecting an icon pushes a history entry so the browser's back and
forward buttons walk your choices.

## Keyboard shortcuts

Press `?` anywhere on the explorer to open the shortcut help dialog.

| Key | Action |
|---|---|
| `/` | Focus the search field |
| `Esc` | Clear the search field |
| `?` | Toggle the keyboard-shortcut help dialog |
| `C` | Copy the current snippet |
| `←` / `→` / `↑` / `↓` | Move focus between grid cards |
| `Home` / `End` | Move focus to the first / last card |
| `Enter` / `Space` | Select the focused card |

Shortcuts are ignored while a text field has focus, so typing a `c` or `?` into the search box is
never intercepted.

## Accessibility

- The grid uses a **roving tabindex**: exactly one card is tabbable, and the arrow keys move focus
  within the grid.
- The theme toggle, search field, facets, and dialog all carry accessible names and roles.
- Motion respects `prefers-reduced-motion`, and the colour theme follows `prefers-color-scheme` until
  the user chooses explicitly.
- The layout collapses to a stacked sheet at `≤768px`.

## Architecture map

| Area | Files |
|---|---|
| Shell / layout / key handling | `Components/Explorer.razor(.cs)` |
| Search box, style facets, grid, cards | `Components/SearchBar`, `StyleFilter`, `IconGrid`, `IconCard` |
| Preview and its controls | `Components/PreviewPanel`, `PreviewControls`, `UnsupportedNotice` |
| Snippet output and copy | `Components/SnippetPanel`, `Snippets/SnippetGenerator` |
| Theming / help dialog | `Components/ThemeToggle`, `KeyboardHelp` |
| Catalogue + search | `Data/ExplorerCatalogue`, `Data/ExplorerIconEntry` |
| URL state | `Routing/ExplorerRoute`, `Routing/ExplorerRouteState` |
| State models | `ExplorerState`, `ExplorerPreviewState`, `PreviewIconFactory` |
| Static assets | `wwwroot/css/explorer.css`, `wwwroot/js/explorer.js` |

The catalogue is loaded from `IconMetadataProvider` (the optional `Jenin.FontAwesome.Blazor.IconPicker`
package) and indexed once at startup, so searching the whole catalogue involves no network access and
stays well inside the sub-100 ms budget. The `Explorer` component owns the `ExplorerState` and is its
only writer; children receive values through parameters and report changes through callbacks.

## Tests

Unit and integration coverage lives in `tests/Jenin.FontAwesome.Blazor.Explorer.Tests` (MSTest,
`net10.0`): catalogue totals and ordering, the single-token search-parity contract with
`IconMetadataProvider.Search`, the URL round trip, and snippet generation. Browser-level coverage lives
in `tests/e2e/explorer.spec.ts` (Playwright) and is a required CI job alongside the rest of the
end-to-end suite.

```bash
dotnet test tests/Jenin.FontAwesome.Blazor.Explorer.Tests

cd tests/e2e
npm ci
npx playwright install --with-deps chromium
npx playwright test explorer.spec.ts
```

## Attribution

The Font Awesome Free 7.3.1 metadata bundled by the IconPicker package is redistributed under
[CC BY 4.0](https://creativecommons.org/licenses/by/4.0/); see the repository `LICENSES` file for the
full attribution. The explorer's footer links back to the upstream icon on fontawesome.com.
