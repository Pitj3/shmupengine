#version 330 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;

out vec2 frag_texCoords;

uniform mat4 projection;
uniform mat4 view;

void main()
{
    frag_texCoords = aTexCoord;

    gl_Position = projection * view * vec4(aPos.x, aPos.y, aPos.z, 1.0);
}