using System;
using OpenTK.Graphics.OpenGL4;

namespace ArlekinEngine;

internal class ShapeObject : IDisposable
{
    private int _vbo;
    public int VBO => _vbo;
    private bool _sharedVBO;

    private int _vao;
    private int _vaoAttrIndex;

    private bool _disposed;


    /// <summary>
    /// Creates the VBO and VAO and sends the vertex data to GPU
    /// </summary>
    /// <remarks>
    /// Changes the VBO context
    /// </remarks>
    public ShapeObject(float[] data)
    {
        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);

        _vao = GL.GenVertexArray();
        _vaoAttrIndex = 0;
    }

    /// <summary>
    /// Creates the VAO using existing VBO
    /// </summary>
    /// <remarks>
    /// Changes the VBO context
    /// </remarks>
    public ShapeObject(int vbo)
    {
        _sharedVBO = true;
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

        _vao = GL.GenVertexArray();
        _vaoAttrIndex = 0;
    }

    public void SetAttribute(int size, VertexAttribPointerType type, bool normalized, int stride, int offset)
    {
        GL.BindVertexArray(_vao);

        GL.VertexAttribPointer(_vaoAttrIndex, size, type, normalized, stride, offset);
        GL.EnableVertexAttribArray(_vaoAttrIndex);

        _vaoAttrIndex++;
        GL.BindVertexArray(0);
    }

    /// <summary>
    /// Changes the current VAO context
    /// </summary>
    public void Bind()
    {
        GL.BindVertexArray(_vao);
    }

    public void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (!_sharedVBO)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(_vbo);
        }

        GL.BindVertexArray(0);
        GL.DeleteVertexArray(_vao);
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
