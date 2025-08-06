Shader "DissolverShader/DissolveShader" {
    Properties {
        [Header(Main)]
        [Space]
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _NormalStrength ("Normal Strength", Range(0, 1.5)) = 0.5
        _MetallicMap ("Metallic Map", 2D) = "white" {}
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _OcclusionMap ("Occlusion Map", 2D) = "white" {}
        _OcclusionStrength ("Strength", Range(0.000000,1.000000)) = 1.000000
        [HDR] _EmissionColor ("Emission Color", Color) = (0,0,0,1)
        _EmissionMap ("Emission Map", 2D) = "white" {}

        [Header(Dissolve)]
        [Space]
        _DissolveColor ("Dissolve Color", Color) = (1,1,1,1)
        _DissolveMap ("Dissolve Map", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0,1)) = 0
        _DissolveWidth ("Dissolve Width", Range(0,0.1)) = 0.05
        _DissolveEmission ("Dissolve Emission", Range(0,1)) = 1
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 300

        CGPROGRAM
        #pragma surface surf Standard addshadow
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _NormalMap;
        sampler2D _DissolveMap;
        sampler2D _MetallicMap;
        sampler2D _OcclusionMap;
        sampler2D _EmissionMap;

        struct Input {
            float2 uv_MainTex;
            float2 uv_NormalMap;
            float2 uv_DissolveMap;
            float2 uv_OcclusionMap;
            float2 uv_MetallicMap;
            float2 uv_EmissionMap;
        };

        half _DissolveAmount;
        half _NormalStrength;
        half _Glossiness;
        half _Metallic;
        half _OcclusionStrength;
        half _DissolveWidth;
        half _DissolveEmission;
        fixed4 _Color;
        fixed4 _EmissionColor;
        fixed4 _DissolveColor;

        void surf (Input IN, inout SurfaceOutputStandard o) {
            // Main Texture.
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            
            // Normal Mapping.
            o.Normal = UnpackScaleNormal(tex2D(_NormalMap, IN.uv_NormalMap), _NormalStrength);

            // Dissolve.
            fixed4 mask = tex2D(_DissolveMap, IN.uv_DissolveMap);
            if(mask.r < _DissolveAmount)
            {
                discard;
            }

            if(mask.r < _DissolveAmount + _DissolveWidth)
            {
                o.Albedo = _DissolveColor;
                o.Emission = _DissolveColor * _DissolveEmission;
                return;
            }

            // Metallic and Smoothness.
            float4 metallic = tex2D(_MetallicMap, IN.uv_MetallicMap) * _Metallic;
            o.Metallic = metallic.rgb; 
            o.Smoothness = _Glossiness * metallic.a;

            // Occlusion.
            o.Occlusion = lerp(1.0, tex2D(_OcclusionMap, IN.uv_OcclusionMap), _OcclusionStrength);

            // Emission.
            o.Emission = tex2D(_EmissionMap, IN.uv_EmissionMap) * _EmissionColor;
        }
        ENDCG
    }

    FallBack "VertexLit"
}
