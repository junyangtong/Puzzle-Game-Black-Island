#ifndef UNIVERSAL_NPR_META_PASS_INCLUDED
#define UNIVERSAL_NPR_META_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UniversalMetaPass.hlsl"

float4 _EmissCol;
TEXTURE2D(_EmissMap);
SAMPLER(sampler_EmissMap);

half4 UniversalFragmentMetaLit(Varyings input) : SV_Target
{
    SurfaceData surfaceData;
    InitializeStandardLitSurfaceData(input.uv, surfaceData);

    BRDFData brdfData;
    InitializeBRDFData(surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness, surfaceData.alpha, brdfData);
    float emissMap = SAMPLE_TEXTURE2D(_EmissMap, sampler_EmissMap, input.uv).r;
    MetaInput metaInput;
    metaInput.Albedo = brdfData.diffuse + brdfData.specular * brdfData.roughness * 0.5;
    metaInput.Emission = _EmissCol.rgb * emissMap;
    return UniversalFragmentMeta(input, metaInput);
}
#endif
