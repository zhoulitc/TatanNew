//namespace Tatan.Data
//{
//    using System.Xml.Serialization;

//    #region 数据库配置类
//    /// <summary>
//    /// 配置文件类，保存DataConfig.xml数据
//    /// </summary>
//    public class DataConfig
//    {
//        [XmlElement]
//        public string Provider { get; set; }

//        [XmlElement]
//        public string ConnectionString { get; set; }

//        [XmlElement]
//        public DBPager DbPager { get; set; }

//        public class DBPager
//        {
//            [XmlAttribute]
//            public string Name { get; set; }
//        }
//    }
//    #endregion
//}