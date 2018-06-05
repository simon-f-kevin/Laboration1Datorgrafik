#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


matrix World;
matrix View;
matrix Projection;

float4 AmbientColor;
float AmbientIntensity;

float3 DiffuseLightDirection;
float4 DiffuseColor;
float DiffuseIntensity;

float Shininess;
float4 SpecularColor;
float SpecularIntensity;

float3 ViewVector = float3(1, 0, 0);

///FOG
float FogStart = 40;
float FogEnd = 100;
float4 FogColor = float4(1, 0, 0, 1);
bool FogEnabled = 1;
float3 CameraPosition;

bool TextureEnabled = 1;
texture Texture;

sampler2D textureSampler = sampler_state
{
	Texture = (Texture);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
	AddressW = Clamp;
};


float3 LightDirection;
float4x4 LightViewProj;
float DepthBias;
float ShadowStrenght;

texture ShadowMap;
sampler ShadowMapSampler = sampler_state
{
	Texture = <ShadowMap>;
};

struct DrawWithShadowMap_VSIn
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float2 TexCoord : TEXCOORD0;
};

struct DrawWithShadowMap_VSOut
{
	float4 Position : POSITION0;
	float3 Normal : TEXCOORD0;
	float2 TexCoord : TEXCOORD1;
	float4 WorldPos : TEXCOORD2;

	float4 Color : COLOR0;
	float fogFactor : FOG;
};

struct CreateShadowMap_VSOut
{
	float4 Position : POSITION;
	float Depth : TEXCOORD0;
};

CreateShadowMap_VSOut CreateShadowMap_VertexShader(float4 Position : POSITION)
{
	CreateShadowMap_VSOut Out;
	Out.Position = mul(Position, mul(World, LightViewProj));
	Out.Depth = Out.Position.z / Out.Position.w;
	return Out;
}


float4 CreateShadowMap_PixelShader(CreateShadowMap_VSOut input) : COLOR
{
	return float4(input.Depth, 0, 0, 0);
}

float ComputeFogFactor(float d)
{
	//d is the distance to the geometry sampling from the camera
	//this simply returns a value that interpolates from 0 to 1 
	//with 0 starting at FogStart and 1 at FogEnd 
	return clamp((d - FogStart) / (FogEnd - FogStart), 0, 1) * FogEnabled;
}

DrawWithShadowMap_VSOut DrawWithShadowMap_VertexShader(DrawWithShadowMap_VSIn input)
{
	DrawWithShadowMap_VSOut Output;

	float4x4 WorldViewProj = mul(mul(World, View), Projection);

	Output.Position = mul(input.Position, WorldViewProj);
	Output.Normal = (float3) normalize(mul(input.Normal, (float3x4) World));
	Output.TexCoord = input.TexCoord;

	float4 worldPosition = mul(input.Position, World);
	Output.WorldPos = worldPosition;

	float distance;

	float lightIntensity = dot(Output.Normal.xyz, DiffuseLightDirection);

	Output.Color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);

	distance = length(worldPosition.xyz - CameraPosition);
	Output.fogFactor = saturate(ComputeFogFactor(distance));

	return Output;
}


float4 DrawWithShadowMap_PixelShader(DrawWithShadowMap_VSOut input) : COLOR
{
	float3 normal = normalize(input.Normal);
	float4 returnColor = { 1, 1, 1, 1 };

	float nl = max(0, dot(normalize(DiffuseLightDirection), normal));

	float4 diffuseColor = tex2D(textureSampler, input.TexCoord);

	float diffuseIntensity = saturate(dot(LightDirection, input.Normal));
	float4 diffuse = diffuseIntensity * diffuseColor * nl + AmbientColor * AmbientIntensity;

	float4 textureColor = tex2D(textureSampler, input.TexCoord);
	if (TextureEnabled)
	{
		diffuse = diffuse * textureColor;
	}

	float3 light = normalize(LightDirection);
	float3 r = normalize(2 * dot(light, normal) * normal - light);
	float3 v = normalize(ViewVector);

	diffuse + SpecularIntensity * SpecularColor * max(pow(abs(dot(r, v)), Shininess), 0);

	float4 lightingPosition = mul(input.WorldPos, LightViewProj);

	float2 ShadowTexCoord = 0.5 * lightingPosition.xy /
		lightingPosition.w + float2(0.5, 0.5);
	ShadowTexCoord.y = 1.0f - ShadowTexCoord.y;

	float shadowdepth = (float)tex2D(ShadowMapSampler, ShadowTexCoord).r;

	float ourdepth = (lightingPosition.z / lightingPosition.w) - DepthBias;


	if (shadowdepth < ourdepth)
	{
		// Shadow the pixel by lowering the intensity
		ShadowStrenght = 1 - ShadowStrenght;
		diffuse *= float4(ShadowStrenght, ShadowStrenght, ShadowStrenght, 0);
	};

	returnColor = saturate(returnColor * diffuse);

	returnColor.a = 1;

	if (FogEnabled)
	{
		return lerp(returnColor, FogColor, input.fogFactor);
	}
	return returnColor;
}

// Technique for creating the shadow map
technique CreateShadowMap
{
	pass Pass1
	{
		VertexShader = compile VS_SHADERMODEL CreateShadowMap_VertexShader();
		PixelShader = compile PS_SHADERMODEL CreateShadowMap_PixelShader();
	}
}

// Technique for drawing with the shadow map
technique DrawWithShadowMap
{
	pass Pass1
	{
		VertexShader = compile VS_SHADERMODEL DrawWithShadowMap_VertexShader();
		PixelShader = compile PS_SHADERMODEL DrawWithShadowMap_PixelShader();
	}
}