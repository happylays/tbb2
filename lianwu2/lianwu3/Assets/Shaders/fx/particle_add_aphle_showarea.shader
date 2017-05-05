Shader "XuanQu/FX/Particle_Add_Aphle_Showarea" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MinX ("Min X", Float) = -1
		_MaxX ("Max X", Float) = 1
		_MinY ("Min Y", Float) = -1
		_MaxY ("Max Y", Float) = 1
	}
	
	Category {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
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
			fixed4 _TintColor;
			float _MinX;
            float _MaxX;
            float _MinY;
            float _MaxY;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float3 vpos : TEXCOORD2;
			};
			
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vpos = v.vertex.xyz;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 c =2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord);
				c.a *= (i.vpos.x >= _MinX);
				c.a *= (i.vpos.x <= _MaxX);
				c.a *= (i.vpos.y >= _MinY);
				c.a *= (i.vpos.y <= _MaxY);
				c.rgb *= c.a;
				return c;
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
