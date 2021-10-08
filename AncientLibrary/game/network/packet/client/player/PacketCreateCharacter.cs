using ancient.game.entity.player;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.client.player
{
    public class PacketCreateCharacter : Packet
    {
        protected string name;

        protected Color skinColor;

        protected byte hairID;
        protected Color hairColor;

        protected byte eyesID;
        protected Color eyesColor;

        public PacketCreateCharacter()
        { }

        public PacketCreateCharacter(EntityPlayer player)
        {
            this.name = player.GetName();
            this.skinColor = player.GetSkinColor();
            this.hairID = player.GetHairID();
            this.hairColor = player.GetHairColor();
            this.eyesID = player.GetEyesID();
            this.eyesColor = player.GetEyesColor();
        }

        public override void Read(BinaryReader reader)
        {
            this.name = reader.ReadString();
            this.skinColor = reader.ReadColorRGB();
            this.hairID = reader.ReadByte();
            this.hairColor = reader.ReadColorRGB();
            this.eyesID = reader.ReadByte();
            this.eyesColor = reader.ReadColorRGB();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(name);
            writer.WriteColorRGB(skinColor);
            writer.Write(hairID);
            writer.WriteColorRGB(hairColor);
            writer.Write(eyesID);
            writer.WriteColorRGB(eyesColor);
        }

        public string GetName()
        {
            return this.name;
        }

        public Color GetSkinColor()
        {
            return this.skinColor;
        }

        public byte GetHairID()
        {
            return this.hairID;
        }

        public Color GetHairColor()
        {
            return this.hairColor;
        }

        public byte GetEyesID()
        {
            return this.eyesID;
        }

        public Color GetEyesColor()
        {
            return this.eyesColor;
        }
    }
}
