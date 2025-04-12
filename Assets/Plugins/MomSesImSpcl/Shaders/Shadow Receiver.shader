Shader "MomSesImSpcl/2D/Shadow Receiver"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 position : SV_POSITION;
                half2 uv : TEXCOORD0;
                half4 color : COLOR;
                float3 worldPos : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                float4 _Color;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.position = TransformObjectToHClip(IN.vertex.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color * _Color;
                OUT.worldPos = TransformObjectToWorld(IN.vertex.xyz);
                return OUT;
            }
            
            half4 frag(Varyings IN) : SV_Target
            {
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                half4 col = texColor * IN.color;
                
                float4 shadowCoord = TransformWorldToShadowCoord(IN.worldPos);
                Light mainLight = GetMainLight(shadowCoord);
                col.rgb *= mainLight.shadowAttenuation;

                return col;
            }
            ENDHLSL
        }
    }
    FallBack "Universal Forward"
}
