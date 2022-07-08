#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;
//layout(location = 2) in vec3 aColor;

uniform mat4 transformation;

out vec2 texCoord;
//out vec3 color;

void main() 
{
    gl_Position = vec4(aPosition, 1.0) * transformation;
    texCoord = aTexCoord;
    //color = aColor;
}