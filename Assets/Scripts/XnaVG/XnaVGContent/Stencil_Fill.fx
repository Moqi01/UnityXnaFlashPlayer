float3x3 Transformation;
float3x3 Projection;
float4	 Offset; // XY = User space offset, ZW = Screen space offset

struct VSIn
{
    float4 Position : POSITION0;
};

struct VSOut
{
    float4 Position : POSITION0;
	float2 Texcoord : TEXCOORD0;
};

VSOut TransformVertex(VSIn i)
{
    VSOut o;

	o.Texcoord     = i.Position.zw;
	o.Position.xyz = float3(i.Position.xy + Offset.xy, 1);
	o.Position.xyz = mul(o.Position.xyz, Transformation);
	o.Position.xyz = mul(o.Position.xyz, Projection);	
	o.Position.xy += Offset.zw;
    o.Position.w   = 1;

    return o;
}

float4 StencilCurve(VSOut i) : COLOR0
{   	
	clip(i.Texcoord.x * (1 - i.Texcoord.x) - i.Texcoord.y);
	return float4(1, 1, 1, 1);
}

technique StencilFill
{
    pass Normal
    {
        VertexShader = compile vs_2_0 TransformVertex();
        PixelShader = compile ps_2_0 StencilCurve();
    }
}
