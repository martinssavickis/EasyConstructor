# EasyConstructor
Library for easily constructing immutable objects in tests

#### Intended usage
Cases where you need to mock classes with long constructor parameter lists

#### Current features
Creating instances from sparse list of parameters from anonymous types with defaults for missing values

#### Planned features
* Nested parameter initialization
* Picking the most appropriate constructor (currently using first available)
* A way to configure constructor to use
* Refactor to fluent interface
* TBD - construct empty class object and set private fields (if you really wish to shoot you legs clean off)