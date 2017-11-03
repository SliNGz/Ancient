using ancientlib.game.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.utils
{
    public class AttackInfo
    {
        private EntityLiving attacker;

        private int damage;
        private bool isCritical;

        public AttackInfo()
        { }

        public AttackInfo(EntityLiving attacker, int damage)
        {
            this.attacker = attacker;
            this.damage = damage;
            this.isCritical = false;
        }

        public AttackInfo SetAttacker(EntityLiving attacker)
        {
            this.attacker = attacker;
            return this;
        }

        public EntityLiving GetAttacker()
        {
            return this.attacker;
        }

        public AttackInfo SetDamage(int damage)
        {
            this.damage = damage;
            return this;
        }

        public int GetDamage()
        {
            return this.damage;
        }

        public bool IsCritical()
        {
            return this.isCritical;
        }

        public AttackInfo SetCritical(bool isCritical)
        {
            this.isCritical = isCritical;
            return this;
        }

        public void Read(BinaryReader reader)
        {
            this.damage = reader.ReadInt32();
            this.isCritical = reader.ReadBoolean();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(damage);
            writer.Write(isCritical);
        }
    }
}
