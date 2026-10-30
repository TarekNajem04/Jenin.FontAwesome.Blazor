import { test, expect } from '@playwright/test';

// IconSvg.ToSvgString renders standalone SVG markup on the server: the sampled email preview
// must contain a self-contained svg (own viewBox, inlined path, no external references).
test.describe('server-side svg strings', () => {
    test('the email preview renders a standalone svg', async ({ page }) => {
        await page.goto('/svg-strings');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        const preview = page.locator('.border.rounded.p-4', { has: page.locator('svg[aria-label="Home"]') }).first();
        await expect(preview).toBeVisible();

        const svg = preview.locator('svg').first();
        await expect(svg).toHaveAttribute('viewBox', '0 0 512 512');
        await expect(svg).toHaveAttribute('width', '64');
        await expect(svg).toHaveAttribute('height', '64');
        await expect(svg.locator('title')).toHaveText('Home');

        // The path data is inlined: no <use> or external file is referenced.
        const outer = (await svg.evaluate((node) => node.outerHTML)) ?? '';
        expect(outer).toContain('<path');
        expect(outer).not.toContain('<use');

        // The same markup is shown as source for copy-paste (Prism may split it into
        // token spans, so assert on the raw text content).
        const source = await page.locator('pre').first().textContent();
        expect(source ?? '').toContain('<svg viewBox="0 0 512 512"');
    });
});
