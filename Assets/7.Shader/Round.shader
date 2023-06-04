Shader "Custom/RoundedBoxShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Radius("Radius", Range(0.0, 1.0)) = 0.5
    }

        SubShader{
            Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }
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
                float _Radius;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    fixed4 col = tex2D(_MainTex, i.uv);
                // 이미지의 네모 영역을 동그란 형태로 마스킹
                float d = length(i.uv - 0.5) * 2.0;
                col.a *= smoothstep(_Radius, _Radius + 0.01, d);
                col.rgb *= _Color.rgb;
                return col;
            }
            ENDCG
        }
        }

            FallBack "Diffuse"
}
