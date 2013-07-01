﻿using System;
using System.Linq;
using UnityEngine;

namespace Kethane
{
    public class KethaneParticleEmitter : PartModule
    {
        #region Particle property bindings

        public float AngularVelocity
        {
            get { return emitter.angularVelocity; }
            set { emitter.angularVelocity = value; }
        }

        public float CameraVelocityScale
        {
            get { return renderer.cameraVelocityScale; }
            set { renderer.cameraVelocityScale = value; }
        }

        public Color ColorAnimation1
        {
            get { return animator.colorAnimation[0]; }
            set
            {
                var colors = animator.colorAnimation;
                colors[0] = value;
                animator.colorAnimation = colors;
            }
        }

        public Color ColorAnimation2
        {
            get { return animator.colorAnimation[1]; }
            set
            {
                var colors = animator.colorAnimation;
                colors[1] = value;
                animator.colorAnimation = colors;
            }
        }

        public Color ColorAnimation3
        {
            get { return animator.colorAnimation[2]; }
            set
            {
                var colors = animator.colorAnimation;
                colors[2] = value;
                animator.colorAnimation = colors;
            }
        }

        public Color ColorAnimation4
        {
            get { return animator.colorAnimation[3]; }
            set
            {
                var colors = animator.colorAnimation;
                colors[3] = value;
                animator.colorAnimation = colors;
            }
        }

        public Color ColorAnimation5
        {
            get { return animator.colorAnimation[4]; }
            set
            {
                var colors = animator.colorAnimation;
                colors[4] = value;
                animator.colorAnimation = colors;
            }
        }

        public float Damping
        {
            get { return animator.damping; }
            set { animator.damping = value; }
        }

        public bool Emit
        {
            get { return emitter.emit; }
            set { emitter.emit = value; }
        }

        public float EmitterVelocityScale
        {
            get { return emitter.emitterVelocityScale; }
            set { emitter.emitterVelocityScale = value; }
        }

        public Vector3 Force
        {
            get { return animator.force; }
            set { animator.force = value; }
        }

        public float LengthScale
        {
            get { return renderer.lengthScale; }
            set { renderer.lengthScale = value; }
        }

        public Vector3 LocalRotationAxis
        {
            get { return animator.localRotationAxis; }
            set { animator.localRotationAxis = value; }
        }

        public Vector3 LocalVelocity
        {
            get { return emitter.localVelocity; }
            set { emitter.localVelocity = value; }
        }

        public float MaxEmission
        {
            get { return emitter.maxEmission; }
            set { emitter.maxEmission = value; }
        }

        public float MaxEnergy
        {
            get { return emitter.maxEnergy; }
            set { emitter.maxEnergy = value; }
        }

        public float MaxParticleSize
        {
            get { return renderer.maxParticleSize; }
            set { renderer.maxParticleSize = value; }
        }

        public float MaxSize
        {
            get { return emitter.maxSize; }
            set { emitter.maxSize = value; }
        }

        public float MinEmission
        {
            get { return emitter.minEmission; }
            set { emitter.minEmission = value; }
        }

        public float MinEnergy
        {
            get { return emitter.minEnergy; }
            set { emitter.minEnergy = value; }
        }

        public float MinSize
        {
            get { return emitter.minSize; }
            set { emitter.minSize = value; }
        }

        public float RandomAngularVelocity
        {
            get { return emitter.rndAngularVelocity; }
            set { emitter.rndAngularVelocity = value; }
        }

        public Vector3 RandomForce
        {
            get { return animator.rndForce; }
            set { animator.rndForce = value; }
        }

        public bool RandomRotation
        {
            get { return emitter.rndRotation; }
            set { emitter.rndRotation = value; }
        }

        public Vector3 RandomVelocity
        {
            get { return emitter.rndVelocity; }
            set { emitter.rndVelocity = value; }
        }

        public ParticleRenderMode RenderMode
        {
            get { return renderer.particleRenderMode; }
            set { renderer.particleRenderMode = value; }
        }

        public float SizeGrow
        {
            get { return animator.sizeGrow; }
            set { animator.sizeGrow = value; }
        }

        public bool UseWorldSpace
        {
            get { return emitter.useWorldSpace; }
            set { emitter.useWorldSpace = value; }
        }

        public float VelocityScale
        {
            get { return renderer.velocityScale; }
            set { renderer.velocityScale = value; }
        }

        public Vector3 WorldRotationAxis
        {
            get { return animator.worldRotationAxis; }
            set { animator.worldRotationAxis = value; }
        }

        public Vector3 WorldVelocity
        {
            get { return emitter.worldVelocity; }
            set { emitter.worldVelocity = value; }
        }

        #endregion

        public Vector3 EmitterPosition
        {
            get { return obj.transform.localPosition; }
            set { obj.transform.localPosition = value; }
        }

        public Vector3 EmitterScale
        {
            get { return obj.transform.localScale; }
            set { obj.transform.localScale = value; }
        }

        public Transform EmitterTransform
        {
            get { return obj.transform; }
        }

        public int ParticleCount
        {
            get { return emitter.particleCount; }
        }

        [KSPField(isPersistant = false)]
        public String Label;

        private GameObject obj;
        private ParticleAnimator animator;
        private ParticleEmitter emitter;
        private new ParticleRenderer renderer;

        public override void OnStart(StartState state)
        {
            if (state == StartState.Editor) { return; }
            Setup();
        }

        public void Setup()
        {
            if (part.partInfo == null) { return; }
            if (obj != null) { return; }

            var node = GameDatabase.Instance.GetConfigs("PART").Single(c => part.partInfo.name == c.name.Replace('_', '.')).config.GetNodes("MODULE").Where(n => n.GetValue("name") == moduleName).Single(n => n.GetValue("Label") == Label);

            var shaderName = node.GetValue("ShaderName");
            var textureName = node.GetValue("TextureName");

            obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.collider.enabled = false;
            obj.renderer.material.color = new Color(0, 0, 0, 0);
            obj.renderer.material.shader = Shader.Find("Transparent/Diffuse");
            obj.transform.parent = part.transform;
            obj.transform.localRotation = Quaternion.identity;

            animator = (ParticleAnimator)obj.AddComponent<ParticleAnimator>();
            emitter = (ParticleEmitter)obj.AddComponent("MeshParticleEmitter");
            renderer = (ParticleRenderer)obj.AddComponent<ParticleRenderer>();

            var material = new Material(Shader.Find(shaderName));
            material.mainTexture = GameDatabase.Instance.GetTexture(textureName, false);
            material.color = Color.white;

            renderer.materials = new Material[] { material };
            animator.colorAnimation = new Color[5];

            if (Misc.Parse(node.GetValue("Collision"), false))
            {
                obj.AddComponent("WorldParticleCollider");
            }

            AngularVelocity         = Misc.Parse(node.GetValue("AngularVelocity"), 0f);
            CameraVelocityScale     = Misc.Parse(node.GetValue("CameraVelocityScale"), 0f);
            ColorAnimation1         = ConfigNode.ParseColor(node.GetValue("ColorAnimation1"));
            ColorAnimation2         = ConfigNode.ParseColor(node.GetValue("ColorAnimation2"));
            ColorAnimation3         = ConfigNode.ParseColor(node.GetValue("ColorAnimation3"));
            ColorAnimation4         = ConfigNode.ParseColor(node.GetValue("ColorAnimation4"));
            ColorAnimation5         = ConfigNode.ParseColor(node.GetValue("ColorAnimation5"));
            Damping                 = Misc.Parse(node.GetValue("Damping"), 1f);
            Emit                    = Misc.Parse(node.GetValue("Emit"), true);
            EmitterVelocityScale    = Misc.Parse(node.GetValue("EmitterVelocityScale"), 1f);
            Force                   = Misc.Parse(node.GetValue("Force"), Vector3.zero);
            LengthScale             = Misc.Parse(node.GetValue("LengthScale"), 1f);
            LocalRotationAxis       = Misc.Parse(node.GetValue("LocalRotationAxis"), Vector3.zero);
            LocalVelocity           = Misc.Parse(node.GetValue("LocalVelocity"), Vector3.zero);
            MaxEmission             = Misc.Parse(node.GetValue("MaxEmission"), 0f);
            MaxEnergy               = Misc.Parse(node.GetValue("MaxEnergy"), 0f);
            MaxParticleSize         = Misc.Parse(node.GetValue("MaxParticleSize"), 0f);
            MaxSize                 = Misc.Parse(node.GetValue("MaxSize"), 0f);
            MinEmission             = Misc.Parse(node.GetValue("MinEmission"), 0f);
            MinEnergy               = Misc.Parse(node.GetValue("MinEnergy"), 0f);
            MinSize                 = Misc.Parse(node.GetValue("MinSize"), 0f);
            RandomAngularVelocity   = Misc.Parse(node.GetValue("RandomAngularVelocity"), 0f);
            RandomForce             = Misc.Parse(node.GetValue("RandomForce"), Vector3.zero);
            RandomRotation          = Misc.Parse(node.GetValue("RandomRotation"), false);
            RandomVelocity          = Misc.Parse(node.GetValue("RandomVelocity"), Vector3.zero);
            RenderMode              = Misc.Parse(node.GetValue("RenderMode"), ParticleRenderMode.Billboard);
            SizeGrow                = Misc.Parse(node.GetValue("SizeGrow"), 0f);
            UseWorldSpace           = Misc.Parse(node.GetValue("UseWorldSpace"), false);
            VelocityScale           = Misc.Parse(node.GetValue("VelocityScale"), 0f);
            WorldRotationAxis       = Misc.Parse(node.GetValue("WorldRotationAxis"), Vector3.zero);
            WorldVelocity           = Misc.Parse(node.GetValue("WorldVelocity"), Vector3.zero);

            EmitterPosition         = Misc.Parse(node.GetValue("EmitterPosition"), Vector3.zero);
            EmitterScale            = Misc.Parse(node.GetValue("EmitterScale"), Vector3.zero);
        }
    }
}
