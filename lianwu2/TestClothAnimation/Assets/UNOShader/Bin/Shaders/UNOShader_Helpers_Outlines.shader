//Version=1
Shader"UNOShader/_Library/Helpers/Outlines"
{
	Properties
	{
		_OutlineTex ("Outline Texture", 2D) = "white" {}
		_Outline ("Outline Width", Range (0, 1)) = .1
		_OutlineX ("Outline X", Range (0, 1)) = .1
		_OutlineY ("Outline Y", Range (0, 1)) = .1
		_OutlineColor ("Outline Color", Color) = (1,1,1,1)
		_OutlineIntensity ("Outline Emission", Range (1, 10)) = 1

		//--------------------- Outline Shader Features  --------------------------------
		[HideInInspector] _ORTHO("Orthographic support", Float) = 0.0
	 	[HideInInspector] _OUTLINE("Outline setting", Float) = 0.0
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
		//Ztest Always
		Pass// Pass drawing outline
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
            Cull Front
           	Blend SrcAlpha OneMinusSrcAlpha //-- transparency enable                	           	
//           	ZTest (Less | Greater | LEqual | GEqual | Equal | NotEqual | Always)
//           	Zwrite Off
           	//Ztest Always
           	
           
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile ORTHO_OFF ORTHO_ON
            #pragma multi_compile OUTLINE_BASIC OUTLINE_ADVANCED  
                      
 								
			fixed _Outline;			
 			fixed4 _OutlineColor;
 			fixed _OutlineIntensity;
 			
 			#ifdef OUTLINE_ADVANCED
				sampler2D _OutlineTex;
				float4 _OutlineTex_ST;			
				fixed _OutlineX;
				fixed _OutlineY;
			#endif
 			
           	struct customData
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				#ifdef OUTLINE_ADVANCED
					float2 texcoord : TEXCOORD0;
					float4 texcoord1 : TEXCOORD1;
				#endif
				UNITY_FOG_COORDS(5)
			};
            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
             	#ifdef OUTLINE_ADVANCED
            		float2 uv : TEXCOORD0;
                #endif
                half4 normalDir : TEXCOORD3;//vertex Normal Direction in world space
            };
           
            v2f vert(customData v)
            {           
                v2f o;
                o.normalDir = fixed4 (0,0,0,0);				
				o.normalDir.xyz = UnityObjectToWorldNormal(v.normal);
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                float3 norm   = normalize(mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                float2 offset = TransformViewToProjection(norm.xy);
                float ortho = .1;
                float persp = 1;               
                #ifdef OUTLINE_BASIC
	                #ifdef ORTHO_OFF
	                	o.pos.xy += offset  * o.pos.z * _Outline / persp;
	                #endif
	                #ifdef ORTHO_ON
	                	o.pos.xy += offset * _Outline / ortho;// fix for othographic                
	            	#endif
            	#endif

                #ifdef OUTLINE_ADVANCED
	                o.uv =		TRANSFORM_TEX (v.texcoord, _OutlineTex); // this allows you to offset uvs and such                               
	                #ifdef ORTHO_OFF		                
		                o.pos.x += offset.x  * o.pos.z * _OutlineX / persp;
		                o.pos.y += offset.y  * o.pos.z * _OutlineY / persp;
		            #endif
		            #ifdef ORTHO_ON
	                	o.pos.x += offset.x  * _OutlineX / ortho;// fix for orthographic                               
	                	o.pos.y += offset.y  * _OutlineY / ortho;// fix for orthographic
	            	#endif
	            #endif
                o.color = _OutlineColor;    
                UNITY_TRANSFER_FOG(o,o.pos);                                         
                return o;
            }
           
            half4 frag(v2f i) :COLOR
            {          
            	fixed4 result = fixed4(0,0,0,0);  	
            	#ifdef OUTLINE_BASIC
            		result = fixed4(i.color.rgb * _OutlineIntensity,i.color.a);
        		#endif
        		#ifdef OUTLINE_ADVANCED
		        	fixed4 T_Outline = tex2D(_OutlineTex, i.uv);
					result = fixed4(i.color.rgb * T_Outline.rgb * _OutlineIntensity,i.color.a * T_Outline.a);
				#endif
                return result;				
            }
                   
            ENDCG
        }// ------------------- Pass ---------------------------------------
		
		
		Pass// Pass drawing outline
		{
            Name "BASIC"
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
            Cull Front            
           	Blend SrcAlpha OneMinusSrcAlpha //-- transparency enable                	
//           	ZTest (Less | Greater | LEqual | GEqual | Equal | NotEqual | Always)
//           	Zwrite Off
           	
           	
           
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile ORTHO_OFF ORTHO_ON
            #pragma multi_compile OUTLINE_BASIC OUTLINE_ADVANCED            
 								
			fixed _Outline;			
 			fixed4 _OutlineColor;
 			fixed _OutlineIntensity;
 			
 			#ifdef OUTLINE_ADVANCED
				sampler2D _OutlineTex;
				float4 _OutlineTex_ST;			
				fixed _OutlineX;
				fixed _OutlineY;
			#endif
 			
           	struct customData
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				#ifdef OUTLINE_ADVANCED
					float2 texcoord : TEXCOORD0;
					float4 texcoord1 : TEXCOORD1;
				#endif
			};
            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
             	#ifdef OUTLINE_ADVANCED
            	float2 uv : TEXCOORD0;
                #endif
            };
           
            v2f vert(customData v)
            {
                v2f o;
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                float2 offset = TransformViewToProjection(norm.xy);
                float ortho = .1;
                float persp = 1;               
                #ifdef OUTLINE_BASIC
	                #ifdef ORTHO_OFF
	                	o.pos.xy += offset  * o.pos.z * _Outline / persp;
	                #endif
	                #ifdef ORTHO_ON
	                	o.pos.xy += offset * _Outline / ortho;// fix for othographic                
	            	#endif
            	#endif

                #ifdef OUTLINE_ADVANCED
	                o.uv = TRANSFORM_TEX (v.texcoord, _OutlineTex); // this allows you to offset uvs and such                               
	                #ifdef ORTHO_OFF		                
		                o.pos.x += offset.x  * o.pos.z * _OutlineX / persp;
		                o.pos.y += offset.y  * o.pos.z * _OutlineY / persp;
		            #endif
		            #ifdef ORTHO_ON
	                	o.pos.x += offset.x  * _OutlineX / ortho;// fix for orthographic                               
	                	o.pos.y += offset.y  * _OutlineY / ortho;// fix for orthographic
	            	#endif
	            #endif
                o.color = _OutlineColor;                                             
                return o;
            }
           
            half4 frag(v2f i) :COLOR
            {          
            	fixed4 result = fixed4(0,0,0,0);  	
            	#ifdef OUTLINE_BASIC
            		result = fixed4(i.color.rgb * _OutlineIntensity,i.color.a);
        		#endif
        		#ifdef OUTLINE_ADVANCED
		        	fixed4 T_Outline = tex2D(_OutlineTex, i.uv);
					result = fixed4(i.color.rgb * T_Outline.rgb * _OutlineIntensity,i.color.a * T_Outline.a);
				#endif
                return result;				
            }
                   
            ENDCG
        }// ------------------- Pass ---------------------------------------
        Pass// Pass drawing outline
		{
            Name "ADVANCED"
            Tags
			{
			//--- "RenderType" sets the group that it belongs to type and uses: Opaque, Transparent,
			//--- TransparentCutout, Background, Overlay(Gui,halo,Flare shaders), TreeOpaque, TreeTransparentCutout, TreeBilboard,Grass, GrassBilboard.
			//--- "Queue" sets order and uses: Background (for skyboxes), Geometry(default), AlphaTest(?, water),
			//--- Transparent(draws after AlphaTest, back to front order), Overlay(effects,ie lens flares)
			//--- adding +number to tags "Geometry +1" will affect draw order. B=1000 G=2000 AT= 2450 T=3000 O=4000
			//"RenderType" = "Opaque"
			//"Queue" = "Transparent"
			//"LightMode" = "ForwardBase"
			}
            
            Cull Front
           	Blend SrcAlpha OneMinusSrcAlpha //-- transparency enable
//           	Zwrite Off
           	//Offset 10,0
           
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile ORTHO_OFF ORTHO_ON
 			
 			sampler2D _OutlineTex;
			float4 _OutlineTex_ST;
			fixed _OutlineX;
			fixed _OutlineY;
 			fixed4 _OutlineColor;
 			fixed _OutlineIntensity;
           	struct customData
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
			};
            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
           
            v2f vert(customData v)
            {
                v2f o;
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                o.uv =		TRANSFORM_TEX (v.texcoord, _OutlineTex); // this allows you to offset uvs and such
                float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                float2 offset = TransformViewToProjection(norm.xy);
                #ifdef ORTHO_OFF	                
	                o.pos.x += offset.x  * o.pos.z * _OutlineX;
	                o.pos.y += offset.y  * o.pos.z * _OutlineY;
	            #endif
	            #ifdef ORTHO_ON
                	o.pos.x += offset.x  * _OutlineX * 10;// fix for orthographic                               
                	o.pos.y += offset.y  * _OutlineY * 10;// fix for orthographic
            	#endif
                
                o.color = _OutlineColor;
                return o;
                                                           
            }
           
            half4 frag(v2f i) :COLOR
            {
            	fixed4 result = fixed4(0,0,0,0);  
                fixed4 T_Outline = tex2D(_OutlineTex, i.uv);
				result = fixed4(i.color.rgb * T_Outline.rgb * _OutlineIntensity,i.color.a * T_Outline.a);
                return result;
            }
                   
            ENDCG
        }// ------------------- Pass ---------------------------------------
	} //-------------------------------SubShader-------------------------------
	//Fallback "VertexLit" // for shadows
}
