<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Range.ascx.cs" Inherits="DynamicDataProject.DynamicData.Filters.Range" %>

<div style="height:10px">
<asp:TextBox ID="MinTextBox" runat="server" Style="display: none;" />
<asp:TextBox ID="MaxTextBox" runat="server" Style="display: none;" />
<asp:TextBox ID="SliderTextBox" runat="server" Style="display: none;" AutoPostBack="true" />

<ajaxToolkit:MultiHandleSliderExtender ID="MultiHandleSliderExtender1" runat="server"
    BehaviorID="MultiHandleSliderExtender1" TargetControlID="SliderTextBox" Minimum="0"
    Maximum="100" Length="175" TooltipText="{0}" EnableHandleAnimation="true"
    EnableKeyboard="false" EnableMouseWheel="false" ShowHandleDragStyle="true" ShowHandleHoverStyle="true">
    <MultiHandleSliderTargets>
        <ajaxToolkit:MultiHandleSliderTarget ControlID="MinTextBox" />
        <ajaxToolkit:MultiHandleSliderTarget ControlID="MaxTextBox" />
    </MultiHandleSliderTargets>
</ajaxToolkit:MultiHandleSliderExtender>
</div>