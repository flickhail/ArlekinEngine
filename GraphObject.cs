using System;
using OpenTK.Graphics.OpenGL4;

namespace ArlekinEngine;

internal class GraphObject
{
    private int _vbo;
    private int _vao;
    private int _vaoAttrIndex;

    public GraphObject(float[] data)
    {
        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);

        _vao = GL.GenVertexArray();
        _vaoAttrIndex = 0;
    }

    public void SetAttribute(int size, VertexAttribPointerType type, bool normalized, int stride, int offset)
    {
        GL.BindVertexArray(_vao);
        GL.VertexAttribPointer(_vaoAttrIndex, size, type, normalized, stride, offset);
        GL.EnableVertexAttribArray(_vaoAttrIndex);

        _vaoAttrIndex++;
    }

    public void Bind()
    {
        GL.BindVertexArray(_vao);
    }
}
