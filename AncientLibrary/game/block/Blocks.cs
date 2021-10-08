using ancient.game.world.block;
using ancientlib.game.block;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.init
{
    public class Blocks
    {
        public static readonly Dictionary<int, Block> blocks = new Dictionary<int, Block>();

        public static readonly BlockAir air = new BlockAir();
        public static readonly BlockDirt dirt = new BlockDirt();
        public static readonly BlockGrass grass = new BlockGrass();
        public static readonly BlockLog log = new BlockLog();
        public static readonly BlockLeaves leaves = new BlockLeaves();
        public static readonly BlockWater water = new BlockWater();
        public static readonly BlockLantern lantern = new BlockLantern();
        public static readonly BlockSand sand = new BlockSand();
        public static readonly BlockSnow snow = new BlockSnow();
        public static readonly BlockIce ice = new BlockIce();
        public static readonly BlockCloud cloud = new BlockCloud();
        public static readonly BlockCampfire campfire = new BlockCampfire();
        public static readonly BlockTallGrass tall_grass = new BlockTallGrass(false);
        public static readonly BlockTallGrass tall_grass_snow = new BlockTallGrass(true);
        public static readonly BlockFlowers flowers = new BlockFlowers();
        public static readonly BlockBlueberriesBush blueberries_bush = new BlockBlueberriesBush(false);
        public static readonly BlockBlueberriesBush blueberries_bush_snow = new BlockBlueberriesBush(true);
        public static readonly BlockSnowLayer snow_layer = new BlockSnowLayer();
        public static readonly BlockLog log_sakura = (BlockLog)(new BlockLog().SetName("Sakura Log").SetColor(new Color(69, 69, 82)).SetSecondaryColor(new Color(54, 37, 47)));
        public static readonly BlockLeaves leaves_sakura = (BlockLeaves)(new BlockLeaves().SetName("Sakura Leaves").SetColor(new Color(219, 144, 183)).SetSecondaryColor(new Color(247, 221, 234)));
        public static readonly BlockStone stone = new BlockStone();
        public static readonly BlockBase baseBlock = new BlockBase();
        public static readonly BlockBranch branch = new BlockBranch();

        public static void Initialize()
        {
            InitializeBlock(0, air);
            InitializeBlock(1, dirt);
            InitializeBlock(2, grass);
            InitializeBlock(3, log);
            InitializeBlock(4, leaves);
            InitializeBlock(5, water);
            InitializeBlock(6, lantern);
            InitializeBlock(7, sand);
            InitializeBlock(8, snow);
            InitializeBlock(9, ice);
            InitializeBlock(10, cloud);
            InitializeBlock(11, campfire);
            InitializeBlock(12, tall_grass);
            InitializeBlock(13, tall_grass_snow);
            InitializeBlock(14, flowers);
            InitializeBlock(15, blueberries_bush);
            InitializeBlock(16, blueberries_bush_snow);
            InitializeBlock(17, snow_layer);
            InitializeBlock(18, log_sakura);
            InitializeBlock(19, leaves_sakura);
            InitializeBlock(20, stone);
            InitializeBlock(21, baseBlock);
            InitializeBlock(22, branch);
        }

        private static void InitializeBlock(int id, Block block)
        {
            blocks.Add(id, block);
        }

        public static Block GetBlockFromID(int id)
        {
            Block block = null;
            blocks.TryGetValue(id, out block);

            return block;
        }

        public static int GetIDFromBlock(Block block)
        {
            return blocks.FirstOrDefault(x => x.Value == block).Key;
        }
    }
}
