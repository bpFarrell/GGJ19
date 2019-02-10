Shader "Unlit/Levels"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "black" {}
		_MetaTex ("Meta Mask", 2D) = "black" {}
		_Resource("Resources", Vector) = (0,0,0,0)
		_ResourceCharge("Charge", Vector) = (0,0,0,0)
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
			float4 _ResourceCharge;
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
				// sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 meta = tex2D(_MetaTex, i.uv)*0.9;

				fixed4 extra = float4(
					meta.r * smoothstep((1-_Resource.r)-0.05, (1-_Resource.r), (1-i.uv.y)),
					meta.g * smoothstep((1-_Resource.g)-0.05, (1-_Resource.g), (1-i.uv.y)),
					meta.b * smoothstep((1-_Resource.b)-0.05, (1-_Resource.b), (1-i.uv.y)),
					0
				);

				extra.a = max(max(extra.r, extra.g), extra.b);

				fixed4 filled = lerp(col, extra, extra.a) + meta * 0.25;
				
				fixed3 block = fixed3(
					i.uv.x<0.333,
					i.uv.x<0.666 && i.uv.x>0.333,
					i.uv.x>.666);
				float PI = 3.141529;
				float arrow = _Time.x * -100 + (i.uv.y-abs(sin(i.uv.x*PI*3))) * 10;
				block *= _ResourceCharge * sin(arrow)*0.2+1;
				fixed4 final = 
					block.x * _RedCol +
					block.y * _GreenCol +
					block.z * _BlueCol;
				return filled * final;
            }
            ENDCG
        }
    }
}
