using ancient.game.entity.player;
using ancientlib.game.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.server.player
{
    public class PacketPlayerMountPet : Packet
    {
        private MountAction mountAction;
        private int mountID;

        public PacketPlayerMountPet()
        { }

        public PacketPlayerMountPet(EntityMount mount, MountAction mountAction)
        {
            this.mountAction = mountAction;

            if (mountAction == MountAction.MOUNT)
                this.mountID = mount.GetID();
        }

        public override void Read(BinaryReader reader)
        {
            this.mountAction = (MountAction)reader.ReadByte();

            if (this.mountAction == MountAction.MOUNT)
                this.mountID = reader.ReadInt32();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write((byte)mountAction);

            if (this.mountAction == MountAction.MOUNT)
                writer.Write(mountID);
        }

        public MountAction GetMountAction()
        {
            return this.mountAction;
        }

        public int GetMountID()
        {
            return this.mountID;
        }
    }

    public enum MountAction
    {
        MOUNT = 0x00,
        DISMOUNT = 0x01
    }
}
