Shader "Custom/TerrainShader" {
	Properties {
		_Water ("Water", 2D) = "white" {}
		_Sand ("Sand", 2D) = "white" {}
		_Grass ("Rock", 2D) = "white" {}
		_Rock ("Rock", 2D) = "white" {}
		_WaterLevel ("Water Level", Float) = 0
		_LayerSize ("Layer Size", Float) = 20
		_BlendRange ("Blend Range", Range(0, 0.5)) = 0.1
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
     
            uniform sampler2D _Water;
            uniform sampler2D _Sand;
            uniform sampler2D _Grass;
            uniform sampler2D _Rock;
			
			uniform float4 _Water_ST;
            uniform float4 _Sand_ST;
            uniform float4 _Grass_ST;
            uniform float4 _Rock_ST;

            uniform float _WaterLevel;
            uniform float _LayerSize;
            uniform float _BlendRange;

			struct fragmentInput {
				float4 pos : SV_POSITION;
                float4 texcoord : TEXCOORD0;
				float4 blend: COLOR;
			};
      
			fragmentInput vert (appdata_full v)
			{
				float NumOfTextures = 4;
				fragmentInput o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                o.texcoord = v.texcoord;

				//  |-----------|--------|--------|------------------|
				//  +   Water   L   Sand    Green    Rock            0
				//     |--------|--------|--------|--------|
				//     0                                   1

				float MinValue = _WaterLevel - (NumOfTextures - 1) * _LayerSize; 
				float MaxValue = (_WaterLevel + _LayerSize); 
				float Blend = MaxValue - v.vertex.z;
				Blend = clamp(Blend / (NumOfTextures *_LayerSize), 0, 1);

				o.blend.xyz = 0;
				o.blend.w = Blend;

				o.blend.x = v.color.a;  //S alpha from Depth Mesh as value for Layer
				return o;
			}
			

			inline float CalculateBlend(float TextureFloat)
			{
				return  1 - clamp((1 - TextureFloat) / _BlendRange, 0 , 1);
			}

			inline float4 DoBlending(float TextureID, float TextureFloat, fixed4 BaseTexture, fixed4 BlendTexture)
			{
				float Blend = CalculateBlend(clamp(TextureFloat - TextureID, 0 , 1));
				return lerp(BaseTexture, BlendTexture, Blend);
			} 

			float4 frag (fragmentInput i) : COLOR0 
			{ 	
				float NumOfTextures = 4;
				//float TextureFloat = i.blend.w * NumOfTextures; //old
				float TextureFloat = i.blend.x * 256;         // new from DepthMesh


				if(TextureFloat < 1)
				{
					fixed4 WaterColor = tex2D(_Water, i.texcoord * _Water_ST.xy + _Water_ST.zw);
					fixed4 SandColor = tex2D(_Sand, i.texcoord * _Sand_ST.xy + _Sand_ST.zw);

					return DoBlending(0, TextureFloat, WaterColor, SandColor);
				} 
				else if(TextureFloat < 2)
				{
					fixed4 SandColor = tex2D(_Sand, i.texcoord * _Sand_ST.xy + _Sand_ST.zw);
					fixed4 GrassColor = tex2D(_Grass, i.texcoord * _Grass_ST.xy + _Grass_ST.zw);

					return DoBlending(1, TextureFloat, SandColor, GrassColor);
				} 
				else if(TextureFloat < 3)
				{
					fixed4 GrassColor = tex2D(_Grass, i.texcoord * _Grass_ST.xy + _Grass_ST.zw);
					fixed4 RockColor = tex2D(_Rock, i.texcoord * _Rock_ST.xy + _Rock_ST.zw);

					return DoBlending(2, TextureFloat, GrassColor, RockColor);
				}
				
				fixed4 RockColor = tex2D(_Rock, i.texcoord * _Rock_ST.xy + _Rock_ST.zw);

				return RockColor;

				fixed4 WaterColor = tex2D(_Water, i.texcoord * _Water_ST.xy + _Water_ST.zw);
				fixed4 SandColor = tex2D(_Sand, i.texcoord * _Sand_ST.xy + _Sand_ST.zw);

				return lerp(WaterColor, SandColor, i.blend.w);
			}

      ENDCG
    }
  } 
	FallBack "Diffuse"
}
