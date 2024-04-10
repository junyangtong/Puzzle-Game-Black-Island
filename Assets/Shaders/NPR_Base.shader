Shader "NPR/NPR_Base"
{
    Properties 
    {
        _DiffStep       ("明暗交界线阈值",Range(0,1)) = 0.5
        _BaseColor1     ("暗面颜色", Color) = (0,0,0,0)
        _BaseColor2     ("亮面颜色", Color) = (1,1,1,1)
        [HDR]_EmissCol  ("自发光颜色", Color) = (0,0,0,0)
        [Header(Texture)]
        _MainMap        ("颜色贴图", 2D) = "white" {}
        _AOMap          ("环境光遮蔽", 2D)="white"{}
        _NormalMap      ("法线贴图", 2D)="bump"{}
        _EmissMap      ("自发光贴图", 2D)="white"{}
        [Toggle(_AdditionalLights)] _AddLights ("AddLights", Float) = 1
        [Header(Step)]
        [Toggle] _T1        ("地面?显示脚印?", Float) = 0
        _RippleColor        ("脚印颜色", Color) = (1, 1, 1, 1) 

    }
    SubShader {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue"="Geometry"
        }
        Pass {
            Name "Stone"
            Tags 
            {
                "LightMode" = "UniversalForward"
            }
            
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ Anti_Aliasing_ON
            #pragma multi_compile _ LIGHTMAP_ON //光照贴图支持
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED //方向贴图
            #pragma shader_feature _AdditionalLights
            #pragma shader_feature _T1_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            // 获取参数
            CBUFFER_START(UnityPerMaterial)
            float _DiffStep;
            float4 _BaseColor1;
            float4 _BaseColor2;
            float4 _EmissCol;
            #ifdef _T1_ON
                float3 _Position;
                float _OrthographicCamSize;
                float4 _RippleColor;
            #endif
            
            CBUFFER_END

            TEXTURE2D(_MainMap);
            SAMPLER(sampler_MainMap);
            float4 _MainMap_ST;
            TEXTURE2D(_AOMap);
            SAMPLER(sampler_AOMap);
            TEXTURE2D(_WarpTex);
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            float4 _NormalMap_ST;
            TEXTURE2D(_EmissMap);
            SAMPLER(sampler_EmissMap);
            #ifdef _T1_ON
                TEXTURE2D(_GlobalRipplesRT);
                SAMPLER(sampler_GlobalRipplesRT);
            #endif
            //输入结构
            struct VertexInput 
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent  : TANGENT;
                float2 staticLightmapUV : TEXCOORD1;
            };
            // 输出结构
            struct VertexOutput 
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 posWS :TEXCOOND1;
                float3 tDirWS : TEXCOORD2;
                float3 bDirWS : TEXCOORD3; 
                float3 nDirWS : NORMAL_WS;  
                float4 shadowCoord : TEXCOORD4;
                float2 staticLightmapUV : TEXCOORD5;
            };
           // 输出结构>>>顶点Shader>>>输出结构
            VertexOutput vert (VertexInput v) 
            {
                VertexOutput o = (VertexOutput)0;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainMap);
                o.posWS = mul(unity_ObjectToWorld, v.vertex);
                o.nDirWS = TransformObjectToWorldNormal(v.normal);
                o.tDirWS = TransformObjectToWorldDir(v.tangent.xyz);
                o.bDirWS = cross(o.nDirWS, o.tDirWS) * v.tangent.w * unity_WorldTransformParams.w;
                o.shadowCoord = TransformWorldToShadowCoord(o.posWS.xyz);
                o.staticLightmapUV = v.staticLightmapUV * unity_LightmapST.xy + unity_LightmapST.zw;
                return o;
            }
            // 片段着色器
            float4 frag(VertexOutput i) : COLOR {
                float3 finalRGB = 0.0;
                // 向量准备
                Light mainLight = GetMainLight(i.shadowCoord);                                      // 获取主光源数据
                float shadow = MainLightRealtimeShadow(i.shadowCoord);
                float3 nDirTS = normalize(UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, i.uv * _NormalMap_ST.xy + _NormalMap_ST.zw)));
                float3x3 TBN = float3x3(i.tDirWS, i.bDirWS, i.nDirWS);                            // 计算TBN矩阵                        
                float3 nDirWS = normalize(mul(nDirTS, TBN));
                float3 lDir = normalize(mainLight.direction);
                float3 vDirWS = normalize (_WorldSpaceCameraPos.xyz - i.posWS.rgb);                 //视线方向                   
                float3 hDir = normalize (vDirWS + lDir);
                // 准备中间数据
                float nl = saturate(dot(nDirWS, lDir));

                // 提取信息
                float aoCol = SAMPLE_TEXTURE2D(_AOMap, sampler_AOMap, i.uv * _MainMap_ST.xy + _MainMap_ST.zw).r;
                float3 mainTex = SAMPLE_TEXTURE2D(_MainMap, sampler_MainMap, i.uv).rgb;
                float emissMap = SAMPLE_TEXTURE2D(_EmissMap, sampler_EmissMap, i.uv).r;
                // 光照计算
                    // 漫反射颜色
                    float diffMask = min(smoothstep(_DiffStep - 0.05, _DiffStep + 0.05, nl), shadow);
                    float3 diffCol =  lerp(mainTex * _BaseColor1.rgb , mainTex * _BaseColor2.rgb, diffMask) * aoCol;
                    finalRGB += diffCol * mainLight.color.rgb;
                //脚步交互
                float3 stepCol = 0.0;
                #ifdef _T1_ON
                    float2 RTuv = i.posWS.xz - _Position.xz;                                                // 像素点相对于相机中心的距离
                    RTuv = RTuv / (_OrthographicCamSize * 2);                                               // 转为 -0.5~0.5
                    RTuv += 0.5; // 转为 0~1
                    float ripples = SAMPLE_TEXTURE2D(_GlobalRipplesRT, sampler_GlobalRipplesRT,saturate(RTuv)).b;  //采样RenderTexture
                    ripples = step(2, ripples * 3);
                    stepCol = ripples * _RippleColor.rgb;
                #else
                #endif

                    // 烘焙光照
                    half3 bakeGI = half3(0,0,0);
                    #if defined(LIGHTMAP_ON)
                        float4 encodedIrradiance = SAMPLE_TEXTURE2D(unity_Lightmap,samplerunity_Lightmap,i.staticLightmapUV);               // unity_Lightmap是内置LightMap，unity_LightmapInd是内置的方向贴图名称
                        bakeGI = DecodeLightmap(encodedIrradiance, float4(LIGHTMAP_HDR_MULTIPLIER, LIGHTMAP_HDR_EXPONENT, 0.0h, 0.0h));
                        #if defined(DIRLIGHTMAP_COMBINED)
                        float4 direction = SAMPLE_TEXTURE2D(unity_LightmapInd,samplerunity_Lightmap,i.staticLightmapUV);
                        half3 LightDir = direction.xyz * 2.0f - 1.0f;
                        half lambert = saturate(dot(nDirWS,LightDir));
                        bakeGI = bakeGI * lambert / max(1e-4,direction.w);
                        #endif
                    #else
                        bakeGI = SampleSH(nDirWS);// SH,Light Probe
                    #endif
                    finalRGB += bakeGI;

                    //自发光
                    float3 emiss = emissMap * _EmissCol.rgb;
                    finalRGB += emiss;

                    //其它光源计算
                    #ifdef _AdditionalLights 
                        int pixelLightCount = GetAdditionalLightsCount();               // 获取副光源个数
                        for(int index = 0; index < pixelLightCount; index++)
                        {
                            Light additionLight = GetAdditionalLight(index, i.posWS.xyz);  // 获取其他光照的世界空间位置
                            half3 addLDir = normalize(additionLight.direction);
                            half addLightNl = saturate(dot(nDirWS, addLDir));
                            half3 addLightDiffCol = additionLight.color * ((additionLight.distanceAttenuation * additionLight.shadowAttenuation) * addLightNl);
                            finalRGB += addLightDiffCol;
                        }
                    #endif
                // finalRGB
                return float4(finalRGB + stepCol, 1.0);
            }
            ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            // -------------------------------------
            // Universal Pipeline keywords

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            // This is used during shadow map generation to differentiate between directional and punctual light shadows, as they use different formulas to apply Normal Bias
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }
        // This pass is used when drawing to a _CameraNormalsTexture texture
        Pass
        {
            Name "DepthNormals"
            Tags{"LightMode" = "DepthNormals"}

            ZWrite On
            Cull[_Cull]

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            #pragma vertex DepthNormalsVertex
            #pragma fragment DepthNormalsFragment

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _PARALLAXMAP
            #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
            // Universal Pipeline keywords
            #pragma multi_compile_fragment _ _WRITE_RENDERING_LAYERS

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitDepthNormalsPass.hlsl"
            ENDHLSL
        }
        // This pass it not used during regular rendering, only for lightmap baking.
        Pass
        {
            Name "Meta"
            Tags{"LightMode" = "Meta"}

            Cull Off

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            #pragma vertex UniversalVertexMeta
            #pragma fragment UniversalFragmentMetaLit

            #pragma shader_feature EDITOR_VISUALIZATION
            #pragma shader_feature_local_fragment _SPECULAR_SETUP
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local _ _DETAIL_MULX2 _DETAIL_SCALED

            #pragma shader_feature_local_fragment _SPECGLOSSMAP

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Assets/Shaders/Include/NPRMetaPass.hlsl"

            ENDHLSL
        }
        
    }
    }