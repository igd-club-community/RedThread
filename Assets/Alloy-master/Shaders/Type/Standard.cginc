///////////////////////////////////////////////////////////////////////////////
/// @file Standard.cginc
/// @brief Standard shader type callbacks.
///////////////////////////////////////////////////////////////////////////////

#ifndef ALLOY_SHADERS_TYPE_STANDARD_CGINC
#define ALLOY_SHADERS_TYPE_STANDARD_CGINC

#include "../Framework/Type.cginc"

void aVertexShader(
    inout AVertex v)
{
    aStandardVertexShader(v);
}

void aColorShader(
    inout half4 color,
    ASurface s)
{
    aStandardColorShader(color, s);
}

void aGbufferShader(
    inout AGbuffer gb,
    ASurface s)
{

}

#endif // ALLOY_SHADERS_TYPE_STANDARD_CGINC
