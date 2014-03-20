matrix gWVP;

struct VOut
{
    float4 position : SV_POSITION;
    float4 color : COLOR;
};

VOut VShader(float4 position : POSITION, float4 color : COLOR)
{
    VOut output;

    output.position = mul( position, gWVP);
    output.color = color;

    return output;
}

float4 PShader(float4 position : SV_POSITION, float4 color : COLOR) : SV_TARGET
{
    return color;
}

RasterizerState WireframeState
{
    FillMode = Wireframe;
    CullMode = None;
    FrontCounterClockwise = false;
};

technique10 Render
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_4_0, VShader() ));
		SetGeometryShader( NULL );
		SetPixelShader( CompileShader( ps_4_0, PShader() ));
		//SetRasterizerState(WireframeState);
	}
}