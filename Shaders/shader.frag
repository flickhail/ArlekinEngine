#version 330 core

uniform sampler2D texture0;
uniform sampler2D texture1;

in vec2 texCoord;
//in vec3 color;

out vec4 FragColor;

void main()
{
	vec2 wallCoord = texCoord / 2;
    vec2 faceCoord = texCoord;//vec2(-texCoord.y, texCoord.x);
    FragColor = mix(texture(texture0, wallCoord), texture(texture1, faceCoord), 0.5); //* vec4(color, 1.0);
}