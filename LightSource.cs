using System;

using OpenTK.Mathematics;

namespace ArlekinEngine;

internal struct LightSource
{
    public Vector3 Position;
    public Vector3 Scale;
    public Vector3 Color;

    public LightSource(Vector3 position, Vector3 color, Vector3 scale)
    {
        Position = position;
        Color = color;
        Scale = scale;
    }
}
