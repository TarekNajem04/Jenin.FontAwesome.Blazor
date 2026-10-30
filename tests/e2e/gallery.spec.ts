import { expect, test, type Page } from '@playwright/test';

const THEME_KEY = 'jenin-fa-theme';

async function gotoGallery(page: Page, slug: string): Promise<void> {
    await page.goto(`/gallery/${slug}`);
    await expect(page.locator(`[data-testid="gallery-${slug}"]`).first()).toBeVisible();
}

test.describe('gallery screens', () => {
    test('dashboard composes stat cards and a status list', async ({ page }) => {
        await gotoGallery(page, 'dashboard');
        await expect(page.getByText('Operations overview').first()).toBeVisible();
        await expect(page.getByText('12,483').first()).toBeVisible();
        await expect(page.getByText('API gateway').first()).toBeVisible();
        await expect(page.locator('.fa-gauge-high, svg[data-icon="gauge-high"]').first()).toBeAttached();
    });

    test('notification center marks all as read', async ({ page }) => {
        await gotoGallery(page, 'notifications');
        await expect(page.getByText('Deploy finished').first()).toBeVisible();
        await page.getByRole('button', { name: /mark all read/i }).click();
        await expect(page.getByText('All caught up').first()).toBeVisible();
    });

    test('timeline renders dated entries with icons', async ({ page }) => {
        await gotoGallery(page, 'timeline');
        await expect(page.getByText('Activity timeline').first()).toBeVisible();
        await expect(page.getByText('october.csv').first()).toBeVisible();
        await expect(page.locator('.fa-flag-checkered, svg[data-icon="flag-checkered"]').first()).toBeAttached();
    });

    test('settings toggles flip with accessible names', async ({ page }) => {
        await gotoGallery(page, 'settings');
        const push = page.getByRole('button', { name: /toggle push notifications/i });
        await expect(push).toHaveAttribute('aria-pressed', 'true');
        await push.click();
        await expect(push).toHaveAttribute('aria-pressed', 'false');
        const pushOff = page.getByText("Push: off", { exact: false }).first();
        await expect(pushOff).toBeVisible();
    });

    test('inbox deletes rows and empties to zero state', async ({ page }) => {
        await gotoGallery(page, 'inbox');
        await expect(page.locator('[data-testid^="inbox-row-"]')).toHaveCount(3);
        for (const id of [1, 2, 3]) {
            await page.locator(`[data-testid="inbox-row-${id}"] button[title="Delete"]`).click();
            await expect(page.locator('[data-testid^="inbox-row-"]')).toHaveCount(3 - id);
        }
        await expect(page.getByText('Inbox zero').first()).toBeVisible();
    });
});

test.describe('gallery responsive and themed', () => {
    test('dashboard fits a mobile viewport with the drawer toggle', async ({ page }) => {
        await page.setViewportSize({ width: 480, height: 900 });
        await gotoGallery(page, 'dashboard');
        await expect(page.getByText('Operations overview').first()).toBeVisible();
        const overflow = await page.evaluate(() => document.documentElement.scrollWidth - document.documentElement.clientWidth);
        expect(overflow).toBeLessThanOrEqual(1);
        await expect(page.getByRole('button', { name: /open navigation/i })).toBeVisible();
    });

    test('settings honors the dark theme', async ({ page }) => {
        await page.addInitScript((key) => window.localStorage.setItem(key, 'dark'), THEME_KEY);
        await gotoGallery(page, 'settings');
        await expect.poll(async () => page.evaluate(() => document.documentElement.getAttribute('data-theme'))).toBe('dark');
        await expect(page.getByText('Notification settings').first()).toBeVisible();
        const background = await page.evaluate(() => getComputedStyle(document.body).backgroundColor);
        expect(background).not.toBe('rgb(255, 255, 255)');
    });
});
