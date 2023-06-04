Shader "Custom/BurningShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _BurnAmount("Burn Amount", Range(0.0, 1.0)) = 0.0
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
                float _BurnAmount;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    float burn = _BurnAmount * (col.r + col.g + col.b) / 3.0;
                    return fixed4(col.rgb - burn, col.a);
                }
                ENDCG
            }
        }
}
