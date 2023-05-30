using Aiv.Fast2D.Component;
using Aiv.Fast2D.Component.UI;
using Aiv.Tiled;
using MisteryDungeon.MysteryDungeon;
using OpenTK;
using System;

namespace MisteryDungeon {
    internal class Room_0: Scene {
        protected override void LoadAssets() {
            FontMgr.AddFont("std_font", "Assets/text_sheet.png", 15, 32, 20, 20);
            GfxMgr.AddTexture("red_button", "Assets/red_button.png");
            GfxMgr.AddTexture("crate", "Assets/crate.png");
            GfxMgr.AddTexture("lamp_gate", "Assets/lamp_gate.png");
            GfxMgr.AddTexture("default_bullet", "Assets/default_bullet.png");
            GfxMgr.AddTexture("player", "Assets/Spritesheets/player.png");
        }

        public override void InitializeScene() {
            base.InitializeScene();

            Map map = new Map("Assets/Tiled/Room0.tmx");
            float tileUnitWidth = (float)Game.Win.OrthoWidth / map.Width;
            float tileUnitHeight = (float)Game.Win.OrthoHeight / map.Height;

            CreateMap(tileUnitWidth, tileUnitHeight, map);
            CreatePlayer(tileUnitWidth, tileUnitHeight);
        }

        private void CreateMap(float tileUnitWidth, float tileUnitHeight, Map map) {
            
            //Layers
            foreach (Layer layer in map.Layers) {
                if (layer.Name == "Background") {
                    CreateBackground(layer, map, map.TileWidth,
                        tileUnitWidth, map.TileHeight, tileUnitHeight);
                } else if (layer.Name == "Collision") {
                    //TODO: pathfinding per le collisioni
                }
            }
            //Objects
            foreach (ObjectGroup objectGroup in map.ObjectGroups) {
                for (int i = 0; i < objectGroup.Objects.Count; i++) {
                    switch (objectGroup.Objects[i].Name) {
                        case "obstacle":
                            CreateObstacle(objectGroup.Objects[i], i, map.TileWidth, tileUnitWidth, tileUnitHeight);
                            break;
                        case "button":
                            CreateButton(objectGroup.Objects[i], i, map.TileWidth, tileUnitWidth, tileUnitHeight);
                            break;
                        case "gate":
                            CreateLampGate(objectGroup.Objects[i], i, map.TileWidth, tileUnitWidth, tileUnitHeight);
                            break;
                        case "door":
                            CreateDoor(objectGroup.Objects[i], i, map.TileWidth, tileUnitWidth, tileUnitHeight);
                            break;
                    }
                }
            }
        }

        public void CreateBackground(Layer layer, Map map, float tileWidth,
            float tileUnitWidth, float tileHeight, float tileUnitHeight) {
            for (int i = 0; i < layer.Tiles.GetLength(0); i++) {
                for (int j = 0; j < layer.Tiles.GetLength(1); j++) {
                    CreateTile(new TileSprite(map.Tilesets, layer.Tiles[i, j]),
                        i, j, tileWidth, tileUnitWidth, tileHeight, tileUnitHeight);
                }
            }
        }

        public void CreateTile(TileSprite tileSprite, int xIndex, int yIndex,
            float tileWidth, float tileUnitWidth, float tileHeight, float tileUnitHeight) {
            Vector2 pos = new Vector2(tileUnitWidth * xIndex, tileUnitHeight * yIndex);
            GameObject go = new GameObject("Background_Tile_" + xIndex + "_" + yIndex, pos);
            go.AddComponent(SpriteRenderer.Factory(go, tileSprite.Texture, Vector2.Zero, DrawLayer.Background, tileWidth, tileHeight));
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            sr.TextureOffset = new Vector2(tileSprite.OffsetX, tileSprite.OffsetY);
            go.transform.Scale = new Vector2(tileUnitWidth / sr.Width, tileUnitHeight / sr.Height);
            Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }

        public void CreateObstacle(Aiv.Tiled.Object obj, int index, float tileWidth, float tileUnitWidth, float tileUnitHeight) {
            Vector2 pos = new Vector2(((float)obj.X / tileWidth) * tileUnitWidth, ((float)obj.Y / tileWidth) * tileUnitHeight);
            GameObject go = new GameObject("Object_Obstacle_" + index, pos);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "crate", Vector2.UnitY, DrawLayer.Background);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(tileUnitWidth / sr.Width, tileUnitHeight / sr.Height);
            Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }
        
        public void CreateButton(Aiv.Tiled.Object obj, int index, float tileWidth, float tileUnitWidth, float tileUnitHeight) {
            Vector2 pos = new Vector2(((float)obj.X / tileWidth) * tileUnitWidth,((float)obj.Y / tileWidth) * tileUnitHeight);
            GameObject go = new GameObject("Object_Red_Button_" + index, pos);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "red_button", Vector2.UnitY, DrawLayer.Background);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(tileUnitWidth / sr.Width, tileUnitHeight / sr.Height);
            int ID = 0;
            foreach (var property in obj.Properties) {
                if (property.Name != "sequenceId") ID = int.Parse(property.Value);
            }
            go.AddComponent<PlatformButton>(ID);
            Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }

        public void CreateLampGate(Aiv.Tiled.Object obj, int index, float tileWidth, float tileUnitWidth, float tileUnitHeight) {
            Vector2 pos = new Vector2(((float)obj.X / tileWidth) * tileUnitWidth, ((float)obj.Y / tileWidth) * tileUnitHeight);
            GameObject go = new GameObject("Object_Lamp_Gate_" + index, pos);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "lamp_gate", Vector2.UnitY, DrawLayer.Background);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(tileUnitWidth / sr.Width, tileUnitHeight / sr.Height);
            int ID = 0;
            foreach(var property in obj.Properties) {
                if (property.Name != "gateId") ID = int.Parse(property.Value);
            }
            go.AddComponent<LampGate>(ID);
            Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }

        public void CreateDoor(Aiv.Tiled.Object obj, int index, float tileWidth, float tileUnitWidth, float tileUnitHeight) {
            Vector2 pos = new Vector2(((float)obj.X / tileWidth) * tileUnitWidth, ((float)obj.Y / tileWidth) * tileUnitHeight);
            GameObject go = new GameObject("Object_Lamp_Gate_" + index, pos);
            int ID = 0;
            foreach (var property in obj.Properties) {
                if (property.Name != "doorId") ID = int.Parse(property.Value);
            }
            go.AddComponent<Door>(ID);
            Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }

        public void CreatePlayer(float tileUnitWidth, float tileUnitHeight) {
            Vector2 pos = new Vector2(8 * tileUnitWidth, 3 * tileUnitHeight);
            GameObject go = new GameObject("Player", pos);
            Sheet sheet = new Sheet(GfxMgr.GetTexture("player"), 6, 4);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "player", Vector2.Zero,
                DrawLayer.Playground, sheet.FrameWidth, sheet.FrameHeight);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(tileUnitWidth / sr.Width, tileUnitHeight / sr.Height);
            CreateAnimations(go, sheet);
            go.AddComponent<PlayerController>();
            Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }

        private void CreateAnimations(GameObject go, Sheet sheet) {
            SheetClip idleDown = new SheetClip(
                sheet, "idleDown", new int[] { 0 }, false, 1
            );
            SheetClip idleRight = new SheetClip(
                sheet, "idleRight", new int[] { 1 }, false, 1
            );
            SheetClip idleLeft = new SheetClip(
                sheet, "idleLeft", new int[] { 2 }, false, 1
            );
            SheetClip idleUp = new SheetClip(
                sheet, "idleUp", new int[] { 3 }, false, 1
            );
            SheetClip death = new SheetClip(
                sheet, "death", new int[] { 4, 5 }, false, 1
            );
            SheetClip walkingDown = new SheetClip(
                sheet, "walkingDown", new int[] { 6, 7, 8, 9 }, true, 10
            );
            SheetClip walkingRight = new SheetClip(
                sheet, "walkingRight", new int[] { 10, 11, 12, 13 }, true, 10
            );
            SheetClip walkingLeft = new SheetClip(
                sheet, "walkingLeft", new int[] { 14, 15, 16, 17 }, true, 10
            );
            SheetClip walkingUp = new SheetClip(
                sheet, "walkingUp", new int[] { 18, 19, 20, 21 }, true, 10
            );
            SheetAnimator animator = go.AddComponent<SheetAnimator>(go.GetComponent<SpriteRenderer>());
            animator.AddClip(idleDown);
            animator.AddClip(idleUp);
            animator.AddClip(idleRight);
            animator.AddClip(idleLeft);
            animator.AddClip(death);
            animator.AddClip(walkingUp);
            animator.AddClip(walkingRight);
            animator.AddClip(walkingLeft);
            animator.AddClip(walkingDown);
        }
    }
}
