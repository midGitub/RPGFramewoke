Shader "Test/CGTest1"
{
	SubShader
	{
		pass{
			CGPROGRAM
// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it uses non-square matrices
#pragma exclude_renderers gles
			#pragma vertex vert
			#pragma fragment frag
			//定义宏
			#define MACROFL FL4(fl4.ab,fl3.zy);
			
			//别名
			typedef float4 FL4;
			//结构体
			struct v2f{
				//float4 pos;
				//float2 uv;
			};
			
			void vert(in float2 objPos:POSITION,out float4 pos:POSITION,out float4 col:COLOR){
				pos=float4(objPos,0,1);
				//col=float4(0,0,0,0);
				col=pos;
			}
			
			void frag(inout float4 col:COLOR){
				//col=float4(1,0,0,1);
				
				//half为float的一半
				//fixed	2,3,4,最高为4
				//float
				//bool
				//int
				//sampler*
				float r=1;
				float g=0;
				float b=0;
				float a=1;
				col=float4(r,g,b,a);
				//布尔类型(C语言没有)
				bool b1=false;
				col=b1?col:fixed4(0,1,0,1);
				
				float2 fl2=float2(1,0);
				float3 fl3=float3(1,0,1);
				float4 fl4=float4(1,1,0,1);
				FL4 f1=MACROFL	//xyzw	/	rgba
				col=f1;
				
				//矩阵
				float2x4 M2x4={fl4,{1,0,1,1}};
				col= M2x4[0];
				//数组
				float arr[4]={1,0.5,0.5,1};
				col=float4(arr[0],arr[1],arr[2],arr[3]);
				
				v2f o;
				//o.pos=fl4;
				//o.uv=fl2;
			}
		
			ENDCG
		}
	}
}
