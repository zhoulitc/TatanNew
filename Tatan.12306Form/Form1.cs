using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tatan.Common.Extension.String.Codec;
using Tatan._12306Logic.Common;
using Tatan._12306Logic.Login;
using Tatan._12306Logic.Order;
using Tatan._12306Logic.Query;

namespace Tatan._12306Form
{
    public partial class Form1 : Form
    {
        private readonly IDictionary<string, string> _entity = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _entity["username"] = textBox1.Text;
            _entity["password"] = textBox2.Text;
            _entity["code"] = code1.Text;
            try
            {
                if (!LoginHandler.Request(_entity))
                {
                    MessageBox.Show(@"login fail.");

                    Init();
                    LoginHandler.Init(_entity);
                    randCode.LoadAsync(LoginHandler.GetCode(_entity));
                }
                else
                {
                    MessageBox.Show(@"login ok.");
                    QueryHandler.Init(_entity);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void randCode_Click(object sender, EventArgs e)
        {
            randCode.LoadAsync(LoginHandler.GetCode(_entity));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = @"zhoulitc";
            textBox2.Text = @"82508891zl";
            LoginHandler.InitAfter += ExtensionHandler.HandleDynamicJs;
            QueryHandler.InitAfter += ExtensionHandler.HandleDynamicJs;
            OrderHandler.InitAfter += ExtensionHandler.HandleDynamicJs;

            Init();
            LoginHandler.Init(_entity);
            randCode.LoadAsync(LoginHandler.GetCode(_entity));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var s = "3%2C0%2C1%2C%E5%91%A8%E8%A0%A1%2C1%2C431021198805280038%2C13686893341%2CN".AsDecode(Coding.Url);
            var s1 = "Tue+Mar+10+2015+00%3A00%3A00+GMT%2B0800+(%E4%B8%AD%E5%9B%BD%E6%A0%87%E5%87%86%E6%97%B6%E9%97%B4)".AsDecode(Coding.Url);
            var cookie = string.Format("; _jc_save_fromStation={0}; _jc_save_toStation={1}; _jc_save_fromDate={2}; _jc_save_toDate={3}; _jc_save_wfdc_flag=dc; ",
                "%u5E7F%u5DDE%2CGZQ", "%u6210%u90FD%2CCDW", _entity["date"], _entity["date"]);
            if (!_entity["cookie"].Contains(cookie))
            {
                _entity["cookie"] = _entity["cookie"] + cookie;

            }

            try
            {
                if (QueryHandler.Request(_entity))
                {
                    OrderHandler.Init(_entity);
                    randCode.LoadAsync(OrderHandler.GetCode(_entity));
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void Init()
        {
            _entity.Clear();
            _entity["date"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            _entity["from"] = "GZQ";
            _entity["to"] = "CDW";
            _entity["fromName"] = "广州";
            _entity["toName"] = "成都";
            _entity["passenger"] = "3%2C0%2C1%2C%E5%91%A8%E8%A0%A1%2C1%2C431021198805280038%2C13686893341%2CN";
            _entity["oldPassenger"] = "%E5%91%A8%E8%A0%A1%2C1%2C431021198805280038%2C1_";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                _entity["code"] = code1.Text;
                if (!OrderHandler.Request(_entity))
                {
                    MessageBox.Show(@"buy fail.");
                    randCode.LoadAsync(OrderHandler.GetCode(_entity));
                }
                else
                {
                    MessageBox.Show(@"buy ok.");
                    //登出
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
