Shader "DDL/Scene/Alpha-Cutoff-Reflection" {
	Properties {
		_MainTex ("Diffuse (RGB)", 2D) = "white" {}
		_Color ("DiffuseColor(RGBA)",Color)=(1,1,1,1)
		_MainTexAphle ("Aphle (A)", 2D) = "white" {}
		_Cube ("Environment(Cubemap))", CUBE) = "" {}
		_ReflectedColor("Reflected Color(RGB)",Color) = (1,1,1,1)
		_Transparency("Reflected Intensity(Level:0-2)", Range (0.0,2.0)) = 0.0
		_Cutoff( "Cutoff(Level:0-1;default:0.2)", Range (0.0,1.0)) = 0.2
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 200
		Fog {Mode Off}
		Zwrite on
		cull off
		Blend SrcAlpha OneMinusSrcAlpha 

		CGPROGRAM
		#pragma surface surf Lambert 
		
		sampler2D _MainTex;
		float4 _Color;
		sampler2D _MainTexAphle;
		samplerCUBE _Cube;
		fixed _Transparency;
		float4 _ReflectedColor;
		fixed _Cutoff;
		
		struct Input {
			float2 uv_MainTex;
			float3 worldRefl;
			INTERNAL_DATA
		};
		
		float4 _DDL_Global_Add_Color;
		
		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 a = tex2D(_MainTexAphle, IN.uv_MainTex);
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Emission = texCUBE (_Cube, WorldReflectionVector (IN, o.Normal)).rgb* _Transparency * _DDL_Global_Add_Color.rgb*_Color.rgb;
			o.Albedo = c.rgb * _DDL_Global_Add_Color.rgb * _Color.rgb;
			o.Alpha = a.r * _DDL_Global_Add_Color.a * _Color.a;

			clip ( o.Alpha - _Cutoff );
		}
		ENDCG
	}
	Fallback "VertexLit"
}


