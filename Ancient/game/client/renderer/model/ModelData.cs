using ancient.game.entity;
using ancient.game.entity.player;
using ancient.game.renderers.voxel;
using ancient.game.utils;
using ancient.game.world.block;
using ancientlib.game.entity;
using ancientlib.game.init;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace ancient.game.renderers.model
{
    public class ModelData
    {
        private string name;

        private Color[,,] voxels;
        private VoxelRendererData voxelRendererData;

        private int width;
        private int height;
        private int length;

        private Vector3 scale;
        private Vector3 offset;

        private List<ModelData> attachments;

        public ModelData(string name, Color[,,] voxels)
        {
            this.name = name;

            this.voxels = voxels;
            this.voxelRendererData = new VoxelRendererData(voxels);

            this.width = voxels.GetLength(0);
            this.height = voxels.GetLength(1);
            this.length = voxels.GetLength(2);

            this.scale = Vector3.One;
            this.offset = Vector3.Zero;

            this.attachments = new List<ModelData>();
        }

        public ModelData(Block block) : this(block.GetModelName(), new Color[,,] { { { block.GetColor() } } })
        {
            this.width = 1;
            this.height = 1;
            this.length = 1;
        }

        public string GetName()
        {
            return this.name;
        }

        public Color[,,] GetVoxels()
        {
            return this.voxels;
        }

        public VoxelRendererData GetVoxelRendererData()
        {
            return this.voxelRendererData;
        }

        public int GetWidth()
        {
            return this.width;
        }

        public int GetHeight()
        {
            return this.height;
        }

        public int GetLength()
        {
            return this.length;
        }

        public Vector3 GetScale()
        {
            return this.scale;
        }

        public ModelData SetScale(Vector3 scale)
        {
            this.scale = scale;
            return this;
        }

        public Vector3 GetOffset()
        {
            return this.offset;
        }

        public ModelData SetOffset(Vector3 offset)
        {
            this.offset = offset;
            return this;
        }

        public ModelData SetAlpha(float alpha)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < length; z++)
                    {
                        if (this.voxels[x, y, z] != Color.Transparent)
                            this.voxels[x, y, z].A = (byte)(alpha * 255);
                    }
                }
            }
            return this;
        }

        public List<ModelData> GetAttachments()
        {
            return this.attachments;
        }

        public ModelData AddAttachment(ModelData attachment)
        {
            this.attachments.Add(attachment);
            return this;
        }
    }
}
