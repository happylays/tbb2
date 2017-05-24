Shader "XuanQu/ManualScroll" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_AlphaTex ("Alpha (A)",2D ) = "white" {}
	_Color ("Main Color", Color) = (1,1,1,1)
	_UVOffsetx ("UV Offst x",Float) = 0
	_UVOffsety ("UV Offst y",Float) = 0
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }

	pass{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest		
#include "UnityCG.cginc"
		
		sampler2D _MainTex;
		sampler2D _AlphaTex;
		float4 _MainTex_ST;
		float _UVOffsetx;
		float _UVOffsety;
		fixed4 _Color;
		
		struct appdata_My {
	    float4 vertex : POSITION;
    	float4 texcoord : TEXCOORD0;
    	};
    	
		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
		};
		
		v2f vert (appdata_My v)
		{
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv.xy = TRANSFORM_TEX(v.texcoord.xy,_MainTex);
			o.uv.x += _UVOffsetx;
			o.uv.y += _UVOffsety;
			return o;
		};
		
		fixed4 frag (v2f i) : COLOR
		{
			fixed4 o;
			fixed4 tex = tex2D (_MainTex, i.uv.xy);
			fixed4 texaphla = tex2D (_AlphaTex, i.uv.xy);
			tex.a = texaphla.r;
			o = tex * _Color;
			return o;
		};

ENDCG 

	}	
}

Fallback "VertexLit"
}
