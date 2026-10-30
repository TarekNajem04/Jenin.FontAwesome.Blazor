import { test, expect } from '@playwright/test';

// Animation triggers gate FA keyframes behind interaction. Playwright runs without a
// reduced-motion preference so the paused/running play states are observable; the shipped CSS
// still disables the keyframes under prefers-reduced-motion.
test.use({ reducedMotion: 'no-preference' });

test.describe('animation triggers', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/animation');
        await expect(page.getByTestId('app-interactive')).toBeAttached();
        await page.locator('.sample__preview').first().waitFor();
        await page.waitForTimeout(1000);
    });

    test('hover and parent-hover start the paused animation', async ({ page }) => {
        const scope = page.locator('.tab-pane.active');
        const bell = scope.locator('span[title="Hover me"] svg').first();
        const card = scope.locator('.jfa-anim-hover-within');
        const heart = card.locator('svg').first();

        await expect(bell).toHaveCSS('animation-play-state', 'paused');
        await bell.hover();
        await expect(bell).toHaveCSS('animation-play-state', 'running');

        await expect(heart).toHaveCSS('animation-play-state', 'paused');
        await card.hover();
        await expect(heart).toHaveCSS('animation-play-state', 'running');
    });

    test('keyboard focus starts the paused animation', async ({ page }) => {
        const scope = page.locator('.tab-pane.active');
        const plus = scope.locator('svg.jfa-anim-focus').first();

        await expect(plus).toHaveCSS('animation-play-state', 'paused');
        await plus.focus();
        await expect(plus).toHaveCSS('animation-play-state', 'running');

        await plus.blur();
        await expect(plus).toHaveCSS('animation-play-state', 'paused');
    });

    test('scrolling reveals the hidden icon', async ({ page }) => {
        const scope = page.locator('.tab-pane.active');
        const rocket = scope.locator('svg.fa-rocket').first();

        await expect(rocket).toHaveCSS('opacity', '0');

        await rocket.scrollIntoViewIfNeeded();
        await expect(rocket).toHaveClass(/jfa-anim-reveal-visible/);
        await expect(rocket).toHaveCSS('opacity', '1');
        await expect(rocket).toHaveCSS('animation-play-state', 'running');
    });
});
