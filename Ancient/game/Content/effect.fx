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

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float4 Color : COLOR0;
	float Fog : FOG;
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
		
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
	
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.Color = input.Color;
	output.Fog = clamp((length(output.Position) - FogStart) / (FogEnd - FogStart), 0, 1);
	
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{	
	float4 color = input.Color;
	
	if(MultiplyColorEnabled)
		color *= MultiplyColor;
		
	if(FogEnabled)
		color = lerp(color, FogColor, input.Fog);
	
	return color;
}

technique RegularTechnique
{	
    pass AlphaBlend
	{	
		AlphaBlendEnable = true;
		DestBlend = InvSrcAlpha;
		SrcBlend = SrcAlpha;
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}