/////////////////////////////////////////////////////////////////////////////////
/// @file Details0.cginc
/// @brief Unity Terrain details VertexLit surface shader definition.
/////////////////////////////////////////////////////////////////////////////////

#ifndef ALLOY_SHADERS_DEFINITION_DETAILS0_CGINC
#define ALLOY_SHADERS_DEFINITION_DETAILS0_CGINC

#include "../Lighting/Standard.cginc"
#include "../Type/Standard.cginc"

void aSurfaceShader(
    inout ASurface s)
{
    half4 base = aSampleBase(s) * s.vertexColor;

    s.baseColor = base.rgb;
    s.opacity = base.a;
    s.specularity = 0.5h;
    s.roughness = 1.0h;
}

#endif // ALLOY_SHADERS_DEFINITION_DETAILS0_CGINC
