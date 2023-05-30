using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiv.Tiled
{
    public class TileSprite
    {
        public static int CorrectionFactor = 1;

        private Sprite sprite;
        public int Width { get; private set; }
        public int Height { get; private set; }

        private int offsetX;
        public int OffsetX { get { return offsetX; } }
        private int offsetY;
        public int OffsetY { get { return offsetY; } }
        private Texture texture;
        public Texture Texture { get { return texture; } }

        private Vector2 fakePosition;
        private Vector2 fakeScale;
        public Vector2 Position { get { return sprite != null ? sprite.position : fakePosition; } set { if (sprite != null) { sprite.position = value; } else { fakePosition = value; } } }
        public Vector2 Scale { get { return sprite != null ? sprite.scale : fakeScale; } set { if (sprite != null) { sprite.scale = value; } else { fakeScale = value; } } }

        public TileSprite(List<Tileset> _tilesets, Tile _tile)
        {
            var tileset = _tilesets.Find((ts) =>
            {
                return ts.FirstGid <= _tile.Gid && (ts.TileCount + ts.FirstGid) > _tile.Gid;
            });
            if (tileset != null)
            {
                sprite = new Sprite(tileset.TileWidth, tileset.TileHeight);

                texture = tileset.Source;
                Width = tileset.TileWidth;
                Height = tileset.TileHeight;
                offsetX = tileset.HorizontalOffset(_tile.Gid);
                offsetY = tileset.VerticalOffset(_tile.Gid);
                sprite.FlipX = _tile.HorizontalFlip;
                sprite.FlipY = _tile.VerticalFlip;
            }
        }

        public void Draw()
        {
            if (sprite != null)
            {
                sprite.DrawTexture(texture, offsetX + CorrectionFactor, offsetY + CorrectionFactor, Width - CorrectionFactor, Height - CorrectionFactor);
            }
        }
    }
}
