using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Crossrail.ObservationForm.Mvc.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Converts a virtual (relative) path to an application absolute path if provided, no content otherwise.
        /// </summary>
        /// <returns>A <see cref="MvcHtmlString"/> with the <see cref="contentPath"/> if provided, <c>NULL</c> otherwise</returns>

        public static MvcHtmlString ContentWithDefault(
            this UrlHelper urlHelper, 
            string contentPath,
            string linkText = "View",
            object htmlAttributes = null)
        {
            if (!string.IsNullOrWhiteSpace(contentPath))
            {
                TagBuilder tagBuilder = new TagBuilder("a");

                tagBuilder.Attributes.Add("href", urlHelper.Content(contentPath));
                tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
                tagBuilder.SetInnerText(linkText);

                return MvcHtmlString.Create(tagBuilder.ToString());
            }

            return null;
        }

        /// <summary>
        /// Creates a group of radio buttons from a list of SelectListItem's.
        /// </summary>

        public static MvcHtmlString RadioButtonGroupFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> listOfValues,
            object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var stringBuilder = new StringBuilder();

            if (listOfValues != null)
            {
                // Create a radio button for each item in the list 
                foreach (SelectListItem item in listOfValues)
                {
                    // Generate an id to be given to the radio button field 
                    var id = string.Format("{0}-{1}", metadata.PropertyName, item.Value);

                    // Create and populate a radio button using the existing html helpers
                    var label = htmlHelper.Label(id, HttpUtility.HtmlEncode(item.Text));

                    // First radio button will get the unobtrusive attributes since it is using
                    // the generic html helper e.g. "For", and this will only output the attributes
                    // for the first radio button

                    var radio = htmlHelper.RadioButtonFor(expression, item.Value, new { id = id }).ToHtmlString();

                    // Create the html string that will be returned to the client 
                    // e.g. 
                    //      <input data-val="true" data-val-required="" id="TestRadio_1" name="TestRadio" type="radio" value="1" />
                    //      <label for="TestRadio_1">Line1</label> 

                    var tagBuilder = new TagBuilder("span");

                    tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
                    tagBuilder.InnerHtml = string.Format("{0} {1}", radio, label);

                    stringBuilder.AppendFormat("{0} ", tagBuilder);
                }
            }

            return MvcHtmlString.Create(stringBuilder.ToString());
        }

        /// <summary>
        /// Generates a better label. 
        /// Text based off given labelText, [DisplayName], or property name.
        /// If the field is optional ([Required]), adds an (optional) em tag. 
        /// </summary>
        /// <remarks>
        /// Output markup:
        ///     <code><![CDATA[
        ///     <label for="input">
        ///         [Label text]
        ///         <em class="optional">(optional)</em>
        ///     </label>
        ///     ]]>            
        ///     </code>
        /// Inspired by: http://kamranicus.com/Blog/Posts/3/a-better-mvc-label-helper
        /// </remarks>
        
        public static MvcHtmlString FieldLabelFor<TModel, TResult>(
            this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TResult>> expression,
            object htmlAttributes = null,
            string labelText = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            // if there is a . in the name, take the rightmost part.
            string propName = metadata.PropertyName;
            string unqualifiedPropName = propName.Split('.').Last();

            string finalLabelText = labelText ?? metadata.DisplayName ?? metadata.PropertyName ?? unqualifiedPropName;

            if (String.IsNullOrEmpty(finalLabelText))
            {
                return MvcHtmlString.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder();
            TagBuilder tagBuilder = new TagBuilder("label");

            tagBuilder.Attributes.Add("for", htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(propName));
            tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            stringBuilder.AppendFormat("{0}:", finalLabelText);

            if (!metadata.IsRequired)
            {
                stringBuilder.Append(" <em class='optional'>(Optional)</em>");
            }

            tagBuilder.InnerHtml = stringBuilder.ToString();

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}