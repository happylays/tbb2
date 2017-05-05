
Shader "XuanQu/Charactor/Cloth-AlphaTest" {

Properties {
	_MainTex ("Base", 2D) = "grey" {}
	_MainTexAphle ("Base (RGB)", 2D) = "white" {}	
	_Cutoff( "Cutoff", Range (0,1)) = 0.5
}

SubShader {
	Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Transparent" }
	LOD 200
	cull off

	Pass {			

		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		
		#include "UnityCG.cginc"
		
		uniform half4 _MainTex_ST;
		uniform sampler2D _MainTex;
		uniform sampler2D _MainTexAphle;
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
			fixed4 a = tex2D(_MainTexAphle, i.uv.xy);			
			tex.a = a.r;
			clip ( a.r - _Cutoff );

			return tex;
		}
		
		ENDCG
	}
}

Fallback "VertexLit"
}
