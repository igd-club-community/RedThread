/////////////////////////////////////////////////////////////////////////////////
/// @file Alpha.cginc
/// @brief Alpha deferred decal surface shader definition.
/////////////////////////////////////////////////////////////////////////////////

#ifndef ALLOY_SHADERS_DEFINITION_DECAL_ALPHA_CGINC
#define ALLOY_SHADERS_DEFINITION_DECAL_ALPHA_CGINC

#define A_MAIN_TEXTURES_ON

#include "../Lighting/Standard.cginc"
#include "../Type/DecalAlpha.cginc"

void aSurfaceShader(
    inout ASurface s)
{
    aParallax(s);
    aDissolve(s);
    aMainTextures(s);
    aDetail(s);
    aTeamColor(s);
    aAo2(s);
    aDecal(s);
    aWetness(s);
    aRim(s);
    aEmission(s);
}

#endif // ALLOY_SHADERS_DEFINITION_DECAL_ALPHA_CGINC
