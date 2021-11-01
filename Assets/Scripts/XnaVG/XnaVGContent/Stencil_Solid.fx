float3x3 Transformation;
float3x3 Projection;

struct VSIn
{
    float2 Position : POSITION0;
};

struct VSOut
{
    float4 Position : POSITION0;
};

VSOut TransformVertex(VSIn i)
{
    VSOut o;
	o.Position = float4(mul(mul(float3(i.Position, 1), Transformation), Projection), 1);   
    return o;
}

float4 StencilSolid(VSOut i) : COLOR0
{   	
	return float4(1, 1, 1, 1);
}

technique StencilFill
{
    pass Normal
    {
        VertexShader = compile vs_2_0 TransformVertex();
        PixelShader = compile ps_2_0 StencilSolid();
    }
}
