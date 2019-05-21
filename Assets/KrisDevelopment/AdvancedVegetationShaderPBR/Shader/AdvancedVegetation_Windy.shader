// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

//Shader by Hristo Mihailov Ivanov
Shader "Advanced Vegetation/Windy Vegetation" {
	Properties {
		_VegColor ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo", 2D) = "white" {} 
		_BumpMap ("Normal Map", 2D) = "bump" {}
		[KeywordEnum(Off, Face, Back)] _Cull("Culling Mode", float) = 0
		[KeywordEnum(Off, On)] _A2C("A2C", float) = 0
		_BumpIntensity ("Normal Intensity", Range(0,1)) = 1
		[HideInInspector] _Cutoff ("Cutoff", Range(0.1,0.1)) = 0.1
		_Cutoff2 ("Cutoff", Range(0,1)) = 0.5
		_Glossiness ("Smoothness", Range(0.0, 1.0)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Speed("Speed",Range(0.1,10)) = 1
		_Amount("Amount", Range(0.1,10)) = 3
		_Distance("Distance", Range( 0, 0.5)) = 0.1
		_ZMotion("Z Motion", Range( 0, 1)) = 0.5
		_ZMotionSpeed("Z Motion Speed", Range( 0, 10)) = 10
		_OriginWeight("Origin Weight", Range( 0, 1)) = 0
	}
	SubShader {
		Tags {	
			"RenderQueue"="AlphaTest"
			"RenderType"="TransparentCutout"
			"IgnoreProjector"="True"
			"RenderType"="Grass"
			"DisableBatching"="True"}
		LOD 100
		AlphaToMask [_A2C]
		Cull [_Cull]
		CGPROGRAM
		#pragma surface surf Standard alphatest:_Cutoff vertex:vert addshadow
		#pragma target 3.0
		#pragma multi_compile_instancing
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		
		sampler2D _MainTex;
		sampler2D _BumpMap;
		
		UNITY_INSTANCING_BUFFER_START(Props)
           UNITY_DEFINE_INSTANCED_PROP(fixed4, _VegColor)
#define _VegColor_arr Props
        UNITY_INSTANCING_BUFFER_END(Props)

		struct Input {
			fixed2 uv_MainTex;
			fixed2 uv_BumpMap;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		
		half _BumpIntensity;
		half _Glossiness;
		half _Metallic;

		fixed _Speed;
		fixed _Amount;
		fixed _Distance;
		fixed _ZMotion;
		fixed _ZMotionSpeed;
		fixed _OriginWeight;
		fixed _Cutoff2;
		
		fixed4 getNewVertPosition( fixed4 nv1 )
		{
			fixed4 nv = mul( unity_ObjectToWorld, nv1 );
			fixed4 objectOrigin = mul(unity_ObjectToWorld, fixed4(0,0,0,1));
			fixed _DistanceFromOrigin = distance(objectOrigin.y, nv.y);
			fixed _anchored = sin(_Time.y * _Speed + nv.y * _Amount ) * _Distance * (_DistanceFromOrigin/3);
			fixed _unanchored = sin(_Time.y * _Speed + nv.y * _Amount ) * _Distance;
			fixed _nxz = _ZMotion *  sin(_Time.y *(_Speed/10 * _ZMotionSpeed) + nv.y * _Amount * (_ZMotionSpeed/10) );
			nv.x += lerp(_unanchored,_anchored, _OriginWeight) * (1-_nxz);
			nv.z += lerp(_unanchored,_anchored, _OriginWeight) * (_nxz);
			nv1 = mul( unity_WorldToObject, nv );
			return nv1;
		}
		
		void vert( inout appdata_full v )
		{
			UNITY_SETUP_INSTANCE_ID (v);
			fixed4 newVert = getNewVertPosition(v.vertex);
			v.vertex = newVert;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 tex = tex2D (_MainTex, IN.uv_MainTex) * UNITY_ACCESS_INSTANCED_PROP(_VegColor_arr, _VegColor);
			o.Albedo = tex.rgb; 
			fixed3 normal2 = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Normal = lerp(fixed3(0,0,1), normal2, _BumpIntensity);
			o.Smoothness = _Glossiness;
			o.Metallic = _Metallic;
			tex.a = (tex.a - _Cutoff2) / max(fwidth(tex.a), 0.0001) + 0.5;
			o.Alpha = tex.a;
		}
		 
		ENDCG
	}
	Fallback "Legacy/Transparent/Cutout/VertexLit"
}
