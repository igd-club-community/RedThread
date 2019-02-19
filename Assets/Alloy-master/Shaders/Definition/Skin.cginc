/////////////////////////////////////////////////////////////////////////////////
/// @file Skin.cginc
/// @brief Skin surface shader definition.
/////////////////////////////////////////////////////////////////////////////////

#ifndef ALLOY_SHADERS_DEFINITION_SKIN_CGINC
#define ALLOY_SHADERS_DEFINITION_SKIN_CGINC

#define A_SKIN_TEXTURES_ON

#include "../Lighting/Standard.cginc"
#include "../Type/Standard.cginc"

void aSurfaceShader(
    inout ASurface s)
{
    aParallax(s);
    aDissolve(s);
    aSkinTextures(s);
    aDetail(s);
    aTeamColor(s);
    aDecal(s);
    aWetness(s);	
    aRim(s);
    aEmission(s);
}

#endif // ALLOY_SHADERS_DEFINITION_SKIN_CGINC
