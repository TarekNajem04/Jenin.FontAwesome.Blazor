import { test, expect, Page } from '@playwright/test';

// The Blazor circuit attaches asynchronously after the prerendered markup paints, so an early
// click may be swallowed. Re-clicking until the library's re-render lands makes the check
// independent of circuit boot timing and keeps a cold host from flaking the suite.
async function toggleChecked(page: Page): Promise<void> {
    const square = page.locator('svg[data-icon="square"]');
    await expect(square).toHaveCount(1);
    const squareCheck = page.locator('svg[data-icon="square-check"]');
    await expect
        .poll(async () => {
            if ((await squareCheck.count()) === 0) {
                await square.click();
            }
            return squareCheck.count();
        }, { timeout: 15_000 })
        .toBe(1);
    await expect(page.locator('svg[data-icon="square"]')).toHaveCount(0);
    await expect(squareCheck).toBeVisible();
}

test('parameter changes re-enhance through the Blazor circuit', async ({ page }) => {
    await page.goto('/events');
    await toggleChecked(page);

    // The digits row (Font Awesome 1-5 icons) is re-emitted after the counter update and
    // must re-enhance as well.
    await expect(page.locator('svg[data-icon="2"]')).toHaveCount(1);
    await expect(page.locator('svg[data-icon="2"]')).toBeVisible();
});

test('the Blazor error UI stays hidden while the circuit is healthy', async ({ page }) => {
    await page.goto('/events');
    await toggleChecked(page);

    await expect(page.locator('#blazor-error-ui')).toHaveCSS('display', 'none');
});
