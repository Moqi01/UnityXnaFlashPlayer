Shader "Unlit/VectorShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _ProjectionT("ProjectionT",Vector) = (0,0,0,0)
        _ProjectionR("ProjectionR",Vector) = (0,0,0,0)
        _ProjectionS("ProjectionS",Vector) = (0,0,0,0)
		_PaintTransformationT("PaintTransformationT",Vector) = (0,0,0,0)
		_PaintTransformationR("PaintTransformationR",Vector) = (0,0,0,0)
		_PaintTransformationS("PaintTransformationS",Vector) = (0,0,0,0)
        _Color("Color",Color) = (1,1,1,1)
		_Transformation("Transformation",Vector) = (0,0,0,0)
        _Rotation("Rotation",Vector) = (0,0,0,0)
        _Scale("Scale",Vector) = (1,1,1,1)
        _AddTerm("AddTerm",Vector) = (0,0,0,0)
        _MulTerm("MulTerm",Vector) = (1,1,1,1)
		_FocalPoint("FocalPoint",Vector)=(0,0,0,0)
        _Offset("Offset",Vector) = (0,0,0,0)
        _IsTex("_IsTex",float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		Blend One OneMinusSrcAlpha
		ZClip Off
		ZWrite Off
		Cull Off
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uniform float4 _Color;
            uniform float4 _ProjectionT;
            uniform float4 _ProjectionR;
            uniform float4 _ProjectionS;

			uniform float4 _PaintTransformationT;
            uniform float4 _PaintTransformationR;
            uniform float4 _PaintTransformationS;

            uniform float4 _Transformation;
            uniform float4 _Rotation;
            uniform float4 _Scale;
            uniform float4 _AddTerm;
            uniform float4 _MulTerm;
            uniform float4 _Offset;
            uniform float4 _FocalPoint;
            uniform float _IsTex;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			 float4x4 Translational(float4 translational,float4 rotation,float4 scale)
            {
                return float4x4(scale.x,    rotation.x, 0.0,     translational.x,
                                rotation.y, scale.y,    0.0,     translational.y,
                                0.0,        0.0,        scale.z, translational.z,
                                0.0,        0.0,        0.0,     0.0
                    );
            }
			
			v2f vert (appdata v)
			{
				v2f o;
				float4 tcTransform=float4(1, 1, 0.5, 0.5);
				v.vertex.xyz = float3(v.vertex.xy + _Offset.xy, 1);

				v.uv = mul( Translational(_PaintTransformationT,_PaintTransformationR,_PaintTransformationS),v.vertex).xy;
				v.uv = (v.uv.xy + tcTransform.xy) * tcTransform.zw - _FocalPoint.xy;
				//v.vertex = float4(mul(mul(v.Position.xyz, Transformation), Projection), 1);
				v.vertex =mul(Translational( _Transformation,_Rotation,_Scale),v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			float4 CxForm(float4 color)
            {
	            return clamp(color * _MulTerm + _AddTerm, 0, 1);
            }

			float4 Premultiply(float4 color)
            {
	            color.xyz *= color.w;
	            return color;
            }
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				
				return Premultiply(CxForm(col));
			}
			ENDCG
		}
	}
}
