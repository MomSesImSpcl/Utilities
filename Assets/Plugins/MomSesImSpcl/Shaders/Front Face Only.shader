Shader "MomSesImSpcl/2D/Front Face Only"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        [PerRendererData] _Color ("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        LOD 0
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "PreviewType"="Plane"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }

        Cull Back
        ZWrite Off
        Lighting Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            //#pragma multi_compile_instancing // Uncomment this to support instancing.
            
	    UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
	    UNITY_INSTANCING_BUFFER_END(Props)
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            v2f vert(appdata IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                OUT.pos = TransformObjectToHClip(IN.vertex.xyz);
                OUT.uv = IN.uv;
                OUT.color = IN.color * UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
                return OUT;
            }

            half4 frag(v2f IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * IN.color;
            }
            ENDHLSL
        }
    }
    FallBack Off
}
