Shader "DDL/Scene/Simple_Transparent" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

		Pass {
			Lighting Off
			SetTexture [_MainTex] 
			{ 
				combine texture 
			} 
		}
	}
	FallBack "VertexLit"
}
