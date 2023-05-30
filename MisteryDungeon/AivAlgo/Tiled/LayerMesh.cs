using Aiv.Fast2D;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiv.Tiled
{
    public class LayerMesh
    {
        private static string tiledVertexShader = @"
#version 330 core

layout(location=0) in vec2 vertex;
layout(location=1) in vec2 uv;
layout(location=2) in vec4 vc;

uniform mat4 mvp;
out vec2 uvout;
out vec4 vertex_color;

void main(){
    gl_Position = mvp * vec4(vertex.xy, 0, 1.0);
    uvout = uv;
    vertex_color = vc;
}
";
        private static string tiledFragmentShader = @"
#version 330 core

precision highp float;

uniform vec4 color;
uniform float use_texture;
uniform float use_wireframe;
uniform sampler2D tex;

in vec2 uvout;
in vec4 vertex_color;

out vec4 out_color;

void main() {
    if (use_texture > 0.0) {
        out_color = texture(tex, uvout);
        if(!(vertex_color.a > 0)){
            discard;
        }
    }
    else if (use_wireframe > 0.0) {
        if(any(lessThan(vertex_color.xyz, vec3(use_wireframe)))) {
            out_color = color;
        }
        else {
            discard;
        }
        return;
    }
    else {
        out_color = vertex_color;
    }
    out_color += color;
}
";
        private static string tiledVertexShaderObsolete = @"
layout(location=0) in vec2 vertex;
layout(location=1) in vec2 uv;
layout(location=2) in vec4 vc;

uniform mat4 mvp;
out vec2 uvout;
out vec4 vertex_color;

void main(){
    gl_Position = mvp * vec4(vertex.xy, 0.0, 1.0);
    uvout = uv;
    vertex_color = vc;
}
";
        private static string tiledFragmentShaderObsolete = @"
precision mediump float;

uniform vec4 color;
uniform float use_texture;
uniform float use_wireframe;
uniform sampler2D tex;

in vec2 uvout;
in vec4 vertex_color;

out vec4 out_color;

void main() {
    if (use_texture > 0.0) {
        out_color = texture(tex, uvout);
        if(!(vertex_color.a > 0)){
            discard;
        }
    }
    else if (use_wireframe > 0.0) {
        if(any(lessThan(vertex_color.xyz, vec3(use_wireframe)))) {
            out_color = color;
        }
        else {
            discard;
        }
        return;
    }
    else {
        out_color = vertex_color;
    }
    out_color += color;
}
";
        internal static Shader tiledShader = new Shader(tiledVertexShader, tiledFragmentShader, tiledVertexShaderObsolete, tiledFragmentShaderObsolete, new string[] { "vertex", "uv", "vc" });

        private int width { get { return layer.Tiles.GetLength(0); } }
        private int height { get { return layer.Tiles.GetLength(1); } }

        public Vector2 Position
        {
            get { return mapMesh.position; }
            set { mapMesh.position = value; }
        }

        public Vector2 Scale
        {
            get { return this.mapMesh.scale; }
            set { mapMesh.scale = value; }
        }

        private Mesh mapMesh = new Mesh(tiledShader);
        private Tileset tileset;
        private Layer layer;

        public LayerMesh(Layer layer, Tileset tileset, int tileWidth, int tileHeight, EOrientation orientation, EStaggerAxis staggerAxis, EStaggerIndex staggerIndex, int hexSideLength)
        {
            this.tileset = tileset;
            this.layer = layer;

            tileset.Source.SetNearest();

            switch (orientation)
            {
                case EOrientation.Orthogonal:
                    mapMesh.v = ComputeVerticesOrthogonal(tileWidth, tileHeight);
                    mapMesh.uv = ComputeUVs(EStaggerAxis.Y);
                    mapMesh.vc = ComputeVCs(EStaggerAxis.Y);
                    break;
                case EOrientation.Isometric:
                    mapMesh.v = ComputeVerticesIsometric(tileWidth, tileHeight);
                    mapMesh.uv = ComputeUVs(EStaggerAxis.Y);
                    mapMesh.vc = ComputeVCs(EStaggerAxis.Y);
                    break;
                case EOrientation.Staggered:
                    mapMesh.v = ComputeVerticesHexagonal(tileWidth, tileHeight, staggerAxis, staggerIndex, 0);
                    mapMesh.uv = ComputeUVs(staggerAxis);
                    mapMesh.vc = ComputeVCs(staggerAxis);
                    break;
                case EOrientation.Hexagonal:
                    mapMesh.v = ComputeVerticesHexagonal(tileWidth, tileHeight, staggerAxis, staggerIndex, hexSideLength);
                    mapMesh.uv = ComputeUVs(staggerAxis);
                    mapMesh.vc = ComputeVCs(staggerAxis);
                    break;
                default:
                    mapMesh.v = ComputeVerticesOrthogonal(tileWidth, tileHeight);
                    mapMesh.uv = ComputeUVs(EStaggerAxis.Y);
                    mapMesh.vc = ComputeVCs(EStaggerAxis.Y);
                    break;
            }
            mapMesh.hasVertexColors = true;
            mapMesh.Update();
        }

        private int SetQuadAt(float[] vertices, int i, float x, float y)
        {
            vertices[i++] = x + (float)layer.OffsetX;
            vertices[i++] = y + (float)layer.OffsetY;

            vertices[i++] = x + (float)layer.OffsetX;
            vertices[i++] = y + (float)layer.OffsetY + tileset.TileHeight;

            vertices[i++] = x + (float)layer.OffsetX + tileset.TileWidth;
            vertices[i++] = y + (float)layer.OffsetY;

            vertices[i++] = x + (float)layer.OffsetX + tileset.TileWidth;
            vertices[i++] = y + (float)layer.OffsetY;

            vertices[i++] = x + (float)layer.OffsetX + tileset.TileWidth;
            vertices[i++] = y + (float)layer.OffsetY + tileset.TileHeight;

            vertices[i++] = x + (float)layer.OffsetX;
            vertices[i++] = y + (float)layer.OffsetY + tileset.TileHeight;

            return i;
        }

        private float[] ComputeVerticesOrthogonal(int tileWidth, int tileHeight)
        {
            // 2 triangoli, 3 vertici, 2 float (x, y) = 2*3*2 = 12
            float[] vertices = new float[height * width * 12];
            int i = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    i = SetQuadAt(vertices, i, x * tileWidth, y * tileHeight);
                }
            }

            return vertices;
        }

        private float[] ComputeVerticesIsometric(int tileWidth, int tileHeight)
        {
            float[] vertices = new float[height * width * 12];
            var halfTileWidth = tileWidth * 0.5f;
            var halfTileHeight = tileHeight * 0.5f;
            int i = 0;
            for (int tileY = 0; tileY < height; tileY++)
            {
                for (int tileX = 0; tileX < width; tileX++)
                {
                    var x = (tileX - tileY - 1) * halfTileWidth;
                    var y = (tileX + tileY) * halfTileHeight;

                    i = SetQuadAt(vertices, i, x, y);
                }
            }

            return vertices;
        }

        private float[] ComputeVerticesHexagonal(int tileWidth, int tileHeight, EStaggerAxis staggerAxis, EStaggerIndex staggerIndex, int hexSideLength)
        {
            float[] vertices = new float[height * width * 12];
            int i = 0;
            int oddity = (int)staggerIndex;

            if (staggerAxis == EStaggerAxis.X)
            {
                for (int tileY = 0; tileY < height; tileY++)
                {
                    for (int tileX = 0; tileX < width; tileX++)
                    {
                        var x = tileX * (tileWidth + hexSideLength) / 2.0f;
                        var y = (tileY + (((oddity + tileX) % 2) / 2.0f)) * tileHeight;

                        i = SetQuadAt(vertices, i, x, y);
                    }
                }
            }
            else
            {
                for (int tileY = 0; tileY < height; tileY++)
                {
                    for (int tileX = 0; tileX < width; tileX++)
                    {
                        var x = (tileX + (((oddity + tileY) % 2) / 2.0f)) * tileWidth;
                        var y = tileY * (tileHeight + hexSideLength) / 2.0f;

                        i = SetQuadAt(vertices, i, x, y);
                    }
                }
            }

            return vertices;
        }

        private int SetUVAt(float[] uvs, int index, float left, float top)
        {
            float right = left + tileset.TileWidth / (float)(tileset.Source.Width);
            float bottom = top + tileset.TileHeight / (float)(tileset.Source.Height);

            uvs[index++] = left;
            uvs[index++] = top;

            uvs[index++] = left;
            uvs[index++] = bottom;

            uvs[index++] = right;
            uvs[index++] = top;

            uvs[index++] = right;
            uvs[index++] = top;

            uvs[index++] = right;
            uvs[index++] = bottom;

            uvs[index++] = left;
            uvs[index++] = bottom;

            return index;
        }

        private float[] ComputeUVs(EStaggerAxis staggerAxis)
        {
            // 2 triangoli, 3 vertici, 2 float (u,v) = 2*3*2 = 12
            float[] uvs = new float[height * width * 12];

            int index = 0;

            for (int tileY = 0; tileY < height; tileY++)
            {
                for (int tileX = 0; tileX < width; tileX++)
                {
                    if (layer.Tiles[tileX, tileY].Gid == 0)
                    {
                        index += 12;
                        continue;
                    }

                    float left = tileset.HorizontalOffset(layer.Tiles[tileX, tileY].Gid) / (float)(tileset.Source.Width);
                    float top = tileset.VerticalOffset(layer.Tiles[tileX, tileY].Gid) / (float)(tileset.Source.Height);

                    index = SetUVAt(uvs, index, left, top);
                }
            }

            return uvs;
        }

        private int SetVCAt(float[] vcs, int index, float alpha)
        {
            for (int c = 0; c < 6; ++c)
            {
                vcs[index++] = 0; // r
                vcs[index++] = 0; // g
                vcs[index++] = 0; // b
                vcs[index++] = alpha; // a
            }
            return index;
        }

        private float[] ComputeVCs(EStaggerAxis staggerAxis)
        {
            // 2 triangoli, 3 vertici, 4 float (RGBA) = 4*3*2 = 24
            float[] vcs = new float[height * width * 24];

            int index = 0;

            for (int tileY = 0; tileY < height; tileY++)
            {
                for (int tileX = 0; tileX < width; tileX++)
                {
                    index = SetVCAt(vcs, index, layer.Tiles[tileX, tileY].Gid == 0 ? 0 : 1);
                }
            }

            return vcs;
        }

        public void Draw()
        {
            mapMesh.DrawTexture(tileset.Source);
        }

        public void DrawWireframe(Vector4 color)
        {
            mapMesh.DrawWireframe(color);
        }
    }
}
