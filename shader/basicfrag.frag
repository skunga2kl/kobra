#version 330 core

in vec3 FragPos;
in vec3 Normal;

out vec4 FragColor;

uniform vec3 u_LightDirection; 
uniform vec3 u_LightColor;
uniform vec3 u_ObjectColor;
uniform vec3 u_ViewPos;        
uniform float u_Intensity;

void main()
{
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(-u_LightDirection);

    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * u_LightColor * u_Intensity;

    vec3 ambient = 0.1 * u_LightColor;

    vec3 viewDir = normalize(u_ViewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0);
    vec3 specular = spec * u_LightColor * 0.5;

    vec3 result = (ambient + diffuse + specular) * u_ObjectColor;
    FragColor = vec4(result, 1.0);
}
