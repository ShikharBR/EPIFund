using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inview.Epi.EpiFund.Domain.ViewModel
{
    public enum PageCategory
    {
        BasicInfo,
        Paper,
        DetailedInfo,
        ExistingMortgages,
        Images,
        Documents,
        Videos,
        Misc
    }
    //Immutable class
    public sealed class ModelValidationError
    {
        public readonly string PropertyName;
        public readonly string ErrorMessage;
        public readonly PageCategory Category;

        public ModelValidationError(string propertyName, string errorMessage, PageCategory pageCategory)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
            Category = pageCategory;
        }
    }
}
