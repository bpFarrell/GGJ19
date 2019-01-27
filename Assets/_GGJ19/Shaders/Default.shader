Shader "Unlit/Default"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Meta ("Meta", 2D) = "white" {}
		_LightColor("LightColor",Color)= (1,1,1,1)
		_BaseColor("BaseColor",Color)= (1,1,1,1)
		_FullRoom("FullRoom",Color)= (1,1,1,1)
		_EnergySpeed("EnergySpeed",Vector)=(1,1,1,1)
		_EnergyPower("EnergyPower",Vector)=(1,1,1,1)
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
				float4 localPos : TOEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Meta;
			fixed4 _LightColor;
			fixed4 _BaseColor;
			fixed4 _EnergyPower;
			fixed4 _EnergySpeed;
			fixed4 _FullRoom;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.localPos = v.vertex;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o; 
            }
            fixed4 frag (v2f i) : SV_Target
            {
				
				float4 axisTable = float4(
					0,2,0,2
				);


				fixed4 region = fixed4(
				abs(i.localPos.x)>abs(i.localPos.z)&i.localPos.x<0,
				abs(i.localPos.x)<abs(i.localPos.z)&i.localPos.z>0,
				abs(i.localPos.x)>abs(i.localPos.z)&i.localPos.x>0,
				abs(i.localPos.x)<abs(i.localPos.z)&i.localPos.z<0);
				float power = 0;
				for(int x = 0;x<4;x++){
					power += sin(_EnergySpeed[x]*_Time.x*10+i.localPos[axisTable[x]])*region[x]*_EnergyPower[x];

				}
				//int temp = 2;
				//test = sin(dirs[temp]*_Time.x*20+i.localPos[axisTable[temp]])*region[temp];
				power=power*0.5+0.5;
				power = floor(power*7.99)*0.19;
				//return fixed4(i.localPos.xyzz);


                fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 meta = tex2D(_Meta,i.uv);
				fixed4 light = meta.r*(sin(_Time.x*600)*0.3+0.8) * _LightColor;
				fixed4 energy = meta.b*(power) * fixed4(0.85,0.85,0.3,1.0);
				fixed4 baseColor = lerp(col,col*_BaseColor,meta.g)*_FullRoom;
                return (baseColor)+light+energy;
            }
            ENDCG
        }
    }
}
