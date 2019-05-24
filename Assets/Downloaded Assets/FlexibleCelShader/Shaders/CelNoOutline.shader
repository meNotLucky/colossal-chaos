Shader "FlexibleCelShader/Cel No Outline"
{
	Properties
	{
		_Color("Global Color Modifier", Color) = (1, 1, 1, 1)
		[NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
		[NoScaleOffset] _EmmisTex("Emission", 2D) = "black" {}
		
		_RampLevels("Ramp Levels", Range(2, 50)) = 2
		_LightScalar("Light Scalar", Range(0, 10)) = 1

		_HighColor("High Light Color", Color) = (1, 1, 1, 1)
		_HighIntensity("High Light Intensity", Range(0, 10)) = 1.5

		_LowColor("Low Light Color", Color) = (1, 1, 1, 1)
		_LowIntensity("Low Light Intensity", Range(0, 10)) = 1

		_OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
		_OutlineSize("Outline Size", float) = 10

		_RimColor("Hard Edge Light Color", Color) = (1, 1, 1, 1)
		_RimAlpha("Hard Edge Light Brightness", Range(0, 1)) = 0
		_RimPower("Hard Edge Light Size", Range(0,1)) = 0
		_RimDropOff("Hard Edge Light Dropoff", range(0, 1)) = 0

		_FresnelColor("Soft Edge Light Color", Color) = (1,1,1,1)
		_FresnelBrightness("Soft Edge Light Brightness", Range(0, 1)) = 0
		_FresnelPower("Soft Edge Light Size", Range(0, 1)) = 0
		_FresnelShadowDropoff("Soft Edge Light Dropoff", range(0, 1)) = 0

		_GradYCap("Vertical Gradient Cap", Range(0, 10)) = 1
		_GradIntensity("Vertical Gradient Intensity", Range(0, 1)) = 0.3
	}
	
	SubShader
	{
		
		// This pass renders the object
		Cull back
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
			#include "AutoLight.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				SHADOW_COORDS(1)
				float3 locPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				float3 worldPos : TEXCOORD4;
				float3 screenPos : TEXCOORD5;
				fixed4 diffuse : COLOR0;
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata_base v)
			{
				float4 worldVertPos = mul(unity_ObjectToWorld, v.vertex);

				v2f o;
				o.locPos = v.vertex;
				o.pos = mul(UNITY_MATRIX_VP, worldVertPos);
				o.uv = v.texcoord;
				o.screenPos = ComputeScreenPos(UnityObjectToClipPos(v.vertex));

				// Get normal
				o.worldNormal = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);

				// Get position in world space
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				// Get vertex normal in world space
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				
				// Dot product between normal and light direction for diffuse lighting
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				
				// Factor in the light color
				o.diffuse = nl * _LightColor0;
				
				// Compute shadows data
				TRANSFER_SHADOW(o);

				return o;
			}

			float4    _Color;
			sampler2D _MainTex;
			sampler2D _EmmisTex;
			int       _RampLevels;
			float     _LightScalar;
			float     _HighIntensity;
			float4    _HighColor;
			float     _LowIntensity;
			float4    _LowColor;
			float     _GradYCap;
			float     _GradIntensity;
			float     _RimPower;
			float	  _RimAlpha;
			float4    _RimColor;
			float     _RimDropOff;
			float     _FresnelBrightness;
			float     _FresnelPower;
			float4    _FresnelColor;
			float     _FresnelShadowDropoff;

			fixed4 frag(v2f i) : SV_Target
			{
				_RampLevels -= 1;

				// Get view direction && light direction for rim lighting
				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);

				// Sample textures
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 emmision = tex2D(_EmmisTex, i.uv);

				// Rim Lighting
				half factor = dot(viewDirection, i.worldNormal);
				half fresnelFactor = 1 - min(pow(max(1 - factor, 0), (1 - _FresnelPower) * 10), 1);
				//half fresnelFactor = 1 - ((1-factor) * _FresnelPower);

				//float3 rim = pow(1.0 - saturate(factor), _RimPower);
				//float3 rimLighting = saturate(dot(i.worldNormal, lightDirection)) * rim;

				// Get shadow attenuation
				fixed shadow = SHADOW_ATTENUATION(i);

				// Calculate light intensity
				float intensity = clamp(i.diffuse.b * _LightScalar, 0, 1);
				
				// Factor in the shadow
				intensity *= shadow;
				
				// Determine level
				float rampLevel = round(intensity * _RampLevels);
				
				// Get light multiplier based on level
				float lightMultiplier = _LowIntensity + ((_HighIntensity - _LowIntensity) / (_RampLevels)) * rampLevel;

				// Get color multiplier based on level
				float4 highColor = (rampLevel / _RampLevels) * _HighColor;
				float4 lowColor = ((_RampLevels - rampLevel) / _RampLevels) * _LowColor;
				float4 mixColor = (highColor + lowColor) / 2;
				
				// Apply light multiplier and color
				col *= lightMultiplier;
				col *= _Color * mixColor;

				// Apply vertical gradient
				//col *= (1 - _GradIntensity / 2 + clamp(i.locPos.y, 0, _GradYCap)*_GradIntensity);

				// Apply soft Fresnel
				float rampPercentSoftFresnel = 1 - ((1 - rampLevel / _RampLevels) * (1 - _FresnelShadowDropoff));
				col.rgb = col.rgb + _FresnelColor*(_FresnelBrightness*10 - fresnelFactor*_FresnelBrightness*10) * rampPercentSoftFresnel;

				// Apply hard rim lighting
				_RimAlpha *= 1 - ((1 - rampLevel / _RampLevels) * (1 - _RimDropOff));
				if (factor <= _RimPower) {
					col.rgb = _RimColor.rgb * _RimAlpha + col.rgb * (1 - _RimAlpha);
				}

				// Apply emmision lighting
				half eIntensity = max(emmision.r, emmision.g);
				eIntensity = max(eIntensity, emmision.b);
				col = emmision*eIntensity + col*(1 - eIntensity);

				return col;
			}
				
			ENDCG
		} // End Main Pass


		// Shadow casting
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}

	CustomEditor "CelCustomEditor"
}