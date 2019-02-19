///////////////////////////////////////////////////////////////////////////////
/// @file Standard.cginc
/// @brief Physical BRDF with optional SSS effects. Forward+Deferred.
///////////////////////////////////////////////////////////////////////////////

#ifndef ALLOY_SHADERS_LIGHTING_STANDARD_CGINC
#define ALLOY_SHADERS_LIGHTING_STANDARD_CGINC

#include "../Framework/Lighting.cginc"

void aPreLighting(
    inout ASurface s)
{
    aStandardPreLighting(s);
}

half3 aDirectLighting( 
    ADirect d,
    ASurface s)
{
    return aStandardDirectLighting(d, s);
}

half3 aIndirectLighting(
    AIndirect i,
    ASurface s)
{
    return aStandardIndirectLighting(i, s);
}

#endif // ALLOY_SHADERS_LIGHTING_STANDARD_CGINC
