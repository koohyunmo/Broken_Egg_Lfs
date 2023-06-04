Shader "Custom/EggShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Shininess("Shininess", Range(0, 1)) = 0.5
        _Reflectivity("Reflectivity", Range(0, 1)) = 0.5
        _ReflectionTex("Reflection Texture", 2D) = "white" {}
    }

        SubShader{
            Tags { "RenderType" = "Opaque" }

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float3 worldNormal : TEXCOORD1;
                    float3 worldPos : TEXCOORD2;
                };

                sampler2D _MainTex;
                float4 _Color;
                float _Shininess;
                float _Reflectivity;
                sampler2D _ReflectionTex;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    o.worldNormal = mul((float3x3)unity_WorldToObject, v.normal);
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    fixed4 texColor = tex2D(_MainTex, i.uv);
                    float3 normal = normalize(i.worldNormal);
                    float3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));

                    // 반사 벡터 계산
                    float3 reflDir = reflect(viewDir, normal);
                    // 반사 매핑 계산
                    float4 reflColor = tex2D(_ReflectionTex, reflDir.xy);

                    // 쉐이딩 계산
                    float4 finalColor = texColor * _Color;
                    finalColor.rgb += _Reflectivity * reflColor.rgb;
                    finalColor.rgb *= (1.0 - _Shininess);

                    return finalColor;
                }
                ENDCG
            }
    }

        FallBack "Diffuse"
}
