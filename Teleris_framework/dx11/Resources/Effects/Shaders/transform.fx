cbuffer ConstBuffer : register(c0)
{
	float4x4 gWVP;
}

float4 VShader(float4 position : POSITION) : SV_POSITION
{
	return mul( position, gWVP);
}

float4 PShader(float4 position : SV_POSITION) : SV_Target
{
	return float4(0.0f, 0.5f, 0.0f, 1.0f);
}

