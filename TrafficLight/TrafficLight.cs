using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

namespace TrafficLight
{
    public partial class TrafficLight : Form
    {

        private List<Panel> _panels;
        private List<Button> _controlButtons;
        private int _currentPanelIndex;
        private List<Color> _colors;

        public TrafficLight()
        {
            InitializeComponent();

            _panels = new List<Panel> { pnGreen, pnYellow, pnRed };
            _controlButtons = new List<Button> { btnGreen, btnYellow, btnBlue };
            _colors = new List<Color> { Color.Green, Color.Yellow, Color.Red, };
            _currentPanelIndex = 0;
        }

        private void LoadSerialPort()
        {
            string[] ports = SerialPort.GetPortNames();
            cbxPort.Items.AddRange(ports);
            if (ports.Length > 0)
                cbxPort.SelectedIndex = 0;

            for (int i = 0; i < 3; i++)
                _controlButtons[i].Enabled = false;

            for (int i = 0; i < 3; i++)
                _panels[i].BackColor = Color.Gray;
        }

        private void TrafficLight_Load(object sender, EventArgs e)
        {
            LoadSerialPort();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadSerialPort();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = cbxPort.Text;
                serialPort1.BaudRate = 9600;
                serialPort1.Parity = Parity.None;
                serialPort1.StopBits = StopBits.One;
                serialPort1.DataBits = 8;
                serialPort1.Open();

                for (int i = 0; i < 3; i++)
                    _controlButtons[i].Enabled = true;

                for (int i = 0; i < 3; i++)
                    _panels[i].BackColor = Color.Gray;
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnOpen.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                btnOpen.Enabled = true;

                for (int i = 0; i < 3; i++)
                    _controlButtons[i].Enabled = false;

                for (int i = 0; i < 3; i++)
                    _panels[i].BackColor = Color.Gray;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (serialPort1.IsOpen)
            {
                switch (button.Name)
                {
                    case "btnGreen":
                        serialPort1.Write("0");
                        break;

                    case "btnYellow":
                        serialPort1.Write("1");
                        break;

                    case "btnBlue":
                        serialPort1.Write("2");
                        break;
                }
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            int index = int.Parse(indata);
            for(int i = 0; i < 3; i++)
                _panels[i].BackColor = Color.Gray;
            _panels[index].BackColor = _colors[index];
        }
    }
}
