using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcDynamicForms.NetCore.Enums;
using MvcDynamicForms.NetCore.Fields.Abstract;

namespace MvcDynamicForms.NetCore.Fields
{
    /// <summary>
    /// Represents a list of html radio button inputs.
    /// </summary>
    [Serializable]
    public class RadioList : OrientableField
    {
        public override string RenderHtml()
        {
            var html = new StringBuilder(this.Template);
            var inputName = this.GetHtmlId();

            // prompt label
            var prompt = new TagBuilder("label");
            prompt.AddCssClass(this._promptClass);
            prompt.InnerHtml.AppendHtml(this.GetPrompt());
            html.Replace(PlaceHolders.Prompt, prompt.ToString());

            // error label
            if (!this.ErrorIsClear)
            {
                var error = new TagBuilder("label");
                error.AddCssClass(this._errorClass);
                error.InnerHtml.AppendHtml(this.Error);
                html.Replace(PlaceHolders.Error, error.ToString());
            }

            // list of radio buttons        
            var input = new StringBuilder();
            var ul = new TagBuilder("ul");
            ul.Attributes.Add("class",
                this._orientation == Orientation.Vertical ? this._verticalClass : this._horizontalClass);
            ul.AddCssClass(this._listClass);
            ul.TagRenderMode = TagRenderMode.StartTag;
            input.Append(ul.ToString());

            var choicesList = this._choices.ToList();
            for (int i = 0; i < choicesList.Count; i++)
            {
                ListItem choice = choicesList[i];
                string radId = inputName + i;

                // open list item
                var li = new TagBuilder("li");
                li.TagRenderMode = TagRenderMode.StartTag;
                input.Append(li.ToString());

                // radio button input
                var rad = new TagBuilder("input");
                rad.Attributes.Add("type", "radio");
                rad.Attributes.Add("name", inputName);
                rad.Attributes.Add("id", radId);
                rad.Attributes.Add("value", choice.Value);
                if (choice.Selected)
                    rad.Attributes.Add("checked", "checked");
                rad.MergeAttributes(this._inputHtmlAttributes);
                rad.MergeAttributes(choice.HtmlAttributes);
                rad.TagRenderMode = TagRenderMode.SelfClosing;
                input.Append(rad.ToString());

                // checkbox label
                var lbl = new TagBuilder("label");
                lbl.Attributes.Add("for", radId);
                lbl.Attributes.Add("class", this._inputLabelClass);
                lbl.InnerHtml.AppendHtml(choice.Text);
                input.Append(lbl.ToString());

                // close list item
                li.TagRenderMode = TagRenderMode.EndTag;
                input.Append(li.ToString());
            }
            ul.TagRenderMode = TagRenderMode.EndTag;
            input.Append(ul.ToString());
            html.Replace(PlaceHolders.Input, input.ToString());

            // wrapper id
            html.Replace(PlaceHolders.FieldWrapperId, this.GetWrapperId());

            return html.ToString();
        }
    }
}