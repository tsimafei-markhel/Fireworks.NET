# Programming Guidelines

This document contains standard conventions that are to be used in Fireworks.NET. Therefore please use this guideline as part of your development process.


##Design Rules
- Use C# native types instead of CTS types.
```csharp
int counter;            // good
Int32 anotherCounter;   // bad
```
- Avoid changing arguments in methods. If a method has to changes its argument's value, declare corresponding parameter with ```ref``` or ```out``` modifier, even the reference type parameters.
- Use properties, not public fields.


##Code Style
###Naming Conventions
- All names should be written in English.
- Don’t be cute.
```csharp
Kill();     // NOT: Whack();
Abort();    // NOT: Goodbye();
```
- Names representing types must be nouns and written in mixed case starting with upper case.
```
Problem, FireworksAlgorithmSettings
```
- Abstract base types have to be named with postfix ```Base```.
```
DistanceBase
```
- Private fields, local variables and method parameters names must be in mixed case starting with lower case.
```
firework, locationSelector
```
- Public fields, properties and events names must be in mixed case starting with upper case.
```
LocationsNumber
```
- Names representing methods must be verbs and written in mixed case starting with upper case.
```
GetBest(), MakeStep()
```
- Names representing interfaces must follow type naming rule and start with ```I```.
```
ISelector, IFireworksAlgorithm
```
- Abbreviations and acronyms should not be uppercase when used as name.
```csharp
Id                  // NOT: ID
OpenDvdPlayer();    // NOT: openDVDPlayer();
```
- The name of the object is implicit, and should be avoided in a method name.
```csharp
line.GetLength();   // NOT: line.GetLineLength();
```
- Prefix *is* should be used for boolean variables and methods.
```
isSet, IsVisible()
```
- Plural form should be used on names representing a collection of objects.
```csharp
List<Point> points;
int[] values;
```
- Complement names must be used for complement entities.
```
get/set, add/remove, create/destroy,
start/stop, insert/delete,
increment/decrement, old/new, begin/end,
first/last, up/down, min/max,
next/previous, old/new, open/close,
show/hide, suspend/resume, etc.
```
- Abbreviations in names should be avoided, unless following Framework naming convention.
```csharp
CalculateAverage();     // NOT: CalcAvg();
EventArgs eventArgs;    // NOT: EventArgs e; or EventArgs eventArguments;
```
- Negated boolean variable names must be avoided.
```csharp
bool isError;   // NOT: isNoError
bool isFound;   // NOT: isNotFound
```
- Implementations per 2010 paper do not need any additional prefixes/postfixes. This relates to benchmark problems, too.
```
LocationSelector
```
- Enhanced/alternative/post-2010-paper implementations must have corresponding prefixes/suffixes.
```
Rosenbrock2013
```


##General Rules
- Spaces, not tabs. Tab indent = 4 spaces.
- Always use ```this``` keyword to access instance members.
- Always specify type name to access static & constant members.
- Do not add new line in the end of file.
- Keep ```using <Namespace>;``` outside of namespace block.
- ```using <Namespace>;``` ordering: first System, then everything else. Maintain alphabetic ascending order inside these groups.


##Code Comment

- Comments do not make up for bad code.
- Explain yourself in code.
```csharp
// Check to see if the employee is eligible for full benefits
if ((employee.isHourly()) && (employee.age > 65))
```
Or
```csharp
if (employee.IsEligibleForFullBenefits())
```
- Add *TODO*, *FIXME* comments for the job that should be done, but for some reason do not have time to do it.
```csharp
// TODO: Add input validation
```
- Do not express your emotions in comment.
- Provide meaningful XML comments for all (at least public) types and members.
- End an XML comment text with a dot.
- Use tools like [Spell Checker](https://visualstudiogallery.msdn.microsoft.com/7c8341f1-ebac-40c8-92c2-476db8d523ce) VS extension to check your spelling.


##Testing
- Test code must follow the same code conventions and programming styles.
- Unit tests should be fully automated and non-interactive.
- Tests should be independent of each other.
- Tests should be repeatable.
- Fix failing tests immediately and do not commit/push your code to the repository if your test is failing.
- The person who broke the tests is responsible to fix the test without deleting the test.
- Follow [Arrange-Act-Assert](http://www.arrangeactassert.com/why-and-what-is-arrange-act-assert/) pattern.
- Follow [Naming Standards for Unit Tests](http://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html).
- When creating a test class, put it to the folder and namespace that correspond the class under test.
```
src/FireworksNet/Selection/SamplingMethods/BestSelector.cs              // Class under test
src/FireworksNet.Tests/Selection/SamplingMethods/BestSelectorTests.cs   // Test class
```


-----


##Inspired by
* [software-development-guidelines](https://github.com/yetu/software-development-guidelines)
