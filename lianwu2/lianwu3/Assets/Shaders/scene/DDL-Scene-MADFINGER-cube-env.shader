Shader "DDL/Scene/MADFINGER/Environment/Cube env map (Supports Lightmap)" {

Properties 
{
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_EnvTex ("Cube env tex", CUBE) = "black" {}
	_Spread("Spread", Range (0.1,0.5)) = 0.5
	_MainTexIntensity("MainTex Intensity", Range (0.1,1)) = 0.5
	//_ReflIntensity("aaa", float) = 0.5
}

SubShader {
	Tags { "RenderType"="Opaque" "LightMode"="ForwardBase"}
	LOD 100
	
	
	
	CGINCLUDE
	#include "UnityCG.cginc"
	
	sampler2D _MainTex;
	samplerCUBE _EnvTex;
	
	#ifndef LIGHTMAP_OFF
	float4 unity_LightmapST;
	sampler2D unity_Lightmap;
	#endif

	fixed _Spread;
	fixed _MainTexIntensity;
	fixed _ReflIntensity;
	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		#ifndef LIGHTMAP_OFF
		float2 lmap : TEXCOORD1;
		#endif
		float3 refl : TEXCOORD2;
	};

	
	v2f vert (appdata_full v)
	{
		v2f o;
		
		o.pos	= mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv		= v.texcoord;
		
		float3 worldNormal = mul((float3x3)_Object2World, v.normal);
		//float3 i = -WorldSpaceViewDir(v.vertex);
		o.refl = reflect( -WorldSpaceViewDir(v.vertex), worldNormal );//-WorldSpaceViewDir(v.vertex) - 2 * worldNormal * dot( worldNormal , i );//
		o.refl.x = -o.refl.x;
		
		#ifndef LIGHTMAP_OFF
		o.lmap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
		#endif
		
		
		return o;
	}
	ENDCG


	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest	
		
		float4 _DDL_Global_Add_Color;		
		fixed4 frag (v2f i) : COLOR
		{
			fixed4 c 	= tex2D (_MainTex, i.uv.xy) * _MainTexIntensity;
			
			#ifndef LIGHTMAP_OFF
			c.xyz *= DecodeLightmap (tex2D(unity_Lightmap, i.lmap));
			#endif
			
			c.xyz += texCUBE(_EnvTex,i.refl) * c.a * _Spread;
			
			return c * _DDL_Global_Add_Color;
		}
		ENDCG 
	}	
}
}


