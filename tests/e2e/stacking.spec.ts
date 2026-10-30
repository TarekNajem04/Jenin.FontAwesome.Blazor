import { test, expect } from '@playwright/test';

// The sample app uses the Font Awesome SVG+JS engine, which injects its own sizing for stacked
// layers. The page includes a compatibility rule that keeps the 2x layer inside the stack box, so
// this checks the geometry rather than just the emitted classes.

test.describe('icon stacking', () => {
    test('the 2x layer fits inside the stack and the 1x layer stays smaller', async ({ page }) => {
        await page.goto('/stacking');
        await page.getByTestId('app-interactive').waitFor({ state: 'attached' });

        const stack = page.locator('.fa-stack:visible').first();
        await expect(stack.locator('svg.fa-stack-2x')).toHaveCount(1);
        await expect(stack.locator('svg.fa-stack-1x')).toHaveCount(1);

        const box = await stack.boundingBox();
        const big = await stack.locator('svg.fa-stack-2x').boundingBox();
        const small = await stack.locator('svg.fa-stack-1x').boundingBox();

        expect(box).not.toBeNull();
        expect(big).not.toBeNull();
        expect(small).not.toBeNull();

        // The larger layer fills the stack box rather than overflowing it by a factor of two.
        expect(big!.width).toBeLessThanOrEqual(box!.width + 1);
        expect(big!.height).toBeLessThanOrEqual(box!.height + 1);
        expect(small!.width).toBeLessThan(big!.width);
        expect(small!.height).toBeLessThan(big!.height);

        // Both layers are centered on the stack.
        expect(Math.abs(big!.x + (big!.width / 2) - (box!.x + (box!.width / 2)))).toBeLessThanOrEqual(1);
        expect(Math.abs(big!.y + (big!.height / 2) - (box!.y + (box!.height / 2)))).toBeLessThanOrEqual(1);
    });
});
