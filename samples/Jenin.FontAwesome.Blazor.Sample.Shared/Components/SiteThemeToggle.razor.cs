//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Sample.Shared.Components;

/// <summary>
/// The light/dark theme switch for the sample host. The initial value is read from the browser (the
/// inline theme bootstrap has already applied it before first paint); toggling writes the choice back
/// through <c>window.sampleTheme</c>, which persists it and stamps <c>data-theme</c> on the document
/// root so every CSS override flips at once.
/// </summary>
public partial class SiteThemeToggle : ComponentBase {
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private bool IsDark { get; set; }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);

        if(firstRender) {
            bool isDark;
            try {
                isDark = await JSRuntime.InvokeAsync<string>("sampleTheme.current") == "dark";
            }
            catch(JSException) {
                isDark = false;
            }

            if(isDark != IsDark) {
                IsDark = isDark;
                StateHasChanged();
            }
        }
    }

    private async Task ToggleAsync() {
        IsDark = !IsDark;

        try {
            var resolved = await JSRuntime.InvokeAsync<string>("sampleTheme.set", IsDark ? "dark" : "light");
            IsDark = resolved == "dark";
        }
        catch(JSException) {
            /* Prerendering or a disconnected circuit: keep the optimistic value. */
        }
    }
}
