#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

const static int MAX_LIGHTS = 16;

float4 emissive;
float4 diffuse_reflection;
float4 specular_reflection;
float shininess;

float3 position[16];
float3 direction[16];
float4 colour[16];
float constantK[16];
float linearK[16];
float quadraticK[16];

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float3 Normal : NORMAL0;
	float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
	float3 Position : TEXCOORD3;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainVs(in VertexShaderInput input, out VertexShaderOutput output) : POSITION
{
	output.Position = input.Position.xyz;
	output.Color = input.Color;
	output.TextureCoordinates = input.TextureCoordinates;
	return input.Position;
}

float4 MainPS(in VertexShaderOutput input) : COLOR
{
	float4 texColour = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;

	float4 diffuse;
	float4 specular;

	float3 normal = { 0, 0, -1 };

	for (int i = 0; i < MAX_LIGHTS; i++)
	{
		// ********************************
		// Calculate direction to the light
		// ********************************
		float3 lightDir = normalize(position[i] - input.Position);

		// ***************************
		// Calculate distance to light
		// ***************************
		float d = distance(position[i], input.Position);

		// ***************************
		// Calculate attenuation value
		// ***************************
		float attenuation = constantK[i] + linearK[i] * d + quadraticK[i] * d * d;

		// **********************
		// Calculate light colour
		// **********************
		float4 lightColour = (1 / attenuation) * colour[i];

		// ******************************************************************************
		// Now use standard phong shading but using calculated light colour and direction
		// - note no ambient
		// ******************************************************************************
		diffuse += (diffuse_reflection * lightColour) * max(dot(normal, lightDir), 0.0);


		float3 viewDir = { 0, 0, -1 };
		float3 halfVector = normalize(lightDir + viewDir);//JUST THIS view dir
		specular += (specular_reflection * lightColour) * pow(max(dot(normal, halfVector), 0.0), shininess);
	}

	float4 colour = ((emissive + diffuse) * texColour) + specular;
	colour.a = texColour.a;

	return colour;
}

technique SpriteDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVs();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};