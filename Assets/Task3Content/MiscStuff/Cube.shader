// clang-format off
Shader "centerout"
{
    Properties
    {

    }

    SubShader
    {
        Tags{ "RenderType"="Transparent" "Queue"="Transparent" }
        ZWrite Off
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

			// clang-format on

			struct vertdata
			{
				float4 position_clip : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct fragdata
			{
				float4 position_clip : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 position_world : TEXCOORD2;
				half3  normal_world : TEXCOORD3;
				float3 view_dir_world : TEXCOORD4;
			};

			fragdata vert( vertdata v )
			{
				fragdata o;
				o.position_clip       = UnityObjectToClipPos( v.position_clip );
				o.uv                  = v.uv;
				float3 position_world = mul( unity_ObjectToWorld, v.position_clip ).xyz;
				o.position_world      = position_world;
				o.normal_world        = UnityObjectToWorldNormal( v.normal );
				o.view_dir_world      = -normalize( UnityWorldSpaceViewDir( position_world ) );
				return o;
			}

			float4 frag( fragdata i )
			: SV_Target
			{
				float4 frag_color = float4( 0.0, 0.0, 0.0, 1.0 );
				{
					frag_color.a = 1.0;

					float2 uv = i.uv;

					float3 gradient = float3( 0.5 + 0.5 * cos( _Time.y + uv.xyx + float3( 0, 2, 4 ) ) );
					frag_color.rgb  = gradient;

					frag_color.rgb *= smoothstep( 0.1, 0.9, distance( float2( 0.5, 0.5 ), uv ) );

					frag_color.rgb *= 2.5;

					frag_color.a = max( max( frag_color.x, frag_color.y ), frag_color.z );
				}

				return frag_color;
			}
			ENDCG
		}
	}
}
