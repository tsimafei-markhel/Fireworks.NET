Fireworks.NET
=============

[![Build status](https://ci.appveyor.com/api/projects/status/em6rtw0cj5lre0k4?svg=true)](https://ci.appveyor.com/project/tsimafei-markhel/fireworks)

Fireworks.NET is a .NET implementation of swarm intelligence [Fireworks Algorithm for Optimization](http://www.cil.pku.edu.cn/research/fa/) (FWA) and its variations.

###Project Purpose
This project is aimed at creating a set of .NET "building blocks" for FWA - and build some variations of FWA from these "blocks".

All rights for the algorithms belong to their respective authors.

###Intended Audience
* .NET developers interested in Optimization algorithms.
* Researchers seeking for .NET implementation of FWA that can be used in other projects.

###Reference
Implementation is based on the following papers that can be found by the [link](http://www.cil.pku.edu.cn/publications/):
* Ying Tan, Yuanchun Zhu, "Fireworks Algorithms for Optimization", Proc. of International Conference on Swarm Intelligence (ICSI’2010), June 12-15, Beijing, China, 2010. ICSI 2010, Part II, LNCS 6145, pp.355-364, 2010. Springer-Verlag, 2010. - Referred in code as **2010 paper**.
* Y. Pei, S.Q. Zheng, Y. Tan and Hideyuki Takagi, "An Empirical Study on Influence of Approximation Approaches on Enhancing Fireworks Algorithm", IEEE International Conference on System, Man and Cybernetics (SMC 2012), Seoul, Korea. October 14-17, 2012. - Referred in code as **2012 paper**.
* (Items to be added to this list as new implementations appear.)

##License
The project is released under the terms of the MIT license. See [LICENSE](LICENSE) file for the complete text.

##Dependencies and Third-Party Libraries
FireworksNet:
- [Math.NET Numerics 3.5.0](https://www.nuget.org/packages/MathNet.Numerics/3.5.0)

FireworksNet.Tests:
- [xUnit.net 2.0.0](https://www.nuget.org/packages/xunit/2.0.0)
- [xUnit.net [Runner: Visual Studio] 2.0.0](https://www.nuget.org/packages/xunit.runner.visualstudio/2.0.0)
- [NSubstitute 1.8.1](https://www.nuget.org/packages/NSubstitute/)

**Note** NuGet packages are not committed to the repository, you need to use Package Restore to get them before the build.

##Contribution
###Programming Guidelines
Please refer to the [programming-guidelines.md](programming-guidelines.md) file for the code style and other guidelines.
