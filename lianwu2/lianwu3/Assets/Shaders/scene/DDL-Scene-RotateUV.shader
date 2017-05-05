Shader "DDL/Scene/MADFINGER/FX/Rotate UV" 
{
    Properties 
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Mask ("Mask",2D) = "white"{}
        _RotateSpeed("Speed",float) = 1
    }
    
    SubShader {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType" = "Transparent" }
        
        ZWrite Off
		Blend SrcAlpha One
        Cull Off
        Lighting Off 
        Fog { Color (0,0,0,0) }
     
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            struct v2f 
            {
                float4  pos : SV_POSITION;
                float4  uv : TEXCOORD0;
            };

			sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Mask;
            float _RotateSpeed;
            
            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
            
            half4 frag(v2f i) : COLOR
            {
            	float2 Ruv = i.uv.xy - float2(0.5,0.5);
            	float2 rotate = float2(cos(_RotateSpeed * _Time.z),sin(_RotateSpeed * _Time.z));
            	Ruv = float2(Ruv.x*rotate.x - Ruv.y*rotate.y,Ruv.x*rotate.y + Ruv.y*rotate.x);
            	Ruv += float2(0.5,0.5);
            	
                half4 col = tex2D(_MainTex, Ruv); 
                half4 msk = tex2D(_Mask,i.uv.xy);
                
                half4 o = (0,0,0,0);
                o.rgb = col.rgb;
                o.a = col.a*msk.r;
                return o;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Cutout/Diffuse"
}