//Version=0
Shader"UNOShader/UNLIT"
{
	Properties
	{
		_Color ("Main Color (A)Opacity", Color) = (1,1,1,1)	
		//_DiffuseTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags
		{
			"LightMode" = "ForwardBase"
		}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct customData
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
			struct v2f // = vertex to fragment ( pass vertex data to pixel pass )
			{
				float4 pos : SV_POSITION;
			};
			v2f vert (customData v)
			{
				v2f o;
				o.pos = 	mul (UNITY_MATRIX_MVP, v.vertex);
				return o;
			}

			fixed4 frag (v2f i) : COLOR  // i = in gets info from the out of the v2f vert
			{
				fixed4 result = fixed4(1,1,1,1);
				return result;
			}
			ENDCG
		}//-------------------------------Pass-------------------------------
	} //-------------------------------SubShader-------------------------------
	CustomEditor "UNOShader_UNLIT"
}