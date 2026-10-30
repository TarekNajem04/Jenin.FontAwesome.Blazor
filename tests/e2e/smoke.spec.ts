import { test, expect } from '@playwright/test';

test('home page renders and Font Awesome enhances its placeholders', async ({ page }) => {
    await page.goto('/');

    await expect(page).toHaveTitle(/JENIN Font Awesome Blazor Components/i);

    // The library emits Font Awesome markup; the hosted Font Awesome runtime performs the actual
    // DOM replacement (`<i class="fa-...">` -> `<svg class="svg-inline--fa ...">`).
    await expect(page.locator('svg.svg-inline--fa').first()).toBeVisible();
});

test('library config asset is served by the host', async ({ request }) => {
    const response = await request.get('/_content/Jenin.FontAwesome.Blazor/js/fontawesome-config-nest.js');

    expect(response.status()).toBe(200);
    expect(await response.text()).toContain('autoReplaceSvg');
});
