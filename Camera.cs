using System;

using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Windowing;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ArlekinEngine;

public enum CameraMovement
{
    Forward,
    Backward,
    Right,
    Left,
    Up,
    Down
}

/// <summary>
/// A class for FPS-like camera
/// </summary>
///
//  All angle-related is in radians for better performance
internal class Camera
{
    private Vector3 _position = Vector3.Zero;

    private Vector3 _front = -Vector3.UnitZ;
    private Vector3 _right = Vector3.UnitX;
    private Vector3 _localUp = Vector3.UnitY;
    private Vector3 _worldUp = Vector3.UnitY;

    private float _fov = MathHelper.PiOver4;
    private float _aspect;
    private float _sensivity;
    private float _speed = 0f;

    private float _pitch = 0f;
    private float _yaw = -MathHelper.PiOver2;

    /// <value>
    /// Front unit vector of the camera
    /// </value>
    public Vector3 Front => _front;
    /// <value>
    /// Right unit vector of the camera
    /// </value>
    public Vector3 Right => _right;
    /// <value>
    /// Local up unit vector of the camera
    /// </value>
    public Vector3 LocalUp => _localUp;

    /// <summary>
    /// Creation of camera object
    /// </summary>
    /// <remarks>
    /// <paramref name="position"/> - initial position of camera in 3D space <br/>
    /// <paramref name="fov"/> - camera's field of view in degrees <br/>
    /// <paramref name="aspect"/> - the ratio of width to height
    /// </remarks>
    public Camera(Vector3 position, float fov, float aspect)
    {
        _position = position;
        _fov = fov * MathF.PI / 180;
        _aspect = aspect;
    }

    /// <summary>
    /// The position of camera in 3D space
    /// </summary>
    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
        }
    }

    /// <summary>
    /// The camera's speed
    /// </summary>
    public float Speed
    {
        get => _speed;
        set
        {
            if (value < 0f)
            {
                value = 0f;
            }

            _speed = value;
        }
    }

    /// <summary>
    /// Relation of width by height of the screen
    /// </summary>
    public float Aspect
    {
        get => _aspect;
        set
        {
            _aspect = value;
        }
    }

    /// <summary>
    /// Field of view of the camera
    /// </summary>
    public float Fov
    {
        get => MathHelper.RadiansToDegrees(_fov);
        set
        {
            _fov = MathHelper.DegreesToRadians(MathHelper.Clamp(value, 1f, 90f));
        }
    }

    /// <summary>
    /// Rotation sensivity of the camera
    /// </summary>
    public float Sensivity
    {
        get => _sensivity;
        set
        {
            _sensivity = value;
        }
    }

    /// <summary>
    /// Rotation angle around X axis in radians
    /// </summary>
    public float Pitch
    {
        get => MathHelper.RadiansToDegrees(_pitch);
        set
        {
            _pitch = MathHelper.DegreesToRadians(MathHelper.Clamp(value , -89f, 89f));
            UpdateDirVectors();
        }
    }

    /// <summary>
    /// Rotation angle around Y axis in radians
    /// </summary>
    public float Yaw
    {
        get => MathHelper.RadiansToDegrees(_yaw);
        set
        {
            _yaw = MathHelper.DegreesToRadians(value);
        }
    }

    /// <summary>
    /// Creates matrix that transforms vectors to camera space
    /// </summary>
    /// <returns>Matrix of this transformation</returns>
    public Matrix4 GetView()
    {
        return Matrix4.LookAt(_position, _position + _front, _localUp);
    }

    /// <summary>
    /// Creates a perspective projection matrix
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <returns>Matrix that transforms camera space to perspective view</returns>
    public Matrix4 GetProjection()
    {
        return Matrix4.CreatePerspectiveFieldOfView(_fov, _aspect, 0.1f, 100f);
    }

    /// <summary>
    /// Rotates the camera around X and Y coordinates of 3D space
    /// </summary>
    /// 
    /// <remarks>
    ///     <paramref name="mouse"/> is a required mouse data
    /// </remarks>
    public void ProcessMouseRotation(MouseState mouse)
    {
        Yaw += mouse.Delta.X * _sensivity;
        Pitch -= mouse.Delta.Y * _sensivity;
    }

    /// <summary>
    /// Changes the position of camera in 3D space
    /// </summary>
    /// <remarks>
    ///     <paramref name="camMove"/> - directional ENUM <br/>
    ///     <paramref name="deltaTime"/> - travel time
    /// </remarks>
    public void ProcessMovementInput(CameraMovement camMove, float deltaTime)
    {
        switch(camMove)
        {
            case CameraMovement.Forward:
                Position += _speed * deltaTime * _front;
                break;
            case CameraMovement.Backward:
                Position -= _speed * deltaTime * _front;
                break;
            case CameraMovement.Right:
                Position += _speed * deltaTime * Vector3.Normalize(Vector3.Cross(_front, _localUp));
                break;
            case CameraMovement.Left:
                Position -= _speed * deltaTime * Vector3.Normalize(Vector3.Cross(_front, _localUp));
                break;
            case CameraMovement.Up:
                Position += _speed * deltaTime * _worldUp;
                break;
            case CameraMovement.Down:
                Position -= _speed * deltaTime * _worldUp;
                break;
        }
    }

    /// <summary>
    /// Zoom of camera <br/>
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// <paramref name="offset"/> - offset in angles
    /// </remarks>
    public void Zoom(float offset)
    {
        Fov -= offset;
    }

    // Each change in pitch and yaw should cause recalculation of camera's direction vectors
    private void UpdateDirVectors()
    {
        _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
        _front.Y = MathF.Sin(_pitch);
        _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);
        _front = Vector3.NormalizeFast(_front);

        _right = Vector3.Normalize(Vector3.Cross(_front, _worldUp));
        _localUp = Vector3.Normalize(Vector3.Cross(_right, _front));
    }
}
