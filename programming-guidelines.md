# Programming Guidelines

This document contains standard conventions that are to be used in Fireworks.NET. Therefore please use this guideline as part of your development process.


##Design Rules
1. Use C# native types instead of CTS types.
```csharp
int counter;            // good
Int32 anotherCounter;   // bad
```
2. Avoid changing arguments in methods. If a method has to changes its argument's value, declare corresponding parameter with ```ref``` or ```out``` modifier, even the reference type parameters.
3. Use properties, not public fields.


##Code Style
###Naming Conventions
1. All names should be written in English.
2. Don’t be cute.
```csharp
Kill();     // NOT: Whack();
Abort();    // NOT: Goodbye();
```
3. Names representing types must be nouns and written in mixed case starting with upper case.
```
Problem, FireworksAlgorithmSettings
```
4. Abstract base types have to be named with postfix ```Base```.
```
DistanceBase
```
5. Private fields, local variables and method parameters names must be in mixed case starting with lower case.
```
firework, locationSelector
```
6. Public fields, properties and events names must be in mixed case starting with upper case.
```
LocationsNumber
```
7. Names representing methods must be verbs and written in mixed case starting with upper case.
```
GetBest(), MakeStep()
```
8. Names representing interfaces must follow type naming rule and start with ```I```.
```
ISelector, IFireworksAlgorithm
```
9. Abbreviations and acronyms should not be uppercase when used as name.
```csharp
Id                  // NOT: ID
OpenDvdPlayer();    // NOT: openDVDPlayer();
```
10. The name of the object is implicit, and should be avoided in a method name.
```csharp
line.GetLength();   // NOT: line.GetLineLength();
```
11. Prefix *is* should be used for boolean variables and methods.
```
isSet, IsVisible()
```
12. Plural form should be used on names representing a collection of objects.
```csharp
List<Point> points;
int[] values;
```
13. Complement names must be used for complement entities.
```
get/set, add/remove, create/destroy,
start/stop, insert/delete,
increment/decrement, old/new, begin/end,
first/last, up/down, min/max,
next/previous, old/new, open/close,
show/hide, suspend/resume, etc.
```
14. Abbreviations in names should be avoided, unless following Framework naming convention.
```csharp
CalculateAverage();     // NOT: CalcAvg();
EventArgs eventArgs;    // NOT: EventArgs e; or EventArgs eventArguments;
```
15. Negated boolean variable names must be avoided.
```csharp
bool isError;   // NOT: isNoError
bool isFound;   // NOT: isNotFound
```
16. Implementations per 2010 paper do not need any additional prefixes/postfixes. This relates to benchmark problems, too.
```
LocationSelector
```
17. Enhanced/alternative/post-2010-paper implementations must have corresponding prefixes/suffixes.
```
Rosenbrock2013
```


##General Rules
1. Spaces, not tabs. Tab indent = 4 spaces.
2. Always use ```this``` keyword to access instance members.
3. Always specify type name to access static & constant members.
4. Do not add new line in the end of file.
5. Keep ```using <Namespace>;``` outside of namespace block.
6. ```using <Namespace>;``` ordering: first System, then everything else. Maintain alphabetic ascending order inside these groups.


##Code Comment

1. Comments do not make up for bad code.
2. Explain yourself in code.
```csharp
// Check to see if the employee is eligible for full benefits
if ((employee.isHourly()) && (employee.age > 65))
```
Or
```csharp
if (employee.IsEligibleForFullBenefits())
```
3. Add *TODO*, *FIXME* comments for the job that should be done, but for some reason do not have time to do it.
```csharp
// TODO: Add input validation
```
4. Do not express your emotions in comment.
5. Provide meaningful XML comments for all (at least public) types and members.
6. End an XML comment text with a dot.
7. Use tools like [Spell Checker](https://visualstudiogallery.msdn.microsoft.com/7c8341f1-ebac-40c8-92c2-476db8d523ce) VS extension to check your spelling.


##Testing
1. Test code must follow the same code conventions and programming styles.
2. Unit tests should be fully automated and non-interactive.
3. Tests should be independent of each other.
4. Tests should be repeatable.
5. Fix failing tests immediately and do not commit/push your code to the repository if your test is failing.
6. The person who broke the tests is responsible to fix the test without deleting the test.
7. Follow [Arrange-Act-Assert](http://www.arrangeactassert.com/why-and-what-is-arrange-act-assert/) pattern.
8. Follow [Naming Standards for Unit Tests](http://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html).
9. When creating a test class, put it to the folder and namespace that correspond the class under test.
```
src/FireworksNet/Selection/SamplingMethods/BestSelector.cs              // Class under test
src/FireworksNet.Tests/Selection/SamplingMethods/BestSelectorTests.cs   // Test class
```


-----


##Inspired by
* [software-development-guidelines](https://github.com/yetu/software-development-guidelines)
