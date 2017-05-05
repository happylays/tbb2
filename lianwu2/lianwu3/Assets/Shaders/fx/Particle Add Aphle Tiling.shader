Shader "XuanQu/FX/Particle add aphle Tiling" {
Properties {
	_MainTex ("Particle Texture", 2D) = "white" {}
	_MainAlpha("Alpha Texture",2D) = "white" {}
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_particles

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _MainAlpha;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 pos : POSITION;
				fixed4 color : COLOR;
				float4 _MainTex_UV : TEXCOORD0;
			};
			
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o._MainTex_UV.xy = TRANSFORM_TEX(v.texcoord.xy,_MainTex);
				o._MainTex_UV.zw = v.texcoord.xy;
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 o;
				fixed4 tex = tex2D (_MainTex, i._MainTex_UV.xy);
				fixed4 texaphla = tex2D (_MainAlpha, i._MainTex_UV.zw);
				tex.a = texaphla.r;
				o = tex * i.color;
				return o ;
			}
			ENDCG 
		}
	} 	
	
	SubShader {
		Pass {
			SetTexture [_MainTex] {
				combine texture * primary
			}
		}
	}
}
}
