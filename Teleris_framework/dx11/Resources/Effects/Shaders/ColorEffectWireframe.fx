matrix gWVP;
float4 wireFrameColor;

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

float4 PShaderWireframe(float4 position : SV_POSITION, float4 color : COLOR) : SV_TARGET
{
    return wireFrameColor;
}

RasterizerState WireframeState
{
    FillMode = Wireframe;
	SlopeScaledDepthBias = -0.5f;
};

RasterizerState SolidState
{
	FillMode = Solid;
};

technique10 Render
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_4_0, VShader() ));
		SetGeometryShader( NULL );
		SetPixelShader( CompileShader( ps_4_0, PShader() ));
		SetRasterizerState(SolidState);
	}

	pass P1
	{
		SetVertexShader( CompileShader( vs_4_0, VShader() ));
		SetGeometryShader( NULL );
		SetPixelShader( CompileShader( ps_4_0, PShaderWireframe() ));
		SetRasterizerState(WireframeState);
	}
}