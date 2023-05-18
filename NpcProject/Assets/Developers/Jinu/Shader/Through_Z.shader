Shader "ThroughWall_Z"
{
    Properties
    {
        [NoScaleOffset]_Base_Map("Base Map", 2D) = "white" {}
        _Base_Map_Color("Base Map Color", Color) = (0, 0, 0, 0)
        [NoScaleOffset]_Specular_Map("Specular Map", 2D) = "white" {}
        _Specular_Smoothness("Specular Smoothness", Range(0, 1)) = 0.5
        [NoScaleOffset]_Occlusion_Map("Occlusion Map", 2D) = "white" {}
        _Occlusion_Range("Occlusion Range", Range(0, 1)) = 1
        _Emission_Map("Emission Map", 2D) = "white" {}
        [HDR]_Emission_Color("Emission Color", Color) = (0, 0, 0, 0)
        _Opacity("Opacity", Range(0, 1)) = 1
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
        _Size("Size", Float) = 1
        _Position("Player Position", Vector) = (0.5, 0.5, 0, 0)
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Lit"
            "Queue"="Transparent"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalLitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
        
        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DYNAMICLIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
        #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
        #pragma multi_compile_fragment _ _SHADOWS_SOFT
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile _ SHADOWS_SHADOWMASK
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ _LIGHT_LAYERS
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma multi_compile_fragment _ _LIGHT_COOKIES
        #pragma multi_compile _ _CLUSTERED_RENDERING
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_TEXCOORD2
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #define VARYINGS_NEED_SHADOW_COORD
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_FORWARD
        #define _FOG_FRAGMENT 1
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _ALPHATEST_ON 1
        #define _SPECULAR_SETUP 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
             float3 viewDirectionWS;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh;
            #endif
             float4 fogFactorAndVertexLight;
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             float4 shadowCoord;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float3 WorldSpacePosition;
             float4 ScreenPosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float3 interp1 : INTERP1;
             float4 interp2 : INTERP2;
             float4 interp3 : INTERP3;
             float3 interp4 : INTERP4;
             float2 interp5 : INTERP5;
             float2 interp6 : INTERP6;
             float3 interp7 : INTERP7;
             float4 interp8 : INTERP8;
             float4 interp9 : INTERP9;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            output.interp3.xyzw =  input.texCoord0;
            output.interp4.xyz =  input.viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            output.interp5.xy =  input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.interp6.xy =  input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.interp7.xyz =  input.sh;
            #endif
            output.interp8.xyzw =  input.fogFactorAndVertexLight;
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.interp9.xyzw =  input.shadowCoord;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            output.texCoord0 = input.interp3.xyzw;
            output.viewDirectionWS = input.interp4.xyz;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.interp5.xy;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.interp6.xy;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.interp7.xyz;
            #endif
            output.fogFactorAndVertexLight = input.interp8.xyzw;
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.interp9.xyzw;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Emission_Map_TexelSize;
        float4 _Emission_Map_ST;
        float4 _Base_Map_TexelSize;
        float4 _Base_Map_Color;
        float2 _Position;
        float _Size;
        float _Smoothness;
        float _Opacity;
        float4 _Occlusion_Map_TexelSize;
        float4 _Specular_Map_TexelSize;
        float4 _Emission_Color;
        float _Specular_Smoothness;
        float _Occlusion_Range;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Emission_Map);
        SAMPLER(sampler_Emission_Map);
        TEXTURE2D(_Base_Map);
        SAMPLER(sampler_Base_Map);
        TEXTURE2D(_Occlusion_Map);
        SAMPLER(sampler_Occlusion_Map);
        TEXTURE2D(_Specular_Map);
        SAMPLER(sampler_Specular_Map);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Smoothstep_float4(float4 Edge1, float4 Edge2, float4 In, out float4 Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float3 NormalTS;
            float3 Emission;
            float3 Specular;
            float Smoothness;
            float Occlusion;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_ff5df97f968648deb9218ebd0f57a611_Out_0 = UnityBuildTexture2DStructNoScale(_Base_Map);
            float4 _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0 = SAMPLE_TEXTURE2D(_Property_ff5df97f968648deb9218ebd0f57a611_Out_0.tex, _Property_ff5df97f968648deb9218ebd0f57a611_Out_0.samplerstate, _Property_ff5df97f968648deb9218ebd0f57a611_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_R_4 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.r;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_G_5 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.g;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_B_6 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.b;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_A_7 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.a;
            float4 _Property_39e7dd531d594db7858b368c95645d45_Out_0 = _Base_Map_Color;
            float4 _Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0, _Property_39e7dd531d594db7858b368c95645d45_Out_0, _Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2);
            UnityTexture2D _Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0 = UnityBuildTexture2DStruct(_Emission_Map);
            float4 _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0 = SAMPLE_TEXTURE2D(_Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0.tex, _Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0.samplerstate, _Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_R_4 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.r;
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_G_5 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.g;
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_B_6 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.b;
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_A_7 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.a;
            float4 _Property_81eee4bebb484cbfb13d307e345f7b1f_Out_0 = IsGammaSpace() ? LinearToSRGB(_Emission_Color) : _Emission_Color;
            float4 _Multiply_7888b607ca38464ebd3e7c5bf46b867d_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0, _Property_81eee4bebb484cbfb13d307e345f7b1f_Out_0, _Multiply_7888b607ca38464ebd3e7c5bf46b867d_Out_2);
            float _Property_6bde81484b294684824b4543814fc10a_Out_0 = _Specular_Smoothness;
            UnityTexture2D _Property_b0520e2066b245fab7437aa29909370b_Out_0 = UnityBuildTexture2DStructNoScale(_Specular_Map);
            float4 _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0 = SAMPLE_TEXTURE2D(_Property_b0520e2066b245fab7437aa29909370b_Out_0.tex, _Property_b0520e2066b245fab7437aa29909370b_Out_0.samplerstate, _Property_b0520e2066b245fab7437aa29909370b_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_R_4 = _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0.r;
            float _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_G_5 = _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0.g;
            float _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_B_6 = _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0.b;
            float _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_A_7 = _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0.a;
            float4 _Smoothstep_0d8a1afabb714658a43925ccdf140eae_Out_3;
            Unity_Smoothstep_float4(float4(0, 0, 0, 0), (_Property_6bde81484b294684824b4543814fc10a_Out_0.xxxx), _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0, _Smoothstep_0d8a1afabb714658a43925ccdf140eae_Out_3);
            float _Property_470a4164b9354c248d31ec3f48361069_Out_0 = _Occlusion_Range;
            UnityTexture2D _Property_8c5e84c73fae476696bd9104b6271171_Out_0 = UnityBuildTexture2DStructNoScale(_Occlusion_Map);
            float4 _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_8c5e84c73fae476696bd9104b6271171_Out_0.tex, _Property_8c5e84c73fae476696bd9104b6271171_Out_0.samplerstate, _Property_8c5e84c73fae476696bd9104b6271171_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_R_4 = _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0.r;
            float _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_G_5 = _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0.g;
            float _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_B_6 = _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0.b;
            float _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_A_7 = _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0.a;
            float4 _Multiply_c2ee88b04ae340409cb43d19b31cfd7b_Out_2;
            Unity_Multiply_float4_float4((_Property_470a4164b9354c248d31ec3f48361069_Out_0.xxxx), _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0, _Multiply_c2ee88b04ae340409cb43d19b31cfd7b_Out_2);
            float _Property_6ea10c18c9c34569a855686378327cc8_Out_0 = _Smoothness;
            float4 _ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
            float2 _Property_0928590f372a44f8bcd9097364aff754_Out_0 = _Position;
            float2 _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3;
            Unity_Remap_float2(_Property_0928590f372a44f8bcd9097364aff754_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3);
            float2 _Add_131ff5157647400ba05686cf4cd9af53_Out_2;
            Unity_Add_float2((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3, _Add_131ff5157647400ba05686cf4cd9af53_Out_2);
            float2 _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3;
            Unity_TilingAndOffset_float((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), float2 (1, 1), _Add_131ff5157647400ba05686cf4cd9af53_Out_2, _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3);
            float2 _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2;
            Unity_Multiply_float2_float2(_TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3, float2(2, 2), _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2);
            float2 _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2;
            Unity_Subtract_float2(_Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2, float2(1, 1), _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2);
            float _Divide_24db64b136ad4e2686c03d443955741e_Out_2;
            Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_24db64b136ad4e2686c03d443955741e_Out_2);
            float _Property_0732a32fbc134633b862e2211df9c672_Out_0 = _Size;
            float _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2;
            Unity_Multiply_float_float(_Divide_24db64b136ad4e2686c03d443955741e_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0, _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2);
            float2 _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0 = float2(_Multiply_446885ec7db048b1854271b841c6cfdc_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0);
            float2 _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2;
            Unity_Divide_float2(_Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2, _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0, _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2);
            float _Length_46c089a12cf44859b56a8951f003539c_Out_1;
            Unity_Length_float2(_Divide_1bac0e4fa3ec426f84168140beae7209_Out_2, _Length_46c089a12cf44859b56a8951f003539c_Out_1);
            float _OneMinus_51a034513eb542269394eea27d7be50a_Out_1;
            Unity_OneMinus_float(_Length_46c089a12cf44859b56a8951f003539c_Out_1, _OneMinus_51a034513eb542269394eea27d7be50a_Out_1);
            float _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1;
            Unity_Saturate_float(_OneMinus_51a034513eb542269394eea27d7be50a_Out_1, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1);
            float _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3;
            Unity_Smoothstep_float(0, _Property_6ea10c18c9c34569a855686378327cc8_Out_0, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1, _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3);
            float _Property_8d111374334f4c068688697b8dbc7025_Out_0 = _Opacity;
            float _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2;
            Unity_Multiply_float_float(_Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3, _Property_8d111374334f4c068688697b8dbc7025_Out_0, _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2);
            float _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            Unity_OneMinus_float(_Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2, _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1);
            surface.BaseColor = (_Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2.xyz);
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.Emission = (_Multiply_7888b607ca38464ebd3e7c5bf46b867d_Out_2.xyz);
            surface.Specular = (_Smoothstep_0d8a1afabb714658a43925ccdf140eae_Out_3.xyz);
            surface.Smoothness = 0.5;
            surface.Occlusion = (_Multiply_c2ee88b04ae340409cb43d19b31cfd7b_Out_2).x;
            surface.Alpha = _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            surface.AlphaClipThreshold = 0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "GBuffer"
            Tags
            {
                "LightMode" = "UniversalGBuffer"
            }
        
        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DYNAMICLIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
        #pragma multi_compile_fragment _ _SHADOWS_SOFT
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
        #pragma multi_compile _ SHADOWS_SHADOWMASK
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
        #pragma multi_compile_fragment _ _LIGHT_LAYERS
        #pragma multi_compile_fragment _ _RENDER_PASS_ENABLED
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_TEXCOORD2
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #define VARYINGS_NEED_SHADOW_COORD
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_GBUFFER
        #define _FOG_FRAGMENT 1
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _ALPHATEST_ON 1
        #define _SPECULAR_SETUP 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
             float3 viewDirectionWS;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh;
            #endif
             float4 fogFactorAndVertexLight;
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             float4 shadowCoord;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float3 WorldSpacePosition;
             float4 ScreenPosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float3 interp1 : INTERP1;
             float4 interp2 : INTERP2;
             float4 interp3 : INTERP3;
             float3 interp4 : INTERP4;
             float2 interp5 : INTERP5;
             float2 interp6 : INTERP6;
             float3 interp7 : INTERP7;
             float4 interp8 : INTERP8;
             float4 interp9 : INTERP9;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            output.interp3.xyzw =  input.texCoord0;
            output.interp4.xyz =  input.viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            output.interp5.xy =  input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.interp6.xy =  input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.interp7.xyz =  input.sh;
            #endif
            output.interp8.xyzw =  input.fogFactorAndVertexLight;
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.interp9.xyzw =  input.shadowCoord;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            output.texCoord0 = input.interp3.xyzw;
            output.viewDirectionWS = input.interp4.xyz;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.interp5.xy;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.interp6.xy;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.interp7.xyz;
            #endif
            output.fogFactorAndVertexLight = input.interp8.xyzw;
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.interp9.xyzw;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Emission_Map_TexelSize;
        float4 _Emission_Map_ST;
        float4 _Base_Map_TexelSize;
        float4 _Base_Map_Color;
        float2 _Position;
        float _Size;
        float _Smoothness;
        float _Opacity;
        float4 _Occlusion_Map_TexelSize;
        float4 _Specular_Map_TexelSize;
        float4 _Emission_Color;
        float _Specular_Smoothness;
        float _Occlusion_Range;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Emission_Map);
        SAMPLER(sampler_Emission_Map);
        TEXTURE2D(_Base_Map);
        SAMPLER(sampler_Base_Map);
        TEXTURE2D(_Occlusion_Map);
        SAMPLER(sampler_Occlusion_Map);
        TEXTURE2D(_Specular_Map);
        SAMPLER(sampler_Specular_Map);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Smoothstep_float4(float4 Edge1, float4 Edge2, float4 In, out float4 Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float3 NormalTS;
            float3 Emission;
            float3 Specular;
            float Smoothness;
            float Occlusion;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_ff5df97f968648deb9218ebd0f57a611_Out_0 = UnityBuildTexture2DStructNoScale(_Base_Map);
            float4 _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0 = SAMPLE_TEXTURE2D(_Property_ff5df97f968648deb9218ebd0f57a611_Out_0.tex, _Property_ff5df97f968648deb9218ebd0f57a611_Out_0.samplerstate, _Property_ff5df97f968648deb9218ebd0f57a611_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_R_4 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.r;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_G_5 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.g;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_B_6 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.b;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_A_7 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.a;
            float4 _Property_39e7dd531d594db7858b368c95645d45_Out_0 = _Base_Map_Color;
            float4 _Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0, _Property_39e7dd531d594db7858b368c95645d45_Out_0, _Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2);
            UnityTexture2D _Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0 = UnityBuildTexture2DStruct(_Emission_Map);
            float4 _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0 = SAMPLE_TEXTURE2D(_Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0.tex, _Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0.samplerstate, _Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_R_4 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.r;
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_G_5 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.g;
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_B_6 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.b;
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_A_7 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.a;
            float4 _Property_81eee4bebb484cbfb13d307e345f7b1f_Out_0 = IsGammaSpace() ? LinearToSRGB(_Emission_Color) : _Emission_Color;
            float4 _Multiply_7888b607ca38464ebd3e7c5bf46b867d_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0, _Property_81eee4bebb484cbfb13d307e345f7b1f_Out_0, _Multiply_7888b607ca38464ebd3e7c5bf46b867d_Out_2);
            float _Property_6bde81484b294684824b4543814fc10a_Out_0 = _Specular_Smoothness;
            UnityTexture2D _Property_b0520e2066b245fab7437aa29909370b_Out_0 = UnityBuildTexture2DStructNoScale(_Specular_Map);
            float4 _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0 = SAMPLE_TEXTURE2D(_Property_b0520e2066b245fab7437aa29909370b_Out_0.tex, _Property_b0520e2066b245fab7437aa29909370b_Out_0.samplerstate, _Property_b0520e2066b245fab7437aa29909370b_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_R_4 = _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0.r;
            float _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_G_5 = _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0.g;
            float _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_B_6 = _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0.b;
            float _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_A_7 = _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0.a;
            float4 _Smoothstep_0d8a1afabb714658a43925ccdf140eae_Out_3;
            Unity_Smoothstep_float4(float4(0, 0, 0, 0), (_Property_6bde81484b294684824b4543814fc10a_Out_0.xxxx), _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0, _Smoothstep_0d8a1afabb714658a43925ccdf140eae_Out_3);
            float _Property_470a4164b9354c248d31ec3f48361069_Out_0 = _Occlusion_Range;
            UnityTexture2D _Property_8c5e84c73fae476696bd9104b6271171_Out_0 = UnityBuildTexture2DStructNoScale(_Occlusion_Map);
            float4 _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_8c5e84c73fae476696bd9104b6271171_Out_0.tex, _Property_8c5e84c73fae476696bd9104b6271171_Out_0.samplerstate, _Property_8c5e84c73fae476696bd9104b6271171_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_R_4 = _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0.r;
            float _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_G_5 = _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0.g;
            float _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_B_6 = _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0.b;
            float _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_A_7 = _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0.a;
            float4 _Multiply_c2ee88b04ae340409cb43d19b31cfd7b_Out_2;
            Unity_Multiply_float4_float4((_Property_470a4164b9354c248d31ec3f48361069_Out_0.xxxx), _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0, _Multiply_c2ee88b04ae340409cb43d19b31cfd7b_Out_2);
            float _Property_6ea10c18c9c34569a855686378327cc8_Out_0 = _Smoothness;
            float4 _ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
            float2 _Property_0928590f372a44f8bcd9097364aff754_Out_0 = _Position;
            float2 _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3;
            Unity_Remap_float2(_Property_0928590f372a44f8bcd9097364aff754_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3);
            float2 _Add_131ff5157647400ba05686cf4cd9af53_Out_2;
            Unity_Add_float2((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3, _Add_131ff5157647400ba05686cf4cd9af53_Out_2);
            float2 _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3;
            Unity_TilingAndOffset_float((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), float2 (1, 1), _Add_131ff5157647400ba05686cf4cd9af53_Out_2, _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3);
            float2 _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2;
            Unity_Multiply_float2_float2(_TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3, float2(2, 2), _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2);
            float2 _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2;
            Unity_Subtract_float2(_Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2, float2(1, 1), _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2);
            float _Divide_24db64b136ad4e2686c03d443955741e_Out_2;
            Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_24db64b136ad4e2686c03d443955741e_Out_2);
            float _Property_0732a32fbc134633b862e2211df9c672_Out_0 = _Size;
            float _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2;
            Unity_Multiply_float_float(_Divide_24db64b136ad4e2686c03d443955741e_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0, _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2);
            float2 _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0 = float2(_Multiply_446885ec7db048b1854271b841c6cfdc_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0);
            float2 _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2;
            Unity_Divide_float2(_Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2, _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0, _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2);
            float _Length_46c089a12cf44859b56a8951f003539c_Out_1;
            Unity_Length_float2(_Divide_1bac0e4fa3ec426f84168140beae7209_Out_2, _Length_46c089a12cf44859b56a8951f003539c_Out_1);
            float _OneMinus_51a034513eb542269394eea27d7be50a_Out_1;
            Unity_OneMinus_float(_Length_46c089a12cf44859b56a8951f003539c_Out_1, _OneMinus_51a034513eb542269394eea27d7be50a_Out_1);
            float _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1;
            Unity_Saturate_float(_OneMinus_51a034513eb542269394eea27d7be50a_Out_1, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1);
            float _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3;
            Unity_Smoothstep_float(0, _Property_6ea10c18c9c34569a855686378327cc8_Out_0, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1, _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3);
            float _Property_8d111374334f4c068688697b8dbc7025_Out_0 = _Opacity;
            float _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2;
            Unity_Multiply_float_float(_Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3, _Property_8d111374334f4c068688697b8dbc7025_Out_0, _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2);
            float _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            Unity_OneMinus_float(_Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2, _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1);
            surface.BaseColor = (_Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2.xyz);
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.Emission = (_Multiply_7888b607ca38464ebd3e7c5bf46b867d_Out_2.xyz);
            surface.Specular = (_Smoothstep_0d8a1afabb714658a43925ccdf140eae_Out_3.xyz);
            surface.Smoothness = 0.5;
            surface.Occlusion = (_Multiply_c2ee88b04ae340409cb43d19b31cfd7b_Out_2).x;
            surface.Alpha = _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            surface.AlphaClipThreshold = 0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRGBufferPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
             float4 ScreenPosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float3 interp1 : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Emission_Map_TexelSize;
        float4 _Emission_Map_ST;
        float4 _Base_Map_TexelSize;
        float4 _Base_Map_Color;
        float2 _Position;
        float _Size;
        float _Smoothness;
        float _Opacity;
        float4 _Occlusion_Map_TexelSize;
        float4 _Specular_Map_TexelSize;
        float4 _Emission_Color;
        float _Specular_Smoothness;
        float _Occlusion_Range;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Emission_Map);
        SAMPLER(sampler_Emission_Map);
        TEXTURE2D(_Base_Map);
        SAMPLER(sampler_Base_Map);
        TEXTURE2D(_Occlusion_Map);
        SAMPLER(sampler_Occlusion_Map);
        TEXTURE2D(_Specular_Map);
        SAMPLER(sampler_Specular_Map);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_6ea10c18c9c34569a855686378327cc8_Out_0 = _Smoothness;
            float4 _ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
            float2 _Property_0928590f372a44f8bcd9097364aff754_Out_0 = _Position;
            float2 _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3;
            Unity_Remap_float2(_Property_0928590f372a44f8bcd9097364aff754_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3);
            float2 _Add_131ff5157647400ba05686cf4cd9af53_Out_2;
            Unity_Add_float2((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3, _Add_131ff5157647400ba05686cf4cd9af53_Out_2);
            float2 _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3;
            Unity_TilingAndOffset_float((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), float2 (1, 1), _Add_131ff5157647400ba05686cf4cd9af53_Out_2, _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3);
            float2 _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2;
            Unity_Multiply_float2_float2(_TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3, float2(2, 2), _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2);
            float2 _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2;
            Unity_Subtract_float2(_Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2, float2(1, 1), _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2);
            float _Divide_24db64b136ad4e2686c03d443955741e_Out_2;
            Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_24db64b136ad4e2686c03d443955741e_Out_2);
            float _Property_0732a32fbc134633b862e2211df9c672_Out_0 = _Size;
            float _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2;
            Unity_Multiply_float_float(_Divide_24db64b136ad4e2686c03d443955741e_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0, _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2);
            float2 _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0 = float2(_Multiply_446885ec7db048b1854271b841c6cfdc_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0);
            float2 _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2;
            Unity_Divide_float2(_Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2, _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0, _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2);
            float _Length_46c089a12cf44859b56a8951f003539c_Out_1;
            Unity_Length_float2(_Divide_1bac0e4fa3ec426f84168140beae7209_Out_2, _Length_46c089a12cf44859b56a8951f003539c_Out_1);
            float _OneMinus_51a034513eb542269394eea27d7be50a_Out_1;
            Unity_OneMinus_float(_Length_46c089a12cf44859b56a8951f003539c_Out_1, _OneMinus_51a034513eb542269394eea27d7be50a_Out_1);
            float _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1;
            Unity_Saturate_float(_OneMinus_51a034513eb542269394eea27d7be50a_Out_1, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1);
            float _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3;
            Unity_Smoothstep_float(0, _Property_6ea10c18c9c34569a855686378327cc8_Out_0, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1, _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3);
            float _Property_8d111374334f4c068688697b8dbc7025_Out_0 = _Opacity;
            float _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2;
            Unity_Multiply_float_float(_Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3, _Property_8d111374334f4c068688697b8dbc7025_Out_0, _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2);
            float _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            Unity_OneMinus_float(_Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2, _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1);
            surface.Alpha = _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            surface.AlphaClipThreshold = 0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthNormals"
            Tags
            {
                "LightMode" = "DepthNormals"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHNORMALS
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float3 WorldSpacePosition;
             float4 ScreenPosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float3 interp1 : INTERP1;
             float4 interp2 : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Emission_Map_TexelSize;
        float4 _Emission_Map_ST;
        float4 _Base_Map_TexelSize;
        float4 _Base_Map_Color;
        float2 _Position;
        float _Size;
        float _Smoothness;
        float _Opacity;
        float4 _Occlusion_Map_TexelSize;
        float4 _Specular_Map_TexelSize;
        float4 _Emission_Color;
        float _Specular_Smoothness;
        float _Occlusion_Range;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Emission_Map);
        SAMPLER(sampler_Emission_Map);
        TEXTURE2D(_Base_Map);
        SAMPLER(sampler_Base_Map);
        TEXTURE2D(_Occlusion_Map);
        SAMPLER(sampler_Occlusion_Map);
        TEXTURE2D(_Specular_Map);
        SAMPLER(sampler_Specular_Map);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 NormalTS;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_6ea10c18c9c34569a855686378327cc8_Out_0 = _Smoothness;
            float4 _ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
            float2 _Property_0928590f372a44f8bcd9097364aff754_Out_0 = _Position;
            float2 _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3;
            Unity_Remap_float2(_Property_0928590f372a44f8bcd9097364aff754_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3);
            float2 _Add_131ff5157647400ba05686cf4cd9af53_Out_2;
            Unity_Add_float2((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3, _Add_131ff5157647400ba05686cf4cd9af53_Out_2);
            float2 _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3;
            Unity_TilingAndOffset_float((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), float2 (1, 1), _Add_131ff5157647400ba05686cf4cd9af53_Out_2, _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3);
            float2 _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2;
            Unity_Multiply_float2_float2(_TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3, float2(2, 2), _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2);
            float2 _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2;
            Unity_Subtract_float2(_Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2, float2(1, 1), _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2);
            float _Divide_24db64b136ad4e2686c03d443955741e_Out_2;
            Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_24db64b136ad4e2686c03d443955741e_Out_2);
            float _Property_0732a32fbc134633b862e2211df9c672_Out_0 = _Size;
            float _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2;
            Unity_Multiply_float_float(_Divide_24db64b136ad4e2686c03d443955741e_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0, _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2);
            float2 _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0 = float2(_Multiply_446885ec7db048b1854271b841c6cfdc_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0);
            float2 _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2;
            Unity_Divide_float2(_Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2, _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0, _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2);
            float _Length_46c089a12cf44859b56a8951f003539c_Out_1;
            Unity_Length_float2(_Divide_1bac0e4fa3ec426f84168140beae7209_Out_2, _Length_46c089a12cf44859b56a8951f003539c_Out_1);
            float _OneMinus_51a034513eb542269394eea27d7be50a_Out_1;
            Unity_OneMinus_float(_Length_46c089a12cf44859b56a8951f003539c_Out_1, _OneMinus_51a034513eb542269394eea27d7be50a_Out_1);
            float _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1;
            Unity_Saturate_float(_OneMinus_51a034513eb542269394eea27d7be50a_Out_1, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1);
            float _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3;
            Unity_Smoothstep_float(0, _Property_6ea10c18c9c34569a855686378327cc8_Out_0, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1, _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3);
            float _Property_8d111374334f4c068688697b8dbc7025_Out_0 = _Opacity;
            float _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2;
            Unity_Multiply_float_float(_Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3, _Property_8d111374334f4c068688697b8dbc7025_Out_0, _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2);
            float _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            Unity_OneMinus_float(_Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2, _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1);
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.Alpha = _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            surface.AlphaClipThreshold = 0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "Meta"
            Tags
            {
                "LightMode" = "Meta"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma shader_feature _ EDITOR_VISUALIZATION
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_TEXCOORD2
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD1
        #define VARYINGS_NEED_TEXCOORD2
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_META
        #define _FOG_FRAGMENT 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 texCoord1;
             float4 texCoord2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
             float4 ScreenPosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float4 interp2 : INTERP2;
             float4 interp3 : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyzw =  input.texCoord0;
            output.interp2.xyzw =  input.texCoord1;
            output.interp3.xyzw =  input.texCoord2;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.texCoord1 = input.interp2.xyzw;
            output.texCoord2 = input.interp3.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Emission_Map_TexelSize;
        float4 _Emission_Map_ST;
        float4 _Base_Map_TexelSize;
        float4 _Base_Map_Color;
        float2 _Position;
        float _Size;
        float _Smoothness;
        float _Opacity;
        float4 _Occlusion_Map_TexelSize;
        float4 _Specular_Map_TexelSize;
        float4 _Emission_Color;
        float _Specular_Smoothness;
        float _Occlusion_Range;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Emission_Map);
        SAMPLER(sampler_Emission_Map);
        TEXTURE2D(_Base_Map);
        SAMPLER(sampler_Base_Map);
        TEXTURE2D(_Occlusion_Map);
        SAMPLER(sampler_Occlusion_Map);
        TEXTURE2D(_Specular_Map);
        SAMPLER(sampler_Specular_Map);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_ff5df97f968648deb9218ebd0f57a611_Out_0 = UnityBuildTexture2DStructNoScale(_Base_Map);
            float4 _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0 = SAMPLE_TEXTURE2D(_Property_ff5df97f968648deb9218ebd0f57a611_Out_0.tex, _Property_ff5df97f968648deb9218ebd0f57a611_Out_0.samplerstate, _Property_ff5df97f968648deb9218ebd0f57a611_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_R_4 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.r;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_G_5 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.g;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_B_6 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.b;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_A_7 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.a;
            float4 _Property_39e7dd531d594db7858b368c95645d45_Out_0 = _Base_Map_Color;
            float4 _Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0, _Property_39e7dd531d594db7858b368c95645d45_Out_0, _Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2);
            UnityTexture2D _Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0 = UnityBuildTexture2DStruct(_Emission_Map);
            float4 _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0 = SAMPLE_TEXTURE2D(_Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0.tex, _Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0.samplerstate, _Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_R_4 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.r;
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_G_5 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.g;
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_B_6 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.b;
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_A_7 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.a;
            float4 _Property_81eee4bebb484cbfb13d307e345f7b1f_Out_0 = IsGammaSpace() ? LinearToSRGB(_Emission_Color) : _Emission_Color;
            float4 _Multiply_7888b607ca38464ebd3e7c5bf46b867d_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0, _Property_81eee4bebb484cbfb13d307e345f7b1f_Out_0, _Multiply_7888b607ca38464ebd3e7c5bf46b867d_Out_2);
            float _Property_6ea10c18c9c34569a855686378327cc8_Out_0 = _Smoothness;
            float4 _ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
            float2 _Property_0928590f372a44f8bcd9097364aff754_Out_0 = _Position;
            float2 _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3;
            Unity_Remap_float2(_Property_0928590f372a44f8bcd9097364aff754_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3);
            float2 _Add_131ff5157647400ba05686cf4cd9af53_Out_2;
            Unity_Add_float2((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3, _Add_131ff5157647400ba05686cf4cd9af53_Out_2);
            float2 _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3;
            Unity_TilingAndOffset_float((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), float2 (1, 1), _Add_131ff5157647400ba05686cf4cd9af53_Out_2, _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3);
            float2 _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2;
            Unity_Multiply_float2_float2(_TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3, float2(2, 2), _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2);
            float2 _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2;
            Unity_Subtract_float2(_Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2, float2(1, 1), _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2);
            float _Divide_24db64b136ad4e2686c03d443955741e_Out_2;
            Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_24db64b136ad4e2686c03d443955741e_Out_2);
            float _Property_0732a32fbc134633b862e2211df9c672_Out_0 = _Size;
            float _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2;
            Unity_Multiply_float_float(_Divide_24db64b136ad4e2686c03d443955741e_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0, _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2);
            float2 _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0 = float2(_Multiply_446885ec7db048b1854271b841c6cfdc_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0);
            float2 _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2;
            Unity_Divide_float2(_Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2, _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0, _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2);
            float _Length_46c089a12cf44859b56a8951f003539c_Out_1;
            Unity_Length_float2(_Divide_1bac0e4fa3ec426f84168140beae7209_Out_2, _Length_46c089a12cf44859b56a8951f003539c_Out_1);
            float _OneMinus_51a034513eb542269394eea27d7be50a_Out_1;
            Unity_OneMinus_float(_Length_46c089a12cf44859b56a8951f003539c_Out_1, _OneMinus_51a034513eb542269394eea27d7be50a_Out_1);
            float _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1;
            Unity_Saturate_float(_OneMinus_51a034513eb542269394eea27d7be50a_Out_1, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1);
            float _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3;
            Unity_Smoothstep_float(0, _Property_6ea10c18c9c34569a855686378327cc8_Out_0, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1, _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3);
            float _Property_8d111374334f4c068688697b8dbc7025_Out_0 = _Opacity;
            float _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2;
            Unity_Multiply_float_float(_Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3, _Property_8d111374334f4c068688697b8dbc7025_Out_0, _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2);
            float _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            Unity_OneMinus_float(_Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2, _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1);
            surface.BaseColor = (_Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2.xyz);
            surface.Emission = (_Multiply_7888b607ca38464ebd3e7c5bf46b867d_Out_2.xyz);
            surface.Alpha = _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            surface.AlphaClipThreshold = 0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
             float4 ScreenPosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Emission_Map_TexelSize;
        float4 _Emission_Map_ST;
        float4 _Base_Map_TexelSize;
        float4 _Base_Map_Color;
        float2 _Position;
        float _Size;
        float _Smoothness;
        float _Opacity;
        float4 _Occlusion_Map_TexelSize;
        float4 _Specular_Map_TexelSize;
        float4 _Emission_Color;
        float _Specular_Smoothness;
        float _Occlusion_Range;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Emission_Map);
        SAMPLER(sampler_Emission_Map);
        TEXTURE2D(_Base_Map);
        SAMPLER(sampler_Base_Map);
        TEXTURE2D(_Occlusion_Map);
        SAMPLER(sampler_Occlusion_Map);
        TEXTURE2D(_Specular_Map);
        SAMPLER(sampler_Specular_Map);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_6ea10c18c9c34569a855686378327cc8_Out_0 = _Smoothness;
            float4 _ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
            float2 _Property_0928590f372a44f8bcd9097364aff754_Out_0 = _Position;
            float2 _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3;
            Unity_Remap_float2(_Property_0928590f372a44f8bcd9097364aff754_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3);
            float2 _Add_131ff5157647400ba05686cf4cd9af53_Out_2;
            Unity_Add_float2((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3, _Add_131ff5157647400ba05686cf4cd9af53_Out_2);
            float2 _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3;
            Unity_TilingAndOffset_float((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), float2 (1, 1), _Add_131ff5157647400ba05686cf4cd9af53_Out_2, _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3);
            float2 _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2;
            Unity_Multiply_float2_float2(_TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3, float2(2, 2), _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2);
            float2 _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2;
            Unity_Subtract_float2(_Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2, float2(1, 1), _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2);
            float _Divide_24db64b136ad4e2686c03d443955741e_Out_2;
            Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_24db64b136ad4e2686c03d443955741e_Out_2);
            float _Property_0732a32fbc134633b862e2211df9c672_Out_0 = _Size;
            float _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2;
            Unity_Multiply_float_float(_Divide_24db64b136ad4e2686c03d443955741e_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0, _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2);
            float2 _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0 = float2(_Multiply_446885ec7db048b1854271b841c6cfdc_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0);
            float2 _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2;
            Unity_Divide_float2(_Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2, _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0, _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2);
            float _Length_46c089a12cf44859b56a8951f003539c_Out_1;
            Unity_Length_float2(_Divide_1bac0e4fa3ec426f84168140beae7209_Out_2, _Length_46c089a12cf44859b56a8951f003539c_Out_1);
            float _OneMinus_51a034513eb542269394eea27d7be50a_Out_1;
            Unity_OneMinus_float(_Length_46c089a12cf44859b56a8951f003539c_Out_1, _OneMinus_51a034513eb542269394eea27d7be50a_Out_1);
            float _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1;
            Unity_Saturate_float(_OneMinus_51a034513eb542269394eea27d7be50a_Out_1, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1);
            float _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3;
            Unity_Smoothstep_float(0, _Property_6ea10c18c9c34569a855686378327cc8_Out_0, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1, _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3);
            float _Property_8d111374334f4c068688697b8dbc7025_Out_0 = _Opacity;
            float _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2;
            Unity_Multiply_float_float(_Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3, _Property_8d111374334f4c068688697b8dbc7025_Out_0, _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2);
            float _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            Unity_OneMinus_float(_Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2, _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1);
            surface.Alpha = _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            surface.AlphaClipThreshold = 0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Back
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
             float4 ScreenPosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Emission_Map_TexelSize;
        float4 _Emission_Map_ST;
        float4 _Base_Map_TexelSize;
        float4 _Base_Map_Color;
        float2 _Position;
        float _Size;
        float _Smoothness;
        float _Opacity;
        float4 _Occlusion_Map_TexelSize;
        float4 _Specular_Map_TexelSize;
        float4 _Emission_Color;
        float _Specular_Smoothness;
        float _Occlusion_Range;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Emission_Map);
        SAMPLER(sampler_Emission_Map);
        TEXTURE2D(_Base_Map);
        SAMPLER(sampler_Base_Map);
        TEXTURE2D(_Occlusion_Map);
        SAMPLER(sampler_Occlusion_Map);
        TEXTURE2D(_Specular_Map);
        SAMPLER(sampler_Specular_Map);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_6ea10c18c9c34569a855686378327cc8_Out_0 = _Smoothness;
            float4 _ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
            float2 _Property_0928590f372a44f8bcd9097364aff754_Out_0 = _Position;
            float2 _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3;
            Unity_Remap_float2(_Property_0928590f372a44f8bcd9097364aff754_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3);
            float2 _Add_131ff5157647400ba05686cf4cd9af53_Out_2;
            Unity_Add_float2((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3, _Add_131ff5157647400ba05686cf4cd9af53_Out_2);
            float2 _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3;
            Unity_TilingAndOffset_float((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), float2 (1, 1), _Add_131ff5157647400ba05686cf4cd9af53_Out_2, _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3);
            float2 _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2;
            Unity_Multiply_float2_float2(_TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3, float2(2, 2), _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2);
            float2 _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2;
            Unity_Subtract_float2(_Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2, float2(1, 1), _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2);
            float _Divide_24db64b136ad4e2686c03d443955741e_Out_2;
            Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_24db64b136ad4e2686c03d443955741e_Out_2);
            float _Property_0732a32fbc134633b862e2211df9c672_Out_0 = _Size;
            float _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2;
            Unity_Multiply_float_float(_Divide_24db64b136ad4e2686c03d443955741e_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0, _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2);
            float2 _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0 = float2(_Multiply_446885ec7db048b1854271b841c6cfdc_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0);
            float2 _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2;
            Unity_Divide_float2(_Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2, _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0, _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2);
            float _Length_46c089a12cf44859b56a8951f003539c_Out_1;
            Unity_Length_float2(_Divide_1bac0e4fa3ec426f84168140beae7209_Out_2, _Length_46c089a12cf44859b56a8951f003539c_Out_1);
            float _OneMinus_51a034513eb542269394eea27d7be50a_Out_1;
            Unity_OneMinus_float(_Length_46c089a12cf44859b56a8951f003539c_Out_1, _OneMinus_51a034513eb542269394eea27d7be50a_Out_1);
            float _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1;
            Unity_Saturate_float(_OneMinus_51a034513eb542269394eea27d7be50a_Out_1, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1);
            float _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3;
            Unity_Smoothstep_float(0, _Property_6ea10c18c9c34569a855686378327cc8_Out_0, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1, _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3);
            float _Property_8d111374334f4c068688697b8dbc7025_Out_0 = _Opacity;
            float _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2;
            Unity_Multiply_float_float(_Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3, _Property_8d111374334f4c068688697b8dbc7025_Out_0, _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2);
            float _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            Unity_OneMinus_float(_Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2, _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1);
            surface.Alpha = _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            surface.AlphaClipThreshold = 0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            // Name: <None>
            Tags
            {
                "LightMode" = "Universal2D"
            }
        
        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_2D
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
             float4 ScreenPosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Emission_Map_TexelSize;
        float4 _Emission_Map_ST;
        float4 _Base_Map_TexelSize;
        float4 _Base_Map_Color;
        float2 _Position;
        float _Size;
        float _Smoothness;
        float _Opacity;
        float4 _Occlusion_Map_TexelSize;
        float4 _Specular_Map_TexelSize;
        float4 _Emission_Color;
        float _Specular_Smoothness;
        float _Occlusion_Range;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Emission_Map);
        SAMPLER(sampler_Emission_Map);
        TEXTURE2D(_Base_Map);
        SAMPLER(sampler_Base_Map);
        TEXTURE2D(_Occlusion_Map);
        SAMPLER(sampler_Occlusion_Map);
        TEXTURE2D(_Specular_Map);
        SAMPLER(sampler_Specular_Map);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_ff5df97f968648deb9218ebd0f57a611_Out_0 = UnityBuildTexture2DStructNoScale(_Base_Map);
            float4 _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0 = SAMPLE_TEXTURE2D(_Property_ff5df97f968648deb9218ebd0f57a611_Out_0.tex, _Property_ff5df97f968648deb9218ebd0f57a611_Out_0.samplerstate, _Property_ff5df97f968648deb9218ebd0f57a611_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_R_4 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.r;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_G_5 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.g;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_B_6 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.b;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_A_7 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.a;
            float4 _Property_39e7dd531d594db7858b368c95645d45_Out_0 = _Base_Map_Color;
            float4 _Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0, _Property_39e7dd531d594db7858b368c95645d45_Out_0, _Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2);
            float _Property_6ea10c18c9c34569a855686378327cc8_Out_0 = _Smoothness;
            float4 _ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
            float2 _Property_0928590f372a44f8bcd9097364aff754_Out_0 = _Position;
            float2 _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3;
            Unity_Remap_float2(_Property_0928590f372a44f8bcd9097364aff754_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3);
            float2 _Add_131ff5157647400ba05686cf4cd9af53_Out_2;
            Unity_Add_float2((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3, _Add_131ff5157647400ba05686cf4cd9af53_Out_2);
            float2 _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3;
            Unity_TilingAndOffset_float((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), float2 (1, 1), _Add_131ff5157647400ba05686cf4cd9af53_Out_2, _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3);
            float2 _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2;
            Unity_Multiply_float2_float2(_TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3, float2(2, 2), _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2);
            float2 _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2;
            Unity_Subtract_float2(_Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2, float2(1, 1), _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2);
            float _Divide_24db64b136ad4e2686c03d443955741e_Out_2;
            Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_24db64b136ad4e2686c03d443955741e_Out_2);
            float _Property_0732a32fbc134633b862e2211df9c672_Out_0 = _Size;
            float _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2;
            Unity_Multiply_float_float(_Divide_24db64b136ad4e2686c03d443955741e_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0, _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2);
            float2 _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0 = float2(_Multiply_446885ec7db048b1854271b841c6cfdc_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0);
            float2 _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2;
            Unity_Divide_float2(_Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2, _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0, _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2);
            float _Length_46c089a12cf44859b56a8951f003539c_Out_1;
            Unity_Length_float2(_Divide_1bac0e4fa3ec426f84168140beae7209_Out_2, _Length_46c089a12cf44859b56a8951f003539c_Out_1);
            float _OneMinus_51a034513eb542269394eea27d7be50a_Out_1;
            Unity_OneMinus_float(_Length_46c089a12cf44859b56a8951f003539c_Out_1, _OneMinus_51a034513eb542269394eea27d7be50a_Out_1);
            float _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1;
            Unity_Saturate_float(_OneMinus_51a034513eb542269394eea27d7be50a_Out_1, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1);
            float _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3;
            Unity_Smoothstep_float(0, _Property_6ea10c18c9c34569a855686378327cc8_Out_0, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1, _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3);
            float _Property_8d111374334f4c068688697b8dbc7025_Out_0 = _Opacity;
            float _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2;
            Unity_Multiply_float_float(_Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3, _Property_8d111374334f4c068688697b8dbc7025_Out_0, _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2);
            float _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            Unity_OneMinus_float(_Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2, _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1);
            surface.BaseColor = (_Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2.xyz);
            surface.Alpha = _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            surface.AlphaClipThreshold = 0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Lit"
            "Queue"="Transparent"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalLitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
        
        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DYNAMICLIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
        #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
        #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
        #pragma multi_compile_fragment _ _SHADOWS_SOFT
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile _ SHADOWS_SHADOWMASK
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ _LIGHT_LAYERS
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        #pragma multi_compile_fragment _ _LIGHT_COOKIES
        #pragma multi_compile _ _CLUSTERED_RENDERING
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_TEXCOORD2
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #define VARYINGS_NEED_SHADOW_COORD
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_FORWARD
        #define _FOG_FRAGMENT 1
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define _ALPHATEST_ON 1
        #define _SPECULAR_SETUP 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
             float3 viewDirectionWS;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh;
            #endif
             float4 fogFactorAndVertexLight;
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             float4 shadowCoord;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float3 WorldSpacePosition;
             float4 ScreenPosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float3 interp1 : INTERP1;
             float4 interp2 : INTERP2;
             float4 interp3 : INTERP3;
             float3 interp4 : INTERP4;
             float2 interp5 : INTERP5;
             float2 interp6 : INTERP6;
             float3 interp7 : INTERP7;
             float4 interp8 : INTERP8;
             float4 interp9 : INTERP9;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            output.interp3.xyzw =  input.texCoord0;
            output.interp4.xyz =  input.viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            output.interp5.xy =  input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.interp6.xy =  input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.interp7.xyz =  input.sh;
            #endif
            output.interp8.xyzw =  input.fogFactorAndVertexLight;
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.interp9.xyzw =  input.shadowCoord;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            output.texCoord0 = input.interp3.xyzw;
            output.viewDirectionWS = input.interp4.xyz;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.interp5.xy;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.interp6.xy;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.interp7.xyz;
            #endif
            output.fogFactorAndVertexLight = input.interp8.xyzw;
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
            output.shadowCoord = input.interp9.xyzw;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Emission_Map_TexelSize;
        float4 _Emission_Map_ST;
        float4 _Base_Map_TexelSize;
        float4 _Base_Map_Color;
        float2 _Position;
        float _Size;
        float _Smoothness;
        float _Opacity;
        float4 _Occlusion_Map_TexelSize;
        float4 _Specular_Map_TexelSize;
        float4 _Emission_Color;
        float _Specular_Smoothness;
        float _Occlusion_Range;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Emission_Map);
        SAMPLER(sampler_Emission_Map);
        TEXTURE2D(_Base_Map);
        SAMPLER(sampler_Base_Map);
        TEXTURE2D(_Occlusion_Map);
        SAMPLER(sampler_Occlusion_Map);
        TEXTURE2D(_Specular_Map);
        SAMPLER(sampler_Specular_Map);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Smoothstep_float4(float4 Edge1, float4 Edge2, float4 In, out float4 Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float3 NormalTS;
            float3 Emission;
            float3 Specular;
            float Smoothness;
            float Occlusion;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_ff5df97f968648deb9218ebd0f57a611_Out_0 = UnityBuildTexture2DStructNoScale(_Base_Map);
            float4 _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0 = SAMPLE_TEXTURE2D(_Property_ff5df97f968648deb9218ebd0f57a611_Out_0.tex, _Property_ff5df97f968648deb9218ebd0f57a611_Out_0.samplerstate, _Property_ff5df97f968648deb9218ebd0f57a611_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_R_4 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.r;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_G_5 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.g;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_B_6 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.b;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_A_7 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.a;
            float4 _Property_39e7dd531d594db7858b368c95645d45_Out_0 = _Base_Map_Color;
            float4 _Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0, _Property_39e7dd531d594db7858b368c95645d45_Out_0, _Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2);
            UnityTexture2D _Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0 = UnityBuildTexture2DStruct(_Emission_Map);
            float4 _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0 = SAMPLE_TEXTURE2D(_Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0.tex, _Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0.samplerstate, _Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_R_4 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.r;
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_G_5 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.g;
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_B_6 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.b;
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_A_7 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.a;
            float4 _Property_81eee4bebb484cbfb13d307e345f7b1f_Out_0 = IsGammaSpace() ? LinearToSRGB(_Emission_Color) : _Emission_Color;
            float4 _Multiply_7888b607ca38464ebd3e7c5bf46b867d_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0, _Property_81eee4bebb484cbfb13d307e345f7b1f_Out_0, _Multiply_7888b607ca38464ebd3e7c5bf46b867d_Out_2);
            float _Property_6bde81484b294684824b4543814fc10a_Out_0 = _Specular_Smoothness;
            UnityTexture2D _Property_b0520e2066b245fab7437aa29909370b_Out_0 = UnityBuildTexture2DStructNoScale(_Specular_Map);
            float4 _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0 = SAMPLE_TEXTURE2D(_Property_b0520e2066b245fab7437aa29909370b_Out_0.tex, _Property_b0520e2066b245fab7437aa29909370b_Out_0.samplerstate, _Property_b0520e2066b245fab7437aa29909370b_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_R_4 = _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0.r;
            float _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_G_5 = _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0.g;
            float _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_B_6 = _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0.b;
            float _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_A_7 = _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0.a;
            float4 _Smoothstep_0d8a1afabb714658a43925ccdf140eae_Out_3;
            Unity_Smoothstep_float4(float4(0, 0, 0, 0), (_Property_6bde81484b294684824b4543814fc10a_Out_0.xxxx), _SampleTexture2D_62269a03e99c49589651b0dae973e1f0_RGBA_0, _Smoothstep_0d8a1afabb714658a43925ccdf140eae_Out_3);
            float _Property_470a4164b9354c248d31ec3f48361069_Out_0 = _Occlusion_Range;
            UnityTexture2D _Property_8c5e84c73fae476696bd9104b6271171_Out_0 = UnityBuildTexture2DStructNoScale(_Occlusion_Map);
            float4 _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_8c5e84c73fae476696bd9104b6271171_Out_0.tex, _Property_8c5e84c73fae476696bd9104b6271171_Out_0.samplerstate, _Property_8c5e84c73fae476696bd9104b6271171_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_R_4 = _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0.r;
            float _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_G_5 = _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0.g;
            float _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_B_6 = _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0.b;
            float _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_A_7 = _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0.a;
            float4 _Multiply_c2ee88b04ae340409cb43d19b31cfd7b_Out_2;
            Unity_Multiply_float4_float4((_Property_470a4164b9354c248d31ec3f48361069_Out_0.xxxx), _SampleTexture2D_d5b1b2b15c1d47e38922c302a1e3412f_RGBA_0, _Multiply_c2ee88b04ae340409cb43d19b31cfd7b_Out_2);
            float _Property_6ea10c18c9c34569a855686378327cc8_Out_0 = _Smoothness;
            float4 _ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
            float2 _Property_0928590f372a44f8bcd9097364aff754_Out_0 = _Position;
            float2 _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3;
            Unity_Remap_float2(_Property_0928590f372a44f8bcd9097364aff754_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3);
            float2 _Add_131ff5157647400ba05686cf4cd9af53_Out_2;
            Unity_Add_float2((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3, _Add_131ff5157647400ba05686cf4cd9af53_Out_2);
            float2 _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3;
            Unity_TilingAndOffset_float((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), float2 (1, 1), _Add_131ff5157647400ba05686cf4cd9af53_Out_2, _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3);
            float2 _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2;
            Unity_Multiply_float2_float2(_TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3, float2(2, 2), _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2);
            float2 _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2;
            Unity_Subtract_float2(_Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2, float2(1, 1), _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2);
            float _Divide_24db64b136ad4e2686c03d443955741e_Out_2;
            Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_24db64b136ad4e2686c03d443955741e_Out_2);
            float _Property_0732a32fbc134633b862e2211df9c672_Out_0 = _Size;
            float _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2;
            Unity_Multiply_float_float(_Divide_24db64b136ad4e2686c03d443955741e_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0, _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2);
            float2 _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0 = float2(_Multiply_446885ec7db048b1854271b841c6cfdc_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0);
            float2 _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2;
            Unity_Divide_float2(_Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2, _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0, _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2);
            float _Length_46c089a12cf44859b56a8951f003539c_Out_1;
            Unity_Length_float2(_Divide_1bac0e4fa3ec426f84168140beae7209_Out_2, _Length_46c089a12cf44859b56a8951f003539c_Out_1);
            float _OneMinus_51a034513eb542269394eea27d7be50a_Out_1;
            Unity_OneMinus_float(_Length_46c089a12cf44859b56a8951f003539c_Out_1, _OneMinus_51a034513eb542269394eea27d7be50a_Out_1);
            float _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1;
            Unity_Saturate_float(_OneMinus_51a034513eb542269394eea27d7be50a_Out_1, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1);
            float _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3;
            Unity_Smoothstep_float(0, _Property_6ea10c18c9c34569a855686378327cc8_Out_0, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1, _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3);
            float _Property_8d111374334f4c068688697b8dbc7025_Out_0 = _Opacity;
            float _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2;
            Unity_Multiply_float_float(_Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3, _Property_8d111374334f4c068688697b8dbc7025_Out_0, _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2);
            float _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            Unity_OneMinus_float(_Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2, _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1);
            surface.BaseColor = (_Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2.xyz);
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.Emission = (_Multiply_7888b607ca38464ebd3e7c5bf46b867d_Out_2.xyz);
            surface.Specular = (_Smoothstep_0d8a1afabb714658a43925ccdf140eae_Out_3.xyz);
            surface.Smoothness = 0.5;
            surface.Occlusion = (_Multiply_c2ee88b04ae340409cb43d19b31cfd7b_Out_2).x;
            surface.Alpha = _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            surface.AlphaClipThreshold = 0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
             float4 ScreenPosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float3 interp1 : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Emission_Map_TexelSize;
        float4 _Emission_Map_ST;
        float4 _Base_Map_TexelSize;
        float4 _Base_Map_Color;
        float2 _Position;
        float _Size;
        float _Smoothness;
        float _Opacity;
        float4 _Occlusion_Map_TexelSize;
        float4 _Specular_Map_TexelSize;
        float4 _Emission_Color;
        float _Specular_Smoothness;
        float _Occlusion_Range;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Emission_Map);
        SAMPLER(sampler_Emission_Map);
        TEXTURE2D(_Base_Map);
        SAMPLER(sampler_Base_Map);
        TEXTURE2D(_Occlusion_Map);
        SAMPLER(sampler_Occlusion_Map);
        TEXTURE2D(_Specular_Map);
        SAMPLER(sampler_Specular_Map);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_6ea10c18c9c34569a855686378327cc8_Out_0 = _Smoothness;
            float4 _ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
            float2 _Property_0928590f372a44f8bcd9097364aff754_Out_0 = _Position;
            float2 _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3;
            Unity_Remap_float2(_Property_0928590f372a44f8bcd9097364aff754_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3);
            float2 _Add_131ff5157647400ba05686cf4cd9af53_Out_2;
            Unity_Add_float2((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3, _Add_131ff5157647400ba05686cf4cd9af53_Out_2);
            float2 _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3;
            Unity_TilingAndOffset_float((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), float2 (1, 1), _Add_131ff5157647400ba05686cf4cd9af53_Out_2, _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3);
            float2 _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2;
            Unity_Multiply_float2_float2(_TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3, float2(2, 2), _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2);
            float2 _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2;
            Unity_Subtract_float2(_Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2, float2(1, 1), _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2);
            float _Divide_24db64b136ad4e2686c03d443955741e_Out_2;
            Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_24db64b136ad4e2686c03d443955741e_Out_2);
            float _Property_0732a32fbc134633b862e2211df9c672_Out_0 = _Size;
            float _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2;
            Unity_Multiply_float_float(_Divide_24db64b136ad4e2686c03d443955741e_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0, _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2);
            float2 _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0 = float2(_Multiply_446885ec7db048b1854271b841c6cfdc_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0);
            float2 _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2;
            Unity_Divide_float2(_Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2, _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0, _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2);
            float _Length_46c089a12cf44859b56a8951f003539c_Out_1;
            Unity_Length_float2(_Divide_1bac0e4fa3ec426f84168140beae7209_Out_2, _Length_46c089a12cf44859b56a8951f003539c_Out_1);
            float _OneMinus_51a034513eb542269394eea27d7be50a_Out_1;
            Unity_OneMinus_float(_Length_46c089a12cf44859b56a8951f003539c_Out_1, _OneMinus_51a034513eb542269394eea27d7be50a_Out_1);
            float _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1;
            Unity_Saturate_float(_OneMinus_51a034513eb542269394eea27d7be50a_Out_1, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1);
            float _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3;
            Unity_Smoothstep_float(0, _Property_6ea10c18c9c34569a855686378327cc8_Out_0, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1, _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3);
            float _Property_8d111374334f4c068688697b8dbc7025_Out_0 = _Opacity;
            float _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2;
            Unity_Multiply_float_float(_Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3, _Property_8d111374334f4c068688697b8dbc7025_Out_0, _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2);
            float _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            Unity_OneMinus_float(_Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2, _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1);
            surface.Alpha = _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            surface.AlphaClipThreshold = 0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthNormals"
            Tags
            {
                "LightMode" = "DepthNormals"
            }
        
        // Render State
        Cull Back
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHNORMALS
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float3 WorldSpacePosition;
             float4 ScreenPosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float3 interp1 : INTERP1;
             float4 interp2 : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Emission_Map_TexelSize;
        float4 _Emission_Map_ST;
        float4 _Base_Map_TexelSize;
        float4 _Base_Map_Color;
        float2 _Position;
        float _Size;
        float _Smoothness;
        float _Opacity;
        float4 _Occlusion_Map_TexelSize;
        float4 _Specular_Map_TexelSize;
        float4 _Emission_Color;
        float _Specular_Smoothness;
        float _Occlusion_Range;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Emission_Map);
        SAMPLER(sampler_Emission_Map);
        TEXTURE2D(_Base_Map);
        SAMPLER(sampler_Base_Map);
        TEXTURE2D(_Occlusion_Map);
        SAMPLER(sampler_Occlusion_Map);
        TEXTURE2D(_Specular_Map);
        SAMPLER(sampler_Specular_Map);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 NormalTS;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_6ea10c18c9c34569a855686378327cc8_Out_0 = _Smoothness;
            float4 _ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
            float2 _Property_0928590f372a44f8bcd9097364aff754_Out_0 = _Position;
            float2 _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3;
            Unity_Remap_float2(_Property_0928590f372a44f8bcd9097364aff754_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3);
            float2 _Add_131ff5157647400ba05686cf4cd9af53_Out_2;
            Unity_Add_float2((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3, _Add_131ff5157647400ba05686cf4cd9af53_Out_2);
            float2 _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3;
            Unity_TilingAndOffset_float((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), float2 (1, 1), _Add_131ff5157647400ba05686cf4cd9af53_Out_2, _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3);
            float2 _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2;
            Unity_Multiply_float2_float2(_TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3, float2(2, 2), _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2);
            float2 _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2;
            Unity_Subtract_float2(_Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2, float2(1, 1), _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2);
            float _Divide_24db64b136ad4e2686c03d443955741e_Out_2;
            Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_24db64b136ad4e2686c03d443955741e_Out_2);
            float _Property_0732a32fbc134633b862e2211df9c672_Out_0 = _Size;
            float _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2;
            Unity_Multiply_float_float(_Divide_24db64b136ad4e2686c03d443955741e_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0, _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2);
            float2 _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0 = float2(_Multiply_446885ec7db048b1854271b841c6cfdc_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0);
            float2 _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2;
            Unity_Divide_float2(_Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2, _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0, _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2);
            float _Length_46c089a12cf44859b56a8951f003539c_Out_1;
            Unity_Length_float2(_Divide_1bac0e4fa3ec426f84168140beae7209_Out_2, _Length_46c089a12cf44859b56a8951f003539c_Out_1);
            float _OneMinus_51a034513eb542269394eea27d7be50a_Out_1;
            Unity_OneMinus_float(_Length_46c089a12cf44859b56a8951f003539c_Out_1, _OneMinus_51a034513eb542269394eea27d7be50a_Out_1);
            float _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1;
            Unity_Saturate_float(_OneMinus_51a034513eb542269394eea27d7be50a_Out_1, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1);
            float _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3;
            Unity_Smoothstep_float(0, _Property_6ea10c18c9c34569a855686378327cc8_Out_0, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1, _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3);
            float _Property_8d111374334f4c068688697b8dbc7025_Out_0 = _Opacity;
            float _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2;
            Unity_Multiply_float_float(_Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3, _Property_8d111374334f4c068688697b8dbc7025_Out_0, _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2);
            float _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            Unity_OneMinus_float(_Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2, _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1);
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.Alpha = _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            surface.AlphaClipThreshold = 0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "Meta"
            Tags
            {
                "LightMode" = "Meta"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma shader_feature _ EDITOR_VISUALIZATION
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_TEXCOORD2
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD1
        #define VARYINGS_NEED_TEXCOORD2
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_META
        #define _FOG_FRAGMENT 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 texCoord1;
             float4 texCoord2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
             float4 ScreenPosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float4 interp2 : INTERP2;
             float4 interp3 : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyzw =  input.texCoord0;
            output.interp2.xyzw =  input.texCoord1;
            output.interp3.xyzw =  input.texCoord2;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.texCoord1 = input.interp2.xyzw;
            output.texCoord2 = input.interp3.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Emission_Map_TexelSize;
        float4 _Emission_Map_ST;
        float4 _Base_Map_TexelSize;
        float4 _Base_Map_Color;
        float2 _Position;
        float _Size;
        float _Smoothness;
        float _Opacity;
        float4 _Occlusion_Map_TexelSize;
        float4 _Specular_Map_TexelSize;
        float4 _Emission_Color;
        float _Specular_Smoothness;
        float _Occlusion_Range;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Emission_Map);
        SAMPLER(sampler_Emission_Map);
        TEXTURE2D(_Base_Map);
        SAMPLER(sampler_Base_Map);
        TEXTURE2D(_Occlusion_Map);
        SAMPLER(sampler_Occlusion_Map);
        TEXTURE2D(_Specular_Map);
        SAMPLER(sampler_Specular_Map);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_ff5df97f968648deb9218ebd0f57a611_Out_0 = UnityBuildTexture2DStructNoScale(_Base_Map);
            float4 _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0 = SAMPLE_TEXTURE2D(_Property_ff5df97f968648deb9218ebd0f57a611_Out_0.tex, _Property_ff5df97f968648deb9218ebd0f57a611_Out_0.samplerstate, _Property_ff5df97f968648deb9218ebd0f57a611_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_R_4 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.r;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_G_5 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.g;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_B_6 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.b;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_A_7 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.a;
            float4 _Property_39e7dd531d594db7858b368c95645d45_Out_0 = _Base_Map_Color;
            float4 _Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0, _Property_39e7dd531d594db7858b368c95645d45_Out_0, _Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2);
            UnityTexture2D _Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0 = UnityBuildTexture2DStruct(_Emission_Map);
            float4 _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0 = SAMPLE_TEXTURE2D(_Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0.tex, _Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0.samplerstate, _Property_82d4e13f4fb1435fab4c7f6aaabfe7b1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_R_4 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.r;
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_G_5 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.g;
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_B_6 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.b;
            float _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_A_7 = _SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0.a;
            float4 _Property_81eee4bebb484cbfb13d307e345f7b1f_Out_0 = IsGammaSpace() ? LinearToSRGB(_Emission_Color) : _Emission_Color;
            float4 _Multiply_7888b607ca38464ebd3e7c5bf46b867d_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_0e46613dd2b2466ca21b50ed54779ba7_RGBA_0, _Property_81eee4bebb484cbfb13d307e345f7b1f_Out_0, _Multiply_7888b607ca38464ebd3e7c5bf46b867d_Out_2);
            float _Property_6ea10c18c9c34569a855686378327cc8_Out_0 = _Smoothness;
            float4 _ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
            float2 _Property_0928590f372a44f8bcd9097364aff754_Out_0 = _Position;
            float2 _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3;
            Unity_Remap_float2(_Property_0928590f372a44f8bcd9097364aff754_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3);
            float2 _Add_131ff5157647400ba05686cf4cd9af53_Out_2;
            Unity_Add_float2((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3, _Add_131ff5157647400ba05686cf4cd9af53_Out_2);
            float2 _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3;
            Unity_TilingAndOffset_float((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), float2 (1, 1), _Add_131ff5157647400ba05686cf4cd9af53_Out_2, _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3);
            float2 _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2;
            Unity_Multiply_float2_float2(_TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3, float2(2, 2), _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2);
            float2 _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2;
            Unity_Subtract_float2(_Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2, float2(1, 1), _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2);
            float _Divide_24db64b136ad4e2686c03d443955741e_Out_2;
            Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_24db64b136ad4e2686c03d443955741e_Out_2);
            float _Property_0732a32fbc134633b862e2211df9c672_Out_0 = _Size;
            float _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2;
            Unity_Multiply_float_float(_Divide_24db64b136ad4e2686c03d443955741e_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0, _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2);
            float2 _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0 = float2(_Multiply_446885ec7db048b1854271b841c6cfdc_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0);
            float2 _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2;
            Unity_Divide_float2(_Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2, _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0, _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2);
            float _Length_46c089a12cf44859b56a8951f003539c_Out_1;
            Unity_Length_float2(_Divide_1bac0e4fa3ec426f84168140beae7209_Out_2, _Length_46c089a12cf44859b56a8951f003539c_Out_1);
            float _OneMinus_51a034513eb542269394eea27d7be50a_Out_1;
            Unity_OneMinus_float(_Length_46c089a12cf44859b56a8951f003539c_Out_1, _OneMinus_51a034513eb542269394eea27d7be50a_Out_1);
            float _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1;
            Unity_Saturate_float(_OneMinus_51a034513eb542269394eea27d7be50a_Out_1, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1);
            float _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3;
            Unity_Smoothstep_float(0, _Property_6ea10c18c9c34569a855686378327cc8_Out_0, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1, _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3);
            float _Property_8d111374334f4c068688697b8dbc7025_Out_0 = _Opacity;
            float _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2;
            Unity_Multiply_float_float(_Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3, _Property_8d111374334f4c068688697b8dbc7025_Out_0, _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2);
            float _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            Unity_OneMinus_float(_Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2, _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1);
            surface.BaseColor = (_Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2.xyz);
            surface.Emission = (_Multiply_7888b607ca38464ebd3e7c5bf46b867d_Out_2.xyz);
            surface.Alpha = _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            surface.AlphaClipThreshold = 0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
             float4 ScreenPosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Emission_Map_TexelSize;
        float4 _Emission_Map_ST;
        float4 _Base_Map_TexelSize;
        float4 _Base_Map_Color;
        float2 _Position;
        float _Size;
        float _Smoothness;
        float _Opacity;
        float4 _Occlusion_Map_TexelSize;
        float4 _Specular_Map_TexelSize;
        float4 _Emission_Color;
        float _Specular_Smoothness;
        float _Occlusion_Range;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Emission_Map);
        SAMPLER(sampler_Emission_Map);
        TEXTURE2D(_Base_Map);
        SAMPLER(sampler_Base_Map);
        TEXTURE2D(_Occlusion_Map);
        SAMPLER(sampler_Occlusion_Map);
        TEXTURE2D(_Specular_Map);
        SAMPLER(sampler_Specular_Map);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_6ea10c18c9c34569a855686378327cc8_Out_0 = _Smoothness;
            float4 _ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
            float2 _Property_0928590f372a44f8bcd9097364aff754_Out_0 = _Position;
            float2 _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3;
            Unity_Remap_float2(_Property_0928590f372a44f8bcd9097364aff754_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3);
            float2 _Add_131ff5157647400ba05686cf4cd9af53_Out_2;
            Unity_Add_float2((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3, _Add_131ff5157647400ba05686cf4cd9af53_Out_2);
            float2 _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3;
            Unity_TilingAndOffset_float((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), float2 (1, 1), _Add_131ff5157647400ba05686cf4cd9af53_Out_2, _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3);
            float2 _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2;
            Unity_Multiply_float2_float2(_TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3, float2(2, 2), _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2);
            float2 _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2;
            Unity_Subtract_float2(_Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2, float2(1, 1), _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2);
            float _Divide_24db64b136ad4e2686c03d443955741e_Out_2;
            Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_24db64b136ad4e2686c03d443955741e_Out_2);
            float _Property_0732a32fbc134633b862e2211df9c672_Out_0 = _Size;
            float _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2;
            Unity_Multiply_float_float(_Divide_24db64b136ad4e2686c03d443955741e_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0, _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2);
            float2 _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0 = float2(_Multiply_446885ec7db048b1854271b841c6cfdc_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0);
            float2 _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2;
            Unity_Divide_float2(_Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2, _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0, _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2);
            float _Length_46c089a12cf44859b56a8951f003539c_Out_1;
            Unity_Length_float2(_Divide_1bac0e4fa3ec426f84168140beae7209_Out_2, _Length_46c089a12cf44859b56a8951f003539c_Out_1);
            float _OneMinus_51a034513eb542269394eea27d7be50a_Out_1;
            Unity_OneMinus_float(_Length_46c089a12cf44859b56a8951f003539c_Out_1, _OneMinus_51a034513eb542269394eea27d7be50a_Out_1);
            float _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1;
            Unity_Saturate_float(_OneMinus_51a034513eb542269394eea27d7be50a_Out_1, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1);
            float _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3;
            Unity_Smoothstep_float(0, _Property_6ea10c18c9c34569a855686378327cc8_Out_0, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1, _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3);
            float _Property_8d111374334f4c068688697b8dbc7025_Out_0 = _Opacity;
            float _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2;
            Unity_Multiply_float_float(_Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3, _Property_8d111374334f4c068688697b8dbc7025_Out_0, _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2);
            float _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            Unity_OneMinus_float(_Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2, _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1);
            surface.Alpha = _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            surface.AlphaClipThreshold = 0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Back
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
             float4 ScreenPosition;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Emission_Map_TexelSize;
        float4 _Emission_Map_ST;
        float4 _Base_Map_TexelSize;
        float4 _Base_Map_Color;
        float2 _Position;
        float _Size;
        float _Smoothness;
        float _Opacity;
        float4 _Occlusion_Map_TexelSize;
        float4 _Specular_Map_TexelSize;
        float4 _Emission_Color;
        float _Specular_Smoothness;
        float _Occlusion_Range;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Emission_Map);
        SAMPLER(sampler_Emission_Map);
        TEXTURE2D(_Base_Map);
        SAMPLER(sampler_Base_Map);
        TEXTURE2D(_Occlusion_Map);
        SAMPLER(sampler_Occlusion_Map);
        TEXTURE2D(_Specular_Map);
        SAMPLER(sampler_Specular_Map);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Property_6ea10c18c9c34569a855686378327cc8_Out_0 = _Smoothness;
            float4 _ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
            float2 _Property_0928590f372a44f8bcd9097364aff754_Out_0 = _Position;
            float2 _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3;
            Unity_Remap_float2(_Property_0928590f372a44f8bcd9097364aff754_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3);
            float2 _Add_131ff5157647400ba05686cf4cd9af53_Out_2;
            Unity_Add_float2((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3, _Add_131ff5157647400ba05686cf4cd9af53_Out_2);
            float2 _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3;
            Unity_TilingAndOffset_float((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), float2 (1, 1), _Add_131ff5157647400ba05686cf4cd9af53_Out_2, _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3);
            float2 _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2;
            Unity_Multiply_float2_float2(_TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3, float2(2, 2), _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2);
            float2 _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2;
            Unity_Subtract_float2(_Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2, float2(1, 1), _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2);
            float _Divide_24db64b136ad4e2686c03d443955741e_Out_2;
            Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_24db64b136ad4e2686c03d443955741e_Out_2);
            float _Property_0732a32fbc134633b862e2211df9c672_Out_0 = _Size;
            float _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2;
            Unity_Multiply_float_float(_Divide_24db64b136ad4e2686c03d443955741e_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0, _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2);
            float2 _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0 = float2(_Multiply_446885ec7db048b1854271b841c6cfdc_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0);
            float2 _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2;
            Unity_Divide_float2(_Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2, _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0, _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2);
            float _Length_46c089a12cf44859b56a8951f003539c_Out_1;
            Unity_Length_float2(_Divide_1bac0e4fa3ec426f84168140beae7209_Out_2, _Length_46c089a12cf44859b56a8951f003539c_Out_1);
            float _OneMinus_51a034513eb542269394eea27d7be50a_Out_1;
            Unity_OneMinus_float(_Length_46c089a12cf44859b56a8951f003539c_Out_1, _OneMinus_51a034513eb542269394eea27d7be50a_Out_1);
            float _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1;
            Unity_Saturate_float(_OneMinus_51a034513eb542269394eea27d7be50a_Out_1, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1);
            float _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3;
            Unity_Smoothstep_float(0, _Property_6ea10c18c9c34569a855686378327cc8_Out_0, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1, _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3);
            float _Property_8d111374334f4c068688697b8dbc7025_Out_0 = _Opacity;
            float _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2;
            Unity_Multiply_float_float(_Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3, _Property_8d111374334f4c068688697b8dbc7025_Out_0, _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2);
            float _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            Unity_OneMinus_float(_Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2, _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1);
            surface.Alpha = _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            surface.AlphaClipThreshold = 0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            // Name: <None>
            Tags
            {
                "LightMode" = "Universal2D"
            }
        
        // Render State
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_2D
        #define _ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
             float4 ScreenPosition;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _Emission_Map_TexelSize;
        float4 _Emission_Map_ST;
        float4 _Base_Map_TexelSize;
        float4 _Base_Map_Color;
        float2 _Position;
        float _Size;
        float _Smoothness;
        float _Opacity;
        float4 _Occlusion_Map_TexelSize;
        float4 _Specular_Map_TexelSize;
        float4 _Emission_Color;
        float _Specular_Smoothness;
        float _Occlusion_Range;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_Emission_Map);
        SAMPLER(sampler_Emission_Map);
        TEXTURE2D(_Base_Map);
        SAMPLER(sampler_Base_Map);
        TEXTURE2D(_Occlusion_Map);
        SAMPLER(sampler_Occlusion_Map);
        TEXTURE2D(_Specular_Map);
        SAMPLER(sampler_Specular_Map);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_ff5df97f968648deb9218ebd0f57a611_Out_0 = UnityBuildTexture2DStructNoScale(_Base_Map);
            float4 _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0 = SAMPLE_TEXTURE2D(_Property_ff5df97f968648deb9218ebd0f57a611_Out_0.tex, _Property_ff5df97f968648deb9218ebd0f57a611_Out_0.samplerstate, _Property_ff5df97f968648deb9218ebd0f57a611_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_R_4 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.r;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_G_5 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.g;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_B_6 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.b;
            float _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_A_7 = _SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0.a;
            float4 _Property_39e7dd531d594db7858b368c95645d45_Out_0 = _Base_Map_Color;
            float4 _Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_3fc59ece01ad4a66a8de58bddeccaff0_RGBA_0, _Property_39e7dd531d594db7858b368c95645d45_Out_0, _Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2);
            float _Property_6ea10c18c9c34569a855686378327cc8_Out_0 = _Smoothness;
            float4 _ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0);
            float2 _Property_0928590f372a44f8bcd9097364aff754_Out_0 = _Position;
            float2 _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3;
            Unity_Remap_float2(_Property_0928590f372a44f8bcd9097364aff754_Out_0, float2 (0, 1), float2 (0.5, -1.5), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3);
            float2 _Add_131ff5157647400ba05686cf4cd9af53_Out_2;
            Unity_Add_float2((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), _Remap_c0c2d4ed4ec2486cb408c0ed9cb2895f_Out_3, _Add_131ff5157647400ba05686cf4cd9af53_Out_2);
            float2 _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3;
            Unity_TilingAndOffset_float((_ScreenPosition_d7fda37e251f4f8b994345c79072fd6b_Out_0.xy), float2 (1, 1), _Add_131ff5157647400ba05686cf4cd9af53_Out_2, _TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3);
            float2 _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2;
            Unity_Multiply_float2_float2(_TilingAndOffset_95d1f76e688645cca7a899db5efb61fe_Out_3, float2(2, 2), _Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2);
            float2 _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2;
            Unity_Subtract_float2(_Multiply_3587f3047e3f45239a3738c7efb9701d_Out_2, float2(1, 1), _Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2);
            float _Divide_24db64b136ad4e2686c03d443955741e_Out_2;
            Unity_Divide_float(unity_OrthoParams.y, unity_OrthoParams.x, _Divide_24db64b136ad4e2686c03d443955741e_Out_2);
            float _Property_0732a32fbc134633b862e2211df9c672_Out_0 = _Size;
            float _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2;
            Unity_Multiply_float_float(_Divide_24db64b136ad4e2686c03d443955741e_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0, _Multiply_446885ec7db048b1854271b841c6cfdc_Out_2);
            float2 _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0 = float2(_Multiply_446885ec7db048b1854271b841c6cfdc_Out_2, _Property_0732a32fbc134633b862e2211df9c672_Out_0);
            float2 _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2;
            Unity_Divide_float2(_Subtract_afe0bf9d6e034cf7bef30c25617bfa44_Out_2, _Vector2_af792dd9eca14412b789eec8fb578d07_Out_0, _Divide_1bac0e4fa3ec426f84168140beae7209_Out_2);
            float _Length_46c089a12cf44859b56a8951f003539c_Out_1;
            Unity_Length_float2(_Divide_1bac0e4fa3ec426f84168140beae7209_Out_2, _Length_46c089a12cf44859b56a8951f003539c_Out_1);
            float _OneMinus_51a034513eb542269394eea27d7be50a_Out_1;
            Unity_OneMinus_float(_Length_46c089a12cf44859b56a8951f003539c_Out_1, _OneMinus_51a034513eb542269394eea27d7be50a_Out_1);
            float _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1;
            Unity_Saturate_float(_OneMinus_51a034513eb542269394eea27d7be50a_Out_1, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1);
            float _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3;
            Unity_Smoothstep_float(0, _Property_6ea10c18c9c34569a855686378327cc8_Out_0, _Saturate_a90b245d39a446b6830b10a0b1c6c55c_Out_1, _Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3);
            float _Property_8d111374334f4c068688697b8dbc7025_Out_0 = _Opacity;
            float _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2;
            Unity_Multiply_float_float(_Smoothstep_8c4ac5743a4d49e19563afed0a8dfec7_Out_3, _Property_8d111374334f4c068688697b8dbc7025_Out_0, _Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2);
            float _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            Unity_OneMinus_float(_Multiply_5f1f3718cf1e40f999c5e4184c33cede_Out_2, _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1);
            surface.BaseColor = (_Multiply_cc2fb7e046b44a60a722e6da656048d7_Out_2.xyz);
            surface.Alpha = _OneMinus_892bbe0259d7408b9166f49c11996ba7_Out_1;
            surface.AlphaClipThreshold = 0;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
            output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditorForRenderPipeline "UnityEditor.ShaderGraphLitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}