import { test, expect } from '@playwright/test';

test.describe('icon buttons', () => {
    test('buttons render with accessible names and decorative glyphs', async ({ page }) => {
        await page.goto('/icon-buttons');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        await expect(page.getByRole('button', { name: 'Search' })).toBeVisible();
        await expect(page.getByRole('button', { name: 'Home' })).toBeVisible();

        // The glyph inside a button is decorative: the button exposes exactly one name.
        const search = page.getByRole('button', { name: 'Search' });
        await expect(search.locator('i[aria-hidden="true"]')).toHaveCount(1);

        const disabled = page.getByRole('button', { name: 'Download' });
        await expect(disabled).toBeDisabled();
        await expect(disabled).toHaveAttribute('title', 'Download (unavailable)');
    });

    test('toggle buttons flip pressed state and glyph', async ({ page }) => {
        await page.goto('/icon-buttons');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        const notifications = page.getByRole('button', { name: 'Enable notifications' });
        await expect(notifications).toHaveAttribute('aria-pressed', 'false');
        await expect(notifications.locator('svg.fa-bell, i.fa-bell')).toHaveCount(1);

        await notifications.click();

        const pressed = page.getByRole('button', { name: 'Disable notifications' });
        await expect(pressed).toHaveAttribute('aria-pressed', 'true');
        await expect(pressed.locator('svg.fa-bell-slash, i.fa-bell-slash')).toHaveCount(1);

        // The favorite preset fills red when pressed.
        const favorite = page.getByRole('button', { name: 'Add to favorites' });
        await favorite.click();

        const saved = page.getByRole('button', { name: 'Remove from favorites' });
        await expect(saved).toHaveAttribute('aria-pressed', 'true');
        await expect(saved.locator('svg.fa-heart, i.fa-heart')).toHaveCount(1);
    });

    test('busy buttons disable without moving the label', async ({ page }) => {
        await page.goto('/icon-buttons');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        const staticBusy = page.getByRole('button', { name: 'Save' });
        await expect(staticBusy).toBeDisabled();
        await expect(staticBusy).toHaveAttribute('aria-busy', 'true');

        const upload = page.getByRole('button', { name: 'Upload' });
        const box = await upload.boundingBox();
        await upload.click();

        await expect(upload).toHaveAttribute('aria-busy', 'true');
        await expect(upload).toBeDisabled();

        // The label is preserved while busy, so the button does not shift.
        const busyBox = await upload.boundingBox();
        expect(box).not.toBeNull();
        expect(busyBox).not.toBeNull();
        expect(Math.abs(busyBox!.width - box!.width)).toBeLessThanOrEqual(2);

        await expect(upload).not.toHaveAttribute('aria-busy', 'true', { timeout: 10_000 });
    });
});
