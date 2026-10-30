# Real-world gallery

Five composed screens built only with this library plus Bootstrap — the answer to "but
what does a whole screen look like?". Each page renders live demos and shows its own
source next to them (the shared `<Sample>` wrapper from the cookbook pattern).

| Screen | Route | What it proves |
|---|---|---|
| Admin dashboard | `/gallery/dashboard` | `StatCard` deltas, duotone header icon, status list with `StatusDot` presence |
| Notification center | `/gallery/notifications` | `NotificationBadge` counts, mark-all-read interaction, empty state on zero |
| File and activity timeline | `/gallery/timeline` | `IconList` markers, per-item colors, FA 7 glyph coverage |
| Settings with toggles | `/gallery/settings` | `IconToggleButton` with `aria-pressed`, live summary text |
| Inbox | `/gallery/inbox` | `AvatarIcon` + badges, per-row delete, zero-state transition |

Verification: `tests/e2e/gallery.spec.ts` asserts each screen's markers plus two
cross-cutting guarantees — a mobile viewport (480px) with no horizontal overflow and the
drawer toggle reachable, and the dark theme applied from storage with readable content.
Run it with the standard harness (`npx playwright test gallery.spec.ts` from
`tests/e2e`); headed runs use the same commands with `--headed` for visual review.
