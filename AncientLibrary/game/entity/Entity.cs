using ancient.game.entity.player;
using ancient.game.utils;
using ancient.game.world;
using ancient.game.world.block;
using ancient.game.world.chunk;
using ancient.game.world.generator;
using ancientlib.game.constants;
using ancientlib.game.entity;
using ancientlib.game.entity.model;
using ancientlib.game.entity.player;
using ancientlib.game.network.packet.server.entity;
using ancientlib.game.network.packet.server.player;
using ancientlib.game.utils;
using ancientlib.game.world;
using ancientlib.game.world.biome;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace ancient.game.entity
{
    public abstract class Entity
    {
        public static int nextEntityID = 0;

        protected World world;

        protected int id;

        protected float x;
        protected float y;
        protected float z;

        protected float xVelocity;
        protected float yVelocity;
        protected float zVelocity;

        protected Vector3 movement;

        protected float yaw;
        protected float pitch;
        protected float roll;

        protected float yawVelocity;
        protected float pitchVelocity;
        protected float rollVelocity;

        protected float yawReach;
        protected float pitchReach;
        protected float rollReach;
        protected float lookAtDestSpeed;

        protected float width;
        protected float height;
        protected float length;

        protected BoundingBox boundingBox;
        public float gravity;

        protected float deltaTime;

        protected float speed;

        protected bool noClip;

        protected float xServer;
        protected float yServer;
        protected float zServer;

        protected bool onGround;

        protected bool inWater;
        protected bool inWaterLower;

        protected float slipperiness;
        protected float speedModifier;

        protected Color colorMultiply;

        protected int ticksExisted;
        protected int lifeSpan;

        /* How many ticks it will take the particle to fade in. */
        protected int fadeInTicks;

        /* How many ticks it will take the particle to fade out - (ends at death tick, starts at (lifeSpan - fadeTicks)). */
        protected int fadeOutTicks;

        protected EntityMount mount;

        /* Whether or not this entity will interact in any way with blocks. */
        protected bool interactWithBlocks;

        /* Whether or not this entity will interact in any way with entities. */
        protected bool interactWithEntities;

        protected EntityModel lastModel;
        protected EntityModel model;

        protected int animationTicks;
        public int animationIndex;

        protected Entity(World world)
        {
            this.world = world;

            this.id = nextEntityID;
            nextEntityID++;

            this.x = 0;
            this.y = 0;
            this.z = 0;

            this.gravity = World.GRAVITY;

            this.yawReach = MathHelper.TwoPi;
            this.pitchReach = MathHelper.TwoPi;
            this.rollReach = MathHelper.TwoPi;

            this.slipperiness = 0;
            this.speedModifier = 1;

            this.colorMultiply = Color.White;

            this.lifeSpan = 76800;

            this.interactWithBlocks = true;
            this.interactWithEntities = true;

            if (GetModelCollection() != null)
            {
                this.model = GetModelCollection().GetStandingModel();
                SetDimensions(model.GetWidth(), model.GetHeight(), model.GetLength());
                this.lastModel = model;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            this.deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * 2;
            ticksExisted++;

            /*    if (GetModelCollection() != null)
                {
                    if (movement.Z != 0 || world.IsRemote() && GetServerPosition() != GetPosition() && this != world.GetMyPlayer() && this != world.GetMyPlayer().GetMount())
                    {
                        EntityModelAnimation walking = GetModelCollection().GetWalkingAnimationModel();

                        if (walking != null)
                        {
                            if (model != walking)
                                this.model = walking;

                            if ((animationTicks++) % walking.GetAnimationTickRate() == 0)
                                animationIndex = (animationIndex + 1) % walking.GetAnimationCount();
                        }
                    }
                    else
                    {
                        if (model == GetModelCollection().GetWalkingAnimationModel())
                        {
                            this.model = GetModelCollection().GetStandingModel();
                            animationTicks = 0;
                        }
                    }
                }*/

            if (ShouldInterpolate())
            {
                Interpolate();
                return;
            }

            this.slipperiness = 0;
            this.speedModifier = 1;
            this.inWater = false;
            this.inWaterLower = false;
            this.speed = GetBaseSpeed() + GetSpeedAddition();

            UpdateVelocity();
            UpdateRotation();
            Move(xVelocity + movement.X, yVelocity + movement.Y, zVelocity + movement.Z);
            CollideWithBlocks();
            CollideWithEntities();
            UpdateOnGround();
            UpdateMovement();
            UpdateAlpha();
            UpdateModel();

            this.lastModel = model;
        }

        protected void UpdateMovement()
        {
            movement *= slipperiness;

            if (Math.Abs(movement.X) < 0.005)
                movement.X = 0;

            if (Math.Abs(movement.Y) < 0.005)
                movement.Y = 0;

            if (Math.Abs(movement.Z) < 0.005)
                movement.Z = 0;
        }

        public Vector3 GetMovement()
        {
            return this.movement;
        }

        public virtual void SetMovement(Vector3 movement)
        {
            if (movement == Vector3.Zero)
                return;

            if (IsRiding())
                mount.SetMovement(movement);
            else
            {
                this.movement = Vector3.Transform(movement, Matrix.CreateFromYawPitchRoll(yaw, pitch, roll));
                this.movement.Normalize();
                this.movement *= speed * speedModifier;
            }
        }

        protected virtual void UpdateVelocity()
        {
            float multiplier = 0.98F;

            this.xVelocity *= multiplier;
            this.zVelocity *= multiplier;

            if (noClip)
                this.yVelocity *= multiplier;

            if (Math.Abs(xVelocity) < 0.005)
                xVelocity = 0;

            if (Math.Abs(yVelocity) < 0.005)
                yVelocity = 0;

            if (Math.Abs(zVelocity) < 0.005)
                zVelocity = 0;
        }

        protected virtual void UpdateRotation()
        {
            this.yaw += yawVelocity * (float)deltaTime;
            this.pitch += pitchVelocity * (float)deltaTime;
            this.roll += rollVelocity * (float)deltaTime;

            if (this.yawReach != MathHelper.TwoPi)
            {
                if (yaw != yawReach)
                {
                    SetYaw(MathHelper.Lerp(yaw, yawReach, (float)deltaTime * lookAtDestSpeed));

                }
                else
                    this.yawReach = MathHelper.TwoPi;
            }

            if (this.pitchReach != MathHelper.TwoPi)
            {
                if (pitch != pitchReach)
                {
                    SetPitch(MathUtils.CurveAngle(pitch, pitchReach, (float)deltaTime * lookAtDestSpeed));

                    if (Math.Abs(pitch - pitchReach) < 0.01)
                        SetPitch(pitchReach);
                }
                else
                    this.pitchReach = MathHelper.TwoPi;
            }

            if (this.rollReach != MathHelper.TwoPi)
            {
                if (roll != rollReach)
                {
                    SetRoll(MathUtils.CurveAngle(roll, rollReach, (float)deltaTime * lookAtDestSpeed));

                    if (Math.Abs(roll - rollReach) < 0.01)
                        SetRoll(roll);
                }
                else
                    this.rollReach = MathHelper.TwoPi;
            }
        }

        public void Move(float x, float y, float z)
        {
            if (x != 0 || y != 0 || z != 0)
                AddPosition(x * deltaTime, y * deltaTime, z * deltaTime);
        }

        public World GetWorld()
        {
            return this.world;
        }

        public void SetWorld(World world)
        {
            this.world = world;
        }

        public int GetID()
        {
            return this.id;
        }

        public void SetID(int id)
        {
            this.id = id;
        }

        public Vector3 GetPosition()
        {
            return new Vector3(this.x, this.y, this.z);
        }

        public void SetPosition(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            UpdateBoundingBox();
        }

        public void SetPosition(Vector3 position)
        {
            SetPosition(position.X, position.Y, position.Z);
        }

        public void AddPosition(float x, float y, float z)
        {
            SetPosition(this.x + x, this.y + y, this.z + z);
        }

        public void AddPosition(Vector3 add)
        {
            AddPosition(add.X, add.Y, add.Z);
        }

        public void SetX(float x)
        {
            SetPosition(x, this.y, this.z);
        }

        public float GetX()
        {
            return this.x;
        }

        public void SetY(float y)
        {
            SetPosition(this.x, y, this.z);
        }

        public float GetY()
        {
            return this.y;
        }

        public void SetZ(float z)
        {
            SetPosition(this.x, this.y, z);
        }

        public float GetZ()
        {
            return this.z;
        }

        public Vector3 GetVelocity()
        {
            return new Vector3(this.xVelocity, this.yVelocity, this.zVelocity);
        }

        public void SetVelocity(float xVelocity, float yVelocity, float zVelocity)
        {
            this.xVelocity = xVelocity;
            this.yVelocity = yVelocity;
            this.zVelocity = zVelocity;
        }

        public void SetVelocity(Vector3 velocity)
        {
            SetVelocity(velocity.X, velocity.Y, velocity.Z);
        }

        public void AddVelocity(float xVelocity, float yVelocity, float zVelocity)
        {
            SetVelocity(this.xVelocity + xVelocity, this.yVelocity + yVelocity, this.zVelocity + zVelocity);
        }

        public void AddVelocity(Vector3 velocity)
        {
            AddVelocity(velocity.X, velocity.Y, velocity.Z);
        }

        public float GetXVelocity()
        {
            return this.xVelocity;
        }

        public void SetXVelocity(float xVelocity)
        {
            this.xVelocity = xVelocity;
        }

        public void AddXVelocity(float add)
        {
            this.xVelocity += add;
        }

        public float GetYVelocity()
        {
            return this.yVelocity;
        }

        public void SetYVelocity(float yVelocity)
        {
            this.yVelocity = yVelocity;
        }

        public void AddYVelocity(float add)
        {
            this.yVelocity += add;
        }

        public float GetZVelocity()
        {
            return this.zVelocity;
        }

        public void SetZVelocity(float zVelocity)
        {
            this.zVelocity = zVelocity;
        }

        public void AddZVelocity(float add)
        {
            this.zVelocity += add;
        }

        public void SetRotation(float yaw, float pitch, float roll)
        {
            SetYaw(yaw);
            SetPitch(pitch);
            SetRoll(roll);
        }

        public float GetYaw()
        {
            return this.yaw;
        }

        public void SetYaw(float yaw)
        {
            this.yaw = MathHelper.WrapAngle(yaw);
        }

        public void AddYaw(float add)
        {
            SetYaw(this.yaw + add);
        }

        public virtual float GetPitch()
        {
            return this.pitch;
        }

        public void SetPitch(float pitch)
        {
            this.pitch = MathHelper.Clamp(pitch, -MathHelper.PiOver2 + 0.001f, MathHelper.PiOver2 - 0.001f);
        }

        public void AddPitch(float add)
        {
            SetPitch(this.pitch + add);
        }

        public float GetRoll()
        {
            return this.roll;
        }

        public void SetRoll(float roll)
        {
            this.roll = MathHelper.WrapAngle(roll);
        }

        public void AddRoll(float add)
        {
            SetRoll(this.roll + add);
        }

        public void SetYawReach(float yaw)
        {
            this.yawReach = MathHelper.WrapAngle(yaw);
        }

        public void SetPitchReach(float pitch)
        {
            this.pitchReach = MathHelper.Clamp(pitch, -MathHelper.PiOver2 + 0.001f, MathHelper.PiOver2 - 0.001f);
        }

        public void SetRollReach(float roll)
        {
            this.rollReach = MathHelper.WrapAngle(roll);
        }

        public virtual Vector3 GetLookAt()
        {
            return Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0));
        }

        public virtual void SetLookAt(double x, double y, double z)
        {
            Vector3 rotation = MathUtils.GetRotationFromPosition(this.x, this.y, this.z, x, y, z);
            SetRotation(rotation.X, rotation.Y, rotation.Z);
        }

        public void SetLookAt(Entity entity)
        {
            SetLookAt(entity.GetX(), entity.GetY(), entity.GetZ());
        }

        public void SetLookAtDestination(double x, double y, double z, float lookAtDestSpeed = 2)
        {
            this.lookAtDestSpeed = lookAtDestSpeed;
            SetYawReach(MathUtils.GetRotationFromPosition(this.x, this.y, this.z, x, y, z).X);
        }

        public float GetWidth()
        {
            return this.width;
        }

        public float GetHeight()
        {
            return this.height;
        }

        public float GetLength()
        {
            return this.length;
        }

        public BoundingBox GetBoundingBox()
        {
            return this.boundingBox;
        }

        public float GetSpeed()
        {
            return this.speed;
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        public void AddSpeed(float add)
        {
            this.speed += add;
        }

        public void SetDimensions(float width, float height, float length)
        {
            this.width = width;
            this.height = height;
            this.length = length;
            UpdateBoundingBox();
        }

        public void SetDimensions(Vector3 dimesions)
        {
            SetDimensions(dimesions.X, dimesions.Y, dimesions.Z);
        }

        public virtual void UpdateBoundingBox()
        {
            boundingBox.Min = GetPosition() + new Vector3(-width / 2, -height - 0.001F, -length / 2);
            boundingBox.Max = GetPosition() + new Vector3(width / 2, 0, length / 2);
        }

        protected void CollideWithBlocks()
        {
            if (!interactWithBlocks)
                return;

            foreach (KeyValuePair<BoundingBox, Block> pair in GetCollidedBlocks())
                OnCollisionWithBlock(pair.Key, pair.Value);
        }

        protected virtual bool CanCollideWithBlockBoundingBox(Block block)
        {
            return !noClip;
        }

        protected virtual void OnCollisionWithBlock(BoundingBox blockBB, Block block)
        {
            Block blockAbove = world.GetBlock((int)blockBB.Min.X, (int)blockBB.Min.Y + 1, (int)blockBB.Min.Z);

            if (block.IsSolid() && CanCollideWithBlockBoundingBox(block))
            {
                OnCollideWithBoundingBox(blockBB);
                block.OnCollide(this, blockBB);

                if (CanAutoClimb())
                {
                    if (blockAbove != null && ((!blockAbove.IsSolid() && blockBB.Min.Y + 1 == Math.Round(this.y - height + 1)) || block.IsSolid() && block.GetDimensions().Y <= 0.2F))
                    {
                        if (this is EntityLiving)
                        {
                            this.yVelocity = 0;
                            this.y = (int)blockBB.Min.Y + block.GetDimensions().Y + height;
                            UpdateBoundingBox();
                        }
                    }
                }
            }

            if (block is BlockWater)
            {
                if (blockBB.Max.Y > this.y - height)
                    this.inWater = true;

                if (blockBB.Max.Y <= this.y - height + 1 && !(blockAbove is BlockWater))
                    inWaterLower = true;
            }
        }

        protected void CollideWithEntities()
        {
            if (!interactWithEntities)
                return;

            foreach (Entity entity in GetCollidedEntities().Keys)
                OnCollideWithEntity(entity);
        }

        protected virtual bool CanCollideWithEntityBoundingBox(Entity entity)
        {
            return true;
        }

        protected virtual void OnCollideWithEntity(Entity entity)
        {
            if (CanCollideWithEntityBoundingBox(entity) && entity.CanCollideWithEntityBoundingBox(this) && !noClip)
                OnCollideWithBoundingBox(entity.GetBoundingBox());
        }

        public void OnCollideWithBoundingBox(BoundingBox box)
        {
            Vector3 penetration = boundingBox.GetPenetration(box);

            if (penetration.Y < Math.Min(penetration.X, penetration.Z))
            {
                UpdateOnGround();
                this.yVelocity = 0;

                if (box.Min.Y < boundingBox.Min.Y && onGround)
                    this.y = box.Max.Y + height;
                else if (box.Max.Y > boundingBox.Max.Y)
                    this.y = box.Min.Y;
            }
            else
            {
                if (penetration.X < penetration.Z)
                {
                    this.xVelocity = 0;

                    if (boundingBox.Max.X > box.Max.X)
                        this.x = box.Max.X + width / 2;
                    else if (boundingBox.Min.X < box.Min.X)
                        this.x = box.Min.X - width / 2;
                }

                else if (penetration.Z < penetration.X)
                {
                    this.zVelocity = 0;

                    if (boundingBox.Max.Z > box.Max.Z)
                        this.z = box.Max.Z + length / 2;
                    else if (boundingBox.Min.Z < box.Min.Z)
                        this.z = box.Min.Z - length / 2;
                }
            }

            UpdateBoundingBox();
        }

        public bool OnGround()
        {
            return this.onGround;
        }

        public void SetOnGround(bool onGround)
        {
            this.onGround = onGround;
        }

        protected void UpdateOnGround()
        {
            this.onGround = false;

            if (IsRiding())
            {
                if (mount.onGround)
                    this.onGround = true;
            }
            else
            {
                int xMin = (int)Math.Floor(boundingBox.Min.X);
                int yMin = (int)Math.Floor(boundingBox.Min.Y) - 1;
                int zMin = (int)Math.Floor(boundingBox.Min.Z);

                int xMax = (int)Math.Ceiling(boundingBox.Max.X);
                int yMax = yMin + 1;
                int zMax = (int)Math.Ceiling(boundingBox.Max.Z);

                for (int x = xMin; x < xMax; x++)
                {
                    for (int y = yMin; y <= yMax; y++)
                    {
                        for (int z = zMin; z < zMax; z++)
                        {
                            Block block = world.GetBlock(x, y, z);

                            if (block != null && block.IsSolid() && IsBlockGround(block))
                            {
                                BoundingBox blockBB = world.GetBlockBoundingBox(block, x, y, z);

                                if (boundingBox.Intersects(blockBB))
                                {
                                    bool onGround = true;

                                    for (int i = y + 1; i < Math.Ceiling(boundingBox.Max.Y); i++)
                                    {
                                        Block blockAboveFeet = world.GetBlock(x, i, z);
                                        if (blockAboveFeet != null && blockAboveFeet.IsSolid() && blockAboveFeet.GetDimensions().Y > 0.2F)
                                        {
                                            onGround = false;
                                            break;
                                        }
                                    }

                                    if (onGround)
                                    {
                                        this.onGround = true;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (!onGround && !noClip)
            {
                if (inWater)
                    this.yVelocity -= gravity * 0.07F * deltaTime;
                else
                    this.yVelocity -= gravity * deltaTime;
            }
        }

        protected virtual bool IsBlockGround(Block block)
        {
            return true;
        }

        public bool InWater()
        {
            return this.inWater;
        }

        public Chunk GetChunk()
        {
            return world.GetChunk((int)Math.Floor(x / 16), (int)Math.Floor(y / 16), (int)Math.Floor(z / 16));
        }

        public bool HasNoClip()
        {
            return this.noClip;
        }

        public void SetNoClip(bool noClip)
        {
            this.noClip = noClip;

            OnToggleNoClip();
        }

        public virtual void OnToggleNoClip()
        {
            if (noClip)
                this.yVelocity = 0;

            if (IsRiding())
                mount.SetNoClip(noClip);
        }

        public void SetServerPosition(float x, float y, float z)
        {
            this.xServer = x;
            this.yServer = y;
            this.zServer = z;
        }

        public void SetServerPosition(Vector3 position)
        {
            SetServerPosition(position.X, position.Y, position.Z);
        }

        public void SetXServer(float x)
        {
            this.xServer = x;
        }

        public float GetXServer()
        {
            return this.xServer;
        }

        public void SetYServer(float y)
        {
            this.yServer = y;
        }

        public float GetYServer()
        {
            return this.yServer;
        }

        public void SetZServer(float z)
        {
            this.zServer = z;
        }

        public float GetZServer()
        {
            return this.zServer;
        }

        public void AddServerPosition(float x, float y, float z)
        {
            SetServerPosition(xServer + x, yServer + y, zServer + z);
        }

        public Vector3 GetServerPosition()
        {
            return new Vector3((float)xServer, (float)yServer, (float)zServer);
        }

        public List<BoundingBox> GetCollidedBoundingBoxes()
        {
            List<BoundingBox> collidedBoxes = new List<BoundingBox>();
            collidedBoxes.AddRange(GetCollidedBlocks().Keys);
            collidedBoxes.AddRange(GetCollidedEntities().Values);

            return collidedBoxes;
        }

        public Dictionary<BoundingBox, Block> GetCollidedBlocks()
        {
            Dictionary<BoundingBox, Block> collidedBlocks = new Dictionary<BoundingBox, Block>();

            int xMin = (int)Math.Floor(boundingBox.Min.X);
            int yMin = (int)Math.Floor(boundingBox.Min.Y);
            int zMin = (int)Math.Floor(boundingBox.Min.Z);

            int xMax = (int)Math.Ceiling(boundingBox.Max.X);
            int yMax = (int)Math.Ceiling(boundingBox.Max.Y);
            int zMax = (int)Math.Ceiling(boundingBox.Max.Z);

            for (int x = xMin; x < xMax; x++)
            {
                for (int y = yMin; y < yMax; y++)
                {
                    for (int z = zMin; z < zMax; z++)
                    {
                        Block block = world.GetBlock(x, y, z);
                        if (block == null)
                            continue;

                        BoundingBox boundingBox = world.GetBlockBoundingBox(block, x, y, z);

                        if (this.boundingBox.Intersects(boundingBox))
                            collidedBlocks.Add(boundingBox, block);
                    }
                }
            }

            return collidedBlocks;
        }

        //Returns a list of entities and their bounding boxes, doesn't eliminate entities which "can not collide" with this entity.
        public virtual Dictionary<Entity, BoundingBox> GetCollidedEntities()
        {
            Dictionary<Entity, BoundingBox> collidedEntities = new Dictionary<Entity, BoundingBox>();

            for (int i = 0; i < world.entityList.Count; i++)
            {
                Entity entity = world.entityList[i];

                if (entity != this)
                {
                    if (boundingBox.Intersects(entity.GetBoundingBox()))
                        collidedEntities.Add(entity, entity.GetBoundingBox());
                }
            }

            return collidedEntities;
        }

        public Vector3 GetTotalVelocity()
        {
            return this.GetVelocity() + this.movement;
        }

        public void SetMaximumSlipperiness(float slipperiness)
        {
            if (slipperiness > this.slipperiness)
                this.slipperiness = slipperiness;
        }

        public float GetSpeedModifier()
        {
            return this.speedModifier;
        }

        public void SetMinimumSpeedModifier(float speedModifier)
        {
            if (speedModifier < this.speedModifier)
                this.speedModifier = speedModifier;
        }

        public virtual Color GetMultiplyColor()
        {
            return this.colorMultiply;
        }

        public int GetTicksExisted()
        {
            return this.ticksExisted;
        }

        public int GetLifeSpan()
        {
            return this.lifeSpan;
        }

        public void SetLifeSpan(int lifeSpan)
        {
            this.lifeSpan = lifeSpan;
        }

        public virtual bool IsAlive()
        {
            return this.ticksExisted < lifeSpan;
        }

        public float GetTemperature()
        {
            Chunk chunk = GetChunk();
            if (chunk == null)
                throw new InvalidOperationException("Chunk is null");

            return BiomeManager.GetTemperature((int)x, (int)z);
        }

        protected void UpdateAlpha()
        {
            if (ticksExisted <= fadeInTicks)
                this.colorMultiply.A = (byte)((ticksExisted / (float)fadeInTicks) * 255);
            else if (ticksExisted >= lifeSpan - fadeOutTicks)
                this.colorMultiply.A = (byte)(((lifeSpan - ticksExisted) / (float)fadeOutTicks) * 255);
        }

        public virtual bool ShouldDespawn()
        {
            return !IsAlive() || this.y <= -16;
        }

        public Vector3 GetFootPosition()
        {
            return this.GetPosition() + new Vector3(0, -height, 0);
        }

        public virtual void OnPlayerInteraction(EntityPlayer player)
        { }

        public EntityMount GetMount()
        {
            return this.mount;
        }

        public void Mount(EntityMount mount)
        {
            if (mount == null)
                return;

            this.mount = mount;

            this.mount.SetRidingEntity(this);
            SetPosition(GetPosition() + this.mount.GetMountOffset());
            this.mount.SetNoClip(noClip);
            this.model = GetModelCollection().GetSittingModel();
        }

        public void Dismount()
        {
            if (this.mount != null)
            {
                this.mount.SetRidingEntity(null);
                this.mount.SetNoClip(false);
                SetPosition(GetPosition() + new Vector3(width, 0, length) + new Vector3(world.rand.Next(1, 3), 0, world.rand.Next(1, 3)));
                this.model = GetModelCollection().GetStandingModel();

                this.mount = null;
            }
        }

        public abstract Vector3 GetModelScale();

        public virtual float GetBaseSpeed()
        {
            return 1.0F;
        }

        public virtual float GetSpeedAddition()
        {
            return 0;
        }

        public EntityPlayer GetNearestPlayer(float range)
        {
            float minDistance = -1;
            EntityPlayer nearestPlayer = null;

            for (int i = 0; i < world.players.Count; i++)
            {
                EntityPlayer player = world.players[i];

                float distance = Vector3.Distance(GetPosition(), player.GetPosition());

                if (distance > range || player == this)
                    continue;

                if (minDistance == -1)
                {
                    minDistance = distance;
                    nearestPlayer = player;
                }
                else
                {
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestPlayer = player;
                    }
                }
            }

            return nearestPlayer;
        }

        public EntityLiving GetNearestLiving(float range)
        {
            float minDistance = -1;
            EntityLiving nearestLiving = null;

            for (int i = 0; i < world.entityList.Count; i++)
            {
                Entity entity = world.entityList[i];

                if (!(entity is EntityLiving) || entity == this)
                    continue;

                EntityLiving living = (EntityLiving)entity;

                float distance = Vector3.Distance(GetPosition(), living.GetPosition());

                if (distance > range)
                    continue;

                if (minDistance == -1)
                {
                    minDistance = distance;
                    nearestLiving = living;
                }
                else
                {
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestLiving = living;
                    }
                }
            }

            return nearestLiving;
        }

        protected virtual bool ShouldInterpolate()
        {
            return world.IsRemote() && this != world.GetMyPlayer();
        }

        protected void Interpolate()
        {
            this.x = MathHelper.Lerp(x, xServer, deltaTime * NetConstants.INTERP_MULTIPLIER);
            this.y = MathHelper.Lerp(y, yServer, deltaTime * NetConstants.INTERP_MULTIPLIER);
            this.z = MathHelper.Lerp(z, zServer, deltaTime * NetConstants.INTERP_MULTIPLIER);

            if (Math.Abs(xServer - x) < 0.005)
                x = xServer;

            if (Math.Abs(yServer - y) < 0.005)
                y = yServer;

            if (Math.Abs(zServer - z) < 0.005)
                z = zServer;
        }

        public virtual bool IsMultiplayerSupported()
        {
            return true;
        }

        public virtual bool ShouldSendPosition()
        {
            return true;
        }

        public virtual bool ShouldSendRotation()
        {
            return true;
        }

        public bool IsRiding()
        {
            return this.mount != null;
        }

        public float GetYawVelocity()
        {
            return this.yawVelocity;
        }

        public void SetYawVelocity(float yawVelocity)
        {
            this.yawVelocity = yawVelocity;
        }

        public float GetPitchVelocity()
        {
            return this.pitchVelocity;
        }

        public void SetPitchVelocity(float pitchVelocity)
        {
            this.pitchVelocity = pitchVelocity;
        }

        public float GetRollVelocity()
        {
            return this.rollVelocity;
        }

        public void SetRollVelocity(float rollVelocity)
        {
            this.rollVelocity = rollVelocity;
        }

        public void SetRotationVelocity(float yawVelocity, float pitchVelocity, float rollVelocity)
        {
            this.yawVelocity = yawVelocity;
            this.pitchVelocity = pitchVelocity;
            this.rollVelocity = rollVelocity;
        }

        public void SetRotationVelocity(Vector3 rotationVelocity)
        {
            SetRotationVelocity(rotationVelocity.X, rotationVelocity.Y, rotationVelocity.Z);
        }

        public abstract string GetEntityName();

        public virtual string GetRendererName()
        {
            return "entity";
        }

        public abstract EntityModelCollection GetModelCollection();

        public EntityModel GetModel()
        {
            return this.model;
        }

        public void SetModel(EntityModel model)
        {
            this.model = model;
        }

        public virtual string GetModelName()
        {
            if (model != null)
                return this.model.GetModelName();

            return "";
        }

        private void UpdateModel()
        {
            if (model == null && GetModelCollection() != null)
                this.model = GetModelCollection().GetStandingModel();

            if (lastModel != model)
                SetDimensions(model.GetWidth(), model.GetHeight(), model.GetLength());
        }

        public virtual bool CanAutoClimb()
        {
            return false;
        }

        public virtual void Read(BinaryReader reader)
        {
            this.x = reader.ReadSingle();
            this.y = reader.ReadSingle();
            this.z = reader.ReadSingle();
            this.xServer = x;
            this.yServer = y;
            this.zServer = z;
            this.yaw = reader.ReadSingle();
        }

        public virtual void Write(BinaryWriter writer)
        {
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
            writer.Write(yaw);
        }
    }
}
