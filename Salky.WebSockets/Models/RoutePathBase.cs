﻿// configure

using Salky.WebSockets.Enums;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Salky.WebSockets.Models;

public class RoutePathBase : IEqualityComparer<RoutePathBase>, IEquatable<RoutePathBase>
{
    public RoutePathBase(string fullPath, Method method)
    {
        Path = fullPath.ToLower();
        Method = method;
    }
    public RoutePathBase() { }
    private string genKey() => $"{Path}-{Method}";
    public string Path { get; init; }
    public Method Method { get; init; }

    [NonSerialized, JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    private string? Key;
    public string GenRouteKey() => Key ??= genKey();
    public override string ToString() => Key ??= genKey();

    public bool Equals(RoutePathBase? x, RoutePathBase? y)
    {
        if (x == null && y == null)
            return true;
        else if (x == null || y == null)
            return false;
        return x.Equals(y);
    }
    public int GetHashCode([DisallowNull] RoutePathBase obj) => obj.GenRouteKey().GetHashCode();
    public bool Equals(RoutePathBase? other) => other != null && other.Method == Method && other.Path == Path;

    public override bool Equals(object? obj)
    {
        return obj is RoutePathBase route && this.Equals(route);
    }


}
