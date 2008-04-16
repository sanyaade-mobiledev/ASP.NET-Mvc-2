﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3021
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace System.Web.Mvc.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class MvcResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MvcResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("System.Web.Mvc.Resources.MvcResources", typeof(MvcResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Order must be greater than or equal to -1..
        /// </summary>
        internal static string ActionFilter_OrderOutOfRange {
            get {
                return ResourceManager.GetString("ActionFilter_OrderOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No route in the route table matches the supplied values..
        /// </summary>
        internal static string ActionRedirectResult_NoRouteMatched {
            get {
                return ResourceManager.GetString("ActionRedirectResult_NoRouteMatched", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value cannot be null or empty..
        /// </summary>
        internal static string Common_NullOrEmpty {
            get {
                return ResourceManager.GetString("Common_NullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The property &apos;{0}&apos; cannot be null or empty..
        /// </summary>
        internal static string Common_PropertyCannotBeNullOrEmpty {
            get {
                return ResourceManager.GetString("Common_PropertyCannotBeNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot call action &apos;{0}&apos; since it is a generic method..
        /// </summary>
        internal static string Controller_ActionCannotBeGeneric {
            get {
                return ResourceManager.GetString("Controller_ActionCannotBeGeneric", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot convert parameter &apos;{0}&apos; in action &apos;{1}&apos; with value &apos;{2}&apos; to type &apos;{3}&apos;..
        /// </summary>
        internal static string Controller_CannotConvertParameter {
            get {
                return ResourceManager.GetString("Controller_CannotConvertParameter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A value is required for parameter &apos;{0}&apos; in action &apos;{1}&apos;. The parameter either has no value or its value could not be converted. To make a parameter optional its type should either be a reference type or a Nullable type..
        /// </summary>
        internal static string Controller_MissingParameter {
            get {
                return ResourceManager.GetString("Controller_MissingParameter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to More than one action named &apos;{0}&apos; was found on controller &apos;{1}&apos;..
        /// </summary>
        internal static string Controller_MoreThanOneAction {
            get {
                return ResourceManager.GetString("Controller_MoreThanOneAction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot set value for parameter &apos;{0}&apos; in action &apos;{1}&apos;. Parameters passed by reference are not supported in action methods..
        /// </summary>
        internal static string Controller_ReferenceParametersNotSupported {
            get {
                return ResourceManager.GetString("Controller_ReferenceParametersNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A public action method &apos;{0}&apos; could not be found on controller &apos;{1}&apos;..
        /// </summary>
        internal static string Controller_UnknownAction {
            get {
                return ResourceManager.GetString("Controller_UnknownAction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The action method &apos;{0}&apos; on controller &apos;{1}&apos; has two filter attributes with filter order {2}. If a filter specifies an order of 0 or greater, no other filter on that action method may specify that same order..
        /// </summary>
        internal static string ControllerActionInvoker_FiltersOnMethodHaveDuplicateOrder {
            get {
                return ResourceManager.GetString("ControllerActionInvoker_FiltersOnMethodHaveDuplicateOrder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Two filter attributes on controller &apos;{0}&apos; have filter order {1}. If a filter specifies an order of 0 or greater, no other filter on that type may specify that same order..
        /// </summary>
        internal static string ControllerActionInvoker_FiltersOnTypeHaveDuplicateOrder {
            get {
                return ResourceManager.GetString("ControllerActionInvoker_FiltersOnTypeHaveDuplicateOrder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The action method &apos;{0}&apos; on controller &apos;{1}&apos; returned a value of type &apos;{2}&apos;. Action methods must return an ActionResult..
        /// </summary>
        internal static string ControllerActionInvoker_MethodReturnedWrongType {
            get {
                return ResourceManager.GetString("ControllerActionInvoker_MethodReturnedWrongType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The action method &apos;{0}&apos; on controller &apos;{1}&apos; has return type &apos;{2}&apos;. Action methods must return an ActionResult..
        /// </summary>
        internal static string ControllerActionInvoker_MethodSignatureHasInvalidReturnType {
            get {
                return ResourceManager.GetString("ControllerActionInvoker_MethodSignatureHasInvalidReturnType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The parameters specified in the dictionary do not match those of the method &apos;{0}&apos;..
        /// </summary>
        internal static string ControllerActionInvoker_ParameterDictionaryIsInvalid {
            get {
                return ResourceManager.GetString("ControllerActionInvoker_ParameterDictionaryIsInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was an error creating the IControllerFactory &apos;{0}&apos;. Check that it has a public parameterless constructor..
        /// </summary>
        internal static string ControllerBuilder_ErrorCreatingControllerFactory {
            get {
                return ResourceManager.GetString("ControllerBuilder_ErrorCreatingControllerFactory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The IControllerFactory &apos;{0}&apos; did not return a controller for a controller named &apos;{1}&apos;..
        /// </summary>
        internal static string ControllerBuilder_FactoryReturnedNull {
            get {
                return ResourceManager.GetString("ControllerBuilder_FactoryReturnedNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The controller factory type &apos;{0}&apos; must implement the IControllerFactory interface..
        /// </summary>
        internal static string ControllerBuilder_MissingIControllerFactory {
            get {
                return ResourceManager.GetString("ControllerBuilder_MissingIControllerFactory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Duplicate controller types found for &apos;{0}&apos;. To disambiguate the controller set the controller&apos;s namespace in the route..
        /// </summary>
        internal static string DefaultControllerFactory_DuplicateControllers {
            get {
                return ResourceManager.GetString("DefaultControllerFactory_DuplicateControllers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while creating a controller of type &apos;{0}&apos;. If the controller doesn&apos;t have a controller factory, ensure that it has a parameterless public constructor..
        /// </summary>
        internal static string DefaultControllerFactory_ErrorCreatingController {
            get {
                return ResourceManager.GetString("DefaultControllerFactory_ErrorCreatingController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The controller type &apos;{0}&apos; must implement the IController interface..
        /// </summary>
        internal static string DefaultControllerFactory_MissingIController {
            get {
                return ResourceManager.GetString("DefaultControllerFactory_MissingIController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The controller for path &apos;{0}&apos; could not be found or it does not implement the IController interface..
        /// </summary>
        internal static string DefaultControllerFactory_NoControllerFound {
            get {
                return ResourceManager.GetString("DefaultControllerFactory_NoControllerFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The provided object or dictionary already contains a definition for &apos;{0}&apos;..
        /// </summary>
        internal static string Helper_DictionaryAlreadyContainsKey {
            get {
                return ResourceManager.GetString("Helper_DictionaryAlreadyContainsKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The parameter must be set to an array of location format strings, such as &quot;~/Views/{1}/{0}.aspx&quot;..
        /// </summary>
        internal static string ViewLocator_LocationsRequired {
            get {
                return ResourceManager.GetString("ViewLocator_LocationsRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The master &apos;{0}&apos; could not be located at these paths: {1}.
        /// </summary>
        internal static string ViewLocator_MasterNotFound {
            get {
                return ResourceManager.GetString("ViewLocator_MasterNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The view &apos;{0}&apos; could not be located at these paths: {1}.
        /// </summary>
        internal static string ViewLocator_ViewNotFound {
            get {
                return ResourceManager.GetString("ViewLocator_ViewNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A ViewMasterPage can only be used with content pages that derive from ViewPage or ViewPage&lt;TViewData&gt;..
        /// </summary>
        internal static string ViewMasterPage_RequiresViewPage {
            get {
                return ResourceManager.GetString("ViewMasterPage_RequiresViewPage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A ViewMasterPage&lt;TViewData&gt; can only be used with content pages that derive from ViewPage&lt;TViewData&gt;..
        /// </summary>
        internal static string ViewMasterPage_RequiresViewPageTViewData {
            get {
                return ResourceManager.GetString("ViewMasterPage_RequiresViewPageTViewData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The view data passed into the page is of type &apos;{0}&apos; but this page requires view data of type &apos;{1}&apos;..
        /// </summary>
        internal static string ViewPageTViewData_WrongViewDataType {
            get {
                return ResourceManager.GetString("ViewPageTViewData_WrongViewDataType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The ViewUserControl &apos;{0}&apos; cannot find an IViewDataContainer. The ViewUserControl must be inside a ViewPage, ViewMasterPage, or another ViewUserControl..
        /// </summary>
        internal static string ViewUserControl_RequiresViewDataProvider {
            get {
                return ResourceManager.GetString("ViewUserControl_RequiresViewDataProvider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A ViewUserControl can only be used inside pages that derive from ViewPage or ViewPage&lt;TViewData&gt;..
        /// </summary>
        internal static string ViewUserControl_RequiresViewPage {
            get {
                return ResourceManager.GetString("ViewUserControl_RequiresViewPage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The view data for ViewUserControl &apos;{0}&apos; could not be found or is not of the type &apos;{1}&apos;..
        /// </summary>
        internal static string ViewUserControl_WrongTViewDataType {
            get {
                return ResourceManager.GetString("ViewUserControl_WrongTViewDataType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The master &apos;{0}&apos; could not be found..
        /// </summary>
        internal static string WebFormViewEngine_MasterNotFound {
            get {
                return ResourceManager.GetString("WebFormViewEngine_MasterNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A master name cannot be specified when the view is a ViewUserControl..
        /// </summary>
        internal static string WebFormViewEngine_UserControlCannotHaveMaster {
            get {
                return ResourceManager.GetString("WebFormViewEngine_UserControlCannotHaveMaster", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The view found at &apos;{0}&apos; could not be created..
        /// </summary>
        internal static string WebFormViewEngine_ViewCouldNotBeCreated {
            get {
                return ResourceManager.GetString("WebFormViewEngine_ViewCouldNotBeCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The view &apos;{0}&apos; could not be found..
        /// </summary>
        internal static string WebFormViewEngine_ViewNotFound {
            get {
                return ResourceManager.GetString("WebFormViewEngine_ViewNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The view at &apos;{0}&apos; must derive from ViewPage, ViewPage&lt;TViewData&gt;, ViewUserControl, or ViewUserControl&lt;TViewData&gt;..
        /// </summary>
        internal static string WebFormViewEngine_WrongViewBase {
            get {
                return ResourceManager.GetString("WebFormViewEngine_WrongViewBase", resourceCulture);
            }
        }
    }
}
