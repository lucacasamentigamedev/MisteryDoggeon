using Aiv.Fast2D.Component;
using Aiv.Tiled;
using MisteryDungeon.AivAlgo.Pathfinding;
using OpenTK;
using System;
using System.Collections.Generic;

namespace MisteryDungeon.MysteryDungeon {

    public class TiledMapCreator {

        private Map map;
        private MovementGrid grid;
        private float tileUnitWidth;
        private float tileUnitHeight;
        private float tileWidth;

        public TiledMapCreator(int roomId) {
            map = new Map("Assets/Tiled/Room"+ roomId + ".tmx");
            tileUnitWidth = (float)Game.Win.OrthoWidth / map.Width;
            tileUnitHeight = (float)Game.Win.OrthoHeight / map.Height;
            tileWidth = map.TileWidth;
        }

        public void CreateMap() {
            //Background & Collisions layers
            foreach (Layer layer in map.Layers) {
                if (layer.Name == "Background") {
                    CreateBackground(layer);
                } else if (layer.Name == "Collisions") {
                    CreatePathfindingMap(layer);
                }
            }
            //Objects layers
            foreach (ObjectGroup objectGroup in map.ObjectGroups) {
                for (int i = 0; i < objectGroup.Objects.Count; i++) {
                    switch (objectGroup.Objects[i].Name) {
                        case "obstacle":
                            CreateObstacle(objectGroup.Objects[i], i);
                            break;
                        case "button":
                            CreateButton(objectGroup.Objects[i], i);
                            break;
                        case "gate":
                            CreateLampGate(objectGroup.Objects[i], i);
                            break;
                        case "door":
                            CreateDoor(objectGroup.Objects[i], i);
                            break;
                        case "player":
                            bool defaultPos = bool.Parse(getPropertyValueByName("startPosition", objectGroup.Objects[i].Properties));
                            if(defaultPos && !GameConfig.FirstDoorPassed) {
                                //disegno in posizione di default
                                CreatePlayer(objectGroup.Objects[i]);
                            } else if (!defaultPos && GameConfig.FirstDoorPassed) {
                                //disegno affianco alla porta
                                CreatePlayer(objectGroup.Objects[i]);
                            }
                            break;
                    }
                }
            }
        }

        private void CreatePathfindingMap(Layer layer) {
            grid = new MovementGrid(map.Width, map.Height, layer);
            PrintMovementGrid();
        }

        private void CreateBackground(Layer layer) {
            for (int i = 0; i < layer.Tiles.GetLength(0); i++) {
                for (int j = 0; j < layer.Tiles.GetLength(1); j++) {
                    CreateTile(new TileSprite(map.Tilesets, layer.Tiles[i, j]), i, j);
                }
            }
        }

        private void CreateTile(TileSprite tileSprite, int xIndex, int yIndex) {
            Vector2 pos = new Vector2(
                tileUnitWidth * xIndex + (tileUnitWidth / 2),
                tileUnitHeight * yIndex + (tileUnitHeight / 2)
            );
            GameObject go = new GameObject("Background_Tile_" + xIndex + "_" + yIndex, pos);
            go.AddComponent(SpriteRenderer.Factory(go, tileSprite.Texture, Vector2.One * 0.5f, DrawLayer.Background, tileWidth, tileWidth));
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            sr.TextureOffset = new Vector2(tileSprite.OffsetX, tileSprite.OffsetY);
            go.transform.Scale = new Vector2(tileUnitWidth / sr.Width, tileUnitHeight / sr.Height);
            Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }

        private void CreateObstacle(Aiv.Tiled.Object obj, int index) {
            Vector2 pos = new Vector2(
                ((float)obj.X / tileWidth) * tileUnitWidth + (tileUnitWidth / 2),
                ((float)obj.Y / tileWidth) * tileUnitHeight - (tileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_Obstacle_" + index, pos);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "crate", Vector2.One * 0.5f, DrawLayer.Background);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(tileUnitWidth / sr.Width, tileUnitHeight / sr.Height);
            Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }

        private void CreateButton(Aiv.Tiled.Object obj, int index) {
            Vector2 pos = new Vector2(
                ((float)obj.X / tileWidth) * tileUnitWidth + (tileUnitWidth / 2),
                ((float)obj.Y / tileWidth) * tileUnitHeight - (tileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_Red_Button_" + index, pos);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "red_button", Vector2.One * 0.5f, DrawLayer.Background);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(tileUnitWidth / sr.Width, tileUnitHeight / sr.Height);
            string res = getPropertyValueByName("sequenceId", obj.Properties);
            int ID = int.Parse(res != null ? res : "0");
            go.AddComponent<PlatformButton>(ID);
            Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }

        private void CreateLampGate(Aiv.Tiled.Object obj, int index) {
            Vector2 pos = new Vector2(
                ((float)obj.X / tileWidth) * tileUnitWidth + (tileUnitWidth / 2),
                ((float)obj.Y / tileWidth) * tileUnitHeight - (tileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_Lamp_Gate_" + index, pos);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "lamp_gate", Vector2.One * 0.5f, DrawLayer.Background);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(tileUnitWidth / sr.Width, tileUnitHeight / sr.Height);
            string res = getPropertyValueByName("gateId", obj.Properties);
            int ID = int.Parse(res != null ? res : "0");
            go.AddComponent<LampGate>(ID);
            Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }

        private void CreateDoor(Aiv.Tiled.Object obj, int index) {
            Vector2 pos = new Vector2(
                ((float)obj.X / tileWidth) * tileUnitWidth + (tileUnitWidth / 2),
                ((float)obj.Y / tileWidth) * tileUnitHeight - (tileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_Door_" + index, pos);
            go.Tag = (int)GameObjectTag.Door;
            string res = getPropertyValueByName("doorId", obj.Properties);
            int ID = int.Parse(res != null ? res : "0");
            go.AddComponent<Door>(ID);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "door", Vector2.One * 0.5f, DrawLayer.Background);
            go.AddComponent(sr);
            sr.Sprite.SetMultiplyTint(0, 0, 0, 0.01f);
            go.transform.Scale = new Vector2((tileUnitWidth / sr.Width), (tileUnitHeight / sr.Height));
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Door;
            go.AddComponent(ColliderFactory.CreateUnscaledBoxFor(go));
            Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }

        private void CreatePlayer(Aiv.Tiled.Object obj) {
            Vector2 cellIndex = new Vector2(
                (float)obj.X / map.TileWidth,
                ((float)obj.Y / map.TileWidth) - 1
            );
            Vector2 pos = new Vector2(
                cellIndex.X * tileUnitWidth + (tileUnitWidth / 2),
                cellIndex.Y * tileUnitHeight + (tileUnitHeight / 2)
            );
            GameObject go = new GameObject("Player", pos);
            go.Tag = (int)GameObjectTag.Player;
            Sheet sheet = new Sheet(GfxMgr.GetTexture("player"), 6, 4);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "player", Vector2.One * 0.5f,
                DrawLayer.Playground, sheet.FrameWidth, sheet.FrameHeight);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(tileUnitWidth / sr.Width, tileUnitHeight / sr.Height);
            go.AddComponent<PlayerController>(grid, 5f, tileUnitWidth, tileUnitHeight, map.Height, map.Width);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Player;
            rb.AddCollisionType((uint)RigidbodyType.Door);
            go.AddComponent(ColliderFactory.CreateUnscaledBoxFor(go));
            CreatePlayerAnimations(go, sheet);
            Console.WriteLine("Creato " + go.Name + " in cella " + cellIndex.ToString());
        }

        private void CreatePlayerAnimations(GameObject go, Sheet sheet) {
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

        private void PrintMovementGrid() {
            Console.WriteLine("Mappa pathfinding");
            Console.WriteLine();
            for (int x = 0; x < grid.Map.GetLength(0); ++x) {
                for (int y = 0; y < grid.Map.GetLength(1); ++y) {
                    Console.Write((int)grid.Map[y, x] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private string getPropertyValueByName(string name, List<Property> properties) {
            foreach (Property property in properties) {
                if (property.Name != name) continue;
                return property.Value;
            }
            return null;
        }
    }
}
