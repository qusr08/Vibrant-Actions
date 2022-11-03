// Peer Plays Tutorials
// Part 1: https://www.youtube.com/watch?v=Ws4ukvCgTOU
// Part 2: https://www.youtube.com/watch?v=sJFu_sdLBy8
// Part 4: https://www.youtube.com/watch?v=KQGNMCwJaNQ

Shader "Spherical/Grayscale Sphere"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		// _Position ("World Position", Vector) = (0,0,0,0)
		// _Radius ("Sphere Radius", Range(0,100)) = 0
		// _Softness ("Sphere Softness", Range(0,10)) = 0
	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM

			// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
			// #pragma exclude_renderers d3d11 gles

			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			// The maximum amount of trash that can be handled by this shader
			#define MAX_TRASH 100

			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
				float3 worldPos;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			// half _Softness;

			// Spherical Mask
			uniform int GS_Trash_Count;
			uniform float4 GS_Trash_Positions[MAX_TRASH];
			uniform half GS_Trash_Radii[MAX_TRASH];

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o) {
				// Tint the color of the surface
				fixed4 c = tex2D(_MainTex,IN.uv_MainTex) * _Color;

				// Get a grayscale value based on the color of the surface
				float grayscale = (c.r + c.g + c.b) * 0.333;
				fixed4 grayscale_c = fixed4(grayscale,grayscale,grayscale,1);

				// Set the area around each trash position to grayscale if it is within range to a trash object
				fixed4 color = c;
				for (int i = 0; i < GS_Trash_Count; i++) {
					// Check the distance between the trash position and the surface position
					half d = distance(GS_Trash_Positions[i], IN.worldPos);

					// If that distance is less than the trash's radius, the position needs to be grayscale
					if (d <= GS_Trash_Radii[i]) {
						color = grayscale_c;
						// This can break out of the loop because there is no need to check any other trash if the surface color is already grayscale
						break;
					}
				}

				// Set the color of the surface
				o.Albedo = color.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
