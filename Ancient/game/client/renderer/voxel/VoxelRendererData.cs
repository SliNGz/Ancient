using ancient.game.renderers.model;
using ancient.game.world.chunk;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ancientlib.game.utils;
using ancient.game.world.block;
using ancient.game.utils;
using ancientlib.game.block;
using ancient.game.client.renderer.model;

namespace ancient.game.renderers.voxel
{
    public class VoxelRendererData
    {
        public VertexBuffer solidVertexBuffer;
        public IndexBuffer solidIndexBuffer;

        public List<VertexPositionColor> solidVertices;
        public List<int> solidIndices;

        public VertexBuffer liquidVertexBuffer;
        public IndexBuffer liquidIndexBuffer;

        public List<VertexPositionColor> liquidVertices;
        public List<int> liquidIndices;

        public VoxelRendererData()
        {
            this.solidVertexBuffer = null;
            this.solidIndexBuffer = null;
            this.solidVertices = new List<VertexPositionColor>();
            this.solidIndices = new List<int>();

            this.liquidVertexBuffer = null;
            this.liquidIndexBuffer = null;
            this.liquidVertices = new List<VertexPositionColor>();
            this.liquidIndices = new List<int>();
        }

        public VoxelRendererData(Chunk chunk) : this()
        {
            LoadData(chunk);
        }

        public VoxelRendererData(Color[,,] voxels) : this()
        {
            for (int x = 0; x < voxels.GetLength(0); x++)
            {
                for (int y = 0; y < voxels.GetLength(1); y++)
                {
                    for (int z = 0; z < voxels.GetLength(2); z++)
                    {
                        if (voxels[x, y, z] != Color.Transparent)
                        {
                            bool down = true;
                            if (y > 0)
                                down = voxels[x, y - 1, z] == Color.Transparent;

                            bool up = true;
                            if (y < voxels.GetLength(1) - 1)
                                up = voxels[x, y + 1, z] == Color.Transparent;

                            bool north = true;
                            if (z > 0)
                                north = voxels[x, y, z - 1] == Color.Transparent;

                            bool south = true;
                            if (z < voxels.GetLength(2) - 1)
                                south = voxels[x, y, z + 1] == Color.Transparent;

                            bool west = true;
                            if (x > 0)
                                west = voxels[x - 1, y, z] == Color.Transparent;

                            bool east = true;
                            if (x < voxels.GetLength(0) - 1)
                                east = voxels[x + 1, y, z] == Color.Transparent;

                            /* y - voxels.GetLength(1) - enitities' position is head position and not foot position.
                             * this subtraction renders them at the proper height. */
                            CreateVoxel(new Vector3(x, y - voxels.GetLength(1), z), voxels[x, y, z], 1, down, up, north, south, west, east);
                        }
                    }
                }
            }

            UpdateVertexBuffer();
        }

        public void LoadData(Chunk chunk)
        {
            Chunk[] neighbors = chunk.GetNeighbors();
            Chunk downChunk = neighbors[0];
            Chunk upChunk = neighbors[1];
            Chunk northChunk = neighbors[2];
            Chunk southChunk = neighbors[3];
            Chunk westChunk = neighbors[4];
            Chunk eastChunk = neighbors[5];

            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        Block block = chunk.GetBlock(x, y, z);

                        bool defaultFlag = false;

                        bool down = defaultFlag;
                        bool up = defaultFlag;
                        bool north = defaultFlag;
                        bool south = defaultFlag;
                        bool west = defaultFlag;
                        bool east = defaultFlag;

                        byte alpha = block.GetColor().A;

                        if (block.IsVisible())
                        {
                            if (y > 0)
                                down = block.ShouldRenderFace(chunk.GetBlock(x, y - 1, z), 0, -1, 0);
                            else
                            {
                                if (downChunk != null)
                                    down = block.ShouldRenderFace(downChunk.GetBlock(x, 15, z), 0, -1, 0);
                            }

                            if (y < 15)
                                up = block.ShouldRenderFace(chunk.GetBlock(x, y + 1, z), 0, 1, 0);
                            else
                            {
                                if (upChunk != null)
                                    up = block.ShouldRenderFace(upChunk.GetBlock(x, 0, z), 0, 1, 0);
                            }

                            if (z > 0)
                                north = block.ShouldRenderFace(chunk.GetBlock(x, y, z - 1), 0, 0, -1);
                            else
                            {
                                if (northChunk != null)
                                    north = block.ShouldRenderFace(northChunk.GetBlock(x, y, 15), 0, 0, -1);
                            }

                            if (z < 15)
                                south = block.ShouldRenderFace(chunk.GetBlock(x, y, z + 1), 0, 0, 1);
                            else
                            {
                                if (southChunk != null)
                                    south = block.ShouldRenderFace(southChunk.GetBlock(x, y, 0), 0, 0, 1);
                            }

                            if (x > 0)
                                west = block.ShouldRenderFace(chunk.GetBlock(x - 1, y, z), -1, 0, 0);
                            else
                            {
                                if (westChunk != null)
                                    west = block.ShouldRenderFace(westChunk.GetBlock(15, y, z), -1, 0, 0);
                            }

                            if (x < 15)
                                east = block.ShouldRenderFace(chunk.GetBlock(x + 1, y, z), 1, 0, 0);
                            else
                            {
                                if (eastChunk != null)
                                    east = block.ShouldRenderFace(eastChunk.GetBlock(0, y, z), 1, 0, 0);
                            }

                            if (block is IBlockModel)
                            {
                                IBlockModel blockModel = (IBlockModel)block;
                                ModelData modelData = ModelDatabase.GetModelFromName(block.GetModelName());
                                LoadBlockModel(modelData.GetVoxels(), new Vector3(x, y, z) + modelData.GetOffset(), modelData.GetScale(), chunk, x, y, z);

                                List<ModelData> attachments = modelData.GetAttachments();
                                for (int i = 0; i < attachments.Count; i++)
                                    LoadBlockModel(attachments[i].GetVoxels(), new Vector3(x, y, z) + attachments[i].GetOffset(), modelData.GetScale(), chunk, x, y, z);
                            }
                            else
                            {
                                Vector3 dimensions = block.GetDimensions();
                                CreateVoxel(new Vector3(x, y, z), BlockColorizer.GetColorOfBlock(block, chunk, x, y, z),
                                    dimensions.X, dimensions.Y, dimensions.Z, down, up, north, south, west, east);
                            }
                        }
                    }
                }
            }

            UpdateVertexBuffer();
        }

        private void LoadBlockModel(Color[,,] voxels, Vector3 position, Vector3 scale, Chunk chunk, int cx, int cy, int cz)
        {
            int light = Utils.GetLightValueAt(chunk.GetWorld(), chunk.GetX() + cx, chunk.GetY() + cy, chunk.GetZ() + cz);

            for (int x = 0; x < voxels.GetLength(0); x++)
            {
                for (int y = 0; y < voxels.GetLength(1); y++)
                {
                    for (int z = 0; z < voxels.GetLength(2); z++)
                    {
                        if (voxels[x, y, z] != Color.Transparent)
                        {
                            bool down = true;
                            if (y > 0)
                                down = voxels[x, y - 1, z] == Color.Transparent;

                            bool up = true;
                            if (y < voxels.GetLength(1) - 1)
                                up = voxels[x, y + 1, z] == Color.Transparent;

                            bool north = true;
                            if (z > 0)
                                north = voxels[x, y, z - 1] == Color.Transparent;

                            bool south = true;
                            if (z < voxels.GetLength(2) - 1)
                                south = voxels[x, y, z + 1] == Color.Transparent;

                            bool west = true;
                            if (x > 0)
                                west = voxels[x - 1, y, z] == Color.Transparent;

                            bool east = true;
                            if (x < voxels.GetLength(0) - 1)
                                east = voxels[x + 1, y, z] == Color.Transparent;

                            CreateVoxel(position + new Vector3(x, y, z) * scale, Utils.GetColorAffectedByLight(voxels[x, y, z], light), scale.X, scale.Y, scale.Z, down, up, north, south, west, east);
                        }
                    }
                }
            }
        }

        public void Setup()
        {
            this.solidVertexBuffer = null;
            this.solidIndexBuffer = null;
            this.solidVertices.Clear();
            this.solidIndices.Clear();

            this.liquidVertexBuffer = null;
            this.liquidIndexBuffer = null;
            this.liquidVertices.Clear();
            this.liquidIndices.Clear();
        }

        public void Draw()
        {
            DrawSolid();
            DrawLiquid();
        }

        public void DrawSolid()
        {
            if (solidVertexBuffer == null && solidIndexBuffer == null)
                return;

            Ancient.ancient.device.SetVertexBuffer(solidVertexBuffer);
            Ancient.ancient.device.Indices = solidIndexBuffer;

            Ancient.ancient.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, solidIndexBuffer.IndexCount / 3);
        }

        public void DrawLiquid()
        {
            if (liquidVertexBuffer == null && liquidIndexBuffer == null)
                return;

            Ancient.ancient.device.SetVertexBuffer(liquidVertexBuffer);
            Ancient.ancient.device.Indices = liquidIndexBuffer;

            Ancient.ancient.device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, liquidIndexBuffer.IndexCount / 3);
        }

        public void UpdateVertexBuffer()
        {
            if (solidVertices.Count > 0)
            {
                solidVertexBuffer = new VertexBuffer(Ancient.ancient.device, typeof(VertexPositionColor), solidVertices.Count, BufferUsage.WriteOnly);
                solidVertexBuffer.SetData(solidVertices.ToArray());

                solidIndexBuffer = new IndexBuffer(Ancient.ancient.device, IndexElementSize.ThirtyTwoBits, solidIndices.Count, BufferUsage.WriteOnly);
                solidIndexBuffer.SetData(solidIndices.ToArray());

                solidVertices.Clear();
                solidIndices.Clear();
            }
            else
            {
                this.solidVertexBuffer = null;
                this.solidIndexBuffer = null;
            }

            if (liquidVertices.Count > 0)
            {
                liquidVertexBuffer = new VertexBuffer(Ancient.ancient.device, typeof(VertexPositionColor), liquidVertices.Count, BufferUsage.WriteOnly);
                liquidVertexBuffer.SetData(liquidVertices.ToArray());

                liquidIndexBuffer = new IndexBuffer(Ancient.ancient.device, IndexElementSize.ThirtyTwoBits, liquidIndices.Count, BufferUsage.WriteOnly);
                liquidIndexBuffer.SetData(liquidIndices.ToArray());

                liquidVertices.Clear();
                liquidIndices.Clear();
            }
            else
            {
                this.liquidVertexBuffer = null;
                this.liquidIndexBuffer = null;
            }
        }

        private void CreateVoxel(Vector3 position, Color color, float size, bool down, bool up, bool north, bool south, bool west, bool east)
        {
            CreateVoxel(position, color, size, size, size, down, up, north, south, west, east);
        }

        private void CreateVoxel(Vector3 position, Color color, float sizeX, float sizeY, float sizeZ, bool down, bool up, bool north, bool south, bool west, bool east)
        {
            Color downColor = color * 0.6F;
            downColor.A = color.A;

            Color zColor = color * 0.7F;
            zColor.A = color.A;

            Color xColor = color * 0.8F;
            xColor.A = color.A;

            VertexPositionColor d1 = new VertexPositionColor(position, downColor);
            VertexPositionColor d2 = new VertexPositionColor(new Vector3(position.X, position.Y, position.Z + sizeZ), downColor);
            VertexPositionColor d3 = new VertexPositionColor(new Vector3(position.X + sizeX, position.Y, position.Z), downColor);
            VertexPositionColor d4 = new VertexPositionColor(new Vector3(position.X + sizeX, position.Y, position.Z + sizeZ), downColor);

            VertexPositionColor u1 = new VertexPositionColor(new Vector3(position.X, position.Y + sizeY, position.Z), color);
            VertexPositionColor u2 = new VertexPositionColor(new Vector3(position.X, position.Y + sizeY, position.Z + sizeZ), color);
            VertexPositionColor u3 = new VertexPositionColor(new Vector3(position.X + sizeX, position.Y + sizeY, position.Z), color);
            VertexPositionColor u4 = new VertexPositionColor(new Vector3(position.X + sizeX, position.Y + sizeY, position.Z + sizeZ), color);

            VertexPositionColor n1 = new VertexPositionColor(d1.Position, zColor);
            VertexPositionColor n2 = new VertexPositionColor(u1.Position, zColor);
            VertexPositionColor n3 = new VertexPositionColor(d3.Position, zColor);
            VertexPositionColor n4 = new VertexPositionColor(u3.Position, zColor);

            VertexPositionColor s1 = new VertexPositionColor(d2.Position, zColor);
            VertexPositionColor s2 = new VertexPositionColor(d4.Position, zColor);
            VertexPositionColor s3 = new VertexPositionColor(u2.Position, zColor);
            VertexPositionColor s4 = new VertexPositionColor(u4.Position, zColor);

            VertexPositionColor w1 = new VertexPositionColor(d1.Position, xColor);
            VertexPositionColor w2 = new VertexPositionColor(d2.Position, xColor);
            VertexPositionColor w3 = new VertexPositionColor(u1.Position, xColor);
            VertexPositionColor w4 = new VertexPositionColor(u2.Position, xColor);

            VertexPositionColor e1 = new VertexPositionColor(d3.Position, xColor);
            VertexPositionColor e2 = new VertexPositionColor(u3.Position, xColor);
            VertexPositionColor e3 = new VertexPositionColor(d4.Position, xColor);
            VertexPositionColor e4 = new VertexPositionColor(u4.Position, xColor);

            if (color.A == 255)
            {
                if (down)
                {
                    AddSolidIndicesToList();
                    this.solidVertices.AddRange(new VertexPositionColor[] { d1, d3, d2, d4 });
                }

                if (up)
                {
                    AddSolidIndicesToList();
                    this.solidVertices.AddRange(new VertexPositionColor[] { u1, u2, u3, u4 });
                }

                if (north)
                {
                    AddSolidIndicesToList();
                    this.solidVertices.AddRange(new VertexPositionColor[] { n1, n2, n3, n4 });
                }

                if (south)
                {
                    AddSolidIndicesToList();
                    this.solidVertices.AddRange(new VertexPositionColor[] { s1, s2, s3, s4 });
                }

                if (west)
                {
                    AddSolidIndicesToList();
                    this.solidVertices.AddRange(new VertexPositionColor[] { w1, w2, w3, w4 });
                }

                if (east)
                {
                    AddSolidIndicesToList();
                    this.solidVertices.AddRange(new VertexPositionColor[] { e1, e2, e3, e4 });
                }
            }
            else
            {
                if (down)
                {
                    AddLiquidIndicesToList();
                    this.liquidVertices.AddRange(new VertexPositionColor[] { d1, d3, d2, d4 });
                }

                if (up)
                {
                    AddLiquidIndicesToList();
                    this.liquidVertices.AddRange(new VertexPositionColor[] { u1, u2, u3, u4 });
                }

                if (north)
                {
                    AddLiquidIndicesToList();
                    this.liquidVertices.AddRange(new VertexPositionColor[] { n1, n2, n3, n4 });
                }

                if (south)
                {
                    AddLiquidIndicesToList();
                    this.liquidVertices.AddRange(new VertexPositionColor[] { s1, s2, s3, s4 });
                }

                if (west)
                {
                    AddLiquidIndicesToList();
                    this.liquidVertices.AddRange(new VertexPositionColor[] { w1, w2, w3, w4 });
                }

                if (east)
                {
                    AddLiquidIndicesToList();
                    this.liquidVertices.AddRange(new VertexPositionColor[] { e1, e2, e3, e4 });
                }
            }
        }

        private void AddSolidIndicesToList()
        {
            this.solidIndices.AddRange(new int[] {
                this.solidVertices.Count,
                this.solidVertices.Count + 1,
                this.solidVertices.Count + 2,
                this.solidVertices.Count + 1,
                this.solidVertices.Count + 3,
                this.solidVertices.Count + 2 });
        }

        private void AddLiquidIndicesToList()
        {
            this.liquidIndices.AddRange(new int[] {
                this.liquidVertices.Count,
                this.liquidVertices.Count + 1,
                this.liquidVertices.Count + 2,
                this.liquidVertices.Count + 1,
                this.liquidVertices.Count + 3,
                this.liquidVertices.Count + 2 });
        }
    }
}
