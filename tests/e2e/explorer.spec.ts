import { test, expect, Page } from '@playwright/test';

// The explorer is an interactive Blazor page: the circuit attaches after the prerendered markup
// paints, and the result grid is virtualized (only the visible slice of cards exists in the DOM).
// The helpers below wait for the circuit to become live and drive the real controls, re-clicking
// until the expected DOM state lands so a cold host cannot flake the suite. Global shortcuts are
// dispatched from an in-page control because the handler lives on the explorer root.

async function waitForGrid(page: Page): Promise<void> {
    await expect(page.locator('.explorer-grid')).toBeVisible();
    await expect(page.locator('.explorer-card').first()).toBeVisible();
    await expect(page.getByTestId('app-interactive')).toBeAttached();
}

async function search(page: Page, query: string): Promise<void> {
    await page.locator('#ctl-query').fill(query);
    // The query is debounced, so wait for the URL to reflect it before asserting the grid.
    await expect(page).toHaveURL(new RegExp(`[?&]q=${encodeURIComponent(query)}`));
}

async function selectIcon(page: Page, name: string): Promise<void> {
    await search(page, name);

    const previewCode = page.locator('.explorer-preview__class code');
    await expect
        .poll(async () => {
            // Re-query each attempt: a cold circuit can swallow the first activation, and the
            // virtualized grid may replace the card element between attempts.
            const card = page.locator(`.explorer-card[data-icon="${name}"]`);
            if ((await card.count()) === 0) {
                return null;
            }

            await card.first().scrollIntoViewIfNeeded().catch(() => undefined);
            await card.first().click().catch(() => undefined);
            return previewCode.textContent().catch(() => null);
        }, { timeout: 20_000 })
        .toContain(`fa-${name}`);
}

test.describe('icon explorer', () => {
    test('renders the catalogue with a populated grid', async ({ page }) => {
        await page.goto('/explorer');
        await waitForGrid(page);

        await expect(page).toHaveTitle(/Icon Explorer/i);
        // The toolbar exposes search, the style facet, and the shortcuts action.
        await expect(page.locator('#ctl-query')).toBeVisible();
        await expect(page.locator('.explorer-filter[role="group"]')).toBeVisible();
        await expect(page.locator('.explorer-help-toggle')).toBeVisible();
        // The host owns the theme here, so the explorer inherits it and hides its own switch.
        await expect(page.locator('.explorer-theme-toggle')).toHaveCount(0);
    });

    test('search narrows the results', async ({ page }) => {
        await page.goto('/explorer');
        await waitForGrid(page);

        await search(page, 'house');

        const card = page.locator('.explorer-card[data-icon="house"]');
        await expect(card).toHaveCount(1);
        await expect(card).toBeVisible();
        // A non-matching icon drops out of the result grid.
        await expect(page.locator('.explorer-card[data-icon="github"]')).toHaveCount(0);
    });

    test('search with no matches shows the empty state', async ({ page }) => {
        await page.goto('/explorer');
        await waitForGrid(page);

        await page.locator('#ctl-query').fill('zzzznotarealicon');

        await expect(page.locator('.explorer-grid--empty')).toBeVisible();
        await expect(page.locator('.explorer-grid__empty-title')).toContainText('No icons match');
    });

    test('style facet filters the catalogue and updates the URL', async ({ page }) => {
        await page.goto('/explorer');
        await waitForGrid(page);

        const brandsButton = page.locator('.explorer-filter button', { hasText: 'Brands' });
        await expect
            .poll(async () => {
                if (!page.url().includes('style=brands')) {
                    await brandsButton.click();
                }
                return page.url();
            }, { timeout: 15_000 })
            .toContain('style=brands');

        // The Brands facet is now the active option.
        await expect(brandsButton).toHaveClass(/is-active/);
        await expect(page.locator('.explorer-filter button', { hasText: 'All' })).not.toHaveClass(/is-active/);

        // With the brands facet active, a brands icon is shown and a solid-only icon is filtered out.
        await search(page, 'github');
        await expect(page.locator('.explorer-card[data-icon="github"]')).toHaveCount(1);
        await search(page, 'house');
        await expect(page.locator('.explorer-card[data-icon="house"]')).toHaveCount(0);
    });

    test('selecting an icon shows details and updates the URL', async ({ page }) => {
        await page.goto('/explorer');
        await waitForGrid(page);

        await selectIcon(page, 'house');

        await expect(page.locator('.explorer-preview__class code')).toHaveText('fa-house');
        await expect(page.locator('.explorer-preview__attribution')).toContainText('CC BY 4.0');
        await expect(page).toHaveURL(/[?&]icon=house/);
    });

    test('a deep link restores the selected icon', async ({ page }) => {
        await page.goto('/explorer?icon=github&style=brands');

        await expect(page.locator('.explorer-preview__class code')).toHaveText(/fa-github/);
        // The brands facet is restored from the URL.
        await expect(page.locator('.explorer-filter button', { hasText: 'Brands' })).toHaveClass(/is-active/);
    });

    test('the "/" shortcut focuses the search field', async ({ page }) => {
        await page.goto('/explorer');
        await waitForGrid(page);

        // Focus a non-input control inside the explorer so the global keydown has an in-page target.
        await page.locator('.explorer-help-toggle').focus();
        await page.keyboard.press('/');

        await expect(page.locator('#ctl-query')).toBeFocused();
    });

    test('the "?" shortcut opens the keyboard help dialog', async ({ page }) => {
        await page.goto('/explorer');
        await waitForGrid(page);

        await page.locator('.explorer-help-toggle').focus();
        await page.keyboard.press('?');

        const dialog = page.locator('.explorer-help[role="dialog"]');
        await expect(dialog).toBeVisible();
        await expect(dialog).toContainText('Keyboard shortcuts');

        await page.locator('.explorer-help__close').click();
        await expect(dialog).toBeHidden();
    });

    test('the explorer follows the site theme switch', async ({ page }) => {
        await page.addInitScript(() => window.localStorage.setItem('jenin-fa-theme', 'light'));
        await page.goto('/explorer');
        await waitForGrid(page);

        const explorer = page.locator('.explorer');
        const hostToggle = page.locator('.site-theme-toggle');

        await expect(explorer).toHaveAttribute('data-theme', 'light');

        await hostToggle.click();

        // The explorer mirrors the host theme on its own root so its palette follows along.
        await expect(explorer).toHaveAttribute('data-theme', 'dark');
        const surface = await page.evaluate(() =>
            getComputedStyle(document.querySelector('.explorer') as Element).getPropertyValue('--ex-surface').trim());
        expect(surface).toBe('#22272e');
    });

    test('arrow keys move focus within the grid', async ({ page }) => {
        await page.goto('/explorer');
        await waitForGrid(page);

        const cards = page.locator('.explorer-card');
        await cards.first().focus();
        const firstIcon = await cards.first().getAttribute('data-icon');

        await page.keyboard.press('ArrowRight');

        await expect
            .poll(async () => page.locator('.explorer-card:focus').getAttribute('data-icon'), { timeout: 10_000 })
            .not.toBe(firstIcon);
    });

    test('the animation duration slider steps by 0.1 and resets when the animation changes', async ({ page }) => {
        await page.goto('/explorer?icon=house');
        await waitForGrid(page);

        await page.locator('#ctl-anim').selectOption('Beat');
        await page.locator('#ctl-use-dur').check();

        const animGroup = page.locator('.explorer-controls__group', { hasText: 'Animation' });
        const duration = animGroup.locator('input[type=range]').first();
        // locator.press() focuses before dispatching, closing the focus-then-act race that
        // leaves the value at 1.0 on loaded runners (separate focus() + keyboard.press() can
        // straddle a Blazor re-render and drop focus).
        await duration.press('ArrowRight');

        // The slider must move by its 0.1 step, not jump to the maximum, regardless of host culture.
        await expect.poll(async () => Number.parseFloat(await duration.inputValue()), { timeout: 10_000 })
            .toBeCloseTo(1.1, 2);
        await expect(page.locator('.explorer-preview__canvas svg').first())
            .toHaveAttribute('style', /--fa-animation-duration:\s*1100ms/);

        // Switching to another animation restores the options to their defaults.
        await page.locator('#ctl-anim').selectOption('Spin');
        await expect(page.locator('#ctl-use-dur')).not.toBeChecked();
        await expect.poll(async () => Number.parseFloat(await duration.inputValue()), { timeout: 10_000 })
            .toBeCloseTo(1.0, 2);
    });

    test('selecting a different icon resets the preview options', async ({ page }) => {
        await page.goto('/explorer?icon=house');
        await waitForGrid(page);

        await page.locator('#ctl-use-color').check();
        await expect(page.locator('#ctl-use-color')).toBeChecked();

        await selectIcon(page, 'star');

        await expect(page.locator('#ctl-use-color')).not.toBeChecked();
    });

    test('clearing or resetting the power transform removes it from the preview', async ({ page }) => {
        await page.goto('/explorer?icon=house');
        await waitForGrid(page);

        const canvas = page.locator('.explorer-preview__canvas');
        const transformToggle = page.locator('#ctl-use-transform');

        await transformToggle.check();
        await expect(canvas.locator('[data-fa-transform]').first()).toBeAttached();

        // Unchecking the option must remove the attribute instead of leaving the last transform applied.
        await transformToggle.uncheck();
        await expect(canvas.locator('[data-fa-transform]')).toHaveCount(0);

        // Selecting another icon resets the state, and the reset button must clear it too.
        await transformToggle.check();
        await selectIcon(page, 'star');
        await expect(canvas.locator('[data-fa-transform]')).toHaveCount(0);

        await transformToggle.check();
        await page.locator('.explorer-controls__reset').click();
        await expect(transformToggle).not.toBeChecked();
        await expect(canvas.locator('[data-fa-transform]')).toHaveCount(0);
    });

    test('changing the style filter resets the grid scroll position', async ({ page }) => {
        await page.goto('/explorer');
        await waitForGrid(page);

        const grid = page.locator('.explorer-grid');
        await grid.evaluate((element) => { element.scrollTop = 800; });
        await expect.poll(async () => grid.evaluate((element) => element.scrollTop)).toBeGreaterThan(0);

        const brandsButton = page.locator('.explorer-filter button', { hasText: 'Brands' });
        await expect
            .poll(async () => {
                if (!page.url().includes('style=brands')) {
                    await brandsButton.click();
                }
                return page.url();
            }, { timeout: 15_000 })
            .toContain('style=brands');

        await expect.poll(async () => grid.evaluate((element) => element.scrollTop), { timeout: 10_000 })
            .toBe(0);
    });
});
