#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

float4 FogColour;
float Fog;

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Colour : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 colour = tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Colour;
	

	float4 outputColour;
	if(colour.a > 0)
		outputColour = (Fog * FogColour) + ((1 - Fog) * colour);

	outputColour.a = colour.a;

	return outputColour;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};