using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity;
using ancient.game.renderers.model;
using ancient.game.entity.player;
using ancient.game.client.renderer.item;
using Microsoft.Xna.Framework;
using ancientlib.game.init;
using ancient.game.renderers.world;
using ancientlib.game.item.equip.special;
using ancient.game.client.renderer.model;
using ancientlib.game.item.equip.bottom;
using Microsoft.Xna.Framework.Graphics;

namespace ancient.game.client.renderer.entity
{
    public class RenderEntityPlayer : RenderEntityPlayerBase
    {
        protected override void Draw(Entity entity, ModelData model, Vector3 scale)
        {
            base.Draw(entity, model, scale);
            EntityPlayer player = (EntityPlayer)entity;
           // DrawBottom(player, model);
            DrawSpecial(player, model);
        }

        private void DrawBottom(EntityPlayer player, ModelData model)
        {
        /*    ItemBottom bottom = player.GetBottom();

            if (bottom == null)
                return;

            Vector3 scale = bottom.GetModelScale() * (player.GetModelScale() * 1.25F);

            ModelData bottomModel = ModelDatabase.GetModelFromName(bottom.GetModelName());
            Vector3 offset = new Vector3(0, -7, 0);
            offset *= player.GetModelScale();

            Vector3 diff = bottomModel.GetDimensions() - bottomModel.GetDimensions() * 1.25F;

            ItemRenderer.Draw(bottomModel, neckPosition + offset, model.GetOffset() + ItemRenderer.GetRotationCenter(bottomModel) * new Vector3(1, 0, 1),
                player.GetYaw(), 0, 0, scale.X, scale.Y, scale.Z);*/
        }

        private void DrawSpecial(EntityPlayer player, ModelData model)
        {
            ItemSpecial special = player.GetSpecial();

            if (special == null)
                return;

            Vector3 scale = special.GetModelScale();

            ModelData specialModel = ModelDatabase.GetModelFromName(special.GetModelName() + (player.usingSpecial ? "_use" : ""));
            Vector3 offset = new Vector3(0, model.GetHeight() / 4F, model.GetLength() / 2F);
            offset *= player.GetModelScale();
            ItemRenderer.Draw(specialModel, player.GetPosition(), -offset / special.GetModelScale() + ItemRenderer.GetRotationCenter(specialModel) * new Vector3(1, 0, 1),
                special.GetRenderYaw() + player.GetYaw(), 0, 0, scale.X, scale.Y, scale.Z);
        }
    }
}
