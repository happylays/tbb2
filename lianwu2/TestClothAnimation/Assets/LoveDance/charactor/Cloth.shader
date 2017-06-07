/* 

skin illum + alpha shader

*/

Shader "XuanQu/Charactor/Cloth" {
	
Properties {
	_MainTex ("Base", 2D) = "grey" {}
	_Transparency( "Transparency", Range (0,1)) = 0.0
}

SubShader {

	Pass {		
		Tags { "RenderType"="Opaque" }
		LOD 200
		cull off

		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		
		#include "UnityCG.cginc"
		
		uniform half4 _MainTex_ST;
		uniform sampler2D _MainTex;
		fixed _Cutoff;
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
			fixed4 tex = tex2D(_MainTex, i.uv.xy);
			
			return tex;
		}
		
		ENDCG
	}
}

Fallback "VertexLit"
}
