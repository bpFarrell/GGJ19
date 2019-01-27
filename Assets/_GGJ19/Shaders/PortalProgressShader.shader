Shader "Unlit/PortalProgressShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_T("T",Float)=0
		_ColorOn("Color On",Color)=(1,1,1,1)
		_ColorOff("Color Off",Color)=(.1,.1,.1,.1)
		_OutEdge("OutEdge",Float) = 0.85
		_InEdge("InEdge",Float) = 0.6
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
			fixed4 _ColorOn;
			fixed4 _ColorOff;
			float _T;
			float _InEdge;
			float _OutEdge;
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
				float PI = 3.141529;

				float2 uv = (i.uv-0.5)*2;
				if(length(uv)>_OutEdge||length(uv)<_InEdge)
					discard;

				_T=(_T-0.5)*-PI*2;
				float a = atan2(uv.x,uv.y);
				a = smoothstep(_T-0.1,_T+0.1,a);

                return lerp(_ColorOff,_ColorOn,a);
            }
            ENDCG
        }
    }
}
