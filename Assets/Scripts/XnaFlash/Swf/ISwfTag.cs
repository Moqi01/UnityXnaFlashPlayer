using XnaFlash.Content;

namespace XnaFlash.Swf
{
    public interface ISwfTag
    {
        void Load(SwfStream stream, uint length, byte version);
    }

    public interface ISwfControlTag : ISwfTag
    {
    }

    public interface ISwfDefinitionTag : ISwfTag
    {
        ushort CharacterID { get; }
        CharacterType Type { get; }
    }
}
