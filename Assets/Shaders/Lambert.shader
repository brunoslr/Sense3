Shader "Custom Shader/ 2 - Lambert" {


			Properties {
			_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
			_AnimSpeed("Animation Speed", Float) = 10.0
			_AnimFreq("Animation Frequency", Float) = 1.0
			_AnimPowerX("Animation Power X", Float) =  0.0
			_AnimPowerY("Animation Power Y", Float) = 0.1
			_AnimPowerZ("Animation Power Z", Float) = 0.1
			_AnimOffsetX("Animation Offset X", Float) = 10.0
			_AnimOffsetY("Animation Offset Y", Float) = 10.0
			_AnimOffsetZ("Animation Offset Z", Float) = 10.0

			}

	SubShader {
				Pass {
				Tags{ "LightMode" = "ForwardBase"}
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag

					// User defined variables
					uniform float4 _Color;

					// Unity Defined variables
					uniform float4 _LightColor0;
					uniform float time;
					uniform half _AnimSpeed;
					uniform half _AnimFreq;
					uniform half _AnimPowerX;
					uniform half _AnimPowerY;
					uniform half _AnimPowerZ;
					uniform half _AnimOffsetX;
					uniform half _AnimOffsetY;
					uniform half _AnimOffsetZ;

					//float4x4 _Object2World;
					//float4x4 _World2DObject;


					struct vertexInput{
						float4 vertex : POSITION;
						float3 normal : NORMAL;

					};
					struct vertexOutput{
						float4 pos : SV_POSITION;
						float4 col : COLOR;
					};

					// vertex fun
					vertexOutput vert(vertexInput v){
						vertexOutput o;
						half3 animOffset = half3(_AnimOffsetX, _AnimOffsetY, _AnimOffsetZ) * v.vertex.xyz;
						half3 animPower = half3(_AnimPowerX, _AnimPowerY, _AnimPowerZ);
						half4 newPos = v.vertex;
						newPos.xyz = newPos.xyz + sin(time * _AnimSpeed + (animOffset.x + animOffset.y + animOffset.z) * _AnimFreq) * animPower.xyz;

						float3 normalDirection = normalize( mul( float4( v.normal,0.0 ), _World2Object ).xyz );

						o.pos = mul(UNITY_MATRIX_MVP, newPos);

						float atten = 1.0;
						float lightDirection;

						lightDirection = normalize( _WorldSpaceLightPos0.xyz);

						float3 diffuseReflection = atten * _LightColor0.xyz* _Color.rgb *max( 0.0, dot( normalDirection, lightDirection) );

						o.col = float4(diffuseReflection,1.0);

						

						return o;
					}
					float4 frag(vertexOutput i) : COLOR {
						return i.col;

					}
					
				ENDCG
				}
	}

}