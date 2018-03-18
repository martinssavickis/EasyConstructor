# EasyConstructor
Library for easily constructing immutable objects in tests

## Build status
[![Build Status](https://travis-ci.org/morowinder/EasyConstructor.svg?branch=master)](https://travis-ci.org/morowinder/EasyConstructor)
[![CodeFactor](https://www.codefactor.io/repository/github/morowinder/easyconstructor/badge)](https://www.codefactor.io/repository/github/morowinder/easyconstructor)

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
