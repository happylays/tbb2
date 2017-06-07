// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

//Version=1
Shader"UNOShader/_Library/Helpers/Xray"
{
	Properties
	{
		_XrayTex ("Xray Texture", 2D) = "white" {}		
		_XrayColor ("Xray Color", Color) = (1,1,1,1)
		_XrayIntensity ("Xray Intensity", Range (1, 10)) = 1
		_XrayEdgeFresnel ("Xray Edge Fresnel", Range (0, 1)) = 1
		//--- Shader Features 
		[HideInInspector] _XRAYEDGE("Xray Edge", Float) = 0.0
	}
	SubShader
	{
		Tags
		{
			//--- "RenderType" sets the group that it belongs to type and uses: Opaque, Transparent,
			//--- TransparentCutout, Background, Overlay(Gui,halo,Flare shaders), TreeOpaque, TreeTransparentCutout, TreeBilboard,Grass, GrassBilboard.
			//--- "Queue" sets order and uses: Background (for skyboxes), Geometry(default), AlphaTest(?, water),
			//--- Transparent(draws after AlphaTest, back to front order), Overlay(effects,ie lens flares)
			//--- adding +number to tags "Geometry +1" will affect draw order. B=1000 G=2000 AT= 2450 T=3000 O=4000
			
//			"RenderType" = "Opaque"
//			"Queue" = "Geometry"
//			
//			"RenderType" = "Transparent"
//			"Queue" = "Transparent"
		//	"LightMode" = "ForwardBase"
		}
		Pass
		{
            Name "BASE"
            Tags
			{
			//--- "RenderType" sets the group that it belongs to type and uses: Opaque, Transparent,
			//--- TransparentCutout, Background, Overlay(Gui,halo,Flare shaders), TreeOpaque, TreeTransparentCutout, TreeBilboard,Grass, GrassBilboard.
			//--- "Queue" sets order and uses: Background (for skyboxes), Geometry(default), AlphaTest(?, water),
			//--- Transparent(draws after AlphaTest, back to front order), Overlay(effects,ie lens flares)
			//--- adding +number to tags "Geometry +1" will affect draw order. B=1000 G=2000 AT= 2450 T=3000 O=4000
//			"RenderType" = "Transparent"
//			"Queue" = "Transparent"
			//"LightMode" = "ForwardBase"
			}
            //Cull Front           	           	 
           	Blend SrcAlpha OneMinusSrcAlpha //-- transparency enable
           	Zwrite Off
           	Ztest Always
           	           
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag  
            #pragma multi_compile XRAYEDGE_NONE XRAYEDGE_NORMAL XRAYEDGE_INVERTED
 											
 			fixed4 _XrayColor;
 			fixed _XrayIntensity;
 			fixed _XrayEdgeFresnel;
 			 		 		 		
           	struct customData
			{
				half4 vertex : POSITION;
				half3 normal : NORMAL;
				half4 tangent : TANGENT;
				fixed2 texcoord : TEXCOORD0;
			};
            struct v2f
            {
        		half4 pos : SV_POSITION;								
				fixed4 color : TEXCOORD1;
				half4 posWorld : TEXCOORD2;//position of vertex in world;
				half4 normalDir : TEXCOORD3;//vertex Normal Direction in world space
            };
           
            v2f vert(customData v)
            {
                v2f o;
                o.normalDir = fixed4 (0,0,0,0);				
				o.posWorld = fixed4 (0,0,0,0);
				o.normalDir.xyz = UnityObjectToWorldNormal(v.normal);
				o.posWorld.xyz = mul(unity_ObjectToWorld, v.vertex);
			//--- Vectors
				half3 normalDirection = normalize(half3( mul(half4(v.normal, 0.0), unity_WorldToObject).xyz ));
				half3 lightDirection = normalize(half3(_WorldSpaceLightPos0.xyz));
				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - o.posWorld.xyz);// world space
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);//UNITY_MATRIX_MVP is a matrix that will convert a model's vertex position to the projection space

                o.color = fixed4(_XrayColor.rgb * _XrayIntensity,_XrayColor.a);

                fixed edgeTransparency = _XrayColor.a;
				fixed fresnel = dot(viewDirection, o.normalDir.xyz);
				#if XRAYEDGE_NORMAL
					edgeTransparency = pow(clamp(fresnel,0,1),_XrayEdgeFresnel)* _XrayColor.a;
				#endif
				#if XRAYEDGE_INVERTED
					edgeTransparency =  pow(clamp((1-fresnel),0,1),_XrayEdgeFresnel)* _XrayColor.a;
				#endif
				o.color.a *=  edgeTransparency;

                return o;
            }
           
            half4 frag(v2f i) :COLOR
            {          
            	fixed4 result = fixed4(0,0,0,0);  	            	
        		result = i.color;
                return result;				
            }
                   
            ENDCG
        }// ------------------- Pass ---------------------------------------				
	} //-------------------------------SubShader-------------------------------
	//Fallback "VertexLit" // for shadows
}
