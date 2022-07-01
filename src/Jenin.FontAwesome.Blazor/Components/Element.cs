namespace Jenin.FontAwesome.Blazor.Components;

public class ElementEventArgs<TEventArgs> : EventArgs {
    public ElementEventArgs(Element element, TEventArgs eventArgs) {
        Element = element;
        EventArgs = eventArgs;
    }

    public Element Element { get; set; }

    public TEventArgs EventArgs { get; set; }
}

public class ElementMouseEventArgs : ElementEventArgs<MouseEventArgs> {
    public ElementMouseEventArgs(Element element, MouseEventArgs eventArgs) : base(element, eventArgs) { }
}

public class Element : ElementBase {
    #region Events
    /// <summary>
    /// Occurs when the component is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<ElementMouseEventArgs> Clicked { get; set; }

    /// <summary>
    /// Occurs when the component is double clicked.
    /// </summary>
    [Parameter]
    public EventCallback<ElementMouseEventArgs> DoubleClicked { get; set; }

    /// <summary>
    /// Occurs when the mouse has entered the component area.
    /// </summary>
    [Parameter]
    public EventCallback<ElementMouseEventArgs> MouseOver { get; set; }

    /// <summary>
    /// Occurs when the mouse has left the component area.
    /// </summary>
    [Parameter]
    public EventCallback<ElementMouseEventArgs> MouseOut { get; set; }

    protected Task OnClickHandlerAsync(MouseEventArgs eventArgs) => Clicked.InvokeAsync(new(this, eventArgs));
    protected Task OnDoubleClickHandlerAsync(MouseEventArgs eventArgs) => DoubleClicked.InvokeAsync(new(this, eventArgs));
    protected Task OnMouseOverHandlerAsync(MouseEventArgs eventArgs) => MouseOver.InvokeAsync(new(this, eventArgs));
    protected Task OnMouseOutHandlerAsync(MouseEventArgs eventArgs) => MouseOut.InvokeAsync(new(this, eventArgs));
    #endregion

    protected override int BuildElementRenderTree(ref int sequence, RenderTreeBuilder builder) {
        _ = base.BuildElementRenderTree(ref sequence, builder);
        builder.AddAttribute(sequence++, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, OnClickHandlerAsync));
        builder.AddAttribute(sequence++, "ondblclick", EventCallback.Factory.Create<MouseEventArgs>(this, OnDoubleClickHandlerAsync));
        //builder.AddAttribute(sequence++, "onmouseover", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseOverHandlerAsync));
        //builder.AddAttribute(sequence++, "onmouseout", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseOutHandlerAsync));

        return sequence;
    }
}
