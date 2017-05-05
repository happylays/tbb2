Shader "FxShader_py/py_additive(add_disturbance-add-mul)_a" {
    Properties {
        _tex01_disturdanceR ("tex01_disturdance(R)", 2D) = "white" {}
        _tex02RGB ("tex02(RGB)", 2D) = "white" {}
        _tex03_maskRGB ("tex03_mask(RGB)", 2D) = "white" {}
        _tex01_strong ("tex01_strong", Float ) = 1
        _tex02_strong ("tex02_strong", Float ) = 1
        _tex01_panner_u ("tex01_panner_u", Float ) = 0
        _tex01_panner_v ("tex01_panner_v", Float ) = 0
        _tex02_panner_u ("tex02_panner_u", Float ) = 0
        _tex02_panner_v ("tex02_panner_v", Float ) = 0
        _colorRGBA ("color(RGBA)", Color) = (0.5,0.5,0.5,1)
        _alpha_strong ("alpha_strong", Float ) = 1
        _tex03_panner_u ("tex03_panner_u", Float ) = 0
        _tex03_panner_v ("tex03_panner_v", Float ) = 0
        _disturbance_strong ("disturbance_strong", Float ) = 0
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
            Blend One One
            Cull Off
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
            uniform sampler2D _tex01_disturdanceR; uniform float4 _tex01_disturdanceR_ST;
            uniform sampler2D _tex02RGB; uniform float4 _tex02RGB_ST;
            uniform sampler2D _tex03_maskRGB; uniform float4 _tex03_maskRGB_ST;
            uniform float _tex01_strong;
            uniform float _tex02_strong;
            uniform float _tex01_panner_u;
            uniform float _tex01_panner_v;
            uniform float _tex02_panner_u;
            uniform float _tex02_panner_v;
            uniform float4 _colorRGBA;
            uniform float _alpha_strong;
            uniform float _tex03_panner_u;
            uniform float _tex03_panner_v;
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
                float4 node_8794 = _Time + _TimeEditor;
                float2 node_4899 = (i.uv0+(node_8794.g*float2(_tex01_panner_u,_tex01_panner_v))*float2(1,1));
                float4 _tex01_disturdanceR_var = tex2D(_tex01_disturdanceR,TRANSFORM_TEX(node_4899, _tex01_disturdanceR));
                float4 node_8937 = _Time + _TimeEditor;
                float2 node_9625 = ((_tex01_disturdanceR_var.r*_disturbance_strong)+(i.uv0+(node_8937.g*float2(_tex02_panner_u,_tex02_panner_v))*float2(1,1)));
                float4 _tex02RGB_var = tex2D(_tex02RGB,TRANSFORM_TEX(node_9625, _tex02RGB));
                float4 node_3192 = _Time + _TimeEditor;
                float2 node_510 = (i.uv0+(node_3192.g*float2(_tex03_panner_u,_tex03_panner_v))*float2(1,1));
                float4 _tex03_maskRGB_var = tex2D(_tex03_maskRGB,TRANSFORM_TEX(node_510, _tex03_maskRGB));
                float3 emissive = ((_colorRGBA.a*i.vertexColor.a)*(((_colorRGBA.rgb*((_tex01_strong*_tex01_disturdanceR_var.r)+(_tex02_strong*_tex02RGB_var.rgb)))*i.vertexColor.rgb)*saturate((_tex03_maskRGB_var.rgb*_alpha_strong))));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
