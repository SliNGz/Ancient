using ancient.game.world.block.type;
using ancientlib.game.entity.player;
using ancientlib.game.network.packet.server.entity;
using ancientlib.game.world.biome;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.world.block
{
    public class BlockGrass : Block
    {
        private Dictionary<string, GrassColor> colors;

        public BlockGrass() : base("Grass", BlockType.grass)
        {
            this.color = new Color(60, 100, 60);
            this.secondaryColor = new Color(33, 63, 33);

            this.colors = new Dictionary<string, GrassColor>();

            this.colors.Add("taiga", new GrassColor(new Color(60, 100, 60), new Color(33, 63, 33)));
            this.colors.Add("taiga_hills", new GrassColor(Color.MediumPurple, new Color(33, 63, 33)));
        }

        public override Color GetColorAt(World world, int x, int y, int z)
        {
            Biome biome = world.GetBiomeAt(x, z);
            /*
                        GrassColor grassColor;

                        Color color1 = this.color;
                        Color color2 = this.secondaryColor;

                        if (colors.TryGetValue(biome.GetName(), out grassColor))
                        {
                            color1 = grassColor.Color1;
                            color2 = grassColor.Color2;
                        }*/

            float noise = (float)(world.GetSimplexNoise().Evaluate(x / 32.0, y / 4.0, z / 32.0));
            noise = (noise + 1) / 2;

            return Color.Lerp(biome.GetGrassColor1(), biome.GetGrassColor2(), noise);
        }
    }

    struct GrassColor
    {
        public Color Color1;
        public Color Color2;

        public GrassColor(Color color1, Color color2)
        {
            this.Color1 = color1;
            this.Color2 = color2;
        }
    }
}