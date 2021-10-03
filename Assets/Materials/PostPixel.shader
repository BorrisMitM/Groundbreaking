Shader "Hidden/PostPixel"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelMul("PixelMul", Range(1, 16)) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _PixelMul;

            fixed4 frag(v2f i) : SV_Target
            {
                float2 div = _ScreenParams.xy / _PixelMul;
                float2 pixelUV = round(i.uv * div) / div;
                fixed4 col = tex2D(_MainTex, pixelUV);
                return col;
            }
            ENDCG
        }
    }
}
