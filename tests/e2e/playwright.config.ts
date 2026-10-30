import { defineConfig } from '@playwright/test';

const baseURL = process.env.SAMPLE_URL ?? 'http://localhost:5000';

export default defineConfig({
    testDir: '.',
    timeout: 30_000,
    expect: { timeout: 10_000 },
    retries: 0,
    reporter: 'list',
    use: {
        baseURL,
        trace: 'on-first-retry'
    },
    webServer: process.env.SAMPLE_URL
        ? undefined
        : {
            command: 'dotnet run --project ../../samples/Jenin.FontAwesome.Blazor.Sample/Jenin.FontAwesome.Blazor.Sample.csproj -c Release --urls http://localhost:5000',
            url: baseURL,
            reuseExistingServer: true,
            timeout: 240_000
        }
});
