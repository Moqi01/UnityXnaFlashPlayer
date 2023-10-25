using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XnaVG
{
    public class VGKerningTable
    {
        private Dictionary<int, float> _table;

        public virtual float this[char left, char right]
        {
            get
            {
                float kern;
                _table.TryGetValue(MakeKey(left, right), out kern);
                return kern;
            }
        }

        public VGKerningTable()
        {
            _table = new Dictionary<int, float>();
        }
        public VGKerningTable(int capacity)
        {
            _table = new Dictionary<int, float>(capacity);
        }

        public void Add(char left, char right, float kerning)
        {
            int v = MakeKey(left, right);
            if(!_table.ContainsKey(v))
            _table.Add(v, kerning);
        }

        public bool Contains(char left, char right)
        {
            return _table.ContainsKey(MakeKey(left, right));
        }

        private int MakeKey(char left, char right)
        {
            return ((int)left) << 16 | (((int)right) & 0xFFFF);
        }

        public void Serialize(BinaryWriter writer)
        {
            if (_table == null)
            {
                writer.Write(0);
                return;
            }

            writer.Write(_table.Count);
            foreach (var k in _table)
            {
                writer.Write(k.Key);
                writer.Write(k.Value);
            }
        }
        public static VGKerningTable Deserialize(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            if (count == 0)
                return null;

            var tab = new VGKerningTable(count);
            for (int i = 0; i < count; i++)
            {
                var k = reader.ReadInt32();
                var v = reader.ReadSingle();
                tab._table.Add(k, v);
            }
            return tab;
        }

        internal static readonly VGKerningTable EmptyTable = new EmptyKerningTable();
        private class EmptyKerningTable : VGKerningTable
        {
            public override float this[char left, char right] { get { return 0f; } }
            public EmptyKerningTable()
                : base()
            {
                _table = null;
            }
        }
    }
}
