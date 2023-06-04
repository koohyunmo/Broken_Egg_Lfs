Shader "Custom/StarShine" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _TintColor("Tint Color", Color) = (1,1,1,1)
        _Speed("Speed", Range(0.1, 5)) = 1
        _Size("Size", Range(0.01, 0.1)) = 0.05
    }
        SubShader{
            Tags {"Queue" = "Transparent" "RenderType" = "Opaque"}
            LOD 100

            CGPROGRAM
            #pragma surface surf Standard
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _TintColor;
            float _Speed;
            float _Size;

            struct Input {
                float2 uv_MainTex;
                float3 worldPos;
            };

            void surf(Input IN, inout SurfaceOutputStandard o) {
                float2 p = IN.worldPos.xz;
                float2 center = float2(0.5, 0.5);

                // calculate star shape
                float d = distance(p, center);
                float t = fmod(_Time.y * _Speed + dot(p, float2(1.7, 2.3)), 3.1415 * 2);
                float f = abs(cos(t + d * 20));
                float star = smoothstep(_Size - 0.001, _Size + 0.001, f);

                // set output color and alpha
                o.Albedo = _TintColor.rgb;
                o.Alpha = star * _TintColor.a;
                o.Alpha *= tex2D(_MainTex, IN.uv_MainTex).a;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
