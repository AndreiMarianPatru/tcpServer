using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace tcpServer
{


    public partial class Server : Form
    {
        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> clientSockets = new List<Socket>();
        private static List<User> users = new List<User>();
        private static List<Room> rooms = new List<Room>();
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

            if (text.ToLower().StartsWith("/reg"))
            {             
                Register_C(text,current);
            }
            else if (text.ToLower().StartsWith("/log"))
            {
                LogIn_C(text,current);
            }
            else if (text.ToLower().StartsWith("/join_room"))
            {
                JoinRoom_C(text,current);
            }
            else if (text.ToLower() == "/list_rooms")
            {
                ListRooms_C(text,current);
            }
            else if (text.ToLower() == "/get time") // Client requested time
            {
               GetTime_C(text,current);
            }
            else if (text.ToLower() == "/exit") // Client wants to exit gracefully
            {
                Exit_C(text,current);
                return;
            }
            else
            {
                updateUI("Invalid request-> "+text+Environment.NewLine);
                byte[] data = Encoding.ASCII.GetBytes("Invalid request");
                current.Send(data);
                updateUI("Warning Sent");
            }

            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
        }


        public static bool ControlInvokeRequired(System.Windows.Forms.Control c, Action a)
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
            if (input == "/start server")
            {
                StartServer_S();
            }
            else if (input == "/stop server")
            {
                CloseAllSockets();
                this.Hide();
                this.Close();
            }
            else if (input == "/list users")
            {
               ListUsers_S();
            }
            else if (input == "/list rooms")
            {
                ListRooms_S();
            }
            else if (input.ToLower().Split(' ')[0] == "/createroom")
            {
               CreateRoom_S(input);
            }
            else
            {
                updateUI("Wong input! Try again");
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
        //_C stands for client _S stands for server
        private static void LogIn_C(string text, Socket current)
        {
            bool logflag = false;
            User loguser = new User();
            updateUI("A user wants to login");
            updateUI("The username tried " + text.Split(' ')[1]);
            updateUI("The password tried " + text.Split(' ')[2]);
            if (users.Count == 0)
            {
                byte[] data0 = Encoding.ASCII.GetBytes("No users registered yet! Type HELP for available commands!");
                current.Send(data0);
            }
            else
            {
                foreach (User user in users)
                {
                    if (user.username == text.Split(' ')[1])
                    {
                        logflag = true;
                        loguser = user;
                    }
                }
                if (logflag)
                {
                    updateUI("user found!");
                    if (loguser.password == text.Split(' ')[2])
                    {
                        updateUI("Correct password! Successful login!");
                        byte[] data2 = Encoding.ASCII.GetBytes("Log in successful! Welcome " + loguser.username + "!");
                        current.Send(data2);
                        loguser.loggedIn = true;


                    }
                    else
                    {
                        updateUI("Wrong password!");
                        byte[] data3 = Encoding.ASCII.GetBytes("Wrong password! Try again!");
                        current.Send(data3);
                    }
                }
                else
                {
                    updateUI("User not found");
                    byte[] data4 = Encoding.ASCII.GetBytes("There is no user registered with this username. Please try again or register new user");
                    current.Send(data4);
                }
            }
        }
        private static void JoinRoom_C(string text,Socket current)
        {
            bool flag = false;
                byte[] finaldata;

                try
                {
                    Room temproom;
                    int id = Int32.Parse(text.Split(' ')[1]);
                    temproom = rooms.Find(x => x.id == id);
                    byte[] data0 = Encoding.ASCII.GetBytes("Valid input ID, searching for a room with this ID " + Environment.NewLine);
                    finaldata = data0;


                    if (temproom == null)
                    {
                        updateUI("1");
                        byte[] data1 = Encoding.ASCII.GetBytes("There is no room with this id!");
                        finaldata = data0.Concat(data1).ToArray();

                    }
                    else
                    {
                        updateUI("2");

                        foreach (User user in users)
                        {
                            updateUI("3");

                            if (user.socket == current)
                            {
                                updateUI("4");

                                temproom.Users.Add(user);
                                byte[] data2 = Encoding.ASCII.GetBytes("Room joined successful!");
                                finaldata = data0.Concat(data2).ToArray();



                            }
                        }

                    }
                }
                catch (FormatException)
                {
                    updateUI("Try another ID");
                    byte[] data3 = Encoding.ASCII.GetBytes("Wrong input ID!");
                    finaldata = data3;

                }
                current.Send(finaldata);
        }
        private static void ListRooms_C(string text,Socket current)
        {
            if (rooms.Count == 0)
                {
                    byte[] data = Encoding.ASCII.GetBytes("There are no avaiable rooms yet!");
                    current.Send(data);
                }
                else
                {
                    foreach (Room room in rooms)
                    {
                        byte[] data0 = Encoding.ASCII.GetBytes("Id: " + room.id + Environment.NewLine + "Name: " + room.name + Environment.NewLine + "Number of users: " + room.Users.Count() + Environment.NewLine);
                        current.Send(data0);


                    }
                }
        }
        private static void Register_C(string text,Socket current)
        {
             bool flag = false;
                string[] input = text.Split(' ');
                if (input.Count() == 3 && text.Split(' ')[1] != "" && text.Split(' ')[2] != "")
                {
                    if (users.Count != 0)
                    {
                        foreach (User user1 in users)
                        {
                            if (text.Split(' ')[1] == user1.username)
                            {
                                flag = true;
                            }
                        }
                    }
                    if (flag == true)
                    {
                        byte[] data1 = Encoding.ASCII.GetBytes("This username is already taken, please change it");
                        current.Send(data1);
                    }
                    else
                    {
                        updateUI("A user wants to register");
                        User user = new User();
                        user.username = text.Split(' ')[1];
                        user.password = text.Split(' ')[2];
                        user.socket = current;
                        users.Add(user);
                        byte[] data = Encoding.ASCII.GetBytes("Account created successfully! Welcome " + user.username + "! Please Log In now!");
                        current.Send(data);
                    }

                }
                else
                {
                    byte[] data = Encoding.ASCII.GetBytes("There is a problem with the input provided. Please try again!");
                    current.Send(data);
                }
        }
        private static void GetTime_C(string text,Socket current)
        {
                updateUI("Text is a get time request");
                byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
                current.Send(data);
                updateUI("Time sent to client");
        }
        private static void Exit_C(string text, Socket current)
        {
            // Always Shutdown before closing
                current.Shutdown(SocketShutdown.Both);
                current.Close();
                clientSockets.Remove(current);
                updateUI("Client disconnected");
               
        }
        private static void StartServer_S()
        {
            updateUI("Setting up server...");
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
                serverSocket.Listen(0);
                serverSocket.BeginAccept(AcceptCallback, null);
                updateUI("Server setup complete");
        }
        private static void ListUsers_S()
        {
             if (users.Count == 0)
                {
                    updateUI("There are no users yet!");
                }
                else
                {
                    foreach (User user in users)
                    {

                        updateUI(user.username);
                        updateUI(user.password);
                        updateUI(user.loggedIn.ToString());
                        updateUI("");
                    }
                }
        }
        private static void ListRooms_S()
        {
            if (rooms.Count == 0)
                {
                    updateUI("There are no rooms yet!");
                }
                else
                {
                    foreach (Room room in rooms)
                    {

                        updateUI("id " + room.id);
                        updateUI("name " + room.name);
                        updateUI("number of users " + room.Users.Count());
                        updateUI("");
                    }
                }
        }
        private static void CreateRoom_S(string input)
        {
             Room room = new Room();
                try
                {
                    int id = Int32.Parse(input.ToLower().Split(' ')[1]);
                    if (rooms.Count > 0)
                    {
                        foreach (Room tmproom in rooms)
                        {
                            if (tmproom.id == id)
                            {
                                updateUI("A room with this id already exists! Try a different id!");
                                return;
                            }
                        }
                    }
                    room.id = id;
                }
                catch (FormatException)
                {
                    updateUI("Try another ID");
                }
                room.name = input.ToLower().Split(' ')[2];
                rooms.Add(room);
                updateUI("room " + room.name + " with id " + room.id + " was created!");
        }
       
    }
    public class User
    {

        public Socket socket;
        public string username;
        public string password;
        public bool loggedIn;
    }
    public class Room
    {
        public int id;
        public string name;
        public List<User> Users = new List<User>();
    }
}
