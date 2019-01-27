Shader "Hidden/O2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_T("T",Float) = 0
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
			float _T;

			float Lumen (fixed4 col){
				return dot(float3(0.2125f, 0.7154f, 0.0721f),col.xyz);
			}
			fixed4 BoxSample(float2 uv, float distance){
				float4 col = tex2D(_MainTex, uv+float2(0,distance));
				col += tex2D(_MainTex, uv+float2(0,-distance));
				col += tex2D(_MainTex, uv+float2(distance,0));
				col += tex2D(_MainTex, uv+float2(-distance,0));

				col += tex2D(_MainTex, uv+float2(distance,distance));
				col += tex2D(_MainTex, uv+float2(-distance,-distance));
				col += tex2D(_MainTex, uv+float2(distance,-distance));
				col += tex2D(_MainTex, uv+float2(-distance,distance));
				return col*0.125;
			}
            fixed4 frag (v2f i) : SV_Target
            {
				float v = length(i.uv*2-1);
				float t = smoothstep(_T-0.4,_T+0.4,v);
                fixed4 col = BoxSample( i.uv,t*0.02);
				//col = tex2D(_MainTex, i.uv);
				float l = Lumen(col);
				col = lerp(col,fixed4(l,l,l,l),t);
                return col*(1-t*0.4)*(1-v*0.4);
            }
            ENDCG
        }
    }
}
