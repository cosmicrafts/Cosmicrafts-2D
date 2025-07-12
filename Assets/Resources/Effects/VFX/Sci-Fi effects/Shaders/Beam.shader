Shader "ErbGameArt/Particles/Blend_WindBeam" {
    Properties {
        _Centerpower ("Center power", Float ) = 4
        _MainTexture ("Main Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,0.5586207,0,1)
        _Mainbeampower ("Main beam power", Float ) = 15
        _WindColor ("Wind Color", Color) = (1,1,1,1)
        _Particles ("Particles", 2D) = "white" {}
        _Particlescolor ("Particles color", Color) = (0.6137934,0,1,1)
        _Speed ("Speed", Vector) = (-1,0,1,0)
        _Beampower ("Beam power", Float ) = 2
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
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Centerpower;
            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
            uniform float4 _Color;
            uniform float _Mainbeampower;
            uniform float4 _WindColor;
            uniform sampler2D _Particles; uniform float4 _Particles_ST;
            uniform float4 _Particlescolor;
            uniform float4 _Speed;
            uniform float _Beampower;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float2 node_8027 = ((float2(_Speed.r,_Speed.g)*_Time.g)+i.uv0);
                float4 _Particles_var = tex2D(_Particles,TRANSFORM_TEX(node_8027, _Particles));
                float node_5557 = saturate((i.uv0.g*(1.0 - i.uv0.g)*4.0));
                float node_5056 = saturate(pow(node_5557,_Mainbeampower));
                float node_8495 = (_Particles_var.r*pow(node_5056,0.5));
                float2 node_7518 = float2(_Speed.b,_Speed.a);
                float2 node_9285 = ((node_7518*_Time.g)+i.uv0);
                float4 node_139 = tex2D(_MainTexture,TRANSFORM_TEX(node_9285, _MainTexture));
                float2 node_3817 = ((node_7518*_Time.b)+i.uv0);
                float4 node_1283 = tex2D(_MainTexture,TRANSFORM_TEX(node_3817, _MainTexture));
                float3 node_9897 = saturate((saturate((pow(node_5557,_Centerpower)*_Beampower))+(node_139.rgb*node_1283.rgb)));
                float3 emissive = ((node_8495*_Particlescolor.rgb)+saturate((saturate(((_Color.rgb*node_5056)+((node_9897-node_5056)*_WindColor.rgb)))-node_8495)));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,(node_9897.r*i.vertexColor.a));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
}
