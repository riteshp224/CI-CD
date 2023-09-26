using System;

namespace Platinum.Model
{
    public class DataConfig
    {
        public string DefaultConnection { get; set; }
        public string FilePath { get; set; }
        public string TemplateFilePath { get; set; }
        public string TaxPer { get; set; }
    }

    public class TemplateFilesConfig
    {
        public string warehouse_import_template { get; set; }
        public string supplier_import_template { get; set; }
        public string product_import_template { get; set; }
    }
}
