Shader "XuanQu/Charactor/Skin Colored Alpha" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_MainTexMask ("Mask (RGB)", 2D) = "white" {}
	_Transparency( "Transparency", Range (0,1)) = 0.0
}

SubShader {
	Tags { "RenderType"="Opaque" }
		
	LOD 200
		
	Pass {	

		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		
		#include "UnityCG.cginc"

		uniform half4 _MainTex_ST;
		sampler2D _MainTex;
		sampler2D _MainTexMask;
		fixed4 _Color;
		fixed _Transparency;
		
		struct v2f
		{
			half4 pos : POSITION;
			half2 uv : TEXCOORD0;
		};
		
		v2f vert (appdata_base v)
		{
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);	
			o.uv.xy = TRANSFORM_TEX(v.texcoord,_MainTex);
			
			return o; 
		}

		half4 frag(v2f i) : COLOR 
		{
			fixed4 c = tex2D(_MainTex, i.uv.xy);
			fixed4 ca = tex2D(_MainTexMask, i.uv.xy);
			
			c.rgb = ((1-ca.r) * c.rgb) * _Color * 2 + ca.r * c.rgb;			
			c.a = ca.r;
			
			clip ( c.r - _Transparency );
						
			return c;

		}

		ENDCG
	}
}

Fallback "Diffuse"
}
