import { expect, test, type Page } from '@playwright/test';

// Family x style x mode matrix. The fixture runs once per mode (env
// PRO_MATRIX_MODE); the edition under test (env FA_EDITION) is rendered as data-edition,
// so the same spec asserts the Free fallback contract and the licensed Pro resolution.
type Cell = {
    id: string;
    content: string;
    freeFamily: string | null;
    proFamily: string | null;
    freeTag: string;
    /** Registered custom glyph: identical self-contained SVG in every mode and edition. */
    custom?: boolean;
};

const cells: Cell[] = [
    // content: decoded glyph char as returned by getComputedStyle (quoted).
    { id: 's-solid', content: '\uf030', freeFamily: 'Font Awesome 7 Free', proFamily: null, freeTag: 'svg' },
    { id: 's-regular', content: '\uf030', freeFamily: 'Font Awesome 7 Free', proFamily: null, freeTag: 'svg' },
    { id: 's-brands', content: '\uf09B', freeFamily: 'Font Awesome 7 Brands', proFamily: null, freeTag: 'svg' },
    // Pro styles: Free hosts render nothing (page default font); licensed Pro hosts resolve.
    { id: 's-light', content: '\uf030', freeFamily: null, proFamily: 'Pro', freeTag: 'i' },
    { id: 's-thin', content: '\uf030', freeFamily: null, proFamily: 'Pro', freeTag: 'i' },
    { id: 's-duotone', content: '\uf030', freeFamily: 'Font Awesome 7 Free', proFamily: 'Pro', freeTag: 'svg' },
    { id: 't-sharp', content: '\uf030', freeFamily: 'Font Awesome 7 Free', proFamily: 'Pro', freeTag: 'svg' },
    { id: 't-duotone', content: '\uf030', freeFamily: 'Font Awesome 7 Free', proFamily: 'Pro', freeTag: 'svg' },
    { id: 't-sharpduotone', content: '\uf030', freeFamily: 'Font Awesome 7 Free', proFamily: 'Pro', freeTag: 'svg' },
    { id: 'm-custom', content: '', freeFamily: null, proFamily: null, freeTag: 'svg', custom: true },
];

function tagFor(page: Page, cell: Cell, tag: string) {
    return page.locator(`${tag}[data-testid="${cell.id}"]`).first();
}

test('pro matrix: emitted contract, engine resolution, and edition behavior', async ({ page }) => {
    test.skip(!process.env.PRO_MATRIX_MODE, 'Requires the pro-matrix fixture host (run eng/verify-pro-render.ps1).');

    await page.goto('/');
    const edition = (await page.getAttribute('#edition', 'data-edition')) ?? 'free';
    const mode = (await page.getAttribute('#edition', 'data-mode')) ?? 'nest';

    if (mode === 'nest') {
        // Free: 7 svgs + light/thin left as <i>; Pro: all 9 resolve to svg. The registered
        // custom glyph always renders its own svg in every mode and edition.
        const expected = edition === 'pro' ? 10 : 8;
        await page.waitForFunction(
            (count) => document.querySelectorAll('svg[data-testid]').length === count,
            expected,
            { timeout: 30000 },
        );
    } else {
        // Webfont: the engine stays inert, every FA cell keeps its anchor; the registered
        // custom glyph renders its own svg regardless.
        await expect(page.locator('i[data-testid]')).toHaveCount(9);
        await expect(page.locator('svg[data-testid]')).toHaveCount(1);
    }

    for (const cell of cells) {
        if (cell.custom === true) {
            const el = tagFor(page, cell, 'svg');
            await expect(el).toBeAttached();
            // eslint-disable-next-line no-console
            console.log(`${cell.id} [${edition}/${mode}]: registered custom glyph`);
            expect(await el.locator('path').count()).toBeGreaterThan(0);
            expect(await el.getAttribute('viewBox')).toBe('0 0 512 496');
            expect(await el.getAttribute('data-icon')).toBe('acme-widget');
            continue;
        }

        // Resolved cells surface as svg (the engine can keep a data-fa-i2svg-marked <i>
        // next to it); Free light/thin stay <i> only. Webfont mode never replaces.
        const tag = mode === 'webfont' ? 'i' : (edition === 'pro' || cell.freeTag === 'svg' ? 'svg' : 'i');
        const el = tagFor(page, cell, tag);
        await expect(el).toBeAttached();

        if (mode === 'webfont') {
            const content = await el.evaluate((node) => getComputedStyle(node as Element, '::before').content);
            const family = await el.evaluate((node) => getComputedStyle(node as Element, '::before').fontFamily);
            // eslint-disable-next-line no-console
            console.log(`${cell.id} [${edition}/${mode}]: content=${content} family=${family}`);
            if (edition === 'pro') {
                expect(content).not.toBe('none');
                if (cell.proFamily !== null) {
                    expect(family).toContain(cell.proFamily);
                }
            } else if (cell.freeFamily === null) {
                expect(content).toBe('none');
                expect(family).not.toContain('Font Awesome');
            } else {
                expect(content).toContain(cell.content);
                expect(family).toContain(cell.freeFamily);
            }
        } else {
            // Nest: font-family on inline SVG paths is meaningless (paths carry the glyph),
            // so the edition contract is structural: resolution + layering.
            const paths = await el.locator('path').count();
            // eslint-disable-next-line no-console
            console.log(`${cell.id} [${edition}/${mode}]: tag=${tag} paths=${paths}`);
            if (tag === 'i') {
                continue;
            }
            expect(paths).toBeGreaterThan(0);
            if (edition === 'free' && cell.id.includes('duotone')) {
                // Free fallback: the classic solid glyph with no layered duotone classes
                // (FA-03 §4); licensed Pro renders fa-primary/fa-secondary layered paths.
                expect(await el.locator('path.fa-primary, path.fa-secondary').count()).toBe(0);
            }
        }
    }
});
