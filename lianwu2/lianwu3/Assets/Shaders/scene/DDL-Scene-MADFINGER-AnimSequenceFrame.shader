Shader "DDL/Scene/MADFINGER/FX/Anim Sequence Frame" {
	Properties {
	    _Color ("Main Color", Color) = (1,1,1,1)
	    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	    _NumTexTiles("Num tex tiles",	Vector) = (4,4,0,0)
	    _Speed ("Speed", Float) = 100
	}

	SubShader {
	    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	    LOD 200
//	    Blend SrcAlpha One
		Blend One One
		Cull Off 
		Lighting Off 
		ZWrite Off 
		Fog { Color (0,0,0,0) }
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _NumTexTiles;
			fixed4 _Color;
			fixed _Speed;
			
			struct v2f {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
			};
			
			v2f vert (appdata_full v)
			{
				v2f o = (v2f)0;
	            o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				int index = floor(_Time .x * _Speed);
				int indexY = -index/_NumTexTiles.x;
			    int indexX = index - indexY*_NumTexTiles.x;
			    float2 testUV = float2(v.texcoord.x /_NumTexTiles.x, v.texcoord.y /_NumTexTiles.y);
			    
			    testUV.x += indexX/_NumTexTiles.x;
			    testUV.y += indexY/_NumTexTiles.y;
			    
			    o.uv = testUV;
			    
			    return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 o = (0,0,0,0);
				fixed4 c = tex2D(_MainTex, i.uv) * _Color;
				o.rgb = c.rgb;
			    o.a = c.a;
				 
				return o;
			}
			ENDCG
		}
	}
	FallBack "Transparent/Cutout/Diffuse"
}