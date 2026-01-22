namespace AbsoluteCinema.Domain.ValueObjects;

public sealed class PermissionCode
{
    public string Value { get; private set; }
    private PermissionCode() { }
    private PermissionCode(string code)
    {
        Value = code;
    }

    public static PermissionCode Create(string code)
    {
        return new PermissionCode(code);
    }
}