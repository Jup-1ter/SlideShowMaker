using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace SlideShowMaker.ValidationRules
{
    class FolderExistenceRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) =>
            Directory.Exists(value as string) ?
            ValidationResult.ValidResult :
            new ValidationResult(false, "Указана несуществующая директория");
    }
}