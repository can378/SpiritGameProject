Shader "Custom/UI_MultiCircleHoles"
{
    Properties
    {
        _Color ("Overlay Color", Color) = (0,0,0,0.8) // 검은 반투명
        _HoleCount ("Hole Count", Int) = 0
        _Holes ("Holes", Vector) = (0,0,0,0) // dummy (실제는 배열)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;

            // 최대 8개 구멍까지
            int _HoleCount;
            float4 _Holes[8]; // (x, y, radius, unused) 0~1 UV 기준

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 여러 개 구멍 검사
                for (int idx = 0; idx < _HoleCount; idx++)
                {
                    float2 c = _Holes[idx].xy;
                    float  r = _Holes[idx].z;

                    float d = distance(i.uv, c);
                    if (d < r)
                    {
                        // 이 픽셀은 구멍 안 → 그리지 않음
                        discard;
                    }
                }

                // 나머지는 전부 검은 오버레이
                return _Color;
            }
            ENDCG
        }
    }
}
