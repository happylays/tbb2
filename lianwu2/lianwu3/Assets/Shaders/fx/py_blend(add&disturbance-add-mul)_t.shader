Shader "FxShader_py/py_blend(add_disturbance-add-mul)_t" {
    Properties {
        _tex01_disturbanceR ("tex01_disturbance(R)", 2D) = "white" {}
        _tex02RGB ("tex02(RGB)", 2D) = "white" {}
        _tex03_maskRGB ("tex03_mask(RGB)", 2D) = "white" {}
        _tex01_strong ("tex01_strong", Float ) = 1
        _tex02_strong ("tex02_strong", Float ) = 1
        _color ("color", Color) = (0.5,0.5,0.5,1)
        _tex03_strong ("tex03_strong", Float ) = 1
        _alpha_strong ("alpha_strong", Float ) = 1
        _tex01_panner_u ("tex01_panner_u", Float ) = 0
        _tex01_panner_v ("tex01_panner_v", Float ) = 0
        _tex02_panner_u ("tex02_panner_u", Float ) = 0
        _tex02_panner_v ("tex02_panner_v", Float ) = 0
        _disturbance_strong ("disturbance_strong", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _tex01_disturbanceR; uniform float4 _tex01_disturbanceR_ST;
            uniform sampler2D _tex02RGB; uniform float4 _tex02RGB_ST;
            uniform sampler2D _tex03_maskRGB; uniform float4 _tex03_maskRGB_ST;
            uniform float _tex01_strong;
            uniform float _tex02_strong;
            uniform float _tex01_panner_u;
            uniform float _tex01_panner_v;
            uniform float _tex02_panner_u;
            uniform float _tex02_panner_v;
            uniform float4 _color;
            uniform float _alpha_strong;
            uniform float _tex03_strong;
            uniform float _disturbance_strong;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
                float4 node_5611 = _Time + _TimeEditor;
                float2 node_7609 = (i.uv0+(node_5611.g*float2(_tex01_panner_u,_tex01_panner_v))*float2(1,1));
                float4 _tex01_disturbanceR_var = tex2D(_tex01_disturbanceR,TRANSFORM_TEX(node_7609, _tex01_disturbanceR));
                float4 node_5489 = _Time + _TimeEditor;
                float2 node_795 = ((_tex01_disturbanceR_var.r*_disturbance_strong)+(i.uv0+(node_5489.g*float2(_tex02_panner_u,_tex02_panner_v))*float2(1,1)));
                float4 _tex02RGB_var = tex2D(_tex02RGB,TRANSFORM_TEX(node_795, _tex02RGB));
                float4 _tex03_maskRGB_var = tex2D(_tex03_maskRGB,TRANSFORM_TEX(i.uv0, _tex03_maskRGB));
                float3 node_5359 = (((_tex01_strong*_tex01_disturbanceR_var.r)+(_tex02_strong*_tex02RGB_var.rgb))*(_tex03_maskRGB_var.rgb*_tex03_strong));
                float3 emissive = ((_color.rgb*node_5359)*i.vertexColor.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,saturate(((i.vertexColor.a*(_color.a*dot(node_5359,float3(0.3,0.59,0.11))))*_alpha_strong)));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
