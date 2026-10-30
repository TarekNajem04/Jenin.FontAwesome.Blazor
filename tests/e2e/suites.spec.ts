import { expect, test } from '@playwright/test';

// The same typed names render through the suites' own icon components.
// The fixture serves vendored FA 7.3.1 CSS, so glyphs resolve with no live dependencies.

test.describe('mudblazor adapter page', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/mud');
        await expect(page.locator('h1').first()).toContainText('MudBlazor adapter');
    });

    test('typed names render through MudIcon', async ({ page }) => {
        const house = page.locator('[data-testid="m-house"] span').first();
        await expect(house).toBeAttached();
        await expect(house).toHaveClass(/mud-icon-root/);
        await expect(house).toHaveClass(/fa-solid/);
        await expect(house).toHaveClass(/fa-house/);
        await expect(house).toHaveAttribute('title', 'Home');
    });

    test('alias names pass through untouched', async ({ page }) => {
        const github = page.locator('[data-testid="m-github"] span').first();
        await expect(github).toHaveClass(/fa-brands/);
        await expect(github).toHaveClass(/fa-github/);
    });

    test('custom kit names render without a mapped style', async ({ page }) => {
        const custom = page.locator('[data-testid="m-custom"] span').first();
        await expect(custom).toBeAttached();
        await expect(custom).toHaveClass(/fa-solid/);
        await expect(custom).toHaveClass(/fa-acme-widget/);
    });

    test('glyphs resolve (vendored Font Awesome CSS is served)', async ({ page }) => {
        const content = await page
            .locator('[data-testid="m-house"] span')
            .first()
            .evaluate((el) => getComputedStyle(el, '::before').content);
        expect(content).not.toBe('none');
    });
});

test.describe('blazorise adapter page', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/blazorise');
        await expect(page.locator('h1').first()).toContainText('Blazorise adapter');
    });

    test('typed names render through the Blazorise Icon', async ({ page }) => {
        const phone = page.locator('[data-testid="b-phone"] i').first();
        await expect(phone).toBeAttached();
        const cls = (await phone.getAttribute('class')) ?? '';
        expect(cls).toContain('fa-phone');
        // Large maps to the Blazorise fa-lg token.
        expect(cls).toContain('fa-lg');
    });

    test('custom kit names pass through', async ({ page }) => {
        const custom = page.locator('[data-testid="b-custom"] i').first();
        await expect(custom).toBeAttached();
        expect((await custom.getAttribute('class')) ?? '').toContain('fa-acme-widget');
    });

    test('glyphs resolve (provider CSS is served)', async ({ page }) => {
        const content = await page
            .locator('[data-testid="b-phone"] i')
            .first()
            .evaluate((el) => getComputedStyle(el, '::before').content);
        expect(content).not.toBe('none');
    });
});
