using StringFormatter.Core.Exceptions;
using System.Text;
namespace StringFormatter.Core;

public class StringFormatter : IStringFormatter
{
    public static readonly StringFormatter Shared = new StringFormatter();

    public string Format(string template, object target)
    {
        ParenthesesValidator.IsValid(template);
        ObjectAnalyzer analyzer = new();
        analyzer.GetObjectFields(target);
        return Format(template, analyzer.ClassMembers);
    }

    private string Format(string template, Dictionary<string, ClassMemberInfo> classMembers)
    {
        StringBuilder result = new();
        int index = 0;
        while (index < template.Length)
        {
            if (template[index] == '{')
            {
                if (index + 1 < template.Length && template[index + 1] == '{')
                {
                    result.Append(template[index]);
                    index += 2;
                }
                else
                {
                    StringBuilder argumentName = new();
                    index++;
                    while (template[index] != '}')
                    {
                        argumentName.Append(template[index]);
                        index++;
                    }
                    Console.WriteLine(argumentName.ToString());
                    index++;
                    result.Append(GetArgumentValue(argumentName.ToString(), classMembers));
                }
            }
            else if (template[index] == '}')
            {
                if (index + 1 < template.Length && template[index + 1] == '}')
                {
                    result.Append(template[index]);
                    index += 2;
                }
                else
                {
                    result.Append(template[index]);
                    index++;
                }
            }
            else
            {
                result.Append(template[index]);
                index++;
            }
        }
        return result.ToString();
    }

    private string GetArgumentValue(string argumentName, Dictionary<string, ClassMemberInfo> classMembers)
    {
        if (!classMembers.ContainsKey(argumentName))
            throw new FormatterException($"Object doesn't contains such field: {argumentName}");
        return (classMembers[argumentName].ToString());
    }
}