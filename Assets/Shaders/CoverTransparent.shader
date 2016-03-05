Shader "Custom/CoverTransparent" {
	Properties {
		_NotVisibleColor("NotVisibleColor(RGB)",Color)=(0.3,0.3,0.3,1)
		_MainTex("Base(RGB)",2D)="White"{}
	}
	SubShader {
		Tags{"Queue"="Geometry+500" "RendeType"="Opaque"}
		LOD 200
		Pass{
			ZTest Greater
            Lighting Off
            ZWrite Off
         //   Color [_NotVisibleColor]
            Blend SrcAlpha OneMinusSrcAlpha
            SetTexture [_MainTex] { ConstantColor [_NotVisibleColor] combine constant * texture }
		}

		Pass {
            ZTest LEqual
            Material {
                Diffuse (1,1,1,1)
                Ambient (1,1,1,1)
            }
            Lighting Off
            SetTexture [_MainTex] { combine texture } 
        }
	} 
	FallBack "Diffuse"
}
