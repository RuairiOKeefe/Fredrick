#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

const static int MAX_LIGHTS = 16;
const static int UNIT_SCALE = 32; //The size of a world unit

float4x4 world;
float4x4 wvp;

float4 emissive;
float4 diffuseReflection;

float3 position[16];
float4 colour[16];

Texture2D SpriteTexture;
Texture2D NormalMap;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

sampler2D NormalMapSampler = sampler_state
{
	Texture = <NormalMap>;
};

struct VertexShaderOutput
{
	float4 Position : POSITION;
	float4 Colour : COLOR0;
	float4 TexCoord : TEXCOORD0;
	float4 PosW : TEXCOORD3;
};

VertexShaderOutput MainVS(float4 Position : POSITION0, float4 Colour : COLOR0, float4 TexCoord : TEXCOORD0)
{
	VertexShaderOutput Out;
	Out.Position = mul(Position, wvp);
	Out.Colour = Colour;
	Out.TexCoord = TexCoord;
	Out.PosW = mul(Position, world);
	return Out;
}

float4 MainPS(in VertexShaderOutput input) : COLOR
{
	float4 texColour = tex2D(SpriteTextureSampler, input.TexCoord.xy) * input.Colour;
	float4 normalColour = tex2D(NormalMapSampler, input.TexCoord.xy);
	normalColour = (normalColour * 2) - 1;
	float4 diffuse = { 0,0,0,0 };

	float3 pixelPos = input.PosW.xyz;

	for (int i = 0; i < MAX_LIGHTS; i++)
	{
		float3 tempPos = { position[i].x, -position[i].y, position[i].z  };
		tempPos *= UNIT_SCALE;
		// ********************************
		// Calculate direction to the light
		// ********************************
		float3 lightDir = normalize(tempPos - pixelPos);

		// ***************************
		// Calculate distance to light
		// ***************************
		float d = distance(tempPos, pixelPos);

		// ***************************
		// Calculate attenuation value
		// ***************************
		float attenuation =  d;

		// **********************
		// Calculate light colour
		// **********************
		float4 lightColour = colour[i] / d;

		float3 normal = { normalColour.x, -normalColour.y, -normalColour.z };
		// ******************************************************************************
		// Now use standard phong shading but using calculated light colour and direction
		// - note no ambient
		// ******************************************************************************
		diffuse += (lightColour*diffuseReflection) *max(dot(normal, lightDir), 0.0);
	}

	float4 colour = (emissive + diffuse) * texColour;
	colour.a = texColour.a;

	return colour;
}

technique SpriteDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};