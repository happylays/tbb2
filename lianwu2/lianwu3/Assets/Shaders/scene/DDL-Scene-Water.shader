Shader "DDL/Scene/Water"
{
	Properties {
		_MainTex("Main Texture", 2D) = "white" {}
		_WaveTex("Wave Texture", 2D) = "bump" {}
		_WaveSpeed("Wave Speed", Range(0.0, 0.1)) = 0.01
		_WaveAmount("Wave Amount", Range(0.0, 2.0)) = 0.5
		_FresnelTex("Fresnel Texture", 2D) = "white" {}
		_FresnelPower("Fresnel Power", Float) = 1
		_CubeTex("Reflection Texture", Cube) = "" {}
		_Transparency("Transparency", Range(0.0, 1.0)) = 0.5
	}
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True" }
		LOD 200

		Pass {
			Lighting Off Fog { Mode Off }
			ZWrite Off

			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
				#include "UnityCG.cginc"
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest

				sampler2D _MainTex, _WaveTex, _FresnelTex;
				fixed4 _MainTex_ST, _WaveTex_ST;
				fixed _WaveSpeed, _WaveAmount, _FresnelPower, _Transparency;
				samplerCUBE _CubeTex;

				struct v2f {
					float4 Pos : SV_POSITION;
					fixed4 UvMain : TEXCOORD0;
					fixed4 UvBump : TEXCOORD1;
					fixed3 View : TEXCOORD2;
				};

				v2f vert (appdata_base v) {
					v2f o;
					o.Pos = mul(UNITY_MATRIX_MVP, v.vertex);
					o.UvMain.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
					fixed2 uv = TRANSFORM_TEX(v.texcoord, _WaveTex);
					o.UvBump.xy = uv + (_Time.y * _WaveSpeed);
					o.UvBump.zw = uv - (_Time.y * _WaveSpeed);
					float3 vPos = UNITY_MATRIX_IT_MV[3].xyz - v.vertex.xyz;
					o.UvMain.zw = 0.5 + (-vPos.xz / ((vPos.y + 5) * _FresnelPower)) * 0.5;
					o.View = normalize(ObjSpaceViewDir(v.vertex));
					o.View.xz *= -1;
					return o;
				}

				fixed4 frag(v2f i) : COLOR {
					fixed3 distort = ((tex2D(_WaveTex, i.UvBump.xy).rgb - 0.5) + (tex2D(_WaveTex, i.UvBump.zw).rgb - 0.5));
					distort *=  _WaveAmount;
					fixed3 refraction = tex2D(_MainTex, i.UvMain.xy + distort.rg).rgb;
					fixed3 reflection = texCUBE(_CubeTex, reflect(i.View, distort)).rgb;
					fixed fresnel = tex2D(_FresnelTex, i.UvMain.zw).r;
					fixed3 color = lerp(reflection, refraction, fresnel);
					return fixed4(color, (1.0 - fresnel) + _Transparency);
				}
			ENDCG
		}
	} 
	FallBack Off
}