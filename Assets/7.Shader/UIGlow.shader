Shader "Custom/UIRadialGlowShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _CenterGlowColor("Center Glow Color", Color) = (1, 1, 1, 1)
        _GlowStrength("Glow Strength", Range(0, 1)) = 0.5
        _Radius("Radius", Range(0, 1)) = 0.5
    }

        SubShader{
            Tags { "RenderType" = "Transparent" }
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
                    float4 screenPos : TEXCOORD1;
                };

                sampler2D _MainTex;
                float4 _Color;
                float4 _CenterGlowColor;
                float _GlowStrength;
                float _Radius;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    o.screenPos = ComputeScreenPos(o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                    float4 centerGlow = 1 - smoothstep(_Radius - 0.01, _Radius, length(i.uv - 0.5));
                    col.rgb += _CenterGlowColor.rgb * centerGlow.rgb * _GlowStrength;
                    col.a *= centerGlow.a;
                    return col;
                }
                ENDCG
            }
        }

            FallBack "UI/Default"
}
