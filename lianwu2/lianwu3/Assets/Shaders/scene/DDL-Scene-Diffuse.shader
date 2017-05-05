// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "DDL/Scene/Diffuse" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 150

CGPROGRAM
#pragma surface surf Lambert noforwardadd

sampler2D _MainTex;

struct Input {
	float2 uv_MainTex;
};

float4 _DDL_Global_Add_Color;

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	o.Albedo = c.rgb*_DDL_Global_Add_Color.rgb;
	o.Alpha = c.a*_DDL_Global_Add_Color.a;
}
ENDCG
}

Fallback "Mobile/VertexLit"
}