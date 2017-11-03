using ancient.game.entity.player;
using ancient.game.world;
using ancientlib.game.classes;
using ancientlib.game.entity.skill;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.skill
{
    public abstract class Skill
    {
        protected EntityPlayer player;
        protected string name;

        protected int level;
        protected int maxLevel;

        protected int manaConsume;

        protected int cooldown;
        protected int ticksElapsed;

        protected int lifeSpan;

        public Skill(EntityPlayer player)
        {
            this.player = player;
            this.name = "Unnamed";
            this.level = 1;
            this.maxLevel = 1;
            this.manaConsume = 0;
            this.cooldown = 0;
            this.lifeSpan = 0;
        }

        public virtual void Activate()
        {
            World world = player.GetWorld();
            world.PlaySound(GetSoundName());
            player.AddMana(-manaConsume);

            this.ticksElapsed = cooldown;
        }

        public virtual bool CanActivate()
        {
            return player.GetMana() >= manaConsume && ticksElapsed <= 0;
        }

        public abstract Vector3 GetModelOffset();

        public abstract string GetSoundName();

        public abstract void OnLevelUp();

        public virtual void Update(GameTime gameTime)
        {
            ticksElapsed--;
        }

        public string GetName()
        {
            return this.name;
        }

        public int GetLifeSpan()
        {
            return this.lifeSpan;
        }

        public string GetModelName()
        {
            string modelName = name;
            modelName = modelName.ToLower();
            modelName = modelName.Replace(' ', '_');
            return modelName;
        }

        public void LevelUp()
        {
            this.level++;
            OnLevelUp();
        }
    }
}
