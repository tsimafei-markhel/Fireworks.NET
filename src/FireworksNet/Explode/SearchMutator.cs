using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Algorithm.Implementation;
using FireworksNet.Model;
using FireworksNet.Selection;

namespace FireworksNet.Explode
{
    /// <summary>
    /// Implements search algorithm.
    /// </summary>
    public class SearchMutator : IFireworkMutator
    {
        private IFireworkMutator mutator;        
        private ISparkGenerator generator;
        private IFireworkSelector selector;
        private ParallelFireworksAlgorithmSettings settings;
                
        public SearchMutator(IFireworkMutator mutator, ISparkGenerator generator, IFireworkSelector selector, ParallelFireworksAlgorithmSettings settings)
        {
            if(mutator == null)
            {
                throw new ArgumentNullException("mutator");
            }                 

            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this.mutator = mutator;
            this.generator = generator;
            this.selector = selector;
            this.settings = settings;
        }
        
        /// <summary>
        /// Executes search. It will be explodes firework 'L' times.
        /// Here, the "L" is set in the <see cref="ParallelFireworksAlgorithmSettings"/>.
        /// 'L' equals ParallelFireworksAlgorithmSettings.QuantityStepsResearch;
        /// </summary>
        /// <param name="bestFirework">The Firework for mutate.</param>
        /// <param name="explosion">The explosion that gives birth to the spark.</param>
        public void MutateFirework(ref Firework mutableFirework, FireworkExplosion explosion)
        {
            Debug.Assert(mutableFirework != null, "Mutable firework is null");
            Debug.Assert(explosion != null, "Explosion is null");
            Debug.Assert(explosion.ParentFirework != null, "Explosion parent firework is null");
            Debug.Assert(explosion.ParentFirework.Coordinates != null, "Explosion parent firework coordinate collection is null");
            Debug.Assert(settings.QuantityStepsResearch > 0, "Quantity steps of research must be > 0");
           
            for (int i = 0; i < settings.QuantityStepsResearch; ++i)
            {
                IEnumerable<Firework> fireworks = generator.CreateSparks(explosion);
                Firework best = selector.SelectFireworks(fireworks, 1).First();
                UpdateFirework(best, ref mutableFirework);
            }

            mutator.MutateFirework(ref mutableFirework, explosion);
        }

        /// <summary>
        /// Updates mutable firework. He simply copy fields.
        /// </summary>
        /// <param name="source">Source firework - from where to copy.</param>
        /// <param name="target">Target firework - where to copy.</param>
        /// <returns></returns>
        private static Firework UpdateFirework(Firework source, ref Firework target)
        {
            target.Coordinates = source.Coordinates;
            target.Quality = source.Quality;
            target.BirthStepNumber = source.BirthStepNumber;//TODO: maybe this field unnecessary     
            target.FireworkType = source.FireworkType; //TODO: maybe this field unnecessary     

            return source;
        }
    }
}
