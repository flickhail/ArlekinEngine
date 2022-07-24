using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics.OpenGL4;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;


namespace ArlekinEngine;

internal class Texture
{
    private int Handle;

    /// <summary>
    /// Creates a texture
    /// </summary>
    /// <remarks>
    /// Changes the texture context
    /// </remarks>
    public Texture(string path)
    {
        Handle = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, Handle);

        using Image<Rgba32> image = Image.Load<Rgba32>(path);
        image.Mutate(x => x.Flip(FlipMode.Vertical));

        byte[] bytePixels = new byte[4 * image.Width * image.Height];
        Span<Rgba32> colorsArray = new Span<Rgba32>(new Rgba32[image.Height * image.Width]);
        image.CopyPixelDataTo(colorsArray);

        //  Moving color data from Span<Rgba32> to byte[]
        for (int i = 0, j = 0; i < bytePixels.Length && j < colorsArray.Length; j++)
        {
            bytePixels[i++] = colorsArray[j].R;
            bytePixels[i++] = colorsArray[j].G;
            bytePixels[i++] = colorsArray[j].B;
            bytePixels[i++] = colorsArray[j].A;
        }

        //  Creating the texture
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, bytePixels);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        Debug.Log($"Texture [{Handle}] created");
    }


    /// <summary>
    /// Changes the current texture context
    /// </summary>
    /// <remarks>
    /// <paramref name="texUnit"/> - the texture unit at wich you want to set the texture
    /// </remarks>
    public void Use(TextureUnit texUnit)
    {
        GL.ActiveTexture(texUnit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }

    /// <summary>
    /// Sets the texture parameter
    /// </summary>
    /// <remarks>
    /// Needs the texture context
    /// </remarks>
    public static void SetParam(TextureParameterName parName, int value)
    {
        GL.TexParameter(TextureTarget.Texture2D, parName, value);
    }
}
