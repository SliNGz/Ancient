using ancient.game.world;
using ancientlib.game.constants;
using ancientlib.game.stats;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ancientlib.game.entity
{
    public abstract class EntityDeveloping : EntityLiving
    {
        protected string name;

        protected int level;
        protected int exp;

        protected bool isRunning;
        protected double runningSpeed;

        public EntityDeveloping(World world) : base(world)
        {
            this.level = 1;
            this.exp = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateExp();

            if (HasMount())
                mount.SetRunning(isRunning);
        }

        public string GetName()
        {
            return this.name;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetLevel(int level)
        {
            this.level = MathHelper.Clamp(level, 0, GameConstants.MAX_LEVEL);
            OnLevelChange();
        }

        public void AddLevel(int add)
        {
            this.level = MathHelper.Clamp(this.level + add, 0, GameConstants.MAX_LEVEL);
            OnLevelChange();
        }

        public int GetLevel()
        {
            return this.level;
        }

        private void OnLevelChange()
        {
            this.exp = 0;
            this.maxHealth = StatTable.GetMaxHealth(level);
            world.PlaySound("level_up");
        }

        public void SetExp(int exp)
        {
            this.exp = MathHelper.Clamp(exp, 0, int.MaxValue);
        }

        public void AddExp(int add)
        {
            this.exp = MathHelper.Clamp(this.exp + add, 0, int.MaxValue);
        }

        public int GetExp()
        {
            return this.exp;
        }

        //Checks if entity leveled up and calls level up handler until leftover exp is smaller than 0 (exp is smaller than maxExp)
        private void UpdateExp()
        {
            if (this.level == GameConstants.MAX_LEVEL)
                return;

            int maxExp = StatTable.GetExpToNextLevel(level);
            int loExp = this.exp - maxExp;

            //level up
            if (loExp >= 0)
            {
                AddLevel(1);

                this.exp = loExp;
                UpdateExp();
            }
        }

        public override void Respawn()
        {
            base.Respawn();
            AddExp((int)(-0.2F * StatTable.GetExpToNextLevel(this.level)));
        }

        public bool IsRunning()
        {
            return this.isRunning;
        }

        public void SetRunning(bool isRunning)
        {
            this.isRunning = isRunning;
        }

        public override double GetSpeedAddition()
        {
            return base.GetSpeedAddition() + (isRunning ? runningSpeed : 0);
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.name = reader.ReadString();
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(name);
        }
    }
}
