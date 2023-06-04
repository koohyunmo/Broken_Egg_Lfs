Shader "Custom/ColorShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,0,0,1)
    }
        SubShader{
            Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }
            LOD 100

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                // Declare input and output structures
                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                // Declare shader properties
                sampler2D _MainTex;
                float4 _Color;

                // Declare vertex shader function
                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                // Declare fragment shader function
                fixed4 frag(v2f i) : SV_Target {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    col.rgb = _Color.rgb;
                    return col;
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}
