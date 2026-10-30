import { test, expect } from '@playwright/test';

// The sprite path needs no Font Awesome JavaScript at all: block all.min.js outright and the
// <svg><use> glyphs must still render, while regular <i> placeholders stay untouched.
test.describe('self-hosted sprite', () => {
    test.beforeEach(async ({ page }) => {
        await page.route('**/fontawesome/js/all.min.js', (route) => route.abort());
        await page.goto('/icon-sprites');
        await expect(page.getByTestId('app-interactive')).toBeAttached();
    });

    test('sprite glyphs render with the runtime blocked', async ({ page }) => {
        const house = page.locator('use[href$="#fa-solid-house"]');
        await expect(house).toBeAttached();

        const svg = page.locator('svg:has(> use[href$="#fa-solid-house"])').first();
        await expect(svg).toBeVisible();

        // The engine never touched it: still a plain svg with the authored href.
        await expect(svg).toHaveAttribute('viewBox', '0 0 512 512');
        await expect(svg).not.toHaveAttribute('data-fa-i2svg', /.*/);
        await expect(house).toHaveAttribute('href', /fa-sprite\.svg#fa-solid-house$/);
    });

    test('generated inline-svg components render without the runtime', async ({ page }) => {
        await page.goto('/icon-sprites');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        // Generated components are plain inline SVG (no <use>, no FA processing).
        const house = page.locator('svg[data-testid="generated-house"]');
        await expect(house).toBeVisible();
        await expect(house).not.toHaveAttribute('data-fa-i2svg', /.*/);
        await expect(house.locator(':scope > path')).toHaveCount(1);

        const path = await house.locator(':scope > path').getAttribute('d');
        expect(path).toBeTruthy();
        expect(path!.length).toBeGreaterThan(100);
    });

    test('the engine really stayed off', async ({ page }) => {
        await page.goto('/icon-sprites');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        // A regular placeholder icon stays an <i>: without all.min.js nothing is enhanced.
        await expect(page.locator('i.fa-compass').first()).toBeAttached();
        await expect(page.locator('svg.fa-compass')).toHaveCount(0);
    });
});
