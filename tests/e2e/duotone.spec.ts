import { test, expect } from '@playwright/test';

test.describe('themed duotone palettes', () => {
    test('the palette swaps layer colors with the site theme', async ({ page }) => {
        await page.addInitScript(() => window.localStorage.setItem('jenin-fa-theme', 'light'));
        await page.goto('/duotone');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        const themed = page.locator('.jfa-duotone-theme').first();
        await expect(themed).toBeVisible();
        await expect(themed).toHaveCSS('--fa-primary-color', '#0d6efd');

        await page.locator('.site-theme-toggle').click();
        await expect(page.locator('.jfa-duotone-theme').first()).toHaveCSS(
            '--fa-primary-color', '#8bb9fe');
    });

    test('an explicit icon color wins over the inherited palette', async ({ page }) => {
        await page.goto('/duotone');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        const themed = page.locator('.jfa-duotone-theme').first();
        await expect(themed).toBeVisible();

        const inherited = themed.locator('i.fa-circle-info, svg.fa-circle-info').first();
        await expect(inherited).toHaveCSS('--fa-primary-color', '#0d6efd');

        const explicit = page.locator('.jfa-duotone-theme', { has: page.locator('i.fa-circle-exclamation, svg.fa-circle-exclamation') }).first();
        await expect(explicit.locator('i.fa-circle-exclamation, svg.fa-circle-exclamation')).toHaveCSS('--fa-primary-color', '#e11d48');
    });
});
