using ancientlib.game.constants;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancientlib.game.classes;
using ancientlib.game.item.statbased;
using ancientlib.game.item.tool;
using ancient.game.world.block;

namespace ancientlib.game.item
{
    public abstract class ItemWeapon : ItemTool
    {
        protected int damage;

        public ItemWeapon(string name, Class _class, int damage, int cooldown) : base(name)
        {
            this._class = _class;
            SetDamage(damage);
            this.cooldown = cooldown;
        }

        public int GetDamage()
        {
            return this.damage;
        }

        public void SetDamage(int damage)
        {
            this.damage = MathHelper.Clamp(damage, 0, GameConstants.MAX_DAMAGE);
        }

        public override float GetRenderSpeed(EntityPlayer player)
        {
            return this.renderSpeed * (1 + player.GetDex() / (8 * 128F));
        }

        public override int GetCooldown(EntityPlayer player)
        {
            return MathHelper.Clamp((int)(cooldown - (player.GetDex() / 20F)), 0, int.MaxValue);
        }

        protected override bool CanDestroyBlock(Block block)
        {
            return false;
        }
    }
}
