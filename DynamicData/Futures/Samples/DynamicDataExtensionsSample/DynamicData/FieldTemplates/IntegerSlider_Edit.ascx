<%@ Control Language="C#" CodeBehind="IntegerSlider_Edit.ascx.cs" Inherits="DynamicDataExtensionsSample.DynamicData.FieldTemplates.IntegerSlider_EditField" %>

<asp:TextBox ID="TextBox1" runat="server" Text='<%# FieldValueEditString %>' CssClass="droplist"></asp:TextBox>

<asp:Label ID="Label1" runat="server" />

<ajaxToolkit:SliderExtender ID="SliderExtender1" runat="server" 
    TargetControlID="TextBox1" 
    Minimum="-100" 
    Maximum="100"  
    BoundControlID="Label1" />

<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="droplist" ControlToValidate="TextBox1" Display="Dynamic" />