Shader "Custom/FlickeringFlameShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Distortion("Distortion", Range(0.01, 1.0)) = 0.1
        _Speed("Speed", Range(0.1, 10.0)) = 1.0
        _Frequency("Frequency", Range(0.1, 10.0)) = 1.0
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
                float _Distortion;
                float _Speed;
                float _Frequency;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    // 불꽃의 위치 계산
                    float2 flamePos = i.uv;
                    float2 offset = float2(sin(_Time.y * _Speed * 0.5), cos(_Time.y * _Speed)) * _Distortion;
                    flamePos += offset;

                    // 불꽃에 적용할 텍스처 좌표 계산
                    float2 texCoord = (i.uv + offset) * _Frequency;
                    float flameValue = tex2D(_MainTex, texCoord).r;

                    // 쉐이딩 계산
                    fixed4 col = _Color;
                    col.rgb *= flameValue;
                    col.a = flameValue;
                    return col;
                }
                ENDCG
            }
        }

            FallBack "Diffuse"
}
