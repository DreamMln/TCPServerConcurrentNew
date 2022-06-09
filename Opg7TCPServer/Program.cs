using MovieLib;
using Newtonsoft.Json;
using RESTMovie.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Opg7TCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Opgave 7 TCP-server!");
            //concurrent server modtager flere clienter
            //initialisere et objekt af tcplistner class
            TcpListener listener = new TcpListener(IPAddress.Any, 43214);
            //begynd at "lytte" for en connection
            listener.Start();

            while (true)
            {
                //acceptere tcp client objekt
                //AcceptTcpClient - dette returnere et TcpClient object
                //krav
                TcpClient socket = listener.AcceptTcpClient();

                //CONCURRENT server - Task.Run(() håndtere/starter
                //flere tråde

                //så vi kan have flere clients samtidig
                //kadler på metoden HandleAClient
                //Task: som kører asynkron  
                Task.Run(()=> HandleAClient(socket));
            }
        }
        public static void HandleAClient(TcpClient socket)
        {
            //streams - read and write i connectionen
            //data strøm frem og tilbage, det er to-vejs kommunikation
            NetworkStream networkStream = socket.GetStream();
            //derefter splittes de op i to streams
            StreamReader reader = new StreamReader(networkStream);
            StreamWriter writer = new StreamWriter(networkStream);

            //for at andvende metoderne i manager 
            MoviesManager moviesManager = new MoviesManager();

            //der skal læses det clienten anmoder om 
            //som er listen med movies eller listen med country som filter

            string message = reader.ReadLine();

            //andmoder kan vælge mellem getAllmovies eller getAllmovies countryFilter
            //GetAll Movies
            if (message == "GetList")//true
            {
                //json er letvægt format
                // jsons navn og 
                //lave json her så man kan bruge manager til andre ting -på en måde bliver"generic"
                List<Movie> getAll = moviesManager.GetAll();

                //Lavet om til JSON-format, udveksle data
                //data udveksle en liste af Movie objekter
                string serilaizerJSON = JsonConvert.SerializeObject(getAll);
                
                Console.WriteLine("the List will be returned");
                //resulatet af serlization 
                // skriver/sender til client 
                writer.WriteLine(serilaizerJSON);
            }
             if (message.StartsWith("GetByCountry"))
            {
                //13 GetByCountry
                //jeg vil kun have en del af stringen, derfor bruger jeg Substring
                //tæller antal karaktere hvor den skal starte med at returnere stringen fra
                //jeg ignorere derfor de første 13 karaktere | alt før 13
                string filterCountry = message.Substring(13);

                //der skal sendes et country fra clienten 
                List<Movie> getByCountry = moviesManager.GetFilter(filterCountry);

                string serlizeJSON = JsonConvert.SerializeObject(getByCountry);

                Console.WriteLine("List with filter will be returned ");
                //resulatet af serlization 
                writer.WriteLine(serlizeJSON);

            }

            //besked i konsollen
            //Console.WriteLine("Client send this: " + message);

            //skyl ud, rydde op, TCP streams har buffer og cache
            //- tvinge den til at sende pakken her og nu
            writer.Flush();
            //stopper med at ''lytte'' - stopper serveren
            //listener.Stop();
            //luk socket - fordi det er TCP, for at spare på com ressourcer, så lad os afslutte den med det samme
            //spar på ressourcer, ryd ordenligt op
            socket.Close();
        }
    }
}
