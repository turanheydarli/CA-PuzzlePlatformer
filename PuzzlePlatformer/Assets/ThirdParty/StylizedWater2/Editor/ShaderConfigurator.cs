//Stylized Water 2
//Staggart Creations (http://staggart.xyz)
//Copyright protected under Unity Asset Store EULA

#if SWS_DEV
//#define ENABLE_SHADER_STRIPPING_LOG
//Deep debugging only, makes the stripping process A LOT slower
//#define ENABLE_DEEP_STRIPPING_LOG
#endif

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
#if URP
using UnityEngine.Rendering.Universal;
#endif

namespace StylizedWater2
{
    public static class ShaderConfigurator
    {
        public const string ShaderName = "Universal Render Pipeline/FX/Stylized Water 2";
        public const string ShaderNameTessellation = "Universal Render Pipeline/FX/Stylized Water 2 (Tessellation)";
        
        private const string ShaderGUID = "f04c9486b297dd848909db26983c9ddb";
        private static string ShaderFilePath;
        private const string TessellationShaderGUID = "97d819883abaa514984aaeb4e870697d";
        private static string TessellationFilePath;

        private const string FogLibraryGUID = "8427feba489ff354ab7b22c82a15ba03";
        private static string FogLibraryFilePath;

        private const string ENVIRO_LIBRARY_GUID = "221026906cc779e4b9ab486bbbd30032";
        private const string AZURE_LIBRARY_GUID = "5da6209b193d6eb49b25c88c861e52fa";
        private const string AHF_LIBRARY_GUID = "8db8edf9bba0e9d48998019ca6c2f9ff";
        private const string SCPE_LIBRARY_GUID = "a66e4b0e5c776404e9a091531ccac3f2";
        private const string COZY_LIBRARY_GUID = "e9bc566e199a97947a3e600f2563fe85";

        private const string ConfigurationPrefix = "/* Configuration: ";
        
        public enum FogConfiguration
        {
            [InspectorName("Default Unity")]
            UnityFog,
            [InspectorName("Enviro - Sky & Weather")]
            Enviro,
            [InspectorName("Azure[Sky] Dynamic Skybox")]
            Azure,
            AtmosphericHeightFog,
            SCPostEffects,
            [InspectorName("COZY: Weather")]
            COZY
        }

        public static FogConfiguration CurrentFogConfiguration
        {
            get { return (FogConfiguration)SessionState.GetInt("SWS2_FOG_INTEGRATION", (int)FogConfiguration.UnityFog); }
            set { SessionState.SetInt("SWS2_FOG_INTEGRATION", (int)value); }
        }

        public static void GetCurrentFogConfiguration()
        {
            FogConfiguration config;
            FogConfiguration.TryParse(GetConfiguration(FogLibraryFilePath), out config);
            
            CurrentFogConfiguration = config;
        }
        
        public static void RefreshShaderFilePaths()
        {
            ShaderFilePath = AssetDatabase.GUIDToAssetPath(ShaderGUID);
            TessellationFilePath = AssetDatabase.GUIDToAssetPath(TessellationShaderGUID);
            FogLibraryFilePath = AssetDatabase.GUIDToAssetPath(FogLibraryGUID);
        }

        public static void SetFogConfiguration(FogConfiguration config)
        {
            RefreshShaderFilePaths();
            
            EditorUtility.DisplayProgressBar(AssetInfo.ASSET_NAME, "Modifying shader...", 1f);
            {
                ToggleCodeBlock(FogLibraryFilePath, FogConfiguration.UnityFog.ToString(), config == FogConfiguration.UnityFog);
                ToggleCodeBlock(FogLibraryFilePath, FogConfiguration.Enviro.ToString(), config == FogConfiguration.Enviro);
                ToggleCodeBlock(FogLibraryFilePath, FogConfiguration.Azure.ToString(), config == FogConfiguration.Azure);
                ToggleCodeBlock(FogLibraryFilePath, FogConfiguration.AtmosphericHeightFog.ToString(), config == FogConfiguration.AtmosphericHeightFog);
                ToggleCodeBlock(FogLibraryFilePath, FogConfiguration.SCPostEffects.ToString(), config == FogConfiguration.SCPostEffects);
                ToggleCodeBlock(FogLibraryFilePath, FogConfiguration.COZY.ToString(), config == FogConfiguration.COZY);

                //multi_compile keywords
                ToggleCodeBlock(ShaderFilePath, FogConfiguration.UnityFog.ToString(), config == FogConfiguration.UnityFog);
                ToggleCodeBlock(ShaderFilePath, FogConfiguration.AtmosphericHeightFog.ToString(), config == FogConfiguration.AtmosphericHeightFog);
                ToggleCodeBlock(TessellationFilePath, FogConfiguration.UnityFog.ToString(), config == FogConfiguration.UnityFog);
                ToggleCodeBlock(TessellationFilePath, FogConfiguration.AtmosphericHeightFog.ToString(), config == FogConfiguration.AtmosphericHeightFog);
                
                //Stencil
                ToggleCodeBlock(ShaderFilePath, FogConfiguration.COZY.ToString(), config == FogConfiguration.COZY);
                ToggleCodeBlock(TessellationFilePath, FogConfiguration.COZY.ToString(), config == FogConfiguration.COZY);
                
                SetFogIncludePath(config);
            }
            
            EditorUtility.ClearProgressBar();
            
            CurrentFogConfiguration = config;
            Debug.Log("Water shader files successfully modified to use <b>" + CurrentFogConfiguration + "</b> fog rendering");

            /*
            if (config == FogConfiguration.COZY)
            {
                EditorUtility.DisplayDialog("Further set up for " + config.ToString(), "To ensure both the fog and water render correctly, set the \"Render Queue\" on your water material to 2999 (default is 3000).\n\n" +
                                                                                       "You can find this option under the Advanced tab, on the water material.", "Ok");
            }
            */
        }
        
        //Also accessed in TessellationShaderGenerator (dev)
        public struct CodeBlock
        {
            public int startLine;
            public int endLine;
        }

        private static void ToggleCodeBlock(string filePath, string id, bool enable)
        {
            string[] lines = File.ReadAllLines(filePath);

            List<CodeBlock> codeBlocks = new List<CodeBlock>();

            //Find start and end line indices
            for (int i = 0; i < lines.Length; i++)
            {
                bool blockEndReached = false;

                if (lines[i].Contains(ConfigurationPrefix) && enable)
                {
                    lines[i] = lines[i].Replace(lines[i], ConfigurationPrefix + id + " */");
                }

                if (lines[i].Contains("start " + id))
                {
                    CodeBlock codeBlock = new CodeBlock();

                    codeBlock.startLine = i;

                    //Find related end point
                    for (int l = codeBlock.startLine; l < lines.Length; l++)
                    {
                        if (blockEndReached == false)
                        {
                            if (lines[l].Contains("end " + id))
                            {
                                codeBlock.endLine = l;

                                blockEndReached = true;
                            }
                        }
                    }

                    codeBlocks.Add(codeBlock);
                    blockEndReached = false;
                }

            }

            if (codeBlocks.Count == 0)
            {
                //Debug.Log("No code blocks with the marker \"" + id + "\" were found in file");

                return;
            }

            foreach (CodeBlock codeBlock in codeBlocks)
            {
                if (codeBlock.startLine == codeBlock.endLine) continue;

                //Debug.Log((enable ? "Enabled" : "Disabled") + " \"" + id + "\" code block. Lines " + (codeBlock.startLine + 1) + " through " + (codeBlock.endLine + 1));

                for (int i = codeBlock.startLine + 1; i < codeBlock.endLine; i++)
                {
                    //Uncomment lines
                    if (enable == true)
                    {
                        if (lines[i].StartsWith("//") == true) lines[i] = lines[i].Remove(0, 2);
                    }
                    //Comment out lines
                    else
                    {
                        if (lines[i].StartsWith("//") == false) lines[i] = "//" + lines[i];
                    }
                }
            }

            File.WriteAllLines(filePath, lines);

            AssetDatabase.ImportAsset(filePath);
        }

        private static void SetFogIncludePath(FogConfiguration config)
        {
            string GUID = string.Empty;
            
            switch (config)
            {
                case FogConfiguration.Enviro: GUID = ENVIRO_LIBRARY_GUID;
                    break;
                case FogConfiguration.Azure: GUID = AZURE_LIBRARY_GUID;
                    break;
                case FogConfiguration.AtmosphericHeightFog: GUID = AHF_LIBRARY_GUID;
                    break;
                case FogConfiguration.SCPostEffects: GUID = SCPE_LIBRARY_GUID;
                    break;
                case FogConfiguration.COZY: GUID = COZY_LIBRARY_GUID;
                    break;
            }

            //Would be the case for default Unity fog
            if (GUID == string.Empty) return;
            
            string libraryFilePath = AssetDatabase.GUIDToAssetPath(GUID);

            if (libraryFilePath == string.Empty)
            {
                if (EditorUtility.DisplayDialog(AssetInfo.ASSET_NAME,
                    config + " shader library could not be found with GUID " + GUID + ".\n\n" +
                    "This means it was changed by the author, you deleted the meta file at some point, or the asset simply isn't installed.", "Revert back", "Continue"))
                {
                    SetFogConfiguration(FogConfiguration.UnityFog);
                }
            }
            else
            {
                SetIncludePath(FogLibraryFilePath, config.ToString(), libraryFilePath);
            }
        }
        
        private static void SetIncludePath(string filePath, string id, string libraryPath)
        {
            string[] lines = File.ReadAllLines(filePath);

            //Find start and end line indices
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("include " + id))
                {
                    lines[i + 1] = String.Format("#include \"{0}\"", libraryPath);
                    
                    File.WriteAllLines(filePath, lines);
                    AssetDatabase.ImportAsset(filePath);

                    return;
                }
            }
        }

        private static string GetConfiguration(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            string configStr = lines[0].Replace(ConfigurationPrefix, string.Empty);
            configStr = configStr.Replace(" */", string.Empty);

            return configStr;
        }
    }
    
    //Strips keywords from the shader for extensions not installed. Would otherwise trow errors during the build process, blocking it.
    //Also strips features belonging to newer URP versions.
    partial class KeywordStripper : IPreprocessShaders
    {
        #if ENABLE_DEEP_STRIPPING_LOG
		private const string LOG_FILEPATH = "Library/Stylized Water 2 Compilation.log";
		#endif

		#if ENABLE_DEEP_STRIPPING_LOG
        private System.Diagnostics.Stopwatch m_stripTimer;
        #endif

        private static List<ShaderKeyword> StrippedKeywords = new List<ShaderKeyword>();
        private static List<Shader> TargetShaders = new List<Shader>();
		
        //Note: Constructor is called once, when building starts
        public KeywordStripper()
        {
            #if ENABLE_SHADER_STRIPPING_LOG
            Debug.Log("KeywordStripper initialized...");
            #endif

            StrippedKeywords.Clear();
            TargetShaders.Clear();
            
            TargetShaders.Add(Shader.Find(ShaderConfigurator.ShaderName));
            TargetShaders.Add(Shader.Find(ShaderConfigurator.ShaderNameTessellation));
            
            //Extension states
            bool isInstalledUnderwater = StylizedWaterEditor.UnderwaterRenderingInstalled();
            bool isInstalledSurfaceModifiers = StylizedWaterEditor.SurfaceModifiersInstalled();

            if (isInstalledUnderwater)
            {
                AddUnderwaterShaders(ref TargetShaders);
            }

            //Strip extension features
            if(isInstalledUnderwater == false) StrippedKeywords.Add(new ShaderKeyword("UNDERWATER_ENABLED"));
            if(isInstalledSurfaceModifiers == false) StrippedKeywords.Add(new ShaderKeyword("MODIFIERS_ENABLED"));
            
            #if !URP_10_0_OR_NEWER
            StrippedKeywords.Add(new ShaderKeyword("_ADDITIONAL_LIGHT_SHADOWS"));
            #endif
            
            #if !URP_12_0_OR_NEWER
            StrippedKeywords.Add(new ShaderKeyword("_REFLECTION_PROBE_BLENDING"));
            StrippedKeywords.Add(new ShaderKeyword("_REFLECTION_PROBE_BOX_PROJECTION"));
            StrippedKeywords.Add(new ShaderKeyword("DEBUG_DISPLAY"));
            StrippedKeywords.Add(new ShaderKeyword("DYNAMICLIGHTMAP_ON"));
            #endif

            #if ENABLE_SHADER_STRIPPING_LOG  
			for (int i = 0; i < StrippedKeywords.Count; i++)
            {
                Debug.Log($"<b>{StrippedKeywords[i]}</b> keyword to be stripped");
            }
			Debug.Log(StrippedKeywords.Count + " total keywords to be stripped");
			
            for (int i = 0; i < TargetShaders.Count; i++)
            {
                Debug.Log($"<b>{TargetShaders[i].name}</b> included in stripping process");
            }
            #endif
			
			#if ENABLE_DEEP_STRIPPING_LOG
			//Clear log file first
			File.WriteAllLines(LOG_FILEPATH, new string[] {});

            File.AppendAllText(LOG_FILEPATH, $"\n{StrippedKeywords.Count} keywords to be stripped" + "\n" );
            for (int i = 0; i < TargetShaders.Count; i++)
            {
                File.AppendAllText(LOG_FILEPATH, $"\n{TargetShaders[i].name} included in stripping process" + "\n" );
            }
			
			m_stripTimer = new System.Diagnostics.Stopwatch();
			#endif
        }

        partial void AddUnderwaterShaders(ref List<Shader> shaders);

        public int callbackOrder => 0;

        public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> compilerDataList)
        {
			#if URP
			if (UniversalRenderPipeline.asset == null || compilerDataList == null || compilerDataList.Count == 0) return;
			#endif
			
            if (TargetShaders.Contains(shader) == false) return;
            
            #if ENABLE_SHADER_STRIPPING_LOG && !ENABLE_DEEP_STRIPPING_LOG
            Debug.Log("OnProcessShader running for " + shader.name + ". Pass: " + snippet.passName);
            #endif
			
			#if ENABLE_DEEP_STRIPPING_LOG
			File.AppendAllText(LOG_FILEPATH, $"\nOnProcessShader running for {shader.name}, Pass {snippet.passName}, (stage: {snippet.shaderType}). Num variants: {compilerDataList.Count}" + "\n" );
			m_stripTimer.Start();
			#endif
            
            for (int i = 0; i < compilerDataList.Count; ++i)
            {
                for (int j = 0; j < StrippedKeywords.Count; j++)
                {
                    StripKeyword(shader, StrippedKeywords[j], ref compilerDataList, ref i, snippet);
                }
            }
			
			#if ENABLE_DEEP_STRIPPING_LOG
			m_stripTimer.Stop();
			System.TimeSpan stripTimespan = m_stripTimer.Elapsed;
			File.AppendAllText(LOG_FILEPATH, $"OnProcessShader, stripping for pass {snippet.shaderType} took {stripTimespan.Minutes}m{stripTimespan.Seconds}s. Remaining variants to compile: {compilerDataList.Count}" + "\n" );
			m_stripTimer.Reset();
			#endif
        }
		
		private string GetKeywordName(Shader shader, ShaderKeyword keyword)
        {
            #if UNITY_2021_2_OR_NEWER
			return keyword.name;
			#else
            return ShaderKeyword.GetKeywordName(shader, keyword);
			#endif
        }

        private void StripKeyword(Shader shader, ShaderKeyword keyword, ref IList<ShaderCompilerData> compilerDataList, ref int i, ShaderSnippetData snippet)
        {
			#if SWS_DEV
			if(i < 0 || i > compilerDataList.Count) 
			{
				Debug.LogError($"Error tring to strip {GetKeywordName(shader, keyword)} keyword from {shader.name}. (pass: {snippet.passName} stage: {snippet.shaderType})");
				return;
			}
			#endif
			
            if (compilerDataList[i].shaderKeywordSet.IsEnabled(keyword))
            {
                compilerDataList.RemoveAt(i);
                --i;
                
                #if ENABLE_SHADER_STRIPPING_LOG && !ENABLE_DEEP_STRIPPING_LOG
                Debug.Log($"Stripped <b>{GetKeywordName(shader, keyword)}</b> from {shader.name}. (pass: {snippet.passName} stage: {snippet.shaderType})");
                #endif
				
				#if ENABLE_DEEP_STRIPPING_LOG
                File.AppendAllText(LOG_FILEPATH, "- " + $"Stripped {GetKeywordName(shader, keyword)} ({shader.name}) variant from pass {snippet.passName} (stage: {snippet.shaderType})" + "\n" );
				#endif		
            }
        }
    }
}