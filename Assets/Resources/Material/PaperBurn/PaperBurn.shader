Shader "Effect/PaperBurn"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}                        
        _Color ("Color",Color) = (0,0,0,0)
        _InnerBurnColor ("InnerBurnColor", Color) = (0, 0, 0, 0)    // 없어지는 안쪽 색깔
        _OuterBurnColor ("OuterBurnColor", Color) = (0, 0, 0, 0)    // 없어지는 바깥쪽 색깔
        _InnerRange ("InnerRange", Range(0,1)) = 0                  // 안쪽 색깔 범위
        _OuterRange ("OuterRange", Range(0,1)) = 0                  // 바깥쪽 색깔 범위
        _Timer ("Time", Range(0,1)) = 0                           // 타들어가는 시간
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0

    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Cull off                        // 앞면 뒷면 모두 보이게
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color    : COLOR;

                float3 worldPos : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color    : COLOR;

                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _InnerBurnColor;
            fixed4 _OuterBurnColor;
            float _Timer;
            float _InnerRange;
            float _OuterRange;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;

                // sample the texture
                fixed4 noise = tex2D(_NoiseTex, i.worldPos.xy * 0.2f);

                if(0 < _Timer)
                {
                    if(_InnerRange < noise.r + _Timer && noise.r + _Timer <= _OuterRange)
                    {
                        col = _InnerBurnColor;
                        
                    }
                    else if(_OuterRange < noise.r + _Timer)
                    {
                        col = _OuterBurnColor;
                    }


                    if(1.f < noise.r + _Timer)
                    {
                        discard;
                    }
                }





                return col;
            }
            ENDCG
        }
    }
}
