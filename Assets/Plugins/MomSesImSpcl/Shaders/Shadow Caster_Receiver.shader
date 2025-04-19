Shader "MomSesImSpcl/2D/Shadow Caster_Receiver"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.004
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "PreviewType"="Plane"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }
        Pass
        {
            Name "ForwardLit"
            Tags
            {
                "LightMode"="UniversalForward"
            }
            
            Cull back
            AlphaToMask On
            Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
            
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            
            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float3 worldPos : TEXCOORD1;
            };
            
            CBUFFER_START(UnityPerMaterial)
                uniform float4 _MainTex_ST;
                uniform float4 _Color;
                uniform float _Cutoff;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.position = TransformObjectToHClip(IN.vertex.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color * _Color;
                OUT.worldPos = TransformObjectToWorld(IN.vertex.xyz);
                return OUT;
            }
            
            float4 frag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                float4 col = texColor * IN.color;
                
                float4 shadowCoord = TransformWorldToShadowCoord(IN.worldPos);
                Light mainLight = GetMainLight(shadowCoord);
                col.rgb *= mainLight.shadowAttenuation;

                return col;
            }
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode"="ShadowCaster"
            }
            
            Cull off
            AlphaToMask On
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
                uniform float4 _MainTex_ST;
                uniform float4 _Color;
                uniform float _Cutoff;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 positionWS = TransformObjectToWorld(IN.vertex.xyz);
                OUT.position = TransformWorldToHClip(positionWS);
                OUT.uv = IN.uv;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                clip(texColor.a - _Cutoff);
                
                return 0;
            }
            ENDHLSL
        }
    }
    FallBack Off
}
