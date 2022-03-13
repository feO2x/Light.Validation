using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.Validation.Tools;
using Xunit;

namespace Light.Validation.Tests.Tools;

public static class ExtensibleObjectTests
{
    [Fact]
    public static void SetAndRetrieveAttachedObject()
    {
        var attachedObject = new object();
        var extensibleObject = new ExtensibleObjectDummy();
        const string name = "My Object";

        extensibleObject.SetAttachedObject(name, attachedObject);
        var retrievedObject = extensibleObject.GetAttachedObject<object>(name);

        retrievedObject.Should().BeSameAs(attachedObject);
    }

    [Fact]
    public static void ImmutableExtensibleObject()
    {
        var attachedObject = new object();
        const string name = "My awesome object";
        var attachedObjects = new Dictionary<string, object> { [name] = attachedObject };
        var extensibleObject = new ExtensibleObjectDummy(attachedObjects, true);

        var setAnotherObject = () => extensibleObject.SetAttachedObject("some other object", new object());

        extensibleObject.GetAttachedObject<object>(name).Should().BeSameAs(attachedObject);
        setAnotherObject.Should().Throw<InvalidOperationException>()
                        .And.Message.Should().Be("Setting attached objects is not allowed when DisallowSettingAttachedObjects is set to true.");
    }

    [Fact]
    public static void KeyNotFound()
    {
        var extensibleObject = new ExtensibleObjectDummy(new Dictionary<string, object> { ["Foo"] = new () });

        var getWithInvalidName = () => extensibleObject.GetAttachedObject<object>("Bar");

        getWithInvalidName.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public static void CastNotValid()
    {
        var attachedObject = new List<string> { "Foo", "Bar", "Baz" };
        const string name = "My Object";
        var extensibleObject = new ExtensibleObjectDummy(new Dictionary<string, object> { [name] = attachedObject });

        var getWithInvalidType = () => extensibleObject.GetAttachedObject<string[]>(name);

        getWithInvalidType.Should().Throw<InvalidCastException>();
    }

    [Fact]
    public static void NoAttachedObjects()
    {
        var extensibleObject = new ExtensibleObjectDummy();

        var getAttachedObject = () => extensibleObject.GetAttachedObject<List<string>>("Foo");

        getAttachedObject.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public static void ReplaceExistingObject()
    {
        var extensibleObject = new ExtensibleObjectDummy();
        const string name = "Foo";
        var attachedObject1 = new object();
        extensibleObject.SetAttachedObject(name, attachedObject1);

        var attachedObject2 = new object();
        extensibleObject.SetAttachedObject(name, attachedObject2);

        extensibleObject.GetAttachedObject<object>(name).Should().BeSameAs(attachedObject2);
    }

    public sealed class ExtensibleObjectDummy : ExtensibleObject
    {
        public ExtensibleObjectDummy(Dictionary<string, object>? attachedObjects = null, bool disallowSettingAttachedObjects = false)
            : base(attachedObjects, disallowSettingAttachedObjects) { }
    }
}