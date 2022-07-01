namespace Jenin.FontAwesome.Blazor;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ValueAttribute : Attribute {
    public ValueAttribute(object value) => Value = value;

    public object Value { get; set; }
}
