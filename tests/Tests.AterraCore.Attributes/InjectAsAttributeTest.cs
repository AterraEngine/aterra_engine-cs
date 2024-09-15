// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Globalization;

namespace Tests.AterraCore.Attributes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[TestSubject(typeof(ResolveAsSpecificAttribute))]
public class InjectAsAttributeTests {

    [Fact]
    public void InjectAsAttribute_ShouldInitializeWithUlid() {
        const string ulidString = "01F8MECHZX3TBDSZ7T1R7Q3Q3E";

        var attribute = new ResolveAsSpecificAttribute(ulidString);

        Assert.Equal(Ulid.Parse(ulidString, CultureInfo.InvariantCulture), attribute.Ulid);
    }

    [Fact]
    public void InjectAsAttribute_ShouldGenerateNewUlid_WhenNotProvided() {
        var attribute = new ResolveAsSpecificAttribute();

        Assert.NotEqual(default, attribute.Ulid);
    }

    [Fact]
    public void InjectAsAttribute_ShouldThrowFormatException_ForInvalidUlid() {
        const string invalidUlidString = "INVALID_ULID";

        Assert.Throws<ArgumentException>(() => new ResolveAsSpecificAttribute(invalidUlidString));
    }

    [Fact]
    public void MethodWithInjectAsAttribute_ShouldHaveCorrectUlid() {
        MethodInfo? methodInfo = typeof(ClassWithMethodInjection).GetMethod(nameof(ClassWithMethodInjection.MethodWithInjection));
        ParameterInfo? parameterInfo = methodInfo?.GetParameters().FirstOrDefault();
        var attribute = parameterInfo?.GetCustomAttribute<ResolveAsSpecificAttribute>();

        Assert.NotNull(attribute);
        Assert.Equal(Ulid.Parse("01F8MECHZX3TBDSZ7T1R7Q3Q3E", CultureInfo.InvariantCulture), attribute.Ulid);
    }

    [Fact]
    public void MethodWithMultipleInjectAsAttributes_ShouldHaveAllAttributes() {
        MethodInfo? methodInfo = typeof(ClassWithMethodInjection).GetMethod(nameof(ClassWithMethodInjection.MethodWithMultipleInjections));
        ParameterInfo? parameterInfo = methodInfo?.GetParameters().FirstOrDefault();
        ResolveAsSpecificAttribute[]? attributes = parameterInfo?.GetCustomAttributes<ResolveAsSpecificAttribute>().ToArray();

        Assert.NotNull(attributes);
        Assert.Equal(2, attributes.Length);
        Assert.Equal(Ulid.Parse("01F8MECHZX3TBDSZ7T1R7Q3Q3E", CultureInfo.InvariantCulture), attributes[0].Ulid);
    }

    [ExcludeFromCodeCoverage]
    private class ClassWithMethodInjection {
        [ExcludeFromCodeCoverage] public static void MethodWithInjection([ResolveAsSpecific("01F8MECHZX3TBDSZ7T1R7Q3Q3E")] string _) {}
        [ExcludeFromCodeCoverage] public static void MethodWithMultipleInjections([ResolveAsSpecific("01F8MECHZX3TBDSZ7T1R7Q3Q3E")] [ResolveAsSpecific] string _) {}
    }
}
