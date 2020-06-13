using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ParsingApiData
{
    public class ToDo
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }
    }
    class Program
    {
        static HttpClient client = new HttpClient();

        static void ShowToDo(ToDo toDo)
        {
            Console.WriteLine($"userId: {toDo.userId} \tid: " + $"{toDo.id}\ttitle: {toDo.title}\tcompleted: {toDo.completed}");
        }

        static async Task<ToDo[]> GetTodosAsync(String path)
        {
            ToDo[] data = null;
            HttpResponseMessage responseMessage = await client.GetAsync(path);
            if (responseMessage.IsSuccessStatusCode)
            {
                data = JsonConvert.DeserializeObject<ToDo[]>(await responseMessage.Content.ReadAsStringAsync());
            }
            return data;
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {

                ToDo[] toDos = await GetTodosAsync("todos");
                int completed = 0;
                int notCompleted = 0;
                foreach (ToDo element in toDos)
                {
                    ShowToDo(element);

                    if (element.completed)
                    {
                        completed++;
                    }
                    else
                    {
                        notCompleted++;
                    }
                }
                Console.WriteLine($"Completed #{completed}; Not completed #{notCompleted}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
            ///Console.WriteLine("Hello World!");
        }
    }
}
