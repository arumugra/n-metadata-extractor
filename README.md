NMetadataExtractor
====================

**Update June 25 2015** See [the original author's fork instead](https://github.com/drewnoakes/metadata-extractor-dotnet)! We're happy to see an official C# version kept in sync with the Java original. 

master: [![Build status](https://ci.appveyor.com/api/projects/status/12bkj9y5wcydqak7/branch/master?svg=true)](https://ci.appveyor.com/project/imazen/n-metadata-extractor/branch/master) most recent commit: [![Build status](https://ci.appveyor.com/api/projects/status/12bkj9y5wcydqak7?svg=true)](https://ci.appveyor.com/project/imazen/n-metadata-extractor) [Download documentation archive](https://ci.appveyor.com/project/imazen/n-metadata-extractor/build/artifacts)


C# port of the [excellent Java MetadataExtractor library by Drew Noakes](https://drewnoakes.com/code/exif/). 


Automated conversion was performed with our [enhanced version of Sharpen](https://github.com/imazen/sharpen), [custom configuration](https://github.com/imazen/sharpen_imazen_config), and then a set of manual patches. MetadataExtractor 2.8.0 was converted.

Many thanks to Yakov Danilov <yakodani@gmail.com> his work in porting this library from Java to C#. 


Also, special thanks to [Ferret Renaud, who provided a C# port for many years](http://ferretrenaud.fr/Projets/MetaDataExtractor/index.html). His code can be found in the 'renaud' branch. 

All code herein is licensed under one of the following licenses: Apache 2, BSD, MIT. See LICENSE for details about which code is licensed under which license. 

ICSharpCode.SharpZipLib is licensed under the GPL with a linking exception, so it can be used in commerical products. 


### To-do

* Publish to NuGet
* Add documentation (and where possible, re-use documentation from the original).

## Example use

Autocoverted examples: 

https://github.com/imazen/n-metadata-extractor/blob/master/Com.Drew/Com/drew/tools/ProcessAllImagesInFolderUtility.cs

https://github.com/imazen/n-metadata-extractor/blob/master/Com.Drew/Com/drew/tools/ProcessUrlUtility.cs

Sample project "SampleReader"


### Basic usage

```

var meta = ImageMetadataReader.ReadMetadata(InputStream.Wrap(stream)); //Stream must be seekable

//Produce a list of strings containing metadata key/value pairs. 
var strings = metadata.GetDirectories().SelectMany(d => d.GetTags().Select(t => String.Format("[{0}] {1} = {2}\n",d.GetName(), t.GetTagName(), t.GetDescription()))).ToList();
```
