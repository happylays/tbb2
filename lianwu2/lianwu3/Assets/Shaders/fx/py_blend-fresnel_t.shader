Shader "FxShader_py/py_blend-fresnel_t" {
    Properties {
        _tex01RGB ("tex01(RGB)", 2D) = "white" {}
        _color_strong ("color_strong", Float ) = 1
        _colorRGBA ("color(RGBA)", Color) = (0.5,0.5,0.5,0)
        _fre_width ("fre_width", Float ) = 5
        _fre_strong ("fre_strong", Float ) = 2
        _alpha1 ("(alpha+1)", Float ) = 1
        _tex01_panner_u ("tex01_panner_u", Float ) = 0
        _tex01_panner_v ("tex01_panner_v", Float ) = 0
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
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _tex01RGB; uniform float4 _tex01RGB_ST;
            uniform float _color_strong;
            uniform float4 _colorRGBA;
            uniform float _fre_width;
            uniform float _fre_strong;
            uniform float _alpha1;
            uniform float _tex01_panner_u;
            uniform float _tex01_panner_v;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = mul(_Object2World, float4(v.normal,0)).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);

                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;

                float4 node_627 = _Time + _TimeEditor;
                float2 node_6897 = (i.uv0+(node_627.g*float2(_tex01_panner_u,_tex01_panner_v))*float2(1,1));
                float4 _tex01RGB_var = tex2D(_tex01RGB,TRANSFORM_TEX(node_6897, _tex01RGB));
                float3 node_4130 = ((_color_strong*_tex01RGB_var.rgb)+(_tex01RGB_var.rgb*(pow(1.0-max(0,dot(normalDirection, viewDirection)),_fre_width)*_fre_strong)));
                float3 emissive = ((_colorRGBA.rgb*node_4130)*i.vertexColor.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,saturate(((i.vertexColor.a*_colorRGBA.a)*(dot(node_4130,float3(0.3,0.59,0.11))+_alpha1))));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
