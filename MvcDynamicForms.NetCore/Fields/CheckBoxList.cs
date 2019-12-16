﻿using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcDynamicForms.NetCore.Enums;
using MvcDynamicForms.NetCore.Fields.Abstract;

namespace MvcDynamicForms.NetCore.Fields
{
    /// <summary>
    /// Represents a list of html checkbox inputs.
    /// </summary>
    [Serializable]
    public class CheckBoxList : OrientableField
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
                ;
                error.InnerHtml.AppendHtml(this.Error);
                html.Replace(PlaceHolders.Error, error.ToString());
            }

            // list of checkboxes
            var input = new StringBuilder();
            var ul = new TagBuilder("ul");
            ul.AddCssClass(this._orientation == Orientation.Vertical ? this._verticalClass : this._horizontalClass);
            ul.AddCssClass(this._listClass);
            ul.TagRenderMode = TagRenderMode.StartTag;
            input.Append(ul.InnerHtml.ToString());

            var choicesList = this._choices.ToList();
            for (int i = 0; i < choicesList.Count; i++)
            {
                ListItem choice = choicesList[i];
                string chkId = inputName + i;

                // open list item
                var li = new TagBuilder("li");
                li.TagRenderMode = TagRenderMode.StartTag;
                input.Append(li.ToString());

                // checkbox input
                var chk = new TagBuilder("input");
                chk.Attributes.Add("type", "checkbox");
                chk.Attributes.Add("name", inputName);
                chk.Attributes.Add("id", chkId);
                chk.Attributes.Add("value", choice.Value);
                if (choice.Selected)
                    chk.Attributes.Add("checked", "checked");
                chk.MergeAttributes(this._inputHtmlAttributes);
                chk.MergeAttributes(choice.HtmlAttributes);
                chk.TagRenderMode = TagRenderMode.SelfClosing;
                input.Append(chk.ToString());

                // checkbox label
                var lbl = new TagBuilder("label");
                lbl.Attributes.Add("for", chkId);
                lbl.AddCssClass(this._inputLabelClass);
                lbl.InnerHtml.AppendHtml(choice.Text);
                input.Append(lbl.ToString());
                lbl.TagRenderMode = TagRenderMode.EndTag;
                // close list item
                input.Append(li.ToString());
            }
            ul.TagRenderMode = TagRenderMode.EndTag;
            input.Append(ul.ToString());

            // add hidden tag, so that a value always gets sent
            var hidden = new TagBuilder("input");
            hidden.Attributes.Add("type", "hidden");
            hidden.Attributes.Add("id", inputName + "_hidden");
            hidden.Attributes.Add("name", inputName);
            hidden.Attributes.Add("value", string.Empty);
            hidden.TagRenderMode = TagRenderMode.SelfClosing;
            html.Replace(PlaceHolders.Input, input.ToString() + hidden.ToString());

            // wrapper id
            html.Replace(PlaceHolders.FieldWrapperId, this.GetWrapperId());

            return html.ToString();
        }
    }
}