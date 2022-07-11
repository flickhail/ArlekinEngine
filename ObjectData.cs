using System;

using OpenTK;
using OpenTK.Mathematics;

namespace ArlekinEngine;

/// <summary>
/// Structure that holds all the data about an object
/// </summary>
internal struct ObjectData
{
    public float[] VertData;
    public Vector3 Position;
    public Vector3 Scale;
    public Vector3 Rotation;

    public ObjectData(ref float[] data, Vector3 pos, Vector3 sc, Vector3 rot)
    {
        VertData = data;
        Position = pos;
        Scale = sc;
        Rotation = rot;
    }

    public override string ToString()
    {
        return $"position: {Position.ToString()}\tscale: {Scale.ToString()}\trotation: {Rotation.ToString()}";
    }
}
