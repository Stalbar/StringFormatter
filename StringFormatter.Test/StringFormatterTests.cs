using StringFormatter.Core;
using StringFormatter.Core.Exceptions;

namespace StringFormatter.Test;

public class UnitTest1
{
    public class TestClass
    {
        public int Age;
        public string Name { get; set; }

        private int privateField;

        public int[] internalArray { get; set; }

        public List<int> internalList { get; set; }

        public TestClass(int age, string name, int privateField, int[] internalArray, List<int> internalList)
        {
            Age = age;
            Name = name;
            this.privateField = privateField;
            this.internalArray = internalArray;
            this.internalList = internalList;
        }
    }

    TestClass testClass = new(15, "dfgd", 1, new int[] { 1, 2, 3, 4, 5 }, new List<int> { 1, 2, 3, 4, 5 });
    StringFormatter.Core.IStringFormatter formatter = StringFormatter.Core.StringFormatter.Shared;

    [Fact]
    public void OneOpenBracketTest()
    {
        Assert.Throws<ValidatorException>(() => formatter.Format("{", testClass));
    }
    [Fact]
    public void OneClosingBracketTest()
    {
        Assert.Throws<ValidatorException>(() => formatter.Format("}", testClass));
    }
    [Fact]
    public void UnequalNumberOfBracketsTest1()
    {
        Assert.Throws<ValidatorException>(() => formatter.Format("{{}", testClass));
    }
    [Fact]
    public void UnequalNumberOfBracketsTest2()
    {
        Assert.Throws<ValidatorException>(() => formatter.Format("{}}", testClass));
    }
    [Fact]
    public void UnequalNumberOfBracketsTest3()
    {
        Assert.Throws<ValidatorException>(() => formatter.Format("}}{{{", testClass));
    }
    [Fact]
    public void UnequalNumberOfBracketsTest4()
    {
        Assert.Throws<ValidatorException>(() => formatter.Format("{{{{age}}}", testClass));
    }
    [Fact]
    public void UnequalNumberOfBracketsTest5()
    {
        Assert.Throws<ValidatorException>(() => formatter.Format("{{{{{}{{{{{{}}}}}}}}}}}}", testClass));
    }
    [Fact]
    public void EmptyArgumentNameTest()
    {
        Assert.Throws<ValidatorException>(() => formatter.Format("{}", testClass));
    }
    [Fact]
    public void NoSuchArgumentTest()
    {
        Assert.Throws<FormatterException>(() => formatter.Format("{Arg}", testClass));
    }
    [Fact]
    public void PrivateFieldTest()
    {
        Assert.Throws<FormatterException>(() => formatter.Format("{privateField}", testClass));
    }
    [Fact]
    public void BracketInterpolationTest()
    {
        Assert.Equal($"{{}}", formatter.Format("{{}}", testClass));
    }
    [Fact]
    public void NullReferenceTest()
    {
        Assert.Throws<AnalyzerException>(() => formatter.Format("{age}", null));
    }
    [Fact]
    public void InvalidIndexTest1()
    {
        Assert.Throws<FormatterException>(() => formatter.Format("{internalArray[]}", testClass));
    }
    [Fact]
    public void InvalidIndexTest2()
    {
        Assert.Throws<FormatterException>(() => formatter.Format("{internalArray[}", testClass));
    }
    [Fact]
    public void ArrayAccessionTest()
    {
        Assert.Equal($"{testClass.internalArray[0]}", formatter.Format("{internalArray[0]}", testClass));
    }
    [Fact]
    public void ListAccessionTest()
    {
        Assert.Equal($"{testClass.internalList[0]}", formatter.Format("{internalList[0]}", testClass));
    }
    [Fact]
    public void BasicInterpolationTest()
    {
        Assert.Equal($"Age: {testClass.Age}, Name: {testClass.Name}", formatter.Format("Age: {Age}, Name: {Name}", testClass));
    }
}