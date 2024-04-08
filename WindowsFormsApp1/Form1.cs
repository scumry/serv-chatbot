using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        bool isConnected = false;
        Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Queue<string> historyQueue = new Queue<string>();

        public Form1()
        {
            tcpClient.ConnectAsync("192.168.220.220", 12345);
            InitializeComponent();
        }

        private async void SendTcpMessage(string message)
        {
            try
            {
                string command = "192.168.124.544:" + message;

                byte[] responseData = new byte[1024];
                byte[] requestData = Encoding.UTF8.GetBytes(command);
                tcpClient.Send(requestData, requestData.Length, new SocketFlags());

                historyQueue.Enqueue(message);
                UpdateChatListBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка отправки сообщения: " + ex.Message);
            }
        }

        private void UpdateChatListBox()
        {
            listBox1.Items.Clear();
            foreach (string historyMessage in historyQueue)
            {
                listBox1.Items.Add(historyMessage);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                richTextBox1.Clear();

                int selectedIndex = listBox1.SelectedIndex;
                richTextBox1.AppendText(historyQueue.ElementAt(selectedIndex) + Environment.NewLine);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ipAddress = textBox2.Text;

            try
            {
                //string message = "Подключено!";
                //SendTcpMessage(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string message = richTextBox1.Text;
                if (!string.IsNullOrEmpty(message))
                {
                    SendTcpMessage(message);
                    richTextBox1.Clear();
                }
                else
                {
                    MessageBox.Show("Введите текст сообщения.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка отправки сообщения: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                DisconnectFromChat();
                isConnected = false;
                MessageBox.Show("Отключен от IP-чата.");
            }
            else
            {
                MessageBox.Show("Окей, вы отключены от IP чата.");
            }
        }

        private void DisconnectFromChat()
        {
            // Дополнительные действия при отключении от чата
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}