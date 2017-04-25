Shader "XQ/UI/XQ-Alpha-ETC"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AlphaTex ("Alpha (A)",2D ) = "white" {}
	}

	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		Fog {Mode Off}
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Cull OFF
Pass
{
	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#pragma fragmentoption ARB_precision_hint_fastest
	#include "UnityCG.cginc"

	sampler2D _MainTex;
	sampler2D _AlphaTex;
	float4 _MainTex_ST;
	
	struct v2f {
	    float4 pos : SV_POSITION;
	    float4 uv : TEXCOORD0;
	    fixed4 color : COLOR;
	};
	
	struct appdata_My {
    float4 vertex : POSITION;
    float4 texcoord : TEXCOORD0;
    fixed4 color : COLOR;
    };
    
	v2f vert (appdata_My v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv.xy = TRANSFORM_TEX(v.texcoord.xy,_MainTex);
	 	o.color = v.color;
	    return o;
	}
	
	fixed4 frag (v2f i) : COLOR0
	{
		fixed4 o;
		fixed4 tex = tex2D (_MainTex, i.uv.xy);
		fixed4 texaphla = tex2D (_AlphaTex, i.uv.xy);
		tex.a = texaphla.r;
		o = tex * i.color;
		return o;
	}
	
   ENDCG
}
	}
Fallback "Diffuse"
}