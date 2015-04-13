using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FireworksNet.Algorithm.Implementation;
using FireworksNet.Model;
using FireworksNet.Selection;
using FireworksNet.Algorithm;

namespace FireworksNet.Explode
{
    /// <summary>
    /// Implements search algorithm.
    /// </summary>
    public class FireworkSearchMutator : IFireworkMutator
    {
        private ParallelFireworksAlgorithm.QualityCalculatorDelegate calculator;
        private IFireworkMutator mutator;
        private ISparkGenerator generator;
        private IFireworkSelector selector;
        private int quantityStepsResearch;

        /// <summary>
        /// Create instance of SearchMutator.
        /// </summary>
        /// <param name="calculator">Delegate of function to calculate quality during search.</param>
        /// <param name="mutator">Execute mutation of firework.</param>
        /// <param name="generator">Generate fireworks during search.</param>
        /// <param name="selector">Util class for select best firework, after generate.</param>
        /// <param name="quantityStepsResearch">Quantity of search iterations.</param>
        /// <exception cref="System.ArgumentNullException"> 
        /// if <paramref name="calculator"/> or <paramref name="mutator"/> or <paramref name="generator"/> or <paramref name="selector"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// if <paramref name="quantityStepsResearch"/> is negative.
        /// </exception>
        public FireworkSearchMutator(
            ParallelFireworksAlgorithm.QualityCalculatorDelegate calculator,
            IFireworkMutator mutator,
            ISparkGenerator generator,
            IFireworkSelector selector,
            int quantityStepsResearch)
        {
            if (calculator == null)
            {
                throw new ArgumentNullException("calculator");
            }

            if (mutator == null)
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

            if (quantityStepsResearch < 1)
            {
                throw new ArgumentException("quantityStepsResearch");
            }

            this.calculator = calculator;
            this.mutator = mutator;
            this.generator = generator;
            this.selector = selector;
            this.quantityStepsResearch = quantityStepsResearch;
        }

        /// <summary>
        /// Executes search. It will be explodes firework 'L' times.
        /// Here, the "L" is set in the <see cref="ParallelFireworksAlgorithmSettings"/>.
        /// 'L' equals ParallelFireworksAlgorithmSettings.QuantityStepsResearch;
        /// </summary>
        /// <param name="bestFirework">The Firework for mutate.</param>
        /// <param name="explosion">The explosion that gives birth to the spark.</param>
        public void MutateFirework(ref MutableFirework mutableFirework, FireworkExplosion explosion)
        {
            Debug.Assert(mutableFirework != null, "Mutable firework is null");
            Debug.Assert(explosion != null, "Explosion is null");
            Debug.Assert(explosion.ParentFirework != null, "Explosion parent firework is null");
            Debug.Assert(explosion.ParentFirework.Coordinates != null, "Explosion parent firework coordinate collection is null");
            Debug.Assert(quantityStepsResearch < 1, "Quantity steps of research < 1");

            for (int i = 0; i < quantityStepsResearch; ++i)
            {
                IEnumerable<Firework> sparks = this.generator.CreateSparks(explosion);
                Debug.Assert(sparks != null, "Sparks cannot is null");

                this.calculator(sparks);

                Firework newState = this.selector.SelectFireworks(sparks, 1).First();//best firework
                Debug.Assert(newState != null, "New state firework is null");

                mutableFirework.Update(newState);
            }

            mutator.MutateFirework(ref mutableFirework, explosion);
        }
    }
}
