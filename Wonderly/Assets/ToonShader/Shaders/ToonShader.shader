Shader "ToonShader"
{
	Properties
	{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor ("Outline color", Color) = (0,0,0,1)
		_OutlineWidth ("Outlines width", Range (0.0, 2.0)) = 1.1
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;

	};

	struct v2f
	{
		float4 pos : POSITION;
		UNITY_FOG_COORDS(0)
		fixed4 color : COLOR;

	};

	uniform float _OutlineWidth;
	uniform float4 _OutlineColor;
	uniform sampler2D _MainTex;
	uniform float4 _Color;
	
	v2f vert(appdata v)
			{
				appdata original = v;
				v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz);

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
		}
		
		
	ENDCG

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		UsePass "Toon/Basic/BASE"

		Pass //Outline
		{
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			half4 frag(v2f i) : COLOR
			{
				return _OutlineColor;
}
			
			ENDCG
		}
	}
	
	Fallback "Toon/Basic"
}
