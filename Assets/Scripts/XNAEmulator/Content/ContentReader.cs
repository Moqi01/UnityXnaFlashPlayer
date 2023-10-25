using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Microsoft.Xna.Framework.Content
{
	public class ContentReader : BinaryReader
	{
		private ContentManager contentManager;

		// Token: 0x040002BD RID: 701
		private string assetName;

		// Token: 0x040002BE RID: 702
		private Action<IDisposable> recordDisposableObject;

		// Token: 0x040002BF RID: 703
		//private ContentTypeReader[] typeReaders;

		// Token: 0x040002C0 RID: 704
		private List<Action<object>>[] sharedResourceFixups;

		// Token: 0x040002C1 RID: 705
		internal int graphicsProfile;
		private ContentReader(ContentManager contentManager, Stream input, string assetName, Action<IDisposable> recordDisposableObject, int graphicsProfile) : base(input)
		{
			this.contentManager = contentManager;
			this.assetName = assetName;
			this.recordDisposableObject = recordDisposableObject;
			this.graphicsProfile = graphicsProfile;
		}

		/// <summary>Gets the ContentManager associated with the ContentReader.</summary>
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x00021940 File Offset: 0x00020D40
		public ContentManager ContentManager
		{
			get
			{
				return this.contentManager;
			}
		}
		public string AssetName
		{
			get
			{
				return this.assetName;
			}
		}

        internal Vector4 ReadVector4()
        {
			return new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }
    }
}