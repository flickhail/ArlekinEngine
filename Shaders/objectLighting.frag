#version 330 core

struct Material 
{
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};

struct Light
{
    vec3 position;
    
    vec3 ambientStrength;
    vec3 diffuseStrength;
    vec3 specularStrength;
};

uniform Material material;
uniform Light light;

uniform sampler2D texture0;
uniform sampler2D texture1;
uniform vec3 lightColor;
uniform vec3 viewPos;

in vec2 texCoord;
in vec3 Normal;
in vec3 FragPos;

out vec4 FragColor;

void main()
{
	vec2 wallCoord = texCoord / 2;
    vec2 faceCoord = texCoord;
    vec3 objectColor = mix(texture(texture0, wallCoord), texture(texture1, faceCoord), 0.5).xyz;

    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;

    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);

    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuseStrength * diff * lightColor;

    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);

    float spec = pow(max(dot(viewDir, reflectDir), 0), 128);
    vec3 specular = light.specularStrength * spec * lightColor;

    vec3 result = (ambient + diffuse + specular) * objectColor;
    FragColor = vec4(result, 1.0);
}