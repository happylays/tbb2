Shader "XQ/UI/logo_uv"
 {
 	Properties 
 	{
	  _MainTex ("Texture", 2D) = "white" { }
	  _Color ("_Color", Color) = (1,1,1,1)
	  _rotate  ("angle",Float) = 75
	  _interval("_jiange_time",Float) = 5
	  _loop ("_loop_time",Float) = 0.7
	  _kuan("_line_kuan",Float) =0.25
	  _MaskTex("Mask", 2D) = "white" { }		
	}
	SubShader
	{
		Tags{ "Queue"="Transparent+599"  "RenderType"="Transparent" }
		Cull Off
		ZWrite Off
		ZTest LEqual
		ColorMask RGBA
		Fog { Mode Off }
		Blend one one
		pass	
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _MaskTex;
			float4 _Color;
			float _rotate;
			float _interval;
			float _loop;
			float _kuan;
			struct v2f 
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
			};
			v2f vert  (appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
				o.uv =		TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}
	
			float inFlash(float angle,float2 uv,float xLength,int interval,int beginTime, float offX, float loopTime)
			{
				float brightness =0;
				float angleInRad = 0.0174444 * angle ;
				float currentTime = _Time.y;
				int currentTimeInt = _Time.y/interval;
				currentTimeInt *=interval;
				float currentTimePassed = currentTime -currentTimeInt;
				 if(currentTimePassed >beginTime)
				 {
					 float xBottomLeftBound;
					 float xBottomRightBound;
					 float xPointLeftBound;
					 float xPointRightBound;
					 float x0 = currentTimePassed-beginTime;
					 x0 /= loopTime;
					 xBottomRightBound = x0;
					 xBottomLeftBound = x0 - xLength;
					 float xProjL;
					 xProjL= (uv.y)/tan(angleInRad);
					 xPointLeftBound = xBottomLeftBound - xProjL;
					 xPointRightBound = xBottomRightBound - xProjL;
					 xPointLeftBound += offX;
					 xPointRightBound += offX;
					  if(uv.x > xPointLeftBound && uv.x < xPointRightBound)
					  {
						  float midness = (xPointLeftBound + xPointRightBound)/2;
						  float rate= (xLength -2*abs(uv.x - midness))/(xLength);
						  brightness = rate;
				  	}
		   		}
			   brightness= max(brightness,0);
			   float4 col =  _Color * brightness;
			   return brightness;
			   }
		   float4 frag (v2f i) : COLOR
		   {
			   float4 outp;
			   float4 texCol = tex2D(_MainTex,i.uv);
			   float4 maskCol = tex2D(_MaskTex,i.uv);
			   float tmpBrightness;
			   tmpBrightness =inFlash(_rotate,i.uv,_kuan,_interval, 0.1f,0,_loop);
			    if(texCol.w >0.3)
			    outp  = float4(1,1,1,texCol.a) * tmpBrightness  * _Color * texCol.aaaa * texCol.aaaa * maskCol.aaaa * maskCol.aaaa;
			    else
			    outp  = float4(0,0,0,0);
			  
			    return outp;
		    }
		    ENDCG
		}
   }
}