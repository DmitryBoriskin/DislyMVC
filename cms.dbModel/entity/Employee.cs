﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace cms.dbModel.entity
{
    [Serializable()]
    [XmlRoot(Namespace = "Employee", IsNullable = true)]
    public class ArrayOfEmployee
    {
        [XmlElement(ElementName = "Employee")]
        public Employee[] Employees { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=true)]
    public partial class Employee
    {

        private System.Guid idField;

        private decimal populationField;

        private string tabelNumberField;

        private LPU uzField;

        private System.DateTime changeTimeField;

        private Territory regionField;

        private string nameField;

        private string surnameField;

        private string patronameField;

        private SexEnum sexField;

        private System.Nullable<System.DateTime> birthdateField;

        private System.Nullable<System.DateTime> deathdateField;

        private DocumentEntity documentField;

        private string sNILSField;

        private string iNNField;

        private AddressEntity[] addressesField;

        private string phoneField;

        private Marriage marriageStateField;

        private Сitizenship citezenshipStateField;

        private System.Nullable<bool> isRealPersonField;

        private System.Nullable<bool> hasAutoField;

        private System.Nullable<bool> hasChildrenField;

        private Award[] employeeAwardsField;

        private CardRecord[] employeeRecordsField;

        private DiplomaEducation[] employeeSpecialitiesField;

        private PostGraduateEducation[] employeePostGraduateEducationField;

        private SertificateEducation[] employeeSertificateEducationField;

        private SkillImprovement[] employeeSkillImprovementField;

        private Retrainment[] employeeRetrainmentField;

        private Qualification[] employeeQualificationField;

        /// <remarks/>
        public System.Guid ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public decimal Population
        {
            get
            {
                return this.populationField;
            }
            set
            {
                this.populationField = value;
            }
        }

        /// <remarks/>
        public string TabelNumber
        {
            get
            {
                return this.tabelNumberField;
            }
            set
            {
                this.tabelNumberField = value;
            }
        }

        /// <remarks/>
        public LPU UZ
        {
            get
            {
                return this.uzField;
            }
            set
            {
                this.uzField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ChangeTime
        {
            get
            {
                return this.changeTimeField;
            }
            set
            {
                this.changeTimeField = value;
            }
        }

        /// <remarks/>
        public Territory Region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Surname
        {
            get
            {
                return this.surnameField;
            }
            set
            {
                this.surnameField = value;
            }
        }

        /// <remarks/>
        public string Patroname
        {
            get
            {
                return this.patronameField;
            }
            set
            {
                this.patronameField = value;
            }
        }

        /// <remarks/>
        public SexEnum Sex
        {
            get
            {
                return this.sexField;
            }
            set
            {
                this.sexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<System.DateTime> Birthdate
        {
            get
            {
                return this.birthdateField;
            }
            set
            {
                this.birthdateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<System.DateTime> Deathdate
        {
            get
            {
                return this.deathdateField;
            }
            set
            {
                this.deathdateField = value;
            }
        }

        /// <remarks/>
        public DocumentEntity Document
        {
            get
            {
                return this.documentField;
            }
            set
            {
                this.documentField = value;
            }
        }

        /// <remarks/>
        public string SNILS
        {
            get
            {
                return this.sNILSField;
            }
            set
            {
                this.sNILSField = value;
            }
        }

        /// <remarks/>
        public string INN
        {
            get
            {
                return this.iNNField;
            }
            set
            {
                this.iNNField = value;
            }
        }

        /// <remarks/>
        public AddressEntity[] Addresses
        {
            get
            {
                return this.addressesField;
            }
            set
            {
                this.addressesField = value;
            }
        }

        /// <remarks/>
        public string Phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }

        /// <remarks/>
        public Marriage MarriageState
        {
            get
            {
                return this.marriageStateField;
            }
            set
            {
                this.marriageStateField = value;
            }
        }

        /// <remarks/>
        public Сitizenship CitezenshipState
        {
            get
            {
                return this.citezenshipStateField;
            }
            set
            {
                this.citezenshipStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> IsRealPerson
        {
            get
            {
                return this.isRealPersonField;
            }
            set
            {
                this.isRealPersonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> HasAuto
        {
            get
            {
                return this.hasAutoField;
            }
            set
            {
                this.hasAutoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> HasChildren
        {
            get
            {
                return this.hasChildrenField;
            }
            set
            {
                this.hasChildrenField = value;
            }
        }

        /// <remarks/>
        public Award[] EmployeeAwards
        {
            get
            {
                return this.employeeAwardsField;
            }
            set
            {
                this.employeeAwardsField = value;
            }
        }

        /// <remarks/>
        public CardRecord[] EmployeeRecords
        {
            get
            {
                return this.employeeRecordsField;
            }
            set
            {
                this.employeeRecordsField = value;
            }
        }

        /// <remarks/>
        public DiplomaEducation[] EmployeeSpecialities
        {
            get
            {
                return this.employeeSpecialitiesField;
            }
            set
            {
                this.employeeSpecialitiesField = value;
            }
        }

        /// <remarks/>
        public PostGraduateEducation[] EmployeePostGraduateEducation
        {
            get
            {
                return this.employeePostGraduateEducationField;
            }
            set
            {
                this.employeePostGraduateEducationField = value;
            }
        }

        /// <remarks/>
        public SertificateEducation[] EmployeeSertificateEducation
        {
            get
            {
                return this.employeeSertificateEducationField;
            }
            set
            {
                this.employeeSertificateEducationField = value;
            }
        }

        /// <remarks/>
        public SkillImprovement[] EmployeeSkillImprovement
        {
            get
            {
                return this.employeeSkillImprovementField;
            }
            set
            {
                this.employeeSkillImprovementField = value;
            }
        }

        /// <remarks/>
        public Retrainment[] EmployeeRetrainment
        {
            get
            {
                return this.employeeRetrainmentField;
            }
            set
            {
                this.employeeRetrainmentField = value;
            }
        }

        /// <remarks/>
        public Qualification[] EmployeeQualification
        {
            get
            {
                return this.employeeQualificationField;
            }
            set
            {
                this.employeeQualificationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LPU
    {

        private System.Guid idField;

        private string nameField;

        private string iNNField;

        private string kPPField;

        private OrganisationType[] typeField;

        private Level lPULevelField;

        private Nomenclature nomenField;

        /// <remarks/>
        public System.Guid ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string INN
        {
            get
            {
                return this.iNNField;
            }
            set
            {
                this.iNNField = value;
            }
        }

        /// <remarks/>
        public string KPP
        {
            get
            {
                return this.kPPField;
            }
            set
            {
                this.kPPField = value;
            }
        }

        /// <remarks/>
        public OrganisationType[] Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public Level LPULevel
        {
            get
            {
                return this.lPULevelField;
            }
            set
            {
                this.lPULevelField = value;
            }
        }

        /// <remarks/>
        public Nomenclature Nomen
        {
            get
            {
                return this.nomenField;
            }
            set
            {
                this.nomenField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class OrganisationType
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class QualificationCategory
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Qualification
    {

        private QualificationCategory categoryField;

        private SertificateSpeciality specialityField;

        private decimal yearField;

        /// <remarks/>
        public QualificationCategory Category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        /// <remarks/>
        public SertificateSpeciality Speciality
        {
            get
            {
                return this.specialityField;
            }
            set
            {
                this.specialityField = value;
            }
        }

        /// <remarks/>
        public decimal Year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SertificateSpeciality
    {

        private int idField;

        private System.Nullable<int> parentField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<int> Parent
        {
            get
            {
                return this.parentField;
            }
            set
            {
                this.parentField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Retrainment
    {

        private EducationInstitution organisationField;

        private SertificateSpeciality educationSpecialityField;

        private string diplomaSerieField;

        private string diplomaNumberField;

        private decimal hoursField;

        private decimal trainingDateField;

        /// <remarks/>
        public EducationInstitution Organisation
        {
            get
            {
                return this.organisationField;
            }
            set
            {
                this.organisationField = value;
            }
        }

        /// <remarks/>
        public SertificateSpeciality EducationSpeciality
        {
            get
            {
                return this.educationSpecialityField;
            }
            set
            {
                this.educationSpecialityField = value;
            }
        }

        /// <remarks/>
        public string DiplomaSerie
        {
            get
            {
                return this.diplomaSerieField;
            }
            set
            {
                this.diplomaSerieField = value;
            }
        }

        /// <remarks/>
        public string DiplomaNumber
        {
            get
            {
                return this.diplomaNumberField;
            }
            set
            {
                this.diplomaNumberField = value;
            }
        }

        /// <remarks/>
        public decimal Hours
        {
            get
            {
                return this.hoursField;
            }
            set
            {
                this.hoursField = value;
            }
        }

        /// <remarks/>
        public decimal TrainingDate
        {
            get
            {
                return this.trainingDateField;
            }
            set
            {
                this.trainingDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EducationInstitution
    {

        private int idField;

        private string nameField;

        private System.Nullable<int> parentField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<int> Parent
        {
            get
            {
                return this.parentField;
            }
            set
            {
                this.parentField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SkillImprovement
    {

        private EducationInstitution organisationField;

        private string cycleField;

        private decimal hoursField;

        private decimal yearField;

        private string diplomaSerieField;

        private string diplomaNumberField;

        private System.Nullable<System.DateTime> issueDateField;

        private SertificateSpeciality educationSpecialityField;

        /// <remarks/>
        public EducationInstitution Organisation
        {
            get
            {
                return this.organisationField;
            }
            set
            {
                this.organisationField = value;
            }
        }

        /// <remarks/>
        public string Cycle
        {
            get
            {
                return this.cycleField;
            }
            set
            {
                this.cycleField = value;
            }
        }

        /// <remarks/>
        public decimal Hours
        {
            get
            {
                return this.hoursField;
            }
            set
            {
                this.hoursField = value;
            }
        }

        /// <remarks/>
        public decimal Year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
            }
        }

        /// <remarks/>
        public string DiplomaSerie
        {
            get
            {
                return this.diplomaSerieField;
            }
            set
            {
                this.diplomaSerieField = value;
            }
        }

        /// <remarks/>
        public string DiplomaNumber
        {
            get
            {
                return this.diplomaNumberField;
            }
            set
            {
                this.diplomaNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<System.DateTime> IssueDate
        {
            get
            {
                return this.issueDateField;
            }
            set
            {
                this.issueDateField = value;
            }
        }

        /// <remarks/>
        public SertificateSpeciality EducationSpeciality
        {
            get
            {
                return this.educationSpecialityField;
            }
            set
            {
                this.educationSpecialityField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SertificateEducation
    {

        private EducationInstitution issueOrgField;

        private System.Nullable<System.DateTime> issueDateField;

        private string sertificateSerieField;

        private string sertificateNumberField;

        private SertificateSpeciality educationSpecialityField;

        /// <remarks/>
        public EducationInstitution IssueOrg
        {
            get
            {
                return this.issueOrgField;
            }
            set
            {
                this.issueOrgField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<System.DateTime> IssueDate
        {
            get
            {
                return this.issueDateField;
            }
            set
            {
                this.issueDateField = value;
            }
        }

        /// <remarks/>
        public string SertificateSerie
        {
            get
            {
                return this.sertificateSerieField;
            }
            set
            {
                this.sertificateSerieField = value;
            }
        }

        /// <remarks/>
        public string SertificateNumber
        {
            get
            {
                return this.sertificateNumberField;
            }
            set
            {
                this.sertificateNumberField = value;
            }
        }

        /// <remarks/>
        public SertificateSpeciality EducationSpeciality
        {
            get
            {
                return this.educationSpecialityField;
            }
            set
            {
                this.educationSpecialityField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AcademicDegree
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PostGraduationEducationType
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PostGraduateEducation
    {

        private EducationInstitution baseOrgField;

        private PostGraduationEducationType typeField;

        private System.Nullable<System.DateTime> dateBeginField;

        private System.Nullable<System.DateTime> dateEndField;

        private System.Nullable<System.DateTime> dateDocumField;

        private AcademicDegree degreeField;

        private string diplomaSerieField;

        private string diplomaNumberField;

        private SertificateSpeciality postGraduationSpecialityField;

        /// <remarks/>
        public EducationInstitution BaseOrg
        {
            get
            {
                return this.baseOrgField;
            }
            set
            {
                this.baseOrgField = value;
            }
        }

        /// <remarks/>
        public PostGraduationEducationType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<System.DateTime> DateBegin
        {
            get
            {
                return this.dateBeginField;
            }
            set
            {
                this.dateBeginField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<System.DateTime> DateEnd
        {
            get
            {
                return this.dateEndField;
            }
            set
            {
                this.dateEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<System.DateTime> DateDocum
        {
            get
            {
                return this.dateDocumField;
            }
            set
            {
                this.dateDocumField = value;
            }
        }

        /// <remarks/>
        public AcademicDegree Degree
        {
            get
            {
                return this.degreeField;
            }
            set
            {
                this.degreeField = value;
            }
        }

        /// <remarks/>
        public string DiplomaSerie
        {
            get
            {
                return this.diplomaSerieField;
            }
            set
            {
                this.diplomaSerieField = value;
            }
        }

        /// <remarks/>
        public string DiplomaNumber
        {
            get
            {
                return this.diplomaNumberField;
            }
            set
            {
                this.diplomaNumberField = value;
            }
        }

        /// <remarks/>
        public SertificateSpeciality PostGraduationSpeciality
        {
            get
            {
                return this.postGraduationSpecialityField;
            }
            set
            {
                this.postGraduationSpecialityField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Speciality
    {

        private int idField;

        private System.Nullable<int> parentField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<int> Parent
        {
            get
            {
                return this.parentField;
            }
            set
            {
                this.parentField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EducationType
    {

        private long idField;

        private string nameField;

        /// <remarks/>
        public long ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DiplomaEducation
    {

        private EducationInstitution graduatedFromField;

        private EducationType typeField;

        private decimal graduationDateField;

        private string diplomaSerieField;

        private string diplomaNumberField;

        private Speciality graduationSpecialityField;

        /// <remarks/>
        public EducationInstitution GraduatedFrom
        {
            get
            {
                return this.graduatedFromField;
            }
            set
            {
                this.graduatedFromField = value;
            }
        }

        /// <remarks/>
        public EducationType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public decimal GraduationDate
        {
            get
            {
                return this.graduationDateField;
            }
            set
            {
                this.graduationDateField = value;
            }
        }

        /// <remarks/>
        public string DiplomaSerie
        {
            get
            {
                return this.diplomaSerieField;
            }
            set
            {
                this.diplomaSerieField = value;
            }
        }

        /// <remarks/>
        public string DiplomaNumber
        {
            get
            {
                return this.diplomaNumberField;
            }
            set
            {
                this.diplomaNumberField = value;
            }
        }

        /// <remarks/>
        public Speciality GraduationSpeciality
        {
            get
            {
                return this.graduationSpecialityField;
            }
            set
            {
                this.graduationSpecialityField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Post
    {

        private int idField;

        private System.Nullable<int> parentField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<int> Parent
        {
            get
            {
                return this.parentField;
            }
            set
            {
                this.parentField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PositionType
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Military
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Regime
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RecordTypeOut
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RecordTypeIn
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Subdivision
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Position
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class MedicalCondition
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class MedicalCare
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CardRecord
    {

        private decimal durationField;

        private string organisationField;

        private OrganisationType orgTypeField;

        private MedicalCare careField;

        private MedicalCondition conditionsField;

        private Position recrodPositionField;

        private Subdivision recordSubdivisionField;

        private RecordTypeIn typeInField;

        private string orderInField;

        private RecordTypeOut typeOutField;

        private string orderOutField;

        private System.Nullable<System.DateTime> dateBeginField;

        private System.Nullable<System.DateTime> dateEndField;

        private Regime recordRegimeField;

        private string unitField;

        private Military recordMilitaryField;

        private decimal wageField;

        private PositionType recordPositionTypeField;

        private Post recordPostField;

        /// <remarks/>
        public decimal Duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
            }
        }

        /// <remarks/>
        public string Organisation
        {
            get
            {
                return this.organisationField;
            }
            set
            {
                this.organisationField = value;
            }
        }

        /// <remarks/>
        public OrganisationType OrgType
        {
            get
            {
                return this.orgTypeField;
            }
            set
            {
                this.orgTypeField = value;
            }
        }

        /// <remarks/>
        public MedicalCare Care
        {
            get
            {
                return this.careField;
            }
            set
            {
                this.careField = value;
            }
        }

        /// <remarks/>
        public MedicalCondition Conditions
        {
            get
            {
                return this.conditionsField;
            }
            set
            {
                this.conditionsField = value;
            }
        }

        /// <remarks/>
        public Position RecrodPosition
        {
            get
            {
                return this.recrodPositionField;
            }
            set
            {
                this.recrodPositionField = value;
            }
        }

        /// <remarks/>
        public Subdivision RecordSubdivision
        {
            get
            {
                return this.recordSubdivisionField;
            }
            set
            {
                this.recordSubdivisionField = value;
            }
        }

        /// <remarks/>
        public RecordTypeIn TypeIn
        {
            get
            {
                return this.typeInField;
            }
            set
            {
                this.typeInField = value;
            }
        }

        /// <remarks/>
        public string OrderIn
        {
            get
            {
                return this.orderInField;
            }
            set
            {
                this.orderInField = value;
            }
        }

        /// <remarks/>
        public RecordTypeOut TypeOut
        {
            get
            {
                return this.typeOutField;
            }
            set
            {
                this.typeOutField = value;
            }
        }

        /// <remarks/>
        public string OrderOut
        {
            get
            {
                return this.orderOutField;
            }
            set
            {
                this.orderOutField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<System.DateTime> DateBegin
        {
            get
            {
                return this.dateBeginField;
            }
            set
            {
                this.dateBeginField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<System.DateTime> DateEnd
        {
            get
            {
                return this.dateEndField;
            }
            set
            {
                this.dateEndField = value;
            }
        }

        /// <remarks/>
        public Regime RecordRegime
        {
            get
            {
                return this.recordRegimeField;
            }
            set
            {
                this.recordRegimeField = value;
            }
        }

        /// <remarks/>
        public string Unit
        {
            get
            {
                return this.unitField;
            }
            set
            {
                this.unitField = value;
            }
        }

        /// <remarks/>
        public Military RecordMilitary
        {
            get
            {
                return this.recordMilitaryField;
            }
            set
            {
                this.recordMilitaryField = value;
            }
        }

        /// <remarks/>
        public decimal Wage
        {
            get
            {
                return this.wageField;
            }
            set
            {
                this.wageField = value;
            }
        }

        /// <remarks/>
        public PositionType RecordPositionType
        {
            get
            {
                return this.recordPositionTypeField;
            }
            set
            {
                this.recordPositionTypeField = value;
            }
        }

        /// <remarks/>
        public Post RecordPost
        {
            get
            {
                return this.recordPostField;
            }
            set
            {
                this.recordPostField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Award
    {

        private string numberField;

        private string nameField;

        private System.Nullable<System.DateTime> issuedField;

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<System.DateTime> Issued
        {
            get
            {
                return this.issuedField;
            }
            set
            {
                this.issuedField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Сitizenship
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Marriage
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RegistrationType
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class KLADR
    {

        private long idField;

        private string nameField;

        private string prefixField;

        private System.Nullable<long> parentField;

        /// <remarks/>
        public long ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Prefix
        {
            get
            {
                return this.prefixField;
            }
            set
            {
                this.prefixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<long> Parent
        {
            get
            {
                return this.parentField;
            }
            set
            {
                this.parentField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddressEntity
    {

        private Territory regionField;

        private KLADR districtField;

        private KLADR cityField;

        private string streetField;

        private string houseField;

        private string buildingField;

        private string apartmentField;

        private RegistrationType registrationField;

        private System.Nullable<System.DateTime> registrationDateField;

        /// <remarks/>
        public Territory Region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }

        /// <remarks/>
        public KLADR District
        {
            get
            {
                return this.districtField;
            }
            set
            {
                this.districtField = value;
            }
        }

        /// <remarks/>
        public KLADR City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public string Street
        {
            get
            {
                return this.streetField;
            }
            set
            {
                this.streetField = value;
            }
        }

        /// <remarks/>
        public string House
        {
            get
            {
                return this.houseField;
            }
            set
            {
                this.houseField = value;
            }
        }

        /// <remarks/>
        public string Building
        {
            get
            {
                return this.buildingField;
            }
            set
            {
                this.buildingField = value;
            }
        }

        /// <remarks/>
        public string Apartment
        {
            get
            {
                return this.apartmentField;
            }
            set
            {
                this.apartmentField = value;
            }
        }

        /// <remarks/>
        public RegistrationType Registration
        {
            get
            {
                return this.registrationField;
            }
            set
            {
                this.registrationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<System.DateTime> RegistrationDate
        {
            get
            {
                return this.registrationDateField;
            }
            set
            {
                this.registrationDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Territory
    {

        private int idField;

        private string nameField;

        private System.Nullable<int> parentField;

        private System.Nullable<int> orderField;

        private string oUZField;

        private System.Nullable<long> kLADRField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<int> Parent
        {
            get
            {
                return this.parentField;
            }
            set
            {
                this.parentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<int> Order
        {
            get
            {
                return this.orderField;
            }
            set
            {
                this.orderField = value;
            }
        }

        /// <remarks/>
        public string OUZ
        {
            get
            {
                return this.oUZField;
            }
            set
            {
                this.oUZField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<long> KLADR
        {
            get
            {
                return this.kLADRField;
            }
            set
            {
                this.kLADRField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DocumentType
    {

        private int idField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DocumentEntity
    {

        private DocumentType typeField;

        private string serieField;

        private string numberField;

        private string issuedField;

        private System.Nullable<System.DateTime> issueDateField;

        /// <remarks/>
        public DocumentType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string Serie
        {
            get
            {
                return this.serieField;
            }
            set
            {
                this.serieField = value;
            }
        }

        /// <remarks/>
        public string Number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        public string Issued
        {
            get
            {
                return this.issuedField;
            }
            set
            {
                this.issuedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<System.DateTime> IssueDate
        {
            get
            {
                return this.issueDateField;
            }
            set
            {
                this.issueDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Nomenclature
    {

        private int idField;

        private System.Nullable<int> parentField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<int> Parent
        {
            get
            {
                return this.parentField;
            }
            set
            {
                this.parentField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Level
    {

        private int idField;

        private System.Nullable<int> parentField;

        private string nameField;

        /// <remarks/>
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<int> Parent
        {
            get
            {
                return this.parentField;
            }
            set
            {
                this.parentField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    public enum SexEnum
    {

        /// <remarks/>
        Male,

        /// <remarks/>
        Female,
    }

}
