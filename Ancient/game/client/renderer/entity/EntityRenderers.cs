using ancient.game.entity;
using ancientlib.game.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.renderer.entity
{
    public class EntityRenderers
    {
        private static Dictionary<string, RenderEntity> renderers = new Dictionary<string, RenderEntity>();

        public static RenderEntity renderEntity = new RenderEntity();
        public static RenderEntityLiving renderLiving = new RenderEntityLiving();
        public static RenderEntityPlayerBase renderPlayerBase = new RenderEntityPlayerBase();
        public static RenderEntityPlayer renderPlayer = new RenderEntityPlayer();
        public static RenderEntityDrop renderDrop = new RenderEntityDrop();
        public static RenderEntityPortal renderPortal = new RenderEntityPortal();

        public static void Initialize()
        {
            InitializeRenderEntity("entity", renderEntity);
            InitializeRenderEntity("living", renderLiving);
            InitializeRenderEntity("player_base", renderPlayerBase);
            InitializeRenderEntity("player", renderPlayer);
            InitializeRenderEntity("drop", renderDrop);
            InitializeRenderEntity("portal", renderPortal);
        }

        private static void InitializeRenderEntity(string name, RenderEntity renderEntity)
        {
            renderers.Add(name, renderEntity);
        }

        public static RenderEntity GetRenderEntityFromEntity(Entity entity)
        {
            RenderEntity renderEntity = null;
            renderers.TryGetValue(entity.GetRendererName(), out renderEntity);

            return renderEntity;
        }
    }
}
