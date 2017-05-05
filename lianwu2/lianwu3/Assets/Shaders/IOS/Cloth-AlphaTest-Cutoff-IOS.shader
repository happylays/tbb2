/* 

cloth illum + alpha shader

*/

Shader "XuanQu/Charactor/Cloth-AlphaTest-Cutoff-IOS" {
	
Properties {
	_MainTex ("Base", 2D) = "grey" {}
	_Cutoff( "Cutoff", Range (0,1)) = 0.5
}

SubShader {
	Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Transparent" }
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
		uniform fixed _SelfIllumination;
		fixed _Cutoff;
		
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
			clip ( tex.a - _Cutoff );

			return tex;
		}
		
		ENDCG
	}
}

Fallback "VertexLit"
}
