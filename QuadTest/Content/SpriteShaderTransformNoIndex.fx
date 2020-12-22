#include "Macros.fxh"

#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0


BEGIN_CONSTANTS
MATRIX_CONSTANTS
float Z;
float4x4 MatrixTransform    _vs(c0) _cb(c0);
float4 TextureDim;
END_CONSTANTS


DECLARE_TEXTURE(Texture, 0);


struct VertexShaderInput
{
	float2 UV0 : POSITION;
	float2 UV1 : TANGENT;
	float2 Transform0 : BINORMAL0;
	float2 Transform1 : BINORMAL1;
	float2 Transform2 : BINORMAL2;
	float4 Color	: COLOR0;
	float2 Corner : POSITION3;
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
	float2 corner = input.Corner;
	float3x2 Transform = float3x2(input.Transform0.x, input.Transform0.y,
		input.Transform1.x, input.Transform1.y,
		input.Transform2.x, input.Transform2.y);
	float2 dUV = (input.UV1 - input.UV0) * 0.5 * corner;
	float2 UV = (input.UV1 + input.UV0) * 0.5;
	float2 pos = corner *0.5;
	output.Position = mul(float4(mul(float3(pos.x, pos.y, 1), Transform), Z, 1), MatrixTransform);
	output.UV = (UV + dUV) * TextureDim.zw;
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