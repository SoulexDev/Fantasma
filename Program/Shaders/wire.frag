#version 330 core
out vec4 FragColor;

in vec3 vPosition;

void main()
{
    FragColor = vec4(vPosition, 1);
}