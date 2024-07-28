Shader "Hidden/MichisMeshMakers/CanvasGrid"
{
    Properties
    {
        _EvenColor ("Even Cell Color", Color) = (0.15,0.15,0.15,1)
        _UnevenColor ("Uneven Cell Color", Color) = (0.25,0.25,0.25,1)
        _RectSize("Rect Size", Vector) = (100,100,0,0)
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
            float2 _RectSize;
            float _CellSize;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float invLerp(const float from, const float to, const float value)
            {
                return (value - from) / (to - from);
            }

            v2f vert(const appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
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
                const float2 uv = i.uv - 0.5f;
                const float2 pixel = uv * _RectSize; // Transform from [0,1] to screen space
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
                const int2 cellIndex = floor(cell);
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