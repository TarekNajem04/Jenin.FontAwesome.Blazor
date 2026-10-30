import { test, expect } from '@playwright/test';

test.describe('content blocks', () => {
    test('stat cards expose values and deltas', async ({ page }) => {
        await page.goto('/content-blocks');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        const revenue = page.getByRole('group', { name: 'Revenue: $8,420' });
        await expect(revenue).toBeVisible();
        await expect(revenue).toContainText('$8,420');
        await expect(revenue.locator('i.fa-cart-shopping, svg.fa-cart-shopping')).toHaveCount(1);
    });

    test('empty state and activity feed render', async ({ page }) => {
        await page.goto('/content-blocks');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        const preview = page.locator('.sample__preview');
        await expect(preview.getByText('Try a different search term or clear the filters.', { exact: true })).toBeVisible();
        await expect(preview.getByRole('button', { name: 'Clear filters' })).toBeVisible();

        await expect(preview.getByText('Sara joined the design team.')).toBeVisible();
        await expect(preview.getByText('Monday')).toBeVisible();
    });

    test('share buttons link the documented intents', async ({ page }) => {
        await page.goto('/content-blocks');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        const group = page.getByRole('group', { name: 'Share' });
        await expect(group).toBeVisible();

        await expect(group.getByRole('link', { name: 'Share on X' }))
            .toHaveAttribute('href', /twitter\.com\/intent\/tweet\?url=.*&text=/);
        await expect(group.getByRole('link', { name: 'Share on LinkedIn' }))
            .toHaveAttribute('href', /linkedin\.com\/sharing\/share-offsite\/\?url=/);
        await expect(group.getByRole('link', { name: 'Share by email' }))
            .toHaveAttribute('href', /^mailto:\?subject=.*&body=/);

        for (const name of ['Share on X', 'Share on LinkedIn', 'Share on WhatsApp', 'Share on Telegram', 'Share by email']) {
            await expect(group.getByRole('link', { name, exact: true })).toHaveAttribute('target', '_blank');
        }
    });

    test('enum status icons render without switches', async ({ page }) => {
        await page.goto('/content-blocks');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        const preview = page.locator('.sample__preview');
        await expect(preview.getByText('Order #102 — Shipped')).toBeVisible();
        await expect(preview.locator('svg.fa-truck, i.fa-truck')).toHaveCount(1);
        await expect(preview.locator('svg.fa-circle-check, i.fa-circle-check')).toHaveCount(1);

        // The unmapped status falls back to the question glyph.
        await expect(preview.getByText('Order #104 — Cancelled')).toBeVisible();
        await expect(preview.locator('svg.fa-circle-question, i.fa-circle-question')).toHaveCount(1);
    });
});
