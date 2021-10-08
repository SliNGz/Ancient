using ancient.game.utils;
using ancient.game.world.block;
using ancientlib.game.init;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.world.structure
{
    public class Structures
    {
        public static Dictionary<string, Structure> structures = new Dictionary<string, Structure>();
        public static string basePath = "Content/structure/";

        public static readonly int TREE_NUM = 3;
        public static readonly int TREE_SNOW_NUM = 3;
        public static readonly int TREE_JUNGLE_NUM = 1;
        public static readonly int TREE_BLOSSOM_NUM = 2;

        public static readonly int CLOUD_NUM = 2;

        public static void Initialize()
        {
            //Tree
            Dictionary<Color, Block> taigaList = new Dictionary<Color, Block>()
            {
                { new Color(36, 17, 0), Blocks.log },
                { new Color(0, 38, 2), Blocks.leaves },
                { Color.White, Blocks.snow }
            };
            for (int i = 0; i < TREE_NUM; i++)
                InitializeStructureTree("tree/tree_" + i, taigaList);

            for (int i = 0; i < TREE_SNOW_NUM; i++)
                InitializeStructureTree("tree/tree_snow_" + i, taigaList);

            Dictionary<Color, Block> jungleList = new Dictionary<Color, Block>() { { new Color(36, 17, 0), Blocks.log }, { new Color(0, 38, 2), Blocks.leaves } };
            for (int i = 0; i < TREE_JUNGLE_NUM; i++)
                InitializeStructureTree("tree/tree_jungle_" + i, jungleList);

            Dictionary<Color, Block> blossomList = new Dictionary<Color, Block>()
            {
                { new Color(69, 69, 82), Blocks.log_sakura },
                { new Color(219, 144, 183), Blocks.leaves_sakura },
                { Color.White, Blocks.snow_layer }
            };
            for (int i = 0; i < TREE_BLOSSOM_NUM; i++)
                InitializeStructureTree("tree/tree_blossom_" + i, blossomList);

            Dictionary<Color, Block> cloudList = new Dictionary<Color, Block>() { { Color.White, Blocks.cloud } };
            //Cloud
            for (int i = 0; i < CLOUD_NUM; i++)
                InitializeStructureCloud("cloud/cloud_" + i, cloudList);
        }

        private static void InitializeStructure(string path, Dictionary<Color, Block> colorToBlockList)
        {
            string[] splitedPath = path.Split('/');
            structures.Add(splitedPath[splitedPath.Length - 1], new Structure(path, colorToBlockList));
        }

        private static void InitializeStructureTree(string path, Dictionary<Color, Block> colorToBlockList)
        {
            string[] splitedPath = path.Split('/');
            structures.Add(splitedPath[splitedPath.Length - 1], new StructureTree(path, colorToBlockList));
        }

        private static void InitializeStructureCloud(string path, Dictionary<Color, Block> colorToBlockList)
        {
            string[] splitedPath = path.Split('/');
            structures.Add(splitedPath[splitedPath.Length - 1], new StructureCloud(path, colorToBlockList));
        }

        public static Structure GetStructureFromName(string name)
        {
            Structure structure = null;
            structures.TryGetValue(name, out structure);
            return structure;
        }
    }
}
