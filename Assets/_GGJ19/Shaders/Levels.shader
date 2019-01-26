Shader "Unlit/Levels"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MetaTex ("Meta Mask", 2D) = "black" {}
		_Resource  ("Resources", Vector) = (0,0,0,0)
		_RedCol  ("Resource 1 Color", Color)  = (1,0,0,0)
		_GreenCol ("Resource 2 Color", Color) = (0,1,0,0)
		_BlueCol ("Resource 3 Color", Color)  = (0,0,1,0)
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
            sampler2D _MetaTex;
			fixed4 _RedCol;
			fixed4 _GreenCol;
			fixed4 _BlueCol;
			float4 _Resource;
            float4 _MainTex_ST;

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
				// sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 meta = tex2D(_MetaTex, i.uv);

				fixed4 extra = float4(
					meta.r * smoothstep((1-_Resource.r)-0.05, (1-_Resource.r), (1-i.uv.y)),
					meta.g * smoothstep((1-_Resource.g)-0.05, (1-_Resource.g), (1-i.uv.y)),
					meta.b * smoothstep((1-_Resource.b)-0.05, (1-_Resource.b), (1-i.uv.y)),
					0
				);

				extra.a = max(max(extra.r, extra.g), extra.b);

				//col = lerp(col, (_RedCol * extra.r), .5);
				//col = lerp(col, (_GreenCol * extra.g), .5);
				//col = lerp(col, (_BlueCol * extra.b), .5);

                // apply fog
				//return fixed4(i.uv.y,1,1,1);
                //return lerp(col, meta,.5);
				return lerp(col, extra, extra.a);
            }
            ENDCG
        }
    }
}
