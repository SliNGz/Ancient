using ancient.game.entity;
using ancientlib.game.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.renderer.entity
{
    class EntityRenderers
    {
        private static Dictionary<string, RenderEntity> renderers = new Dictionary<string, RenderEntity>();

        public static void Initialize()
        {
            InitializeRenderEntity("entity", new RenderEntity());
            InitializeRenderEntity("playerBase", new RenderEntityPlayerBase());
        }

        private static void InitializeRenderEntity(string name, RenderEntity renderEntity)
        {
            renderers.Add(name, renderEntity);
        }

        public static RenderEntity GetRenderEntityFromEntity(Entity entity)
        {
            RenderEntity renderEntity = null;
            renderers.TryGetValue(entity.GetRenderEntity(), out renderEntity);

            return renderEntity;
        }
    }
}
