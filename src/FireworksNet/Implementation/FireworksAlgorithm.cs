using System;
using System.Collections.Generic;
using FireworksNet.Model;
using FireworksNet.Problems;

namespace FireworksNet.Implementation
{
    // Per 2010 paper
    public class FireworksAlgorithm : IFireworksAlgorithm
    {
        public Firework Solve(Problem problem, AlgorithmSetup setup)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Firework> MakeStep(IEnumerable<Firework> currentFireworks, Problem problem, AlgorithmSetup setup)
        {
            throw new NotImplementedException();
        }
    }
}