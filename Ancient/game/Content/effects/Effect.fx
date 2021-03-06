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
float4x4 Projection;

bool MultiplyColorEnabled;
float4 MultiplyColor;

bool FogEnabled;
float4 FogColor;
float FogStart;
float FogEnd;

float WindStrength;
float3 WindDirection;
float time;

float4x4 LightViewProjection;
float4x4 LightWorld;

bool EntityShadowMode;

Texture ShadowMapTexture;

sampler ShadowMapSampler = sampler_state
{ 
	Texture = <ShadowMapTexture>;
	MagFilter = POINT;
	MinFilter = POINT;
	MipFilter = POINT;
	AddressU = CLAMP;
	AddressV = CLAMP;
};

bool ShadowsEnabled;

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float4 Color : COLOR0;
	float4 Normal : NORMAL0;
	float4 Tech : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float4 Color : COLOR0;
	float4 Normal : NORMAL0;
	float4 Tech : TEXCOORD0;
	float Fog : FOG0;
	float4 Position2D : TEXCOORD1;
	float4 LightPosition : TEXCOORD2;
};

float4 ChangeSaturation(float4 color, float change) 
{
	float r = color.r;
	float g = color.g;
	float b = color.b;

	float p = sqrt(r * r * 0.299 + g * g *0.587 + b * b * 0.114);

	r = p + (r - p) * change;
	g = p + (g - p) * change;
	b = p + (b - p) * change; 
	
	return float4(r, g, b, color.a);
}

float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
    float3 lightDir = normalize(pos3D - lightPos);
    return dot(-lightDir, normal);    
}

float4 GetColorAffectedByLight(float4 LightPosition, float4 Color)
{
	float2 TexCoords;
    TexCoords[0] = (LightPosition.x / LightPosition.w) / 2.0 + 0.5f;
    TexCoords[1] = (-LightPosition.y / LightPosition.w) / 2.0 + 0.5f;

	float depthStoredInShadowMap = tex2D(ShadowMapSampler, TexCoords).r;
	float light = depthStoredInShadowMap;
	
	if ((saturate(TexCoords).x == TexCoords.x) && (saturate(TexCoords).y == TexCoords.y))
    {	
		float realDistance = LightPosition.z / LightPosition.w;
	
		if(realDistance - 1.0f / 256.0f <= depthStoredInShadowMap)
			light = 1;
	}
	
	Color.rgb = Color.rgb * saturate(light);
	
	return Color;
}
		
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
	
	float4 position = input.Position;
    float4 worldPosition = mul(position, World);
	float4 tech = input.Tech;
	
	if(tech.w == 1)
		worldPosition.y += sin(time * 0.7F + (worldPosition.x + worldPosition.y + worldPosition.z) * 0.17f) * 0.7F * WindStrength;
	else if(tech.w == 2)
		worldPosition.xyz += sin(time + (position.x + position.y + position.z) * 0.17f) * length(position - tech.xyz) * WindDirection * WindStrength;
	
	float4 lightPosition = mul(input.Position, LightWorld);
	
	if(tech.w == 1)
		lightPosition.y += sin(time * 0.7F + (lightPosition.x + lightPosition.y + lightPosition.z) * 0.17f) * 0.7F * WindStrength;
	else if(tech.w == 2)
		lightPosition.xyz += sin(time + (position.x + position.y + position.z) * 0.17f) * length(position - tech.xyz) * WindDirection * WindStrength;
	
	output.LightPosition = mul(lightPosition, LightViewProjection);
	
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.Color = input.Color;
	output.Normal =  input.Normal;
	output.Tech = tech;
	output.Fog = clamp((length(output.Position) - FogStart) / (FogEnd - FogStart), 0, 1);
	output.Position2D = output.Position;
	
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{	
	float4 Color = input.Color;
	
	Color.rgb = (Color.rgb - 0.5) * (1 + 0.27) + 0.5;
	
	if(ShadowsEnabled)
		Color = GetColorAffectedByLight(input.LightPosition, Color);
	
	if(MultiplyColorEnabled)
		Color *= MultiplyColor;
		
	if(FogEnabled)
		Color = lerp(Color, FogColor, input.Fog);
		
	float x = input.Position2D.x / input.Position2D.w;
	float y = input.Position2D.y / input.Position2D.w;
	float num = x * x + y * y;
	
	Color.xyz *= saturate(1 - num * 0.37);
	
	return Color;
}

float4 OutlinePixelShader(VertexShaderOutput input) : COLOR0
{
    return lerp(float4(0, 0, 0, 1), FogColor, input.Fog);
}

technique RegularTechnique
{	
    pass DrawPass
	{	
		AlphaBlendEnable = true;
		DestBlend = InvSrcAlpha;
		SrcBlend = SrcAlpha;
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
		CullMode = CCW;
	}	
}

technique OutlineTechnique
{	
	pass OutlinePass
	{
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
        PixelShader = compile PS_SHADERMODEL OutlinePixelShader();
		CullMode = CW;
	}
	
    pass DrawPass
	{	
		AlphaBlendEnable = true;
		DestBlend = InvSrcAlpha;
		SrcBlend = SrcAlpha;
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
		CullMode = CCW;
	}	
}

// ......... //
// ShadowMap //
// ......... //
struct ShadowMapVertexShaderOutput
{
	float4 Position : POSITION0;
	float Depth : TEXCOORD0;
};

ShadowMapVertexShaderOutput ShadowMapVertexShaderFunction(float4 Position : POSITION0)
{
	ShadowMapVertexShaderOutput output = (ShadowMapVertexShaderOutput)0;
	
	if(EntityShadowMode)
		LightWorld = World;
		
	output.Position = mul(mul(Position, LightWorld), LightViewProjection);
	output.Depth.x = output.Position.z / output.Position.w;

	return output;
}

float4 ShadowMapPixelShaderFunction(ShadowMapVertexShaderOutput input) : COLOR0
{
	return float4(input.Depth.x, 0, 0, 1);
}

technique ShadowMap
{
	pass Pass0
	{
		VertexShader = compile VS_SHADERMODEL ShadowMapVertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL ShadowMapPixelShaderFunction();
		CullMode = CCW;
	}
};