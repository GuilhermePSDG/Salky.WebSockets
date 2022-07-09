﻿// configure
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Salky.WebSockets.Models;

public struct Key : IEquatable<Key>, IComparable<Key>, IEqualityComparer<Key>
{
    private string _key { get; set; }
    public Key(params string[] PoolKeys) : this(string.Join(":", PoolKeys)) { }
    [JsonConstructor]
    public Key(string value) => _key = value;
    public Key(Guid PoolKey) => _key = PoolKey.ToString();

    public static implicit operator Key(string[] PoolKeys) => new(PoolKeys);
    public static implicit operator Key(string PoolKey) => new(PoolKey);
    public static implicit operator Key(Guid PoolKey) => new(PoolKey);
    public static implicit operator string(Key PoolKey) => PoolKey._key;
    public string Value
    {
        get => _key;
        set
        {
            if (_key != null) throw new InvalidOperationException();
            _key = value;
        }
    }
    public override string ToString() => _key;
    public override int GetHashCode() => _key.GetHashCode();
    public bool Equals(Key other) => _key == other._key;
    public int CompareTo(Key other) => other._key.CompareTo(_key);
    public bool Equals(Key x, Key y) => x._key == y._key;
    public int GetHashCode([DisallowNull] Key obj) => obj._key.GetHashCode();
    public override bool Equals(object? obj) => obj != null && obj is Key key && key._key == _key;
    public static bool operator ==(Key left, Key right) => left.Equals(right);
    public static bool operator !=(Key left, Key right) => !(left == right);
    public static bool operator <(Key left, Key right) => left.CompareTo(right) < 0;
    public static bool operator <=(Key left, Key right) => left.CompareTo(right) <= 0;
    public static bool operator >(Key left, Key right) => left.CompareTo(right) > 0;
    public static bool operator >=(Key left, Key right) => left.CompareTo(right) >= 0;

}

