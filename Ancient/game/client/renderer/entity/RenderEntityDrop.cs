using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity;
using ancient.game.renderers.model;
using Microsoft.Xna.Framework;
using ancientlib.game.entity.world;

namespace ancient.game.client.renderer.entity
{
    public class RenderEntityDrop : RenderEntity
    {
        protected override void Draw(Entity entity, ModelData model, Vector3 scale)
        {
            //scale = entity.GetModelScale();

            Vector3 position = ((EntityDrop)entity).GetAnimationPosition();
            position.Y += model.GetHeight() * scale.Y - entity.GetHeight();
            Draw(entity, model, position, entity.GetYaw(), entity.GetPitch(), entity.GetRoll(), scale.X, scale.Y, scale.Z);
        }
    }
}
