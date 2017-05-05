Shader "DDL/Scene/Lightmap"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AlphaTex ("Alpha (A)",2D ) = "white" {}
		_LightMap ("Lightmap (RGB)", 2D) = "black" {}
	}

	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		Fog {Mode Off}
		ZWrite On
		
CGPROGRAM
#pragma surface surf Lambert noforwardadd alpha

sampler2D _MainTex;
sampler2D _AlphaTex;
sampler2D _LightMap;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
	float2 uv2_LightMap;
};


float4 _DDL_Global_Add_Color;

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 texaphla = tex2D(_AlphaTex, IN.uv_MainTex);
	o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _Color * _DDL_Global_Add_Color.rgb;;
	half4 lm = tex2D (_LightMap, IN.uv2_LightMap);
	o.Emission = lm.rgb*o.Albedo.rgb;
	o.Alpha = texaphla.r*_DDL_Global_Add_Color.a;
}
ENDCG

	}
Fallback "Diffuse"
}