using ancient.game.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using Microsoft.Xna.Framework;
using ancientlib.game.entity.ai;
using ancientlib.game.entity.ai.pathfinding;
using Microsoft.Xna.Framework.Graphics;
using ancient.game.world.chunk;
using ancientlib.game.utils;
using ancient.game.entity.player;
using ancientlib.game.constants;
using ancientlib.game.item;
using ancientlib.game.entity.world;
using ancient.game.world.block;
using ancientlib.game.entity.passive;
using System.IO;
using ancientlib.game.world;
using ancientlib.game.world.entity;

namespace ancientlib.game.entity
{
    public abstract class EntityLiving : Entity
    {
        protected EntityAIManager aiManager;
        protected PathFinder pathFinder;

        protected int maxHealth;
        protected int health;

        protected int maxMana;
        protected int mana;

        protected int healthRegen;
        protected int healthRegenInterval;

        protected int manaRegen;
        protected int manaRegenInterval;

        protected float jumpSpeed;

        protected float headYaw;
        protected float headPitch;

        protected EntityLiving lastAttacker;
        protected int attackedTicks;

        protected float fallVelocity;

        protected int berkins;

        protected int expReward;

        protected List<EntityPet> pets;

        public EntityLiving(World world) : base(world)
        {
            this.aiManager = new EntityAIManager();
            this.pathFinder = new PathFinder(this);

            this.maxHealth = 100;
            this.health = 100;

            this.maxMana = 100;
            this.mana = 100;

            this.healthRegen = 10;
            this.healthRegenInterval = 640;

            this.manaRegen = 10;
            this.manaRegenInterval = 640;

            this.lastAttacker = null;
            this.expReward = 0;

            this.lifeSpan = Utils.TicksInHour;

            this.pets = new List<EntityPet>();

            this.berkins = 0;

            this.fadeInTicks = Utils.TicksInSecond * 2;

            this.attackedTicks = 1024;
        }

        public override void Update(GameTime gameTime)
        {
            this.jumpSpeed = GetBaseJumpSpeed();
            base.Update(gameTime);

            aiManager.Update(gameTime);
            pathFinder.Update(gameTime);
            UpdateRegeneration();
            UpdateFallDamage();
            UpdateIsAttacked();
        }

        private void UpdateRegeneration()
        {
            if (IsAttacked() || !IsAlive())
                return;

            UpdateHealthRegeneration();
            UpdateManaRegeneration();
        }

        private void UpdateHealthRegeneration()
        {
            if (ticksExisted % healthRegenInterval != 0)
                return;

            AddHealth(healthRegen);
        }

        private void UpdateManaRegeneration()
        {
            if (ticksExisted % manaRegenInterval != 0)
                return;

            AddMana(manaRegen);
        }

        public PathFinder GetPathFinder()
        {
            return this.pathFinder;
        }

        public virtual Vector3 GetEyePosition()
        {
            return this.GetPosition();
        }

        public Vector3? GetTargetedBlockPosition(float distance = 5)
        {
            Ray ray = GetLookAtRay();

            int positionX = (int)ray.Position.X;
            int positionY = (int)ray.Position.Y;
            int positionZ = (int)ray.Position.Z;

            int directionX = ray.Direction.X > 0 ? 1 : -1;
            int directionY = ray.Direction.Y > 0 ? 1 : -1;
            int directionZ = ray.Direction.Z > 0 ? 1 : -1;

            for (int x = positionX - directionX; x != positionX + (distance + 1) * directionX; x += directionX)
            {
                for (int y = positionY - directionY; y != positionY + (distance + 1) * directionY; y += directionY)
                {
                    for (int z = positionZ - directionZ; z != positionZ + (distance + 1) * directionZ; z += directionZ)
                    {
                        BoundingBox box = world.GetBlockBoundingBox(x, y, z);
                        Block block = world.GetBlock(x, y, z);

                        if (block != null && block.CanBeDestroyed())
                        {
                            if (ray.Intersects(box) != null)
                            {
                                if (ray.Intersects(box) <= distance)
                                    return new Vector3(x, y, z);
                            }
                        }
                    }
                }
            }

            return null;
        }

        public override Vector3 GetLookAt()
        {
            return Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(headYaw, headPitch, 0));
        }

        public override void SetLookAt(double x, double y, double z)
        {
            Vector3 rotation = MathUtils.GetRotationFromPosition(this.x, this.y, this.z, x, y, z);
            SetYaw(rotation.X);
            SetHeadRotation(rotation.X, rotation.Y);
        }

        public Ray GetLookAtRay()
        {
            return new Ray(GetEyePosition(), GetLookAt());
        }

        public int? GetTargetedBlockFace()
        {
            Ray ray = GetLookAtRay();
            Vector3? position = GetTargetedBlockPosition();

            ChunkManager chunkManager = world.GetChunkManager();

            float min = 5;
            float distance;
            int face = 0;
            bool changed = true;

            if (position.HasValue)
            {
                int x = (int)position.Value.X;
                int y = (int)position.Value.Y;
                int z = (int)position.Value.Z;

                BoundingBox[] boundingBoxes = new BoundingBox[]
                {
                    Utils.GetBlockDownFace(x, y, z),
                    Utils.GetBlockUpFace(x, y, z),
                    Utils.GetBlockNorthFace(x, y, z),
                    Utils.GetBlockSouthFace(x, y, z),
                    Utils.GetBlockWestFace(x, y, z),
                    Utils.GetBlockEastFace(x, y, z)
                };

                for (int i = 0; i < 6; i++)
                {
                    if (ray.Intersects(boundingBoxes[i]).HasValue)
                    {
                        if ((distance = ray.Intersects(boundingBoxes[i]).Value) < min)
                        {
                            changed = true;
                            min = distance;
                            face = i;
                        }
                    }
                }
            }

            if (changed)
                return face;
            else
                return null;
        }

        public virtual void DestroyTargetedBlock()
        {
            Vector3? position = GetTargetedBlockPosition();

            if (position.HasValue)
            {
                int x = (int)position.Value.X;
                int y = (int)position.Value.Y;
                int z = (int)position.Value.Z;

                world.DestroyBlock(x, y, z);
            }
        }

        public void SetMaxHealth(int maxHealth)
        {
            this.maxHealth = MathHelper.Clamp(maxHealth, 0, GameConstants.MAX_HEALTH);
        }

        public void AddMaxHealth(int add)
        {
            this.maxHealth = MathHelper.Clamp(this.maxHealth + add, 0, GameConstants.MAX_HEALTH);
        }

        public int GetMaxHealth()
        {
            return this.maxHealth;
        }

        public void SetHealth(int health)
        {
            this.health = MathHelper.Clamp(health, 0, maxHealth);
        }

        public void AddHealth(int add)
        {
            this.health = MathHelper.Clamp(this.health + add, 0, maxHealth);
        }

        public int GetHealth()
        {
            return this.health;
        }

        public void SetMaxMana(int maxMana)
        {
            this.maxMana = MathHelper.Clamp(maxMana, 0, GameConstants.MAX_MANA);
        }

        public void AddMaxMana(int add)
        {
            this.maxMana = MathHelper.Clamp(this.maxMana + add, 0, GameConstants.MAX_MANA);
        }

        public int GetMaxMana()
        {
            return this.maxMana;
        }

        public void SetMana(int mana)
        {
            this.mana = MathHelper.Clamp(mana, 0, maxMana);
        }

        public void AddMana(int add)
        {
            this.mana = MathHelper.Clamp(this.mana + add, 0, maxMana);
        }

        public int GetMana()
        {
            return this.mana;
        }

        public void SetHeadRotation(float headYaw, float headPitch)
        {
            SetHeadYaw(headYaw);
            SetHeadPitch(headPitch);
        }

        public float GetHeadYaw()
        {
            return this.headYaw;
        }

        public void SetHeadYaw(float headYaw)
        {
            this.headYaw = MathHelper.WrapAngle(headYaw);
        }

        public float GetHeadPitch()
        {
            return this.headPitch;
        }

        public void SetHeadPitch(float headPitch)
        {
            this.headPitch = MathHelper.Clamp(headPitch, -MathHelper.PiOver2 + 0.001f, MathHelper.PiOver2 - 0.001f);
        }

        public override bool IsAlive()
        {
            return base.IsAlive() && this.health > 0;
        }

        public virtual void OnDeath()
        {
            world.PlaySound(GetDeathSound());
            RewardAttacker(lastAttacker);
            DropItems();
            Dismount();
        }

        public virtual int GetDamage()
        {
            return 0;
        }

        public void Damage(AttackInfo attackInfo)
        {
            int damage = attackInfo.GetDamage();
            AddHealth(-damage);
            this.lastAttacker = attackInfo.GetAttacker();

            string text = damage.ToString();

            if (damage == 0)
                text = "Miss";

            Color color = new Color(255, 155, 0);
            float hue = 0;

            if (attackInfo.IsCritical())
                hue = 1;

            world.Display3DText(text, GetPosition() - new Vector3(0, height / 2, 0), new Vector3(0, 2, 0), color, 3, 3, -1, (int)(Utils.TicksInSecond * 2.5F), 0, hue);
            attackedTicks = GetAttackedTicks();

            if (world is WorldServer)
            {
                NetEntity netEntity = ((WorldServer)world).GetNetEntityManager().GetNetEntity(this);

                if (netEntity is NetEntityLiving)
                    ((NetEntityLiving)netEntity).SetAttackInfo(attackInfo);
            }
        }

        public void SetLastAttacker(EntityLiving lastAttacker)
        {
            this.lastAttacker = lastAttacker;
        }

        public EntityLiving GetLastAttacker()
        {
            return this.lastAttacker;
        }

        private void RewardAttacker(Entity entity)
        {
            if (entity is EntityPlayer)
                ((EntityPlayer)entity).AddExp(expReward);
        }

        private void UpdateFallDamage()
        {
            if (!onGround)
            {
                if (yVelocity < 0)
                    this.fallVelocity = (float)yVelocity;
            }
            else
            {
                float fallVelocity = Math.Abs(this.fallVelocity);
                if (fallVelocity >= gravity)
                {
                    int damage = (int)-Math.Floor(Math.Pow(fallVelocity - gravity, 2.25));
                    AddHealth(damage);
                    this.fallVelocity = 0;
                }
            }
        }

        public virtual void Respawn()
        {
            SetHealth(maxHealth);
            SetMana(maxMana);
            SetYaw(0);
            SetPitch(0);
            SetHeadYaw(0);
            SetHeadPitch(0);
            SetVelocity(Vector3.Zero);
            SetMovement(Vector3.Zero);
        }

        public Entity GetLookAtEntity(float range = 256)
        {
            Ray ray = GetLookAtRay();

            for (int i = 0; i < world.entityList.Count; i++)
            {
                Entity entity = world.entityList[i];

                if (entity == this)
                    continue;

                float? rayRange = ray.Intersects(entity.GetBoundingBox());

                if (rayRange.HasValue)
                {
                    if (rayRange.Value <= range)
                        return entity;
                }
            }

            return null;
        }

        public Entity GetLookAtEntityFromList(IEnumerable<Entity> entities, float range = 256)
        {
            Ray ray = GetLookAtRay();

            for (int i = 0; i < world.entityList.Count; i++)
            {
                Entity entity = world.entityList[i];

                if (entity == this)
                    continue;

                float? rayRange = ray.Intersects(entity.GetBoundingBox());

                if (rayRange.HasValue)
                {
                    if (rayRange.Value <= range)
                        return entity;
                }
            }

            return null;
        }

        public virtual string GetDeathSound()
        {
            return null;
        }

        public override Vector3 GetModelScale()
        {
            return new Vector3(0.1F, 0.1F, 0.1F);
        }

        public List<EntityPet> GetPets()
        {
            return this.pets;
        }

        public void AddPet(EntityPet pet)
        {
            pet.SetOwner(this);
            this.pets.Add(pet);
        }

        public void RemovePet(EntityPet pet)
        {
            this.pets.Remove(pet);
        }

        public int GetBerkins()
        {
            return this.berkins;
        }

        public void SetBerkins(int berkins)
        {
            this.berkins = MathHelper.Clamp(berkins, 0, int.MaxValue);
        }

        public void AddBerkins(int add)
        {
            SetBerkins(berkins + add);
        }

        protected virtual void DropItems()
        { }

        protected void DropItem(Item item, int amount, double chance)
        {
            if (amount <= 0 || world.rand.NextDouble() >= chance / 100.0)
                return;

            amount = MathHelper.Clamp(amount, 0, item.GetMaxItemStack());
            world.SpawnEntity(new EntityDrop(world, x, y, z, item, amount));
        }

        public virtual int GetAttackedTicks()
        {
            return 1024;
        }

        public bool IsAttacked()
        {
            return this.lastAttacker != null;
        }

        private void UpdateIsAttacked()
        {
            attackedTicks--;

            if (attackedTicks <= 0)
                this.lastAttacker = null;
        }

        public virtual bool RenderHealthBar()
        {
            return true;
        }

        public void Jump()
        {
            if (HasMount())
                mount.Jump();
            else
            {
                if (isInWater)
                    this.yVelocity += jumpSpeed * 0.42F;

                else if (onGround)
                    this.yVelocity += jumpSpeed;
            }
        }

        public virtual float GetBaseJumpSpeed()
        {
            return 5.0F;
        }

        public virtual bool IsHostile()
        {
            return false;
        }

        public override void OnToggleNoClip()
        {
            base.OnToggleNoClip();

            if (noClip)
                this.fallVelocity = 0;
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.headYaw = yaw;
            this.health = reader.ReadInt32();
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(health);
        }
    }
}