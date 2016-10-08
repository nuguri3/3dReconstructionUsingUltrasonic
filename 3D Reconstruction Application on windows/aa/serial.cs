using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using Tao.Platform.Windows;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Sdl;
using Tao.DevIl;

/*
namespace aa
{
    class Serial
    {
        private void Initialize_coordinates()
        {
            x = new int[20];
            y = new int[20];
            z = new int[20];
        }

        //시리얼 포트 통신 준비
        private void SerialPortInitialize()
        {
            //사용 가능한 포트 콤보박스에 추가
            cmbPort.BeginUpdate();
            foreach (string comport in SerialPort.GetPortNames())
            {
                cmbPort.Items.Add(comport);
            }
            cmbPort.EndUpdate();

            serialPort1.Open(); // 포트 open
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
            //이벤트 헨들러 추가    

        }

        //시리얼 포트로 읽은 내용을 출력
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                string str = serialPort1.ReadLine();
                x[cnt] = Convert.ToInt32(str[1]) - 48;  //문자를 숫자로 변환
                y[cnt] = Convert.ToInt32(str[4]) - 48;
                z[cnt] = Convert.ToInt32(str[7]) - 48;

                richTextBox1.Text += str;
                listBox1.Items.Add(str);
            }

            cnt++;

            //좌표가 20개 모이면 한꺼번에 출력
            if (cnt == 20)
            {
                cnt = 0;
                string str;
                listBox2.Items.Clear();
                for (int i = 0; i < 20; i++)
                {
                    str = "(" + x[i] + ", " + y[i] + ", " + z[i] + ")";
                    listBox2.Items.Add(str);
                }
            }
        }

        private float a, b, c;
        private int[] x, y, z;
        private int cnt;
    }
}

*/