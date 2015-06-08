using System;
using System.IO;
using System.Linq;
using Com.Drew.Metadata;
using JetBrains.Annotations;
using Sharpen;
using Directory = System.IO.Directory;

namespace FileLabeller
{
    /// <summary>
    /// Writes a text file containing the extracted metadata for each input file.
    /// </summary>
    internal class TextFileOutputHandler : FileHandlerBase
    {
        public override void OnStartingDirectory(string directoryPath)
        {
            base.OnStartingDirectory(directoryPath);
            Directory.Delete(Path.Combine(directoryPath, "metadata"), recursive: true);
        }

        public override void OnBeforeExtraction(string filePath, string relativePath, TextWriter log)
        {
            base.OnBeforeExtraction(filePath, relativePath, log);
            log.Write(filePath);
            log.Write('\n');
        }

        public override void OnExtractionSuccess(string filePath, Metadata metadata, string relativePath, TextWriter log)
        {
            base.OnExtractionSuccess(filePath, metadata, relativePath, log);

            try
            {
                using (var writer = OpenWriter(filePath))
                {
                    try
                    {
                        // Build a list of all directories
                        var directories = metadata.GetDirectories().ToList();

                        // Sort them by name
                        directories.Sort((o1, o2) => string.Compare(o1.GetName(), o2.GetName(), StringComparison.Ordinal));

                        // Write any errors
                        if (metadata.HasErrors())
                        {
                            foreach (var directory in directories)
                            {
                                if (!directory.HasErrors())
                                    continue;
                                foreach (var error in directory.GetErrors())
                                    writer.Write("[ERROR: {0}] {1}\n", directory.GetName(), error);
                            }
                            writer.Write('\n');
                        }

                        // Write tag values for each directory
                        foreach (var directory in directories)
                        {
                            var directoryName = directory.GetName();
                            foreach (var tag in directory.GetTags())
                            {
                                var tagName = tag.GetTagName();
                                var description = tag.GetDescription();
                                writer.Write("[{0} - {1}] {2} = {3}\n",
                                    directoryName, tag.GetTagTypeHex(), tagName, description);
                            }

                            if (directory.GetTagCount() != 0)
                                writer.Write('\n');
                        }
                    }
                    finally
                    {
                        writer.Write("Generated using metadata-extractor\n");
                        writer.Write("https://drewnoakes.com/code/exif/\n");
                    }
                }
            }
            catch (Exception e)
            {
                log.Write("Exception after extraction: {0}\n", e.Message);
            }
        }

        public override void OnExtractionError(string filePath, Exception exception, TextWriter log)
        {
            base.OnExtractionError(filePath, exception, log);

            try
            {
                using (var writer = OpenWriter(filePath))
                {
                    writer.Write("EXCEPTION: {0}\n", exception.Message);
                    writer.Write('\n');
                    writer.Write("Generated using metadata-extractor\n");
                    writer.Write("https://drewnoakes.com/code/exif/\n");
                }
            }
            catch (Exception e)
            {
                Console.Error.Write("Error writing exception details to metadata file: {0}\n", e);
            }
        }

        [NotNull]
        private static TextWriter OpenWriter(string filePath)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            var metadataPath = Path.Combine(directoryPath, "metadata");
            var fileName = Path.GetFileName(filePath);

            // Create the output directory if it doesn't exist
            if (!Directory.Exists(metadataPath))
                Directory.CreateDirectory(metadataPath);

            var outputPath = string.Format("{0}/metadata/{1}.txt", directoryPath, fileName);

            var writer = new FileWriter(outputPath, false);
            writer.Write("FILE: {0}\n\n", fileName);

            return writer;
        }
    }
}