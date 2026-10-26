Shader "Custom/DimHoleShader"
{
    Properties
    {
        _Color ("Dim Color", Color) = (0,0,0,0.7)
        _HoleCenter ("Hole Center", Vector) = (0.5,0.5,0,0)
        _HoleSize ("Hole Size", Vector) = (0.2,0.2,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _Color;
            float4 _HoleCenter;
            float4 _HoleSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Calculate distance from hole center
                float2 pos = i.uv;
                float2 center = _HoleCenter.xy;
                float2 size = _HoleSize.xy * 0.5;
                float2 diff = abs(pos - center);

                // If inside the hole, make transparent
                if (diff.x <= size.x && diff.y <= size.y)
                    discard;

                return _Color;
            }
            ENDCG
        }
    }
}
