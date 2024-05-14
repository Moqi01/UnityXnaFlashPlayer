using System;
using System.Collections.Generic;
using System.Linq;

namespace XnaFlash.Swf
{
    public static class SwfTagFactory
    {
        private static Dictionary<ushort, KeyValuePair<string, Type>> sDictionary = null;
        public static IEnumerable<ushort> UnimplementedTags
        {
            get
            {
                return SwfTagAttribute.TagIDs.Except(sDictionary.Keys);
            }
        }

        private static void Initialize()
        {
            sDictionary = new Dictionary<ushort, KeyValuePair<string, Type>>();
            Type[] types = typeof(SwfTagFactory).Assembly.GetTypes();
            foreach (var cls in types)
            {
                if (!cls.IsClass) continue;
                if (!cls.GetInterfaces().Contains(typeof(ISwfTag))) continue;
                if (cls.GetConstructor(new Type[0]) == null) continue;

                var attr = (SwfTagAttribute)cls.GetCustomAttributes(typeof(SwfTagAttribute), false).FirstOrDefault();
                if (attr == null) continue;

                sDictionary.Add(attr.TagID, new KeyValuePair<string, Type>(attr.TagName, cls));
            }
        }

        public static ISwfTag LoadTag(SwfStream stream, ushort id, uint length, byte version, ref string name)
        {
            if (sDictionary == null)
                Initialize();

            KeyValuePair<string, Type> info;
            if (!sDictionary.TryGetValue(id, out info))
                return null;

            name = info.Key;
            var tag = (ISwfTag)Activator.CreateInstance(info.Value);
            tag.Load(stream, length, version);
            return tag;
        }
    }
}
