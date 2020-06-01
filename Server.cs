using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace tcpServer
{
 

    public partial class Server : Form
    {
        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> clientSockets = new List<Socket>();
        private static readonly List<User> users = new List<User>();
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 100;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];


        private static void SetupServer()
        {
            updateUI("Setting up server...");
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
            updateUI("Server setup complete");
        }

        /// <summary>
        /// Close all connected client (we do not need to shutdown the server socket as its connections
        /// are already closed with the clients).
        /// </summary>
        private static void CloseAllSockets()
        {
            foreach (Socket socket in clientSockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

            serverSocket.Close();
        }

        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }

            clientSockets.Add(socket);
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            updateUI("Client connected, waiting for request...");
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;

            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException)
            {
                updateUI("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                current.Close();
                clientSockets.Remove(current);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);
            updateUI("Received Text: " + text);

            if (text.StartsWith("REG"))
            {
                updateUI("A user wants to register");
                User user = new User();
                user.username = text.Split(' ')[1];
                user.password = text.Split(' ')[2];
                users.Add(user);
                byte[] data = Encoding.ASCII.GetBytes("successful registered!");
                current.Send(data);
            }
            else if (text.StartsWith("LOG"))
            {
                

                
                updateUI("A user wants to login");
                updateUI("The username tried " + text.Split(' ')[1]);
                updateUI("The password tried " + text.Split(' ')[2]);
                foreach (User user in users)
                {
                    if(user.username== text.Split(' ')[1])
                    {
                        updateUI("user found!");
                        byte[] data1 = Encoding.ASCII.GetBytes("user found!");
                        current.Send(data1);
                        if(user.password == text.Split(' ')[2])
                        {
                            updateUI("Correct password! Successful login!");
                            byte[] data2 = Encoding.ASCII.GetBytes("Correct password! Successful login!");
                            current.Send(data2);
                            user.loggedIn = true;


                        }
                        else
                        {
                            updateUI("Wrong password! Try again!");
                            byte[] data3 = Encoding.ASCII.GetBytes("Wrong password! Try again!");
                            current.Send(data3);
                        }
                    }
                }
                //user.username = text.Split(' ')[1];
               // user.password = text.Split(' ')[2];
               // users.Add(user);
                //byte[] data = Encoding.ASCII.GetBytes("successful registered!");
               // current.Send(data);
            }
            else if (text.ToLower() == "get time") // Client requested time
            {
                updateUI("Text is a get time request");
                byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
                current.Send(data);
                updateUI("Time sent to client");
            }
            else if (text.ToLower() == "exit") // Client wants to exit gracefully
            {
                // Always Shutdown before closing
                current.Shutdown(SocketShutdown.Both);
                current.Close();
                clientSockets.Remove(current);
                updateUI("Client disconnected");
                return;
            }
            else
            {
                updateUI("Text is an invalid request");
                byte[] data = Encoding.ASCII.GetBytes("Invalid request");
                current.Send(data);
                
                updateUI("Warning Sent");
            }

            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
        }


        public static  bool ControlInvokeRequired(Control c, Action a)
        {
            if (c.InvokeRequired) c.Invoke(new MethodInvoker(delegate { a(); }));
            else return false;

            return true;
        }
        private static void updateUI(string text)
        {
            //Check if invoke requied if so return - as i will be recalled in correct thread
            if (ControlInvokeRequired(Program.form1.tchat, () => updateUI(text))) return;
            
            Program.form1.tchat.AppendText(text);

            Program.form1.tchat.AppendText(Environment.NewLine);
         

        }
        public Server()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetupServer();
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void bStopServer_Click(object sender, EventArgs e)
        {
            CloseAllSockets();
            this.Hide();
            this.Close();
        }

        private void tchat_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void bSend_Click(object sender, EventArgs e)
        {
            string input = mConsole.Text;
            if(input=="start server")
            {
                updateUI("Setting up server...");
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
                serverSocket.Listen(0);
                serverSocket.BeginAccept(AcceptCallback, null);
                updateUI("Server setup complete");
            }
            else if(input == "stop server")
            {
                CloseAllSockets();
                this.Hide();
                this.Close();
            }
            else if(input=="list users")
            {
                if(users.Count==0)
                {
                    updateUI("There are no users yet!");
                }
                else
                {
                    foreach(User user in users)
                    {
                        
                        updateUI(user.username);
                        updateUI(user.password);
                        updateUI(user.loggedIn.ToString());
                        updateUI("");
                    }
                }
            }
        }

        private void bStartServer_Click(object sender, EventArgs e)
        {
            updateUI("Setting up server...");
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
            updateUI("Server setup complete");
        }
    }
    public class User
    {

        public Socket socket;
        public string username;
        public string password;
        public bool loggedIn;
    }
}
