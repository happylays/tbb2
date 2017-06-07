// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "XuanQu/Charactor/Cloth-Alpha-Cutoff-Reflection"{
	Properties {
		_MainTex ("Diffuse (RGB)", 2D) = "white" {}
		_MainColor("Diffuse Color",Color) = (1,1,1,1)
		_MainTexAphle ("Aphle (A)", 2D) = "white" {}
		_Mask("Reflected Alpha",2D) = "white" {}
		_Cube("Environment Map", Cube) = "" {}
		_Reflected("Reflected Intensity", Range (0,2)) = 0
		_Color ("Reflected Color", Color) = (1,1,1,1)
		_Cutoff( "Cutoff", Range (0.0,1.0)) = 0.2
	}
	Category {
		SubShader{
			LOD 200
			Blend SrcAlpha OneMinusSrcAlpha 
			cull off 
			
			CGPROGRAM
			#pragma surface surf Lambert 
			
			sampler2D _MainTex;
			sampler2D _MainTexAphle;
			fixed _Cutoff;
			float4 _MainColor;
			
			struct Input {
				float2 uv_MainTex;
			};
			
			void surf (Input IN, inout SurfaceOutput o) {
				fixed4 a = tex2D(_MainTexAphle, IN.uv_MainTex);
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				o.Emission = c.rgb * _MainColor.rgb;
				o.Alpha = a.r * _MainColor.a;
				clip ( o.Alpha - _Cutoff );
			}
			ENDCG
			
			Pass{
				Blend SrcAlpha One  
				Cull Off  Zwrite Off
		
				CGPROGRAM
				#pragma vertex vert 
				#pragma fragment frag 
				#include "UnityCG.cginc" 

				sampler2D _Mask;
				float4 _Mask_ST;
				samplerCUBE _Cube;
				float _Reflected;
				sampler2D _MainTexAphle;
				float4 _MainTexAphle_ST;
				float4 _Color;

				struct v2f{
					float4 pos:SV_POSITION;
					float2 uv:TEXCOORD0;
					float2 uv_mask:TEXCOORD1;
					float3 Rel:TEXCOORD2;
				};
				
				v2f vert(appdata_full v)
				{
					v2f o;
					o.pos=mul(UNITY_MATRIX_MVP,v.vertex);
					o.uv = TRANSFORM_TEX(v.texcoord, _MainTexAphle);
					o.uv_mask = TRANSFORM_TEX(v.texcoord1, _Mask);
					float3 pos_w=mul(unity_ObjectToWorld,v.vertex).xyz;
					float3 nl=mul((float3x3)unity_ObjectToWorld,v.normal);
					nl=normalize(nl);
					float3 I=pos_w-_WorldSpaceCameraPos;
					o.Rel=reflect(I,nl);
					return o;
				}

				float4 frag(v2f i):COLOR
				{
					float4 o =(0,0,0,0);
					float4 tex_a = tex2D(_MainTexAphle,i.uv);
					float tex_mask = tex2D(_Mask,i.uv_mask);
					float4 RelColor = (0,0,0,0);
					RelColor.rgb = texCUBE(_Cube,i.Rel).rgb*_Reflected;
					o.rgb = RelColor.rgb * _Color;
					o.a = tex_mask.r * tex_a.r * _Color.a;
					
					return o;
				}
				ENDCG
			}
		}
	}
}  