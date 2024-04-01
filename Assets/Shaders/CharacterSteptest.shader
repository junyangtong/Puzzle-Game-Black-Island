Shader "MyShader/Ground"
{
    Properties
    {
        _MainTex            ("Texture", 2D) = "white" {}
        _RippleColor        ("脚步颜色", Color) = (1, 1, 1, 1) 
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
			"RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            Name "Pass"
            HLSLPROGRAM
            #pragma fragment frag
            #pragma vertex vert
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 4.6
            // Includes
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            CBUFFER_START(UnityPerMaterial)
            float3 _Position;
            float _OrthographicCamSize;
            float4 _RippleColor;
            float4 _MainTex_ST;
            CBUFFER_END
            TEXTURE2D(_GlobalRipplesRT);
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            // 贴图采样器
            SamplerState smp_Point_Repeat;
            // 顶点着色器的输入
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 tangent  : TANGENT;
            };

            // 片段着色器的输入
            struct v2f
            {
                float4 color : COLOR;
                float3 nDirWS : NORMAL_WS;
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 posWS:TEXCOORD1;
                float4 scrPos	 :TEXCOORD2;  
                float3 tDirWS : TEXCOORD4;
                float3 bDirWS : TEXCOORD5;
            };

            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.posWS = TransformObjectToWorld(v.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                //基础颜色
                float3 basecol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex,i.uv).rgb;  //采样RenderTexture
                //脚步交互
                float2 RTuv = i.posWS.xz - _Position.xz;                                                // 像素点相对于相机中心的距离
                RTuv = RTuv / (_OrthographicCamSize * 2);                                               // 转为 -0.5~0.5
                RTuv += 0.5; // 转为 0~1
                float ripples = SAMPLE_TEXTURE2D(_GlobalRipplesRT, smp_Point_Repeat,saturate(RTuv)).b;  //采样RenderTexture
                ripples = step(2, ripples * 3);
                float3 ripplesCol = ripples * _RippleColor.rgb;

                return float4(ripplesCol + basecol,1.0);
            }
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
    }
}
