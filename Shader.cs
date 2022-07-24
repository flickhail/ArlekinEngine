using System;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ArlekinEngine;

internal class Shader : IDisposable
{
    //      This is main shader object that represents a shader program
    public readonly int Handle;
    private bool disposed = false;

    /// <summary>
    /// Create and compile the vert and the frag shaders
    /// </summary>
    /// <remarks>
    /// Doesn't change the shader context
    /// </remarks>
    public Shader(string vertexPath, string fragmentPath)
    {
        int VertexShader;
        int FragmentShader;

        //      Reading source code from a file
        string vertexShaderSource = ReadSource(vertexPath, Encoding.UTF8);
        string fragmentShaderSource = ReadSource(fragmentPath, Encoding.UTF8);


        //      Creating an OpenGL objects of shader type
        VertexShader = GL.CreateShader(ShaderType.VertexShader);
        FragmentShader = GL.CreateShader(ShaderType.FragmentShader);


        //      Binding created object with corresponds source code
        GL.ShaderSource(VertexShader, vertexShaderSource);
        GL.ShaderSource(FragmentShader, fragmentShaderSource);


        //      Compilation of source code and check for any compilation errors
        try
        {
            GL.CompileShader(VertexShader);
            CheckCompileInfo(VertexShader);

            GL.CompileShader(FragmentShader);
            CheckCompileInfo(FragmentShader);
        }
        catch (InvalidDataException e)
        {
            Console.WriteLine("Render compilation error: " + e.Message);
            return;
        }


        //      Creating a whole shader program from it's vertex and fragment parts
        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, VertexShader);
        GL.AttachShader(Handle, FragmentShader);

        GL.LinkProgram(Handle);


        //      Cleanup, because we don't need vert and frag shaders to be attached and remain created,
        //      because we already link them in the whole program
        GL.DetachShader(Handle, VertexShader);
        GL.DetachShader(Handle, FragmentShader);
        GL.DeleteShader(VertexShader);
        GL.DeleteShader(FragmentShader);

        Debug.Log($"Shader [{Handle}] created");
    }

    /// <summary>
    /// Change current shader context
    /// </summary>
    public void Use()
    {
        GL.UseProgram(Handle);
    }

    /// <summary>
    /// Sends the int value to the uniform field of the shader
    /// </summary>
    /// <remarks>
    /// Needs shader context
    /// </remarks>
    public void SetUnif1(string varName, int value)
    {
        int loc = GL.GetUniformLocation(Handle, varName);
        GL.Uniform1(loc, value);
    }

    /// <summary>
    /// Sends the vec3 value to the uniform field of the shader
    /// </summary>
    /// <remarks>
    /// Needs shader context
    /// </remarks>
    public void SetUnif3(string varName, Vector3 vec)
    {
        int loc = GL.GetUniformLocation(Handle, varName);
        GL.Uniform3(loc, ref vec);
    }

    /// <summary>
    /// Sends the Matrix4 data to the uniform field of the shader
    /// </summary>
    /// <remarks>
    /// Needs shader context
    /// </remarks>
    public void SetUnifMat4(string varName, bool transpose, ref Matrix4 value)
    {
        int loc = GL.GetUniformLocation(Handle, varName);
        GL.UniformMatrix4(loc, transpose, ref value);
    }


    //      Is used for reading source code from a file with it's encoding
    private static string ReadSource(string shaderPath, Encoding encoding)
    {
        string shaderSource;
        using (StreamReader reader = new StreamReader(shaderPath, encoding))
        {
            shaderSource = reader.ReadToEnd();
        }

        return shaderSource;
    }


    //      Checks if there is some compilation errors
    private static void CheckCompileInfo(int shader)
    {
        string infoLog = GL.GetShaderInfoLog(shader);
        if (infoLog != String.Empty)
        {
            throw new InvalidDataException(infoLog);
        }
    }

    private void Dispose(bool disposing)
    {
        if (!disposed)
        {
            GL.UseProgram(0);
            GL.DeleteProgram(Handle);
            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Shader()
    {
        GL.DeleteProgram(Handle);
    }
}
