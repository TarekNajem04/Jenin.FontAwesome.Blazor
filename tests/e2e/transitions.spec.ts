import { test, expect } from '@playwright/test';

test.describe('icon transitions', () => {
    test('toggling swaps the visible layer without layout shift', async ({ page }) => {
        await page.goto('/animation');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        const toggler = page.getByRole('button', { name: 'Toggle play state' });
        await toggler.scrollIntoViewIfNeeded();

        const box = page.locator('.transitions-sample__stage').first();
        await expect(box).toBeVisible();

        const layers = box.locator('.jfa-anim-transition-layer');
        const before = await box.boundingBox();

        await expect(layers.nth(0)).toHaveCSS('opacity', '1');
        await expect(layers.nth(1)).toHaveCSS('opacity', '0');

        await toggler.click();

        await expect(layers.nth(0)).toHaveCSS('opacity', '0');
        await expect(layers.nth(1)).toHaveCSS('opacity', '1');
        await expect(page.getByText('Showing: pause')).toBeVisible();

        // The shared grid cell keeps the largest layer's size: no shift.
        const after = await box.boundingBox();
        expect(Math.abs((after?.width ?? 0) - (before?.width ?? 0))).toBeLessThanOrEqual(1);
        expect(Math.abs((after?.height ?? 0) - (before?.height ?? 0))).toBeLessThanOrEqual(1);

        await toggler.click();
        await expect(layers.nth(0)).toHaveCSS('opacity', '1');
        await expect(layers.nth(1)).toHaveCSS('opacity', '0');
    });

    test.describe('reduced motion', () => {
        test.use({ reducedMotion: 'reduce' });

        test('the swap still completes without animation', async ({ page }) => {
            await page.goto('/animation');
            await expect(page.getByTestId('app-interactive')).toBeAttached();

            const toggler = page.getByRole('button', { name: 'Toggle play state' });
            await toggler.scrollIntoViewIfNeeded();

            const box = page.locator('.transitions-sample__stage').first();
            await toggler.click();

            await expect(box.locator('.jfa-anim-transition-layer').nth(1)).toHaveCSS('opacity', '1');
        });
    });
});
