/////////////////////////////////////////////////////////////////////////////////
/// @file TriPlanar.cginc
/// @brief TriPlanar shader definition.
/////////////////////////////////////////////////////////////////////////////////

#ifndef ALLOY_LEGACY_SHADERS_DEFINITION_TRIPLANAR_CGINC
#define ALLOY_LEGACY_SHADERS_DEFINITION_TRIPLANAR_CGINC

#define A_ROUGHNESS_SOURCE_BASE_COLOR_ALPHA
#define A_TRIPLANAR_ON
#define A_RIM_EFFECTS_MAP_OFF

#include "../../../Shaders/Lighting/Standard.cginc"
#include "../../../Shaders/Type/Standard.cginc"

void aSurfaceShader(
    inout ASurface s)
{
    aTriPlanar(s);
    aRim(s);
}

#endif // ALLOY_LEGACY_SHADERS_DEFINITION_TRIPLANAR_CGINC
