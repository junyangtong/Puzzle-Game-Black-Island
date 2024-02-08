Shader "MiniGame/Password"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Offest ("Offest",float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Offest;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float Remap(float x, float t1, float t2, float s1, float s2)
            {
                return (s2 - s1) / (t2 - t1) * (x - t1) + s1;
                return (x - t1) / (t2 - t1) * (s2 - s1) + s1;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float per = 1./7.;
                float2 uv = i.uv;
                uv.y = uv.y / 7.;
                uv.y += -per*_Offest;
                fixed4 col = tex2D(_MainTex, uv);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
