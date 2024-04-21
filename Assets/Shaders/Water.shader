Shader "Island/Water"
{
    Properties
    {
        [Header(Texture)]
        _NormalMap          ("法线贴图",2D)    = "bump" {}
        _WarpMap            ("扰动图",2D)    = "bump" {}
        _FoamTex            ("岸边泡沫贴图",2D) = "White"{}
        _RainTex            ("雨天涟漪",2D) = "White"{}
        [Header(Depth)]
        _DepthRange         ("深度范围",float) = 1
        _AlphaRange         ("透明范围",float) = 1
        _WaterShallowColr   ("浅水区颜色", Color) = (1,1,1,1)
		_WaterDeepColr      ("深水区颜色", Color) = (1,1,1,1)
        [Header(Tessellation)]
        _Tess               ("细分程度", Range(1, 32)) = 20
        _MaxTessDistance    ("细分最大距离", Range(1, 320)) = 20
        _MinTessDistance    ("细分最小距离", Range(1, 320)) = 1
        [Header(Genstner)]
        _Steepness          ("重力", Range(0,5)) = 0.8
        _Amplitude          ("振幅",Range(0,1)) = 1
        _WaveLength         ("波长",Range(0,5)) = 1
        _WindSpeed          ("风速",Range(0,1)) = 1
        _WindDir            ("风向", Range(0,360)) = 0
        [Header(Reflection)]
        _ReflectColor       ("反射颜色",Color) = (1,1,1,1)
        [Header(Fresnel)]
        _FresnelScale       ("菲尼尔范围", Range(0,20)) = 15
        _FresnelCol         ("菲尼尔颜色", Color) = (1,1,1,1)
        [Header(Fefraction)]
        _FlowSpeed          ("流动速度", Range(0,5)) = 1
        _WarpInt            ("扭曲强度", Range(0,1)) = 0.3
        [Header(Normal)]
        _NormalIntensity    ("法线扰动强度",Range(0,1)) = 0.1
        _NormalScale        ("法线缩放",Range(0,80)) = 40
        _WaveXSpeed         ("x轴速度",Range(-1,1)) = 1
        _WaveYSpeed         ("y轴速度",Range(-1,1)) = -1
        [Header(Specular)]
        _SpecularColor      ("高光颜色",Color) = (1,1,1,1)
		_SpecularRange      ("高光范围",Range(0.1,200)) = 200
		_SpecularStrenght   ("高光强度",Range(0.1,4)) = 1
        [Header(Rain)]
        _RainInt            ("雨天涟漪强度", Range(0,1)) = 1
        [Header(Other)]
        _Foam               ("岸边泡沫  x:拉伸 y:速度 z:范围",Vector) = (1.0,1.0,1.0,1.0)
        _RippleColor        ("交互水波颜色", Color) = (1, 1, 1, 1) 
        _Diffcol            ("漫反射颜色整体调整", Color) = (1, 1, 1, 1) 
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
			"RenderType"="Opaque"
			"Queue"="Transparent"
			"DisableBatching"="True"
        }
        
        Pass
        {
            Name "Pass"
            // Render State
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On

            HLSLPROGRAM

            #pragma require tessellation
            #pragma require geometry
            
            #pragma vertex BeforeTessVertProgram
            #pragma hull HullProgram
            #pragma domain DomainProgram
            #pragma fragment FragmentProgram

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 4.6

            // Includes
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
            float _MetallicInt;
            float _Smoothness;
			float _DepthRange;
			float _AlphaRange;
            float4 _WaterShallowColr;
            float4 _WaterDeepColr;
            //sampler2D _CameraDepthTexture;
            float _offDepth;
            float _NormalIntensity;
            float _NormalScale;
            float _WaveYSpeed;
            float _WaveXSpeed;

            float _Tess;
            float _MaxTessDistance;
            float _MinTessDistance;
            float _Steepness;
            float _Amplitude;
            float _WaveLength;
            float _WindSpeed;
            float _WindDir;
            float4 _ReflectColor;
            float _FresnelScale;
            float _FlowSpeed;
            float _WarpInt;
            float4 _FresnelCol;

            float4 _SpecularColor;
			float _SpecularRange;
			float _SpecularStrenght;
			float4 _Foam;
            float _RainInt;

            float3 _Position;
            float _OrthographicCamSize;
            float4 _RippleColor;
            float4 _Diffcol;
            
            CBUFFER_END

			TEXTURE2D_X_FLOAT(_CameraDepthTexture);
            TEXTURE2D(_NormalMap);
            TEXTURE2D(_WarpMap);
            float4 _WarpMap_ST;
            TEXTURE2D(_ReflectionTex);
            TEXTURE2D(_FoamTex) ;
            float4 _FoamTex_ST;
            TEXTURE2D(_RainTex) ;
			float4 _RainTex_ST;
            TEXTURE2D(_GlobalRipplesRT);
            SAMPLER(sampler_GlobalRipplesRT);
            TEXTURE2D(_CameraOpaqueTexture);
            // 贴图采样器
            SamplerState smp_Point_Repeat;

            // 顶点着色器的输入
            struct Attributes
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 tangent  : TANGENT;
            };

            // 片段着色器的输入
            struct Varyings
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

            // 内部因素使用SV_InsideTessFactor语义
            struct TessellationFactors
            {
                float edge[3] : SV_TessFactor;
                float inside : SV_InsideTessFactor;
            };

            // 该结构的其余部分与Attributes相同，只是使用INTERNALTESSPOS代替POSITION语意，否则编译器会报位置语义的重用
            struct ControlPoint
            {
                float4 vertex : INTERNALTESSPOS;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float3 normal : NORMAL;
                float4 tangent  : TANGENT;
            };

            // 顶点着色器，此时只是将Attributes里的数据递交给曲面细分阶段
            ControlPoint BeforeTessVertProgram(Attributes v)
            {
                ControlPoint p;
        
                p.vertex = v.vertex;
                p.uv = v.uv;
                p.normal = v.normal;
                p.color = v.color;
                p.tangent = v.tangent;
        
                return p;
            }

            // 随着距相机的距离减少细分数
            float CalcDistanceTessFactor(float4 vertex, float minDist, float maxDist, float tess)
            {
                float3 worldPosition = TransformObjectToWorld(vertex.xyz);
                float dist = distance(worldPosition,  GetCameraPositionWS());
                float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
                return (f);
            }
            // 根据其距离相机的位置来设置细分因子
            TessellationFactors MyPatchConstantFunction(InputPatch<ControlPoint, 3> patch)
            {
                float minDist = _MinTessDistance;
                float maxDist = _MaxTessDistance;
            
                TessellationFactors f;
            
                float edge0 = CalcDistanceTessFactor(patch[0].vertex, minDist, maxDist, _Tess);
                float edge1 = CalcDistanceTessFactor(patch[1].vertex, minDist, maxDist, _Tess);
                float edge2 = CalcDistanceTessFactor(patch[2].vertex, minDist, maxDist, _Tess);
            
                // make sure there are no gaps between different tessellated distances, by averaging the edges out.
                f.edge[0] = (edge1 + edge2) / 2;
                f.edge[1] = (edge2 + edge0) / 2;
                f.edge[2] = (edge0 + edge1) / 2;
                f.inside = (edge0 + edge1 + edge2) / 3;
                return f;
            }

            //细分阶段
            [domain("tri")]//明确地告诉编译器正在处理三角形，其他选项：
            [outputcontrolpoints(3)]//明确地告诉编译器每个补丁输出三个控制点
            [outputtopology("triangle_cw")]//当GPU创建新三角形时，它需要知道我们是否要按顺时针或逆时针定义它们
            [partitioning("integer")]//告知GPU应该如何分割补丁，现在，仅使用整数模式
            [patchconstantfunc("MyPatchConstantFunction")]//GPU还必须知道应将补丁切成多少部分。每个补丁不同。必须提供一个补丁函数（Patch Constant Functions）
            [maxtessfactor(64.0f)] 
            ControlPoint HullProgram(InputPatch<ControlPoint, 3> patch, uint id : SV_OutputControlPointID)
            {
                return patch[id];
            }

            struct Wave{
            float3 wavePos;
            float3 waveNormal;
            };
            Wave GerstnerWave(float2 posXZ, float amp, float waveLen, float speed, int dir) // 传入的是每一个波形的效果，最后叠加，然后由UI参数统一调控。
            {
                Wave o;
                float w = 2*PI / (waveLen * _WaveLength); 
                float A = amp * _Amplitude;
                float WA = w * A;
                float Q = _Steepness / (WA * 6);
                float dirRad = radians((dir + _WindDir) % 360);
                float2 D = normalize(float2(sin(dirRad), cos(dirRad)));
                float common = w * dot(D, posXZ) + _Time.y * sqrt(9.8 * w) * speed * _WindSpeed;
                float sinC = sin(common);
                float cosC = cos(common);
                o.wavePos.xz = Q * A * D.xy * cosC;
                o.wavePos.y = A * sinC / 6;
                return o;
            }
            float4 blendSeaColor(float4 col1,float4 col2)
			{
				float4 col = min(1,1.5-col2.a) * col1+col2.a * col2;
				return col;
			}
			Varyings AfterTessVertProgram (Attributes v)
			{
                float Amplitude[6] = {1.8, 0.8, 0.5, 0.3, 0.1, 0.08};
                float WaveLen[6] = {0.541, 0.6, 0.2, 0.3, 0.1, 0.3};
                float WindSpeed[6] = {0.305, 0.5, 0.34, 0.12, 0.64, 0.11};
                int WindDir[6] = {11, 90, 166, 300, 10, 180};
				Varyings o;
                float3 waveOffset = float3(0.0, 0.0, 0.0);
                for(int i = 0; i < 6; i++)
                {
                    Wave wave = GerstnerWave(v.vertex.xz, Amplitude[i], WaveLen[i], WindSpeed[i], WindDir[i]);
                    waveOffset += wave.wavePos;                    
                }
                v.vertex.xyz += waveOffset;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.nDirWS = TransformObjectToWorldNormal(v.normal);
                o.tDirWS = TransformObjectToWorldDir(v.tangent.xyz);
                o.bDirWS = cross(o.nDirWS, o.tDirWS) * v.tangent.w;
				o.uv = v.uv;
				o.posWS = TransformObjectToWorld(v.vertex.xyz);
                o.scrPos = ComputeScreenPos(o.vertex);
				o.color = v.color;
                return o;
			}

            //生成最终的顶点数据。
            [domain("tri")]//Hull着色器和Domain着色器都作用于相同的域，即三角形。我们通过domain属性再次发出信号
            Varyings DomainProgram(TessellationFactors factors, OutputPatch<ControlPoint, 3> patch, float3 barycentricCoordinates : SV_DomainLocation)
            {
                Attributes v;
                //以相同的方式插值所有顶点数据
                #define DomainInterpolate(fieldName) v.fieldName = \
                        patch[0].fieldName * barycentricCoordinates.x + \
                        patch[1].fieldName * barycentricCoordinates.y + \
                        patch[2].fieldName * barycentricCoordinates.z;
    
                    //对位置、颜色、UV、法线等进行插值
                    DomainInterpolate(vertex)
                    DomainInterpolate(uv)
                    DomainInterpolate(color)
                    DomainInterpolate(normal)
                    DomainInterpolate(tangent)
                    
                    //该顶点将在此阶段之后发送到几何程序或插值器
                    return AfterTessVertProgram(v);
            }
            
            // 片段着色器
            float4 FragmentProgram(Varyings i) : SV_TARGET 
            {   
                //准备向量
                Light light = GetMainLight();//获取主光源数据
                float3 lDir = normalize(light.direction);
                float3 vDirWS = normalize(_WorldSpaceCameraPos.xyz - i.posWS);    
                float3 nDirTS1 = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap,smp_Point_Repeat,i.uv * _NormalScale + float2(_WaveXSpeed * _Time.x, _WaveYSpeed * _Time.x)));
                float3 nDirTS2 = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap,smp_Point_Repeat,i.uv * _NormalScale - float2(_WaveXSpeed * _Time.x, _WaveYSpeed * _Time.x)));
                float3 nDirTS = normalize(nDirTS1 + nDirTS2);
                float normalIntensity = dot(i.nDirWS, vDirWS) * _NormalIntensity;
                nDirTS.xy *= normalIntensity;
                nDirTS.z = sqrt(1 - saturate(dot(nDirTS.xy, nDirTS.xy)));
                float3x3 TBN = float3x3(i.tDirWS,i.bDirWS,i.nDirWS);
                float3 nDirWS = normalize(mul(nDirTS,TBN));
                float3 hDir = normalize(lDir + vDirWS);  //半角向量
                float3 vrDirWS = normalize(reflect(-vDirWS,nDirWS));   
                float2 screenPos= i.scrPos.xy / i.scrPos.w;
                //准备中间数据（点积结果）
                float nl = max(saturate(dot(nDirWS, lDir)), 0.000001);//防止除0
				float nv = max(saturate(dot(nDirWS, vDirWS)), 0.000001);
				float vh = max(saturate(dot(vDirWS, hDir)), 0.000001);
				float lh = max(saturate(dot(lDir, hDir)), 0.000001);
				float nh = max(saturate(dot(nDirWS, hDir)), 0.000001);
                //纹理采样
                float3 offsetColor1 = SAMPLE_TEXTURE2D(_WarpMap,smp_Point_Repeat,i.uv * _WarpMap_ST.xy + _Time.x * _FlowSpeed).rgb;
                float3 offsetColor2 = SAMPLE_TEXTURE2D(_WarpMap,smp_Point_Repeat,i.uv * _WarpMap_ST.xy - _Time.x * _FlowSpeed).rgb;
                //提取信息
                float2 warp = (offsetColor1.rg - 0.5) * _WarpInt + (offsetColor2.rg - 0.5) * -_WarpInt;
                float noise = clamp(0,1,offsetColor1.b + offsetColor2.b);
                float2 warpScreenPos = screenPos + warp;
                float2 warpuv = i.uv + warp;

                //光照计算
                    //深度颜色渐变
                    float depth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, smp_Point_Repeat, screenPos).r;    //获取相机深度图
                    float backgroundDepth = LinearEyeDepth(depth, _ZBufferParams);
                    float surfaceDepth = i.scrPos.w;
                    float viewWaterDepth = backgroundDepth - surfaceDepth;	                                //深度差值
                    float viewWaterDepth01 = saturate(viewWaterDepth / _DepthRange);                        //取绝对值返回比例
                    float4 depthCol = lerp(_WaterShallowColr, _WaterDeepColr, viewWaterDepth01);            //控制浅水区和深水区颜色
                    //折射
                    float4 refraction = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, smp_Point_Repeat, warpScreenPos);
                    float alphaRange = saturate(viewWaterDepth / _AlphaRange);
                    float3 diffColor =  lerp(refraction.rgb,depthCol.rgb,alphaRange) * _Diffcol.rgb;
                    //岸边浪花
                    float4 foamTexCol = SAMPLE_TEXTURE2D(_FoamTex,smp_Point_Repeat,float2(sin( _Time.y * _Foam.y + min(_Foam.x, viewWaterDepth01)/_Foam.x),0.5));
                    float foamRange = 1 - (min(_Foam.z , viewWaterDepth01) / _Foam.z );                     // * step(viewWaterDepth01,_Edge)// * step(viewWaterDepth01,_Edge) * step(ratioZ,_Edge)
                    float3 foamCol = foamTexCol.rgb * foamRange;
                    foamCol = step(0.5,min(foamCol * noise,noise));
                    //水面交互
                    float2 RTuv = i.posWS.xz - _Position.xz;                                                // 像素点相对于相机中心的距离
                    RTuv = RTuv / (_OrthographicCamSize * 2);                                               // 转为 -0.5~0.5
                    RTuv += 0.5; // 转为 0~1
                    float ripples = SAMPLE_TEXTURE2D(_GlobalRipplesRT, sampler_GlobalRipplesRT,saturate(RTuv)).b;  //采样RenderTexture
                    ripples = step(2, ripples * 3);
                    float3 ripplesCol = ripples * _RippleColor.rgb;
                    //雨天涟漪
                    float3 emissive = 1-(frac((_Time.y * 20)));
                    float3 emissive2 = 1-(frac((_Time.y * 10) + 0.5));                                                                 //时间偏移
                    float RainTex = SAMPLE_TEXTURE2D(_RainTex,smp_Point_Repeat,i.uv * _RainTex_ST.xy).r;
                    float RainTex2 = SAMPLE_TEXTURE2D(_RainTex,smp_Point_Repeat,i.uv * _RainTex_ST.xy).g;      //UV 偏移
                    float maskColor = RainTex.r>0.1? saturate(1 - distance(emissive.r - RainTex,0.05)/0.05):0;
                    float maskColor2 = RainTex2.r>0.1? saturate(1 - distance(emissive2.r - RainTex2,0.05)/0.05):0;
                    float maskSwitch = saturate(abs(sin((_Time.y * 0.5))));                                                           //两张图交替淡入
                    float3 spray = lerp(maskColor , maskColor2 ,maskSwitch) * _RainInt * _RippleColor.rgb;
                    //高光
                    float3 specular = _SpecularColor.rgb * _SpecularStrenght * pow(max(0,nh),_SpecularRange);
                    specular = smoothstep(0.2,1,specular) * light.color.rgb;
                    //反射
                    float4 Cubemap = SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0,samplerunity_SpecCube0,vrDirWS,0);
                    float4 ReflectionTex = SAMPLE_TEXTURE2D(_ReflectionTex, smp_Point_Repeat, warpScreenPos);
                    float4 reflection = ReflectionTex;
                    //reflection = blendSeaColor(reflection,Cubemap);
                    
                    //菲涅尔
                    float f0 = 0.02;
                    float fresnel = f0 + (1-f0) * pow(1 - saturate(nv),_FresnelScale);
                    fresnel = saturate(fresnel);	
                //混合
                float3 col = diffColor + specular + foamCol + ripplesCol + spray;	
                col = lerp(col,reflection.rgb,fresnel);
                
                return float4(col,1.0);
                //return refraction;
            }

            ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}