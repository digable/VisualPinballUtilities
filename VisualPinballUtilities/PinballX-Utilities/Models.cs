using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VisualPinballUtilities.PinballX_Utilities
{
    public class Models
    {
        public class VisualPinballTable
        {
            [XmlAttribute("name")]
            public string name { get; set; }
            public string description { get; set; }
            public string rom { get; set; }
            public string manufacturer { get; set; }
            public string year { get; set; }
            public string type { get; set; }
            public string hidedmd { get; set; }
            public string hidebackglass { get; set; }
            public string enabled { get; set; }
            public string rating { get; set; }
        }

        public class VisualPinballItem
        {
            public VisualPinballVersion version { get; set; }
            public List<VisualPinballTable> tables { get; set; } = new List<VisualPinballTable>();
        }

        public enum VisualPinballVersion
        {
            vp9 = 1,
            vp9pm = 2,
            vp10 = 3
        }

        //public class VisualPinballTables
        //{
        //    public 
        //    //[Serializable]
        //    //[XmlRoot("menu")]
        //    //public class menu
        //    //{
        //    //    [XmlArray("game")]
        //    //    public string game { get; set; }
        //    //}
        //    //public class VisualPinballItem
        //    //{
        //    //    [XmlArray("game")]
        //    //    public string name { get; set; }
        //    //    public string rom { get; set; }
        //    //    public string manufacturer { get; set; }
        //    //    public int year { get; set; }
        //    //    public string type { get; set; }
        //    //    public bool hidedmd { get; set; }
        //    //    public bool hidebackglass { get; set; }
        //    //    public bool enabled { get; set; }
        //    //    public decimal rating { get; set; }

        //    //}
        //}
        //    }
        //}
        /// <remarks/>
        //public class VisualPinballTables
        //{
        //    //[System.SerializableAttribute()]
        //    [Serializable]
        //    [XmlRoot("menu")]
        //    //[System.ComponentModel.DesignerCategoryAttribute("code")]
        //    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        //    //[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        //    public partial class menu
        //    {

        //        private menuGame[] gameField;

        //        /// <remarks/>
        //        [System.Xml.Serialization.XmlElementAttribute("game")]
        //        public menuGame[] game
        //        {
        //            get
        //            {
        //                return this.gameField;
        //            }
        //            set
        //            {
        //                this.gameField = value;
        //            }
        //        }
        //    }

        //    /// <remarks/>
        //    [System.SerializableAttribute()]
        //    [System.ComponentModel.DesignerCategoryAttribute("code")]
        //    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        //    public partial class menuGame
        //    {

        //        private string descriptionField;

        //        private object romField;

        //        private string manufacturerField;

        //        private int yearField;

        //        private string typeField;

        //        private bool hidedmdField;

        //        private bool hidebackglassField;

        //        private bool enabledField;

        //        private decimal ratingField;

        //        private string nameField;

        //        /// <remarks/>
        //        public string description
        //        {
        //            get
        //            {
        //                return this.descriptionField;
        //            }
        //            set
        //            {
        //                this.descriptionField = value;
        //            }
        //        }

        //        /// <remarks/>
        //        public object rom
        //        {
        //            get
        //            {
        //                return this.romField;
        //            }
        //            set
        //            {
        //                this.romField = value;
        //            }
        //        }

        //        /// <remarks/>
        //        public string manufacturer
        //        {
        //            get
        //            {
        //                return this.manufacturerField;
        //            }
        //            set
        //            {
        //                this.manufacturerField = value;
        //            }
        //        }

        //        /// <remarks/>
        //        public int year
        //        {
        //            get
        //            {
        //                return this.yearField;
        //            }
        //            set
        //            {
        //                this.yearField = value;
        //            }
        //        }

        //        /// <remarks/>
        //        public string type
        //        {
        //            get
        //            {
        //                return this.typeField;
        //            }
        //            set
        //            {
        //                this.typeField = value;
        //            }
        //        }

        //        /// <remarks/>
        //        public bool hidedmd
        //        {
        //            get
        //            {
        //                return this.hidedmdField;
        //            }
        //            set
        //            {
        //                this.hidedmdField = value;
        //            }
        //        }

        //        /// <remarks/>
        //        public bool hidebackglass
        //        {
        //            get
        //            {
        //                return this.hidebackglassField;
        //            }
        //            set
        //            {
        //                this.hidebackglassField = value;
        //            }
        //        }

        //        /// <remarks/>
        //        public bool enabled
        //        {
        //            get
        //            {
        //                return this.enabledField;
        //            }
        //            set
        //            {
        //                this.enabledField = value;
        //            }
        //        }

        //        /// <remarks/>
        //        public decimal rating
        //        {
        //            get
        //            {
        //                return this.ratingField;
        //            }
        //            set
        //            {
        //                this.ratingField = value;
        //            }
        //        }

        //        /// <remarks/>
        //        [System.Xml.Serialization.XmlAttributeAttribute()]
        //        public string name
        //        {
        //            get
        //            {
        //                return this.nameField;
        //            }
        //            set
        //            {
        //                this.nameField = value;
        //            }
        //        }
        //    }
        //}
    }
}

