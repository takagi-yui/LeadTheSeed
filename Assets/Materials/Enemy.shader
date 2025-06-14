﻿Shader "Unlit/Enemy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_A("A", Float) = 1
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Cull Off
		Pass
        {
		Tags { "Queue"="Transparent" "RenderType" = "Transparent" "PreviewType" = "Plane"}
		Stencil{
			Ref 0
			Comp always
			Pass IncrSat
		}
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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - 0.5);
				col.a = 0;
                return col;
            }
            ENDCG
        }
        Pass
        {
		Tags { "Queue"="Transparent+1" "RenderType" = "Transparent" "PreviewType" = "Plane"}
		Stencil{
			Ref 1
			Comp Equal
			Pass Keep
			Fail DecrSat
		}
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
			float _A;
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
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - 0.5);
				col.a = _A;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
