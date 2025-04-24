#version 330 core
out vec4 FragColor;

in vec3 vColor;
in vec2 vTexCoord;
in vec3 vPosition;

uniform sampler2D uMainTex;
uniform sampler2D uOverlayTex;
uniform sampler2D uGrassTex;

//uniform float uBiome;

void main()
{
    vec4 main = texture(uMainTex, vTexCoord);
    vec4 overlay = texture(uOverlayTex, vTexCoord);

    //float clampedBiome = clamp((vPosition.x / 16.0 + vPosition.z / 16.0) * 0.5, 0.0, 0.99);
    //float normalizedHeight = clamp(vPosition.y / 16.0, 0.0, 0.99);

    //float biomeCoord = mix(clampedBiome, 0, normalizedHeight);
    //vec4 grass = texture(uGrassTex, vec2(biomeCoord, normalizedHeight));
    vec4 grass = texture(uGrassTex, vec2(0.7, 0));
    vec4 color = mix(main, grass * overlay, overlay.a) * vec4(vColor, 1);

    if(color.a == 0)
        discard;

    FragColor = color;
}