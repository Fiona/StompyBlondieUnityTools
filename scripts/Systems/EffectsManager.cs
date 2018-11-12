/*
 * 2018 Stompy Blondie Games
 * Licensed under MIT. See accompanying LICENSE file for details.
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace StompyBlondie.Systems
{
    public class Effect
    {
        public GameObject effect;
        public Transform followTransform;
        public EffectsManager manager;

        private List<ParticleSystem> systems;
        private bool dying;

        public Effect(GameObject effect, EffectsManager manager)
        {
            this.effect = effect;
            this.manager = manager;
            systems = new List<ParticleSystem>();
            foreach(var comp in effect.GetComponentsInChildren<ParticleSystem>())
                systems.Add(comp);
        }

        public void MultiplySpeed(float factor)
        {
            foreach(var particle in systems)
            {
                var main = particle.main;
                main.simulationSpeed *= factor;
            }
        }

        public void Update()
        {
            if(!effect)
                manager.RemoveEffect(this);
            if(followTransform)
                effect.transform.position = followTransform.transform.position;
            // Waiting for all particles to disappear before destroying
            if(dying)
            {
                foreach(var s in systems)
                    if(s && s.IsAlive())
                        return;
                manager.RemoveEffect(this, immediate:true);
            }
        }

        public void Kill()
        {
            foreach(var s in systems)
                if(s)
                    s.Stop();
            dying = true;
        }
    }

    /**
     * Used to fire off and keep track of effects, effects are typically particle effects
     *
     * Should be used in conjunction with a static class containing resource paths to each effect as an instantiable
     * object.
     *
     * Example usage
     * -------------
     *
     * ```{.cs}
     *  public static class EffectsTypes
     *  {
     *     public static string LOOP_RAIN = "looping/Rain";
     *     public static string ONESHOT_EXPLOSION = "oneshot/Explosion";
     *  }
     *
     *  ...
     *
     *  // Initialising the manager
     *  var effectsManager = EffectsManager.CreateEffectsManager(
     *       typeof(EffectsType).GetFields(),
     *       "resources/path/to/prefabs/"
     *  );
     *
     *  // Spawning effects
     *  effectsManager.CreateEffect(EffectsTypes.ONESHOT_EXPLOSION, vector3SpawnLocation);
     *  effectsManager.CreateEffect(EffectsTypes.LOOP_RAIN, vector3SpawnLocation);

     *  // Effects that follow a transform during it's lifespan
     *  effectsManager.CreateEffect(EffectsTypes.ONESHOT_EXPLOSION, transformToFollow);

     *  // Manually removing effects
     *  var rain = CreateEffect(EffectsTypes.LOOP_RAIN, vector3SpawnLocation);
     *  ....
     *  effectsManager.RemoveEffect(rain);
     * ```
     */
    public class EffectsManager: MonoBehaviour
    {
        public string effectResourcesPath = "";
        [HideInInspector]
        public Dictionary<string, GameObject> preloadedEffects;
        [HideInInspector]
        public List<Effect> activeEffects;
        [HideInInspector]
        public FieldInfo[] effectTypes;

        /*
         * Factory to create the effects manager
         */
        public static EffectsManager CreateEffectsManager(FieldInfo[] effectTypes, string effectResourcesPath = "effects/")
        {
            var go = new GameObject("Effects Manager");
            var comp = go.AddComponent<EffectsManager>();
            comp.Initialise(effectTypes, effectResourcesPath);
            return comp;
        }

        public void Initialise(FieldInfo[] effectTypes, string effectResourcesPath = "effects/")
        {
            this.effectResourcesPath = effectResourcesPath;
            this.effectTypes = effectTypes;
            activeEffects = new List<Effect>();
            preloadedEffects  = new Dictionary<string, GameObject>();

            // Preload all effects types
            foreach(var prop in effectTypes)
            {
                var name = (string)prop.GetValue(prop);
                var path = effectResourcesPath + name;
                preloadedEffects[name] = Resources.Load<GameObject>(path);
                if(!preloadedEffects[name])
                    throw new Exception("Can't find resource for defined effect " + path);
            }
        }

        public Effect CreateEffect(string type, Vector3 location)
        {
            if(!preloadedEffects.ContainsKey(type))
                throw new Exception("Can't find a loaded effect called " + type);
            var newObj = Instantiate(preloadedEffects[type]);
            if(!newObj)
                throw new Exception("Error instantiating " + type);
            newObj.transform.SetParent(gameObject.transform);
            newObj.transform.position = location;
            var effect = new Effect(newObj, this);
            activeEffects.Add(effect);
            return effect;
        }

        public Effect CreateEffect(string type, Transform follow)
        {
            var effect = CreateEffect(type, follow.position);
            effect.followTransform = follow;
            return effect;
        }

        public void RemoveEffect(Effect effect, bool immediate = false)
        {
            if(!immediate)
            {
                effect.Kill();
                return;
            }
            if(effect.effect)
                Destroy(effect.effect);
            activeEffects.Remove(effect);
        }

        private void Update()
        {
            foreach(var effect in new List<Effect>(activeEffects))
                effect.Update();
        }
    }
}