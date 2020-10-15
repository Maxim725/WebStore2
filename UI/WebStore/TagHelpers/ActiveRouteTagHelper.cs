using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WebStore.TagHelpers
{
    [HtmlTargetElement(/* Имя тега */ Attributes = AttributeName /*"attribute1,attribute2"*/)]
    public class ActiveRouteTagHelper : TagHelper
    {
        public const string AttributeName = "is-active-route";
        public const string IgnoreAction = "ignore-action";
        
        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        [HtmlAttributeName("asp-controler")]
        public string Controller { get; set; }

        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public Dictionary<string, string> RouteValues { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var ignoreAction = output.Attributes.ContainsName(IgnoreAction);

            if (IsActive(ignoreAction))
                MakeActive(output);

            output.Attributes.RemoveAll(AttributeName);
            output.Attributes.RemoveAll(IgnoreAction);
        }

        private bool IsActive(bool ignoreAction)
        {
            var routeValues = ViewContext.RouteData.Values;

            var currentController = routeValues["Controller"].ToString();
            var currentAction = routeValues["Action"].ToString();

            if (!string.IsNullOrEmpty(Controller) && !string.Equals(currentController, Controller))
                return false;

            if (!ignoreAction && !string.IsNullOrEmpty(Action) && !string.Equals(currentAction, Action))
                return false;

            foreach(var (key, value) in routeValues)
                if (!routeValues.ContainsKey(key) || routeValues[key].ToString() != value.ToString())
                    return false;

            return true;
        }

        private void MakeActive(TagHelperOutput output)
        {
            var classAttribute = output.Attributes.FirstOrDefault(attr => attr.Name.Equals("class"));

            if (classAttribute is null)
                output.Attributes.Add("class", "active");
            else
            {
                if (classAttribute.Value.ToString()?.Contains("active") ?? false) return;

                output.Attributes.SetAttribute("class", classAttribute.Value + " active");
            }
        }
    }
}
