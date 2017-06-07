Shader "UNOShader/_Library/Helpers/Shadows Transparent" 
{
	Properties 
	{	
	_MainTex ("Base (RGB) Trans (A)", 2D) = "Black" {}
	_MaintTexOpacity("Diffuse Opacity",Range (0,1))=1 	
	_Color ("Main Color", Color) = (1,1,1,1)
	
	_DecalTex ("Base (RGB) Trans (A)", 2D) = "Black" {}
	_DecalColor ("Main Color", Color) = (1,1,1,1)	
	
	_EmissionMap ("Base (RGB) Trans (A)", 2D) = "Black" {}
	_EmissionColor ("Main Color", Color) = (1,1,1,1)
	_Transparency("Transparency",Range (0,1))=1 	
	}

	SubShader 
	{
		Pass 
		{
			Name "SHADOWCAST"
			Tags { "LightMode" = "ShadowCaster" }
			Offset 1, 1
			Fog {Mode Off}
			ZWrite On 
			ZTest LEqual 
			//Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _MainTexOpacity;
			uniform fixed4 _Color;
			float4x4 _MainTexMatrix;
			
			uniform sampler2D _DecalTex;
			uniform float4 _DecalTex_ST;
			uniform fixed4 _DecalColor;
			float4x4 _DecalTexMatrix;
			
			uniform sampler2D _EmissionMap;
			uniform float4 _EmissionMap_ST;
			uniform fixed4 _EmissionColor;
			float4x4 _EmissionMapMatrix;
			
			uniform float _Transparency;
			
			
			struct v2f 
			{ 
				V2F_SHADOW_CASTER;
				fixed2  uv : TEXCOORD5;
				fixed2  uv2 : TEXCOORD6;
				fixed2  uv3 : TEXCOORD7;
			};


			v2f vert( appdata_base v )
			{
				v2f o;				
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv = 	mul(_MainTexMatrix, fixed4(o.uv,0,1)); // this allows you to rotate uvs and such with script help
				
				o.uv2 = TRANSFORM_TEX(v.texcoord, _DecalTex);
				o.uv2 =	mul(_DecalTexMatrix, fixed4(o.uv2,0,1)); // this allows you to rotate uvs and such with script help
				
				o.uv3 = TRANSFORM_TEX(v.texcoord, _EmissionMap);
				o.uv3 =	mul(_EmissionMapMatrix, fixed4(o.uv3,0,1)); // this allows you to rotate uvs and such with script help
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				return o;
			}			

			float4 frag( v2f i ) : COLOR
			{
				fixed4 T_Diffuse = tex2D( _MainTex, i.uv );
				fixed4 T_Decal = tex2D( _DecalTex, i.uv2 );
				fixed4 T_Glow = tex2D( _EmissionMap, i.uv3 );
				fixed result = _Color.a;
				result = lerp(result,clamp(T_Diffuse.a + _Color.a,0,1),(T_Diffuse.a *_MainTexOpacity));
				result = lerp( result,1,T_Decal.a * _DecalColor.a );
				result = lerp( result,1,T_Glow.a * _EmissionColor.a ) + _Color.a;
				result *= _Transparency;
				clip( result -.3 );
				
				
				SHADOW_CASTER_FRAGMENT(i)
				
			}
			ENDCG

		}// Pass			
		
		Pass 
		{
			Name "SHADOWCAST CULLOFF"
			Tags { "LightMode" = "ShadowCaster" }
			Offset 1, 1
			Fog {Mode Off}
			ZWrite On 
			ZTest LEqual 
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _MainTexOpacity;
			uniform fixed4 _Color;
			float4x4 _MainTexMatrix;
			
			uniform sampler2D _DecalTex;
			uniform float4 _DecalTex_ST;
			uniform fixed4 _DecalColor;
			float4x4 _DecalTexMatrix;
			
			uniform sampler2D _EmissionMap;
			uniform float4 _EmissionMap_ST;
			uniform fixed4 _EmissionColor;
			float4x4 _EmissionMapMatrix;
			
			uniform float _Transparency;
			
			struct v2f 
			{ 
				V2F_SHADOW_CASTER;
				fixed2  uv : TEXCOORD5;
				fixed2  uv2 : TEXCOORD6;
				fixed2  uv3 : TEXCOORD7;
			};


			v2f vert( appdata_base v )
			{
				v2f o;								
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv = 	mul(_MainTexMatrix, fixed4(o.uv,0,1)); // this allows you to rotate uvs and such with script help
				
				o.uv2 = TRANSFORM_TEX(v.texcoord, _DecalTex);
				o.uv2 =	mul(_DecalTexMatrix, fixed4(o.uv2,0,1)); // this allows you to rotate uvs and such with script help
				
				o.uv3 = TRANSFORM_TEX(v.texcoord, _EmissionMap);
				o.uv3 =	mul(_EmissionMapMatrix, fixed4(o.uv3,0,1)); // this allows you to rotate uvs and such with script help
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				return o;
			}

			

			float4 frag( v2f i ) : COLOR
			{
				fixed4 T_Diffuse = tex2D(_MainTex, i.uv);
				fixed4 T_Decal = tex2D(_DecalTex, i.uv2);
				fixed4 T_Glow = tex2D(_EmissionMap, i.uv3);
				fixed result = _Color.a;
				result = lerp(result,clamp(T_Diffuse.a + _Color.a,0,1),(T_Diffuse.a *_MainTexOpacity));
				result = lerp(result,1,T_Decal.a * _DecalColor.a);
				result = lerp(result,1,T_Glow.a * _EmissionColor.a) + _Color.a;
				result *= _Transparency;
				clip(result - .3);
				
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG

		}// Pass					
	}//-- Subshader
}
