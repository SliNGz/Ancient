using ancient.game.world;
using ancient.game.world.block;
using ancientlib.game.classes;
using ancientlib.game.item;
using ancientlib.game.item.projectile;
using ancientlib.game.item.weapon;
using ancientlib.game.item.world;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.init
{
    public class Items
    {
        public static readonly Dictionary<int, Item> items = new Dictionary<int, Item>();

        //Blocks
        public static readonly ItemBlock air = new ItemBlock("Air", Blocks.air);
        public static readonly ItemBlock dirt = new ItemBlock("Dirt", Blocks.dirt);
        public static readonly ItemBlock grass = new ItemBlock("Grass", Blocks.grass);
        public static readonly ItemBlock log = new ItemBlock("Log", Blocks.log);
        public static readonly ItemBlock leaves = new ItemBlock("Leaves", Blocks.leaves);
        public static readonly ItemBlock water = new ItemBlock("Water", Blocks.water);
        public static readonly ItemBlock lantern = new ItemBlock("Lantern", Blocks.lantern);
        public static readonly ItemBlock sand = new ItemBlock("Sand", Blocks.sand);
        public static readonly ItemBlock snow = new ItemBlock("Snow", Blocks.snow);
        public static readonly ItemBlock ice = new ItemBlock("Ice", Blocks.ice);
        public static readonly ItemBlock cloud = new ItemBlock("Cloud", Blocks.cloud);
        public static readonly ItemBlock campfire = new ItemBlock("Campfire", Blocks.campfire);
        public static readonly ItemBlock tall_grass = new ItemBlock("Tall Grass", Blocks.tall_grass);
        public static readonly ItemBlock tall_grass_snow = new ItemBlock("Snow Tall Grass", Blocks.tall_grass_snow);
        public static readonly ItemBlock flowers = new ItemBlock("Flowers", Blocks.flowers);
        public static readonly ItemBlock blueberries_bush = new ItemBlock("Blueberries Bush", Blocks.blueberries_bush);
        public static readonly ItemBlock blueberries_bush_snow = new ItemBlock("Snow Blueberries Bush", Blocks.blueberries_bush_snow);

        //Swords
        public static readonly ItemSword sword = new ItemSword("Sword", 7, 32, 3);
        public static readonly ItemSword sword2 = new ItemSword("Sword2", 8902, 80, 7);

        //Projectiles
        public static readonly ItemArrow steelArrow = new ItemArrow("Steel Arrow", 1, 80, 0.1F, 0.1F, 0.1F, World.GRAVITY);
        public static readonly ItemArrow explosiveArrow = new ItemArrow("Explosive Arrow", 75, 100, 0.1F, 0.1F, 0.1F, World.GRAVITY);
        public static readonly ItemArrow explosiveArrowSkill = new ItemArrow("Explosive Arrow", 0, 35, 0.1F, 0.1F, 0.1F, World.GRAVITY);

        //Bows
        public static readonly ItemBow woodenBow = new ItemBow("Wooden Bow", 9, 32);

        //Staffs
        public static readonly ItemStaff staff = (ItemStaff)(new ItemStaff("Staff", 500, 64).SetModelOffset(new Vector3(0.8F, -0.3f, -1f)));

        //Daggers
        public static readonly ItemTwoHandedDagger dagger = (ItemTwoHandedDagger)(new ItemTwoHandedDagger("Dagger", 250, 32, 2.5F).SetModelScale(Vector3.One * 0.008F).SetModelOffset(new Vector3(0.8f, -0.55f, -1.25f)));

        //Pet Food
        public static readonly Item carrot = new Item("Carrot");
        public static readonly Item blueberries = new Item("Blueberries");

        //Berkin
        public static readonly ItemCoin berkin = new ItemCoin();

        public static void Initialize()
        {
            //Blocks
            InitializeItemBlock(air);
            InitializeItemBlock(dirt);
            InitializeItemBlock(grass);
            InitializeItemBlock(log);
            InitializeItemBlock(leaves);
            InitializeItemBlock(water);
            InitializeItemBlock(lantern);
            InitializeItemBlock(sand);
            InitializeItemBlock(snow);
            InitializeItemBlock(ice);
            InitializeItemBlock(cloud);
            InitializeItemBlock(campfire);
            InitializeItemBlock(tall_grass);
            InitializeItemBlock(tall_grass_snow);
            InitializeItemBlock(flowers);
            InitializeItemBlock(blueberries_bush);
            InitializeItemBlock(blueberries_bush_snow);

            //Swords
            InitializeItem(256, sword);
            InitializeItem(257, sword2);

            //Bows
            InitializeItem(512, woodenBow);

            //Projectiles
            InitializeItem(768, steelArrow);
            InitializeItem(769, explosiveArrow);

            //Tame
            InitializeItem(1024, carrot);
            InitializeItem(1025, blueberries);

            //Berkin
            InitializeItem(32767, berkin);
        }

        private static void InitializeItemBlock(ItemBlock itemBlock)
        {
            InitializeItem(Blocks.GetIDFromBlock(itemBlock.GetBlock()), itemBlock);
        }

        private static void InitializeItem(int id, Item item)
        {
            items.Add(id, item);
        }

        public static Item GetItemFromID(int id)
        {
            Item item = null;
            items.TryGetValue(id, out item);

            return item;
        }

        public static int GetIDFromItem(Item item)
        {
            return items.FirstOrDefault(x => x.Value == item).Key;
        }
    }
}
