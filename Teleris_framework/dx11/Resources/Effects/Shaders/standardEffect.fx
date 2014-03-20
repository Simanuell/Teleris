cbuffer Transforms
{
	float4x4 worldViewProj;
	float4x4 worldView;
	float4x4 world;
}

float4 VShader(float4 position : POSITION) : SV_POSITION
{
	return mul(float4(position,1.0f), worldViewProj);
}

float4 PShader(float4 position : SV_POSITION) : SV_Target
{
	return float4(0.0f, 0.5f, 0.0f, 1.0f);
}