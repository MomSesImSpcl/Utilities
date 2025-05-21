Shader "MomSesImSpcl/Sprite Shadow"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _AlphaClip("Alpha Clip", Range(0, 1)) = 0.5
        _ShadowClip("Shadow Clip", Range(0, 1)) = 0.5
        _LightIntensity("Light Intensity", Range(0, 3)) = 1.0
        [Toggle] _UseNormalMap("Use Normal Map", Float) = 0
        [Normal] _NormalMap("Normal Map", 2D) = "bump" {}
        _NormalIntensity("Normal Intensity", Range(0, 3)) = 1
        _Smoothness("Smoothness", Range(0, 1)) = 0
        _Metallic("Metallic", Range(0, 1)) = 0
        [Enum(Off,0,Front,1,Back,2)] _CullMode("Culling Mode", Float) = 2
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
            "UniversalMaterialType"="Lit"
        }
        
        HLSLINCLUDE
        
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
        
        CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            float _AlphaClip;
            float _ShadowClip;
            float _LightIntensity;
            float _UseNormalMap;
            float4 _NormalMap_ST;
            float _NormalIntensity;
            float _Smoothness;
            float _Metallic;
        CBUFFER_END
        
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_NormalMap);
        SAMPLER(sampler_NormalMap);
        
        struct attributes
        {
            float4 positionOS   : POSITION;
            float3 normalOS     : NORMAL;
            float4 tangentOS    : TANGENT;
            float2 texcoord     : TEXCOORD0;
            float4 color        : COLOR;
        };

        struct varyings
        {
            float4 positionCS   : SV_POSITION;
            float2 uv           : TEXCOORD0;
            float4 color        : COLOR;
            float3 normalWS     : TEXCOORD1;
            float3 positionWS   : TEXCOORD2;
            float3 tangentWS    : TEXCOORD3;
            float3 bitangentWS  : TEXCOORD4;
            float3 viewDirWS    : TEXCOORD5;
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                float4 shadowCoord  : TEXCOORD6;
            #endif
        };

        ENDHLSL
        
        Pass
        {
            Tags 
            { 
                "LightMode" = "UniversalForward"
            }
            
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull [_CullMode]
            
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
            
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile_fragment _ _CLUSTER_LIGHT_LOOP
            // #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            // #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
            
            varyings vert(attributes input)
            {
                varyings output;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                
                output.uv = TRANSFORM_TEX(input.texcoord, _MainTex);
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = normalInput.normalWS;
                output.tangentWS = normalInput.tangentWS;
                output.bitangentWS = normalInput.bitangentWS;
                output.viewDirWS = GetWorldSpaceViewDir(output.positionWS);
                output.color = input.color;
                
                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                    output.shadowCoord = GetShadowCoord(vertexInput);
                #endif
                
                return output;
            }
            
            float4 frag(varyings input) : SV_Target
            {
                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                float4 color = texColor * input.color;
                
                clip(color.a - _AlphaClip);
                
                float3 normalWS = normalize(input.normalWS);
                if (_UseNormalMap > 0.5)
                {
                    float3 normalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, input.uv), _NormalIntensity);
                    float3x3 tangentToWorld = float3x3(normalize(input.tangentWS), normalize(input.bitangentWS), normalWS);
                    normalWS = normalize(mul(normalTS, tangentToWorld));
                }
                
                float4 shadowCoord = float4(0, 0, 0, 0);
                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                    shadowCoord = input.shadowCoord;
                #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                    shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                #endif
                
                InputData inputData = (InputData)0;
                inputData.positionWS = input.positionWS;
                inputData.normalWS = normalWS;
                inputData.viewDirectionWS = normalize(input.viewDirWS);
                inputData.shadowCoord = shadowCoord;
                
                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.albedo = color.rgb;
                surfaceData.alpha = color.a;
                surfaceData.specular = 0;
                surfaceData.smoothness = _Smoothness;
                surfaceData.metallic = _Metallic;
                surfaceData.occlusion = 1.0;
                
                // Calculate URP standard lighting
                #if VERSION_GREATER_EQUAL(12, 0) // Unity 2021.2+ - URP 12+
                    half4 finalColor = UniversalFragmentPBR(inputData, surfaceData); 
                #else // Manually calculate lighting for older URP versions
                    Light mainLight = GetMainLight(shadowCoord);
                    half3 attenuatedLightColor = mainLight.color * mainLight.distanceAttenuation;
                    half shadowAttenuation = mainLight.shadowAttenuation;
                    half3 shadowColor = 1;
                    
                    #ifdef _MAIN_LIGHT_SHADOWS
                        shadowColor = lerp(1, mainLight.color, (1 - shadowAttenuation));
                    #endif
                
                    float3 radiance = attenuatedLightColor * shadowAttenuation * shadowColor;
                    float NdotL = saturate(dot(normalWS, mainLight.direction));
                    float3 diffuse = radiance * NdotL;
                    float3 halfVector = normalize(mainLight.direction + inputData.viewDirectionWS);
                    float NdotH = saturate(dot(normalWS, halfVector));
                    float NoV = saturate(dot(normalWS, inputData.viewDirectionWS));
                    float roughness = 1.0 - _Smoothness;
                    float roughness2 = roughness * roughness;
                    float D = roughness2 / (3.14159 * pow(NdotH * NdotH * (roughness2 - 1.0) + 1.0, 2.0));
                    float G = min(1, min(2.0 * NdotL * NoV / (NoV + sqrt(roughness2 + (1.0 - roughness2) * NoV * NoV)), 2.0 * NdotL / (NdotL + sqrt(roughness2 + (1.0 - roughness2) * NdotL * NdotL))));
                    float3 F = _Metallic + (1.0 - _Metallic) * pow(1.0 - NoV, 5.0);
                    float3 specular = D * G * F * 0.25 / max(0.001, NoV);
                    float3 additionalLighting = float3(0, 0, 0);
                
                    #ifdef _ADDITIONAL_LIGHTS
                        uint additionalLightsCount = GetAdditionalLightsCount();
                        for (uint lightIndex = 0u; lightIndex < additionalLightsCount; ++lightIndex) {
                            Light light = GetAdditionalLight(lightIndex, input.positionWS);
                            float3 lightDir = normalize(light.direction);
                            float lightAtten = light.distanceAttenuation * light.shadowAttenuation;
                            float ndl = saturate(dot(normalWS, lightDir));
                            float3 lightContrib = light.color * ndl * lightAtten;
                            float3 halfVectorAdd = normalize(lightDir + inputData.viewDirectionWS);
                            float NdotHAdd = saturate(dot(normalWS, halfVectorAdd));
                            float DAdd = roughness2 / (3.14159 * pow(NdotHAdd * NdotHAdd * (roughness2 - 1.0) + 1.0, 2.0));
                            float GAdd = min(1, min(2.0 * ndl * NoV / (NoV + sqrt(roughness2 + (1.0 - roughness2) * NoV * NoV)), 2.0 * ndl / (ndl + sqrt(roughness2 + (1.0 - roughness2) * ndl * ndl))));
                            float3 FAdd = _Metallic + (1.0 - _Metallic) * pow(1.0 - NoV, 5.0);
                            float3 specularAdd = DAdd * GAdd * FAdd * 0.25 / max(0.001, NoV);
                            
                            additionalLighting += lightContrib + specularAdd * light.color * lightAtten;
                        }
                    #endif
                
                    float3 ambient = SampleSH(normalWS) * (1.0 - _Metallic);
                    float3 lighting = (ambient + diffuse + specular * attenuatedLightColor * shadowAttenuation + additionalLighting) * _LightIntensity;
                    float4 finalColor = float4(lighting * color.rgb, color.a);
                #endif
                
                return finalColor;
            }
            ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            
            Tags 
            { 
                "LightMode" = "ShadowCaster" 
            }
            
            ZWrite On
            ZTest LEqual
            Cull Off
            
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            // #pragma multi_compile_shadowcaster
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
            #pragma multi_compile _ DOTS_INSTANCING_ON
            #pragma multi_compile _ LOD_FADE_CROSSFADE
            
            float3 _LightDirection;
            
            struct vertex_shadow_caster
            {
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
            };
            
            vertex_shadow_caster vert(attributes input)
            {
                vertex_shadow_caster output;
                
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                float3 lightDirectionWS = _LightDirection;
                
                positionWS = ApplyShadowBias(positionWS, normalWS, lightDirectionWS);
                output.positionCS = TransformWorldToHClip(positionWS);
                output.uv = input.texcoord;
                
                #if UNITY_REVERSED_Z
                    output.positionCS.z = min(output.positionCS.z, UNITY_NEAR_CLIP_VALUE);
                #else
                    output.positionCS.z = max(output.positionCS.z, UNITY_NEAR_CLIP_VALUE);
                #endif
                
                return output;
            }
            
            float4 frag(vertex_shadow_caster input) : SV_TARGET
            {
                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                clip(texColor.a - _ShadowClip);
                return 0;
            }
            ENDHLSL
        }
    }
}
