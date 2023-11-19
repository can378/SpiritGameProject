Shader "Custom/MiniMapShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _PlayerPos ("Player Position", Vector) = (0,0,0,0)
        _Radius ("Radius", Float) = 5.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
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
            float4 _MainTex_ST;
            float4 _PlayerPos;
            float _Radius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);

                // 플레이어 위치와의 거리 계산
                float dist = distance(_PlayerPos.xy, i.uv);

                // 반경 안에 있으면 정상 표시, 반경 밖이면 어둡게 처리
                if (dist > _Radius)
                {
                    col *= 0.5; // 밝기를 절반으로 줄임
                }

                return col;
            }
            ENDCG
        }
    }
}
