Shader "Custom/RadialShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Glossiness("Glossiness", Range(0, 1)) = 0.5
        _Metallic("Metallic", Range(0, 1)) = 0.0
        _Radius("Radius", Range(0, 1)) = 0.5
        _BorderSize("Border Size", Range(0, 0.1)) = 0.01
        _BorderColor("Border Color", Color) = (0, 0, 0, 1)
    }

        SubShader{
            Tags {"Queue" = "Transparent" "RenderType" = "Opaque"}
            LOD 100

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _Color;
                float _Glossiness;
                float _Metallic;
                float _Radius;
                float _BorderSize;
                float4 _BorderColor;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    // 이미지의 중심점에서의 거리 계산
                    float dist = length(i.uv - 0.5);
                // 반경 이내인지 여부를 마스킹
                float alpha = smoothstep(_Radius - _BorderSize, _Radius, dist);
                // 배경인 부분은 투명으로 처리
                if (alpha < 1.0) {
                    discard;
                }
                // 쉐이딩 계산
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                col.rgb *= 1.0 - _Glossiness;
                col.rgb += _Glossiness * _BorderColor.rgb;
                col.a = alpha;
                return col;
            }
            ENDCG
        }
        }
            FallBack "Diffuse"
}
