import { expect, test, type Page } from '@playwright/test';

// Prefers-reduced-motion must freeze library-driven animation while leaving
// content visible and interactive. A no-preference context proves the test can detect motion.
async function gotoAnimations(page: Page): Promise<void> {
    await page.goto('/animation');
    await expect(page.getByTestId('app-interactive')).toBeAttached();
}

test.describe('prefers-reduced-motion: reduce', () => {
    test.use({ reducedMotion: 'reduce' });

    test('always-on keyframe animations do not run', async ({ page }) => {
        await gotoAnimations(page);
        const spinner = page.locator('svg[data-testid="always-on-spin"]');
        await expect(spinner).toBeAttached({ timeout: 30000 });
        await expect.poll(async () => spinner.evaluate((el) => getComputedStyle(el).animationName)).toBe('none');
    });

    test('no animation transform progresses over time', async ({ page }) => {
        await gotoAnimations(page);
        const spinner = page.locator('svg[data-testid="always-on-spin"]');
        await expect(spinner).toBeAttached({ timeout: 30000 });
        const first = await spinner.evaluate((el) => getComputedStyle(el).transform);
        await page.waitForTimeout(200);
        const second = await spinner.evaluate((el) => getComputedStyle(el).transform);
        expect(second).toBe(first);
    });

    test('hover triggers do not start animations', async ({ page }) => {
        await gotoAnimations(page);
        const hover = page.locator('.jfa-anim-hover').first();
        await expect(hover).toBeAttached();
        await hover.hover();
        await page.waitForTimeout(200);
        expect(await hover.evaluate((el) => getComputedStyle(el).animationName)).toBe('none');
    });

    test('state transitions still complete (instantly)', async ({ page }) => {
        await gotoAnimations(page);
        const toggler = page.getByRole('button', { name: /toggle play state/i }).first();
        await toggler.scrollIntoViewIfNeeded();
        await toggler.click();
        await expect(page.getByText('Showing: pause').first()).toBeVisible();
    });
});

test.describe('prefers-reduced-motion: no-preference (control)', () => {
    test.use({ reducedMotion: 'no-preference' });

    test('the same spinner animates normally', async ({ page }) => {
        await gotoAnimations(page);
        const spinner = page.locator('svg[data-testid="always-on-spin"]');
        await expect(spinner).toBeAttached({ timeout: 30000 });
        await expect
            .poll(async () => spinner.evaluate((el) => getComputedStyle(el).animationName))
            .not.toBe('none');
    });
});
