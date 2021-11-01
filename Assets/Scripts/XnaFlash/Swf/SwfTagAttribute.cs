using System;
using System.Collections.Generic;

namespace XnaFlash.Swf
{
    /// <summary>
    /// Attribute for marking tag holding classes
    /// </summary>
    public class SwfTagAttribute : Attribute
    {
        /// <summary>
        /// Tag type ID tag inside SWF file
        /// </summary>
        public ushort TagID { get; private set; }

        /// <summary>
        /// User-friendly name of this tag
        /// </summary>
        public string TagName { get { return GetName(TagID); } }

        public SwfTagAttribute(ushort id)
        {
            TagID = id;
        }

        public override string ToString()
        {
            return string.Format("Tag '{0}' (ID: {1})", TagName, TagID);
        }

        #region Tag Names

        public static string GetName(ushort tagId)
        {
            string res;
            if (!_tagNames.TryGetValue(tagId, out res))
                return "Tag #" + tagId.ToString();
            return res;
        }

        internal static IEnumerable<ushort> TagIDs { get { return _tagNames.Keys; } }
        private static readonly Dictionary<ushort, string> _tagNames = new Dictionary<ushort, string>
        {
            {0, "End"},
            {1, "ShowFrame"},
            {2, "DefineShape"},
            {4, "PlaceObject"},
            {5, "RemoveObject"},
            {6, "DefineBits"},
            {7, "DefineButton"},
            {8, "JPEGTables"},
            {9, "SetBackgroundColor"},
            {10, "DefineFont"},
            {11, "DefineText"},
            {12, "DoAction"},
            {13, "DefineFontInfo"},
            {14, "DefineSound"},
            {15, "StartSound"},
            {17, "DefineButtonSound"},
            {18, "SoundStreamHead"},
            {19, "SoundStreamBlock"},
            {20, "DefineBitsLossless"},
            {21, "DefineBitsJPEG2"},
            {22, "DefineShape2"},
            {23, "DefineButtonCxform"},
            {24, "Protect"},
            {26, "PlaceObject2"},
            {28, "RemoveObject2"},
            {32, "DefineShape3"},
            {33, "DefineText2"},
            {34, "DefineButton2"},
            {35, "DefineBitsJPEG3"},
            {36, "DefineBitsLossless2"},
            {37, "DefineEditText"},
            {39, "DefineSprite"},
            {43, "FrameLabel"},
            {45, "SoundStreamHead2"},
            {46, "DefineMorphShape"},
            {48, "DefineFont2"},
            {56, "ExportAssets"},
            {57, "ImportAssets"},
            {58, "EnableDebugger"},
            {59, "DoInitAction"},
            {60, "DefineVideoStream"},
            {61, "VideoFrame"},
            {62, "DefineFontInfo2"},
            {64, "EnableDebugger2"},
            {65, "ScriptLimits"},
            {66, "SetTabIndex"},
            {69, "FileAttributes"},
            {70, "PlaceObject3"},
            {71, "ImportAssets2"},
            {73, "DefineFontAlignZones"},
            {74, "CSMTextSettings"},
            {75, "DefineFont3"},
            {76, "SymbolClass"},
            {77, "Metadata"},
            {78, "DefineScalingGrid"},
            {82, "DoABC"},
            {83, "DefineShape4"},
            {84, "DefineMorphShape2"},
            {86, "DefineSceneAndFrameLabelData"},
            {87, "DefineBinaryData"},
            {88, "DefineFontName"},
            {89, "StartSound2"},
            {90, "DefineBitsJPEG4"},
            {91, "DefineFont4"},
        };
        
        #endregion
    }
}
