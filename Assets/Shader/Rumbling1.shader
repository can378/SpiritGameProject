Shader "Custom/Rumbling1"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _RefStrength("Reflection Strength",Range(0,0.1)) = 0.05
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
            zwrite off

            GrabPass{}

            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf nolight noambient alpha:fade

        sampler2D _GrabTexture;
        sampler2D _MainTex;
        float _RefStrength;



        struct Input
        {
            float4 colpr:COLOR;
            float4 screenPos;
            float2 uv_MainTex;
        };


        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 ref = tex2D (_MainTex, float2(IN.uv_MainTex.x,IN.uv_MainTex.y));
            
            float3 screenUV = IN.screenPos.rgb / IN.screenPos.a;
            o.Emission = tex2D(_GrabTexture, (screenUV.xy + ref.x * _RefStrength));
        }

        float4 Lightingnolight(SurfaceOutput s, float3 lightDir, float atten)
        {
            return float4(0, 0, 0, 1);
        }
        ENDCG
    }
    FallBack "Regacy Shaders/Transparent/Vertexlit"
}
