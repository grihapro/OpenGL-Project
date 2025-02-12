#version 400
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 texCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 Normal;
out vec3 FragPos; 
out vec2 TexCoords;

void main()
{
    FragPos = vec3(vec4(position, 1.0f) * model);
    gl_Position = vec4(FragPos, 1.0) * view * projection;
    Normal = normal * mat3(transpose(inverse(model)));
    TexCoords = texCoord;
}