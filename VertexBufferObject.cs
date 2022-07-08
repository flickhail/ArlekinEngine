using System;

using OpenTK.Graphics.OpenGL4;

namespace ArlekinEngine;

internal class VertexBufferObject
{
    public readonly int Handle;

    public VertexBufferObject(float[] vertices, BufferUsageHint usageHint)
    {
        Handle = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, usageHint);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }

    public void Bind(bool isBounded)
    {
        if (isBounded) GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
        else GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }
}
