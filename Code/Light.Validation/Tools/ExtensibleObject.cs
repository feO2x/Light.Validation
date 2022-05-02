using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Light.GuardClauses;

namespace Light.Validation.Tools;

/// <summary>
/// Represents a class that provides the ability to attach any object/value.
/// </summary>
public class ExtensibleObject
{
    /// <summary>
    /// Initializes a new instance of <see cref="ExtensibleObject" />.
    /// </summary>
    /// <param name="other">Another extensible object whose attached objects will be shallow-copied to this instance.</param>
    public ExtensibleObject(ExtensibleObject? other = null)
    {
        if (other?.AttachedObjects is { Count: > 0 })
            AttachedObjects = new Dictionary<string, object>(other.AttachedObjects); 
    }

    /// <summary>
    /// Gets the dictionary that contains all attached objects. This value might be null.
    /// </summary>
    protected Dictionary<string, object>? AttachedObjects { get; private set; }

    /// <summary>
    /// Tries to retrieve an attached object with the specified name.
    /// </summary>
    /// <param name="name">The name that uniquely identifies the attached object.</param>
    /// <param name="foundObject">The found object.</param>
    /// <returns>True if the object was found, else false.</returns>
    public bool TryGetAttachedObject(string name, [NotNullWhen(true)] out object? foundObject)
    {
        if (AttachedObjects?.TryGetValue(name, out foundObject) == true)
            return true;

        foundObject = default;
        return false;
    }

    /// <summary>
    /// Tries to retrieve an attached object with the specified name.
    /// </summary>
    /// <param name="name">The name that uniquely identifies the attached object.</param>
    /// <param name="foundObject">The found object.</param>
    /// <returns>True if the object was found, else false.</returns>
    public bool TryGetAttachedObject<T>(string name, [NotNullWhen(true)] out T? foundObject)
    {
        if (TryGetAttachedObject(name, out var @object) && @object is T castObject)
        {
            foundObject = castObject;
            return true;
        }

        foundObject = default;
        return false;
    }

    /// <summary>
    /// Gets the attached object with the specified name, cast to the target type.
    /// </summary>
    /// <param name="name">The name that uniquely identifies the attached object.</param>
    /// <exception cref="InvalidOperationException">Thrown when there are no objects attached to this instance.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when there is no attached object with the specified name.</exception>
    public object GetAttachedObject(string name)
    {
        if (AttachedObjects is null)
            throw new InvalidOperationException($"You cannot get the attached object with name \"{name}\" when the Attached Objects dictionary is null.");

        if (!AttachedObjects.TryGetValue(name, out var foundObject))
            throw new KeyNotFoundException($"There is no attached object with name \"{name}\".");

        return foundObject;
    }

    /// <summary>
    /// Gets the attached object with the specified name, cast to the target type.
    /// </summary>
    /// <typeparam name="T">The type the attached object will .</typeparam>
    /// <param name="name">The name that uniquely identifies the attached object.</param>
    /// <exception cref="InvalidOperationException">Thrown when there are no objects attached to this instance.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when there is no attached object with the specified name.</exception>
    /// <exception cref="InvalidCastException">Thrown when the found attached object cannot be cast to <typeparamref name="T" />.</exception>
    public T GetAttachedObject<T>(string name)
    {
        var foundObject = GetAttachedObject(name);
        if (foundObject is T castObject)
            return castObject;

        throw new InvalidCastException($"The attached object \"{foundObject}\" cannot be cast to type \"{typeof(T)}\".");
    }

    /// <summary>
    /// Stores the object in the internal dictionary with the specified name. If there already is an object
    /// with the same name, it will be replaced.
    /// </summary>
    /// <param name="name">The name that uniquely identifies the attached object.</param>
    /// <param name="object">The actual object to be stored.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name" /> or <paramref name="object" /> are null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name" /> is an empty string or contains only white space.</exception>
    public void SetAttachedObject(string name, object @object)
    {
        name.MustNotBeNullOrWhiteSpace();
        @object.MustNotBeNull();

        AttachedObjects ??= new Dictionary<string, object>();
        AttachedObjects[name] = @object;
    }

    /// <summary>
    /// Tries to store the object in the internal dictionary. If there already is an object with the same name,
    /// the object will not be stored.
    /// </summary>
    /// <param name="name">The name that uniquely identifies the attached object.</param>
    /// <param name="object">The actual object to be stored.</param>
    /// <returns>True when the object was stored, else false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name" /> or <paramref name="object" /> are null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name" /> is an empty string or contains only white space.</exception>
    public bool TrySetAttachedObject(string name, object @object)
    {
        name.MustNotBeNullOrWhiteSpace();
        @object.MustNotBeNull();

        if (AttachedObjects is null)
        {
            AttachedObjects = new () { { name, @object } };
            return true;
        }

        if (AttachedObjects.ContainsKey(name))
            return false;

        AttachedObjects.Add(name, @object);
        return true;
    }
}