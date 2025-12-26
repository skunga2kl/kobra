#version 450 core

layout(location = 0) in vec3 aPosition;

uniform mat4 u_MVP;

void main()
{
    gl_Position = u_MVP * vec4(aPosition, 1.0);
}
