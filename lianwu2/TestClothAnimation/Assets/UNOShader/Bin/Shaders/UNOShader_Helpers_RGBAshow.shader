//Version=1.32
Shader"UNOShader/_Library/Helpers/RGBAshow "
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Red ("Red", Range(0,1) ) = 0
		_Green ("Green", Range(0,1) ) = 0
		_Blue ("Blue", Range(0,1) ) = 0
		_Alpha ("Alpha", Range(0,1) ) = 0
	}
	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"Queue" = "Geometry"
		}
		Pass
			{
			Name "ForwardBase"
			Tags
			{
				"RenderType" = "Opaque"
				"Queue" = "Geometry"
				"LightMode" = "ForwardBase"
			}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"			

			
			sampler2D _MainTex;
			half4 _MainTex_ST;
			
			fixed _Red;
			fixed _Green;
			fixed _Blue;
			fixed _Alpha;
			
		
			struct customData
			{
				half4 vertex : POSITION;
				half3 normal : NORMAL;
				half4 tangent : TANGENT;
				fixed2 texcoord : TEXCOORD0;
				fixed2 texcoord1 : TEXCOORD1;
			};
			struct v2f // = vertex to fragment ( pass vertex data to pixel pass )
			{
				half4 pos : SV_POSITION;				
				fixed2 uv : TEXCOORD0;				
				half4 normalDir : TEXCOORD3;//vertex Normal Direction in world space				
			};
			v2f vert (customData v)
			{
				v2f o;
				o.normalDir = fixed4 (0,0,0,0);								
				o.normalDir.xyz = UnityObjectToWorldNormal(v.normal);				
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv.xy = v.texcoord;								
//				TRANSFER_VERTEX_TO_FRAGMENT(o)
				return o;
			}

			fixed4 frag (v2f i) : COLOR  // i = in gets info from the out of the v2f vert
			{
				fixed4 resultRGB = fixed4(0,0,0,0);
				fixed4 T_Diffuse = tex2D(_MainTex, i.uv);
				fixed T_R = T_Diffuse.r*_Red;
				fixed T_G = T_Diffuse.g*_Green;
				fixed T_B = T_Diffuse.b*_Blue;
				fixed T_A = T_Diffuse.a*_Alpha;				
				resultRGB = clamp(T_R.rrrr + T_G.rrrr + T_B.rrrr + T_A.rrrr,0,1);
//				resultRGB = fixed4(lerp(T_B,T_Diffuse.a,_Alpha),1);
//				resultRGB = fixed4(lerp(T_RGB,T_Diffuse.a,_Alpha),1);
				

			//__________________________________ result Final  _____________________________________
				return resultRGB;
			}
			ENDCG
		}//-------------------------------Pass-------------------------------
	} //-------------------------------SubShader-------------------------------	
}