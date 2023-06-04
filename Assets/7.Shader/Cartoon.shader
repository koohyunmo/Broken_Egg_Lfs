Shader "Custom/Toon" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _ToonRamp("Toon Ramp", 2D) = "white" {}
    }
        SubShader{
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
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
                sampler2D _ToonRamp;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    float4 texColor = tex2D(_MainTex, i.uv);
                    float3 rampColor = tex2D(_ToonRamp, float2(texColor.r, 0.5)).rgb;
                    return fixed4(rampColor, texColor.a);
                }
                ENDCG
            }
    }
        FallBack "Diffuse"
}
