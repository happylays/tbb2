Shader "FxShader_py/py_blend_t" {
    Properties {
        _tex01RGBA ("tex01(RGBA)", 2D) = "white" {}
        _color_strong ("color_strong", Float ) = 1
        _colorRGBA ("color(RGBA)", Color) = (0.5,0.5,0.5,1)
        _alpha_strong ("alpha_strong", Float ) = 1
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
            uniform sampler2D _tex01RGBA; uniform float4 _tex01RGBA_ST;
            uniform float _color_strong;
            uniform float4 _colorRGBA;
            uniform float _alpha_strong;
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

                float4 _tex01RGBA_var = tex2D(_tex01RGBA,TRANSFORM_TEX(i.uv0, _tex01RGBA));
                float3 emissive = ((_colorRGBA.rgb*(_color_strong*_tex01RGBA_var.rgb))*i.vertexColor.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,saturate((i.vertexColor.a*(_colorRGBA.a*(_tex01RGBA_var.a*_alpha_strong)))));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
