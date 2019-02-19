/////////////////////////////////////////////////////////////////////////////////
/// @file Details1.cginc
/// @brief Unity Terrain details WavingDoublePass surface shader definition.
/////////////////////////////////////////////////////////////////////////////////

#ifndef ALLOY_SHADERS_DEFINITION_DETAILS1_CGINC
#define ALLOY_SHADERS_DEFINITION_DETAILS1_CGINC

#include "../Lighting/Standard.cginc"
#include "../Type/Details1.cginc"

void aSurfaceShader(
    inout ASurface s)
{
    half4 base = aSampleBase(s) * s.vertexColor;

    s.baseColor = base.rgb;
    s.opacity = base.a;
    aCutout(s);
    s.specularity = 0.5h;
    s.roughness = 1.0h;
    s.opacity *= s.vertexColor.a;
}

#endif // ALLOY_SHADERS_DEFINITION_DETAILS1_CGINC
