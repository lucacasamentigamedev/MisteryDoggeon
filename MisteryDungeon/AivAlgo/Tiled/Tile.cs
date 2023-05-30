using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiv.Tiled
{
    public struct Tile
    {
        // 1000 0000 0000 0000 0000 0000 0000 0000
        private static uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
        // 0100 0000 0000 0000 0000 0000 0000 0000
        private static uint FLIPPED_VERTICALLY_FLAG = 0x40000000;
        // 0010 0000 0000 0000 0000 0000 0000 0000
        private static uint FLIPPED_DIAGONALLY_FLAG = 0x20000000;

        public int Gid { get; private set; }
        public bool HorizontalFlip { get; private set; }
        public bool VerticalFlip { get; private set; }
        public bool DiagonalFlip { get; private set; }
        public Tile(uint _gid)
        {
            HorizontalFlip = (_gid & FLIPPED_HORIZONTALLY_FLAG) != 0;
            VerticalFlip = (_gid & FLIPPED_VERTICALLY_FLAG) != 0;
            DiagonalFlip = (_gid & FLIPPED_DIAGONALLY_FLAG) != 0;

            // Save GID remainder to int
            Gid = (int)(_gid & ~(FLIPPED_HORIZONTALLY_FLAG |
                        FLIPPED_VERTICALLY_FLAG |
                        FLIPPED_DIAGONALLY_FLAG));
        }
    }
}
