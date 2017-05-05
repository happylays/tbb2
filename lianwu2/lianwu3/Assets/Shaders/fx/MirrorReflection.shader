

Shader "FX/Mirror Reflection" { 
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _ReflectionTex ("Reflection", 2D) = "Black" { TexGen ObjectLinear }
    _ReflectionRate ( "Reflection Rate",Range(0,1) )= 0.5
}

// two texture cards: full thing
Subshader { 
	Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 400
	Pass {
//	    Lighting  off
		Blend SrcAlpha OneMinusSrcAlpha
//        SetTexture[_MainTex] { combine texture }
//        SetTexture[_ReflectionTex] { matrix [_ProjMatrix] combine texture, previous }
//}

	CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f_full members normal)
#pragma exclude_renderers d3d11 xbox360
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct v2f_full members normal)
#pragma exclude_renderers xbox360
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
// #pragma exclude_renderers gles
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
// #pragma exclude_renderers gles
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
// #pragma exclude_renderers gles
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
// #pragma exclude_renderers gles
//	#pragma surface surf Lambert alpha
//	#include "UnityCG.cginc"
	
//	sampler2D _MainTex;
//	sampler2D _ReflectionTex;
//	float _ReflectionRate;
//	uniform float4x4 _ProjMatrix;
	
//	struct Input {
//		float2 uv_MainTex;
//	};
	
//	void surf (Input IN, inout SurfaceOutput o) {
//	    float2 u = MultiplyUV( _ProjMatrix ,IN.uv_MainTex );
//	    if( _ProjMatrix._m01 > -1 )
//	    {
//			o.Albedo = (0.5,0.5,0.5) ;
//			o.Alpha = 1;
//	    }
//	    else
//	    {
//			fixed4 cm = tex2D( _MainTex, IN.uv_MainTex ) ;
//			fixed4 cr = tex2D(_ReflectionTex,u );//IN.uv_MainTex
//			o.Albedo = cr.rgb ;
//			o.Alpha = cm.a * _ReflectionRate;
//		}
//	}


	#include "UnityCG.cginc" 
	
	struct v2f_full {
		half4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;	
		half4 scr : TEXCOORD1;
		#ifndef LIGHTMAP_OFF
		float2 lmap : TEXCOORD2;
		#endif
		float3 normal : TEXCOORD3;
	};

	float4 _MainTex_ST;
	
	sampler2D _MainTex;
	sampler2D _ReflectionTex;
	float _ReflectionRate;
	
	#ifndef LIGHTMAP_OFF
	float4 unity_LightmapST;
	sampler2D unity_Lightmap;
	#endif
	
	v2f_full vert( appdata_full v )
	{
		v2f_full o;
		
		o.pos = mul (UNITY_MATRIX_MVP, v.vertex);//		//_ProjMatrix;UNITY_MATRIX_MVP
		o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
		o.scr = ComputeScreenPos(o.pos);

		o.normal = v.normal;
		
		#ifndef LIGHTMAP_OFF
		o.lmap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
		#endif
				
		return o; 
	}
	
	half4 frag( v2f_full i ) : COLOR
	{	
		i.scr = i.scr/i.scr.w;
		//i.scr.xy += i.normal.xy;
		half4 color;
		fixed4 cm = tex2D( _MainTex, i.uv.xy ) ;
		fixed4 cr = tex2D(_ReflectionTex,i.scr.xy  );//IN.uv_MainTex
		color.rgb = cr.rgb * _ReflectionRate + cm.rgb * ( 1 - _ReflectionRate ) ;

		#ifndef LIGHTMAP_OFF
		color.xyz *= DecodeLightmap (tex2D(unity_Lightmap, i.lmap));
		#endif
			
		color.a = 1;
		//color.a = _ReflectionRate;
		return color;
		
	}		

	#pragma vertex vert
	#pragma fragment frag
	#pragma fragmentoption ARB_precision_hint_fastest 

	ENDCG
	}
}

// fallback: just main texture
Subshader {
    Pass {
        SetTexture [_MainTex] { combine texture }
    }
}


}


