float4		Color;
float4		AddTerm;
float4		MulTerm;
float4		MaskChannels;
sampler2D	Mask : register(s0);
sampler2D	Texture : register(s1);

// Utilities
float4 CxForm(float4 color)
{
	return clamp(color * MulTerm + AddTerm, 0, 1);
}

float4 FromLinear(float4 color) : COLOR0
{
	// http://chilliant.blogspot.cz/2012/08/srgb-approximations-for-hlsl.html
	return max(1.055 * pow(color, 0.416666667) - 0.055, 0);
}

float4 Premultiply(float4 color)
{
	color.xyz *= color.w;
	return color;
}

float4 MaskPixel(float4 coords, float4 color)
{
	float alpha = clamp(dot(tex2D(Mask, coords.zw), MaskChannels), 0, 1);
	return color * alpha;
}

// Normal fill
float4 SolidFill() : COLOR0
{
	return Premultiply(CxForm(Color));
}

float4 TextureFill(float4 coords : TEXCOORD0) : COLOR0
{
	return CxForm(tex2D(Texture, coords.xy));
}

float4 NPTextureFill(float4 coords : TEXCOORD0) : COLOR0
{
	return Premultiply(CxForm(tex2D(Texture, coords.xy)));
}

float4 LinearFill(float4 coords : TEXCOORD0) : COLOR0
{
	return TextureFill(coords);
}

float4 RadialFill(float4 coords : TEXCOORD0) : COLOR0
{
	return CxForm(tex2D(Texture, float2(length(coords.xy), 0.5)));
}


// Variants
#define V(name) \
	float4 name##Fill_L(float4 c : TEXCOORD0) : COLOR0 { return FromLinear(name##Fill(c)); } \
	float4 name##Fill_M(float4 c : TEXCOORD0) : COLOR0 { return MaskPixel(c, name##Fill(c)); } \
	float4 name##Fill_LM(float4 c : TEXCOORD0) : COLOR0 { return MaskPixel(c, FromLinear(name##Fill(c))); }

V(Texture);
V(NPTexture);
V(Linear);
V(Radial);

// Solid
float4 SolidFill_L() : COLOR0 { return FromLinear(SolidFill()); } \
float4 SolidFill_M(float4 c : TEXCOORD0) : COLOR0 { return MaskPixel(c, SolidFill()); } \
float4 SolidFill_LM(float4 c : TEXCOORD0) : COLOR0 { return MaskPixel(c, FromLinear(SolidFill())); }