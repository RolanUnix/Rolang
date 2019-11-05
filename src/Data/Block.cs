namespace Rolang.Data
{
    public class Block
    {
        public Block LastBlock;
        public readonly Space Space;

        public Block()
        {
            LastBlock = null;
            Space = new Space();
        }
    }
}