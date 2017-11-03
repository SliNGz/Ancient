using ancient.game.entity.player;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network.packet.client.player
{
    public class PacketPlayerInput : Packet
    {
        private bool pressW;
        private bool pressA;
        private bool pressS;
        private bool pressD;
        private bool isJumping;
        private bool isRunning;

        public PacketPlayerInput()
        { }

        public PacketPlayerInput(EntityPlayer player)
        {
            Vector3 movement = player.inputVector;
            this.pressW = movement.Z == -1;
            this.pressA = movement.X == -1;
            this.pressS = movement.Z == 1;
            this.pressD = movement.X == 1;
            this.isJumping = player.IsJumping();
            this.isRunning = player.IsRunning();
        }

        public override void Read(BinaryReader reader)
        {
            byte input = reader.ReadByte();

            this.isRunning = (input & 1) == 1;
            input >>= 1;

            this.isJumping = (input & 1) == 1;
            input >>= 1;

            this.pressD = (input & 1) == 1;
            input >>= 1;

            this.pressS = (input & 1) == 1;
            input >>= 1;

            this.pressA = (input & 1) == 1;
            input >>= 1;

            this.pressW = (input & 1) == 1;
            input >>= 1;
        }

        public override void Write(BinaryWriter writer)
        {
            byte input = 0;
            input |= (byte)(pressW ? 1 : 0);

            input <<= 1;
            input |= (byte)(pressA ? 1 : 0);

            input <<= 1;
            input |= (byte)(pressS ? 1 : 0);

            input <<= 1;
            input |= (byte)(pressD ? 1 : 0);

            input <<= 1;
            input |= (byte)(isJumping ? 1 : 0);

            input <<= 1;
            input |= (byte)(isRunning ? 1 : 0);

            writer.Write(input);
        }

        public Vector3 GetMovementVector()
        {
            int x = (pressA ? -1 : 0) + (pressD ? 1 : 0);
            int z = (pressW ? -1 : 0) + (pressS ? 1 : 0);
            return new Vector3(x, 0, z);
        }

        public bool IsJumping()
        {
            return this.isJumping;
        }

        public bool IsRunning()
        {
            return this.isRunning;
        }
    }
}
