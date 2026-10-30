# TASK-IE-06 — Theming, Accessibility & Keyboard Navigation

**Status:** DONE
**Priority:** high
**Model:** STRONG

## Goal
Make the explorer comfortable and navigable for everyone: a light/dark theme, a complete keyboard
flow with a discoverable shortcut overlay, and accessibility/responsiveness guarantees (focus
visibility, ARIA labelling, reduced-motion respect, and usability down to 360 px).

Scope is derived from the IE-01 requirements sections **S1** (theme toggle), **S2** (keyboard flow),
**C5** (keyboard-shortcut help), and **M7** (responsive + accessible).

## Features
- **S1 — Light/dark theme.** A toggle in the explorer toolbar switches palettes; the explicit choice
  is persisted and, when the user has made no choice, the operating-system colour scheme governs.
- **S2 — Keyboard flow.** `/` focuses search, `Esc` clears the search, arrow keys move between cards,
  `Enter` selects, `C` copies the current snippet.
- **C5 — Shortcut help.** A `?` shortcut (and a toolbar button) opens an overlay listing every
  shortcut; `Esc` or the Close button dismisses it.
- **M7 — Responsive + a11y.** Focus-visible outlines on every interactive element, ARIA labelling on
  the toolbars/grid/details pane, honouring `prefers-reduced-motion` for preview animations, and a
  layout that stays usable at 360 px.

## Deliverables
- Theme tokens (CSS custom properties) + dark palette + toggle component.
- Keyboard handlers (grid + explorer) and a shortcut-help overlay.
- Reduced-motion CSS and responsive rules.
- JS helpers for theme persistence, focus movement, and typing-target detection.

## Acceptance
- Theme toggles and persists; respects the system preference on first load.
- Every listed shortcut works and is documented in the help overlay.
- Preview animations are disabled under `prefers-reduced-motion: reduce`.
- The explorer remains usable and readable at 360 px, with no clipped controls.

## Dependencies
- IE-03 (grid/search shell), IE-04 (preview controls), IE-05 (snippet panel — the `C` shortcut copies
  its output).

## Outcome
Implemented in `samples\Jenin.FontAwesome.Blazor.Explorer`:

- **Theming (S1).** `wwwroot\css\explorer.css` now expresses every colour as a custom property on
  `.explorer` (`--ex-surface`, `--ex-text`, `--ex-accent`, `--ex-focus`, …) with a light default, a
  `[data-theme="dark"]` override, and a `@media (prefers-color-scheme: dark)` block that applies when
  no explicit theme is set. `Components\ThemeToggle.razor(.cs)` is the presentational switch; the
  owning `Explorer` holds the theme state, reads the stored value (else the system preference) on first
  render, stamps `data-theme` through JS, and persists the choice.
- **Keyboard flow (S2).** `Components\IconGrid` adopted a roving tabindex (exactly one card is
  tabbable) and a bubbled `@onkeydown` that moves focus with `← ↑ → ↓` / `Home` / `End` via the
  DOM-relative `moveCardFocus` helper (correct under virtualization, where only the visible window is
  rendered). `Components\Explorer` handles the global `/`, `?`, and `C` shortcuts and ignores keys
  pressed inside a text field (via the `isTypingTarget` JS check) so typing never triggers them; the
  search field keeps its own `Esc` handling.
- **Shortcut help (C5).** `Components\KeyboardHelp.razor(.cs)` renders a modal `role="dialog"`
  overlay listing the shortcuts, opened with `?` or the toolbar button, dismissed with `Esc`/Close,
  and focuses its Close button on open.
- **Accessibility + motion + responsive (M7).** Every focusable control has a `:focus-visible` outline
  built from `--ex-focus`; the grid is `role="group"`, cards are `aria-pressed` toggle buttons, the
  status region is `aria-live="polite"`, and the detail/preview panes are labelled. The preview canvas
  honours `prefers-reduced-motion` by neutralising Font Awesome animation classes and transitions. A
  `≤768 px` block turns the preview into a stacked sheet and keeps a usable grid height, so the layout
  works at 360 px.
- **JS (`wwwroot\js\explorer.js`).** Added `getStoredTheme`/`setStoredTheme`/`applyTheme`,
  `systemPrefersDark`, `isTypingTarget`, `focusSearch`, `focusSelector`, `moveCardFocus`,
  `moveCardFocusTo`, and `focusFirstCard`.
- **Copy shortcut wiring.** `SnippetPanel.CopyCurrentAsync` and `PreviewPanel.CopySnippetAsync` expose
  the copy action so the explorer-level `C` shortcut can invoke it regardless of focus.

## Evidence
- `dotnet build "Jenin.FontAwesome.Blazor.slnx" --no-restore -c Release` → **0 warnings, 0 errors**.
- `dotnet test tests\Jenin.FontAwesome.Blazor.Tests` → **292 passed** (net8.0 / net9.0 / net10.0).
- `pwsh ./eng/verify-encoding.ps1 -Fix` → passed (BOM/CRLF/EOL normalised on the new files).
- e2e (Release server, `http://localhost:5199`):
  - `/explorer?icon=house&style=solid` → **200**; rendered HTML contains `explorer__toolbar-actions`,
    `explorer-help-toggle`, `explorer-theme-toggle`, `aria-label="Icon results"`,
    `aria-label="Icon details"`, the `ctl-query` search input, and the "Keyboard shortcuts" dialog.
  - `/_content/…/js/explorer.js` → **200** and contains `isTypingTarget`, `applyTheme`, `focusSearch`,
    `moveCardFocus`, `moveCardFocusTo`, `systemPrefersDark`, `focusSelector`.
  - `/_content/…/css/explorer.css` → **200** and contains `data-theme="dark"`,
    `prefers-reduced-motion`, `prefers-color-scheme: dark`, `explorer-help__dialog`,
    `explorer-theme-toggle`.

## Notes
- The theme is applied after the interactive circuit connects (JS interop), so the SSR prerender shows
  the light default for a beat; the CSS `prefers-color-scheme` media query covers the no-JS/first-paint
  case. `data-theme` is therefore absent from the prerendered HTML by design.
- Grid arrow navigation is **relative to the focused card**, not a global index: the virtualizer only
  keeps the visible window in the DOM, so a global index would map to the wrong element. Roving
  tabindex still gives Tab a single, deterministic entry point.
- The global shortcut handler runs a `isTypingTarget` interop check per root keydown; this keeps
  single-letter shortcuts safe inside inputs at the cost of one cheap round-trip per key.
- Reduced-motion is enforced purely in CSS so it also covers animations chosen in the preview controls
  without extra state.
