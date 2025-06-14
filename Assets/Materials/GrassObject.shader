Shader "Unlit/GrassObject"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Tilt("Tilt", Float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType" = "Transparent" "PreviewType" = "Plane"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
			float _Tilt;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				i.uv.x = i.uv.x * 3 - 1;
				float x = i.uv.x - (i.uv.y * i.uv.y * _Tilt);
				i.uv.y = distance(fixed2(i.uv.x, i.uv.y),fixed2(x,0));
				i.uv.x = x;
				i.uv.x = (i.uv.x + 1) / 3.0;
                fixed4 col = tex2D(_MainTex, i.uv);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
