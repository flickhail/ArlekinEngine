using System;

using OpenTK.Graphics.OpenGL4;

namespace ArlekinEngine;

internal class VertexArrayObject
{
    public readonly int Handle;


    public VertexArrayObject(VertexBufferObject vbo)
    {
        vbo.Bind(true);
        Handle = GL.GenVertexArray();
    }

    public void Bind(bool isBounded)
    {
        if (isBounded) GL.BindVertexArray(Handle);
        else GL.BindVertexArray(0);
    }

    public void SetAttrib(int index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset)
    {
        GL.BindVertexArray(Handle);

        GL.EnableVertexAttribArray(Handle);
        GL.VertexAttribPointer(index, size, type, normalized, stride, offset);

        GL.BindVertexArray(0);
    }
}
