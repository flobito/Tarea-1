using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Net2
{
    public partial class Form1 : Form
    {
        static private NetworkStream stream;
        static private StreamWriter streamw;
        static private StreamReader streamr;
        static private TcpClient client = new TcpClient();
        static private string username = "unknown";

        private delegate void DaddItem(String s);

        private void AddItem(String s)
        {
            listBox1.Items.Add(s);
        }

        public Form1()
        {
            InitializeComponent();
        }

        void Listen()
        {
            while (client.Connected)
            {
                try
                {
                    this.Invoke(new DaddItem(AddItem), streamr.ReadLine());
                }
                catch
                {
                    MessageBox.Show("No se ha podido conectar al servidor");
                    Application.Exit();
                }
            }
        }

        void Conectar()
        {
            try
            {
                client.Connect("127.0.0.1", 8000);
                if (client.Connected)
                {
                    Thread t = new Thread(Listen);

                    stream = client.GetStream();
                    streamw = new StreamWriter(stream);
                    streamr = new StreamReader(stream);

                    streamw.WriteLine(username);
                    streamw.Flush();

                    t.Start();
                }
                else
                {
                    MessageBox.Show("Servidor no disponible");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Servidor no disponible: " + ex.Message);
                Application.Exit();
            }
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            nick = txtUsuario.Text;
            Conectar();

            lbTitulo1.Visible = false;
            txtUsuario.Visible = false;
            btnConectar.Visible = false;

            listBox1.Visible = true;
            txtMensaje.Visible = true;
            btnEnviar.Visible = true;
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                streamw.WriteLine(txtMensaje.Text);
                streamw.Flush();
                txtMensaje.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar mensaje: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnEnviar.Visible = false;
            txtMensaje.Visible = false;
            listBox1.Visible = false;
        }
    }
}
