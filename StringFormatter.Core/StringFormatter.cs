using System.Security.Cryptography.X509Certificates;
using StringFormatter.Core.Exceptions;
using System.Text;
using System.Collections;
using System.Collections.Generic;

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
        if (argumentName.Contains("["))
            return GetArrayAccessorArgument(argumentName, classMembers);
        if (!classMembers.ContainsKey(argumentName))
            throw new FormatterException($"Object doesn't contains such field: {argumentName}");
        return (classMembers[argumentName].ToString());
    }

    private string GetArrayAccessorArgument(string argumentName, Dictionary<string, ClassMemberInfo> classMembers)
    {
        StringBuilder argumentWithoutIndex = new();
        int i = 0;
        while (i < argumentName.Length && argumentName[i] != '[')
        {
            argumentWithoutIndex.Append(argumentName[i]);
            i++;
        }
        if (!classMembers.ContainsKey(argumentWithoutIndex.ToString()))
            throw new FormatterException($"Object doesn't contains such filed: {argumentWithoutIndex}");
        if (i + 1 >= argumentName.Length || argumentName[i + 1] == ']' || !argumentName.Contains(']'))
            throw new FormatterException("Given empty index");
        StringBuilder index = new();
        i++;
        while (i < argumentName.Length && argumentName[i] != ']')
        {
            index.Append(argumentName[i]);
            i++;
        }
        IList collection = classMembers[argumentWithoutIndex.ToString()].Value as IList;
        return collection[Int32.Parse(index.ToString())].ToString();
    }
}