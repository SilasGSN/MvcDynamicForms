﻿using System;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcDynamicForms.NetCore.Fields.Abstract;

namespace MvcDynamicForms.NetCore.Fields
{
    /// <summary>
    /// Represents an html hidden input element.
    /// </summary>
    [Serializable]
    public class Hidden : InputField
    {
        /// <summary>
        /// The value of the hidden field.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The value of the hidden field. Returns the Value property.
        /// </summary>
        public override string Response
        {
            get { return this.Value; }
        }

        /// <summary>
        /// Validates the hidden field. This always returns true.
        /// </summary>
        public override bool Validate()
        {
            return true;
        }

        /// <summary>
        /// Render the hidden field as an html hidden input element.
        /// </summary>
        /// <returns></returns>
        public override string RenderHtml()
        {
            var html = new StringBuilder(this.Template);
            var inputName = this.GetHtmlId();

            // input element
            var hdn = new TagBuilder("input");
            hdn.Attributes.Add("name", inputName);
            hdn.Attributes.Add("id", inputName);
            hdn.Attributes.Add("type", "hidden");
            hdn.Attributes.Add("value", this.Value);
            hdn.MergeAttributes(this._inputHtmlAttributes);
            hdn.TagRenderMode = TagRenderMode.SelfClosing;
            html.Replace(PlaceHolders.Input, hdn.ToString());

            // wrapper id
            html.Replace(PlaceHolders.FieldWrapperId, this.GetWrapperId());

            return html.ToString();
        }

        protected override string BuildDefaultTemplate()
        {
            return PlaceHolders.Input;
        }
    }
}