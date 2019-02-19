Shader "Hidden/Alloy/Deferred Reflections" {
Properties {
    _SrcBlend ("", Float) = 1
    _DstBlend ("", Float) = 1
}
SubShader {
    // Calculates reflection contribution from a single probe (rendered as cubes) or default reflection (rendered as full screen quad)
    Pass {
        ZWrite Off
        ZTest LEqual
        Blend [_SrcBlend] [_DstBlend]

        CGPROGRAM
        #pragma target 3.0
        #pragma vertex aMainVertexShader
        #pragma fragment aMainFragmentShader

        #include "../../Deferred/ReflectionProbe.cginc"
        #include "../../Lighting/Standard.cginc"

        ENDCG
    }

    // Adds reflection buffer to the lighting buffer
    Pass
    {
	    ZWrite Off
	    ZTest Always
	    Blend [_SrcBlend] [_DstBlend]

	    CGPROGRAM
	    #pragma target 3.0
	    #pragma vertex aMainVertexShader
	    #pragma fragment aMainFragmentShader

	    #pragma multi_compile ___ UNITY_HDR_ON

        #include "../../Deferred/ReflectionAdd.cginc"

	    ENDCG
    }
}
Fallback Off
}

