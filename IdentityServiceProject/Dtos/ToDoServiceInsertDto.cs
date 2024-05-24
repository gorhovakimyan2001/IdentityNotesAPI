namespace IdentityServiceProject.Dtos
{
    public class ToDoServiceInsertDto
    {
        public string UserName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DeadlineDateTime { get; set; }
    }
}
