﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::System.Data.Objects.DataClasses.EdmSchemaAttribute()]

// Original file name:
// Generation date: 12/12/2008 12:25:13 PM
namespace DynamicDataEFProject
{
    
    /// <summary>
    /// There are no comments for InheritanceEntities in the schema.
    /// </summary>
    public partial class InheritanceEntities : global::System.Data.Objects.ObjectContext
    {
        /// <summary>
        /// Initializes a new InheritanceEntities object using the connection string found in the 'InheritanceEntities' section of the application configuration file.
        /// </summary>
        public InheritanceEntities() : 
                base("name=InheritanceEntities", "InheritanceEntities")
        {
            this.OnContextCreated();
        }
        /// <summary>
        /// Initialize a new InheritanceEntities object.
        /// </summary>
        public InheritanceEntities(string connectionString) : 
                base(connectionString, "InheritanceEntities")
        {
            this.OnContextCreated();
        }
        /// <summary>
        /// Initialize a new InheritanceEntities object.
        /// </summary>
        public InheritanceEntities(global::System.Data.EntityClient.EntityConnection connection) : 
                base(connection, "InheritanceEntities")
        {
            this.OnContextCreated();
        }
        partial void OnContextCreated();
        /// <summary>
        /// There are no comments for Person in the schema.
        /// </summary>
        public global::System.Data.Objects.ObjectQuery<Person> Person
        {
            get
            {
                if ((this._Person == null))
                {
                    this._Person = base.CreateQuery<Person>("[Person]");
                }
                return this._Person;
            }
        }
        private global::System.Data.Objects.ObjectQuery<Person> _Person;
        /// <summary>
        /// There are no comments for Person in the schema.
        /// </summary>
        public void AddToPerson(Person person)
        {
            base.AddObject("Person", person);
        }
    }
    /// <summary>
    /// There are no comments for NORTHWNDModel.Person in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ID
    /// </KeyProperties>
    [global::System.Data.Objects.DataClasses.EdmEntityTypeAttribute(NamespaceName="NORTHWNDModel", Name="Person")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
    [global::System.Serializable()]
    [global::System.Runtime.Serialization.KnownTypeAttribute(typeof(global::DynamicDataProject.Contact))]
    [global::System.Runtime.Serialization.KnownTypeAttribute(typeof(global::DynamicDataProject.Employee))]
    public partial class Person : global::System.Data.Objects.DataClasses.EntityObject
    {
        /// <summary>
        /// Create a new Person object.
        /// </summary>
        /// <param name="id">Initial value of ID.</param>
        /// <param name="firstName">Initial value of FirstName.</param>
        /// <param name="lastName">Initial value of LastName.</param>
        public static Person CreatePerson(int id, string firstName, string lastName)
        {
            Person person = new Person();
            person.ID = id;
            person.FirstName = firstName;
            person.LastName = lastName;
            return person;
        }
        /// <summary>
        /// There are no comments for Property ID in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public int ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this.OnIDChanging(value);
                this.ReportPropertyChanging("ID");
                this._ID = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("ID");
                this.OnIDChanged();
            }
        }
        private int _ID;
        partial void OnIDChanging(int value);
        partial void OnIDChanged();
        /// <summary>
        /// There are no comments for Property FirstName in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string FirstName
        {
            get
            {
                return this._FirstName;
            }
            set
            {
                this.OnFirstNameChanging(value);
                this.ReportPropertyChanging("FirstName");
                this._FirstName = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("FirstName");
                this.OnFirstNameChanged();
            }
        }
        private string _FirstName;
        partial void OnFirstNameChanging(string value);
        partial void OnFirstNameChanged();
        /// <summary>
        /// There are no comments for Property LastName in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string LastName
        {
            get
            {
                return this._LastName;
            }
            set
            {
                this.OnLastNameChanging(value);
                this.ReportPropertyChanging("LastName");
                this._LastName = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("LastName");
                this.OnLastNameChanged();
            }
        }
        private string _LastName;
        partial void OnLastNameChanging(string value);
        partial void OnLastNameChanged();
        /// <summary>
        /// There are no comments for Property Birthdate in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public global::System.Nullable<global::System.DateTime> Birthdate
        {
            get
            {
                return this._Birthdate;
            }
            set
            {
                this.OnBirthdateChanging(value);
                this.ReportPropertyChanging("Birthdate");
                this._Birthdate = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("Birthdate");
                this.OnBirthdateChanged();
            }
        }
        private global::System.Nullable<global::System.DateTime> _Birthdate;
        partial void OnBirthdateChanging(global::System.Nullable<global::System.DateTime> value);
        partial void OnBirthdateChanged();
    }
    /// <summary>
    /// There are no comments for NORTHWNDModel.Contact in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ID
    /// </KeyProperties>
    [global::System.Data.Objects.DataClasses.EdmEntityTypeAttribute(NamespaceName="NORTHWNDModel", Name="Contact")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
    [global::System.Serializable()]
    public partial class Contact : Person
    {
        /// <summary>
        /// Create a new Contact object.
        /// </summary>
        /// <param name="id">Initial value of ID.</param>
        /// <param name="firstName">Initial value of FirstName.</param>
        /// <param name="lastName">Initial value of LastName.</param>
        public static Contact CreateContact(int id, string firstName, string lastName)
        {
            Contact contact = new Contact();
            contact.ID = id;
            contact.FirstName = firstName;
            contact.LastName = lastName;
            return contact;
        }
        /// <summary>
        /// There are no comments for Property EmailAddress in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string EmailAddress
        {
            get
            {
                return this._EmailAddress;
            }
            set
            {
                this.OnEmailAddressChanging(value);
                this.ReportPropertyChanging("EmailAddress");
                this._EmailAddress = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("EmailAddress");
                this.OnEmailAddressChanged();
            }
        }
        private string _EmailAddress;
        partial void OnEmailAddressChanging(string value);
        partial void OnEmailAddressChanged();
        /// <summary>
        /// There are no comments for Property Phone in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string Phone
        {
            get
            {
                return this._Phone;
            }
            set
            {
                this.OnPhoneChanging(value);
                this.ReportPropertyChanging("Phone");
                this._Phone = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("Phone");
                this.OnPhoneChanged();
            }
        }
        private string _Phone;
        partial void OnPhoneChanging(string value);
        partial void OnPhoneChanged();
    }
    /// <summary>
    /// There are no comments for NORTHWNDModel.Employee in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ID
    /// </KeyProperties>
    [global::System.Data.Objects.DataClasses.EdmEntityTypeAttribute(NamespaceName="NORTHWNDModel", Name="Employee")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
    [global::System.Serializable()]
    [global::System.Runtime.Serialization.KnownTypeAttribute(typeof(global::DynamicDataProject.SalesPerson))]
    [global::System.Runtime.Serialization.KnownTypeAttribute(typeof(global::DynamicDataProject.Programmer))]
    public partial class Employee : Person
    {
        /// <summary>
        /// Create a new Employee object.
        /// </summary>
        /// <param name="id">Initial value of ID.</param>
        /// <param name="firstName">Initial value of FirstName.</param>
        /// <param name="lastName">Initial value of LastName.</param>
        public static Employee CreateEmployee(int id, string firstName, string lastName)
        {
            Employee employee = new Employee();
            employee.ID = id;
            employee.FirstName = firstName;
            employee.LastName = lastName;
            return employee;
        }
        /// <summary>
        /// There are no comments for Property Phone in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string Phone
        {
            get
            {
                return this._Phone;
            }
            set
            {
                this.OnPhoneChanging(value);
                this.ReportPropertyChanging("Phone");
                this._Phone = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("Phone");
                this.OnPhoneChanged();
            }
        }
        private string _Phone;
        partial void OnPhoneChanging(string value);
        partial void OnPhoneChanged();
        /// <summary>
        /// There are no comments for Property JobTitle in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string JobTitle
        {
            get
            {
                return this._JobTitle;
            }
            set
            {
                this.OnJobTitleChanging(value);
                this.ReportPropertyChanging("JobTitle");
                this._JobTitle = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("JobTitle");
                this.OnJobTitleChanged();
            }
        }
        private string _JobTitle;
        partial void OnJobTitleChanging(string value);
        partial void OnJobTitleChanged();
        /// <summary>
        /// There are no comments for Property Salary in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public global::System.Nullable<decimal> Salary
        {
            get
            {
                return this._Salary;
            }
            set
            {
                this.OnSalaryChanging(value);
                this.ReportPropertyChanging("Salary");
                this._Salary = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("Salary");
                this.OnSalaryChanged();
            }
        }
        private global::System.Nullable<decimal> _Salary;
        partial void OnSalaryChanging(global::System.Nullable<decimal> value);
        partial void OnSalaryChanged();
        /// <summary>
        /// There are no comments for Property HireDate in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public global::System.Nullable<global::System.DateTime> HireDate
        {
            get
            {
                return this._HireDate;
            }
            set
            {
                this.OnHireDateChanging(value);
                this.ReportPropertyChanging("HireDate");
                this._HireDate = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("HireDate");
                this.OnHireDateChanged();
            }
        }
        private global::System.Nullable<global::System.DateTime> _HireDate;
        partial void OnHireDateChanging(global::System.Nullable<global::System.DateTime> value);
        partial void OnHireDateChanged();
    }
    /// <summary>
    /// There are no comments for NORTHWNDModel.SalesPerson in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ID
    /// </KeyProperties>
    [global::System.Data.Objects.DataClasses.EdmEntityTypeAttribute(NamespaceName="NORTHWNDModel", Name="SalesPerson")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
    [global::System.Serializable()]
    public partial class SalesPerson : Employee
    {
        /// <summary>
        /// Create a new SalesPerson object.
        /// </summary>
        /// <param name="id">Initial value of ID.</param>
        /// <param name="firstName">Initial value of FirstName.</param>
        /// <param name="lastName">Initial value of LastName.</param>
        public static SalesPerson CreateSalesPerson(int id, string firstName, string lastName)
        {
            SalesPerson salesPerson = new SalesPerson();
            salesPerson.ID = id;
            salesPerson.FirstName = firstName;
            salesPerson.LastName = lastName;
            return salesPerson;
        }
        /// <summary>
        /// There are no comments for Property TerritoryName in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string TerritoryName
        {
            get
            {
                return this._TerritoryName;
            }
            set
            {
                this.OnTerritoryNameChanging(value);
                this.ReportPropertyChanging("TerritoryName");
                this._TerritoryName = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("TerritoryName");
                this.OnTerritoryNameChanged();
            }
        }
        private string _TerritoryName;
        partial void OnTerritoryNameChanging(string value);
        partial void OnTerritoryNameChanged();
        /// <summary>
        /// There are no comments for Property CommissionRate in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public global::System.Nullable<decimal> CommissionRate
        {
            get
            {
                return this._CommissionRate;
            }
            set
            {
                this.OnCommissionRateChanging(value);
                this.ReportPropertyChanging("CommissionRate");
                this._CommissionRate = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("CommissionRate");
                this.OnCommissionRateChanged();
            }
        }
        private global::System.Nullable<decimal> _CommissionRate;
        partial void OnCommissionRateChanging(global::System.Nullable<decimal> value);
        partial void OnCommissionRateChanged();
        /// <summary>
        /// There are no comments for Property BaseBonus in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public global::System.Nullable<decimal> BaseBonus
        {
            get
            {
                return this._BaseBonus;
            }
            set
            {
                this.OnBaseBonusChanging(value);
                this.ReportPropertyChanging("BaseBonus");
                this._BaseBonus = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("BaseBonus");
                this.OnBaseBonusChanged();
            }
        }
        private global::System.Nullable<decimal> _BaseBonus;
        partial void OnBaseBonusChanging(global::System.Nullable<decimal> value);
        partial void OnBaseBonusChanged();
    }
    /// <summary>
    /// There are no comments for NORTHWNDModel.Programmer in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ID
    /// </KeyProperties>
    [global::System.Data.Objects.DataClasses.EdmEntityTypeAttribute(NamespaceName="NORTHWNDModel", Name="Programmer")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
    [global::System.Serializable()]
    public partial class Programmer : Employee
    {
        /// <summary>
        /// Create a new Programmer object.
        /// </summary>
        /// <param name="id">Initial value of ID.</param>
        /// <param name="firstName">Initial value of FirstName.</param>
        /// <param name="lastName">Initial value of LastName.</param>
        public static Programmer CreateProgrammer(int id, string firstName, string lastName)
        {
            Programmer programmer = new Programmer();
            programmer.ID = id;
            programmer.FirstName = firstName;
            programmer.LastName = lastName;
            return programmer;
        }
        /// <summary>
        /// There are no comments for Property FavoriteLanguage in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string FavoriteLanguage
        {
            get
            {
                return this._FavoriteLanguage;
            }
            set
            {
                this.OnFavoriteLanguageChanging(value);
                this.ReportPropertyChanging("FavoriteLanguage");
                this._FavoriteLanguage = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("FavoriteLanguage");
                this.OnFavoriteLanguageChanged();
            }
        }
        private string _FavoriteLanguage;
        partial void OnFavoriteLanguageChanging(string value);
        partial void OnFavoriteLanguageChanged();
    }
}