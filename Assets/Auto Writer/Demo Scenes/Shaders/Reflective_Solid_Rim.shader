// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "TEXTOR/Reflective_Solid_Rim"
{
	Properties 
	{
_Colour("_Colour", Color) = (1,1,1,1)
_Diffuse("_Diffuse", 2D) = "black" {}
_Reflection_brightness("_Reflection_brightness", Range(0,1) ) = 0.5
_Cubemap("_Cubemap", Cube) = "black" {}
_Spec("_Spec", Color) = (1,1,1,1)
_Gloss("_Gloss", Range(0,10) ) = 0.58
_Rim_Power("_Rim_Power", Range(0,5) ) = 0.5
_Rim_Colour("_Rim_Colour", Color) = (1,1,1,1)

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Geometry"
"IgnoreProjector"="False"
"RenderType"="Opaque"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 2.0


float4 _Colour;
sampler2D _Diffuse;
float _Reflection_brightness;
samplerCUBE _Cubemap;
float4 _Spec;
float _Gloss;
float _Rim_Power;
float4 _Rim_Colour;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float2 uv_Diffuse;
float3 viewDir;
float3 simpleWorldRefl;

			};

			void vert (inout appdata_full v, out Input o) {
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);

o.simpleWorldRefl = -reflect( normalize(WorldSpaceViewDir(v.vertex)), normalize(mul((float3x3)unity_ObjectToWorld, SCALED_NORMAL)));

			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Tex2D0=tex2D(_Diffuse,(IN.uv_Diffuse.xyxy).xy);
float4 Multiply1=_Colour * Tex2D0;
float4 Fresnel1_1_NoInput = float4(0,0,1,1);
float4 Fresnel1=(1.0 - dot( normalize( float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z,1.0 ).xyz), normalize( Fresnel1_1_NoInput.xyz ) )).xxxx;
float4 Pow1=pow(Fresnel1,_Rim_Power.xxxx);
float4 Multiply6=_Rim_Colour * Pow1;
float4 TexCUBE0=texCUBE(_Cubemap,float4( IN.simpleWorldRefl.x, IN.simpleWorldRefl.y,IN.simpleWorldRefl.z,1.0 ));
float4 Multiply0=_Reflection_brightness.xxxx * TexCUBE0;
float4 Multiply4=Tex2D0 * Multiply0;
float4 Add0=Multiply6 + Multiply4;
float4 Multiply3=_Gloss.xxxx * Tex2D0.aaaa;
float4 Multiply2=_Spec * Tex2D0.aaaa;
float4 Master0_1_NoInput = float4(0,0,1,1);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Multiply1;
o.Emission = Add0;
o.Specular = Multiply3;
o.Gloss = Multiply2;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}