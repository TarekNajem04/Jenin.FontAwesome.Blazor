namespace Jenin.FontAwesome.Blazor.Animations;

[Flags]
public enum AnimationDirection {
    [Value("")]
    None = 0,

    [Value("normal")]
    Normal = 1,
    [Value("reverse")]
    Reverse = 1 << 1,
    [Value("alternate")]
    Alternate = 1 << 2,
}
