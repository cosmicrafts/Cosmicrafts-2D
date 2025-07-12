Shader "ErbGameArt/Unlit/ElectroSphere" {
    Properties {
        _Maintexture ("Main texture", 2D) = "black" {}
        _Noise ("Noise", 2D) = "black" {}
        _Color ("Color", Color) = (0,0,0,1)
        _Color2 ("Color2", Color) = (0,0.462069,1,1)
        _Emission ("Emission", Float ) = 2
        _Noisespeed ("Noise speed", Float ) = 0.5
        _Sideglow ("Side glow", Float ) = 6
        _Glowmult ("Glow mult", Float ) = 0.5
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull off
			
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            uniform float4 _Color;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform sampler2D _Maintexture; uniform float4 _Maintexture_ST;
            uniform float _Noisespeed;
            uniform float _Sideglow;
            uniform float _Glowmult;
            uniform float4 _Color2;
            uniform float _Emission;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float2 ccc = (i.uv0*2.0+-1.0);
                float Cir = length(ccc);
                float2 uuu = ((i.uv0*0.5)+_Time.g*float2(1,1));
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(uuu, _Noise));
                float2 nnn = ccc.rg;
                float2 hhh = float2(((Cir*0.1)+(_Time.g*_Noisespeed)),(((_Noise_var.r*2.0+-1.0)*0.1)+((atan2(nnn.g,nnn.r)/6.28318530718)+0.5)));
                float4 _Maintexture_var = tex2Dlod(_Maintexture,float4(TRANSFORM_TEX(hhh, _Maintexture),0.0,1.0));
                float xxx = pow(Cir,_Sideglow);
                float zzz = saturate(((pow((_Maintexture_var.g+(_Maintexture_var.r*xxx)+(xxx*_Glowmult)),1.5)+(1.0 - saturate((Cir*10.0))))*(1.0 - floor(Cir))*1.5));
                float fff = (zzz*i.vertexColor.a*_Emission);
                float3 emissive = (saturate((lerp(_Color.rgb,_Color2.rgb,zzz)*_Emission))*i.vertexColor.rgb*fff);
                return fixed4(emissive,fff);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
