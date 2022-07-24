using System;
using System.Diagnostics;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ArlekinEngine;

public class Window2 : GameWindow
{

    private KeyboardState _keyState;

    private Shader _objectShader;
    private Shader _lightShader;
    private readonly string _shaderFolder = "D:\\heh\\projects\\prog\\csharp\\MyNewLibrary\\Shaders\\";
    private readonly string _texturePath = "D:\\heh\\projects\\prog\\csharp\\MyNewLibrary\\Textures\\";

    private Stopwatch _timer;

    private Texture _wallTex;
    private Texture _faceTex;

    private Matrix4 _modelMat;
    private Matrix4 _viewMat;
    private Matrix4 _projectionMat;
    private Matrix4 _transformation;

    private Camera camera;

    private ShapeObject _container;
    private Vector3 _containerColor;

    private ShapeObject _lightSource;
    private Vector3 _lightPos;
    private Vector3 _lightColor;
    private Vector3 _lightScale;

    private Vector4 _sceneColor = new Vector4(0.2f, 0.3f, 0.3f, 1.0f);

    private ObjectData[] _cubes;
    private Vector3[] _cubePositions =
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
    private int _objCount;

    private readonly float[] _triangle =
    {
        -0.5f, -0.5f, 0.0f,
        0.0f, 0.5f, 0.0f,
        0.5f, -0.5f, 0.0f
    };

    private readonly float[] _cubeVertices =
    {
        //  vertex coords // //texCoords// // normal vector //
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,   0.0f, 0.0f, -1.0f,  
         0.5f, -0.5f, -0.5f,  1.0f, 0.0f,   0.0f, 0.0f, -1.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,   0.0f, 0.0f, -1.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,   0.0f, 0.0f, -1.0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,   0.0f, 0.0f, -1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,   0.0f, 0.0f, -1.0f,

        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,   0.0f, 0.0f, 1.0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,   0.0f, 0.0f, 1.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 1.0f,   0.0f, 0.0f, 1.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 1.0f,   0.0f, 0.0f, 1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,   0.0f, 0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,   0.0f, 0.0f, 1.0f,

        -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,   -1.0f, 0.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,   -1.0f, 0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,   -1.0f, 0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,   -1.0f, 0.0f, 0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,   -1.0f, 0.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,   -1.0f, 0.0f, 0.0f,

         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,   1.0f, 0.0f, 0.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,   1.0f, 0.0f, 0.0f,
         0.5f, -0.5f, -0.5f,  0.0f, 1.0f,   1.0f, 0.0f, 0.0f,
         0.5f, -0.5f, -0.5f,  0.0f, 1.0f,   1.0f, 0.0f, 0.0f,
         0.5f, -0.5f,  0.5f,  0.0f, 0.0f,   1.0f, 0.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,   1.0f, 0.0f, 0.0f,

        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,   0.0f, -1.0f, 0.0f,
         0.5f, -0.5f, -0.5f,  1.0f, 1.0f,   0.0f, -1.0f, 0.0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,   0.0f, -1.0f, 0.0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,   0.0f, -1.0f, 0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,   0.0f, -1.0f, 0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,   0.0f, -1.0f, 0.0f,

        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,   0.0f, 1.0f, 0.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,   0.0f, 1.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,   0.0f, 1.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,   0.0f, 1.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,   0.0f, 1.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,   0.0f, 1.0f, 0.0f
    };

    private readonly float[] texCoords =
    {
        0.0f, 0.0f,
        0.5f, 1f,
        1.0f, 0.0f
    };

    public Window2(GameWindowSettings gameWinSet, NativeWindowSettings natWinSet) : base(gameWinSet, natWinSet)
    {
        _sceneColor = new Vector4(0f, 0f, 0f, 1.0f); // dark
        //_sceneColor = new Vector4(0.2f, 0.3f, 0.3f, 1.0f); // light
        _timer = new Stopwatch();
        _objCount = 1;
        _lightPos = new Vector3(1.2f, 1.0f, 2.0f);
        _lightColor = Vector3.One;
        _lightScale = new Vector3(0.2f);
    }

    protected override void OnLoad()
    {
        Debug.Log("Loading...");

        base.OnLoad();

        CursorState = CursorState.Grabbed;
        
        GL.ClearColor(_sceneColor.X, _sceneColor.Y, _sceneColor.Z, _sceneColor.W);
        GL.Enable(EnableCap.DepthTest);

        _container = new ShapeObject(_cubeVertices);
        _container.SetAttribute(3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        _container.SetAttribute(2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
        _container.SetAttribute(3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));

        _lightSource = new ShapeObject(_container.VBO);
        _lightSource.SetAttribute(3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

        _objectShader = new Shader($"{_shaderFolder}objectLighting.vert", $"{_shaderFolder}objectLighting.frag");
        _lightShader = new Shader($"{_shaderFolder}light.vert", $"{_shaderFolder}light.frag");

        Debug.Log("Shaders initialized");

        _objectShader.Use();
        _objectShader.SetUnif1("texture0", 0);
        _objectShader.SetUnif1("texture1", 1);

        _wallTex = new Texture($"{_texturePath}wall.jpg");
        _faceTex = new Texture($"{_texturePath}awesomeface.png");
        Debug.Log("Textures are loaded");

        Texture.SetParam(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        Texture.SetParam(TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        Debug.Log("awesomeface.png initialized");

        camera = new (3f * Vector3.UnitZ, 45, Size.X / (float)Size.Y);

        _cubes = new ObjectData[_objCount];
        for (int i = 0; i < _objCount; i++)
        {
            _cubes[i] = new (_cubePositions[i], Vector3.One, Vector3.Zero);
        }

        _timer.Start();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _viewMat = camera.GetView();
        _projectionMat = camera.GetProjection();

        RenderObjects();
        RenderLightSource();

        Context.SwapBuffers();
    }

    private void RenderObjects()
    {
        _objectShader.Use();
        float j;
        for (int i = 0; i < _cubes.Length; i++)
        {
            j = MathF.Pow(-1, i) * i * (float)_timer.Elapsed.TotalSeconds / 2;
            Vector3 rotation = new(MathHelper.PiOver2 + j, MathHelper.PiOver2 - j, 0f);

            _modelMat = CreateTransform(_cubePositions[i], Vector3.One, rotation);

            Matrix4 normMat = Matrix4.Transpose(Matrix4.Invert(_modelMat));

            _objectShader.SetUnifMat4("normal", true, ref normMat);
            _objectShader.SetUnifMat4("model", true, ref _modelMat);
            _objectShader.SetUnifMat4("view", true, ref _viewMat);
            _objectShader.SetUnifMat4("projection", true, ref _projectionMat);
            _objectShader.SetUnif3("lightColor", _lightColor);
            _objectShader.SetUnif3("light.position", _lightPos);
            _objectShader.SetUnif3("light.specularStrength", Vector3.One * 0.1f);
            _objectShader.SetUnif3("light.diffuseStrength", Vector3.One);
            _objectShader.SetUnif3("viewPos", camera.Position);

            _wallTex.Use(TextureUnit.Texture0);
            _faceTex.Use(TextureUnit.Texture1);

            _container.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
    }

    private void RenderLightSource()
    {
        _lightShader.Use();

        _modelMat = CreateTransform(_lightPos, _lightScale, Vector3.Zero);
        _transformation = Matrix4.Identity * _modelMat * _viewMat * _projectionMat;

        _lightShader.SetUnifMat4("transformation", true, ref _transformation);
        _lightShader.SetUnif3("lightColor", _lightColor);

        _lightSource.Bind();
        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (!IsFocused)
        {
            return;
        }

        camera.Sensivity = 0.1f;
        camera.Speed = 3f;
        _keyState = KeyboardState;

        camera.ProcessMouseRotation(MouseState);

        if (_keyState.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        if (_keyState.IsKeyDown(Keys.W))
        {
            camera.ProcessMovementInput(CameraMovement.Forward, (float)e.Time);
        }

        if (_keyState.IsKeyDown(Keys.S))
        {
            camera.ProcessMovementInput(CameraMovement.Backward, (float)e.Time);
        }

        if (_keyState.IsKeyDown(Keys.A))
        {
            camera.ProcessMovementInput(CameraMovement.Left, (float)e.Time);
        }

        if (_keyState.IsKeyDown(Keys.D))
        {
            camera.ProcessMovementInput(CameraMovement.Right, (float)e.Time);
        }

        if (_keyState.IsKeyDown(Keys.Space))
        {
            camera.ProcessMovementInput(CameraMovement.Up, (float)e.Time);
        }

        if (_keyState.IsKeyDown(Keys.C))
        {
            camera.ProcessMovementInput(CameraMovement.Down, (float)e.Time);
        }
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        camera.Zoom(e.OffsetY);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
        camera.Aspect = Size.X / (float)Size.Y;
    }

    private Matrix4 CreateTransform(Vector3 pos, Vector3 scale, Vector3 rotation)
    {
        Matrix4 tmp = Matrix4.Identity;

        tmp *= Matrix4.CreateRotationX(rotation.X);
        tmp *= Matrix4.CreateRotationY(rotation.Y);
        tmp *= Matrix4.CreateRotationZ(rotation.Z);
        tmp *= Matrix4.CreateScale(scale);
        tmp *= Matrix4.CreateTranslation(pos);

        return tmp;
    }

    

    protected override void OnUnload()
    {
        base.OnUnload();

        _container.Dispose();
        _objectShader.Dispose();
    }
}

