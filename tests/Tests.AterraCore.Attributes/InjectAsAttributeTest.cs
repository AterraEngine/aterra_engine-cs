// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Globalization;

namespace Tests.AterraCore.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(InjectAsAttribute))]
public class InjectAsAttributeTests {
    [ExcludeFromCodeCoverage]
    private class ClassWithMethodInjection {
        [ExcludeFromCodeCoverage] public static void MethodWithInjection([InjectAs("01F8MECHZX3TBDSZ7T1R7Q3Q3E")] string _) {}
        [ExcludeFromCodeCoverage] public static void MethodWithMultipleInjections([InjectAs("01F8MECHZX3TBDSZ7T1R7Q3Q3E")] [InjectAs] string _) {}
    }

    [Fact]
    public void InjectAsAttribute_ShouldInitializeWithUlid() {
        const string ulidString = "01F8MECHZX3TBDSZ7T1R7Q3Q3E";

        var attribute = new InjectAsAttribute(ulidString);

        Assert.Equal(Ulid.Parse(ulidString, CultureInfo.InvariantCulture), attribute.Ulid);
    }

    [Fact]
    public void InjectAsAttribute_ShouldGenerateNewUlid_WhenNotProvided() {
        var attribute = new InjectAsAttribute();

        Assert.NotEqual(default, attribute.Ulid);
    }

    [Fact]
    public void InjectAsAttribute_ShouldThrowFormatException_ForInvalidUlid() {
        const string invalidUlidString = "INVALID_ULID";

        Assert.Throws<ArgumentException>(() => new InjectAsAttribute(invalidUlidString));
    }

    [Fact]
    public void MethodWithInjectAsAttribute_ShouldHaveCorrectUlid() {
        MethodInfo? methodInfo = typeof(ClassWithMethodInjection).GetMethod(nameof(ClassWithMethodInjection.MethodWithInjection));
        ParameterInfo? parameterInfo = methodInfo?.GetParameters().FirstOrDefault();
        var attribute = parameterInfo?.GetCustomAttribute<InjectAsAttribute>();

        Assert.NotNull(attribute);
        Assert.Equal(Ulid.Parse("01F8MECHZX3TBDSZ7T1R7Q3Q3E", CultureInfo.InvariantCulture), attribute.Ulid);
    }

    [Fact]
    public void MethodWithMultipleInjectAsAttributes_ShouldHaveAllAttributes() {
        MethodInfo? methodInfo = typeof(ClassWithMethodInjection).GetMethod(nameof(ClassWithMethodInjection.MethodWithMultipleInjections));
        ParameterInfo? parameterInfo = methodInfo?.GetParameters().FirstOrDefault();
        InjectAsAttribute[]? attributes = parameterInfo?.GetCustomAttributes<InjectAsAttribute>().ToArray();

        Assert.NotNull(attributes);
        Assert.Equal(2, attributes.Length);
        Assert.Equal(Ulid.Parse("01F8MECHZX3TBDSZ7T1R7Q3Q3E", CultureInfo.InvariantCulture), attributes[0].Ulid);
    }
}
