using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSNT_Lab_5
{
    public partial class Form1 : Form
    {

        string[] ConnectionType = { "Витая пара", "Оптоволоконный кабель", "Wi-Fi" };
        string[] SpeedType = { "Fast Ethernet (100 Мбит/с)", "Gigabit Ethernet (1 Гбит/с)",
        "10G Ethernet (10 Гбит/с)", "40-100G Ethernet (40-100 Гбит/с)" };

        string[] TypesTW = Twisted_pair.GetStringTypes();
        string[] TypesFC = FiberOptic_cable.GetStringTypes();
        string[] TypesWF = Wi_Fi.GetStringTypes();



        int connectionType, speed, type, length;

        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < ConnectionType.Length; i++)
            {
                _connectionType.Items.Add(ConnectionType[i]);
            }

            for (int i = 0; i < SpeedType.Length; i++)
            {
                _speed.Items.Add(SpeedType[i]);
            }

        }

        private void _length_TextChanged(object sender, EventArgs e)
        {
            GetResult();
        }

        private void _type_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetResult();
        }

        private void _speed_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetResult();
        }

        private void _connectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _type.Items.Clear();

            _type.SelectedIndex = -1;
            _type.Text = string.Empty;

            if (_connectionType.SelectedIndex < 2)
            {
                _L_Type.Text = "Вид кабеля:";
                _L_Length.Text = "Длина кабеля:";
            }
            else
            {
                _L_Type.Text = "Диапазон частот:";
                _L_Length.Text = "Расстояние\nдо роутера:";
            }

            if (_connectionType.SelectedIndex == 0)
            {
                for (int i = 0; i < TypesTW.Length; i++)
                {
                    _type.Items.Add(TypesTW[i]);
                }
            }
            else if (_connectionType.SelectedIndex == 1)
            {
                for (int i = 0; i < TypesFC.Length; i++)
                {
                    _type.Items.Add(TypesFC[i]);
                }
            }
            else
            {
                for (int i = 0; i < TypesWF.Length; i++)
                {
                    _type.Items.Add(TypesWF[i]);
                }
            }
        }

        public void GetResult()
        {
            type = _type.SelectedIndex;
            speed = _speed.SelectedIndex;
            
            try
            {
                length = Convert.ToInt32(_length.Text);
            }
            catch
            {
                Result.Text = "Неверные данные";
                return;
            }

            if (speed < 0 || type < 0)
            {
                Result.Text = "Недостаточно данных";
                return;
            }

            Result.Text = "Соединение невозможно";

            switch (_connectionType.SelectedIndex)
            {
                case 0:

                    if (Twisted_pair.IsThereConnection(type, speed, length))
                    {
                        Result.Text = "Соединение возможно";
                    }
                    return;

                case 1: 
                    
                    if (FiberOptic_cable.IsThereConnection(type, speed, length))
                    {
                        Result.Text = "Соединение возможно";
                    }
                    return;

                case 2:

                    if (Wi_Fi.IsThereConnection(type, speed, length))
                    {
                        Result.Text = "Соединение возможно";
                    }
                    return;
            }
        }


    }
}
