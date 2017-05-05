/* 

skin illum + alpha shader

*/

Shader "XuanQu/Charactor/Skin Colored" {
	
Properties {
	_Color ("Main Color", Color) = (0.47843137,0.44313725,0.4745098,1)
	_MainTex ("Base", 2D) = "grey" {}
//	_Level ("Level",float) = 0.2
//	_BlendColor("Blend Color", Color) = (1,1,1,1)
	_Transparency( "Transparency", Range (0,1)) = 0.0
}

SubShader {
	Lighting Off
	Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Transparency" }
	LOD 200
	Fog {Mode Off}
	cull off

	Pass {	

		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		
		#include "UnityCG.cginc"
		
		uniform half4 _MainTex_ST;
		uniform sampler2D _MainTex;
		fixed _Cutoff;
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
		
		half4 frag (v2f i) : COLOR 
		{
			fixed4 tex = tex2D(_MainTex, i.uv.xy) * _Color * 2;
			
			clip ( tex.r - _Transparency );

			return tex;
		}
		
		ENDCG
	}
}

Fallback "Diffuse"
}
