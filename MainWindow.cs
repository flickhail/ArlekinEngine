﻿using System;
using System.Diagnostics;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ArlekinEngine;

public class MainWindow : GameWindow
{
    private int _VertexBufferObject;
    private int _ElementBufferObject;
    private int _VertexArrayObject;

    private KeyboardState _keyState;

    private int _vertColor;


    private Shader _shader;
    private readonly string _shaderFolder = "D:\\heh\\projects\\prog\\csharp\\MyNewLibrary\\Shaders\\";
    private readonly string _testPath = "D:\\heh\\projects\\prog\\csharp\\MyNewLibrary\\";
    private Counter _counter;

    private Stopwatch _timer;
    private double _time;

    private Texture _wallTex;
    private Texture _faceTex;

    private Matrix4 _modelMat;
    private Matrix4 _viewMat;
    private Matrix4 _projectionMat;
    private float _FOV;

    private Matrix4 _transformation;

    private Vector3 camPos;
    private Vector3 camTarget;
    private Vector3 up = new(0f, 1f, 0f);

    private readonly float[] triangle =
    {
        -0.5f, -0.5f, 0.0f,
        0.0f, 0.5f, 0.0f,
        0.5f, -0.5f, 0.0f
    };

    float[] vertices = 
    {
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
        0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
        0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

        -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
         0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
         0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
         0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
         0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
    };

    Vector3[] cubePositions = 
    {
        new Vector3( 0.0f,  0.0f,  0.0f),
        new Vector3( 2.0f,  5.0f, -15.0f),
        new Vector3(-1.5f, -2.2f, -2.5f),
        new Vector3(-3.8f, -2.0f, -12.3f),
        new Vector3( 2.4f, -0.4f, -3.5f),
        new Vector3(-1.7f,  3.0f, -7.5f),
        new Vector3( 1.3f, -2.0f, -2.5f),
        new Vector3( 1.5f,  2.0f, -2.5f),
        new Vector3( 1.5f,  0.2f, -1.5f),
        new Vector3(-1.3f,  1.0f, -1.5f)
    };

    private readonly float[] texCoords =
    {
        0.0f, 0.0f,
        0.5f, 1f,
        1.0f, 0.0f
    };

    private readonly float[] rectangle6 =
    {
        // The first triangle
        -0.5f, -0.5f, 0.0f,
        -0.5f, 0.5f, 0.0f,
        0.5f, -0.5f, 0.0f,

        //  The second triangle
        0.5f, -0.5f, 0.0f,
        -0.5f, 0.5f, 0.0f,
        0.5f, 0.5f, 0.0f
    };

    private readonly float[] rectangle4 =
    {
        // position             // texCoords    // color
        -0.5f, -0.5f, 0.0f,     0.0f, 0.0f,     1.0f, 0.3f, 0.1f,
        -0.5f, 0.5f, 0.0f,      0.0f, 2.0f,     0.2f, 0.2f, 1.0f,
        0.5f, 0.5f, 0.0f,       2.0f, 2.0f,     0.0f, 1.0f, 0.0f,
        0.5f, -0.5f, 0.0f,      2.0f, 0.0f,     1.0f, 1.0f, 0.0f
    };

    private readonly uint[] indices =
    {
        0, 1, 2,
        0, 2, 3
    };

    private double FPS 
    { 
        get 
        {
            return 1 / UpdateTime;
        }
    }

    public MainWindow(GameWindowSettings gameWinSet, NativeWindowSettings natWinSet) : base(gameWinSet, natWinSet) 
    {
        _counter = new Counter(10000);
        _timer = new Stopwatch();

        _FOV = MathF.PI / 4;

        camPos = new Vector3(0f, 0f, -3f);
        camTarget = Vector3.Zero;
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        //  Setting the default color for Clear() method
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        GL.Enable(EnableCap.DepthTest);

        //  Initializing the shader
        _shader = new Shader($"{_shaderFolder}shader.vert", $"{_shaderFolder}fragShader.frag");

        //  Creating buffer object that holds all vertex data. BindBuffer is a global state
        _VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _VertexBufferObject);

        //  Creating array buffer on GPU that managed by buffer object
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        //  VAO is need for creating data structure in buffer, because without VAO in a buffer would be just array of bytes
        //  and shader can't know how should interpret these bytes as vertex position
        //  so VAO is an object that keeps this data structure and tells OpenGL the way it should interpret this bytes in a buffer
        _VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_VertexArrayObject);

        //  This represents the data structure. There was BindVertexArray() call, so OpenGL state is moved to keeping this
        //  representation in the VAO object
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        //GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 5 * sizeof(float));
        //GL.EnableVertexAttribArray(2);

        //  This is for draw optimization. If we want to draw a rectangle using triangles, we need 6 vertices,
        //  this is enefficient, so we can use EBO to draw only 4 vertices
        //_ElementBufferObject = GL.GenBuffer();
        //GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ElementBufferObject);
        //GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        //  Enabling the shader. This is global state
        _shader.Use();

        _wallTex = new Texture($"{_testPath}wall.jpg");
        _faceTex = new Texture($"{_testPath}awesomeface.png");

        _faceTex.Use(TextureUnit.Texture0);
        Texture.SetParam(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        Texture.SetParam(TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        _shader.SetUnif1("texture0", 0);
        _shader.SetUnif1("texture1", 1);

        _timer.Start();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        //  Clears only color data from screen and sets default color using it from GL.ClearColor()
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        float radius = 10f;
        float camXCoord = MathF.Cos((float)_timer.Elapsed.TotalSeconds) * radius;
        float camZCoord = MathF.Sin((float)_timer.Elapsed.TotalSeconds) * radius;

        _viewMat = Matrix4.LookAt(new Vector3(camXCoord, 0f, camZCoord), Vector3.Zero, up);
        Vector3 camTrans = new(camXCoord, 0f, -3f);
        //CreateViewMat(_vAngleVec, _vScaleVec, camTrans);
        _projectionMat = Matrix4.Identity * Matrix4.CreatePerspectiveFieldOfView(_FOV, (float)Size.X / Size.Y, 0.1f, 100f);
        float j = 0;
        //  Drawing a 10 cubes
        for (int i = 0; i < cubePositions.Length; i++)
        {

            j = MathF.Pow(-1, i) * i * (float)_timer.Elapsed.TotalSeconds / 2;

            _modelMat = Matrix4.Identity * Matrix4.CreateRotationX(MathF.PI / 5 * j);
            _modelMat = _modelMat * Matrix4.CreateRotationY(MathF.PI / 6 * j);
            _modelMat = _modelMat * Matrix4.CreateRotationZ(0f);
            _modelMat = _modelMat * Matrix4.CreateScale(1f);
            _modelMat = _modelMat * Matrix4.CreateTranslation(cubePositions[i]);

            _transformation = Matrix4.Identity * _modelMat * _viewMat * _projectionMat;

            _shader.SetUnifMat4("transformation", true, ref _transformation);

            _wallTex.Use(TextureUnit.Texture0);
            _faceTex.Use(TextureUnit.Texture1);

            GL.BindVertexArray(_VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }

        //_time = _timer.Elapsed.TotalSeconds;
        //_greenComp = (float)Math.Sin(_time*2) / 2.0f + 0.5f;
        //_redComp = (float)Math.Cos(_time) / 2.0f + 0.5f;
        //_blueComp = (float)Math.Sin(_time*3 + Math.PI/2) / 2.0f + 0.5f;
        //_vertColor = GL.GetUniformLocation(_shader.Handle, "ourColor");
        //if (_vertColor == -1) throw new Exception();
        //GL.Uniform4(_vertColor, _redComp, _greenComp, _blueComp, 0.0f);

        //  Binding the VAO

        //  Draw the data from VAO using "shader"
        //  This draws all vertices stored in VBO
        //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);


        //  OpenTK windows are double-buffered. Right now we finished rendering current screen and all this data is stored
        //  in render buffer, meanwhile on the screen we can see only displayed buffer. So we need to switch buffers
        Context.SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        _keyState = KeyboardState;

        float camSpeed = 0.05f;

        if (_keyState.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        if (_keyState.IsKeyPressed(Keys.Up))
        {
            
        }

        if (_keyState.IsKeyPressed(Keys.Left))
        {
            
        }

        if (_keyState.IsKeyPressed(Keys.Right))
        {
            
        }


        //Console.WriteLine($"FPS: {FPS}");
        //Console.WriteLine($"Render time: {_counter.Adjust(RenderTime)}");
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
    }


    //  Cleanup do not need at the closing application.
    //  This is just showing the way how you should do it
    //  if you want do free resources from VRAM
    protected override void OnUnload()
    {
        base.OnUnload();

        //  There is need in cleanup resources on GPU memory
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);

        GL.DeleteBuffer(_VertexBufferObject);
        GL.DeleteVertexArray(_VertexArrayObject);

        //  Clear the shader resources
        _shader.Dispose();
    }

    private void CreateViewMat(Vector3 angle, Vector3 scale, Vector3 translation)
    {
        _viewMat = Matrix4.Identity * Matrix4.CreateRotationX(angle.X);
        _viewMat = _viewMat * Matrix4.CreateRotationY(angle.Y);
        _viewMat = _viewMat * Matrix4.CreateRotationZ(angle.Z);
        _viewMat = _viewMat * Matrix4.CreateScale(scale);
        _viewMat = _viewMat * Matrix4.CreateTranslation(translation);
    }
}

