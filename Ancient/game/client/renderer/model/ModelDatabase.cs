using ancient.game.renderers.model;
using ancient.game.utils;
using ancient.game.world.block;
using ancientlib.game.block;
using ancientlib.game.init;
using MagicaVoxelContentExtension;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        private static readonly string basePath = "model/";
        private static string dirPath = "";
        private static readonly Dictionary<string, ModelData> models = new Dictionary<string, ModelData>();

        private static ContentManager Content;

        public static ModelData blueberriesBush;
        public static ModelData blueberriesBushSnow;
        public static ModelData branch;
        public static ModelData campfire;
        public static ModelData flowers;
        public static ModelData lantern;
        public static ModelData lanternGlass;
        public static ModelData tallGrass;
        public static ModelData tallGrassSnow;

        public static ModelData elf;

        public static ModelData human;
        public static ModelData humanHead;
        public static ModelData humanBody;
        public static ModelData humanHand;
        public static ModelData humanLeg;
        public static ModelData humanSitting;

        public static ModelData eyes0;
        public static ModelData eyes0Pupils;
        public static ModelData eyes1;
        public static ModelData eyes1Pupils;
        public static ModelData eyes2;
        public static ModelData eyes3;
        public static ModelData eyes3Pupils;
        public static ModelData eyes4;
        public static ModelData eyes4Pupils;
        public static ModelData eyes5;
        public static ModelData eyes5Pupils;

        public static ModelData eyesFemale0;
        public static ModelData eyesFemale0Pupils;
        public static ModelData eyesFemale1;
        public static ModelData eyesFemale1Pupils;

        public static ModelData hair0;
        public static ModelData hair1;

        public static ModelData tortoise;
        public static ModelData tortoiseSleeping;
        public static ModelData slime;
        public static ModelData bee;

        public static ModelData nymu;

        public static ModelData berkin;
        public static ModelData portal;
        public static ModelData portalInner;

        public static ModelData stoneAxe;

        public static ModelData woodenBow;

        public static ModelData dagger;

        public static ModelData jetpack;
        public static ModelData jetpackUse;

        public static ModelData blueberries;
        public static ModelData carrot;

        public static ModelData explosiveArrow;
        public static ModelData steelArrow;

        public static ModelData stoneShovel;

        public static ModelData stonePickaxe;

        public static ModelData woodenStaff;

        public static ModelData sword;
        public static ModelData sword2;

        public static ModelData particleSnow0;
        public static ModelData particleSnow1;
        public static ModelData particleSnow2;
        public static ModelData particleSnow3;

        public static ModelData flashJump;

        public static ModelData sun;

        public static void Initialize(ContentManager Content)
        {
            ModelDatabase.Content = Content;

            dirPath = "block/";
            blueberriesBush = LoadModelData("blueberries_bush");
            blueberriesBushSnow = LoadModelData("blueberries_bush_snow");
            branch = LoadModelData("branch");
            campfire = LoadModelData("campfire");
            flowers = LoadModelData("flowers");
            lanternGlass = LoadModelData("lantern_glass");
            lantern = LoadModelData("lantern").AddAttachment(lanternGlass);
            tallGrass = LoadModelData("tall_grass");
            tallGrassSnow = LoadModelData("tall_grass_snow");

            dirPath = "entity/player/races/elf/";
            elf = LoadModelData("elf");

            dirPath = "entity/player/races/human/";
            human = LoadModelData("human");
            humanHead = LoadModelData("human_head");
            humanBody = LoadModelData("human_body");
            humanHand = LoadModelData("human_hand");
            humanLeg = LoadModelData("human_leg");
            humanSitting = LoadModelData("human_sitting");

            dirPath = "entity/player/eyes/";
            eyes0Pupils = LoadModelData("eyes_0_pupils");
            eyes0 = LoadModelDataEyes("eyes_0", eyes0Pupils);

            eyes1Pupils = LoadModelData("eyes_1_pupils");
            eyes1 = LoadModelDataEyes("eyes_1", eyes1Pupils);

            eyes2 = LoadModelData("eyes_2");

            eyes3Pupils = LoadModelData("eyes_3_pupils");
            eyes3 = LoadModelDataEyes("eyes_3", eyes3Pupils);

            eyes4Pupils = LoadModelData("eyes_4_pupils");
            eyes4 = LoadModelDataEyes("eyes_4", eyes4Pupils);

            eyes5Pupils = LoadModelData("eyes_5_pupils");
            eyes5 = LoadModelDataEyes("eyes_5", eyes5Pupils);

            dirPath = "entity/player/eyes/female/";
            eyesFemale0Pupils = LoadModelData("eyes_female_0_pupils");
            eyesFemale0 = LoadModelDataEyes("eyes_female_0", eyesFemale0Pupils);

            eyesFemale1Pupils = LoadModelData("eyes_female_1_pupils");
            eyesFemale1 = LoadModelDataEyes("eyes_female_1", eyesFemale1Pupils);

            dirPath = "entity/player/hair/";
            hair0 = LoadModelData("hair_0");
            hair1 = LoadModelData("hair_1");

            dirPath = "entity/passive/tortoise/";
            tortoise = LoadModelData("tortoise");
            tortoiseSleeping = LoadModelData("tortoise_sleeping");

            dirPath = "entity/passive/slime/";
            slime = LoadModelData("slime");

            dirPath = "entity/passive/bee/";
            bee = LoadModelData("bee");

            dirPath = "entity/monster/nymu/";
            nymu = LoadModelData("nymu");

            dirPath = "entity/world/";
            berkin = LoadModelData("berkin");
            portal = LoadModelData("portal");
            portalInner = LoadModelData("portal_inner");

            dirPath = "item/axe/";
            stoneAxe = LoadModelData("stone_axe");

            dirPath = "item/bow/";
            woodenBow = LoadModelData("wooden_bow");

            dirPath = "item/dagger/";
            dagger = LoadModelData("dagger");

            dirPath = "item/equip/special/";
            jetpack = LoadModelData("jetpack");
            jetpackUse = LoadModelData("jetpack_use");

            dirPath = "item/pet/food/";
            carrot = LoadModelData("carrot");
            blueberries = LoadModelData("blueberries");

            dirPath = "item/projectile/arrow/";
            explosiveArrow = LoadModelData("explosive_arrow");
            steelArrow = LoadModelData("steel_arrow");

            dirPath = "item/shovel/";
            stoneShovel = LoadModelData("stone_shovel");

            dirPath = "item/pickaxe/";
            stonePickaxe = LoadModelData("stone_pickaxe");

            dirPath = "item/staff/";
            woodenStaff = LoadModelData("staff");

            dirPath = "item/sword/";
            sword = LoadModelData("sword");
            sword2 = LoadModelData("sword2");

            dirPath = "particle/";
            particleSnow0 = LoadModelData("particle_snow_0");
            particleSnow1 = LoadModelData("particle_snow_1");
            particleSnow2 = LoadModelData("particle_snow_2");
            particleSnow3 = LoadModelData("particle_snow_3");

            dirPath = "skill/thief/";
            flashJump = LoadModelData("flash_jump");

            dirPath = "world/";
            sun = LoadModelData("sun");

            InitializeNonModelBlocks();

            // Simple Voxel
            InitializeModelData(new ModelData("voxel", new Color[,,] { { { Color.White } } }));

            // Hand
            InitializeModelData(new ModelData("hand", new Color[,,] { { { } } }));
        }

        private static ModelData LoadModelData(string path)
        {
            string[] splitedPath = path.Split('/');
            string name = splitedPath[splitedPath.Length - 1];
            ModelData model = new ModelData(name, Content.Load<MagicaVoxelModelData>(basePath + dirPath + path));
            models.Add(name, model);
            return model;
        }

        private static ModelData LoadModelDataEyes(string path, ModelData pupils)
        {
            ModelData eyes = LoadModelData(path);
            eyes.AddAttachment(pupils);

            return eyes;
        }

        public static void Initalize(ContentManager Content)
        {
            /*  Entity  */

            /*  Player  */
            InitializeModelData("entity/player/races/human/human");
            InitializeModelData("entity/player/races/human/human_head");
            InitializeModelData("entity/player/races/human/human_body");
            InitializeModelData("entity/player/races/human/human_hand");
            InitializeModelData("entity/player/races/human/human_leg");
            InitializeWalkingAnimationModelData("entity/player/races/human/human_walking", -1, 1);

            InitializeModelData("entity/player/races/human/human_sitting");
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
            InitializeWalkingAnimationModelData("entity/passive/tortoise/tortoise_walking", -1, 1);

            InitializeModelData("entity/passive/slime");
            InitializeModelData("entity/passive/bee/bee");

            // Monster
            InitializeModelData("entity/monster/nymu/nymu");

            // World
            InitializeModelData("entity/world/berkin");
            InitializeModelData("entity/world/portal");
            InitializeModelData("entity/world/portal_inner");
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

            // Shovel
            InitializeModelData("item/shovel/wooden_shovel");

            // Axe
            InitializeModelData("item/axe/stone_axe");

            /*  Equip    */

            // Bottom
            InitializeModelData("item/equip/bottom/bottom_test");

            // Special
            InitializeModelData("item/equip/special/jetpack");
            InitializeModelData("item/equip/special/jetpack_use");

            /*      */

            /*      */


            /*  Skill   */

            // Thief
            InitializeModelData("skill/thief/flash_jump");
            /*      */


            /*  Block   */
            InitializeModelData(CreateModelData("block/lantern_glass").SetScale(new Vector3(1 / 17F, 1 / 17F, 1 / 17F)).SetOffset(new Vector3(5 / 17F, 2 / 17F, 5 / 17F)).SetAlpha(0.65F));
            InitializeModelData(CreateModelData("block/lantern").SetScale(new Vector3(1 / 17F, 1 / 17F, 1 / 17F)).SetOffset(new Vector3(3 / 17F, 0, 3 / 17F)).AddAttachment(GetModelFromName("lantern_glass")));
            InitializeModelData(CreateModelData("block/campfire").SetScale(new Vector3(1 / 13F, 1 / 13F, 1 / 13F)));
            InitializeModelData(CreateModelData("block/tall_grass").SetScale(new Vector3(0.0625F, 0.0625F, 0.0625F)));
            InitializeModelData(CreateModelData("block/tall_grass_snow").SetScale(new Vector3(0.0625F, 0.0625F, 0.0625F)));
            InitializeModelData(CreateModelData("block/tall_grass_taiga").SetScale(new Vector3(0.0625F, 0.0625F, 0.0625F)));
            InitializeModelData(CreateModelData("block/tall_grass_test").SetScale(new Vector3(0.125F, 0.125F, 0.125F)));
            InitializeModelData(CreateModelData("block/tall_grass_test_snow").SetScale(new Vector3(0.125F, 0.125F, 0.125F)));
            InitializeModelData(CreateModelData("block/flowers").SetScale(new Vector3(0.0625F, 0.0625F, 0.0625F)));
            InitializeModelData(CreateModelData("block/blueberries_bush").SetScale(new Vector3(0.0625F, 0.0625F, 0.0625F)));
            InitializeModelData(CreateModelData("block/blueberries_bush_snow").SetScale(new Vector3(0.0625F, 0.0625F, 0.0625F)));
            InitializeModelData(CreateModelData("block/small_tree").SetScale(Vector3.One / 17F).SetOffset(new Vector3(2, 0, 2) / 17F));
            InitializeModelData(CreateModelData("block/branch").SetScale(Vector3.One / 15F));

            /*  Particle    */
            for (int i = 0; i < 4; i++)
                InitializeModelData("particle/particle_snow_" + i);

            // Non Model
            InitializeNonModelBlocks();

            // Simple Voxel
            InitializeModelData(new ModelData("voxel", new Color[,,] { { { Color.White } } }));

            // Hand
            InitializeModelData(new ModelData("hand", new Color[,,] { { { } } }));

            // World
            InitializeModelData("world/sun");
        }

        private static void InitializeModelData(string path)
        {
            InitializeModelData(CreateModelData(path));
        }

        private static void InitializeModelData(ModelData model)
        {
            models.Add(model.GetName(), model);
        }

        private static ModelData CreateModelData(string path)
        {
            string[] splitedPath = path.Split('/');
            return new ModelData(splitedPath[splitedPath.Length - 1], game.utils.MagicaVoxelImporter.FromMagica(new BinaryReader(File.OpenRead(basePath + path + ".vox"))));
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

                if (block is IBlockModel)
                    continue;

                if (models.ContainsKey(block.GetModelName()))
                    throw new ArgumentException("A model with the name [" + block.GetModelName() + "] already exists.");

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

        private static void InitializeWalkingAnimationModelData(string path, int lowOffset, int highOffset)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector3 offset = Vector3.Zero;
                if (i == 1)
                    offset.Y = lowOffset;
                else if (i == 3)
                    offset.Y = highOffset;

                InitializeModelData(CreateModelData(path + "_" + i).SetOffset(offset));
            }
        }
    }
}
