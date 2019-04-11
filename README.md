# MHFTranslate
[![Build Status](https://travis-ci.com/wroleader/MHFTranslate.svg?branch=master)](https://travis-ci.com/wroleader/MHFTranslate)   [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/wroleader/MHFTranslate/blob/master/LICENSE)

MHFTranslate is a small console application that automatically parses the logs from Monster Hunter Frontier and translates
it using Google's Cloud Translation API. This is a WORK IN PROGRESS/PROOF OF CONCEPT.

## Getting Started
These instructions will get you a copy of the project running on your local machine for development and testing. If you need a
compiled binary, see "Precompiled Binaries" instead.

## Prerequisites
This project makes use of the following NuGet packages:
```
Google.Cloud.Translation.V2
dein.Colorify
Newtonsoft.Json
```

Everything else is pretty self-explanatory. To use this software you need your own Google Cloud Services API key. A test API key will be provided upon request that will last a few days. Please use wisely.

## Usage
To use MHFTranslate, make sure the game is open and that you've spent enough time in a lobby for a log to be written. Then, open the program and it will start reading & translating automatically. It currently requires to be running in a separate window, although an overlay will be considered for Version 2 if it gathers enough interest.

## Author
* Mikhail Tzenkov (MikkyTzen)

## Contributors
* SolluxKarkat (Pending invitation approval)


-- If you would like to contribute, please send me a message. 
First-timers and beginners are welcome. Pull requests welcome. 


## Pre-compiled binaries
To get a pre-compiled binary that you can start using right away, visit our [Releases](https://github.com/wroleader/MHFTranslate/releases) page.

## Acknowledgements:
* StackOverflow
* Friends who supported this project
