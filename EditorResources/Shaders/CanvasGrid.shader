Shader "Hidden/MichisMeshMakers/CanvasGrid"
{
    Properties
    {
        _EvenColor ("Even Cell Color", Color) = (0.15,0.15,0.15,1)
        _UnevenColor ("Uneven Cell Color", Color) = (0.25,0.25,0.25,1)
        _CellSize("Cell Size", Float) = 10
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _EvenColor;
            float4 _UnevenColor;
            float _CellSize;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 viewPos : TEXCOORD0;
            };

            float invLerp(const float from, const float to, const float value)
            {
                return (value - from) / (to - from);
            }

            v2f vert(const appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.viewPos = UnityObjectToViewPos(v.vertex);
                return o;
            }

            float3 toLinear(const float3 color)
            {
                const float3 linearRgbLo = color / 12.92;;
                const float3 linearRgbHi = pow(max(abs((color + 0.055) / 1.055), 1.192092896e-07),
                                               float3(2.4, 2.4, 2.4));
                return float3(color <= 0.04045) ? linearRgbLo : linearRgbHi;
            }
            
            float3 toGamma(const float3 color)
            {
                float3 sRgbLo = color * 12.92;
                float3 sRgbHi = (pow(max(abs(color), 1.192092896e-07), float3(1.0 / 2.4, 1.0 / 2.4, 1.0 / 2.4)) * 1.055)
                    - 0.055;
                return float3(color <= 0.0031308) ? sRgbLo : sRgbHi;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                const float4 clipPos = mul(UNITY_MATRIX_P, i.viewPos);
                float2 clipXy = clipPos.xy / clipPos.w;
                clipXy.y *= _ProjectionParams.x;

                const float2 screenUv = clipXy * 0.5f + float2(0.5f, 0.5f); // Transform from range [-1,1] to [0,1]
                const float2 pixel = screenUv * _ScreenParams.xy; // Transform from [0,1] to screen space
                const float2 invCellSize = 1.0f / _CellSize;
                const float2 cell = pixel * invCellSize;

                // evaluate anti-aliased cell main color weight
                const float2 cellUv = frac(cell);
                const float2 cellUvEdgeDistance2D = abs(cellUv - 0.5f);
                const float longDistance = 0.5f + max(cellUvEdgeDistance2D.x, cellUvEdgeDistance2D.y);
                // uv distance from opposite edge
                const float halfInvCellSize = 0.5f * invCellSize;
                const float from = 1.0f - halfInvCellSize;
                const float to = 1.0f + halfInvCellSize;
                const float mainWeight = saturate(1.0f - invLerp(from, to, longDistance));

                //evaluate main and secondary color
                const uint2 cellIndex = floor(cell);
                const bool isEven = (cellIndex.x + cellIndex.y) % 2 == 0;
                const fixed4 mainColor = isEven ? _EvenColor : _UnevenColor;
                const fixed4 secondaryColor = isEven ? _UnevenColor : _EvenColor;
\
                #if defined(UNITY_COLORSPACE_GAMMA)
                const half3 mainColorSrgb = toLinear(mainColor.rgb);
                const half3 secondaryColorSrgb = toLinear(secondaryColor.rgb);
                const half3 srgb = lerp(secondaryColorSrgb, mainColorSrgb, mainWeight);
                fixed4 color = fixed4(toGamma(srgb), lerp((half)mainColor.a, (half)secondaryColor.a, (half)mainWeight));
                #else
                float4 color = lerp(mainColor, secondaryColor, mainWeight);
                #endif
                return color;
            }
            ENDHLSL
        }
    }
}