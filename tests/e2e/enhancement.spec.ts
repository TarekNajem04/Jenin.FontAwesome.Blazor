import { test, expect } from '@playwright/test';

test('transforms survive the DOM replacement', async ({ page }) => {
    await page.goto('/transforms');

    // The seedling container renders its shrink transform through Font Awesome's
    // `data-fa-transform` token; the enhanced SVG keeps it as a DOM attribute.
    const seedlings = page.locator('svg.svg-inline--fa.fa-seedling[data-fa-transform]');
    await expect(seedlings.first()).toBeVisible();
    await expect(seedlings.first()).toHaveAttribute('data-fa-transform', /shrink/i);

    const transformed = page.locator('[data-fa-transform]');
    expect(await transformed.count()).toBeGreaterThanOrEqual(60);
});

test('masking tokens survive the DOM replacement', async ({ page }) => {
    await page.goto('/masking');

    // Masked icons emit `data-fa-mask`; the enhanced SVG keeps the token even though the
    // embedded mask paths make the icon render differently from its unmasked base.
    await expect(page.locator('svg.svg-inline--fa[data-fa-mask]').first()).toBeVisible();
    expect(await page.locator('[data-fa-mask]').count()).toBeGreaterThanOrEqual(20);
    expect(await page.locator('svg.svg-inline--fa').count()).toBeGreaterThanOrEqual(30);
});

test('layers keep nested SVG stacks after the DOM replacement', async ({ page }) => {
    await page.goto('/layering');

    // Font Awesome's `nest` replacement mode parks the SVG inside the original host, so the
    // `.fa-layers` wrapper must still contain the enhanced child SVGs.
    await expect(page.locator('.fa-layers').first()).toBeVisible();
    expect(await page.locator('.fa-layers').count()).toBeGreaterThanOrEqual(12);
    expect(await page.locator('.fa-layers svg.svg-inline--fa').count()).toBeGreaterThanOrEqual(12);
});

test('accessibility attributes survive the DOM replacement', async ({ page }) => {
    await page.goto('/accessibility');

    // Decorative icons are announced as presentational.
    await expect(page.locator('svg[aria-hidden="true"]').first()).toBeVisible();
    expect(await page.locator('svg[aria-hidden="true"]').count()).toBeGreaterThanOrEqual(20);

    // A title becomes a real accessible image (`role="img"`), not a presentational icon.
    const named = page.locator('svg[role="img"][aria-hidden="false"]');
    await expect(named.first()).toBeVisible();
    await expect(named.first()).toHaveAttribute('title', 'Search for an icon');
});
