Shader "StylizedWater/Hole" 
{
	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent-1" "IgnoreProjector" = "True" "ForceNoShadowCasting" = "True" }
		ColorMask 0
		ZWrite On
		
		Pass 
		{
			Name "Depth mask"
			
			HLSLPROGRAM
			#pragma multi_compile_instancing
			
			#include "UnityCG.cginc"

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };
			
			#pragma vertex vert
            #pragma fragment frag

			Varyings vert(float4 positionOS : POSITION)
            {
                Varyings output;

                output.positionCS = UnityObjectToClipPos(positionOS.xyz);

                return output;
            }
			
			half4 frag() : SV_Target { return 0; }
			
			ENDHLSL
		}
	}
}