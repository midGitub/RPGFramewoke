Shader "Test/Test1"
{
	properties{
		_Color("Main Color",color)=(1,1,1,1)
		_Ambient("Ambient",color)=(0.3,0.3,0.3,0.3)
		_Specular("Specular",color)=(1,1,1,1)
		_Shininess("Shininess",range(0,8))=4
		_Emission("Emission",color)=(1,1,1,1)
	}
	SubShader{
		pass{
			material{
				diffuse[_Color]
				ambient[_Ambient]
				specular[_Specular]
				shininess[_Shininess]
				emission[_Emission]
			}
			lighting on
			separatespecular on
		}		
	}
}
