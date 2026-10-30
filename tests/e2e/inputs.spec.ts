import { test, expect } from '@playwright/test';

test.describe('form inputs', () => {
    test('the icon field validates and binds through the form', async ({ page }) => {
        await page.goto('/icon-inputs');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        // Submitting empty fails validation for both fields. Scoped to the form: the
        // snippet source shown below contains the same strings.
        const form = page.locator('form');
        await form.getByRole('button', { name: 'Submit', exact: true }).click();
        await expect(form.getByText('Pick an icon.', { exact: true })).toBeVisible();
        await expect(form.getByText('Rate 1 to 5.', { exact: true })).toBeVisible();

        // Typing filters the catalogue; arrows move the active option, Enter picks it.
        const field = page.getByRole('combobox', { name: 'Search icons...' });
        await field.fill('house');
        await expect(page.getByRole('listbox', { name: 'Icon results' })).toBeVisible();
        await field.press('ArrowDown');
        await expect(field).toHaveAttribute('aria-activedescendant', /-1$/);
        await field.press('ArrowUp');
        await field.press('Enter');

        await expect(field).toHaveValue('fa-house');

        // The rating is a native radio group: arrow keys move and check.
        const third = page.getByRole('radio', { name: '3 of 5' });
        await third.focus();
        await page.keyboard.press('ArrowRight');
        await expect(page.getByRole('radio', { name: '4 of 5', checked: true })).toBeFocused();

        await page.getByRole('button', { name: 'Submit', exact: true }).click();
        await expect(page.getByText('Icon: fa-house, rating: 4 of 5.')).toBeVisible();
    });

    test('readonly ratings render fractional values', async ({ page }) => {
        await page.goto('/icon-inputs');
        await expect(page.getByTestId('app-interactive')).toBeAttached();

        const average = page.getByRole('img', { name: '3.5 out of 5' });
        await expect(average).toBeVisible();

        // The fractional overlay clips the filled row to 70%.
        const overlay = average.locator('span[style*="overflow:hidden"]').last();
        await expect(overlay).toHaveAttribute('style', /width:70\.00%/);
    });
});
