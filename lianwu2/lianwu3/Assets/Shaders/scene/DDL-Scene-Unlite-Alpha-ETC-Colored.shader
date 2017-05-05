Shader "DDL/Scene/Unlite-Alpha-ETC-Colored"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AlphaTex ("Alpha (A)",2D ) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		Fog {Mode Off}
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		cull off
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
	fixed4 _Color;
	
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
	fixed4 _DDL_Global_Add_Color;
	fixed4 frag (v2f i) : COLOR0
	{
		fixed4 o;
		fixed4 tex= tex2D (_MainTex, i.uv.xy);//
		tex.rgb = tex.rgb * _DDL_Global_Add_Color.rgb;
		tex.a = tex2D (_AlphaTex, i.uv.xy).r * _DDL_Global_Add_Color.a;
		o = tex * _Color ;
		return o;
		
	}
   ENDCG
}
	}
Fallback "Diffuse"
}