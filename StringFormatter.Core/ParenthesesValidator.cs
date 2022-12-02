namespace StringFormatter.Core;

public class ParenthesesValidator
{
    public static void IsValid(string stringToValidate)
    {
        int counter = 0;
        int openBracketIndex = 0, closeBracketIndex = 0;
        for (int i = 0; i < stringToValidate.Length; i++)
        {
            if (stringToValidate[i] == '{')
            {
                if (openBracketIndex == i - 1)
                    counter -= 1;
                else
                {
                    counter += 1;
                    openBracketIndex = i;
                }
            }
            if (stringToValidate[i] == '}')
            {
                if (closeBracketIndex == i - 1)
                    counter += 1;
                else
                {
                    counter -= 1;
                    closeBracketIndex = i;
                    if (i == openBracketIndex + 1)
                        throw new Exception();
                    if (i + 1 < stringToValidate.Length && stringToValidate[i + 1] == '}')
                        continue;
                    if (counter < 0)
                        throw new Exception();
                }
            }
        }
        if (counter != 0)
            throw new Exception();
    }
}