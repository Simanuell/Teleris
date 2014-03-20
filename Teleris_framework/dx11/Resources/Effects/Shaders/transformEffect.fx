cbuffer Transforms
{
	float4x4 worldViewProj;
	float4x4 worldView;
	float4x4 world;
}


struct Out
{
    float4 position : SV_POSITION;
    float4 color;
};

float4 VShader(float4 position : POSITION) : SV_POSITION
{
	Out out;
	out.position =  mul(position, worldViewProj);
	out.color = position;
}






float4 PShader(float4 position : SV_POSITION) : SV_Target
{
	//return position;
	return float4(0.0f, 0.5f, 0.0f, 1.0f);
}


RasterizerState WireframeState
{
    FillMode = Wireframe;
    //CullMode = Front;
    //FrontCounterClockwise = true;
};


/*
technique10 Render
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_4_0, VShader() ));
		SetGeometryShader( NULL );
		SetPixelShader( CompileShader( ps_4_0, PShader() ));
		SetRasterizerState(WireframeState);
	}
}
*/