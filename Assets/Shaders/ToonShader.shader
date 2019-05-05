Shader "Custom/ToonShader"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}

		// Ambient light
		[Space]
		[Header(Ambient Light)]
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		_AmbientSmoothing("Shadow Smoothing", Range(0.0, 0.1)) = 0.01

		// Specular reflection
		[Space]
		[Header(Specular Reflection)]
		[HDR]
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Glossiness("Glossiness", Float) = 32
		_SpecularSmoothing("Specular Smoothing", Range(0.005, 0.1)) = 0.01

		// Rim lighting
		[Space]
		[Header(Rim Lighting)]
		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
		_RimSmoothing("Rim Smoothing", Range(0.0, 0.1)) = 0.01

	}
	SubShader
	{
		Pass
		{
			Tags
			{
				"LightMode" = "ForwardBase"
				"PassFlags" = "OnlyDirectional"
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;				
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				SHADOW_COORDS(2)

				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = WorldSpaceViewDir(v.vertex);

				TRANSFER_SHADOW(o)

				return o;
			}
			
			float4 _Color;

			// Ambient light
			float4 _AmbientColor;
			float _AmbientSmoothing;

			// Specular reflection
			float4 _SpecularColor;
			float _Glossiness;
			float _SpecularSmoothing;

			// Rim lighting
			float4 _RimColor;
			float _RimAmount;
			float _RimThreshold;
			float _RimSmoothing;

			float4 frag (v2f i) : SV_Target
			{
				// Ambient light
				float3 normal = normalize(i.worldNormal);
				float NdotL = dot(_WorldSpaceLightPos0, normal);

				float shadow = SHADOW_ATTENUATION(i);

				float lightIntensity = smoothstep(0, _AmbientSmoothing, NdotL * shadow);
				float4 light = lightIntensity * _LightColor0;
				
				// Specular reflection
				float3 viewDir = normalize(i.viewDir);

				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);

				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				float specularIntensitySmooth = smoothstep(0.005, _SpecularSmoothing, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;

				// Rim lighting
				float4 rimDot = 1 - dot(viewDir, normal);
				float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
				rimIntensity = smoothstep(_RimAmount - _RimSmoothing, _RimAmount + _RimSmoothing, rimIntensity);
				float4 rim = rimIntensity * _RimColor;

				float4 sample = tex2D(_MainTex, i.uv);

				return _Color * sample * (_AmbientColor + light + specular + rim);
			}
			ENDCG

		}

		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}