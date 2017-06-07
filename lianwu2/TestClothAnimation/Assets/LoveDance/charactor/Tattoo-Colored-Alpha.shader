Shader "XuanQu/Charactor/Tattoo Colored Alpha" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MainTexMask ("Mask (RGB)", 2D) = "white" {}
		_Transparency( "Transparency", Range (0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert noforwardadd
		
		sampler2D _MainTex;
		sampler2D _MainTexMask;
		fixed4 _Color;
		fixed _Transparency;
		
		struct Input {
			float2 uv_MainTex;
		};
		
		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 ca = tex2D(_MainTexMask, IN.uv_MainTex);
			
			o.Emission = ((1-ca.r)*c.rgb)*_Color ;
			o.Albedo = ((1-ca.r)*c.rgb)*_Color + ca.r * c.rgb;
			o.Alpha = c.a;
			clip ( o.Albedo - _Transparency );
		}
		ENDCG
		}
	Fallback "Diffuse"
}
