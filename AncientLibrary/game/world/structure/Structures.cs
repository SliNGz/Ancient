using ancient.game.utils;
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

        public static readonly int CLOUD_NUM = 2;

        public static void Initialize()
        {
            //Tree
            for (int i = 0; i < TREE_NUM; i++)
                InitializeStructureTree("tree/tree_" + i);

            for (int i = 0; i < TREE_SNOW_NUM; i++)
                InitializeStructureTree("tree/tree_snow_" + i);

            //Cloud
            for (int i = 0; i < CLOUD_NUM; i++)
                InitializeStructureCloud("cloud/cloud_" + i);
        }

        private static void InitializeStructure(string path)
        {
            string[] splitedPath = path.Split('/');
            structures.Add(splitedPath[splitedPath.Length - 1], new Structure(path));
        }

        private static void InitializeStructureTree(string path)
        {
            string[] splitedPath = path.Split('/');
            structures.Add(splitedPath[splitedPath.Length - 1], new StructureTree(path));
        }

        private static void InitializeStructureCloud(string path)
        {
            string[] splitedPath = path.Split('/');
            structures.Add(splitedPath[splitedPath.Length - 1], new StructureCloud(path));
        }

        public static Structure GetStructureFromName(string name)
        {
            Structure structure = null;
            structures.TryGetValue(name, out structure);
            return structure;
        }
    }
}
