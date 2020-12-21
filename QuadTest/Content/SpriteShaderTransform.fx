#include "Macros.fxh"

#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0

static float2 corners[4] = { float2(-1,1) , float2(1,1) ,
					float2(-1,-1) , float2(1,-1) };

BEGIN_CONSTANTS
MATRIX_CONSTANTS
float4x4 MatrixTransform    _vs(c0) _cb(c0);
float4 TextureDim;
END_CONSTANTS


DECLARE_TEXTURE(Texture, 0);


struct VertexShaderInput
{
	float3 Pos : POSITION0;
	float2 Dim : POSITION1;
	float2 UV : TEXCOORD0;
	float3x2 Transform : TEXCOORD1;
	float4 Color	: COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 UV : TEXCOORD0;
};

VertexShaderOutput MainVS(in VertexShaderInput input, uint id:SV_VERTEXID)
{
	VertexShaderOutput output = (VertexShaderOutput)0;
	float2 corner = corners[id % 4];
	float3x2 Transform = input.Transform;
	float2 pos = input.Pos.xy + mul(float3((input.Dim * corner),1.0), Transform);
	output.Position = mul(float4(pos.x, pos.y, input.Pos.z, 1),MatrixTransform);
	output.UV = (input.UV + (input.Dim * corner)) * TextureDim.zw;
	output.Color = input.Color;
	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float2 uv = input.UV;
	float4 texel = SAMPLE_TEXTURE(Texture, uv);
	return texel;
}

technique AnimationRendering
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};