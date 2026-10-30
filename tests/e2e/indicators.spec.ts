import { test, expect } from '@playwright/test';

test.describe('indicators', () => {
    test('badges show counts and update with state', async ({ page }) => {
        await page.goto('/icon-indicators');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        const inbox = page.getByRole('button', { name: 'Inbox' });
        await expect(inbox).toBeVisible();

        // The live region announces the count with its unit.
        const badge = page.getByRole('status', { name: '3 unread messages' });
        await expect(badge).toBeVisible();
        await expect(badge).toHaveText('3');

        // Capped and hidden states render as documented.
        await expect(page.getByRole('status').filter({ hasText: '99+' })).toBeVisible();

        // Reading one updates the badge through the new count.
        await inbox.click();
        const updated = page.getByRole('status', { name: '2 unread messages' });
        await expect(updated).toBeVisible();
        await expect(updated).toHaveText('2');
    });

    test('progress ring exposes its value', async ({ page }) => {
        await page.goto('/icon-indicators');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        const ring = page.getByRole('progressbar', { name: 'Upload progress' });
        await expect(ring).toBeVisible();
        await expect(ring).toHaveAttribute('aria-valuenow', '0');

        await page.getByRole('button', { name: 'Advance' }).click();
        await expect(ring).toHaveAttribute('aria-valuenow', '25');

        await page.getByRole('button', { name: 'Advance' }).click();
        await expect(ring).toHaveAttribute('aria-valuenow', '50');
    });

    test('presence dots and avatars render', async ({ page }) => {
        await page.goto('/icon-indicators');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        await expect(page.locator('[role="status"][aria-label="Online"]')).toBeVisible();
        await expect(page.locator('[role="status"][aria-label="Busy"]')).toBeVisible();

        const avatar = page.locator('[role="img"][aria-label="Tarek Najem"]');
        await expect(avatar).toBeVisible();
        await expect(avatar).toContainText('TK');
    });
});
