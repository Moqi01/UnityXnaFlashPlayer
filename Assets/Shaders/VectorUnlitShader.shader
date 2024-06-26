Shader "Unlit/VectorUnlitShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
		_Mask("MaskTexture", 2D) = "white" {}
        _Transformation("Transformation",Vector) = (0,0,0,0)
        _ProjectionT("ProjectionT",Vector) = (0,0,0,0)
        _ProjectionR("ProjectionR",Vector) = (0,0,0,0)
        _ProjectionS("ProjectionS",Vector) = (0,0,0,0)
        _Color("Color",Color) = (1,1,1,1)
        _Scale("Scale",Vector) = (1,1,1,1)
        _Rotation("Rotation",Vector) = (0,0,0,0)
        _AddTerm("AddTerm",Vector) = (0,0,0,0)
        _MulTerm("MulTerm",Vector) = (1,1,1,1)
        _Offset("Offset",Vector) = (0,0,0,0)
        _MaskChannels("MaskChannels",Vector) = (0,0,0,0)
        _IsTex("_IsTex",float) = 0
        _Z("_Z",float) = 1

		//_p("_p",float3x3) =(1,1,1,1,1,1,1,1,1,1,1,1)
		//[Enum(UnityEngine.Rendering.BlendOp  )] _BlendOp  ("BlendOp" , Int) = 0
		//[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("SrcBlend", Int) = 1
		//[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("DstBlend", Int) = 10
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "AlphaTest"}
            //Tags { "RenderType" = "Opaque" }
            //Tags { "Queue" = "AlphaTest" "IgnoreProjector" = "True"  }
            //Tags { "QUEUE" = "AlphaTest" "IGNOREPROJECTOR" = "true" "RenderType" = "TransparentCutout" }
            LOD 100
		/*	Tags {
			"Queue"             = "Transparent"
			"IgnoreProjector"   = "True"
			"RenderType"        = "Transparent"
			"PreviewType"       = "Plane"
			"CanUseSpriteAtlas" = "True"
		}*/

		//Cull     Off
		//Lighting Off
		//ZWrite   Off

		//BlendOp [_BlendOp]
		//Blend [_SrcBlend] [_DstBlend]
            Pass
            {
                Tags { "RenderType" = "Transparent" "Queue" = "AlphaTest"}
                //Tags { "QUEUE" = "AlphaTest" "IGNOREPROJECTOR" = "true" "RenderType" = "TransparentCutout" }
                //Blend SrcAlpha OneMinusSrcAlpha
				//Blend OneMinusSrcAlpha One
				Blend One OneMinusSrcAlpha
			    ZClip Off
			    ZWrite Off
			    Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_instancing //这里,第一步
            #include "UnityCG.cginc"
            uniform float4 _Color;
            uniform float4 _ProjectionT;
            uniform float4 _ProjectionR;
            uniform float4 _ProjectionS;

            uniform float4 _Transformation;
            uniform float4 _Rotation;
            uniform float4 _Scale;
            uniform float4 _AddTerm;
            uniform float4 _MulTerm;
            uniform float4 _Offset;
            uniform float4 _MaskChannels;
            uniform float _IsTex;
            uniform float _Z;
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
               
			   UNITY_VERTEX_INPUT_INSTANCE_ID //这里,第二步
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID //这里,第二步
            };

            float4x4 Translational(float4 translational,float4 rotation,float4 scale)
            {
                return float4x4(scale.x,    rotation.x, 0.0,     translational.x,
                                rotation.y, scale.y,    0.0,     translational.y,
                                0.0,        0.0,        scale.z, translational.z,
                                0.0,        0.0,        0.0,     0.0
                    );
            }

            sampler2D _MainTex;
            float4 _MainTex_ST;
			sampler2D _Mask;
            float4 _Mask_ST;

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v); //这里第三步
                UNITY_TRANSFER_INSTANCE_ID(v, o); //第三步 
                v.vertex.z = _Z;
                v.vertex.xy = v.vertex.xy + _Offset.xy;
                v.vertex = mul(Translational(_Transformation,_Rotation,_Scale),v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
				//v.vertex = mul(Translational(_ProjectionT,_ProjectionR,_ProjectionS), v.vertex);
                o.color = v.color;
                /* if(_IsTex>0)
                   o.color = v.color;
                 else
                   o.color = _Color;*/
                 //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				 o.uv=v.uv;
                 //UNITY_TRANSFER_FOG(o,o.vertex);

                 return o;
             }

			float4 CxForm(float4 color)
            {
	            return clamp(color * _MulTerm + _AddTerm, 0, 1);
            }

			float4 Premultiply(float4 color)
            {
	            color.rgb *= color.a;
	            return color;
            }

			//float4 MaskPixel(float4 coords, float4 color)
            //{
	           // float alpha = clamp(dot(tex2D(Mask, coords.zw), _MaskChannels), 0, 1);
	          //  return color * alpha;
            //}

             fixed4 frag(v2f i) : SV_Target
             {
                 // sample the texture
                 fixed4 col = i.color;
                 if (_IsTex > 0)
                 {
                     col = tex2D(_MainTex, i.uv);
                    // clip(col.a < 0.5f);
                     if (col.a < 0.4f)
                     {
                         discard;
                     }
                 }
                 UNITY_SETUP_INSTANCE_ID(i); //最后一步
                 // apply fog
                 //UNITY_APPLY_FOG(i.fogCoord, col);
                 return Premultiply(CxForm(col));
             }
         ENDCG
     }
        }
}
