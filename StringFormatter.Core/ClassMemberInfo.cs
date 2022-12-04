namespace StringFormatter.Core;

public struct ClassMemberInfo
{
    public string Name { get; private set; }
    public Type ValueType { get; private set; }
    public object Value { get; private set; }

    public ClassMemberInfo(string name, Type valueType, object value)
    {
        Name = name;
        ValueType = valueType;
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}