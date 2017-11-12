using ancient.game.world;
using ancient.game.world.block;
using ancientlib.game.classes;
using ancientlib.game.constants;
using ancientlib.game.entity.player;
using ancientlib.game.entity.world;
using ancientlib.game.init;
using ancientlib.game.inventory;
using ancientlib.game.item;
using ancientlib.game.item.projectile;
using ancientlib.game.particle;
using ancientlib.game.skill;
using ancientlib.game.utils;
using ancientlib.game.world.biome;
using Microsoft.Xna.Framework;
using System;
using System.IO;

namespace ancient.game.entity.player
{
    public class EntityPlayer : EntityPlayerBase
    {
        protected int renderDistance;

        protected Inventory inventory;

        protected int handSlot;
        protected ItemStack hand;
        protected ItemStack handDefault;

        protected int str;
        protected int wsd;
        protected int dex;
        protected int luk;

        protected Class _class;
        public Skill[] skillBar;

        public Vector3 inputVector;
        public bool updateSmoothStep;

        public bool usingItemInHand;
        public float handYaw;
        public float handPitch;
        public float handRoll;

        protected bool isJumping;

        private int runningTicks;

        public EntityPlayer(World world) : base(world)
        {
            this.name = "UnnamedPlayer";

            this.maxHealth = 250;
            this.health = maxHealth;
            this.maxMana = 250;
            this.mana = maxMana;

            this.runningSpeed = 1.8F;

            this.renderDistance = 8;

            this.noClip = false;

            this.inventory = new Inventory(40, 10);
            inventory.AddItem(new ItemStack(Items.dagger, 64));
            inventory.AddItem(new ItemStack(Items.staff, 64));
            inventory.AddItem(new ItemStack(Items.log, 64));
            inventory.AddItem(new ItemStack(Items.sand, 64));
            inventory.AddItem(new ItemStack(Items.woodenBow, 64));
            inventory.AddItem(new ItemStack(Items.blueberries_bush, 64));

            this.handSlot = 0;

            this.str = 0;
            this.wsd = 0;
            this.dex = 0;
            this.luk = 0;

            SetClass(Classes.bowman);

            this.inputVector = Vector3.Zero;

            this.handYaw = 0;
            this.handPitch = 0;
            this.handRoll = 0;

            this.level = 255;

            this.handDefault = new ItemStack(Items.air);
        }

        public override void Update(GameTime gameTime)
        {
            SetMovement(inputVector);
            UpdateRunningParticles();

            base.Update(gameTime);
            UpdateSkills();
            UpdateItemInHand();
            UpdateItemChange();
            UpdateHandRotation();
            inputVector = Vector3.Zero;
        }

        private void UpdateRunningParticles()
        {
            if (isRunning)
            {
                runningTicks++;

                if (runningTicks % 16 == 0)
                {
                    Block block = world.GetBlock((int)x, (int)Math.Ceiling(y - height - 1), (int)z);

                    if (block.IsSolid())
                    {
                        ParticleVoxel voxel = new ParticleVoxel(world);
                        voxel.SetPosition(GetPosition() + new Vector3(-width / 2F + (float)world.rand.NextDouble() * width, -height, 0));
                        voxel.SetScale(Vector3.One * world.rand.Next(7, 14) / 100F);
                        voxel.SetVelocity(new Vector3(-movement.X * 2, 3, -movement.Z * 2));
                        voxel.SetRotationVelocity(Vector3.One * world.rand.Next(7, 13));
                        voxel.gravity = World.GRAVITY;
                        voxel.SetColor(block.GetColor());
                        world.SpawnParticle(voxel);
                    }
                }
            }
        }

        private void UpdateSkills()
        {
            foreach (Skill skill in skillBar)
            {
                if (skill != null)
                    skill.Update();
            }
        }

        private void UpdateHandRotation()
        {
            if (hand == null)
            {
                usingItemInHand = false;
                return;
            }

            float delta = (1 / 64F) * hand.GetRenderSpeed(this);

            if (usingItemInHand)
            {
                if (handYaw != hand.GetRenderYaw())
                    handYaw = MathHelper.Lerp(handYaw, hand.GetRenderYaw(), delta);

                if (handPitch != hand.GetRenderPitch())
                    handPitch = MathHelper.Lerp(handPitch, hand.GetRenderPitch(), delta);

                if (handRoll != hand.GetRenderRoll())
                    handRoll = MathHelper.Lerp(handRoll, hand.GetRenderRoll(), delta);

                if (Math.Abs(hand.GetRenderYaw() - handYaw) <= 0.005F)
                    handYaw = hand.GetRenderYaw();

                if (Math.Abs(hand.GetRenderPitch() - handPitch) <= 0.005F)
                    handPitch = hand.GetRenderPitch();

                if (Math.Abs(hand.GetRenderRoll() - handRoll) <= 0.005F)
                    handRoll = hand.GetRenderRoll();

                if (handYaw == hand.GetRenderYaw() && handPitch == hand.GetRenderPitch() && handRoll == hand.GetRenderRoll())
                    usingItemInHand = false;
            }
            else
            {
                if (handYaw != 0)
                    handYaw = MathHelper.LerpPrecise(handYaw, 0, delta);

                if (handPitch != 0)
                    handPitch = MathHelper.LerpPrecise(handPitch, 0, delta);

                if (handRoll != 0)
                    handRoll = MathHelper.LerpPrecise(handRoll, 0, delta);
            }
        }

        public override float GetBaseSpeed()
        {
            return 2.2F;
        }

        public override float GetSpeedAddition()
        {
            return base.GetSpeedAddition() + this.dex / 256f;
        }

        public int GetRenderDistance()
        {
            return this.renderDistance;
        }

        public virtual bool AddItem(ItemStack itemStack)
        {
            return this.inventory.AddItem(itemStack);
        }

        public bool AddItem(Item item, int amount)
        {
            return AddItem(new ItemStack(item, amount));
        }

        public virtual bool RemoveItem(ItemStack itemStack)
        {
            return this.inventory.RemoveItem(itemStack);
        }

        public bool RemoveItem(Item item, int amount)
        {
            return RemoveItem(new ItemStack(item, amount));
        }

        public Inventory GetInventory()
        {
            return this.inventory;
        }

        public int GetHandSlot()
        {
            return this.handSlot;
        }

        public void SetHandSlot(int handSlot)
        {
            this.handSlot = handSlot;
        }

        public void SetItemInHand(ItemStack hand)
        {
            this.hand = hand;
        }

        public ItemStack GetItemInHand()
        {
            return this.hand;
        }

        public void UseItemInHand()
        {
            if (hand != null)
                hand.Use(this);
            else
                handDefault.Use(this);
        }

        public void UseItemInHandRightClick()
        {
            if (hand != null)
                hand.UseRightClick(this);
        }

        public void UseItem(ItemStack itemStack)
        {
            if (itemStack != null)
                itemStack.Use(this);
        }

        public void UseItemRightClick(ItemStack itemStack)
        {
            if (itemStack != null)
                itemStack.UseRightClick(this);
        }

        public override void DestroyTargetedBlock()
        {
            base.DestroyTargetedBlock();
            usingItemInHand = true;
        }

        public override int GetDamage()
        {
            return _class.GetDamage(this);
        }

        public AttackInfo GetAttackInfo()
        {
            return _class.GetAttackInfo(this);
        }

        public void DropItemInHand()
        {
            if (hand != null)
            {
                EntityDrop drop = new EntityDrop(world, x, y, z, hand.GetItem(), 1);
                drop.AddPosition(GetLookAt() * 1.5F);
                drop.SetVelocity(GetTotalVelocity());
                world.SpawnEntity(drop);
                inventory.RemoveItem(hand.GetItem(), 1);
            }
        }

        private void UpdateItemInHand()
        {
            if (hand == null)
                handDefault.Update();
            else
                hand.Update();
        }

        private void UpdateItemChange()
        {
            ItemStack prevHand = this.hand;
            this.hand = inventory.GetItemStackAt(handSlot);

            if (prevHand != this.hand)
            {
                if (hand != null)
                    this.hand.OnItemChange(this);

                world.OnPlayerChangeItemInHand(hand);
            }
        }

        public bool Interact()
        {
            Ray ray = GetLookAtRay();

            Entity interactedEntity = null;
            float minDistance = 8;

            for (int i = 0; i < world.entityList.Count; i++)
            {
                Entity entity = world.entityList[i];

                if (entity == this)
                    continue;

                float? intersect = ray.Intersects(entity.GetBoundingBox());

                if (intersect.HasValue)
                {
                    float distance = intersect.Value;

                    if (distance < minDistance)
                    {
                        distance = minDistance;
                        interactedEntity = entity;
                    }
                }
            }

            if (interactedEntity != null)
            {
                interactedEntity.OnPlayerInteraction(this);
                return true;
            }

            return false;
        }

        public int GetStr()
        {
            return this.str;
        }

        public int GetWsd()
        {
            return this.wsd;
        }

        public int GetDex()
        {
            return this.dex;
        }

        public int GetLuk()
        {
            return this.luk;
        }

        public float GetCriticalHitChance()
        {
            return MathHelper.Clamp(luk / 8192F, 0, 1);
        }

        public float GetCriticalHitPercentage()
        {
            return (float)Math.Pow(luk, 2.25) / 8192F;
        }

        public Class GetClass()
        {
            return this._class;
        }

        public void SetClass(Class _class)
        {
            this._class = _class;
            InitializeSkills();
        }

        private void InitializeSkills()
        {
            Type[] types = this._class.GetSkills();

            this.skillBar = new Skill[types.Length];

            for (int i = 0; i < types.Length; i++)
            {
                if (types[i] == null)
                    return;

                this.skillBar[i] = (Skill)Activator.CreateInstance(types[i], this);
            }
        }

        public void ActivateSkill(int slot)
        {
            Skill skill = skillBar[slot];

            if (skill == null)
                return;

            if (skill.CanActivate())
                skill.Activate();
        }

        public Biome GetBiome()
        {
            return BiomeManager.GetBiomeOfBlock((int)x, (int)z);
        }

        public override void Respawn()
        {
            base.Respawn();
            SetPosition(world.GetSpawnPoint() + new Vector3(0, height + 0.25F, 0));
        }

        public bool IsJumping()
        {
            return this.isJumping;
        }

        public void SetJumping(bool isJumping)
        {
            this.isJumping = isJumping;
        }

        public ItemProjectile GetProjectileInUse(ItemWeapon weapon)
        {
            ItemStack[] items = inventory.GetItems();
            Type weaponType = weapon.GetType();

            for (int i = 0; i < items.Length; i++)
            {
                ItemStack itemStack = items[i];

                if (itemStack != null)
                {
                    Item item = itemStack.GetItem();

                    if (item is ItemProjectile)
                    {
                        ItemProjectile projectile = (ItemProjectile)item;

                        if (projectile.GetWeaponType() == weaponType)
                            return projectile;
                    }
                }
            }

            return null;
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.name = reader.ReadString();
            this.skinColor = reader.ReadColorRGB();
            this.hairID = reader.ReadByte();
            this.hairColor = reader.ReadColorRGB();
            this.eyesID = reader.ReadByte();
            this.eyesColor = reader.ReadColorRGB();
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(name);
            writer.WriteColorRGB(skinColor);
            writer.Write(hairID);
            writer.WriteColorRGB(hairColor);
            writer.Write(eyesID);
            writer.WriteColorRGB(eyesColor);
        }
    }
}