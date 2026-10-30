# Switching to Jenin.FontAwesome.Blazor

Migration guides from each supported source to this library's typed Font Awesome 7 API.
Every mapping below is verified by render tests in
`tests/Jenin.FontAwesome.Blazor.Tests/Migration/` — the target side of each table row is
asserted, so the guides cannot drift from the implementation.

| From | Guide | Verified by |
|---|---|---|
| MudBlazor `MudIcon` / `MudIconButton` (FA class strings) | [from-mudblazor.md](from-mudblazor.md) | `MudBlazorMigrationTests` |
| Blazorise `Icon` (Font Awesome provider) | [from-blazorise.md](from-blazorise.md) | `BlazoriseMigrationTests` |
| EasyAppDev FA5 / FA6 per-icon components | [from-easyappdev.md](from-easyappdev.md) | `EasyAppDevMigrationTests` |
| Blazicons.FontAwesome `<Blazicon>` values | [from-blazicons.md](from-blazicons.md) | `BlaziconsMigrationTests` |
| Raw FA6/FA7 class strings | [from-raw-fontawesome.md](from-raw-fontawesome.md) | `RawClassMigrationTests` |

Source excerpts follow the vendors' public docs as of 2026-10-26 (versions pinned in each
guide); conversions were verified against FA Free 7.3.1, the catalogue this library ships.

Reading order: pick your source guide, work its checklist, then read the
[README setup section](https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor#readme)
(one-line setup) once.
