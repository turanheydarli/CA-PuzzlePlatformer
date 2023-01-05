/*
 * With multiview rendering. the unity_StereoEyeIndex value does not get carried over from the vertex- to the fragment shader
 * Redefine it to strictly use the eye index provided by the API
*/

#if defined(UNITY_STEREO_MULTIVIEW_ENABLED) && defined(SHADER_STAGE_FRAGMENT)

#define UNITY_DECLARE_MULTIVIEW(number_of_views) GLOBAL_CBUFFER_START(OVR_multiview) uint gl_ViewID; uint numViews_##number_of_views; GLOBAL_CBUFFER_END
UNITY_DECLARE_MULTIVIEW(2);

#undef unity_StereoEyeIndex
#define unity_StereoEyeIndex gl_ViewID

#undef _WorldSpaceCameraPos
#define _WorldSpaceCameraPos unity_StereoWorldSpaceCameraPos[unity_StereoEyeIndex]
#endif