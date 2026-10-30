import { test, expect } from '@playwright/test';
import { readFileSync } from 'node:fs';

// Webfont mode (`autoReplaceSvg: false`) is the contract Font Awesome ships for hosts that
// cannot run the SVG engine: the runtime stays idle and anchors render through the
// `all.css` glyph pipeline instead. The sample host boots its own config (nest), so this
// harness intercepts that config script and serves the packaged webfont config verbatim —
// the page then runs Font Awesome's true webfont runtime without a second host.
// This closes the feature x mode matrix gap.
const webfontConfig = readFileSync(
    new URL('../../src/Jenin.FontAwesome.Blazor/wwwroot/js/fontawesome-config-webfont.js', import.meta.url),
    'utf8');

test.describe('webfont mode (autoReplaceSvg: false)', () => {
    test.beforeEach(async ({ page }) => {
        await page.route('**/fontawesome-config-nest.js', (route) =>
            route.fulfill({
                status: 200,
                contentType: 'text/javascript; charset=utf-8',
                body: webfontConfig
            }));
    });

    test('anchors render as unchanged webfont glyphs', async ({ page }) => {
        await page.goto('/icon-basics-style');

        // The engine stays idle: no `<i>` placeholder is replaced by an SVG.
        await expect(page.locator('svg.svg-inline--fa')).toHaveCount(0);

        const icon = page.locator('i.fa-solid').first();
        await expect(icon).toBeVisible();

        // The webfont pipeline renders through the Free typeface and the `::before` glyph
        // token shipped by `all.css` — no runtime involvement. The element and its computed
        // styles are read in one synchronous pass so Blazor's re-renders cannot detach it.
        const snapshot = await page.evaluate(() => {
            const icon = document.querySelector('i.fa-solid');
            return icon
                ? {
                      fontFamily: getComputedStyle(icon).fontFamily,
                      glyph: getComputedStyle(icon, '::before').content
                  }
                : null;
        });
        expect(snapshot?.fontFamily).toContain('Font Awesome 7 Free');
        expect(snapshot?.glyph).toMatch(/^"[^"]+"/);
    });

    test('CSS-driven rotations and flips keep working', async ({ page }) => {
        await page.goto('/rotate');

        // The transforms are pure CSS shipped by `all.css`; they must apply to the raw
        // placeholder element exactly as they would to any static markup.
        const rotations = await page.evaluate(() =>
            ['fa-rotate-90', 'fa-rotate-180', 'fa-rotate-270', 'fa-flip-horizontal', 'fa-flip-vertical', 'fa-flip-both'].map((rotation) => {
                const icon = document.querySelector(`i.${rotation}`);
                return { rotation, transform: icon ? getComputedStyle(icon).transform : null };
            }));

        for (const { rotation, transform } of rotations) {
            expect(transform, rotation).not.toBeNull();
            expect(transform, rotation).not.toBe('none');
        }
    });

    test('arbitrary rotate-by tokens keep working', async ({ page }) => {
        await page.goto('/rotate');

        const icon = page.locator('i.fa-rotate-by').first();
        await expect(icon).toBeVisible();

        // The token itself is plain CSS: the inline custom property feeds the transform.
        // Read in one synchronous pass so Blazor's re-renders cannot detach the element.
        const snapshot = await page.evaluate(() => {
            const icon = document.querySelector('i.fa-rotate-by');
            return icon
                ? {
                      angle: icon.getAttribute('style') ?? '',
                      transform: getComputedStyle(icon).transform
                  }
                : null;
        });
        expect(snapshot?.angle).toMatch(/--fa-rotate-angle/);
        expect(snapshot?.transform).toMatch(/^matrix/);
    });

    test('power transforms stay inert engine tokens', async ({ page }) => {
        await page.goto('/transforms');

        await expect(page.locator('svg.svg-inline--fa')).toHaveCount(0);
        const inert = page.locator('i[data-fa-transform]');
        expect(await inert.count()).toBeGreaterThanOrEqual(20);

        // The token is not consumed: the icon still renders its plain webfont glyph. The font is
        // read in one synchronous pass and polled so a Blazor re-render between the visibility
        // check and the read cannot detach the element mid-evaluation.
        const first = inert.first();
        await expect(first).toBeVisible();
        await expect.poll(async () => page.evaluate(() => {
            const icon = document.querySelector('i[data-fa-transform]');
            return icon ? getComputedStyle(icon).fontFamily : null;
        }), { timeout: 10_000 }).toContain('Font Awesome 7 Free');
    });

    test('masks do not apply without the SVG engine', async ({ page }) => {
        await page.goto('/masking');

        await expect(page.locator('svg.svg-inline--fa')).toHaveCount(0);
        // The masking page renders ten masked anchors. In webfont mode each stays a
        // single placeholder carrying the token; the SVG engine would nest a second
        // element with its own attribute set.
        expect(await page.locator('i[data-fa-mask]').count()).toBeGreaterThanOrEqual(10);
    });

    test('layers render as inert nested hosts', async ({ page }) => {
        await page.goto('/layering');

        await expect(page.locator('.fa-layers').first()).toBeVisible();
        expect(await page.locator('.fa-layers').count()).toBeGreaterThanOrEqual(12);

        // The wrapper keeps the emitted child placeholders; no SVG stack is generated.
        expect(await page.locator('.fa-layers i').count()).toBeGreaterThanOrEqual(12);
        await expect(page.locator('svg.svg-inline--fa')).toHaveCount(0);
    });

    test('duotone family pairs fall back to the classic style with Free assets', async ({ page }) => {
        await page.goto('/duotone');

        await expect(page.locator('svg.svg-inline--fa')).toHaveCount(0);

        // FA 7 pairs the `fa-duotone` family prefix with a style class. With Free assets the
        // Family lacks its own typeface, so the glyph pipeline uses the style class's Classic
        // (solid) typeface — a graceful fallback rather than a missing icon.
        await page.waitForFunction(() => {
            const icon = document.querySelector('i.fa-duotone');
            return icon && getComputedStyle(icon, '::before').content !== 'none';
        }, null, { timeout: 10_000 });

        const snapshot = await page.evaluate(() => {
            const icon = document.querySelector('i.fa-duotone');
            return icon
                ? {
                      fontFamily: getComputedStyle(icon).fontFamily,
                      fontWeight: getComputedStyle(icon).fontWeight,
                      glyph: getComputedStyle(icon, '::before').content
                  }
                : null;
        });
        expect(snapshot?.fontFamily).toContain('Font Awesome 7 Free');
        expect(snapshot?.fontWeight).toBe('900');
        expect(snapshot?.glyph).toMatch(/^"[^"]+"/);
    });
});
