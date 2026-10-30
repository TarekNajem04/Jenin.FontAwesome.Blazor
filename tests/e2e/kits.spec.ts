import { expect, test } from '@playwright/test';

// The Kit demo page emits the Kit runtime tag with the configured code and no
// live Kit is needed (the demo code 404s harmlessly; only the tag is asserted).
test.describe('kit demo page', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/kit-demo');
        await expect(page.getByRole('heading', { name: 'Font Awesome Kit demo' })).toBeVisible();
    });

    test('emits the kit script tag', async ({ page }) => {
        const kit = page.locator('script[src="https://kit.fontawesome.com/demo-kit-code-0000.js"]');
        await expect(kit).toHaveCount(1);
    });

    test('keeps the mode-config script ahead of the kit runtime', async ({ page }) => {
        const scripts = await page.locator('head script[src], body script[src]').evaluateAll((els) => els.map((el) => el.getAttribute('src') ?? ''));
        const config = scripts.findIndex((src) => src.includes('fontawesome-config'));
        const kit = scripts.findIndex((src) => src.includes('kit.fontawesome.com/demo-kit-code-0000.js'));
        expect(config).toBeGreaterThanOrEqual(0);
        expect(kit).toBeGreaterThan(config);
    });

    test('kit-custom names survive the engine as data-icon anchors', async ({ page }) => {
        // Unknown names are not blocked: the engine normalizes them to (empty) SVGs that keep
        // the name in data-icon, which is exactly what a kit resolves at runtime.
        await expect(page.locator('svg[data-icon="acme-widget"]')).toHaveCount(1);
    });
});
