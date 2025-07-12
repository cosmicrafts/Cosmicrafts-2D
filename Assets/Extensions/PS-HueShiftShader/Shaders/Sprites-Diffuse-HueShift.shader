// Writen by Martin Nerurkar ( www.playful.systems). MIT license (see license.txt)
// Based on Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
// Inspired by HSV Shader for Unity from Gregg Tavares (https://github.com/greggman/hsva-unity). MIT License (see license.txt)

Shader "Sprites/Diffuse-HueShift"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
	
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

		[HideInInspector] _HueRangeMin("Min Hue Range", Range(0, 1)) = 0
		[HideInInspector] _HueRangeMax("Max Hue Range", Range(0, 1)) = 1
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
		#pragma multi_compile _ PIXELSNAP_ON
		#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
		#pragma multi_compile __ PS_HSV_ALPHAMASK_ON
		#pragma multi_compile __ PS_HSV_HUERANGE_ON
		#include "UnitySprites.cginc"
		#include "HueShift.cginc"

		struct Input
		{
			float2 uv_MainTex;
			fixed4 color;
		};

        float _HueRangeMin;
        float _HueRangeMax;

		void vert (inout appdata_full v, out Input o)
		{
			v.vertex.xy *= _Flip.xy;

			#if defined(PIXELSNAP_ON)
			v.vertex = UnityPixelSnap (v.vertex);
			#endif

			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = v.color * _RendererColor;
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 c = applyHSV(SampleSpriteTexture (IN.uv_MainTex), IN.color, _HueRangeMin, _HueRangeMax);
			o.Albedo = c.rgb * c.a;
			o.Alpha = c.a;
		}
		ENDCG
	}

	CustomEditor "HueShiftMaterialInspector"
	Fallback "Transparent/VertexLit"
}
