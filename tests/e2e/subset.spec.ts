import { test, expect } from '@playwright/test';
import { readFileSync } from 'node:fs';

// The self-hosted subset (eng/fa_subset.py) replaces the full all.css + fonts with a rewritten
// stylesheet and per-style WOFF2 files containing only the used glyphs. These tests serve the
// checked-in fixture (tests/e2e/fixtures/fa-subset) through route interception and verify the
// browser actually renders the kept icons while a dropped icon renders nothing.
const webfontConfig = readFileSync(
    new URL('../../src/Jenin.FontAwesome.Blazor/wwwroot/js/fontawesome-config-webfont.js', import.meta.url),
    'utf8');

test.describe('self-hosted subset', () => {
    test.beforeEach(async ({ page }) => {
        await page.route('**/fontawesome-config-nest.js', (route) =>
            route.fulfill({ status: 200, contentType: 'text/javascript; charset=utf-8', body: webfontConfig }));
        await page.route('**/fontawesome/css/all.css', (route) =>
            route.fulfill({ path: 'fixtures/fa-subset/fontawesome-subset.css', contentType: 'text/css; charset=utf-8' }));
        await page.route('**/fontawesome/webfonts/*.woff2', (route) => {
            const name = route.request().url().split('/').pop()!;
            return route.fulfill({ path: `fixtures/fa-subset/${name}` });
        });
    });

    test('kept icons render their glyphs from the subset fonts', async ({ page }) => {
        await page.goto('/icon-basics-style');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        for (const [selector, family] of [['i.fa-camera', 'Font Awesome 7 Free'], ['i.fa-youtube', 'Font Awesome 7 Brands']] as const) {
            const icon = page.locator(selector).first();
            await expect(icon).toBeVisible();

            const snapshot = await icon.evaluate((element) => ({
                glyph: getComputedStyle(element, '::before').content,
                fontFamily: getComputedStyle(element).fontFamily,
            }));

            expect(snapshot.glyph).toMatch(/^"[^"]+"/);
            expect(snapshot.fontFamily).toContain(family);
        }
    });

    test('dropped icons render nothing', async ({ page }) => {
        await page.goto('/icon-basics-style');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        const icon = page.locator('i.fa-docker').first();
        await expect(icon).toBeAttached();

        const glyph = await icon.evaluate((element) => getComputedStyle(element, '::before').content);
        expect(glyph).toBe('none');
    });
});
