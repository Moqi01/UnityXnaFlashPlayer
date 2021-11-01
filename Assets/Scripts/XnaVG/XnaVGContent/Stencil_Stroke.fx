float3x3 Transformation;
float3x3 Projection;
float2   Offset;
float	 Thickness;

struct VSIn
{
    float4 Position : POSITION0;
	float2 Texcoord : TEXCOORD0;
};

struct VSOut
{
    float4 Position : POSITION0;
	float3 Texcoord : TEXCOORD0;
};

float4 TransformVertex_Scaling(float4 Position : POSITION0) : POSITION0
{
	float3 position  = float3(Position.xy, 1); // 2D base vertex position on canvas
	float3 offsetDir = float3(Position.zw, 0); // 2D vertex offset direction from the base position
		
	position = position + Thickness * offsetDir;
	position = mul(position, Transformation);

	return float4(mul(position, Projection), 1.0) + float4(Offset, 0, 0);
}

float4 TransformVertex_Nonscaling(float4 Position : POSITION0) : POSITION0
{	
	float3 position  = float3(Position.xy, 1); // 2D base vertex position on canvas
	float3 offsetDir = float3(Position.wz, 0); // 2D vertex offset direction from the base position

	offsetDir = normalize(mul(offsetDir * float3(-1, 1, 0), Transformation));
	position = mul(position, Transformation);
	position = position + Thickness * offsetDir.yxz * float3(1, -1, 0);

	return float4(mul(position, Projection), 1.0) + float4(Offset, 0, 0);
}

VSOut TransformRadialVertex_Scaling(VSIn i)
{
	VSOut o;
	o.Texcoord = float3(i.Texcoord, 0);
	o.Position = TransformVertex_Scaling(i.Position);
	return o;
}

VSOut TransformRadialVertex_Nonscaling(VSIn i)
{
	VSOut o;
	float3 position  = float3(i.Position.xy, 1); // 2D base vertex position on canvas
	float3 offsetDir = float3(i.Position.zw, 0); // 2D vertex offset direction from the base position
		
	position = mul(position, Transformation);
	position = position + Thickness * offsetDir;

	o.Texcoord = float3(i.Texcoord, 0);
	o.Position = float4(mul(position, Projection), 1.0) + float4(Offset, 0, 0);
	return o;
}

float4 StencilSolid() : COLOR0
{   
	return float4(1, 1, 1, 1);	
}

float4 StencilRadial(VSOut i) : COLOR0
{   
	clip(1.0 - length(i.Texcoord.xy));
	return float4(1, 1, 1, 1);
}

technique StencilStroke_Scaling
{
    pass Solid
    {
        VertexShader = compile vs_2_0 TransformVertex_Scaling();
        PixelShader = compile ps_2_0 StencilSolid();
    }

	pass Radial
	{
		VertexShader = compile vs_2_0 TransformRadialVertex_Scaling();
        PixelShader = compile ps_2_0 StencilRadial();
	}
}

technique StencilStroke_Nonscaling
{
    pass Solid
    {
        VertexShader = compile vs_2_0 TransformVertex_Nonscaling();
        PixelShader = compile ps_2_0 StencilSolid();
    }

	pass Radial
	{
		VertexShader = compile vs_2_0 TransformRadialVertex_Nonscaling();
        PixelShader = compile ps_2_0 StencilRadial();
	}
}