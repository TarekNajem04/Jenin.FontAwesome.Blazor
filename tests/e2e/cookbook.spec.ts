import { expect, test } from '@playwright/test';

// Every cookbook recipe renders its demo; the snippet source is shown
// automatically by the shared <Sample> wrapper from the same file.
test.beforeEach(async ({ page }) => {
    await page.goto('/cookbook');
    await expect(page.getByRole('heading', { name: 'Cookbook' })).toBeVisible();
});

type TextCheck = { kind: 'text'; text: string };
type AttrCheck = { kind: 'attr'; selector: string; name: string; value: string; exact: boolean };
type Check = TextCheck | AttrCheck;

const recipes: Array<{ id: string; checks: Check[] }> = [
    { id: 'recipe-busy-button', checks: [{ kind: 'text', text: 'Upload' }] },
    {
        id: 'recipe-notification-badge',
        checks: [{ kind: 'attr', selector: '[role="status"]', name: 'aria-label', value: '3 unread messages', exact: true }],
    },
    { id: 'recipe-empty-state', checks: [{ kind: 'text', text: 'Nothing here yet' }] },
    { id: 'recipe-icon-list', checks: [{ kind: 'text', text: 'Out for delivery' }] },
    { id: 'recipe-stat-card', checks: [{ kind: 'text', text: '12,483' }] },
    {
        id: 'recipe-animated-trigger',
        checks: [{ kind: 'attr', selector: '[class*="fa-shake"]', name: 'class', value: 'fa-shake', exact: false }],
    },
    {
        id: 'recipe-duotone-theme',
        checks: [{ kind: 'attr', selector: '[class*="fa-duotone"]', name: 'class', value: 'fa-duotone', exact: false }],
    },
    {
        id: 'recipe-accessible-icon',
        checks: [{ kind: 'attr', selector: '[title="Home"]', name: 'role', value: 'img', exact: true }],
    },
    {
        id: 'recipe-icon-toggle',
        checks: [{ kind: 'attr', selector: 'button', name: 'aria-pressed', value: 'false', exact: true }],
    },
    { id: 'recipe-avatar-status', checks: [{ kind: 'text', text: 'TJ' }] },
    { id: 'recipe-rating-input', checks: [{ kind: 'text', text: 'Your rating' }] },
    {
        id: 'recipe-two-state',
        checks: [{ kind: 'attr', selector: '[class*="fa-play"]', name: 'class', value: 'fa-play', exact: false }],
    },
    { id: 'recipe-share-buttons', checks: [{ kind: 'text', text: 'Share' }] },
    { id: 'recipe-timeline', checks: [{ kind: 'text', text: 'expected tomorrow' }] },
    { id: 'recipe-toast', checks: [{ kind: 'text', text: 'Saved!' }] },
];

for (const recipe of recipes) {
    test(`cookbook: ${recipe.id} renders its demo`, async ({ page }) => {
        const section = page.locator(`[data-testid="${recipe.id}"]`);
        await expect(section).toBeVisible();
        for (const check of recipe.checks) {
            if (check.kind === 'text') {
                await expect(section).toContainText(check.text);
            } else {
                const actual = (await section.locator(check.selector).first().getAttribute(check.name)) ?? '';
                if (check.exact) {
                    expect(actual).toBe(check.value);
                } else {
                    expect(actual).toContain(check.value);
                }
            }
        }
    });
}
