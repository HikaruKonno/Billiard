// アウトラインのシェーダー

Shader "Custom/URP_Unlit_Outline"
{
    Properties
    {
        _MainTex       ("Texture",        2D)     = "white" {}
        _Color         ("Color",          Color) = (1,1,1,1)
        _OutlineColor  ("Outline Color",  Color) = (0,0,0,1)
        _OutlineWidth  ("Outline Width", Range(0,50)) = 5.0
    }
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }
        LOD 100

        // ==== 1. Base Pass (最初に本体を描画) ====
        Pass
        {
            Name "BASE"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vertBase
            #pragma fragment fragBase
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4   _MainTex_ST;
            float4   _Color;

            Varyings vertBase (Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 fragBase (Varyings IN) : SV_Target
            {
                return tex2D(_MainTex, IN.uv) * _Color;
            }
            ENDHLSL
        }

        // ==== 2. Outline Pass (後から輪郭だけ重ねる) ====
        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode"="SRPDefaultUnlit" }

            Cull Front       // 表面を描かず、裏面だけ描く
            ZWrite Off       // 深度を書き込まない
            ZTest LEqual     // 手前か等しい位置だけ描く

            HLSLPROGRAM
            #pragma vertex vertOutline
            #pragma fragment fragOutline
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes { float3 positionOS : POSITION; float3 normalOS : NORMAL; };
            struct Varyings  { float4 positionCS : SV_POSITION; };

            float4 _OutlineColor;
            float  _OutlineWidth;

            Varyings vertOutline (Attributes IN)
            {
                Varyings OUT;
                float3 normWS = TransformObjectToWorldNormal(IN.normalOS);
                float3 posWS  = TransformObjectToWorld(IN.positionOS) + normWS * _OutlineWidth;
                OUT.positionCS = TransformWorldToHClip(posWS);
                return OUT;
            }

            half4 fragOutline (Varyings IN) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Forward"
}
