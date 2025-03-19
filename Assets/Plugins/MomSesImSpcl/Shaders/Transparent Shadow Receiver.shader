Shader "MomSesImSpcl/2D/Transparent Shadow Receiver"
{
    Properties
    {
        _ShadowColor ("Shadow Color", Color) = (0.35,0.4,0.45,1.0)
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent-1"
        }
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            Blend DstColor Zero, Zero One
            Cull Back
            ZTest LEqual
            ZWrite Off
  
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile_fog
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
            float4 _ShadowColor;
            CBUFFER_END
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct Varyings
            {
                float4 positionCS               : SV_POSITION;
                float3 positionWS               : TEXCOORD0;
                float3 normalWS                 : TEXCOORD1;
                float4 shadowCoord              : TEXCOORD2;
                float fogCoord                  : TEXCOORD3;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS);
                
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = normalInput.normalWS;
                output.shadowCoord = TransformWorldToShadowCoord(output.positionWS);
                output.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
                
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                Light mainLight = GetMainLight(input.shadowCoord);
                float shadowAttenuation = mainLight.shadowAttenuation;
                uint pixelLightCount = GetAdditionalLightsCount();
                
                for (uint lightIndex = 0; lightIndex < pixelLightCount; ++lightIndex)
                {
                    Light light = GetAdditionalLight(lightIndex, input.positionWS, 1);
                    shadowAttenuation = min(shadowAttenuation, light.shadowAttenuation);
                }
                
                Light light = GetAdditionalLight(0, input.positionWS, 1);
                shadowAttenuation = min(shadowAttenuation, light.shadowAttenuation);
                
                half4 color = lerp(half4(1, 1, 1, 1), _ShadowColor, (1.0 - shadowAttenuation) * _ShadowColor.a);
                color.rgb = MixFogColor(color.rgb, half3(1,1,1), input.fogCoord);
                
                return color;
            }
            ENDHLSL
        }
    }
}