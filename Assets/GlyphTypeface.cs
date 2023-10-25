using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace System.Windows.Media
{
    public class GlyphTypeface
    {
        private Uri uri;

        public GlyphTypeface(Uri uri)
        {
            this.uri = uri;
        }

        public Dictionary<int, ushort> CharacterToGlyphMap { get; internal set; }
        public float Height { get; internal set; }
        public float[] AdvanceWidths { get; internal set; }
        public float[] AdvanceHeights { get; internal set; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        internal object GetGlyphOutline(ushort code, int v1, int v2)
        {
            throw new NotImplementedException();
        }
    }
}