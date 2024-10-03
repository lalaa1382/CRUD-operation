namespace MyLWork7.Models.Domain
{

        public class Tag
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }
        public List<BlogPost>? BlogPosts { get; set; }
    }
 

}
