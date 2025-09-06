Shader "Silhouette/Silhouette"
{
    // �Ƿ翧�� ����� ���̴�
    // VisibileSilhouette ���̴������� �Ƿ翧�� ��������.

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

        // �׻� ���ٽ� 1�� �����.
        // �������� Defaultó�� �۵��Ѵ�.
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // �ܰ����� �ε巯����
            Cull off                        // �ո� �޸� ��� ���̰�
            ZWrite Off
    
            // �׻� ���ٽ� 1�� �����.
            Stencil
            {
                Ref 1
                Comp Always     // �׻� ���ٽ��� ����Ѵ�.
                Pass Replace    // ����� �κ��� 1�� ��ü�Ѵ�.
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
