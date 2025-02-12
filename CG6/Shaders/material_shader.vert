#version 400
in vec3 position;
in vec3 normal;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 Normal;
out vec3 FragPos; 

void main()
{
    FragPos = vec3(vec4(position, 1.0f) * model);
    gl_Position = vec4(FragPos, 1.0) * view * projection;
    Normal = normal * mat3(transpose(inverse(model)));
}