Coding Guidelines
================

- Spaces, not tabs. Tab indent = 4 spaces.
- Always use ```this``` keyword to access instance members.
- Always specify type name to access static & const members.
- Do not add new line in the end of file.
- Keep ```using <Namespace>;``` outside of namespace block.
- ```using <Namespace>;``` ordering: first System, then 3<sup>rd</sup>-party, then everything else. Maintain alphabetic ascending order inside these groups.
- Use C# native types instead of CTS types (e.g. ```int``` instead of ```Int32```).
- Provide meaningful XML comments for all (at least public) types and members.
- End an XML comment text with a dot.

##Type Naming Rules
- Abstract base types have to be named with postfix ```Base``` (e.g. ```DistanceBase```).
- Implementations per 2010 paper do not need any additional prefixes/postfixes (e.g. ```LocationSelector```). This relates to benchmark problems, too.
- Enhanced/alternative/post-2010-paper implementations must have corresponding prefixes/suffixes (e.g. ```Rosenbrock2013```).