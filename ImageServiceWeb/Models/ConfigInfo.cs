using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ConfigInfo
    {
        public ConfigInfo(List<string> handlers, string outputDir, string sourceName, string logName, int thumbnailSize)
        {
            Handlers = new List<DirectoryModel>();
            foreach (string dir in handlers)
            {
                Handlers.Add(new DirectoryModel(dir));
            }
            OutputDir = outputDir;
            SourceName = sourceName;
            LogName = logName;
            ThumbnailSize = thumbnailSize;
        }

        //private readonly List<string> handlers;
        [Required]
        [Display(Name = "Handlers")]
        public List<DirectoryModel> Handlers { get; }
        //private readonly string outputDir;
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "OutputDir")]
        public string OutputDir { get; }
        //private readonly string sourceName;
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "SourceName")]
        public string SourceName { get; }
        //private readonly string logName;
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LogName")]
        public string LogName { get; }
        //private readonly int thumbnailSize;
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbnailSize")]
        public int ThumbnailSize { get; }
    }
}