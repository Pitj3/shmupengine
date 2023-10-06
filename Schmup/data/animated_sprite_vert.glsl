#version 330 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;

out vec2 frag_texCoords;

uniform vec2 uLocation;

uniform mat4 projection;
uniform mat4 view;

uniform uint width;
uniform uint height;

uniform uint xframes;
uniform uint yframes;

uniform vec2 offset;

void main()
{
    frag_texCoords = aTexCoord / vec2(xframes, yframes) + offset;

    gl_Position = projection * view * (vec4(uLocation, 0, 0) + vec4(aPos.x * width, aPos.y * height, aPos.z, 1.0));
}