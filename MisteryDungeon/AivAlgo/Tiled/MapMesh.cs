using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiv.Tiled
{
    public class MapMesh
    {
        private List<LayerMesh> layers;

        Vector2 position;
        Vector2 scale;

        public Vector2 Position
        {
            get { return position; }
            set { SetPosition(value); }
        }

        public Vector2 Scale
        {
            get { return scale; }
            set { SetScale(value); }
        }

        public MapMesh(Map _map)
        {
            layers = new List<LayerMesh>();
            foreach (var layer in _map.Layers)
            {
                if (_map.Tilesets.Count > 1) throw new Exception("Unsupported number of Tilesets");
                layers.Add(new LayerMesh(layer, _map.Tilesets[0], _map.TileWidth, _map.TileHeight, _map.Orientation, _map.StaggerAxis, _map.StaggerIndex, _map.HexSideLength));
            }
        }

        public void Draw()
        {
            foreach (var layer in layers) layer.Draw();
        }

        public void DrawWireframe(Vector4 color)
        {
            foreach (var layer in layers) layer.DrawWireframe(color);
        }

        private void SetScale(Vector2 scale)
        {
            foreach(var layer in layers) layer.Scale = scale;
            this.scale = scale;
        }

        private void SetPosition(Vector2 position)
        {
            foreach (var layer in layers) layer.Position = position;
            this.position = position;
        }
    }
}
