using StringFormatter.Core.Exceptions;

namespace StringFormatter.Core;

public class ObjectAnalyzer
{
    public Dictionary<string, ClassMemberInfo> ClassMembers { get; }

    public ObjectAnalyzer()
    {
        ClassMembers = new();
    }

    public void GetObjectFields(object obj)
    {
        if (obj == null)
            throw new AnalyzerException("Object can't be null");
        Type type = obj.GetType();
        foreach (var field in type.GetFields().Where(f => f.IsPublic))
        {
            ClassMembers.Add(field.Name, new ClassMemberInfo(field.Name, field.GetType(), field.GetValue(obj)));
        }
        foreach (var property in type.GetProperties().Where(p => p.CanRead))
        {
            ClassMembers.Add(property.Name, new ClassMemberInfo(property.Name, property.GetType(), property.GetValue(obj)));
        }
    }
}