Shader "Hidden/Greyscale" {
    Properties {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (0,0,0,1)
    }
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform float _NormalisedProgress;

            uniform sampler2D _MainTex;
            uniform float4 _Color;

            float4 frag (v2f_img i) : SV_Target {
                float4 color = tex2D(_MainTex, i.uv);

                float4 result = color;
                result.rgb = lerp(color.rgb, _Color.rgb, _NormalisedProgress);
                
                return result;
            }
            ENDCG
        }
    }
}
