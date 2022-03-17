using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System.Reflection;
using System.IO;

namespace Inview.Epi.EpiFund.Web.Binders
{
    public class AssetViewModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var typeValue = bindingContext.ValueProvider.GetValue("TypeOfAsset");
            Assembly assembly = Assembly.Load(new AssemblyName("Inview.Epi.EpiFund.Domain"));
            var type = assembly.GetType(
                (string)typeValue.ConvertTo(typeof(string)),
                true
            );
            var model = Activator.CreateInstance(type);
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, type);
            return model;
        }

    }
}