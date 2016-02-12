// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'

Shader "Custom/skyboxShader" {
	Properties 
	{
		_Tex1 ("Skybox Texture", 2D) = "white" {}
	}
	SubShader 
	{
		Tags { "Queue" = "Background" }

		pass
		{
			ZWrite Off
			CULL FRONT	
			
			CGPROGRAM
			#pragma vertex   vertex_shader
			#pragma fragment fragment_shader
			 
			#include "UnityCG.cginc"

			uniform sampler2D _Tex1;

			struct a2v
			{
			    float4 vertex   : POSITION;
			    float4 texcoord : TEXCOORD0;
			};
			 
			struct v2f
			{
			    float4 position : POSITION;
			    float4 texcoord : TEXCOORD1;
			};
			 
			v2f vertex_shader(a2v IN)
			{
			    v2f OUT;
			   
			    OUT.position = mul(UNITY_MATRIX_MVP, IN.vertex);
			    OUT.texcoord     = IN.texcoord;
			    return OUT;
			}
			 
			float4 fragment_shader(   v2f IN)
			                        //uniform sampler2D _Tex1,
			                        //out float4 col:COLOR,
			                        //out float depth:DEPTH )
			{
				float4 col;
			    col.xyz = float3(1.0f,0.0f,0.0f);
			    col.w = 1;
			    //depth = 1;
				return col;
			}
			ENDCG
		}
	}
}
