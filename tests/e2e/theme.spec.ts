import { test, expect, type Page } from '@playwright/test';

const THEME_KEY = 'jenin-fa-theme';

/**
 * Waits until the Blazor circuit is live. The page is prerendered, so a click that lands before the
 * circuit connects is lost; the layout marks the moment it becomes interactive.
 */
async function waitForInteractive(page: Page): Promise<void> {
    await expect(page.getByTestId('app-interactive')).toBeAttached();
}

async function currentTheme(page: Page): Promise<string | null> {
    return page.evaluate(() => document.documentElement.getAttribute('data-theme'));
}

async function bodyBackground(page: Page): Promise<string> {
    return page.evaluate(() => getComputedStyle(document.body).backgroundColor);
}

test.describe('site theme', () => {
    test('applies the stored theme before the first paint', async ({ page }) => {
        await page.addInitScript((key) => window.localStorage.setItem(key, 'dark'), THEME_KEY);

        await page.goto('/');

        await expect.poll(() => currentTheme(page)).toBe('dark');
    });

    test('falls back to the operating-system preference when nothing is stored', async ({ browser }) => {
        const context = await browser.newContext({ colorScheme: 'dark' });
        const page = await context.newPage();

        await page.goto('/');

        await expect.poll(() => currentTheme(page)).toBe('dark');
        await context.close();
    });

    test('the toggle switches the theme and persists the choice', async ({ page }) => {
        // Seed only when empty: the same init script runs again on reload, and it must not overwrite
        // the choice the toggle just persisted.
        await page.addInitScript((key) => {
            if (!window.localStorage.getItem(key)) {
                window.localStorage.setItem(key, 'light');
            }
        }, THEME_KEY);
        await page.goto('/');
        await waitForInteractive(page);

        await expect.poll(() => currentTheme(page)).toBe('light');

        const lightBackground = await bodyBackground(page);

        await page.getByRole('button', { name: /switch to dark theme/i }).click();

        await expect.poll(() => currentTheme(page)).toBe('dark');
        expect(await page.evaluate((key) => window.localStorage.getItem(key), THEME_KEY)).toBe('dark');
        await expect.poll(() => bodyBackground(page)).not.toBe(lightBackground);

        // A reload keeps the choice.
        await page.reload();
        await waitForInteractive(page);
        await expect.poll(() => currentTheme(page)).toBe('dark');
    });

    test('the theme survives navigation between pages', async ({ page }) => {
        await page.addInitScript((key) => window.localStorage.setItem(key, 'dark'), THEME_KEY);
        await page.goto('/');

        await page.getByRole('link', { name: /icon explorer/i }).first().click();

        await expect(page).toHaveURL(/\/explorer/);
        await expect.poll(() => currentTheme(page)).toBe('dark');
    });

    test('the explorer inherits the site theme instead of keeping its own', async ({ page }) => {
        await page.addInitScript((key) => window.localStorage.setItem(key, 'dark'), THEME_KEY);
        await page.goto('/explorer');

        await expect.poll(() => currentTheme(page)).toBe('dark');

        // The explorer stamps the inherited theme on its own root so its palette follows the host.
        await expect.poll(() => page.evaluate(() => document.querySelector('.explorer')?.getAttribute('data-theme'))).toBe('dark');

        // The host owns the theme, so the explorer does not show a second switch.
        await expect(page.locator('.explorer-theme-toggle')).toHaveCount(0);
    });
});

test.describe('responsive navigation', () => {
    const navToggle = (page: Page) => page.locator('.app-header__menu-toggle');

    test('the sidebar is a drawer on narrow viewports', async ({ page }) => {
        await page.setViewportSize({ width: 480, height: 900 });
        await page.goto('/');
        await waitForInteractive(page);

        const toggle = navToggle(page);
        await expect(toggle).toBeVisible();
        await expect(toggle).toHaveAccessibleName(/open navigation/i);

        const sidebar = page.locator('#app-sidebar');
        await expect(sidebar).toBeHidden();

        // The desktop collapse control stays out of the way while the drawer toggle owns the
        // narrow layout.
        await expect(page.locator('.app-header__sidebar-toggle')).toBeHidden();

        await toggle.click();

        await expect(toggle).toHaveAttribute('aria-expanded', 'true');
        await expect(sidebar).toBeVisible();
    });

    test('navigating from the drawer closes it', async ({ page }) => {
        await page.setViewportSize({ width: 480, height: 900 });
        await page.goto('/');
        await waitForInteractive(page);

        await navToggle(page).click();
        await page.locator('#app-sidebar').getByRole('link', { name: /animation/i }).first().click();

        await expect(page).toHaveURL(/\/animation/);
        await expect(navToggle(page)).toHaveAttribute('aria-expanded', 'false');
    });

    test('navigating to another example resets the content scroll position', async ({ page }) => {
        await page.setViewportSize({ width: 1440, height: 900 });
        await page.goto('/');
        await waitForInteractive(page);

        const content = page.locator('.app-content');
        await content.evaluate((element) => { element.scrollTop = 600; });
        await expect.poll(async () => content.evaluate((element) => element.scrollTop)).toBeGreaterThan(0);

        await page.locator('#app-sidebar').getByRole('link', { name: /stacking/i }).first().click();
        await expect(page).toHaveURL(/\/stacking/);

        await expect.poll(async () => content.evaluate((element) => element.scrollTop), { timeout: 10_000 })
            .toBe(0);
    });

    test('the sidebar can be collapsed on wide viewports', async ({ page }) => {
        await page.setViewportSize({ width: 1440, height: 900 });
        await page.goto('/');
        await waitForInteractive(page);

        const toggle = page.locator('.app-header__sidebar-toggle');
        const sidebar = page.locator('#app-sidebar');
        await expect(toggle).toBeVisible();
        await expect(sidebar).toBeVisible();

        await toggle.click();
        await expect(page.locator('.app-shell')).toHaveClass(/is-sidebar-collapsed/);
        await expect(sidebar).toBeHidden();
        await expect(toggle).toHaveAttribute('aria-expanded', 'false');

        await toggle.click();
        await expect(page.locator('.app-shell')).not.toHaveClass(/is-sidebar-collapsed/);
        await expect(sidebar).toBeVisible();
        await expect(toggle).toHaveAttribute('aria-expanded', 'true');
    });

    test('the toggle is hidden on wide viewports', async ({ page }) => {
        await page.setViewportSize({ width: 1440, height: 900 });
        await page.goto('/');

        await expect(navToggle(page)).toBeHidden();
        await expect(page.locator('#app-sidebar')).toBeVisible();
    });
});
