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
using ancient.game.world.generator.noise;
using System;
using ancient.game.renderers.world;

namespace ancient.game.renderers.voxel
{
    public class VoxelRendererData
    {
        public static SimplexNoise noise = new SimplexNoise();
        public static Random rand = new Random(15050);

        public VertexBuffer solidVertexBuffer;
        public IndexBuffer solidIndexBuffer;

        public List<VertexPositionColorNormal> solidVertices;
        public List<int> solidIndices;

        public VertexBuffer liquidVertexBuffer;
        public IndexBuffer liquidIndexBuffer;

        public List<VertexPositionColorNormal> liquidVertices;
        public List<int> liquidIndices;

        public VoxelRendererData()
        {
            this.solidVertexBuffer = null;
            this.solidIndexBuffer = null;
            this.solidVertices = new List<VertexPositionColorNormal>();
            this.solidIndices = new List<int>();

            this.liquidVertexBuffer = null;
            this.liquidIndexBuffer = null;
            this.liquidVertices = new List<VertexPositionColorNormal>();
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

                            /* y - voxels.GetLength(1) - entities' position is head position and not foot position.
                             * this subtraction renders them at the proper height. */
                            CreateVoxel(new Vector3(x, y - voxels.GetLength(1), z), voxels[x, y, z], 1, down, up, north, south, west, east, Vector4.Zero);
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

                        int x1 = chunk.GetX() + x;
                        int y1 = chunk.GetY() + y;
                        int z1 = chunk.GetZ() + z;

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
                                down = block.ShouldRenderFace(chunk.GetWorld(), x1, y1, z1, chunk.GetBlock(x, y - 1, z), 0, -1, 0);
                            else
                            {
                                if (downChunk != null)
                                    down = block.ShouldRenderFace(chunk.GetWorld(), x1, y1, z1, downChunk.GetBlock(x, 15, z), 0, -1, 0);
                            }

                            if (y < 15)
                                up = block.ShouldRenderFace(chunk.GetWorld(), x1, y1, z1, chunk.GetBlock(x, y + 1, z), 0, 1, 0);
                            else
                            {
                                if (upChunk != null)
                                    up = block.ShouldRenderFace(chunk.GetWorld(), x1, y1, z1, upChunk.GetBlock(x, 0, z), 0, 1, 0);
                            }

                            if (z > 0)
                                north = block.ShouldRenderFace(chunk.GetWorld(), x1, y1, z1, chunk.GetBlock(x, y, z - 1), 0, 0, -1);
                            else
                            {
                                if (northChunk != null)
                                    north = block.ShouldRenderFace(chunk.GetWorld(), x1, y1, z1, northChunk.GetBlock(x, y, 15), 0, 0, -1);
                            }

                            if (z < 15)
                                south = block.ShouldRenderFace(chunk.GetWorld(), x1, y1, z1, chunk.GetBlock(x, y, z + 1), 0, 0, 1);
                            else
                            {
                                if (southChunk != null)
                                    south = block.ShouldRenderFace(chunk.GetWorld(), x1, y1, z1, southChunk.GetBlock(x, y, 0), 0, 0, 1);
                            }

                            if (x > 0)
                                west = block.ShouldRenderFace(chunk.GetWorld(), x1, y1, z1, chunk.GetBlock(x - 1, y, z), -1, 0, 0);
                            else
                            {
                                if (westChunk != null)
                                    west = block.ShouldRenderFace(chunk.GetWorld(), x1, y1, z1, westChunk.GetBlock(15, y, z), -1, 0, 0);
                            }

                            if (x < 15)
                                east = block.ShouldRenderFace(chunk.GetWorld(), x1, y1, z1, chunk.GetBlock(x + 1, y, z), 1, 0, 0);
                            else
                            {
                                if (eastChunk != null)
                                    east = block.ShouldRenderFace(chunk.GetWorld(), x1, y1, z1, eastChunk.GetBlock(0, y, z), 1, 0, 0);
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
                                Vector3 dimensions = block.GetRenderDimensions(chunk.GetWorld(), x1, y1, z1);

                                CreateVoxel(new Vector3(x, y, z), BlockColorizer.GetColorOfBlock(block, chunk, x, y, z),
                                    dimensions.X, dimensions.Y, dimensions.Z, down, up, north, south, west, east,
                                    block.GetShaderTechnique(chunk.GetWorld(), x1, y1, z1));
                            }
                        }
                    }
                }
            }

            UpdateVertexBuffer();
        }

        private void LoadBlockModel(Color[,,] voxels, Vector3 position, Vector3 scale, Chunk chunk, int cx, int cy, int cz)
        {
            Block block = chunk.GetBlock(cx, cy, cz);

            int x1 = chunk.GetX() + cx;
            int y1 = chunk.GetY() + cy;
            int z1 = chunk.GetZ() + cz;

            int light = Utils.GetLightValueAt(chunk.GetWorld(), x1, y1, z1);

            Vector4 technique = block.GetShaderTechnique(chunk.GetWorld(), x1, y1, z1);

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

                            //Utils.GetColorAffectedByLight(voxels[x, y, z], light)
                            if (technique.W == 2)
                                CreateVoxelWindAffected(position + new Vector3(x, y, z) * scale, position + new Vector3(x, 0, z) * scale, voxels[x, y, z],
                                    scale.X, scale.Y, scale.Z, down, up, north, south, west, east);
                            else
                                CreateVoxel(position + new Vector3(x, y, z) * scale, voxels[x, y, z],
                                    scale.X, scale.Y, scale.Z, down, up, north, south, west, east, technique);
                        }
                    }
                }
            }
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
                solidVertexBuffer = new VertexBuffer(Ancient.ancient.device, typeof(VertexPositionColorNormal), solidVertices.Count, BufferUsage.WriteOnly);
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
                liquidVertexBuffer = new VertexBuffer(Ancient.ancient.device, typeof(VertexPositionColorNormal), liquidVertices.Count, BufferUsage.WriteOnly);
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

        private void CreateVoxel(Vector3 position, Color color, float size, bool down, bool up, bool north, bool south, bool west, bool east, Vector4 tech)
        {
            CreateVoxel(position, color, size, size, size, down, up, north, south, west, east, tech);
        }

        private void CreateVoxel(Vector3 position, Color color, float sizeX, float sizeY, float sizeZ, bool down, bool up, bool north, bool south, bool west, bool east, Vector4 tech)
        {
            Color downColor = color * 0.7F;
            downColor.A = color.A;

            Color zColor = color * 0.8F;
            zColor.A = color.A;

            Color xColor = color * 0.9F;
            xColor.A = color.A;

            VertexPositionColorNormal d1 = new VertexPositionColorNormal(position, downColor, Vector3.Down, tech);
            VertexPositionColorNormal d2 = new VertexPositionColorNormal(position + new Vector3(0, 0, sizeZ), downColor, Vector3.Down, tech);
            VertexPositionColorNormal d3 = new VertexPositionColorNormal(position + new Vector3(sizeX, 0, 0), downColor, Vector3.Down, tech);
            VertexPositionColorNormal d4 = new VertexPositionColorNormal(position + new Vector3(sizeX, 0, sizeZ), downColor, Vector3.Down, tech);

            VertexPositionColorNormal u1 = new VertexPositionColorNormal(d1.Position + Vector3.Up * sizeY, color, Vector3.Up, tech);
            VertexPositionColorNormal u2 = new VertexPositionColorNormal(d2.Position + Vector3.Up * sizeY, color, Vector3.Up, tech);
            VertexPositionColorNormal u3 = new VertexPositionColorNormal(d3.Position + Vector3.Up * sizeY, color, Vector3.Up, tech);
            VertexPositionColorNormal u4 = new VertexPositionColorNormal(d4.Position + Vector3.Up * sizeY, color, Vector3.Up, tech);

            VertexPositionColorNormal n1 = new VertexPositionColorNormal(d1.Position, zColor, Vector3.Forward, tech);
            VertexPositionColorNormal n2 = new VertexPositionColorNormal(u1.Position, zColor, Vector3.Forward, tech);
            VertexPositionColorNormal n3 = new VertexPositionColorNormal(d3.Position, zColor, Vector3.Forward, tech);
            VertexPositionColorNormal n4 = new VertexPositionColorNormal(u3.Position, zColor, Vector3.Forward, tech);

            VertexPositionColorNormal s1 = new VertexPositionColorNormal(d2.Position, zColor, Vector3.Backward, tech);
            VertexPositionColorNormal s2 = new VertexPositionColorNormal(d4.Position, zColor, Vector3.Backward, tech);
            VertexPositionColorNormal s3 = new VertexPositionColorNormal(u2.Position, zColor, Vector3.Backward, tech);
            VertexPositionColorNormal s4 = new VertexPositionColorNormal(u4.Position, zColor, Vector3.Backward, tech);

            VertexPositionColorNormal w1 = new VertexPositionColorNormal(d1.Position, xColor, Vector3.Left, tech);
            VertexPositionColorNormal w2 = new VertexPositionColorNormal(d2.Position, xColor, Vector3.Left, tech);
            VertexPositionColorNormal w3 = new VertexPositionColorNormal(u1.Position, xColor, Vector3.Left, tech);
            VertexPositionColorNormal w4 = new VertexPositionColorNormal(u2.Position, xColor, Vector3.Left, tech);

            VertexPositionColorNormal e1 = new VertexPositionColorNormal(d3.Position, xColor, Vector3.Right, tech);
            VertexPositionColorNormal e2 = new VertexPositionColorNormal(u3.Position, xColor, Vector3.Right, tech);
            VertexPositionColorNormal e3 = new VertexPositionColorNormal(d4.Position, xColor, Vector3.Right, tech);
            VertexPositionColorNormal e4 = new VertexPositionColorNormal(u4.Position, xColor, Vector3.Right, tech);

            if (color.A == 255)
            {
                if (down)
                {
                    AddIndicesToList(solidIndices, solidVertices);
                    this.solidVertices.AddRange(new VertexPositionColorNormal[] { d1, d3, d2, d4 });
                }

                if (up)
                {
                    AddIndicesToList(solidIndices, solidVertices);
                    this.solidVertices.AddRange(new VertexPositionColorNormal[] { u1, u2, u3, u4 });
                }

                if (north)
                {
                    AddIndicesToList(solidIndices, solidVertices);
                    this.solidVertices.AddRange(new VertexPositionColorNormal[] { n1, n2, n3, n4 });
                }

                if (south)
                {
                    AddIndicesToList(solidIndices, solidVertices);
                    this.solidVertices.AddRange(new VertexPositionColorNormal[] { s1, s2, s3, s4 });
                }

                if (west)
                {
                    AddIndicesToList(solidIndices, solidVertices);
                    this.solidVertices.AddRange(new VertexPositionColorNormal[] { w1, w2, w3, w4 });
                }

                if (east)
                {
                    AddIndicesToList(solidIndices, solidVertices);
                    this.solidVertices.AddRange(new VertexPositionColorNormal[] { e1, e2, e3, e4 });
                }
            }
            else
            {
                if (down)
                {
                    AddIndicesToList(liquidIndices, liquidVertices);
                    this.liquidVertices.AddRange(new VertexPositionColorNormal[] { d1, d3, d2, d4 });
                }

                if (up)
                {
                    AddIndicesToList(liquidIndices, liquidVertices);
                    this.liquidVertices.AddRange(new VertexPositionColorNormal[] { u1, u2, u3, u4 });
                }

                if (north)
                {
                    AddIndicesToList(liquidIndices, liquidVertices);
                    this.liquidVertices.AddRange(new VertexPositionColorNormal[] { n1, n2, n3, n4 });
                }

                if (south)
                {
                    AddIndicesToList(liquidIndices, liquidVertices);
                    this.liquidVertices.AddRange(new VertexPositionColorNormal[] { s1, s2, s3, s4 });
                }

                if (west)
                {
                    AddIndicesToList(liquidIndices, liquidVertices);
                    this.liquidVertices.AddRange(new VertexPositionColorNormal[] { w1, w2, w3, w4 });
                }

                if (east)
                {
                    AddIndicesToList(liquidIndices, liquidVertices);
                    this.liquidVertices.AddRange(new VertexPositionColorNormal[] { e1, e2, e3, e4 });
                }
            }
        }

        private void CreateVoxelWindAffected(Vector3 position, Vector3 basePosition, Color color, float sizeX, float sizeY, float sizeZ, bool down, bool up, bool north, bool south, bool west, bool east)
        {
            Color downColor = color * 0.7F;
            downColor.A = color.A;

            Color zColor = color * 0.8F;
            zColor.A = color.A;

            Color xColor = color * 0.9F;
            xColor.A = color.A;

            Vector4 t1 = new Vector4(basePosition, 2);
            Vector4 t2 = new Vector4(basePosition + new Vector3(0, 0, sizeZ), 2);
            Vector4 t3 = new Vector4(basePosition + new Vector3(sizeX, 0, sizeZ), 2);
            Vector4 t4 = new Vector4(basePosition + new Vector3(sizeX, 0, 0), 2);

            VertexPositionColorNormal d1 = new VertexPositionColorNormal(position, downColor, Vector3.Down, t1);
            VertexPositionColorNormal d2 = new VertexPositionColorNormal(position + new Vector3(0, 0, sizeZ), downColor, Vector3.Down, t2);
            VertexPositionColorNormal d3 = new VertexPositionColorNormal(position + new Vector3(sizeX, 0, 0), downColor, Vector3.Down, t3);
            VertexPositionColorNormal d4 = new VertexPositionColorNormal(position + new Vector3(sizeX, 0, sizeZ), downColor, Vector3.Down, t4);

            VertexPositionColorNormal u1 = new VertexPositionColorNormal(d1.Position + Vector3.Up * sizeY, color, Vector3.Up, d1.Tech);
            VertexPositionColorNormal u2 = new VertexPositionColorNormal(d2.Position + Vector3.Up * sizeY, color, Vector3.Up, d2.Tech);
            VertexPositionColorNormal u3 = new VertexPositionColorNormal(d3.Position + Vector3.Up * sizeY, color, Vector3.Up, d3.Tech);
            VertexPositionColorNormal u4 = new VertexPositionColorNormal(d4.Position + Vector3.Up * sizeY, color, Vector3.Up, d4.Tech);

            VertexPositionColorNormal n1 = new VertexPositionColorNormal(d1.Position, zColor, Vector3.Forward, d1.Tech);
            VertexPositionColorNormal n2 = new VertexPositionColorNormal(u1.Position, zColor, Vector3.Forward, u1.Tech);
            VertexPositionColorNormal n3 = new VertexPositionColorNormal(d3.Position, zColor, Vector3.Forward, d3.Tech);
            VertexPositionColorNormal n4 = new VertexPositionColorNormal(u3.Position, zColor, Vector3.Forward, u3.Tech);

            VertexPositionColorNormal s1 = new VertexPositionColorNormal(d2.Position, zColor, Vector3.Backward, d2.Tech);
            VertexPositionColorNormal s2 = new VertexPositionColorNormal(d4.Position, zColor, Vector3.Backward, d4.Tech);
            VertexPositionColorNormal s3 = new VertexPositionColorNormal(u2.Position, zColor, Vector3.Backward, u2.Tech);
            VertexPositionColorNormal s4 = new VertexPositionColorNormal(u4.Position, zColor, Vector3.Backward, u4.Tech);

            VertexPositionColorNormal w1 = new VertexPositionColorNormal(d1.Position, xColor, Vector3.Left, d1.Tech);
            VertexPositionColorNormal w2 = new VertexPositionColorNormal(d2.Position, xColor, Vector3.Left, d2.Tech);
            VertexPositionColorNormal w3 = new VertexPositionColorNormal(u1.Position, xColor, Vector3.Left, u1.Tech);
            VertexPositionColorNormal w4 = new VertexPositionColorNormal(u2.Position, xColor, Vector3.Left, u2.Tech);

            VertexPositionColorNormal e1 = new VertexPositionColorNormal(d3.Position, xColor, Vector3.Right, d3.Tech);
            VertexPositionColorNormal e2 = new VertexPositionColorNormal(u3.Position, xColor, Vector3.Right, u3.Tech);
            VertexPositionColorNormal e3 = new VertexPositionColorNormal(d4.Position, xColor, Vector3.Right, d4.Tech);
            VertexPositionColorNormal e4 = new VertexPositionColorNormal(u4.Position, xColor, Vector3.Right, u4.Tech);

            if (color.A == 255)
            {
                if (down)
                {
                    AddIndicesToList(solidIndices, solidVertices);
                    this.solidVertices.AddRange(new VertexPositionColorNormal[] { d1, d3, d2, d4 });
                }

                if (up)
                {
                    AddIndicesToList(solidIndices, solidVertices);
                    this.solidVertices.AddRange(new VertexPositionColorNormal[] { u1, u2, u3, u4 });
                }

                if (north)
                {
                    AddIndicesToList(solidIndices, solidVertices);
                    this.solidVertices.AddRange(new VertexPositionColorNormal[] { n1, n2, n3, n4 });
                }

                if (south)
                {
                    AddIndicesToList(solidIndices, solidVertices);
                    this.solidVertices.AddRange(new VertexPositionColorNormal[] { s1, s2, s3, s4 });
                }

                if (west)
                {
                    AddIndicesToList(solidIndices, solidVertices);
                    this.solidVertices.AddRange(new VertexPositionColorNormal[] { w1, w2, w3, w4 });
                }

                if (east)
                {
                    AddIndicesToList(solidIndices, solidVertices);
                    this.solidVertices.AddRange(new VertexPositionColorNormal[] { e1, e2, e3, e4 });
                }
            }
            else
            {
                if (down)
                {
                    AddIndicesToList(liquidIndices, liquidVertices);
                    this.liquidVertices.AddRange(new VertexPositionColorNormal[] { d1, d3, d2, d4 });
                }

                if (up)
                {
                    AddIndicesToList(liquidIndices, liquidVertices);
                    this.liquidVertices.AddRange(new VertexPositionColorNormal[] { u1, u2, u3, u4 });
                }

                if (north)
                {
                    AddIndicesToList(liquidIndices, liquidVertices);
                    this.liquidVertices.AddRange(new VertexPositionColorNormal[] { n1, n2, n3, n4 });
                }

                if (south)
                {
                    AddIndicesToList(liquidIndices, liquidVertices);
                    this.liquidVertices.AddRange(new VertexPositionColorNormal[] { s1, s2, s3, s4 });
                }

                if (west)
                {
                    AddIndicesToList(liquidIndices, liquidVertices);
                    this.liquidVertices.AddRange(new VertexPositionColorNormal[] { w1, w2, w3, w4 });
                }

                if (east)
                {
                    AddIndicesToList(liquidIndices, liquidVertices);
                    this.liquidVertices.AddRange(new VertexPositionColorNormal[] { e1, e2, e3, e4 });
                }
            }
        }

        private void AddIndicesToList(List<int> indices, List<VertexPositionColorNormal> vertices)
        {
            indices.AddRange(new int[] {
                vertices.Count,
                vertices.Count + 2,
                vertices.Count + 1,
                vertices.Count + 2,
                vertices.Count + 3,
                vertices.Count + 1 });
        }
    }

    public struct VertexPositionColorNormal : IVertexType
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;
        public Vector4 Tech;

        public VertexPositionColorNormal(Vector3 position, Color color, Vector3 normal, Vector4 Tech)
        {
            this.Position = position;
            this.Color = color;
            this.Normal = normal;
            this.Tech = Tech;
        }

        public VertexPositionColorNormal(Vector3 position, Color color, Vector3 normal)
        {
            this.Position = position;
            this.Color = color;
            this.Normal = normal;
            this.Tech = Vector4.Zero;
        }

        public VertexPositionColorNormal(Vector3 position, Color color)
        {
            this.Position = position;
            this.Color = color;
            this.Normal = Vector3.Zero;
            this.Tech = Vector4.Zero;
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get
            {
                return new VertexDeclaration(
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                    new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                    new VertexElement(sizeof(float) * 6 + 4, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 0));
            }
        }
    }
}
