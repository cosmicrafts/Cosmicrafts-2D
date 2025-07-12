Shader "ErbGameArt/ShieldShader" {
	Properties{
		_MainTex ("MainTex", 2D) = "white" {}
		[HideInInspector]_BumpMap ("Bumpmap", 2D) = "bump" {}
		_Fresnel ("Fresnel", Float ) = 3	
		_Color("Color", Color) = (1,1,1,1)
        _Fresnelemission ("Fresnel emission", Float ) = 0.7
        _Textureemission ("Texture emission", Float ) = 0
		_RingColor("Ring Color", Color) = (1,1,1,1)
		_RingColorIntensity("Ring Color Intensity", float) = 2
		_RingSpeed("Ring Speed", float) = 1
		_RingWidth("Ring Width", float) = 0.1
		_RingIntensityScale("Ring Range", float) = 1
		_RingTex("Ring Texture", 2D) = "white" {}
	}
	SubShader{
	Tags{ "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent" }
				 Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
	Blend One One
	ZWrite Off
	CGPROGRAM
	#pragma surface surf Standard fullforwardshadows

	sampler2D _MainTex;
	sampler2D _RingTex;
	sampler2D _BumpMap;

	struct Input {
		float2 uv_BumpMap;
		float2 uv_MainTex;
		float3 worldPos;
		float4 vertexColor : COLOR;
		float3 viewDir;
	};

	half4 _hitPts[20];
	half _StartTime;

    float _Fresnelemission;
    float _Textureemission;
    float _Fresnel;
	fixed4 _Color;
	fixed4 _RingColor;
	half _RingColorIntensity;
	half _RingSpeed;
	half _RingWidth;
	half _RingIntensityScale;


	void surf(Input IN, inout SurfaceOutputStandard o) 
	{	
		o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
        half rim = pow(1.0 - saturate(dot (normalize(IN.viewDir), o.Normal)),3);
		fixed4 c = ((tex2D(_MainTex, IN.uv_MainTex) * _Color * _Textureemission) + (_Color * _Fresnelemission * rim));
		o.Albedo = c.rgb;

		half DiffFromRingCol = abs(o.Albedo.r - _RingColor.r) + abs(o.Albedo.b - _RingColor.b) + abs(o.Albedo.g - _RingColor.g);

		for (int i = 0; i < 20; i++) {
			half d = distance(_hitPts[i], IN.worldPos);
			half val = (1 - (d / _RingIntensityScale));

			if (d < (_Time.y - _hitPts[i].w) * _RingSpeed && d >(_Time.y - _hitPts[i].w) * _RingSpeed - _RingWidth && val > 0) {
				half posInRing = (d - ((_Time.y - _hitPts[i].w) * _RingSpeed - _RingWidth)) / _RingWidth;

				float angle = acos(dot(normalize(IN.worldPos - _hitPts[i]), float3(1,0,0)));
				val *= tex2D(_RingTex, half2(1 - posInRing, angle));
				half3 tmp = _RingColor * val + c * (1 - val);

				half tempDiffFromRingCol = abs(tmp.r - _RingColor.r) + abs(tmp.b - _RingColor.b) + abs(tmp.g - _RingColor.g);
				if (tempDiffFromRingCol < DiffFromRingCol)
				{
					DiffFromRingCol = tempDiffFromRingCol;
					o.Albedo.r = tmp.r;
					o.Albedo.g = tmp.g;
					o.Albedo.b = tmp.b;
					o.Albedo.rgb *= _RingColorIntensity;
				}
			}
		}
	}
	ENDCG
	}
}
