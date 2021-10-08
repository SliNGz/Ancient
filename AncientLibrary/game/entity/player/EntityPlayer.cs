using ancient.game.world;
using ancient.game.world.block;
using ancientlib.game.classes;
using ancientlib.game.constants;
using ancientlib.game.entity.player;
using ancientlib.game.entity.world;
using ancientlib.game.init;
using ancientlib.game.inventory;
using ancientlib.game.item;
using ancientlib.game.item.equip.bottom;
using ancientlib.game.item.equip.special;
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

        protected InventoryPlayer inventory;

        protected int handSlot;
        protected ItemStack hand;
        protected ItemStack handDefault;

        protected int str;
        protected int wsd;
        protected int dex;
        protected int luk;

        protected Class _class;
        public Skill[] skillBar;

        public bool usingItemInHand;
        private bool animForward;
        public float handRenderYaw;
        public float handRenderPitch;
        public float handRenderRoll;

        protected bool isJumping;

        private int runningTicks;

        public bool usingSpecial;

        public Vector3 targetBlockPos;
        public float destroyAnimation;

        private int score;

        public EntityPlayer(World world) : base(world)
        {
            this.name = "UnnamedPlayer";

            this.maxHealth = 100;
            this.health = maxHealth;
            this.maxMana = 100;
            this.mana = maxMana;

            this.runningSpeed = 1.8F;

            this.renderDistance = 8;

            this.noClip = false;

            this.inventory = new InventoryPlayer(this, 40);
            AddItem(new ItemStack(Items.sword2));
            AddItem(new ItemStack(Items.staff));
            AddItem(new ItemStack(Items.woodenBow));
            AddItem(new ItemStack(Items.dagger));
            AddItem(new ItemStack(Items.steelArrow, 64));
            AddItem(new ItemStack(Items.stoneShovel));
            AddItem(new ItemStack(Items.stoneAxe));
            AddItem(new ItemStack(Items.stonePickaxe));

            this.handSlot = 0;

            this.str = 0;
            this.wsd = 0;
            this.dex = 0;
            this.luk = 0;

            SetClass(Classes.warrior);

            this.inputVector = Vector3.Zero;

            this.handRenderYaw = 0;
            this.handRenderPitch = 0;
            this.handRenderRoll = 0;

            this.level = 1;

            this.handDefault = new ItemStack(Items.handDefault);

            this.hairID = 1;
        }

        public EntityPlayer(World world, EntityPlayer player) : this(world)
        {
            CopyFrom(player);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateRunningParticles();
            UpdateSkills();
            UpdateItemInHand();
            UpdateItemChange();
            UpdateHandAnimation();
            UpdateBlockDestroyAnimation();
        }

        private void UpdateRunningParticles()
        {
            if (isRunning)
            {
                runningTicks++;

                if (runningTicks % 16 == 0)
                {
                    Block block = world.GetBlock((int)x, (int)Math.Ceiling(y - height - 1), (int)z);

                    if (block != null && block.IsSolid())
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

        private void UpdateHandAnimation()
        {
            if (hand == null)
            {
                usingItemInHand = false;
                return;
            }

            float delta = (1 / 64F) * hand.GetRenderSpeed(this);

            if (usingItemInHand)
            {
                float yawReach = hand.GetBaseRenderYaw();
                float pitchReach = hand.GetBaseRenderPitch();
                float rollReach = hand.GetBaseRenderRoll();

                if (animForward)
                {
                    yawReach = hand.GetRenderYaw();
                    pitchReach = hand.GetRenderPitch();
                    rollReach = hand.GetRenderRoll();
                }

                handRenderYaw = MathHelper.Lerp(handRenderYaw, yawReach, delta);
                handRenderPitch = MathHelper.Lerp(handRenderPitch, pitchReach, delta);
                handRenderRoll = MathHelper.Lerp(handRenderRoll, rollReach, delta);

                if (Math.Abs(yawReach - handRenderYaw) <= 0.005F)
                    handRenderYaw = yawReach;

                if (Math.Abs(pitchReach - handRenderPitch) <= 0.005F)
                    handRenderPitch = pitchReach;

                if (Math.Abs(rollReach - handRenderRoll) <= 0.005F)
                    handRenderRoll = rollReach;

                if (handRenderYaw == yawReach && handRenderPitch == pitchReach && handRenderRoll == rollReach)
                {
                    if (!hand.CanBeSpammed())
                        usingItemInHand = false;
                    else
                        animForward = !animForward;
                }
            }
            else
            {
                animForward = true;

                if (handRenderYaw != hand.GetBaseRenderYaw())
                    handRenderYaw = MathHelper.Lerp(handRenderYaw, hand.GetBaseRenderYaw(), delta);

                if (handRenderPitch != hand.GetBaseRenderPitch())
                    handRenderPitch = MathHelper.Lerp(handRenderPitch, hand.GetBaseRenderPitch(), delta);

                if (handRenderRoll != hand.GetBaseRenderRoll())
                    handRenderRoll = MathHelper.Lerp(handRenderRoll, hand.GetBaseRenderRoll(), delta);

                if (Math.Abs(hand.GetBaseRenderYaw() - handRenderYaw) <= 0.005F)
                    handRenderYaw = hand.GetBaseRenderYaw();

                if (Math.Abs(hand.GetBaseRenderPitch() - handRenderPitch) <= 0.005F)
                    handRenderPitch = hand.GetBaseRenderPitch();

                if (Math.Abs(hand.GetBaseRenderRoll() - handRenderRoll) <= 0.005F)
                    handRenderRoll = hand.GetBaseRenderRoll();
            }
        }

        private void UpdateBlockDestroyAnimation()
        {
            Vector3? position = GetTargetedBlockPosition();

            if (position.HasValue)
            {
                if (targetBlockPos != position.Value)
                    destroyAnimation = 0;
            }
            else
                destroyAnimation = 0;
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

        public void SetRenderDistance(int renderDistance)
        {
            this.renderDistance = renderDistance;
        }

        public void AddRenderDistance(int add)
        {
            SetRenderDistance(this.renderDistance + add);
        }

        public virtual bool AddItem(ItemStack itemStack)
        {
            return this.inventory.AddItemUnsafe(itemStack);
        }

        public bool AddItem(Item item, int amount)
        {
            return AddItem(new ItemStack(item, amount));
        }

        public virtual bool RemoveItem(ItemStack itemStack)
        {
            return this.inventory.RemoveItemUnsafe(itemStack);
        }

        public bool RemoveItem(Item item, int amount)
        {
            return RemoveItem(new ItemStack(item, amount));
        }

        public InventoryPlayer GetInventory()
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
            return this.hand == null ? handDefault : hand;
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

        public void OnUseItemInHandFinish()
        {
            if (hand != null)
                hand.OnUseFinish(this);
        }

        public void OnUseRightItemInHandFinish()
        {
            if (hand != null)
                hand.OnUseRightFinish(this);
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
            if (!world.IsRemote() && hand != null)
            {
                EntityDrop drop = new EntityDrop(world, x, y, z, hand.GetItem(), 1);
                drop.AddPosition(GetLookAt() * 1.5F);
                drop.SetVelocity(GetTotalVelocity());
                world.SpawnEntity(drop);
                RemoveItem(hand.GetItem(), 1);
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
                    this.hand.OnItemChange(this, prevHand);

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

        public void SetStr(int str)
        {
            this.str = str;
        }

        public void AddStr(int add)
        {
            SetStr(str + add);
        }

        public int GetWsd()
        {
            return this.wsd;
        }

        public void SetWsd(int wsd)
        {
            this.wsd = wsd;
        }

        public void AddWsd(int add)
        {
            SetWsd(wsd + add);
        }

        public int GetDex()
        {
            return this.dex;
        }

        public void SetDex(int dex)
        {
            this.dex = dex;
        }

        public void AddDex(int add)
        {
            SetDex(dex + add);
        }

        public int GetLuk()
        {
            return this.luk;
        }

        public void SetLuk(int luk)
        {
            this.luk = luk;
        }

        public void AddLuk(int add)
        {
            SetLuk(luk + add);
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
            return BiomeManager.GetBiomeAt((int)x, (int)z);
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

        public override string GetRendererName()
        {
            return "player";
        }

        public override string GetEntityName()
        {
            return "player";
        }

        public ItemBottom GetBottom()
        {
            return this.inventory.GetBottom();
        }

        public ItemSpecial GetSpecial()
        {
            return this.inventory.GetSpecial();
        }

        public void UseSpecial()
        {
            inventory.UseSpecial();
        }

        public virtual void Teleport(float x, float y, float z)
        {
            SetPosition(x, y, z);
        }

        public void Teleport(Vector3 position)
        {
            Teleport(position.X, position.Y, position.Z);
        }

        public int GetScore()
        {
            return this.score;
        }

        public void SetScore(int score)
        {
            this.score = score;
        }

        public void AddScore(int add)
        {
            this.score += add;
        }

        public void CopyFrom(EntityPlayer player)
        {
            this.name = player.GetName();
            this.maxHealth = player.GetMaxHealth();
            this.maxMana = player.GetMaxMana();
            this.level = player.GetLevel();
            this.exp = player.GetExp();
            this.str = player.GetStr();
            this.wsd = player.GetWsd();
            this.dex = player.GetDex();
            this.luk = player.GetLuk();
            this.skinColor = player.GetSkinColor();
            this.hairID = player.GetHairID();
            this.hairColor = player.GetHairColor();
            this.eyesID = player.GetEyesID();
            this.eyesColor = player.GetEyesColor();
            this._class = player.GetClass();
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.name = reader.ReadString();
            this.maxHealth = reader.ReadInt32();
            this.maxMana = reader.ReadInt32();
            this.level = reader.ReadInt32();
            this.exp = reader.ReadInt32();
            this.str = reader.ReadInt32();
            this.wsd = reader.ReadInt32();
            this.dex = reader.ReadInt32();
            this.luk = reader.ReadInt32();
            this.skinColor = reader.ReadColorRGB();
            this.hairID = reader.ReadByte();
            this.hairColor = reader.ReadColorRGB();
            this.eyesID = reader.ReadByte();
            this.eyesColor = reader.ReadColorRGB();
            this._class = Classes.GetClassFromID(reader.ReadInt32());
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(name);
            writer.Write(maxHealth);
            writer.Write(maxMana);
            writer.Write(level);
            writer.Write(exp);
            writer.Write(str);
            writer.Write(wsd);
            writer.Write(dex);
            writer.Write(luk);
            writer.WriteColorRGB(skinColor);
            writer.Write(hairID);
            writer.WriteColorRGB(hairColor);
            writer.Write(eyesID);
            writer.WriteColorRGB(eyesColor);
            writer.Write(Classes.GetIDFromClass(_class));
        }
    }
}