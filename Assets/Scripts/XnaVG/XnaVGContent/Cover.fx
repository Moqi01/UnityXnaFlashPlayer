#include "Cover_Paint.fx"
#include "Cover_Transform.fx"

#define T(tname, name, xform, xformm) \
	technique Fill_##tname \
	{ \
		pass Normal \
		{ \
			VertexShader = compile vs_2_0 Transform_##xform(); \
			PixelShader = compile ps_2_0 name##Fill(); \
		} \
		pass Linear \
		{ \
			VertexShader = compile vs_2_0 Transform_##xform(); \
			PixelShader = compile ps_2_0 name##Fill_L(); \
		} \
		pass Masked \
		{ \
			VertexShader = compile vs_2_0 Transform_##xformm(); \
			PixelShader = compile ps_2_0 name##Fill_M(); \
		} \
		pass MaskedLinear \
		{ \
			VertexShader = compile vs_2_0 Transform_##xformm(); \
			PixelShader = compile ps_2_0 name##Fill_LM(); \
		} \
	}

T(Color, Solid, NoTC, ZO);
T(PatternPremultiplied, Texture, ZO, ZO);
T(Pattern, NPTexture, ZO, ZO);
T(LinearGradient, Linear, ZO, ZO);
T(RadialGradient, Radial, MOO, MOO);