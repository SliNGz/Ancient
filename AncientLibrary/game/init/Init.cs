using ancientlib.AncientService;
using ancientlib.game.classes;
using ancientlib.game.command;
using ancientlib.game.entity;
using ancientlib.game.network.packet;
using ancientlib.game.particle;
using ancientlib.game.stats;
using ancientlib.game.utils.chat;
using ancientlib.game.world.biome;
using ancientlib.game.world.entity;
using ancientlib.game.world.structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.init
{
    public class Init
    {
        public static void Initialize()
        {
            StatTable.Initialize();
            Blocks.Initialize();
            Items.Initialize();
            Entities.Initialize();
            BiomeManager.Initialize();
            CommandHandler.Initialize();
            Structures.Initialize();
            Packets.Initialize();
            NetEntities.Initialize();
            Particles.Initialize();
            ChatComponents.Initialize();
            Classes.Initialize();
        }
    }
}
