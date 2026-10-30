//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Base element that adds the mouse event parameters (<c>OnClick</c>, <c>OnDoubleClick</c>,
/// <c>OnMouseOver</c>, <c>OnMouseOut</c>) on top of <see cref="ElementBase"/>. Not typically
/// consumed directly.
/// </summary>
public class Element : ElementBase {
    #region Events
    /// <summary>
    /// Occurs when the component is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Occurs when the component is double clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnDoubleClick { get; set; }

    /// <summary>
    /// Occurs when the mouse has entered the component area.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnMouseOver { get; set; }

    /// <summary>
    /// Occurs when the mouse has left the component area.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnMouseOut { get; set; }

    private protected Task OnClickHandlerAsync(MouseEventArgs eventArgs) => OnClick.InvokeAsync(eventArgs);

    private protected Task OnDoubleClickHandlerAsync(MouseEventArgs eventArgs) => OnDoubleClick.InvokeAsync(eventArgs);

    private protected Task OnMouseOverHandlerAsync(MouseEventArgs eventArgs) => OnMouseOver.InvokeAsync(eventArgs);

    private protected Task OnMouseOutHandlerAsync(MouseEventArgs eventArgs) => OnMouseOut.InvokeAsync(eventArgs);
    #endregion Events

    /// <summary>
    /// The sequence is incremented by 4 [sequence  + 4]
    /// </summary>
    /// <param name="builder">The render tree builder</param>
    /// <param name="sequence">The sequence number</param>
    private protected virtual void AddEventsToRenderTree(RenderTreeBuilder builder, int sequence) {
        ArgumentNullException.ThrowIfNull(builder);

        var sequences = new int[] { sequence + 1, sequence + 2, sequence + 3, sequence + 4 };

        if(OnClick.HasDelegate) {
            builder.AddAttribute(sequences[0], "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, OnClickHandlerAsync));
        }

        if(OnDoubleClick.HasDelegate) {
            builder.AddAttribute(sequences[1], "ondblclick", EventCallback.Factory.Create<MouseEventArgs>(this, OnDoubleClickHandlerAsync));
        }

        if(OnMouseOver.HasDelegate) {
            builder.AddAttribute(sequences[2], "onmouseover", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseOverHandlerAsync));
        }

        if(OnMouseOut.HasDelegate) {
            builder.AddAttribute(sequences[3], "onmouseout", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseOutHandlerAsync));
        }
    }
}
