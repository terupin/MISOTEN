Shader "Custom/UIUnlitShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {} // メインテクスチャ
        _SubTex ("Sub Texture", 2D) = "white" {}  // サブテクスチャ
        _CoolDown ("Cool Down Value", Float) = 0.0 // CoolDownの値
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _SubTex;
            float _CoolDown;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // メインテクスチャとサブテクスチャをサンプリング
                fixed4 mainColor = tex2D(_MainTex, i.uv);
                fixed4 subColor = tex2D(_SubTex, i.uv);

                // CoolDown値を利用したアルファマスク処理
                float mask = step(_CoolDown, i.uv.x); // UV.yを使ったマスク
                fixed4 resultColor = lerp(mainColor, subColor, mask);

                // 出力色
                return fixed4(resultColor.rgb, resultColor.a);
            }
            ENDHLSL
        }
    }
}
