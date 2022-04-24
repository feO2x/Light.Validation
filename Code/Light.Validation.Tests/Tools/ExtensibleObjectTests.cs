using System;
using System.Collections.Generic;
using System.Security.Claims;
using FluentAssertions;
using Light.Validation.Tools;
using Xunit;
using Newtonsoft.Json;

namespace Light.Validation.Tests.Tools;

public static class ExtensibleObjectTests
{
    [Fact]
    public static void SetAndRetrieveAttachedObject()
    {
        var attachedObject = new object();
        var extensibleObject = new ExtensibleObject();
        const string name = "My Object";

        extensibleObject.SetAttachedObject(name, attachedObject);
        var retrievedObject = extensibleObject.GetAttachedObject<object>(name);

        retrievedObject.Should().BeSameAs(attachedObject);
    }

    [Fact]
    public static void KeyNotFound()
    {
        var extensibleObject = new ExtensibleObject();
        extensibleObject.SetAttachedObject("Foo", new ());

        var getWithInvalidName = () => extensibleObject.GetAttachedObject<object>("Bar");

        getWithInvalidName.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public static void CastNotValid()
    {
        var attachedObject = new List<string> { "Foo", "Bar", "Baz" };
        const string name = "My Object";
        var extensibleObject = new ExtensibleObject();
        extensibleObject.SetAttachedObject("My Object", attachedObject);

        var getWithInvalidType = () => extensibleObject.GetAttachedObject<string[]>(name);

        getWithInvalidType.Should().Throw<InvalidCastException>();
    }

    [Fact]
    public static void NoAttachedObjects()
    {
        var extensibleObject = new ExtensibleObject();

        var getAttachedObject = () => extensibleObject.GetAttachedObject<List<string>>("Foo");

        getAttachedObject.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public static void ReplaceExistingObject()
    {
        var extensibleObject = new ExtensibleObject();
        const string name = "Foo";
        var attachedObject1 = new object();
        extensibleObject.SetAttachedObject(name, attachedObject1);

        var attachedObject2 = new object();
        extensibleObject.SetAttachedObject(name, attachedObject2);

        extensibleObject.GetAttachedObject<object>(name).Should().BeSameAs(attachedObject2);
    }

    [Fact]
    public static void TryGetOnEmptyExtensibleObject()
    {
        var extensibleObject = new ExtensibleObject();

        var result = extensibleObject.TryGetAttachedObject("Foo", out string? foundObject);

        result.Should().BeFalse();
        foundObject.Should().BeNull();
    }

    [Fact]
    public static void TryGetWithWrongName()
    {
        var extensibleObject = new ExtensibleObject();
        extensibleObject.SetAttachedObject("Foo", "ABC");

        var result = extensibleObject.TryGetAttachedObject("Bar", out string? foundObject);

        result.Should().BeFalse();
        foundObject.Should().BeNull();
    }

    [Fact]
    public static void TryGetWithWrongType()
    {
        var extensibleObject = new ExtensibleObject();
        extensibleObject.SetAttachedObject("Foo", "ABC");

        var result = extensibleObject.TryGetAttachedObject("Foo", out JsonSerializer? serializer);

        result.Should().BeFalse();
        serializer.Should().BeNull();
    }

    [Fact]
    public static void TryGetSuccessfully()
    {
        var extensibleObject = new ExtensibleObject();
        var attachedObject = new ClaimsPrincipal();
        extensibleObject.SetAttachedObject("Foo", attachedObject);

        var result = extensibleObject.TryGetAttachedObject("Foo", out ClaimsPrincipal? foundObject);

        result.Should().BeTrue();
        foundObject.Should().BeSameAs(attachedObject);
    }

    [Fact]
    public static void TrySetSuccessfully()
    {
        var extensibleObject = new ExtensibleObject();
        const string attachedObject = "Yeah!";

        var result = extensibleObject.TrySetAttachedObject("Foo", attachedObject);

        result.Should().BeTrue();
        extensibleObject.GetAttachedObject<string>("Foo").Should().BeSameAs(attachedObject);
    }

    [Fact]
    public static void TrySetWithExistingObject()
    {
        var extensibleObject = new ExtensibleObject();
        extensibleObject.SetAttachedObject("Foo", "Yeah");

        var result = extensibleObject.TrySetAttachedObject("Foo", "Hell");

        result.Should().BeFalse();
        extensibleObject.GetAttachedObject<string>("Foo").Should().Be("Yeah");
    }

    [Fact]
    public static void CopyAttachedObjectsFromOther()
    {
        var extensibleObject1 = new ExtensibleObject();
        var attachedObjectA = new object();
        extensibleObject1.SetAttachedObject("Foo", attachedObjectA);

        var extensibleObject2 = new ExtensibleObject(extensibleObject1);
        extensibleObject2.GetAttachedObject("Foo").Should().BeSameAs(attachedObjectA);

        var attachedObjectB = new object();
        extensibleObject2.SetAttachedObject("Bar", attachedObjectB);
        extensibleObject1.TryGetAttachedObject("Bar", out _).Should().BeFalse();

        var attachedObjectC = new object();
        extensibleObject1.SetAttachedObject("Baz", attachedObjectC);
        extensibleObject2.TryGetAttachedObject("Baz", out _).Should().BeFalse();
    }
}