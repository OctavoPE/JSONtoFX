using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Converter
{
    internal class JSONReader
    {
        private JObject json = new JObject();

        public String MaterialName = "";
        public String MaterialShaderKeywords = "";
        public float MainTexAlphaCutoff = 0;
        public JObject FaceBlushColor = new JObject();
        public float FaceBlushStrength = 0;

        public float BumpScale = 0;
        public JObject LineMultiplier = new JObject();
        public JObject LineDistanceControl = new JObject();
        public float LineSmoothness = 0;
        public float LineThickness = 0;

        public float ShadowRampWidth = 0;
        public float LightArea = 0;
        public float FaceMapSoftness = 0;

        public JObject FirstShadowMultColor = new JObject();
        public JObject CoolShadowMultColor = new JObject();
        public JObject SpecularColor = new JObject();
        public float SpecMulti = 0;
        public float Shininess = 0;
        public float OutlineWidth = 0;
        public JObject OutlineColor = new JObject();

        public JObject FirstShadowMultColor2 = new JObject();
        public JObject FirstShadowMultColor3 = new JObject();
        public JObject FirstShadowMultColor4 = new JObject();
        public JObject FirstShadowMultColor5 = new JObject();
        public JObject CoolShadowMultColor2 = new JObject();
        public JObject CoolShadowMultColor3 = new JObject();
        public JObject CoolShadowMultColor4 = new JObject();
        public JObject CoolShadowMultColor5 = new JObject();
        public float SpecMulti2 = 0;
        public float SpecMulti3 = 0;
        public float SpecMulti4 = 0;
        public float SpecMulti5 = 0;
        public float Shininess2 = 0;
        public float Shininess3 = 0;
        public float Shininess4 = 0;
        public float Shininess5 = 0;
        public JObject OutlineColor2 = new JObject();
        public JObject OutlineColor3 = new JObject();
        public JObject OutlineColor4 = new JObject();
        public JObject OutlineColor5 = new JObject();

        public float MetalBrightness = 0;
        public float MetalMapTileScale = 0;
        public float MetalSharpLayerOffset = 0;
        public float MetalShininess = 0;
        public float MetalSpecularAttenInShadow = 0;
        public float MetalSpecularScale = 0;

        public JObject MapLightColor = new JObject();
        public JObject MapDarkColor = new JObject();
        public JObject MapSharpLayerColor = new JObject();
        public JObject MapSpecularColor = new JObject();
        public JObject MapShadowMultiColor = new JObject();

        public bool UseBackFace = false;
        public bool UseBumpMap = false;
        public bool UseShadowRamp = false;
        public bool UseFaceMap = false;
        public bool UseMat2 = false;
        public bool UseMat3 = false;
        public bool UseMat4 = false;
        public bool UseMat5 = false;

        /// <summary>
        /// Constructor for the JSON reader. Reads all values from the provided JSON file.
        /// </summary>
        /// <param name="PathToFile">File path to the JSON file</param>
        public JSONReader(String PathToFile)
        {
            String jsonString = @File.ReadAllText(PathToFile);
            jsonString = jsonString.Replace("\n", "").Replace("\t", "").Replace(@"\", "");
            json = JObject.Parse(jsonString);

            extractGeneral();
            extractNormalValues();
            extractShadowValues();
            extractMaterialValues();
            extractMetalValues();
            extractBools();
        }

        /// <summary>
        /// Writes the .fx file from the provided JSON file. This function takes in extra optional paramters to use normals or to increase the outline size by a multiplier
        /// </summary>
        /// <param name="NameOfFile">Name of the .fx file to generate</param>
        /// <param name="useNormal">True uses normal map, false ignores normal maps; default is false</param>
        /// <param name="outlineMultiplier">Increase outline size by given multiplier; default is 1</param>
        public void WriteFXFile(String NameOfFile, bool useNormal = false, float outlineMultiplier = 1)
        {
            String txt = @$"//====================//
//  MATERIAL GENERAL : 
//====================//
{writeBool(UseBackFace)}#define BACKFACE_USE_UV2 
#define MATERIAL_ALPHA_USE 3 // 0 : none, 1 : AlphaTest, 2 : Emission, 3 : Blush
#define ALPHA_CUTOFF {MainTexAlphaCutoff.ToString("0.0")}f 
#define BLUSH_COLOR float4({writeFourArray(FaceBlushColor)})
#define BLUSH_STRENGTH {FaceBlushStrength.ToString("0.0")}f
#define BLUSH_SLIDER_NAME ""mmd blush facial""
// this is the pmx morph slider that will control the blush so it is animatable 


//====================// 
//   NORMAL MAPPING:
//====================//
{writeBool(useNormal)}#define USE_NORMAL_TEXTURE ""normalmap"" 
#define NORMAL_BUMP_SCALE {BumpScale.ToString("0.0")}f
// #define DEBUG_VISUALIZE_NORMALS 
// #define DEBUG_VISUALIZE_NORMALS_B 
// this will work regardless of if the normal map is enabled      

//====================// 
//    DETAIL LINE:
//====================// 
#define USE_TEXTURE_LINE
// this will be disabled no matter what if the normal texture is disabled
#define LINE_MULTIPLIER       float4({writeFourArray(LineMultiplier)})
#define LINE_DISTANCE_CONTROL float3({writeThreeArray(LineDistanceControl)})
#define LINE_SMOOTHNESS       {LineSmoothness.ToString("0.000")}f
#define LINE_THICKNESS        {LineThickness.ToString("0.000")}f 
// #define DEBUG_VISUALIZE_LINES // test if the lines are rendering properly 

//====================//     
//      SHADOW : 
//====================//
#define USE_LIGHTMAP_AO 
#define USE_VERTEXCOLOR_AO 
#define USE_VERTEXCOLOR_RAMP_WIDTH   
{writeBool(UseShadowRamp)}#define USE_RAMP_TEXTURE  
#define SHADOW_RAMP_WIDTH 0.1f 
#define SHADOW_LIGHT_AREA  0.5f 
#define FACE_SHADOW_SOFTNESS {FaceMapSoftness.ToString("0.00")}f
#define FACE_LIGHTMAP ""sub/tex/Avatar_Girl_Tex_FaceLightmap.png"" // face map path
#define USE_FACE_SHADOW_MAP
//#define DEBUG_VISUALIZE_SHADOW

//====================//
//    MATERIAL 1 : 
//====================//
#define COLOR_1 float4(1.0f, 1.0f, 1.0f, 1.0f)
#define SHADOW_WARM_COLOR_1 float4({writeFourArray(FirstShadowMultColor)})
#define SHADOW_COOL_COLOR_1 float4({writeFourArray(CoolShadowMultColor)})
#define USE_SPECULAR_MAT
#define SPECULAR_COLOR float4({writeCustomSpecular()})
// specular color is shared by all materials 
#define SPECULAR_MULTI_1 {SpecMulti.ToString("0.00")}f
#define SPECULAR_SHINE_1 {Shininess.ToString("0.00")}f
#define USE_OUTLINE 
#define OUTLINE_WIDTH {(OutlineWidth * outlineMultiplier).ToString("0.00")}f
#define OUTLINE_COLOR_1 float4({writeFourArray(OutlineColor)})

//====================//
//    MATERIAL 2 : 
//====================//
{writeBool(UseMat2)}#define USE_MATERIAL_2
#define COLOR_2 float4(1.0f, 1.0f, 1.0f, 1.0f) 
#define SHADOW_WARM_COLOR_2 float4({writeFourArray(FirstShadowMultColor2)})
#define SHADOW_COOL_COLOR_2 float4({writeFourArray(CoolShadowMultColor2)})
#define SPECULAR_MULTI_2 {SpecMulti2.ToString("0.00")}f
#define SPECULAR_SHINE_2 {Shininess2.ToString("0.00")}f
#define OUTLINE_COLOR_2 float4({writeFourArray(OutlineColor2)})

//====================//
//    MATERIAL 3 : 
//====================//
{writeBool(UseMat3)}#define USE_MATERIAL_3
#define COLOR_3 float4(1.0f, 1.0f, 1.0f, 1.0f) 
#define SHADOW_WARM_COLOR_3 float4({writeFourArray(FirstShadowMultColor3)})
#define SHADOW_COOL_COLOR_3 float4({writeFourArray(CoolShadowMultColor3)})
#define SPECULAR_MULTI_3 {SpecMulti3.ToString("0.00")}f
#define SPECULAR_SHINE_3 {Shininess3.ToString("0.00")}f
#define OUTLINE_COLOR_3 float4({writeFourArray(OutlineColor3)})

//====================//
//    MATERIAL 4 : 
//====================//
{writeBool(UseMat4)}#define USE_MATERIAL_4
#define COLOR_4 float4(1.0f, 1.0f, 1.0f, 1.0f) 
#define SHADOW_WARM_COLOR_4 float4({writeFourArray(FirstShadowMultColor4)})
#define SHADOW_COOL_COLOR_4 float4({writeFourArray(CoolShadowMultColor4)})
#define SPECULAR_MULTI_4 {SpecMulti4.ToString("0.00")}f
#define SPECULAR_SHINE_4 {Shininess4.ToString("0.00")}f
#define OUTLINE_COLOR_4 float4({writeFourArray(OutlineColor4)})

//====================//
//    MATERIAL 5 : 
//====================//
{writeBool(UseMat5)}#define USE_MATERIAL_5
#define COLOR_5 float4(1.0f, 1.0f, 1.0f, 1.0f) 
#define SHADOW_WARM_COLOR_5 float4({writeFourArray(FirstShadowMultColor5)})
#define SHADOW_COOL_COLOR_5 float4({writeFourArray(CoolShadowMultColor5)})
#define SPECULAR_MULTI_5 {SpecMulti5.ToString("0.00")}f
#define SPECULAR_SHINE_5 {Shininess5.ToString("0.00")}f
#define OUTLINE_COLOR_5 float4({writeFourArray(OutlineColor5)})

//====================//
//       METAL 
//====================//
{writeBool(!UseFaceMap)}#define USE_METAL_MAT            ""sub/tex/Avatar_Tex_MetalMap.png""
{writeBool(!UseFaceMap)}#define USE_SPECULAR_RAMP        ""sub/tex/Avatar_Tex_Specular_Ramp.PNG""
#define METAL_BRIGHTNESS         {MetalBrightness.ToString("0.00")}f
#define METAL_TILE               float2({MetalMapTileScale.ToString("0.00")}f, {MetalMapTileScale.ToString("0.00")}f) 
#define METAL_SHARP_OFFSET       {MetalSharpLayerOffset.ToString("0.00")}f
#define METAL_SHININESS          {MetalShininess.ToString("0.00")}f
#define METAL_SPECULAR_IN_SHADOW {MetalSpecularAttenInShadow.ToString("0.00")}f
#define METAL_SPECULAR_SCALE     {MetalSpecularScale.ToString("0.00")}f 
#define METAL_LIGHT              float4({writeFourArray(MapLightColor)})
#define METAL_DARK               float4({writeFourArray(MapDarkColor)})
#define METAL_SHARP_COLOR        float4({writeFourArray(MapSharpLayerColor)})
#define METAL_SPECULAR_COLOR     float4({writeFourArray(MapSpecularColor)})
#define METAL_SHADOW_COLOR       float4({writeFourArray(MapShadowMultiColor)})
//============================================================================//
#include ""shader.fxsub""";
            if (NameOfFile.Contains(".fx"))
            {
                File.WriteAllText(NameOfFile, txt);
            }
            else
            {
                File.WriteAllText(NameOfFile + ".fx", txt);
            }
        }
        private String writeBool(bool calc)
        {
            if (calc)
            {
                return "";
            }
            else
            {
                return "//";
            }
        }
        private String writeThreeArray(JObject jo)
        {
            float r = (float)jo.GetValue("r");
            float g = (float)jo.GetValue("g");
            float b = (float)jo.GetValue("b");
            return $"{r.ToString("0.0000")}f, {g.ToString("0.0000")}f, {b.ToString("0.0000")}f";
        }
        private String writeFourArray(JObject jo)
        {
            float r = (float)jo.GetValue("r");
            float g = (float)jo.GetValue("g");
            float b = (float)jo.GetValue("b");
            float a = (float)jo.GetValue("a");
            return $"{r.ToString("0.0000")}f, {g.ToString("0.0000")}f, {b.ToString("0.0000")}f, {a.ToString("0.0000")}f";
        }
        private String writeCustomSpecular() {
            if (MaterialName.Contains("Hair")) {
                return "(0.5f, 0.5f, 0.5f, 0.5f)";
            }
            return "(0.1f, 0.1f, 0.1f, 0.1f)";
        }
        private void extractGeneral()
        {
            MaterialName = json["m_Name"].ToString();
            MaterialShaderKeywords = json["m_ShaderKeywords"].ToString();
            MainTexAlphaCutoff = getMaterialValue("_MainTexAlphaCutoff");
            FaceBlushColor = getMaterialObj("_FaceBlushColor");
            FaceBlushStrength = getMaterialValue("_FaceBlushStrength");
        }
        private void extractNormalValues()
        {
            BumpScale = getMaterialValue("_BumpScale");
            LineMultiplier = getMaterialObj("_TextureLineMultiplier");
            LineDistanceControl = getMaterialObj("_TextureLineDistanceControl");
            LineSmoothness = getMaterialValue("_TextureLineSmoothness");
            LineThickness = getMaterialValue("_TextureLineThickness");
        }
        private void extractShadowValues()
        {
            ShadowRampWidth = getMaterialValue("_ShadowRampWidth");
            LightArea = getMaterialValue("_LightArea");
            FaceMapSoftness = getMaterialValue("_FaceMapSoftness");
        }
        private void extractMaterialValues()
        {
            FirstShadowMultColor = getMaterialObj("_FirstShadowMultColor");
            CoolShadowMultColor = getMaterialObj("_CoolShadowMultColor");
            SpecularColor = getMaterialObj("_SpecularColor");
            SpecMulti = getMaterialValue("_SpecMulti");
            Shininess = getMaterialValue("_Shininess");
            OutlineWidth = getMaterialValue("_OutlineWidth");
            OutlineColor = getMaterialObj("_OutlineColor");

            FirstShadowMultColor2 = getMaterialObj("_FirstShadowMultColor2");
            FirstShadowMultColor3 = getMaterialObj("_FirstShadowMultColor3");
            FirstShadowMultColor4 = getMaterialObj("_FirstShadowMultColor4");
            FirstShadowMultColor5 = getMaterialObj("_FirstShadowMultColor5");
            CoolShadowMultColor2 = getMaterialObj("_CoolShadowMultColor2");
            CoolShadowMultColor3 = getMaterialObj("_CoolShadowMultColor3");
            CoolShadowMultColor4 = getMaterialObj("_CoolShadowMultColor4");
            CoolShadowMultColor5 = getMaterialObj("_CoolShadowMultColor5");
            SpecMulti2 = getMaterialValue("_SpecMulti2");
            SpecMulti3 = getMaterialValue("_SpecMulti3");
            SpecMulti4 = getMaterialValue("_SpecMulti4");
            SpecMulti5 = getMaterialValue("_SpecMulti5");
            Shininess2 = getMaterialValue("_Shininess2");
            Shininess3 = getMaterialValue("_Shininess3");
            Shininess4 = getMaterialValue("_Shininess4");
            Shininess5 = getMaterialValue("_Shininess5");
            OutlineColor2 = getMaterialObj("_OutlineColor2");
            OutlineColor3 = getMaterialObj("_OutlineColor3");
            OutlineColor4 = getMaterialObj("_OutlineColor4");
            OutlineColor5 = getMaterialObj("_OutlineColor5");
        }
        private void extractMetalValues()
        {
            MetalBrightness = getMaterialValue("_MTMapBrightness");
            MetalMapTileScale = getMaterialValue("_MTMapTileScale");
            MetalSharpLayerOffset = getMaterialValue("_MTSharpLayerOffset");
            MetalShininess = getMaterialValue("_MTShininess");
            MetalSpecularAttenInShadow = getMaterialValue("_MTSpecularAttenInShadow");
            MetalSpecularScale = getMaterialValue("_MTSpecularScale");
            MapLightColor = getMaterialObj("_MTMapLightColor");
            MapDarkColor = getMaterialObj("_MTMapDarkColor");
            MapSharpLayerColor = getMaterialObj("_MTSharpLayerColor");
            MapSpecularColor = getMaterialObj("_MTSpecularColor");
            MapShadowMultiColor = getMaterialObj("_MTShadowMultiColor");
        }
        private void extractBools()
        {
            UseBackFace = IsBool("_UseBackFaceUV2");
            UseBumpMap = IsBool("_UseBumpMap");
            UseShadowRamp = IsBool("_UseShadowRamp");
            UseFaceMap = IsBool("_UseFaceMapNew");
            UseMat2 = IsBool("_UseMaterial2");
            UseMat3 = IsBool("_UseMaterial3");
            UseMat4 = IsBool("_UseMaterial4");
            UseMat5 = IsBool("_UseMaterial5");
        }
        private bool IsBool(String key)
        {
            float temp = getMaterialValue(key);
            return temp == 1;
        }

        private float getMaterialValue(String key)
        {
            JObject savedProperties = (JObject)json["m_SavedProperties"];
            JArray floatsArray = (JArray)savedProperties["m_Floats"];

            float value = 0;

            foreach (JArray floatItem in floatsArray)
            {
                if (floatItem[0].ToString() == key)
                {
                    value = (float)floatItem[1];
                    break;
                }
            }
            return value;
        }
        private JObject getMaterialObj(String key)
        {
            JObject savedProperties = (JObject)json["m_SavedProperties"];
            JArray floatsArray = (JArray)savedProperties["m_Colors"];

            JObject value = new JObject();

            foreach (JArray floatItem in floatsArray)
            {
                if (floatItem[0].ToString() == key)
                {
                    value = (JObject)floatItem[1];
                    break;
                }
            }
            return value;
        }

        public void PrintAllValues()
        {
            PrintGeneralValues();
            PrintNormalValues();
            PrintShadowValues();
            PrintMaterialValues();
            PrintMetalValues();
            PrintBools();
        }
        public void PrintGeneralValues()
        {
            Console.WriteLine($"Material Name: {MaterialName}");
            Console.WriteLine($"Material Shader Keywords: {MaterialShaderKeywords}");
            Console.WriteLine($"MainTexAlphaCutoff: {MainTexAlphaCutoff}");
            Console.WriteLine($"FaceBlushColor: {FaceBlushColor}");
            Console.WriteLine($"FaceBlushStrength: {FaceBlushStrength}");
        }
        public void PrintNormalValues()
        {
            Console.WriteLine($"Bump Scale: {BumpScale}");
            Console.WriteLine($"Line Multiplier: {LineMultiplier}");
            Console.WriteLine($"Line Distance Control: {LineDistanceControl}");
            Console.WriteLine($"Line Smoothness: {LineSmoothness}");
            Console.WriteLine($"Line Thickness: {LineThickness}");
        }
        public void PrintShadowValues()
        {
            Console.WriteLine($"ShadowRampWidth: {ShadowRampWidth}");
            Console.WriteLine($"LightArea: {LightArea}");
            Console.WriteLine($"FaceMapSoftness: {FaceMapSoftness}");
        }
        public void PrintMaterialValues()
        {
            Console.WriteLine($"FirstShadowMultiColor: {FirstShadowMultColor}");
            Console.WriteLine($"CoolShadowMultiColor: {CoolShadowMultColor}");
            Console.WriteLine($"SpecularColor: {SpecularColor}");
            Console.WriteLine($"SpecMulti: {SpecMulti}");
            Console.WriteLine($"Shininess: {Shininess}");
            Console.WriteLine($"OutlineWidth: {OutlineWidth}");
            Console.WriteLine($"OutlineColor: {OutlineColor}");

            Console.WriteLine($"FirstShadowMultiColor2: {FirstShadowMultColor2}");
            Console.WriteLine($"FirstShadowMultiColor3: {FirstShadowMultColor3}");
            Console.WriteLine($"FirstShadowMultiColor4: {FirstShadowMultColor4}");
            Console.WriteLine($"FirstShadowMultiColor5: {FirstShadowMultColor5}");
            Console.WriteLine($"CoolShadowMultiColor2: {CoolShadowMultColor2}");
            Console.WriteLine($"CoolShadowMultiColor3: {CoolShadowMultColor3}");
            Console.WriteLine($"CoolShadowMultiColor4: {CoolShadowMultColor4}");
            Console.WriteLine($"CoolShadowMultiColor5: {CoolShadowMultColor5}");
            Console.WriteLine($"SpecMulti2: {SpecMulti2}");
            Console.WriteLine($"SpecMulti3: {SpecMulti3}");
            Console.WriteLine($"SpecMulti4: {SpecMulti4}");
            Console.WriteLine($"SpecMulti5: {SpecMulti5}");
            Console.WriteLine($"Shininess2: {Shininess2}");
            Console.WriteLine($"Shininess3: {Shininess3}");
            Console.WriteLine($"Shininess4: {Shininess4}");
            Console.WriteLine($"Shininess5: {Shininess5}");
            Console.WriteLine($"OutlineColor2: {OutlineColor2}");
            Console.WriteLine($"OutlineColor3: {OutlineColor3}");
            Console.WriteLine($"OutlineColor4: {OutlineColor4}");
            Console.WriteLine($"OutlineColor5: {OutlineColor5}");

        }
        public void PrintMetalValues()
        {
            Console.WriteLine($"Metal Brightness: {MetalBrightness}");                          // good
            Console.WriteLine($"Metal MapTileScale: {MetalMapTileScale}");                      // good
            Console.WriteLine($"Metal SharpLayerOffset: {MetalSharpLayerOffset}");              // good
            Console.WriteLine($"Metal Shininess: {MetalShininess}");                            // good
            Console.WriteLine($"Metal SpecularAttenInShadow: {MetalSpecularAttenInShadow}");    // good
            Console.WriteLine($"Metal SpecularScale: {MetalSpecularScale}");                    // good

            Console.WriteLine($"Metal MapLightColor: {MapLightColor}");        // { "r": 1.0, "g": 1.0, "b": 1.0, "a": 1.0 }
            //Console.WriteLine($"RGBA values: R:{MapLightColor.GetValue("r")} G:{MapLightColor.GetValue("g")} B:{MapLightColor.GetValue("b")} A:{MapLightColor.GetValue("a")}");
            Console.WriteLine($"Metal MapDarkColor: {MapDarkColor}");          // { "r": 0.0, "g": 0.0, "b": 0.0, "a": 0.0 } 
            Console.WriteLine($"Metal SharpLayerColor: {MapSharpLayerColor}");    // { "r": 1.0, "g": 1.0, "b": 1.0, "a": 1.0 }
            Console.WriteLine($"Metal SpecularColor: {MapSpecularColor}");        // { "r": 1.0, "g": 1.0, "b": 1.0, "a": 1.0 }
            Console.WriteLine($"Metal Shadow MultiColor: {MapShadowMultiColor}"); // { "r": 0.800000011920929, "g": 0.800000011920929, "b": 0.800000011920929, "a": 0.800000011920929 }
        }
        public void PrintBools()
        {
            Console.WriteLine($"UseBackFace: {UseBackFace}");
            Console.WriteLine($"UseBumpMap: {UseBumpMap}");
            Console.WriteLine($"UseShadowRamp: {UseShadowRamp}");
            Console.WriteLine($"UseFaceMap: {UseFaceMap}");
            Console.WriteLine($"UseMat2: {UseMat2}");
            Console.WriteLine($"UseMat3: {UseMat3}");
            Console.WriteLine($"UseMat4: {UseMat4}");
            Console.WriteLine($"UseMat5: {UseMat5}");
        }

    }
}
