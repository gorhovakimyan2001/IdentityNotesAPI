namespace IdentityServiceProject.Dtos
{
    public class ToDoListShowDto
    {
        public string UserName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsDone { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime DeadlineDateTime { get; set; }
    }
}
