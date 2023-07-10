Shader "Custom/PanelBlurTransparency"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BlurSize("Blur Size", Range(0.0, 10.0)) = 1.0
        _Transparency("Transparency", Range(0.0, 1.0)) = 0.5
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

            Pass
            {
                ZWrite Off
                Blend SrcAlpha OneMinusSrcAlpha

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float _BlurSize;
                float _Transparency;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                float4 frag(v2f i) : SV_Target
                {
                    float2 texelSize = 1.0 / _ScreenParams.xy;
                    float4 blurColor = float4(0.0, 0.0, 0.0, 0.0);
                    float weightSum = 0.0;

                    // Gaussian Blur
                    for (int x = -5; x <= 5; x++)
                    {
                        for (int y = -5; y <= 5; y++)
                        {
                            float2 offset = float2(x, y) * _BlurSize * texelSize;
                            float weight = 1.0 - length(offset);
                            weightSum += weight;
                            blurColor += tex2D(_MainTex, i.uv + offset) * weight;
                        }
                    }

                    blurColor /= weightSum;

                    float transparency = 1.0 - _Transparency;
                    blurColor.a *= transparency;

                    return blurColor;
                }

                ENDCG
            }
        }
}