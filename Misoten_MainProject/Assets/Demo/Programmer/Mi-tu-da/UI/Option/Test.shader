Shader "Custom/UIUnlitShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {} // ���C���e�N�X�`��
        _SubTex ("Sub Texture", 2D) = "white" {}  // �T�u�e�N�X�`��
        _CoolDown ("Cool Down Value", Float) = 0.0 // CoolDown�̒l
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
                // ���C���e�N�X�`���ƃT�u�e�N�X�`�����T���v�����O
                fixed4 mainColor = tex2D(_MainTex, i.uv);
                fixed4 subColor = tex2D(_SubTex, i.uv);

                // CoolDown�l�𗘗p�����A���t�@�}�X�N����
                float mask = step(_CoolDown, i.uv.x); // UV.y���g�����}�X�N
                fixed4 resultColor = lerp(mainColor, subColor, mask);

                // �o�͐F
                return fixed4(resultColor.rgb, resultColor.a);
            }
            ENDHLSL
        }
    }
}
