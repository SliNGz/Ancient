using ancient.game.renderers.model;
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

namespace ancient.game.client.renderer.model
{
    class ModelDatabase
    {
        private static readonly string basePath = "Content/model/";
        private static readonly Dictionary<string, ModelData> models = new Dictionary<string, ModelData>();

        public static void Initialize()
        {
            /*  Entity  */

            /*  Player  */
            InitializeModelData("entity/player/races/human/human");
            InitializeModelData(CreateModelData("entity/player/races/human/human_sitting").SetOffset(new Vector3(0, 0, 4)));
            InitializeModelData("entity/player/races/elf/elf");

            // Hair
            InitializeModelData("entity/player/hair/hair_0");
            InitializeModelData("entity/player/hair/hair_1");

            // Eyes
            InitializeModelData(CreateEyesModelData("entity/player/eyes/eyes_0"));
            InitializeModelData(CreateEyesModelData("entity/player/eyes/eyes_1", 0, -1, 0));
            InitializeModelData(CreateEyesModelData("entity/player/eyes/eyes_2"));
            InitializeModelData(CreateEyesModelData("entity/player/eyes/eyes_3"));

            InitializeModelData(CreateModelData("entity/player/eyes/eyes_4_pupils").SetOffset(new Vector3(0, -1, 0)));
            InitializeModelData(CreateModelData("entity/player/eyes/eyes_4").SetOffset(new Vector3(0, 0, 0)).AddAttachment(GetModelFromName("eyes_4_pupils")));

            InitializeModelData(CreateModelData("entity/player/eyes/eyes_5_pupils").SetOffset(new Vector3(0, -1, 0)));
            InitializeModelData(CreateModelData("entity/player/eyes/eyes_5").AddAttachment(GetModelFromName("eyes_5_pupils")));

            //Female Eyes
            InitializeModelData(CreateEyesModelData("entity/player/eyes/female/eyes_female_0"));
            /*      */


            // Passive
            InitializeModelData("entity/passive/tortoise/tortoise");
            InitializeModelData("entity/passive/tortoise/tortoise_sleeping");
            InitializeModelData("entity/passive/slime");

            // World
            InitializeModelData("entity/world/berkin");
            /*      */


            /*  Item    */

            // Bows
            InitializeModelData("item/bow/wooden_bow");

            // Projectile
            InitializeModelData("item/projectile/arrow/steel_arrow");
            InitializeModelData("item/projectile/arrow/explosive_arrow");

            // Swords
            InitializeModelData("item/sword/sword");
            InitializeModelData("item/sword/sword2");

            // Staffs
            InitializeModelData("item/staff/staff");

            // Daggers
            InitializeModelData("item/dagger/dagger");

            // Food
            InitializeModelData("item/pet/food/carrot");
            InitializeModelData("item/pet/food/blueberries");
            /*      */


            /*  Skill   */

            //Thief
            InitializeModelData("skill/thief/flash_jump");
            /*      */


            /*  Block   */
            InitializeModelData(CreateModelData("block/lantern_glass").SetScale(new Vector3(1 / 17F, 1 / 17F, 1 / 17F)).SetOffset(new Vector3(5 / 17F, 2 / 17F, 5 / 17F)).SetAlpha(0.65F));
            InitializeModelData(CreateModelData("block/lantern").SetScale(new Vector3(1 / 17F, 1 / 17F, 1 / 17F)).SetOffset(new Vector3(3 / 17F, 0, 3 / 17F)).AddAttachment(GetModelFromName("lantern_glass")));
            InitializeModelData(CreateModelData("block/campfire").SetScale(new Vector3(1 / 13F, 1 / 13F, 1 / 13F)));
            InitializeModelData(CreateModelData("block/tall_grass").SetScale(new Vector3(0.0625F, 0.0625F, 0.0625F)));
            InitializeModelData(CreateModelData("block/tall_grass_snow").SetScale(new Vector3(0.0625F, 0.0625F, 0.0625F)));
            InitializeModelData(CreateModelData("block/tall_grass_taiga").SetScale(new Vector3(0.0625F, 0.0625F, 0.0625F)));
            InitializeModelData(CreateModelData("block/flowers").SetScale(new Vector3(0.0625F, 0.0625F, 0.0625F)));
            InitializeModelData(CreateModelData("block/blueberries_bush").SetScale(new Vector3(0.0625F, 0.0625F, 0.0625F)));
            InitializeModelData(CreateModelData("block/blueberries_bush_snow").SetScale(new Vector3(0.0625F, 0.0625F, 0.0625F)));

            /*  Particle    */
            for (int i = 0; i < 4; i++)
                InitializeModelData("particle/particle_snow_" + i);

            // Non Model
            InitializeNonModelBlocks();

            // Simple Voxel
            InitializeModelData(new ModelData("voxel", new Color[,,] { { { Color.White } } }));
        }

        public static void InitializeModelData(string path)
        {
            InitializeModelData(CreateModelData(path));
        }

        public static void InitializeModelData(ModelData model)
        {
            models.Add(model.GetName(), model);
        }

        public static ModelData CreateModelData(string path)
        {
            string[] splitedPath = path.Split('/');
            return new ModelData(splitedPath[splitedPath.Length - 1], MagicaVoxelImporter.FromMagica(new BinaryReader(File.OpenRead(basePath + path + ".vox"))));
        }

        public static ModelData GetModelFromName(string name)
        {
            ModelData model = null;
            models.TryGetValue(name, out model);
            return model;
        }

        private static void InitializeNonModelBlocks()
        {
            for (int i = 0; i < Blocks.blocks.Count; i++)
            {
                Block block = Blocks.blocks[i];

                if (!models.ContainsKey(block.GetModelName()))
                    InitializeModelData(new ModelData(block));
            }
        }

        private static ModelData CreateEyesModelData(string path, float xOffset = 0, float yOffset = 0, float zOffset = 0)
        {
            Vector3 offset = new Vector3(xOffset, yOffset, zOffset);
            ModelData eyes = CreateModelData(path).SetOffset(offset);

            if (File.Exists(basePath + path + "_pupils.vox"))
            {
                ModelData pupils = CreateModelData(path + "_pupils").SetOffset(offset);
                eyes.AddAttachment(pupils);
            }

            return eyes;
        }
    }
}
