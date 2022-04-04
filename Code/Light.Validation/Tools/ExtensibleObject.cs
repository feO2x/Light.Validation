using System;
using System.Collections.Generic;
using Light.GuardClauses;

namespace Light.Validation.Tools;

/// <summary>
/// Represents a base class that provides the ability to attach any object to this instance.
/// The access to the dictionary is not thread-safe, but you can make instances of this class
/// immutable and thus thread-safe.
/// </summary>
public abstract class ExtensibleObject
{
    /// <summary>
    /// Initializes a new instance of <see cref="ExtensibleObject" />.
    /// </summary>
    /// <param name="attachedObjects">The dictionary that will be used as the internal storage for attached objects.</param>
    /// <param name="disallowSettingAttachedObjects">
    /// The value indicating whether <see cref="SetAttachedObject" /> will throw an exception when being called.
    /// If this value is set to true, the extensible object is immutable and the fully-filled dictionary of attached objects
    /// must be passed as a parameter to the constructor. Using this feature makes instances of this class thread-safe.
    /// </param>
    protected ExtensibleObject(Dictionary<string, object>? attachedObjects = null,
                               bool disallowSettingAttachedObjects = false)
    {
        AttachedObjects = attachedObjects;
        DisallowSettingAttachedObjects = disallowSettingAttachedObjects;
    }

    /// <summary>
    /// Gets the dictionary that contains all attached objects. This value might be null.
    /// </summary>
    protected Dictionary<string, object>? AttachedObjects { get; private set; }

    /// <summary>
    /// Gets the value indicating whether <see cref="SetAttachedObject" /> will throw an exception when being called.
    /// </summary>
    protected bool DisallowSettingAttachedObjects { get; }

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
        if (AttachedObjects is null)
            throw new InvalidOperationException($"You cannot get the attached object with name \"{name}\" when the Attached Objects dictionary is null.");

        if (!AttachedObjects.TryGetValue(name, out var foundObject))
            throw new KeyNotFoundException($"There is no attached object with name \"{name}\".");

        if (foundObject is T castObject)
            return castObject;

        throw new InvalidCastException($"The attached object \"{foundObject}\" cannot be cast to type \"{typeof(T)}\".");
    }

    /// <summary>
    /// Stores an object in the internal dictionary with the specified name. If there already is an object
    /// with the same name, it will be replaced.
    /// </summary>
    /// <param name="name">The name that uniquely identifies the attached object.</param>
    /// <param name="object">The actual object to be stored.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name" /> or <paramref name="object" /> are null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name" /> is an empty string or contains only white space.</exception>
    public void SetAttachedObject(string name, object @object)
    {
        if (DisallowSettingAttachedObjects)
            throw new InvalidOperationException("Setting attached objects is not allowed when DisallowSettingAttachedObjects is set to true.");

        name.MustNotBeNullOrWhiteSpace();
        @object.MustNotBeNull();

        AttachedObjects ??= new Dictionary<string, object>();
        AttachedObjects[name] = @object;
    }
}