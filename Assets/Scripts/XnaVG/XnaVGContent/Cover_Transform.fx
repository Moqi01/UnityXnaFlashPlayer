float3x3	Transformation;
float3x3	Projection;
float3x3	PaintTransformation;
float2		FocalPoint;
float2		Offset; // User space offset

struct VSIn
{
    float2 Position : POSITION0;
};

struct VSOut
{
    float4 Position : POSITION0;
	float4 Texcoord : TEXCOORD0;
};

VSOut Transform(VSIn i, float4 tcTransform)
{
    VSOut o;

	// Make float3 from coordinates and offset them
	o.Position.xyz = float3(i.Position + Offset, 1);

	// Transform path coordinates to gradient square [(-1, -1) to (1, 1)]
	o.Texcoord.xy = mul(o.Position.xyz, PaintTransformation).xy;

	// Transform gradient square to specific texture coordinates
	o.Texcoord.xy = (o.Texcoord.xy + tcTransform.xy) * tcTransform.zw - FocalPoint;
	
	// Transform path coordinates to screen space
	o.Position = float4(mul(mul(o.Position.xyz, Transformation), Projection), 1);

	// Screen space to mask texture space
	o.Texcoord.zw = o.Position.xy * float2(0.5, -0.5) + float2(0.5, 0.5);
    
    return o;
}

VSOut Transform_ZO(VSIn i)
{
	return Transform(i, float4(1, 1, 0.5, 0.5));
}

VSOut Transform_MOO(VSIn i)
{
	return Transform(i, float4(0, 0, 1, 1));
}

float4 Transform_NoTC(VSIn i) : POSITION0
{
	return Transform_MOO(i).Position;
}