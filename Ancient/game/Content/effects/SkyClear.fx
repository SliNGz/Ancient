#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World;
float4x4 View;
float4x4 PlayerView;
float4x4 Projection;

float4 Color1;
float4 Color2;

float Time;

Texture SkyTexture;
sampler SkyTextureSampler = sampler_state 
{
	Texture = <SkyTexture>;
	MinFilter = LINEAR;
	MagFilter = LINEAR;
	MipFilter  = LINEAR;
	AddressU = CLAMP;
	AddressV = CLAMP;
};

Texture NoiseTexture;
sampler NoiseTextureSampler = sampler_state 
{
	Texture = <NoiseTexture>;
	MinFilter = POINT;
	MagFilter = POINT;
	MipFilter  = POINT;
	AddressU = MIRROR;
	AddressV = MIRROR;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 TextureCoords : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(float4 Position : POSITION0)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 worldPosition = mul(Position, World);
	float4 viewPosition = mul(worldPosition, View);
//	viewPosition.xyz += sin(Time) * 0.015;
	output.Position = mul(viewPosition, Projection);
	output.TextureCoords = mul(mul(mul(Position, World), PlayerView), Projection);

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float2 ProjectedTexCoords;
    ProjectedTexCoords[0] = (input.TextureCoords.x / input.TextureCoords.w) / 2.0 + 0.5f;
    ProjectedTexCoords[1] = (-input.TextureCoords.y / input.TextureCoords.w) / 2.0 + 0.5f;
	
	float4 color = lerp(Color1, Color2, sin(Time));
	
	return saturate(color * (1.25 + sin(Time)) * tex2D(NoiseTextureSampler, ProjectedTexCoords));
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
};