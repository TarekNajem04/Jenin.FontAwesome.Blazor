import { test, expect } from '@playwright/test';

// Opt-in smoke suite for a deployed, base-path-hosted build (GitHub Pages).
// Run with DOCS_SITE_URL=https://<user>.github.io/<repo> (and SAMPLE_URL set, to suppress the
// default web server). Local proof of the CI layout: serve the published wwwroot one directory
// above a `Jenin.FontAwesome.Blazor` folder and point DOCS_SITE_URL at that folder.

const baseUrl = process.env.DOCS_SITE_URL?.replace(/\/+$/, '');

test.describe('deployed docs site', () => {
    test.skip(!baseUrl, 'Set DOCS_SITE_URL to smoke-test the published docs site.');

    const url = (path: string) => `${baseUrl}/${path}`;

    test('home renders under the sub-path base', async ({ page }) => {
        await page.goto(`${baseUrl}/`);

        await expect(page).toHaveTitle(/Documentation/i);
        await expect(page.getByTestId('app-interactive')).toBeAttached({ timeout: 30_000 });
        await expect(page.locator('svg.svg-inline--fa').first()).toBeVisible();
        await expect(page.locator('#blazor-error-ui')).toBeHidden();
    });

    test('explorer deep links resolve through the 404 fallback', async ({ page }) => {
        await page.goto(url('explorer'));

        await expect(page.locator('.explorer-grid')).toBeVisible({ timeout: 30_000 });
        await expect(page.locator('.explorer-card').first()).toBeVisible({ timeout: 30_000 });

        await page.locator('#ctl-query').fill('star');
        await expect(page).toHaveURL(/[?&]q=star/);
    });

    test('theme toggle switches and persists', async ({ page }) => {
        await page.addInitScript(() => window.localStorage.setItem('jenin-fa-theme', 'light'));
        await page.goto(`${baseUrl}/`);

        await expect(page.getByRole('button', { name: /switch to dark theme/i }).first()).toBeVisible();
        await page.getByRole('button', { name: /switch to dark theme/i }).first().click();

        await expect
            .poll(() => page.evaluate(() => document.documentElement.getAttribute('data-theme')))
            .toBe('dark');
        expect(await page.evaluate(() => window.localStorage.getItem('jenin-fa-theme'))).toBe('dark');
    });

    test('vendored assets and the API reference load from the base path', async ({ request }) => {
        const css = await request.get(url('_content/Jenin.FontAwesome.Blazor.Sample.Shared/fontawesome/css/all.css'));
        expect(css.status()).toBe(200);

        const framework = await request.get(url('_framework/blazor.webassembly.js'));
        expect(framework.status()).toBe(200);

        const docsHub = await request.get(url('api/'));
        expect(docsHub.status()).toBe(200);
        expect(await docsHub.text()).toContain('Jenin.FontAwesome.Blazor Documentation');

        const api = await request.get(url('api/api/'));
        expect(api.status()).toBe(200);
        expect(await api.text()).toContain('Jenin.FontAwesome.Blazor API Reference');
    });
});
