float3x3 Transformation;
float3x3 Projection;
float2	 Offset; // Screen space offset

// Font Vertices
float2	 VertexCount;
texture  FontVertices;
sampler  VerticesSampler = sampler_state
{
	texture = <FontVertices>; 
	mipfilter = POINT;
	minfilter = POINT;
	magfilter = POINT;
};

// Glyph offsets
float2	 GlyphCount;
texture  GlyphOffsets;
sampler  OffsetSampler = sampler_state
{
	texture = <GlyphOffsets>; 
	mipfilter = POINT;
	minfilter = POINT;
	magfilter = POINT;
};

struct VSIn
{
    float2 Instance : POSITION0;
};

struct VSOut
{
    float4 Position : POSITION0;
	float2 Texcoord : TEXCOORD0;
};

float4 ArrayLookup(sampler data, float index, float2 size)
{
	return tex2Dlod(data, float4((float2(fmod(index, size.x), trunc(index / size.x)) + 0.5) / size, 0, 0));
}

VSOut TransformVertex(VSIn i)
{
    VSOut o;
	
	float4 vertex = ArrayLookup(VerticesSampler, i.Instance.x, VertexCount);
	float4 glyph = ArrayLookup(OffsetSampler, i.Instance.y, GlyphCount);

	o.Texcoord = vertex.zw;
	o.Position = float4(mul(mul(float3(vertex.xy + glyph, 1), Transformation), Projection), 1);
	o.Position.xy += Offset;
    
    return o;
}

float4 StencilCurve(VSOut i) : COLOR0
{   	
	clip(i.Texcoord.x * (1 - i.Texcoord.x) - i.Texcoord.y);
	return float4(1, 1, 1, 1);
}

technique StencilText
{
    pass Normal
    {
        VertexShader = compile vs_3_0 TransformVertex();
        PixelShader = compile ps_3_0 StencilCurve();
    }
}
