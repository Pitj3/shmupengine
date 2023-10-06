#version 330 core

in vec2 frag_texCoords;
out vec4 FragColor;

uniform sampler2D uTexture;

void main()
{
	vec4 color = texture(uTexture, frag_texCoords);
    FragColor = color;
}