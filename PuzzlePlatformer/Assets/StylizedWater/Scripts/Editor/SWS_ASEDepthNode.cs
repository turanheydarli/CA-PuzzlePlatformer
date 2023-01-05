using UnityEngine;
using UnityEditor;
using System;

#if AMPLIFY_SHADER_EDITOR
namespace AmplifyShaderEditor
{
    [Serializable]
    [NodeAttributes("Depth (SWS)", "Surface Data", "Outputs a 0 - 1 gradient representing the distance between the surface of this object and geometry behind")]
    public sealed class SWS_ASEDepthNode : ParentNode
    {
        protected override void CommonInit(int uniqueId)
        {
            base.CommonInit(uniqueId);
            m_headerColor = new Color(0.2f, 0.6f, 1.0f, 1f);
            AddOutputPort(WirePortDataType.FLOAT, "Out");
            m_useInternalPortData = true;
            m_autoWrapProperties = true;
        }

        public override string GenerateShaderForOutput(int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalvar)
        {
            if (dataCollector.PortCategory == MasterNodePortCategory.Vertex || dataCollector.PortCategory == MasterNodePortCategory.Tessellation)
            {
                UIUtils.ShowNoVertexModeNodeMessage(this);
                return "0";
            }

            if (m_outputPorts[0].IsLocalValue(dataCollector.PortCategory))
                return GetOutputColorItem(0, outputId, m_outputPorts[0].LocalValue(dataCollector.PortCategory));

            dataCollector.AddToIncludes(UniqueId, Constants.UnityCgLibFuncs);
            dataCollector.AddToUniforms(UniqueId, "UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);");

            dataCollector.AddCodeComments(true, new string[] { "Start - Stylized Water custom depth" });
            string screenPos = GeneratorUtils.GenerateScreenPosition(ref dataCollector, UniqueId, m_currentPrecisionType, true);
            string screenPosNorm = GeneratorUtils.GenerateScreenPositionNormalized(ref dataCollector, UniqueId, m_currentPrecisionType, true);

            string screenDepth = "SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, " + screenPosNorm + ".xy)";

            screenDepth = string.Format("DECODE_EYEDEPTH({0})", screenDepth);

            string distance = "lerp(1.0, (1.0 / _ProjectionParams.z), unity_OrthoParams.w)";

            dataCollector.AddLocalVariable(UniqueId, "float screenDepth" + OutputId + " = " + screenDepth + ";");

            string depthVal = "(screenDepth" + OutputId + " - DECODE_EYEDEPTH(" + screenPosNorm + ".z) ) / (" + distance + ");";

            dataCollector.AddLocalVariable(UniqueId, "float distanceDepth" + OutputId + " = " + depthVal);
            
            m_outputPorts[0].SetLocalValue("distanceDepth" + OutputId, MasterNodePortCategory.Fragment);
            dataCollector.AddCodeComments(true, new string[] { "End - Stylized Water custom depth" });

            return GetOutputColorItem(0, outputId, "distanceDepth" + OutputId);
        }
    }
}
#endif
