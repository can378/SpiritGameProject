Shader "Silhouette/Silhouette"
{
    // 실루엣을 남기는 셰이더
    // VisibileSilhouette 셰이더에서만 실루엣이 보여진다.

    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Color", Color) = (0,0,0,0)
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

        // 항상 스텐실 1을 남긴다.
        // 나머지는 Default처럼 작동한다.
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // 외곽선이 부드러워짐
            Cull off                        // 앞면 뒷면 모두 보이게
            ZWrite Off
    
            // 항상 스텐실 1을 남긴다.
            Stencil
            {
                Ref 1
                Comp Always     // 항상 스텐실을 통과한다.
                Pass Replace    // 통과한 부분은 1로 대체한다.
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
    }
}
