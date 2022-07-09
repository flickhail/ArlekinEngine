using System;

using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Windowing;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ArlekinEngine;

internal class Camera
{
    private Vector3 _position = Vector3.Zero;

    private Vector3 _front = -Vector3.UnitZ;
    private Vector3 _right = Vector3.UnitX;
    private Vector3 _localUp = Vector3.UnitY;
    private Vector3 _worldUp = Vector3.UnitY;

    //  fov is in radians
    private float _fov = MathHelper.PiOver4;
    private float _aspect;
    private float _sensivity;

    // in radians for better performance
    private float _pitch = 0f;
    private float _yaw = -MathHelper.PiOver2;

    public Camera(Vector3 position, float fov, float aspect)
    {
        _position = position;
        _fov = fov;
        _aspect = aspect;
    }

    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
        }
    }

    public Vector3 Front => _front;
    public Vector3 Right => _right;
    public Vector3 LocalUp => _localUp;
    public float Aspect
    {
        get => _aspect;
        set
        {
            _aspect = value;
        }
    }

    public float Fov
    {
        get => MathHelper.RadiansToDegrees(_fov);
        set
        {
            _fov = MathHelper.DegreesToRadians(MathHelper.Clamp(value, 1f, 90f));
        }
    }

    public float Sensivity
    {
        get => _sensivity;
        set
        {
            _sensivity = value;
        }
    }

    public float Pitch
    {
        get => MathHelper.RadiansToDegrees(_pitch);
        set
        {
            _pitch = MathHelper.DegreesToRadians(MathHelper.Clamp(value , -89f, 89f));
            UpdateDirVectors();
        }
    }

    public float Yaw
    {
        get => MathHelper.RadiansToDegrees(_yaw);
        set
        {
            _yaw = MathHelper.DegreesToRadians(value);
        }
    }

    public Matrix4 GetView()
    {
        return Matrix4.LookAt(_position, _position + _front, _localUp);
    }

    public Matrix4 GetProjection()
    {
        return Matrix4.CreatePerspectiveFieldOfView(_fov, _aspect, 0.1f, 100f);
    }

    public void UpdateRotation(MouseState mouse)
    {
        Yaw += mouse.Delta.X * _sensivity;
        Pitch -= mouse.Delta.Y * _sensivity;
    }

    public void Zoom()
    {

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
