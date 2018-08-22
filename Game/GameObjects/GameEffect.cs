﻿using ClassicUO.AssetsLoader;
using ClassicUO.Game.GameObjects.Interfaces;
using System.Collections.Generic;

namespace ClassicUO.Game.GameObjects
{
    public abstract class GameEffect : GameObject, IDeferreable
    {
        private readonly List<GameEffect> _children;

        protected GameEffect() : base(World.Map)
        {
            _children = new List<GameEffect>();
        }

        public IReadOnlyList<GameEffect> Children => _children;

        protected GameObject Source { get; set; }
        protected GameObject Target { get; set; }

        protected int SourceX { get; set; }
        protected int SourceY { get; set; }
        protected int SourceZ { get; set; }

        protected int TargetX { get; set; }
        protected int TargetY { get; set; }
        protected int TargetZ { get; set; }

        protected AnimDataFrame AnimDataFrame { get; set; }


        public int Speed { get; set; }
        public long LastChangeFrameTime { get; set; }
        public bool IsEnabled { get; set; }
        public Graphic AnimationGraphic { get; set; }

        public DeferredEntity DeferredObject { get; set; }


        public void Load()
        {
            AnimDataFrame = AnimData.CalculateCurrentGraphic(Graphic);
            IsEnabled = true;
            AnimIndex = (sbyte)AnimDataFrame.FrameStart;
            Speed = AnimDataFrame.FrameInterval * 45;
        }

        public virtual void UpdateAnimation(in double ms)
        {
            if (IsEnabled)
            {
                if (LastChangeFrameTime < World.Ticks)
                {
                    AnimationGraphic = (Graphic)(Graphic + AnimDataFrame.FrameData[AnimIndex]);
                    AnimIndex++;

                    if (AnimIndex >= AnimDataFrame.FrameCount)
                    {
                        AnimIndex = (sbyte)AnimDataFrame.FrameStart;
                    }

                    LastChangeFrameTime = World.Ticks + Speed;
                }
            }
            else if (Graphic != AnimationGraphic)
            {
                AnimationGraphic = Graphic;
            }
        }


        public void AddChildEffect(in GameEffect effect)
        {
            _children.Add(effect);
        }

        protected (int x, int y, int z) GetSource()
        {
            if (Source == null)
            {
                return (SourceX, SourceY, SourceZ);
            }

            return (Source.Position.X, Source.Position.Y, Source.Position.Z);
        }

        public void SetSource(in GameObject source)
        {
            Source = source;
        }

        public void SetSource(in int x, in int y, in int z)
        {
            Source = null;
            SourceX = x;
            SourceY = y;
            SourceZ = z;
        }

        protected (int x, int y, int z) GetTarget()
        {
            if (Target == null)
            {
                return (TargetX, TargetY, TargetZ);
            }

            return (Target.Position.X, Target.Position.Y, Target.Position.Z);
        }

        public void SetTarget(in GameObject target)
        {
            Target = target;
        }

        public void SetTarget(in int x, in int y, in int z)
        {
            Target = null;
            TargetX = x;
            TargetY = y;
            TargetZ = z;
        }

        public override void Dispose()
        {
            Source = null;
            Target = null;
            base.Dispose();
        }
    }
}