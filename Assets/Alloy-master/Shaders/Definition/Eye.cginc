/////////////////////////////////////////////////////////////////////////////////
/// @file Eye.cginc
/// @brief Eye surface shader definition.
/////////////////////////////////////////////////////////////////////////////////

#ifndef ALLOY_SHADERS_DEFINITION_EYE_CGINC
#define ALLOY_SHADERS_DEFINITION_EYE_CGINC

#define A_EYE_ON
#define A_DETAIL_MASK_OFF
#define A_DETAIL_COLOR_MAP_OFF

#include "../Lighting/Standard.cginc"
#include "../Type/Standard.cginc"

void aSurfaceShader(
    inout ASurface s)
{
    aEye(s);
    aDetail(s);

    s.mask = 1.0h;
    aDissolve(s);
    aDecal(s);
    aEmission(s);
    aRim(s);
}

#endif // ALLOY_SHADERS_DEFINITION_EYE_CGINC
