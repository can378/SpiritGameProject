Shader "Silhouette/VisibleSilhouette"
{
    // 실루엣을 보여주는 셰이더
    // Silhouette 셰이더에서만 실루엣을 보여준다.


    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Color", Color) = (0,0,0,0)
        
        _StencilRef ("Player Stencil Reference", Int) = 1
        _Color1("Player Silhouette Color", Color) = (0,0,0,0)

        _StencilRef1 ("Enemy Stencil Reference", Int) = 2
        _Color2("Enemy Silhouette color", Color) = (0,0,0,0)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }
        LOD 100

        // Sprite 패스
        // 스텐실이 1이 아닌 곳은 원래 색상을 그린다.
        // 나머지는 Defualt와 동일하다.
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            ZWrite Off

            // 스텐실이 1인 곳에는 원래 색상을 입힌다.
            Stencil
            {
                Ref 0
                Comp Equal // NotEqual로 설정하여 Ref 값(1)과 다른 픽셀만 통과
            }

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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color    : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                return col;
            }

            ENDCG
            
        }

        // 실루엣 패스
        // 플레이어 또는 아군
        // Stencil이 1인 곳에는 실루엣을 그린다.
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // 외곽선이 부드러워짐
            Cull off                        // 앞면 뒷면 모두 보이게

            // 깊이 테스트에 실패하고 스텐실이 1인 곳에만 그린다.
            Stencil
            {
                Ref [_StencilRef]
                Comp Equal // 스텐실 값이 1인 곳에만 렌더링
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color    : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color    : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _Color1;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                // sample the texture
                if(col.a < 0.5f)
                {
                    discard;
                }
                col = _Color1;
                return col;
            }

            ENDCG

        }

        // 적
        // 실루엣 패스
        // Stencil이 2인 곳에는 실루엣을 그린다.
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // 외곽선이 부드러워짐
            Cull off                        // 앞면 뒷면 모두 보이게

            // 깊이 테스트에 실패하고 스텐실이 2인 곳에만 그린다.
            Stencil
            {
                Ref [_StencilRef1]
                Comp Equal // 스텐실 값이 2인 곳에만 렌더링
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color    : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color    : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _Color2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                // sample the texture
                if(col.a < 0.5f)
                {
                    discard;
                }
                col = _Color2;
                return col;
            }

            ENDCG

        }
    }
}
